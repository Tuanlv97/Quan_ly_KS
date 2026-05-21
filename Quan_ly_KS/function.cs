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

        // Runs once on startup: upgrades VARCHAR text columns to NVARCHAR so
        // Vietnamese diacritics are stored and read back correctly.
        public void MigrateToNvarchar()
        {
            string sql = @"
IF (SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME='customer' AND COLUMN_NAME='cname') = 'varchar'
BEGIN
    ALTER TABLE [customer] ALTER COLUMN [cname]       NVARCHAR(250) NOT NULL;
    ALTER TABLE [customer] ALTER COLUMN [nationality] NVARCHAR(250) NOT NULL;
    ALTER TABLE [customer] ALTER COLUMN [gender]      NVARCHAR(50)  NOT NULL;
    ALTER TABLE [customer] ALTER COLUMN [dob]         NVARCHAR(50)  NOT NULL;
    ALTER TABLE [customer] ALTER COLUMN [idproof]     NVARCHAR(250) NOT NULL;
    ALTER TABLE [customer] ALTER COLUMN [address]     NVARCHAR(350) NOT NULL;
    ALTER TABLE [customer] ALTER COLUMN [checkin]     NVARCHAR(250) NULL;
    ALTER TABLE [customer] ALTER COLUMN [checkout]    NVARCHAR(250) NULL;
    ALTER TABLE [customer] ALTER COLUMN [chekout]     NVARCHAR(250) NULL;
END
IF (SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME='employee' AND COLUMN_NAME='ename') = 'varchar'
BEGIN
    ALTER TABLE [employee] ALTER COLUMN [ename]    NVARCHAR(250) NOT NULL;
    ALTER TABLE [employee] ALTER COLUMN [gender]   NVARCHAR(50)  NOT NULL;
    ALTER TABLE [employee] ALTER COLUMN [emailid]  NVARCHAR(120) NOT NULL;
    ALTER TABLE [employee] ALTER COLUMN [username] NVARCHAR(150) NOT NULL;
    ALTER TABLE [employee] ALTER COLUMN [pass]     NVARCHAR(150) NOT NULL;
END
IF (SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME='customer' AND COLUMN_NAME='mobile') <> 'nvarchar'
BEGIN
    ALTER TABLE [customer] ALTER COLUMN [mobile] NVARCHAR(20) NULL;
    UPDATE [customer] SET [mobile] = '0' + [mobile] WHERE LEN([mobile]) = 9;
END
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='customer_services')
BEGIN
    CREATE TABLE [customer_services] (
        [id]        INT           IDENTITY(1,1) PRIMARY KEY,
        [cid]       INT           NOT NULL,
        [sid]       INT           NOT NULL,
        [quantity]  INT           NOT NULL DEFAULT 1,
        [used_date] NVARCHAR(50)  NOT NULL,
        FOREIGN KEY ([cid]) REFERENCES [customer]([cid]),
        FOREIGN KEY ([sid]) REFERENCES [services]([sid])
    )
END";
            try
            {
                SqlConnection conn = GetConnection();
                conn.Open();
                new SqlCommand(sql, conn).ExecuteNonQuery();
                conn.Close();
            }
            catch { }
        }
    }
}
