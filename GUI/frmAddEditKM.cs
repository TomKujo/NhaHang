using BLL;
using DTO;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmAddEditKM : Form
    {
        private ServiceBLL bll = new ServiceBLL();
        private KhuyenMaiDTO _khuyenMai;
        private bool isEdit = false;
        public frmAddEditKM(KhuyenMaiDTO km)
        {
            InitializeComponent();
            _khuyenMai = km;
            isEdit = (km != null);
            UIHelper.SetupDialog(this, isEdit ? "Sửa khuyến mãi" : "Thêm khuyến mãi");
            InitData();
            InitEvents();
        }

        private void InitData()
        {
            cboLoaiGiam.Items.AddRange(new object[] { "Tiền mặt", "Phần trăm" });
            if (isEdit)
            {
                txtTenKM.Text = _khuyenMai.Ten;
                txtMoTa.Text = _khuyenMai.MoTa;
                txtDiemCanThiet.Text = _khuyenMai.DiemCan.ToString();
                txtGiaTriGiam.Text = _khuyenMai.GiaTriGiam.ToString();
                cboLoaiGiam.SelectedItem = _khuyenMai.LoaiGiam;
                if (_khuyenMai.NgayKT.HasValue)
                {
                    chkKhongHetHan.Checked = false;
                    dtpNgayKetThuc.Value = _khuyenMai.NgayKT.Value;
                }
                else
                {
                    chkKhongHetHan.Checked = true;
                    dtpNgayKetThuc.Enabled = false;
                }

                this.Text = "Sửa khuyến mãi";
            }
            else
            {
                cboLoaiGiam.SelectedIndex = 0;
                chkKhongHetHan.Checked = true;
                dtpNgayKetThuc.Enabled = false;
            }
        }

        private void InitEvents()
        {
            btnSave.Click += BtnSave_Click;
            chkKhongHetHan.CheckedChanged += ChkKhongHetHan_CheckedChanged;
        }

        private void ChkKhongHetHan_CheckedChanged(object sender, EventArgs e)
        {
            dtpNgayKetThuc.Enabled = !chkKhongHetHan.Checked;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenKM.Text) || cboLoaiGiam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên KM và Loại Giảm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(txtDiemCanThiet.Text, out int diem) || !decimal.TryParse(txtGiaTriGiam.Text, out decimal giam))
            {
                MessageBox.Show("Điểm và Giá trị giảm phải là số nguyên lớn hơn 0!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime? ngayKetThuc = chkKhongHetHan.Checked ? (DateTime?)null : dtpNgayKetThuc.Value;

            KhuyenMaiDTO kmToSave = new KhuyenMaiDTO
            {
                MaKM = isEdit ? _khuyenMai.MaKM : "",
                Ten = txtTenKM.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                DiemCan = diem,
                GiaTriGiam = giam,
                LoaiGiam = cboLoaiGiam.SelectedItem.ToString(),
                NgayBD = DateTime.Now,
                TrangThai = (ngayKetThuc.HasValue && ngayKetThuc.Value < DateTime.Now)
                    ? "Ngừng áp dụng"
                    : "Hoạt động"
            };

            kmToSave.NgayKT = chkKhongHetHan.Checked ? (DateTime?)null : dtpNgayKetThuc.Value;

            string result = isEdit ? bll.UpdateKhuyenMai(kmToSave) : bll.AddKhuyenMai(kmToSave);

            MessageBox.Show(result);
            if (result.Contains("thành công"))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}