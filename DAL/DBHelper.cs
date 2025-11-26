using System;
using System.Data;
using System.Data.SqlClient; // Thư viện để kết nối SQL Server

namespace DAL
{
    public class DBHelper
    {
        // ⚠️ QUAN TRỌNG: Bạn phải sửa chuỗi kết nối này cho đúng với máy tính của bạn
        // Data Source = Tên máy chủ SQL (VD: DESKTOP-XYZ\SQLEXPRESS hoặc dấu chấm . nếu là bản full)
        // Initial Catalog = Tên cơ sở dữ liệu (NhaHang)
        private static string strConn = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NhaHang;Integrated Security=True;Encrypt=False";

        // Hàm lấy kết nối
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(strConn);
        }

        // Hàm thực thi câu lệnh SELECT (Trả về bảng dữ liệu)
        public static DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        return dt;
                    }
                }
                catch (Exception ex)
                {
                    // Ghi log lỗi nếu cần thiết hoặc ném ra ngoài để debug
                    throw new Exception("Lỗi CSDL: " + ex.Message);
                }
            }
        }

        // Hàm thực thi INSERT, UPDATE, DELETE (Trả về số dòng bị ảnh hưởng)
        public static int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi CSDL: " + ex.Message);
                }
            }
        }
    }
}