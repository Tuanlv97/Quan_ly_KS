using System;
using System.Data;
using System.Windows.Forms;

namespace Quan_ly_KS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            try { new function().MigrateToNvarchar(); } catch { }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUsername.Text.Trim();
            string pass = txtPassword.Text;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                LabelError.Visible = true;
                return;
            }

            string safeUser = user.Replace("'", "''");
            string safePass = pass.Replace("'", "''");
            string sql =
                "SELECT e.eid, e.ename, ISNULL(r.roleName, N'') AS roleName " +
                "FROM employee e LEFT JOIN roles r ON e.rid = r.rid " +
                "WHERE e.username = N'" + safeUser + "' AND e.pass = N'" + safePass + "'";

            try
            {
                DataSet ds = new function().GetData(sql);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    Session.EmployeeId   = Convert.ToInt32(dr["eid"]);
                    Session.EmployeeName = dr["ename"].ToString();
                    Session.RoleName     = dr["roleName"].ToString();

                    LabelError.Visible = false;
                    Dashboard dashboard = new Dashboard();
                    this.Hide();
                    dashboard.Show();
                }
                else
                {
                    LabelError.Visible = true;
                    txtPassword.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi đăng nhập: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
