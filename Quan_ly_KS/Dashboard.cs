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

        private const float DESIGN_UC_W = 1882f;

        private void LayoutDashboard()
        {
            int w = ClientSize.Width;
            int h = ClientSize.Height;

            // === Navbar (guna2Panel1) ===
            const int NAV_LEFT = 94, NAV_TOP = 17, NAV_H = 110;
            int navW = w - NAV_LEFT - 6;
            guna2Panel1.Location = new Point(NAV_LEFT, NAV_TOP);
            guna2Panel1.Size = new Size(navW, NAV_H);

            // Chia đều các nút visible, chừa 290px bên phải cho user profile
            const int PROFILE_W = 290;
            int availForBtns = navW - PROFILE_W - 20;
            var navBtns = new Guna2Button[] {
                btnTongQuan, btnAddRoom, btnCustomerRes, btnCheckOut,
                btnCustomerDetail, btnEmployee, btnDichVu, btnDichVuKhach
            }.Where(b => b.Visible).ToArray();
            int spacing = availForBtns / navBtns.Length;
            int btnW = spacing - 6;
            float fs = Math.Max(7f, Math.Min(10f, btnW * 10f / 179f));
            var navFont = new Font("Segoe UI", fs, FontStyle.Bold);

            for (int i = 0; i < navBtns.Length; i++)
            {
                navBtns[i].Location = new Point(10 + i * spacing, 10);
                navBtns[i].Size = new Size(btnW, 88);
                navBtns[i].Font = navFont;
            }

            pnlUserProfile.Location = new Point(navW - PROFILE_W, 0);

            // Gạch indicator ngắn, nhỏ, căn giữa button
            int pmW = Math.Max(36, Math.Min(52, btnW / 2));
            PanelMoving.Size = new Size(pmW, 4);
            PanelMoving.Top = NAV_TOP + NAV_H + 2;

            // === Content panel lấp đầy phần còn lại ===
            const int CONTENT_LEFT = 37, CONTENT_TOP = 148;
            int contentW = w - CONTENT_LEFT - 6;
            int contentH = h - CONTENT_TOP - 6;
            guna2Panel2.Location = new Point(CONTENT_LEFT, CONTENT_TOP);
            guna2Panel2.Size = new Size(contentW, contentH);

            // Tất cả UC fill đầy content panel (giống cách Nhân Viên đang hoạt động)
            foreach (Control uc in guna2Panel2.Controls)
            {
                uc.Dock = DockStyle.Fill;
                uc.Location = new Point(0, 0);
            }
        }

        // Ẩn tất cả UC trước khi hiện tab mới — tránh UC cũ lộ ra phía sau
        private void HideAllUCs()
        {
            uC_Dashboard1.Visible = false;
            uC_AddRoom1.Visible = false;
            uC_AddRoom2.Visible = false;
            uC_CustomerRes1.Visible = false;
            uC_CheckOut1.Visible = false;
            uC_CustomerDetails1.Visible = false;
            uC_Emloyee1.Visible = false;
            uC_DichVu1.Visible = false;
            uC_DichVuKhach1.Visible = false;
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e) { }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private bool IsAdmin =>
            string.Equals(Session.RoleName, "Admin", StringComparison.OrdinalIgnoreCase);

        private void Dashboard_Load(object sender, EventArgs e)
        {
            bool isAdmin = IsAdmin;
            btnTongQuan.Visible = isAdmin;
            btnAddRoom.Visible = isAdmin;
            btnEmployee.Visible = isAdmin;

            HideAllUCs();
            LayoutDashboard();

            if (isAdmin)
                btnTongQuan.PerformClick();
            else
                btnCustomerRes.PerformClick();

            string name = Session.EmployeeName;
            string role = Session.RoleName;
            lblUserName.Text = name;
            lblUserRole.Text = role;

            string[] parts = name.Trim().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
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

            using (var brush = new SolidBrush(Color.FromArgb(90, 70, 200)))
                g.FillEllipse(brush, 1, 1, panel.Width - 2, panel.Height - 2);

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
            PanelMoving.Left = guna2Panel1.Left + btnTongQuan.Left + (btnTongQuan.Width - PanelMoving.Width) / 2;
            HideAllUCs();
            uC_Dashboard1.Visible = true;
            uC_Dashboard1.BringToFront();
        }

        public void ShowBookingTab() { btnCustomerRes.PerformClick(); }
        public void ShowEmployeeTab() { if (IsAdmin) btnEmployee.PerformClick(); }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = guna2Panel1.Left + btnAddRoom.Left + (btnAddRoom.Width - PanelMoving.Width) / 2;
            HideAllUCs();
            uC_AddRoom1.Visible = true;
            uC_AddRoom1.BringToFront();
        }

        private void btnCustomerRes_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = guna2Panel1.Left + btnCustomerRes.Left + (btnCustomerRes.Width - PanelMoving.Width) / 2;
            HideAllUCs();
            uC_CustomerRes1.Visible = true;
            uC_CustomerRes1.BringToFront();
            uC_CustomerRes1.Reload();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = guna2Panel1.Left + btnCheckOut.Left + (btnCheckOut.Width - PanelMoving.Width) / 2;
            HideAllUCs();
            uC_CheckOut1.Visible = true;
            uC_CheckOut1.BringToFront();
            uC_CheckOut1.ReloadData();
        }

        private void btnCustomerDetail_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = guna2Panel1.Left + btnCustomerDetail.Left + (btnCustomerDetail.Width - PanelMoving.Width) / 2;
            HideAllUCs();
            uC_CustomerDetails1.Visible = true;
            uC_CustomerDetails1.BringToFront();
            uC_CustomerDetails1.Reload();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = guna2Panel1.Left + btnEmployee.Left + (btnEmployee.Width - PanelMoving.Width) / 2;
            HideAllUCs();
            uC_Emloyee1.Visible = true;
            uC_Emloyee1.BringToFront();
        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = guna2Panel1.Left + btnDichVu.Left + (btnDichVu.Width - PanelMoving.Width) / 2;
            HideAllUCs();
            uC_DichVu1.Visible = true;
            uC_DichVu1.BringToFront();
        }

        private void btnDichVuKhach_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = guna2Panel1.Left + btnDichVuKhach.Left + (btnDichVuKhach.Width - PanelMoving.Width) / 2;
            HideAllUCs();
            uC_DichVuKhach1.Visible = true;
            uC_DichVuKhach1.BringToFront();
            uC_DichVuKhach1.Reload();
        }
    }
}
