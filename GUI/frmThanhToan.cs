using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using BLL;
using DTO;
using System.Net;
using System.Linq;

namespace GUI
{
    public partial class frmThanhToan : Form
    {
        public string SelectedMaKM { get; private set; }
        public string HinhThucThanhToan { get; private set; } = "Tiền mặt";
        public string CustomerSDT { get; private set; }

        ServiceBLL bll = new ServiceBLL();
        private string maBan;
        private decimal currentTotal;
        private CultureInfo viVN = new CultureInfo("vi-VN");
        private string currentMaHD;
        private string currentMaKH;
        private const string BANK_ID = "970403";
        private const string ACCOUNT_NO = "4221510017460372";
        private const string ACCOUNT_NAME = "HOANG TRAN THIEN LOC";

        public frmThanhToan(string maBan, string maHD, decimal total)
        {
            InitializeComponent();
            
            lblLabelKM.Text = "Khách thành viên?";

            this.maBan = maBan;
            this.currentTotal = total;
            this.currentMaHD = maHD;

            ApplyUIStyles();
            SetupData();
        }

        private void ApplyUIStyles()
        {
            btnThanhToan.BackColor = UIHelper.DangerColor;
        }

        private void SetupData()
        {
            viVN.NumberFormat.CurrencyDecimalDigits = 0;
            viVN.NumberFormat.CurrencyGroupSeparator = ".";
            viVN.NumberFormat.NumberGroupSeparator = ".";

            lblTotal.Text = $"Tổng tiền: {currentTotal.ToString("#,##0", viVN)} VNĐ";
            cboApplyKM.SelectedIndex = 0;
            cboKhuyenMai.DisplayMember = "Ten";
            cboKhuyenMai.ValueMember = "MaKM";
        }

        private void CboApplyKM_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enable = cboApplyKM.Text == "Có";
            txtSDT.Enabled = enable;
            cboKhuyenMai.Enabled = enable;
            lblDiemTichLuy.Text = "Điểm: 0";
            cboKhuyenMai.DataSource = null;
            currentMaKH = null;

            if (!enable)
            {
                txtSDT.Text = "";
                lblTotal.Text = $"Tổng tiền: {currentTotal.ToString("#,##0", viVN)} VNĐ";
                UpdateQRCode(currentTotal);
            }
        }

        private void TxtSDT_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TxtSDT_Leave(null, null);
                e.SuppressKeyPress = true;
            }
        }

        private void TxtSDT_Leave(object sender, EventArgs e)
        {
            if (!txtSDT.Enabled || string.IsNullOrWhiteSpace(txtSDT.Text)) return;

            string sdt = txtSDT.Text.Trim();
            CustomerSDT = sdt;

            currentMaKH = bll.dal.GetKhachHangBySDT(sdt);

            if (currentMaKH == null)
            {
                MessageBox.Show("Khách hàng mới. Sẽ tạo tài khoản khách hàng mới khi thanh toán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblDiemTichLuy.Text = "Điểm: 0";
                cboKhuyenMai.DataSource = null;
                return;
            }

            int diem = bll.GetDiemTichLuy(currentMaKH);
            lblDiemTichLuy.Text = $"Điểm: {diem.ToString("N0", viVN)}";

            var listKM = bll.GetListKhuyenMai()
                            .FindAll(km => km.DiemCan <= diem && (km.NgayKT == null || km.NgayKT.Value >= DateTime.Now))
                            .ToList();

            listKM.Insert(0, new KhuyenMaiDTO { MaKM = "", Ten = "--- Không áp dụng KM ---", GiaTriGiam = 0 });

            cboKhuyenMai.DataSource = listKM;
            cboKhuyenMai.SelectedIndex = 0;

            lblTotal.Text = $"Tổng tiền: {currentTotal.ToString("#,##0", viVN)} VNĐ";
        }

        private void CboKhuyenMai_SelectedIndexChanged(object sender, EventArgs e)
        {
            decimal totalAfterDiscount = currentTotal;
            if (cboKhuyenMai.SelectedItem is KhuyenMaiDTO km)
            {
                SelectedMaKM = km.MaKM;

                if (km.LoaiGiam == "Phần trăm")
                {
                    decimal discountAmount = currentTotal * (km.GiaTriGiam / 100);
                    totalAfterDiscount = currentTotal - discountAmount;
                }
                else if (km.LoaiGiam == "Tiền mặt")
                {
                    totalAfterDiscount = currentTotal - km.GiaTriGiam;
                    if (totalAfterDiscount < 0) totalAfterDiscount = 0;
                }

                lblTotal.Text = $"Tổng tiền: {totalAfterDiscount.ToString("#,##0", viVN)} VNĐ";
            }
            else
            {
                SelectedMaKM = null;
                lblTotal.Text = $"Tổng tiền: {currentTotal.ToString("#,##0", viVN)} VNĐ";
                totalAfterDiscount = currentTotal;
            }
            UpdateQRCode(totalAfterDiscount);
        }

        private void BtnThanhToan_Click(object sender, EventArgs e)
        {
            if (cboApplyKM.Text == "Có" && string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập SĐT khách hàng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (currentMaKH != null)
                {
                    bll.UpdateMaKMToBill(currentMaHD, currentMaKH, SelectedMaKM);
                }

                string resultMessage = "";
                int pointsAdded = 0;

                if (bll.Checkout(maBan, HinhThucThanhToan, out resultMessage, out pointsAdded))
                {
                    if (!string.IsNullOrEmpty(SelectedMaKM) && currentMaKH != null)
                    {
                        var km = bll.GetListKhuyenMai().Find(k => k.MaKM == SelectedMaKM);
                        if (km != null)
                        {
                            bll.DeductPointOnUse(currentMaHD, currentMaKH, SelectedMaKM, km.DiemCan);
                        }
                    }

                    string finalMsg = resultMessage;
                    if (pointsAdded > 0)
                    {
                        finalMsg += $"\n\nKhách hàng được cộng thêm: {pointsAdded} điểm tích lũy!";
                    }

                    MessageBox.Show(finalMsg, "Thanh toán thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(resultMessage, "Lỗi Thanh Toán", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateQRCode(decimal amount)
        {
            pbQRCode.Image = null;
            qrTimer.Stop();
            qrTimer.Start();
        }

        private void LoadQROnline(decimal amount)
        {
            try
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

                string amountStr = ((int)amount).ToString();
                string content = $"TT {currentMaHD}";

                string apiUrl = $"https://img.vietqr.io/image/{BANK_ID}-{ACCOUNT_NO}-compact2.jpg?amount={amountStr}&addInfo={Uri.EscapeDataString(content)}&accountName={Uri.EscapeDataString(ACCOUNT_NAME)}";

                pbQRCode.LoadAsync(apiUrl);
            }
            catch
            {
            }
        }

        private void QrTimer_Tick(object sender, EventArgs e)
        {
            qrTimer.Stop();
            decimal finalAmount = currentTotal;

            if (!string.IsNullOrEmpty(SelectedMaKM))
            {
                var km = bll.GetListKhuyenMai().Find(k => k.MaKM == SelectedMaKM);
                if (km != null)
                {
                    if (km.LoaiGiam == "Phần trăm") finalAmount -= currentTotal * (km.GiaTriGiam / 100);
                    else finalAmount -= km.GiaTriGiam;
                    if (finalAmount < 0) finalAmount = 0;
                }
            }

            LoadQROnline(finalAmount);
        }
    }
}