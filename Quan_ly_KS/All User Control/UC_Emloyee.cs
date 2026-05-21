using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_Emloyee : UserControl
    {
        private function fn = new function();
        private int _editingId = -1;

        // List controls
        private DataGridView dgvEmployees;
        private Guna2TextBox txtSearch;
        private Label lblCount;

        // Form overlay controls (Guna2 to match UC_DichVu)
        private Panel pnlForm;
        private Label lblFormTitle;
        private Guna2TextBox fEname, fMobile, fEmail, fUser, fPass;
        private Guna2ComboBox fGender;
        private Guna2ComboBox fRole;
        private Dictionary<string, int> _roleMap = new Dictionary<string, int>();

        public UC_Emloyee()
        {
            InitializeComponent();
            BuildUI();
        }

        // ─── MAIN LAYOUT ──────────────────────────────────────────────────────────

        private void BuildUI()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Header: title+breadcrumb left, search+add-button right (same as UC_DichVu)
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.White };
            pnlHeader.Controls.Add(new Label {
                Text = "Danh Sách Nhân Viên",
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true, Left = 20, Top = 18
            });
            pnlHeader.Controls.Add(new Label {
                Text = "Quản lý nhân viên  /  Danh sách nhân viên",
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = true, Left = 22, Top = 62
            });

            // Guna2TextBox search — matching UC_DichVu txtSearch
            txtSearch = new Guna2TextBox {
                BorderRadius = 8,
                FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Tìm kiếm nhân viên...",
                Width = 250, Height = 40, Top = 20
            };
            txtSearch.TextChanged += (s, e) => LoadData(txtSearch.Text.Trim());

            // Guna2Button add — matching UC_DichVu btnAdd exactly
            var btnAdd = new Guna2Button {
                BorderRadius = 8,
                FillColor = Color.SlateBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = "+  Thêm Nhân Viên",
                Width = 200, Height = 45, Top = 18,
                Cursor = Cursors.Hand
            };
            btnAdd.Click += (s, e) => ShowForm(-1);

            Action layoutHeader = () => {
                if (pnlHeader.Width < 200) return;
                btnAdd.Left    = pnlHeader.Width - btnAdd.Width - 24;
                txtSearch.Left = btnAdd.Left - txtSearch.Width - 12;
            };
            pnlHeader.Resize       += (s, e) => layoutHeader();
            pnlHeader.HandleCreated += (s, e) => layoutHeader();

            pnlHeader.Controls.Add(txtSearch);
            pnlHeader.Controls.Add(btnAdd);

            // Thin separator
            var sep = new Panel { Dock = DockStyle.Top, Height = 1, BackColor = Color.FromArgb(220, 220, 235) };

            // Status bar
            var pnlBottom = new Panel { Dock = DockStyle.Bottom, Height = 36, BackColor = Color.White };
            lblCount = new Label { Text = "", Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray, AutoSize = true, Left = 22, Top = 10 };
            pnlBottom.Controls.Add(lblCount);

            // Grid
            var pnlGrid = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20, 10, 20, 10), BackColor = Color.FromArgb(245, 245, 250) };
            dgvEmployees = new DataGridView {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                GridColor = Color.FromArgb(230, 230, 240),
                EditMode = DataGridViewEditMode.EditProgrammatically,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                ColumnHeadersHeight = 45
            };
            SetupGrid();
            dgvEmployees.CellContentClick += DgvEmployees_CellContentClick;
            pnlGrid.Controls.Add(dgvEmployees);

            BuildFormPanel();

            this.Controls.Add(pnlGrid);
            this.Controls.Add(pnlBottom);
            this.Controls.Add(sep);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlForm); // absolute-positioned overlay
        }

        // ─── GRID ─────────────────────────────────────────────────────────────────

        private void SetupGrid()
        {
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSTT",      HeaderText = "STT",           Width = 60  });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEid",      HeaderText = "Mã NV",         Width = 80  });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName",     HeaderText = "Họ và Tên",     MinimumWidth = 180 });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { Name = "colGender",   HeaderText = "Giới Tính",     Width = 110 });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMobile",   HeaderText = "Số Điện Thoại", Width = 155 });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { Name = "colEmail",    HeaderText = "Email",         MinimumWidth = 180 });
            dgvEmployees.Columns.Add(new DataGridViewTextBoxColumn { Name = "colUsername", HeaderText = "Tài Khoản",     Width = 150 });
            dgvEmployees.Columns.Add(new DataGridViewButtonColumn  { Name = "colEdit",   HeaderText = "Sửa",  Text = "✏", UseColumnTextForButtonValue = true, Width = 70 });
            dgvEmployees.Columns.Add(new DataGridViewButtonColumn  { Name = "colDelete", HeaderText = "Xóa",  Text = "🗑", UseColumnTextForButtonValue = true, Width = 70 });

            dgvEmployees.Columns["colName"].AutoSizeMode  = DataGridViewAutoSizeColumnMode.Fill;
            dgvEmployees.Columns["colEmail"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvEmployees.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvEmployees.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(132, 112, 255);
            dgvEmployees.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEmployees.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEmployees.EnableHeadersVisualStyles = false;
            dgvEmployees.RowTemplate.Height = 45;
            dgvEmployees.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvEmployees.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 191, 255);
            dgvEmployees.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvEmployees.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 244, 255);
        }

        // ─── FORM OVERLAY — matching UC_DichVu exact measurements ─────────────────
        // UC_DichVu: pnlForm 600×410, lx=20, fw=555, inputH=40
        // Label→input gap: 25px; fieldGap: 82px; startY: 78; btn gap: 33px

        private void BuildFormPanel()
        {
            const int formW  = 600;
            const int lx     = 20;
            const int fw     = 555;
            const int inputH = 40;
            const int startY = 78;  // first label top (same as UC_DichVu)
            const int l2i    = 25;  // label-to-input vertical offset
            const int fGap   = 82;  // between consecutive label tops
            const int btnGap = 33;  // gap between last input bottom and button top

            // 7 fields → last input bottom, then button row
            int lastInputBot = startY + l2i + 6 * fGap + inputH;  // 78+25+492+40 = 635
            int btnY         = lastInputBot + btnGap;               // 635+33 = 668
            int formH        = btnY + 45 + 25;                      // 668+45+25 = 738

            pnlForm = new Panel {
                Width = formW, Height = formH,
                BackColor = Color.White,
                Visible = false,
                BorderStyle = BorderStyle.FixedSingle
            };
            this.SizeChanged    += (s, e) => CenterFormPanel();
            this.HandleCreated  += (s, e) => CenterFormPanel();

            // Title — SlateBlue like UC_DichVu lblFormTitle
            lblFormTitle = new Label {
                Text = "Thêm Nhân Viên",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.SlateBlue,
                AutoSize = true, Left = lx, Top = 20
            };
            pnlForm.Controls.Add(lblFormTitle);

            // Field labels (above inputs, same font/color as UC_DichVu)
            string[] lblTexts = { "Họ và Tên", "Giới Tính", "Số Điện Thoại", "Email", "Tài Khoản", "Mật Khẩu", "Quyền" };
            for (int i = 0; i < lblTexts.Length; i++)
            {
                pnlForm.Controls.Add(new Label {
                    Text = lblTexts[i],
                    Left = lx, Top = startY + i * fGap,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 10F),
                    ForeColor = Color.FromArgb(60, 60, 60)
                });
            }

            // Guna2TextBox inputs — matching UC_DichVu txtServiceName/txtPrice style
            fEname  = G2Txt(lx, startY + l2i + 0*fGap, fw, inputH, "Nhập họ và tên");
            fMobile = G2Txt(lx, startY + l2i + 2*fGap, fw, inputH, "Nhập số điện thoại");
            fEmail  = G2Txt(lx, startY + l2i + 3*fGap, fw, inputH, "Nhập địa chỉ email");
            fUser   = G2Txt(lx, startY + l2i + 4*fGap, fw, inputH, "Nhập tên tài khoản");
            fPass   = G2Txt(lx, startY + l2i + 5*fGap, fw, inputH, "Nhập mật khẩu");
            fPass.PasswordChar = '*';

            // Guna2ComboBox gender — matching UC_DichVu cmbFormStatus style
            fGender = new Guna2ComboBox {
                BorderRadius = 8,
                FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Left = lx, Top = startY + l2i + 1*fGap,
                Width = fw, Height = inputH
            };
            fGender.Items.AddRange(new[] { "Nam", "Nữ", "Khác" });
            fGender.SelectedIndex = 0;

            fRole = new Guna2ComboBox {
                BorderRadius = 8,
                FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Left = lx, Top = startY + l2i + 6*fGap,
                Width = fw, Height = inputH
            };

            pnlForm.Controls.AddRange(new Control[] { fEname, fGender, fMobile, fEmail, fUser, fPass, fRole });

            // Guna2Button Lưu — matching UC_DichVu btnSave (SlateBlue, BorderRadius=8, 160×45)
            var btnSave = new Guna2Button {
                BorderRadius = 8,
                FillColor = Color.SlateBlue,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = "Lưu",
                Left = lx, Top = btnY, Width = 160, Height = 45,
                Cursor = Cursors.Hand
            };
            btnSave.Click += BtnSave_Click;

            // Guna2Button Hủy — matching UC_DichVu btnCancel (grey, BorderRadius=8, 160×45)
            var btnCancel = new Guna2Button {
                BorderRadius = 8,
                FillColor = Color.FromArgb(220, 220, 220),
                ForeColor = Color.FromArgb(80, 80, 80),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = "Hủy",
                Left = 200, Top = btnY, Width = 160, Height = 45,
                Cursor = Cursors.Hand
            };
            btnCancel.Click += (s, e) => pnlForm.Visible = false;

            pnlForm.Controls.Add(btnSave);
            pnlForm.Controls.Add(btnCancel);
        }

        private static Guna2TextBox G2Txt(int x, int y, int w, int h, string placeholder) =>
            new Guna2TextBox {
                BorderRadius = 8,
                FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = placeholder,
                Left = x, Top = y, Width = w, Height = h
            };

        // gender column is VARCHAR — store ASCII-safe codes, display Vietnamese
        private static string GenderToDisplay(string stored)
        {
            if (stored == "Nu")   return "Nữ";
            if (stored == "Khac") return "Khác";
            return stored; // "Nam" stays "Nam"
        }
        private static string GenderToStore(string display)
        {
            if (display == "Nữ")   return "Nu";
            if (display == "Khác") return "Khac";
            return display; // "Nam" stays "Nam"
        }

        private void LoadRolesCombo()
        {
            _roleMap.Clear();
            fRole.Items.Clear();
            try
            {
                DataSet ds = fn.GetData("SELECT rid, roleName FROM roles ORDER BY rid");
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    string name = r["roleName"].ToString();
                    _roleMap[name] = Convert.ToInt32(r["rid"]);
                    fRole.Items.Add(name);
                }
                if (fRole.Items.Count > 0) fRole.SelectedIndex = 0;
            }
            catch { }
        }

        private void CenterFormPanel()
        {
            if (this.Width < 100 || this.Height < 100) return;
            pnlForm.Left = (this.Width  - pnlForm.Width)  / 2;
            pnlForm.Top  = (this.Height - pnlForm.Height) / 2;
        }

        // ─── LOAD / EVENTS ────────────────────────────────────────────────────────

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CenterFormPanel();
            LoadData();
        }

        public void LoadData(string filter = "")
        {
            string sql = "SELECT eid, ename, gender, mobile, emailid, username FROM employee WHERE 1=1";
            if (!string.IsNullOrEmpty(filter))
                sql += " AND ename LIKE N'%" + filter.Replace("'", "''") + "%'";
            sql += " ORDER BY eid";

            try
            {
                DataSet ds = fn.GetData(sql);
                dgvEmployees.Rows.Clear();
                int stt = 1;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    int rawId = Convert.ToInt32(r["eid"]);
                    string maNV = "NV_" + rawId.ToString().PadLeft(2, '0');
                    dgvEmployees.Rows.Add(
                        stt++, maNV, r["ename"],
                        GenderToDisplay(r["gender"].ToString()),
                        r["mobile"], r["emailid"], r["username"],
                        "✏", "🗑"
                    );
                }
                lblCount.Text = string.Format("Hiển thị {0} nhân viên", dgvEmployees.Rows.Count);
            }
            catch { }
        }

        private void DgvEmployees_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            string maNV = dgvEmployees.Rows[e.RowIndex].Cells["colEid"].Value.ToString();
            int eid = int.Parse(maNV.Replace("NV_", "").TrimStart('0').PadLeft(1, '0'));

            if (e.ColumnIndex == dgvEmployees.Columns["colEdit"].Index)
            {
                ShowForm(eid);
            }
            else if (e.ColumnIndex == dgvEmployees.Columns["colDelete"].Index)
            {
                string name = dgvEmployees.Rows[e.RowIndex].Cells["colName"].Value?.ToString();
                if (MessageBox.Show(
                        string.Format("Bạn có chắc muốn xóa nhân viên '{0}'?", name),
                        "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                    ) == DialogResult.Yes)
                {
                    fn.SetData("DELETE FROM employee WHERE eid=" + eid, "Xóa nhân viên thành công!");
                    LoadData(txtSearch.Text.Trim());
                }
            }
        }

        // ─── SHOW / SAVE FORM ─────────────────────────────────────────────────────

        private void ShowForm(int eid)
        {
            _editingId = eid;
            bool isEdit = eid > 0;

            fEname.Text = ""; fMobile.Text = ""; fEmail.Text = ""; fUser.Text = ""; fPass.Text = "";
            fGender.SelectedIndex = 0;
            fPass.PasswordChar = '*';
            lblFormTitle.Text = isEdit ? "Cập Nhật Nhân Viên" : "Thêm Nhân Viên";
            LoadRolesCombo();

            if (isEdit)
            {
                try
                {
                    DataSet ds = fn.GetData("SELECT * FROM employee WHERE eid=" + eid);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow r = ds.Tables[0].Rows[0];
                        fEname.Text  = r["ename"].ToString();
                        fMobile.Text = r["mobile"].ToString();
                        fEmail.Text  = r["emailid"].ToString();
                        fUser.Text   = r["username"].ToString();
                        fPass.Text   = r["pass"].ToString();
                        fPass.PasswordChar = '\0';
                        string g = GenderToDisplay(r["gender"].ToString());
                        fGender.SelectedItem = fGender.Items.Contains(g) ? g : "Nam";

                        int currentRid = r["rid"] != DBNull.Value ? Convert.ToInt32(r["rid"]) : 0;
                        for (int i = 0; i < fRole.Items.Count; i++)
                        {
                            string itemName = fRole.Items[i].ToString();
                            if (_roleMap.ContainsKey(itemName) && _roleMap[itemName] == currentRid)
                            { fRole.SelectedIndex = i; break; }
                        }
                    }
                }
                catch { }
            }

            CenterFormPanel();
            pnlForm.Visible = true;
            pnlForm.BringToFront();
            fEname.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            string n  = fEname.Text.Trim();
            string m  = fMobile.Text.Trim();
            string em = fEmail.Text.Trim();
            string u  = fUser.Text.Trim();
            string p  = fPass.Text.Trim();
            string g  = GenderToStore(fGender.SelectedItem?.ToString() ?? "Nam");

            if (string.IsNullOrEmpty(n) || string.IsNullOrEmpty(m) ||
                string.IsNullOrEmpty(em) || string.IsNullOrEmpty(u) || string.IsNullOrEmpty(p))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!long.TryParse(m, out _))
            {
                MessageBox.Show("Số điện thoại phải là số!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nS  = n.Replace("'",  "''");
            string emS = em.Replace("'", "''");
            string uS  = u.Replace("'",  "''");
            string pS  = p.Replace("'",  "''");
            bool isEdit = _editingId > 0;

            string roleName = fRole.SelectedItem?.ToString() ?? "";
            string ridSql = (_roleMap.ContainsKey(roleName) && _roleMap[roleName] > 0)
                ? _roleMap[roleName].ToString() : "NULL";

            string sql = isEdit
                ? string.Format(
                    "UPDATE employee SET ename=N'{0}',mobile={1},gender=N'{2}',emailid='{3}',username='{4}',pass='{5}',rid={6} WHERE eid={7}",
                    nS, m, g, emS, uS, pS, ridSql, _editingId)
                : string.Format(
                    "INSERT INTO employee (ename,mobile,gender,emailid,username,pass,rid) VALUES (N'{0}',{1},N'{2}','{3}','{4}','{5}',{6})",
                    nS, m, g, emS, uS, pS, ridSql);

            try
            {
                fn.SetData(sql, isEdit ? "Cập nhật nhân viên thành công!" : "Thêm nhân viên thành công!");
                pnlForm.Visible = false;
                LoadData(txtSearch.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
