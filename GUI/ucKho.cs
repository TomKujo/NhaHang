using BLL;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GUI
{
    public class CartItemViewModel
    {
        public string MaNguyenLieu { get; set; }
        public string Ten { get; set; }
        public decimal LuongYeuCau { get; set; }
        public decimal LuongThucTe { get; set; }
        public decimal DonGia { get; set; }
        public string TinhTrang { get; set; }
        public decimal ThanhTien => LuongThucTe * DonGia;
    }

    public partial class ucKho : UserControl
    {
        ServiceBLL bll = new ServiceBLL();
        NguoiDungDTO currentUser;
        List<CartItemViewModel> gioHangNhap = new List<CartItemViewModel>();

        public ucKho(NguoiDungDTO user)
        {
            InitializeComponent();
            this.currentUser = user;

            StyleControls();
            BindEvents();

            LoadDataKho();
            LoadDataNCC();
        }

        private Button btnXoaDongGH;

        private void StyleControls()
        {
            UIHelper.StyleDataGridView(dgvKho);
            UIHelper.StyleDataGridView(dgvKhoSelection);
            UIHelper.StyleDataGridView(dgvGioHang);
            UIHelper.StyleDataGridView(dgvNCC);
            UIHelper.StyleDataGridView(dgvPhieuNhap);
            UIHelper.StyleDataGridView(dgvChiTietPhieu);

            UIHelper.StyleButton(btnThemNL, true);
            UIHelper.StyleButton(btnSuaNL, false);
            UIHelper.StyleButton(btnXoaNL, false);

            UIHelper.StyleButton(btnXacNhanNhap, true);

            btnXoaDongGH = UIHelper.CreateButton("Xóa", 100, Color.IndianRed);
            pnlActionNhap.Controls.Add(btnXoaDongGH);

            btnXoaDongGH.Location = new Point(225, 67);
            UIHelper.StyleButton(btnThemNCC, true);
            UIHelper.StyleButton(btnSuaNCC, false);
            UIHelper.StyleButton(btnXoaNCC, false);

            UIHelper.StyleButton(btnXoaPhieu, false);

            if (currentUser != null)
            {
                if (currentUser.VaiTro == "Phục vụ" || currentUser.VaiTro == "Thu ngân")
                {
                    btnXoaNL.Visible = false;
                    btnXoaPhieu.Visible = false;
                    btnXoaNCC.Visible = false;
                }
            }
        }

        private void BindEvents()
        {
            btnThemNL.Click += BtnThemNL_Click;
            btnSuaNL.Click += BtnSuaNL_Click;
            btnXoaNL.Click += BtnXoaNL_Click;

            dgvKhoSelection.CellDoubleClick += DgvKhoSelection_CellDoubleClick;
            btnXacNhanNhap.Click += BtnXacNhanNhap_Click;
            dgvGioHang.CellDoubleClick += DgvGioHang_CellDoubleClick;
            btnXoaDongGH.Click += BtnXoaDongGH_Click;

            dgvPhieuNhap.CellClick += DgvPhieuNhap_CellClick;
            btnXoaPhieu.Click += BtnXoaPhieu_Click;

            btnThemNCC.Click += BtnThemNCC_Click;
            btnSuaNCC.Click += BtnSuaNCC_Click;
            btnXoaNCC.Click += BtnXoaNCC_Click;
        }

        // =========================================================
        // LOGIC CHUNG & TAB LOAD
        // =========================================================
        private void TabCtrlKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabCtrlKho.SelectedTab == tabTonKho) LoadDataKho();
            else if (tabCtrlKho.SelectedTab == tabNhapHang)
            {
                LoadDataKhoSelection();
                LoadDataNCC();
            }
            else if (tabCtrlKho.SelectedTab == tabPhieuNhap) LoadDataPhieuNhap();
            else if (tabCtrlKho.SelectedTab == tabNCC) LoadDataNCC();
        }

        private void LoadDataKho()
        {
            dgvKho.DataSource = bll.GetInventory();
            UIHelper.SetGridColumns(dgvKho,
                new[] { "Mã NL", "Tên Nguyên Liệu", "Tồn Kho", "Đơn Vị" },
                new[] { "MaNguyenLieu", "Ten", "SoLuongTon", "DonVi" },
                new[] { 300, 0, 300, 300 });

            int countCanhBao = 0;

            foreach (DataGridViewRow row in dgvKho.Rows)
            {
                if (row.Cells["SoLuongTon"].Value != null)
                {
                    if (decimal.TryParse(row.Cells["SoLuongTon"].Value.ToString(), out decimal sl))
                    {
                        if (sl <= 5)
                        {
                            row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 192);
                            row.DefaultCellStyle.ForeColor = Color.Red;
                            row.DefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);

                            row.Cells["SoLuongTon"].ToolTipText = "Cảnh báo: Sắp hết hàng!";

                            countCanhBao++;
                        }
                    }
                }
            }
        }

        // =========================================================
        // 1. MODULE NGUYÊN LIỆU (THÊM/SỬA/XÓA)
        // =========================================================
        private void BtnThemNL_Click(object sender, EventArgs e)
        {
            frmAddEditNL f = new frmAddEditNL(null);
            if (f.ShowDialog() == DialogResult.OK)
            {
                NguyenLieuDTO nl = f.NguyenLieuData;
                string msg = bll.AddNL(nl.Ten, nl.DonVi);

                if (msg.Contains("OK") || msg.Contains("thành công"))
                {
                    MessageBox.Show("Thêm nguyên liệu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataKho();
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSuaNL_Click(object sender, EventArgs e)
        {
            if (dgvKho.CurrentRow == null) return;

            var row = dgvKho.CurrentRow;
            NguyenLieuDTO nlHienTai = new NguyenLieuDTO
            {
                MaNguyenLieu = row.Cells["MaNguyenLieu"].Value.ToString(),
                Ten = row.Cells["Ten"].Value.ToString(),
                DonVi = row.Cells["DonVi"].Value.ToString(),
                SoLuongTon = Convert.ToDecimal(row.Cells["SoLuongTon"].Value)
            };

            frmAddEditNL f = new frmAddEditNL(nlHienTai);
            if (f.ShowDialog() == DialogResult.OK)
            {
                NguyenLieuDTO nlMoi = f.NguyenLieuData;
                string msg = bll.UpdateNL(nlMoi.MaNguyenLieu, nlMoi.Ten, nlMoi.DonVi);

                if (msg.Contains("OK") || msg.Contains("thành công"))
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataKho();
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnXoaNL_Click(object sender, EventArgs e)
        {
            if (dgvKho.CurrentRow != null)
            {
                string ma = dgvKho.CurrentRow.Cells["MaNguyenLieu"].Value.ToString();
                if (MessageBox.Show("Xóa nguyên liệu này?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    MessageBox.Show(bll.DeleteIngredient(ma));
                    LoadDataKho();
                }
            }
        }

        // =========================================================
        // 2. MODULE NHẬP HÀNG
        // =========================================================
        private void LoadDataKhoSelection()
        {
            dgvKhoSelection.DataSource = bll.GetInventory();
            UIHelper.SetGridColumns(dgvKhoSelection,
                new[] { "Mã NL", "Tên" },
                new[] { "MaNguyenLieu", "Ten" },
                new[] { 150, 0 });
        }

        private void DgvKhoSelection_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string ma = dgvKhoSelection.Rows[e.RowIndex].Cells["MaNguyenLieu"].Value.ToString();
            string ten = dgvKhoSelection.Rows[e.RowIndex].Cells["Ten"].Value.ToString();

            if (gioHangNhap.Any(x => x.MaNguyenLieu == ma))
            {
                MessageBox.Show($"Nguyên liệu '{ten}' đã có trong giỏ hàng!\nVui lòng sửa dòng cũ thay vì thêm mới.", "Trùng lặp");
                return;
            }

            frmEditChiTietNhap frm = new frmEditChiTietNhap(ma, ten, null);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                gioHangNhap.Add(frm.ResultItem);
                RefreshGioHang();
            }
        }

        private void RefreshGioHang()
        {
            dgvGioHang.DataSource = null;
            dgvGioHang.DataSource = gioHangNhap;

            UIHelper.SetGridColumns(dgvGioHang,
                new[] { "Mã NL", "Tên NL", "Lượng Yêu Cầu", "Lượng Thực Tế", "Đơn Giá", "Tình Trạng", "Thành Tiền" },
                new[] { "MaNguyenLieu", "Ten", "LuongYeuCau", "LuongThucTe", "DonGia", "TinhTrang", "ThanhTien" },
                new[] { 100, 0, 130, 130, 130, 100, 100 });

            lblTongTienNhap.Text = $"Tổng: {gioHangNhap.Sum(x => x.ThanhTien):N0} VNĐ";
        }

        private void BtnXacNhanNhap_Click(object sender, EventArgs e)
        {
            if (cboNCC_Import.SelectedValue == null) { MessageBox.Show("Chưa chọn NCC!"); return; }
            if (gioHangNhap.Count == 0) { MessageBox.Show("Giỏ hàng trống!"); return; }

            List<ChiTietNhapDTO> listDTO = gioHangNhap.Select(x => new ChiTietNhapDTO
            {
                MaNguyenLieu = x.MaNguyenLieu,
                LuongYeuCau = x.LuongYeuCau,
                LuongThucTe = x.LuongThucTe,
                DonGia = x.DonGia,
                TinhTrang = x.TinhTrang
            }).ToList();

            try
            {
                string msg = bll.ImportStock(
                    cboNCC_Import.SelectedValue.ToString(),
                    currentUser.MaNguoiDung,
                    listDTO,
                    cboNCC_Import.Text
                );

                if (msg.Contains("thành công") || msg.Contains("OK"))
                {
                    MessageBox.Show("Nhập hàng thành công!");
                    gioHangNhap.Clear();
                    RefreshGioHang();
                    LoadDataKho();
                }
                else
                {
                    MessageBox.Show("Hệ thống trả về lỗi:\n" + msg, "Lỗi Lưu Phiếu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi ngoại lệ:\n{ex.Message}", "Lỗi Crash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // 3. MODULE QUẢN LÝ PHIẾU NHẬP
        // =========================================================
        private void LoadDataPhieuNhap()
        {
            dgvPhieuNhap.DataSource = bll.GetAllImportSlips();
            UIHelper.SetGridColumns(dgvPhieuNhap,
                new[] { "Mã Phiếu", "Thời Gian Nhập", "NCC", "Mã Nhân Viên Nhập", "Tổng Tiền" },
                new[] { "MaPhieu", "ThoiGian", "TenNCC", "MaNV", "TongTien" },
                new[] { 150, 120, 0, 150, 100 });
            dgvChiTietPhieu.DataSource = null;
        }

        private void DgvPhieuNhap_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string maPhieu = dgvPhieuNhap.Rows[e.RowIndex].Cells["MaPhieu"].Value.ToString();

                RepositoryDAL dalTemp = new RepositoryDAL();
                dgvChiTietPhieu.DataSource = dalTemp.GetChiTietPhieuNhap(maPhieu);

                UIHelper.SetGridColumns(dgvChiTietPhieu,
                    new[] { "Tên NL", "Yêu Cầu", "Thực Nhập", "Đơn Giá", "Tình Trạng" },
                    new[] { "TenNguyenLieu", "LuongYeuCau", "SoLuong", "DonGia", "TinhTrang" },
                    new[] { 150, 0, 100, 100, 100 });
            }
        }

        private void BtnXoaPhieu_Click(object sender, EventArgs e)
        {
            if (dgvPhieuNhap.CurrentRow == null) return;
            string maPhieu = dgvPhieuNhap.CurrentRow.Cells["MaPhieu"].Value.ToString();

            if (MessageBox.Show($"Xóa phiếu {maPhieu}? (Kho sẽ KHÔNG tự động trừ lại số lượng)",
                "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                string res = bll.DeleteImportSlip(maPhieu);
                MessageBox.Show(res);
                LoadDataPhieuNhap();
                dgvChiTietPhieu.DataSource = null;
            }
        }

        // =========================================================
        // 4. MODULE NHÀ CUNG CẤP
        // =========================================================
        private void LoadDataNCC()
        {
            var dt = bll.GetListNCC();
            dgvNCC.DataSource = dt;
            UIHelper.SetGridColumns(dgvNCC,
                new[] { "Mã NCC", "Tên NCC", "Địa Chỉ", "SĐT" },
                new[] { "MaNCC", "Ten", "DiaChi", "SDT" },
                new[] { 200, 300, 0, 200 });

            cboNCC_Import.DataSource = dt;
            cboNCC_Import.DisplayMember = "Ten";
            cboNCC_Import.ValueMember = "MaNCC";
        }

        private void BtnThemNCC_Click(object sender, EventArgs e)
        {
            frmAddEditNCC f = new frmAddEditNCC(null);
            if (f.ShowDialog() == DialogResult.OK)
            {
                NhaCungCapDTO ncc = f.NCCData;
                string msg = bll.AddSupplier(ncc.Ten, ncc.DiaChi, ncc.SDT);

                if (msg.Contains("thành công") || msg.Contains("OK"))
                {
                    MessageBox.Show("Thêm Nhà cung cấp thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataNCC();
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnSuaNCC_Click(object sender, EventArgs e)
        {
            if (dgvNCC.CurrentRow == null) return;

            var row = dgvNCC.CurrentRow;
            NhaCungCapDTO nccHienTai = new NhaCungCapDTO
            {
                MaNCC = row.Cells["MaNCC"].Value.ToString(),
                Ten = row.Cells["Ten"].Value.ToString(),
                DiaChi = row.Cells["DiaChi"].Value.ToString(),
                SDT = row.Cells["SDT"].Value.ToString()
            };

            frmAddEditNCC f = new frmAddEditNCC(nccHienTai);
            if (f.ShowDialog() == DialogResult.OK)
            {
                NhaCungCapDTO nccMoi = f.NCCData;
                string msg = bll.UpdateSupplier(nccMoi.MaNCC, nccMoi.Ten, nccMoi.DiaChi, nccMoi.SDT);

                if (msg.Contains("thành công") || msg.Contains("OK"))
                {
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadDataNCC();
                }
                else
                {
                    MessageBox.Show(msg, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnXoaNCC_Click(object sender, EventArgs e)
        {
            if (dgvNCC.CurrentRow == null) return;
            string ma = dgvNCC.CurrentRow.Cells["MaNCC"].Value.ToString();
            if (MessageBox.Show("Xóa NCC này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                MessageBox.Show(bll.DeleteSupplier(ma));
                LoadDataNCC();
            }
        }

        private void BtnXoaDongGH_Click(object sender, EventArgs e)
        {
            if (dgvGioHang.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn dòng cần xóa!");
                return;
            }

            string maNL = dgvGioHang.CurrentRow.Cells["MaNguyenLieu"].Value.ToString();
            var item = gioHangNhap.FirstOrDefault(x => x.MaNguyenLieu == maNL);

            if (item != null)
            {
                gioHangNhap.Remove(item);
                RefreshGioHang();
            }
        }

        private void DgvGioHang_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string ma = dgvGioHang.Rows[e.RowIndex].Cells["MaNguyenLieu"].Value.ToString();
            string ten = dgvGioHang.Rows[e.RowIndex].Cells["Ten"].Value.ToString();

            var item = gioHangNhap.FirstOrDefault(x => x.MaNguyenLieu == ma);
            if (item != null)
            {
                frmEditChiTietNhap frm = new frmEditChiTietNhap(ma, ten, item);
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    var newItem = frm.ResultItem;
                    item.LuongYeuCau = newItem.LuongYeuCau;
                    item.LuongThucTe = newItem.LuongThucTe;
                    item.DonGia = newItem.DonGia;
                    item.TinhTrang = newItem.TinhTrang;

                    RefreshGioHang();
                }
            }
        }
    }
}