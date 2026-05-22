using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quan_ly_KS
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            new function().MigrateToNvarchar();
            InitializeComponent();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            uC_Dashboard1.Visible = false;
            uC_AddRoom1.Visible = false;
            uC_CustomerRes1.Visible = false;
            uC_CheckOut1.Visible = false;
            uC_CustomerDetails1.Visible = false;
            uC_Emloyee1.Visible = false;
            uC_DichVu1.Visible = false;
            uC_DichVuKhach1.Visible = false;
            btnTongQuan.PerformClick();

            // Hiển thị thông tin user đăng nhập
            string name = Session.EmployeeName;
            string role = Session.RoleName;

            lblUserName.Text = name;
            lblUserRole.Text = role;

            // Tạo chữ viết tắt từ tên (lấy chữ cái đầu của 2 từ cuối, tối đa 2 ký tự)
            string[] parts = name.Trim().Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
            string initials = parts.Length >= 2
                ? ("" + parts[parts.Length - 2][0] + parts[parts.Length - 1][0]).ToUpper()
                : (name.Length > 0 ? name.Substring(0, Math.Min(2, name.Length)).ToUpper() : "?");

            btnAvatarCircle.Tag = initials;
            btnAvatarCircle.Paint += AvatarCircle_Paint;
        }

        private void AvatarCircle_Paint(object sender, PaintEventArgs e)
        {
            var panel = (Panel)sender;
            string text = panel.Tag?.ToString() ?? "";
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Vẽ hình tròn nền
            using (var brush = new SolidBrush(Color.FromArgb(90, 70, 200)))
                g.FillEllipse(brush, 1, 1, panel.Width - 2, panel.Height - 2);

            // Vẽ chữ viết tắt căn giữa
            using (var font = new Font("Segoe UI", 14f, FontStyle.Bold))
            using (var brush = new SolidBrush(Color.White))
            {
                SizeF sz = g.MeasureString(text, font);
                g.DrawString(text, font, brush,
                    (panel.Width - sz.Width) / 2f,
                    (panel.Height - sz.Height) / 2f);
            }
        }

        private void btnDangXuat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc chắn muốn đăng xuất không?",
                "Xác nhận đăng xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes) return;

            Session.EmployeeId = 0;
            Session.EmployeeName = "";
            Session.RoleName = "";

            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Close();
        }

        private void btnTongQuan_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnTongQuan.Left + 50;
            uC_Dashboard1.Visible = true;
            uC_Dashboard1.BringToFront();
        }

        public void ShowBookingTab() { btnCustomerRes.PerformClick(); }
        public void ShowEmployeeTab() { btnEmployee.PerformClick(); }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnAddRoom.Left + 50;
            uC_AddRoom1.Visible = true;
            uC_AddRoom1.BringToFront();
        }

        

        private void btnCustomerRes_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnCustomerRes.Left + 60;
            uC_CustomerRes1.Visible = true;
            uC_CustomerRes1.BringToFront();
            uC_CustomerRes1.Reload();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnCheckOut.Left + 50;
            
            uC_CheckOut1.Visible = true;
            uC_CheckOut1.BringToFront();
            uC_CheckOut1.ReloadData();

        }

        private void btnCustomerDetail_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnCustomerDetail.Left + 50;
            uC_CustomerDetails1.Visible = true;
            uC_CustomerDetails1.BringToFront();
            uC_CustomerDetails1.Reload(); // luôn reload data mới nhất
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnEmployee.Left + 60;
            uC_Emloyee1.Visible = true;
            uC_Emloyee1.BringToFront();
        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnDichVu.Left + 60;
            uC_DichVu1.Visible = true;
            uC_DichVu1.BringToFront();
        }

        private void btnDichVuKhach_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnDichVuKhach.Left + 60;
            uC_DichVuKhach1.Visible = true;
            uC_DichVuKhach1.BringToFront();
            uC_DichVuKhach1.Reload();
        }
    }
}
