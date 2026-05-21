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

        // Executes a non-query SQL without showing a MessageBox.
        public void ExecNonQuery(string sql)
        {
            SqlConnection conn = GetConnection();
            conn.Open();
            new SqlCommand(sql, conn).ExecuteNonQuery();
            conn.Close();
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

        public void MigrateToNvarchar()
        {
            RunStep(@"
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
END");

            RunStep(@"
IF (SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME='employee' AND COLUMN_NAME='ename') = 'varchar'
BEGIN
    ALTER TABLE [employee] ALTER COLUMN [ename]    NVARCHAR(250) NOT NULL;
    ALTER TABLE [employee] ALTER COLUMN [gender]   NVARCHAR(50)  NOT NULL;
    ALTER TABLE [employee] ALTER COLUMN [emailid]  NVARCHAR(120) NOT NULL;
    ALTER TABLE [employee] ALTER COLUMN [username] NVARCHAR(150) NOT NULL;
    ALTER TABLE [employee] ALTER COLUMN [pass]     NVARCHAR(150) NOT NULL;
END");

            RunStep(@"
IF (SELECT DATA_TYPE FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME='customer' AND COLUMN_NAME='mobile') <> 'nvarchar'
BEGIN
    ALTER TABLE [customer] ALTER COLUMN [mobile] NVARCHAR(20) NULL;
    UPDATE [customer] SET [mobile] = '0' + [mobile] WHERE LEN([mobile]) = 9;
END");

            // Step 1: create guests table and migrate person data from customer
            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='guests')
BEGIN
    CREATE TABLE [guests] (
        [gid]         INT           IDENTITY(1,1) PRIMARY KEY,
        [cname]       NVARCHAR(250) NOT NULL DEFAULT '',
        [mobile]      NVARCHAR(20)  NULL,
        [nationality] NVARCHAR(250) NOT NULL DEFAULT '',
        [gender]      NVARCHAR(50)  NOT NULL DEFAULT '',
        [dob]         NVARCHAR(50)  NOT NULL DEFAULT '',
        [idproof]     NVARCHAR(250) NOT NULL DEFAULT '',
        [address]     NVARCHAR(350) NOT NULL DEFAULT ''
    )
    INSERT INTO [guests] ([cname],[mobile],[nationality],[gender],[dob],[idproof],[address])
    SELECT [cname],[mobile],[nationality],[gender],[dob],[idproof],[address]
    FROM [customer] ORDER BY [cid]
END");

            // Step 2: create bookings table — runs after guests exists so CROSS APPLY compiles fine
            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='bookings')
BEGIN
    CREATE TABLE [bookings] (
        [bid]      INT           IDENTITY(1,1) PRIMARY KEY,
        [gid]      INT           NOT NULL,
        [roomid]   INT           NOT NULL,
        [checkin]  NVARCHAR(250) NULL,
        [checkout] NVARCHAR(250) NULL,
        [chekout]  NVARCHAR(250) NOT NULL DEFAULT 'NO',
        FOREIGN KEY ([gid])    REFERENCES [guests]([gid]),
        FOREIGN KEY ([roomid]) REFERENCES [rooms]([roomid])
    )
    INSERT INTO [bookings] ([gid],[roomid],[checkin],[checkout],[chekout])
    SELECT g.[gid], c.[roomid], c.[checkin], c.[checkout], c.[chekout]
    FROM [customer] c
    CROSS APPLY (
        SELECT TOP 1 [gid] FROM [guests]
        WHERE [cname]=c.[cname] AND [mobile]=c.[mobile]
        ORDER BY [gid]
    ) g
END");

            // Step 3: replace customer_services(cid) with customer_services(bid)
            RunStep(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='customer_services' AND COLUMN_NAME='cid')
    DROP TABLE [customer_services]");

            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='customer_services')
    CREATE TABLE [customer_services] (
        [id]        INT           IDENTITY(1,1) PRIMARY KEY,
        [bid]       INT           NOT NULL,
        [sid]       INT           NOT NULL,
        [quantity]  INT           NOT NULL DEFAULT 1,
        [used_date] NVARCHAR(50)  NOT NULL DEFAULT '',
        FOREIGN KEY ([bid]) REFERENCES [bookings]([bid]),
        FOREIGN KEY ([sid]) REFERENCES [services]([sid])
    )");

            // Step 4: replace invoices(cid) with invoices(bid)
            RunStep(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='invoices' AND COLUMN_NAME='cid')
    DROP TABLE [invoices]");

            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='invoices')
    CREATE TABLE [invoices] (
        [invoiceId]   INT           IDENTITY(1,1) PRIMARY KEY,
        [invoiceNo]   NVARCHAR(20)  NOT NULL,
        [bid]         INT           NOT NULL,
        [createdDate] DATETIME      NOT NULL DEFAULT GETDATE(),
        [totalAmount] BIGINT        NOT NULL,
        [status]      NVARCHAR(50)  NOT NULL DEFAULT N'Đã thanh toán',
        FOREIGN KEY ([bid]) REFERENCES [bookings]([bid])
    )");
        }

        private void RunStep(string sql)
        {
            try
            {
                SqlConnection conn = GetConnection();
                conn.Open();
                new SqlCommand(sql, conn).ExecuteNonQuery();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Migration error:\n" + ex.Message, "DB Setup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
