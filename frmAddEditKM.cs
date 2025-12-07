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
        private KhuyenMaiDTO _khuyenMai; // Null nếu là Thêm mới
        private bool isEdit = false;

        // Constructor cho Thêm/Sửa
        public frmAddEditKM(KhuyenMaiDTO km)
        {
            InitializeComponent();
            _khuyenMai = km;
            isEdit = (km != null);

            // Setup giao diện chung
            UIHelper.SetupDialog(this, isEdit ? "SỬA KHUYẾN MÃI" : "THÊM KHUYẾN MÃI MỚI");

            // Khởi tạo các sự kiện và dữ liệu
            InitData();
            InitEvents();
        }

        private void InitData()
        {
            // Load ComboBox Loại Giảm
            cboLoaiGiam.Items.AddRange(new object[] { "Tiền mặt", "Phần trăm" });

            if (isEdit)
            {
                txtMaKM.Text = _khuyenMai.MaKM;
                txtTenKM.Text = _khuyenMai.TenKM;
                txtMoTa.Text = _khuyenMai.MoTa;
                txtDiemCanThiet.Value = _khuyenMai.DiemCanThiet;
                txtGiaTriGiam.Value = _khuyenMai.GiaTriGiam;
                cboLoaiGiam.SelectedItem = _khuyenMai.LoaiGiam;
                cboTrangThai.SelectedItem = _khuyenMai.TrangThai;

                // Xử lý Ngày Kết Thúc (Nullable DateTime)
                if (_khuyenMai.NgayKetThuc.HasValue)
                {
                    chkKhongHetHan.Checked = false;
                    dtpNgayKetThuc.Value = _khuyenMai.NgayKetThuc.Value;
                }
                else
                {
                    chkKhongHetHan.Checked = true;
                    dtpNgayKetThuc.Enabled = false;
                }

                this.Text = "SỬA KHUYẾN MÃI: " + _khuyenMai.TenKM;
            }
            else
            {
                txtMaKM.Text = "Mã sẽ được tự động sinh";
                cboLoaiGiam.SelectedIndex = 0;
                cboTrangThai.SelectedIndex = 0; // Mặc định Hoạt động
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
            // Bật/tắt DateTimePicker tùy thuộc vào CheckBox
            dtpNgayKetThuc.Enabled = !chkKhongHetHan.Checked;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra dữ liệu chung
            if (string.IsNullOrWhiteSpace(txtTenKM.Text) || cboLoaiGiam.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Tên KM và Loại Giảm.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Thu thập dữ liệu
            KhuyenMaiDTO kmToSave = new KhuyenMaiDTO
            {
                MaKM = isEdit ? _khuyenMai.MaKM : "",
                TenKM = txtTenKM.Text.Trim(),
                MoTa = txtMoTa.Text.Trim(),
                DiemCanThiet = (int)txtDiemCanThiet.Value,
                GiaTriGiam = txtGiaTriGiam.Value,
                LoaiGiam = cboLoaiGiam.SelectedItem.ToString(),
                NgayBatDau = DateTime.Now, // Giả định ngày bắt đầu là hiện tại
                TrangThai = cboTrangThai.SelectedItem.ToString()
            };

            // 3. Xử lý Ngày Kết Thúc
            if (!chkKhongHetHan.Checked)
            {
                kmToSave.NgayKetThuc = dtpNgayKetThuc.Value.Date.AddHours(23).AddMinutes(59).AddSeconds(59); // Cuối ngày
            }
            else
            {
                kmToSave.NgayKetThuc = null; // NULL trong DB
            }

            // 4. Gọi BLL
            string result = "";
            if (isEdit)
            {
                result = bll.UpdateKhuyenMai(kmToSave);
            }
            else
            {
                result = bll.AddKhuyenMai(kmToSave);
            }

            // 5. Hiển thị kết quả
            MessageBox.Show(result);
            if (result.Contains("thành công"))
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}