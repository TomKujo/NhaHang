using BLL;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GUI
{
    public partial class frmAddEditDatBan : Form
    {
        private ServiceBLL bll = new ServiceBLL();

        private string _tableID;
        private string _bookingID;
        private bool _isEdit;

        /// <summary>
        /// Constructor dùng cho cả Thêm mới và Sửa
        /// </summary>
        /// <param name="tableID">Mã bàn</param>
        /// <param name="bookingID">Mã đặt bàn (nếu sửa)</param>
        /// <param name="date">Ngày đặt (nếu sửa)</param>
        /// <param name="name">Tên khách (nếu sửa)</param>
        /// <param name="phone">SĐT khách (nếu sửa)</param>
        public frmAddEditDatBan(string tableID, string bookingID = null, DateTime? date = null, string name = "", string phone = "")
        {
            InitializeComponent();

            this._tableID = tableID;
            this._bookingID = bookingID;
            this._isEdit = !string.IsNullOrEmpty(bookingID);

            UIHelper.SetupDialog(this, _isEdit ? "Sửa lịch đặt" : "Thêm lịch đặt");

            dtpThoiGian.Value = date ?? DateTime.Now.AddDays(1);
            txtTenKhach.Text = name;
            txtSDT.Text = phone;

            if (_isEdit)
            {
                lblTenKhach.Visible = false;
                txtTenKhach.Visible = false;
                lblSDT.Visible = false;
                txtSDT.Visible = false;

                this.Height = 220;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fullDate = dtpThoiGian.Value;

                // Luật 1: Không đặt về quá khứ
                if (fullDate < DateTime.Now)
                {
                    MessageBox.Show("LỖI THỜI GIAN:\nKhông thể đặt bàn lùi về quá khứ!",
                        "Quy tắc đặt bàn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Luật 2: Đặt trước ít nhất 1 ngày
                if (fullDate < DateTime.Now.AddDays(1))
                {
                    MessageBox.Show("QUY ĐỊNH NHÀ HÀNG:\nPhải đặt bàn trước ít nhất 1 ngày (24 giờ)!",
                        "Quy tắc đặt bàn", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!_isEdit)
                {
                    if (string.IsNullOrWhiteSpace(txtTenKhach.Text) || string.IsNullOrWhiteSpace(txtSDT.Text))
                    {
                        MessageBox.Show("Vui lòng nhập tên và số điện thoại khách!", "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                string msg = "";
                if (_isEdit)
                {
                    msg = bll.UpdateBookingSmart(_bookingID, _tableID, fullDate);
                }
                else
                {
                    msg = bll.BookTableSmart(_tableID, txtTenKhach.Text, txtSDT.Text, fullDate);
                }

                MessageBox.Show(msg, "Thông báo", MessageBoxButtons.OK, msg.Contains("thành công") ? MessageBoxIcon.Information : MessageBoxIcon.Error);

                if (msg.Contains("thành công"))
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}