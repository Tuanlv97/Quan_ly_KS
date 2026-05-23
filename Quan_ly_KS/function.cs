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
            conn.ConnectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=dbMyHotel;Integrated Security=True;Connect Timeout=30";
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
    WHERE TABLE_NAME='employee' AND COLUMN_NAME='mobile') <> 'nvarchar'
BEGIN
    ALTER TABLE [employee] ALTER COLUMN [mobile] NVARCHAR(20) NULL;
    UPDATE [employee] SET [mobile] = '0' + CAST([mobile] AS NVARCHAR(20)) WHERE LEN(CAST([mobile] AS NVARCHAR(20))) = 9;
END");

            // Step 1: create guests table
            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='guests')
    CREATE TABLE [guests] (
        [gid]         INT           IDENTITY(1,1) PRIMARY KEY,
        [cname]       NVARCHAR(250) NOT NULL DEFAULT '',
        [mobile]      NVARCHAR(20)  NULL,
        [nationality] NVARCHAR(250) NOT NULL DEFAULT '',
        [gender]      NVARCHAR(50)  NOT NULL DEFAULT '',
        [dob]         NVARCHAR(50)  NOT NULL DEFAULT '',
        [idproof]     NVARCHAR(250) NOT NULL DEFAULT '',
        [address]     NVARCHAR(350) NOT NULL DEFAULT ''
    )");

            // Step 2: create bookings table
            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='bookings')
    CREATE TABLE [bookings] (
        [bid]      INT           IDENTITY(1,1) PRIMARY KEY,
        [gid]      INT           NOT NULL,
        [roomid]   INT           NOT NULL,
        [checkin]  NVARCHAR(250) NULL,
        [checkout] NVARCHAR(250) NULL,
        [chekout]  NVARCHAR(250) NOT NULL DEFAULT 'NO',
        FOREIGN KEY ([gid])    REFERENCES [guests]([gid]),
        FOREIGN KEY ([roomid]) REFERENCES [rooms]([roomid])
    )");

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

            // Step 5: roles table + seed default roles
            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='roles')
BEGIN
    CREATE TABLE [roles] (
        [rid]         INT           IDENTITY(1,1) PRIMARY KEY,
        [roleName]    NVARCHAR(100) NOT NULL,
        [description] NVARCHAR(250) NULL
    )
    INSERT INTO [roles] ([roleName],[description]) VALUES (N'Admin',   N'Quản trị hệ thống')
    INSERT INTO [roles] ([roleName],[description]) VALUES (N'Lễ Tân',  N'Nhân viên lễ tân')
    INSERT INTO [roles] ([roleName],[description]) VALUES (N'Kế Toán', N'Nhân viên kế toán')
END");

            // Step 6: add rid (role FK) to employee
            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='employee' AND COLUMN_NAME='rid')
    ALTER TABLE [employee] ADD [rid] INT NULL");

            // Step 7: seed default admin account if username 'admin' does not exist
            RunStep(@"
IF NOT EXISTS (SELECT * FROM [employee] WHERE [username] = 'admin')
    INSERT INTO [employee] ([ename],[mobile],[gender],[emailid],[username],[pass])
    VALUES (N'Administrator', N'', N'Nam', N'admin@hotel.com', N'admin', N'123')");

            // Step 8: add eid (creator) to invoices
            RunStep(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='invoices')
    IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='invoices' AND COLUMN_NAME='eid')
        ALTER TABLE [invoices] ADD [eid] INT NULL");

            // Step 9: add status column to rooms (Trống / Có khách / Bẩn / Bảo trì)
            RunStep(@"
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='rooms' AND COLUMN_NAME='status')
    ALTER TABLE [rooms] ADD [status] NVARCHAR(50) NOT NULL CONSTRAINT DF_rooms_status DEFAULT N'Trống'");

            // Step 10: sync status from old booked column for existing data
            RunStep(@"
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='rooms' AND COLUMN_NAME='booked')
    UPDATE [rooms] SET [status] = N'Có khách' WHERE [booked] = 'YES' AND [status] = N'Trống'");
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
