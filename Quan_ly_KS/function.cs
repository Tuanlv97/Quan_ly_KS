using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Quan_ly_KS
{
    internal class function
    {
        protected SqlConnection GetConnection()
        {
                        SqlConnection conn = new SqlConnection();
            conn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\DoAnLinh\\Quan_ly_KS\\Data\\dbMyHotel.mdf;Initial Catalog=dbMyHotel;Integrated Security=True;Connect Timeout=30";
            return conn;
        }

        public DataSet GetData(String sql)
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = sql;

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds;
        }

        public void SetData(String sql, String message)
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd.CommandText = sql;
            cmd.ExecuteNonQuery();
            conn.Close();

            MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public SqlDataReader GetForCombo(String sql)
        {
            SqlConnection conn = GetConnection();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            conn.Open();
            cmd = new SqlCommand(sql, conn);
            cmd.CommandText = sql;
            SqlDataReader reader = cmd.ExecuteReader();
            return reader;
        }
    }
}
