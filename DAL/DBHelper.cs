using System;
using System.Data;
using System.Data.SqlClient;

namespace DAL
{
    public class DBHelper
    {
        //MSI\\SQLEXPRESS
        private static string strConn = "Data Source=MSI\\SQLEXPRESS;Initial Catalog=NhaHang;Integrated Security=True;Encrypt=False";

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(strConn);
        }

        public static DataTable ExecuteQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
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
                    throw new Exception("Lỗi truy vấn: " + ex.Message);
                }
            }
        }

        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                        {
                            cmd.Parameters.AddRange(parameters);
                        }
                        return cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi thực thi lệnh: " + ex.Message);
                }
            }
        }

        public static object ExecuteScalar(string query, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                try
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        if (parameters != null && parameters.Length > 0)
                            cmd.Parameters.AddRange(parameters);
                        return cmd.ExecuteScalar();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Lỗi Scalar: " + ex.Message);
                }
            }
        }
    }
}