using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_CustomerRes : UserControl
    {
        private readonly function fn = new function();
        private int _editingBid     = -1;   // booking id being edited (-1 = new)
        private int _editingGuestId = -1;   // guest id linked to the booking being edited
        private int _editingRoomId  = -1;
        private int _selectedRoomId = -1;
        private int _selectedGuestId = -1;  // guest chosen from the search combo

        private DataGridView  dgvCustomers;
        private Guna2TextBox  txtSearch;
        private Label         lblCount;
        private Panel         pnlForm;
        private Label         lblFormTitle;

        private Guna2ComboBox       fGuestSearch;
        private Label               lblGuestTag;
        private Guna2TextBox        fName, fMobile, fNationality, fIdProof, fAddress, fPrice;
        private Guna2ComboBox       fGender, fBed, fRoomType, fRoomNo;
        private Guna2DateTimePicker fDob, fCheckin;

        public UC_CustomerRes()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.BackColor = Color.White;

            // ── Header ────────────────────────────────────────────────
            var pnlHeader = new Panel {
                Dock = DockStyle.Top, Height = 72, BackColor = Color.White,
                Padding = new Padding(24, 0, 24, 0)
            };
            var lblTitle = new Label {
                Text = "Danh Sách Đặt Phòng",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true, Left = 0, Top = 18
            };
            lblCount = new Label {
                Font = new Font("Segoe UI", 9F), ForeColor = Color.Gray,
                AutoSize = true, Left = 0, Top = 50
            };
            txtSearch = new Guna2TextBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F), PlaceholderText = "Tìm kiếm...",
                Width = 220, Height = 38, Top = 17, Cursor = Cursors.IBeam
            };
            var btnAdd = new Guna2Button {
                BorderRadius = 8, FillColor = Color.SlateBlue, ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Text = "+  Đặt Phòng", Width = 200, Height = 45, Top = 13,
                Cursor = Cursors.Hand
            };
            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblCount, txtSearch, btnAdd });

            Action layoutHeader = () => {
                if (pnlHeader.Width < 200) return;
                btnAdd.Left    = pnlHeader.Width - btnAdd.Width - 24;
                txtSearch.Left = btnAdd.Left - txtSearch.Width - 12;
            };
            pnlHeader.Resize        += (s, e) => layoutHeader();
            pnlHeader.HandleCreated += (s, e) => layoutHeader();

            // ── Separator ─────────────────────────────────────────────
            var sep = new Panel {
                Dock = DockStyle.Top, Height = 1,
                BackColor = Color.FromArgb(220, 220, 220)
            };

            // ── Grid ──────────────────────────────────────────────────
            var pnlGrid = new Panel {
                Dock = DockStyle.Fill, BackColor = Color.White,
                Padding = new Padding(20, 16, 20, 16)
            };
            dgvCustomers = new DataGridView {
                Dock = DockStyle.Fill, BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None, RowHeadersVisible = false,
                AllowUserToAddRows = false, AllowUserToResizeRows = false,
                ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 10F), GridColor = Color.FromArgb(230, 230, 230),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustomers.ColumnHeadersHeight = 42;
            dgvCustomers.RowTemplate.Height  = 40;

            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colBid",     HeaderText = "Mã ĐP",         FillWeight = 55  });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStt",     HeaderText = "STT",           FillWeight = 40  });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName",    HeaderText = "Họ Tên",        FillWeight = 160 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPhone",   HeaderText = "SĐT",           FillWeight = 100 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colNation",  HeaderText = "Quốc Tịch",    FillWeight = 100 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRoom",    HeaderText = "Số Phòng",      FillWeight = 75  });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCheckin", HeaderText = "Ngày Check-in", FillWeight = 110 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus",  HeaderText = "Trạng Thái",    FillWeight = 100 });
            dgvCustomers.Columns.Add(new DataGridViewButtonColumn  { Name = "colEdit",    HeaderText = "", Text = "✏",  UseColumnTextForButtonValue = true, FillWeight = 38 });
            dgvCustomers.Columns.Add(new DataGridViewButtonColumn  { Name = "colDel",     HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 38 });

            dgvCustomers.CellClick += DgvCustomers_CellClick;
            pnlGrid.Controls.Add(dgvCustomers);

            // ── Overlay form ──────────────────────────────────────────
            BuildFormPanel();

            // ── Wire root events ──────────────────────────────────────
            btnAdd.Click          += (s, e) => ShowForm(-1);
            txtSearch.TextChanged += (s, e) => LoadData();
            this.SizeChanged      += (s, e) => CenterFormPanel();
            this.HandleCreated    += (s, e) => { LoadData(); CenterFormPanel(); };

            // Fill first → DockTop inner→outer → overlay last
            this.Controls.Add(pnlGrid);
            this.Controls.Add(sep);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlForm);
        }

        private void BuildFormPanel()
        {
            const int formW = 860, lx = 20, col2x = 460, colW = 380, inputH = 40;
            const int startY = 130, l2i = 25, fGap = 82, btnGap = 33;
            int lastBot = startY + l2i + 5 * fGap + inputH; // 605
            int btnY    = lastBot + btnGap;                  // 638
            int formH   = btnY + 45 + 25;                   // 708

            pnlForm = new Panel {
                Width = formW, Height = formH, BackColor = Color.White,
                Visible = false, BorderStyle = BorderStyle.FixedSingle
            };

            lblFormTitle = new Label {
                Text = "Đặt Phòng Mới",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.SlateBlue, AutoSize = true, Left = lx, Top = 20
            };
            pnlForm.Controls.Add(lblFormTitle);

            // ── Guest search section (only for new bookings) ─────────
            AddLbl(pnlForm, lx, 54, "Tìm khách đã có (để điền tự động):");
            fGuestSearch = new Guna2ComboBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDown,
                Left = lx, Top = 73, Width = 550, Height = inputH
            };

            lblGuestTag = new Label {
                Text = "", AutoSize = true,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.White, BackColor = Color.MediumSeaGreen,
                Left = 580, Top = 84, Padding = new Padding(6, 3, 6, 3),
                Visible = false
            };

            var btnClearGuest = new Guna2Button {
                Text = "Xóa chọn", Left = 580, Top = 73, Width = 90, Height = inputH,
                BorderRadius = 8, FillColor = Color.FromArgb(200, 200, 200), ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F), Cursor = Cursors.Hand
            };
            btnClearGuest.Click += (s, e) => ClearGuestSelection();

            var sepGuest = new Panel {
                Left = lx, Top = 122, Width = formW - 40, Height = 1,
                BackColor = Color.FromArgb(220, 220, 220)
            };

            pnlForm.Controls.AddRange(new Control[] { fGuestSearch, lblGuestTag, btnClearGuest, sepGuest });

            fGuestSearch.TextChanged          += GuestSearch_TextChanged;
            fGuestSearch.SelectedIndexChanged += GuestSearch_Selected;
            fGuestSearch.Click                += (s, e) => LoadGuestList(_selectedGuestId > 0 ? "" : fGuestSearch.Text.Trim());

            // ── Left column ───────────────────────────────────────────
            AddLbl(pnlForm, lx, startY + 0 * fGap, "Họ Tên");
            fName = G2Txt(lx, startY + 0 * fGap + l2i, colW, inputH, "Nhập họ tên...");

            AddLbl(pnlForm, lx, startY + 1 * fGap, "SĐT");
            fMobile = G2Txt(lx, startY + 1 * fGap + l2i, colW, inputH, "Nhập số điện thoại...");

            AddLbl(pnlForm, lx, startY + 2 * fGap, "Quốc Tịch");
            fNationality = G2Txt(lx, startY + 2 * fGap + l2i, colW, inputH, "Nhập quốc tịch...");

            AddLbl(pnlForm, lx, startY + 3 * fGap, "Giới Tính");
            fGender = G2Combo(lx, startY + 3 * fGap + l2i, colW, inputH, new[] { "Nam", "Nữ", "Khác" });

            AddLbl(pnlForm, lx, startY + 4 * fGap, "Ngày Sinh");
            fDob = new Guna2DateTimePicker {
                FillColor = Color.WhiteSmoke, Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short, Left = lx,
                Top = startY + 4 * fGap + l2i, Width = colW, Height = inputH,
                Value = DateTime.Now.AddYears(-20)
            };

            AddLbl(pnlForm, lx, startY + 5 * fGap, "CMND / CCCD");
            fIdProof = G2Txt(lx, startY + 5 * fGap + l2i, colW, inputH, "Nhập số CMND/CCCD...");

            // ── Right column ──────────────────────────────────────────
            AddLbl(pnlForm, col2x, startY + 0 * fGap, "Địa Chỉ");
            fAddress = G2Txt(col2x, startY + 0 * fGap + l2i, colW, inputH, "Nhập địa chỉ...");

            AddLbl(pnlForm, col2x, startY + 1 * fGap, "Ngày Check-in");
            fCheckin = new Guna2DateTimePicker {
                FillColor = Color.WhiteSmoke, Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short, Left = col2x,
                Top = startY + 1 * fGap + l2i, Width = colW, Height = inputH,
                Value = DateTime.Now
            };

            AddLbl(pnlForm, col2x, startY + 2 * fGap, "Loại Giường");
            fBed = G2Combo(col2x, startY + 2 * fGap + l2i, colW, inputH, new[] { "Single", "Double", "Triple" });

            AddLbl(pnlForm, col2x, startY + 3 * fGap, "Loại Phòng");
            fRoomType = G2Combo(col2x, startY + 3 * fGap + l2i, colW, inputH, new[] { "Ac", "Non-Ac" });

            AddLbl(pnlForm, col2x, startY + 4 * fGap, "Số Phòng");
            fRoomNo = G2Combo(col2x, startY + 4 * fGap + l2i, colW, inputH, new string[0]);

            AddLbl(pnlForm, col2x, startY + 5 * fGap, "Giá Tiền (VND)");
            fPrice = G2Txt(col2x, startY + 5 * fGap + l2i, colW, inputH, "");
            fPrice.ReadOnly = true;

            // ── Buttons ───────────────────────────────────────────────
            var btnSave = new Guna2Button {
                Text = "Lưu", Left = lx, Top = btnY, Width = 160, Height = 45,
                BorderRadius = 8, FillColor = Color.SlateBlue, ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand
            };
            var btnCancel = new Guna2Button {
                Text = "Hủy", Left = lx + 180, Top = btnY, Width = 160, Height = 45,
                BorderRadius = 8, FillColor = Color.FromArgb(180, 180, 180), ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand
            };

            btnSave.Click   += BtnSave_Click;
            btnCancel.Click += (s, e) => pnlForm.Visible = false;

            fBed.SelectedIndexChanged      += FBed_SelectedIndexChanged;
            fRoomType.SelectedIndexChanged += FRoomType_SelectedIndexChanged;
            fRoomNo.SelectedIndexChanged   += FRoomNo_SelectedIndexChanged;

            pnlForm.Controls.AddRange(new Control[] {
                fName, fMobile, fNationality, fGender, fDob, fIdProof,
                fAddress, fCheckin, fBed, fRoomType, fRoomNo, fPrice,
                btnSave, btnCancel
            });
        }

        // ── Guest search ──────────────────────────────────────────────

        private void LoadGuestList(string kw, bool openDropDown = false)
        {
            try
            {
                string where = string.IsNullOrEmpty(kw)
                    ? ""
                    : " WHERE cname LIKE N'%" + kw.Replace("'", "''") + "%' " +
                      "OR mobile LIKE '%" + kw.Replace("'", "''") + "%'";
                var ds = fn.GetData("SELECT gid, cname, mobile FROM guests" + where + " ORDER BY cname");
                string current = fGuestSearch.Text;
                fGuestSearch.TextChanged -= GuestSearch_TextChanged;
                fGuestSearch.Items.Clear();
                foreach (DataRow r in ds.Tables[0].Rows)
                    fGuestSearch.Items.Add(r["cname"] + " — " + r["mobile"] + " [" + r["gid"] + "]");
                fGuestSearch.Text = current;
                fGuestSearch.TextChanged += GuestSearch_TextChanged;
                if (openDropDown && fGuestSearch.Items.Count > 0 && !fGuestSearch.DroppedDown)
                    fGuestSearch.DroppedDown = true;
            }
            catch { }
        }

        private void GuestSearch_TextChanged(object sender, EventArgs e)
        {
            // Skip if text changed because user selected an item (not because they typed)
            if (fGuestSearch.SelectedItem != null && fGuestSearch.SelectedItem.ToString() == fGuestSearch.Text)
                return;
            _selectedGuestId = -1;
            LoadGuestList(fGuestSearch.Text.Trim(), openDropDown: false);
        }

        private void GuestSearch_Selected(object sender, EventArgs e)
        {
            string item = fGuestSearch.SelectedItem?.ToString();
            if (item == null) return;
            // Extract gid from "[gid]" at end of string
            int start = item.LastIndexOf('[') + 1;
            int end   = item.LastIndexOf(']');
            if (start <= 0 || end <= start) return;
            if (!int.TryParse(item.Substring(start, end - start), out int gid)) return;
            FillGuestInfo(gid);
        }

        private void FillGuestInfo(int gid)
        {
            try
            {
                var ds = fn.GetData("SELECT * FROM guests WHERE gid=" + gid);
                if (ds.Tables[0].Rows.Count == 0) return;
                var r = ds.Tables[0].Rows[0];

                _selectedGuestId = gid;
                fName.Text        = r["cname"].ToString();
                fMobile.Text      = r["mobile"]?.ToString() ?? "";
                fNationality.Text = r["nationality"].ToString();
                fIdProof.Text     = r["idproof"].ToString();
                fAddress.Text     = r["address"].ToString();
                if (DateTime.TryParse(r["dob"].ToString(), out var dob)) fDob.Value = dob;
                fGender.SelectedItem = GenderToDisplay(r["gender"].ToString());

                lblGuestTag.Text    = "Khách cũ: " + r["cname"];
                lblGuestTag.Visible = true;
            }
            catch { }
        }

        private void ClearGuestSelection()
        {
            _selectedGuestId    = -1;
            fGuestSearch.Text   = "";
            fGuestSearch.Items.Clear();
            lblGuestTag.Visible = false;
            fName.Text = ""; fMobile.Text = ""; fNationality.Text = "";
            fIdProof.Text = ""; fAddress.Text = "";
            fGender.SelectedIndex = -1;
            fDob.Value = DateTime.Now.AddYears(-20);
        }

        // ── Helpers ───────────────────────────────────────────────────

        private static void AddLbl(Panel parent, int x, int y, string text)
        {
            parent.Controls.Add(new Label {
                Text = text, AutoSize = true, Left = x, Top = y,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80)
            });
        }

        private static Guna2TextBox G2Txt(int x, int y, int w, int h, string ph) =>
            new Guna2TextBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                PlaceholderText = ph, Left = x, Top = y, Width = w, Height = h
            };

        private static Guna2ComboBox G2Combo(int x, int y, int w, int h, string[] items)
        {
            var cb = new Guna2ComboBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Left = x, Top = y, Width = w, Height = h
            };
            if (items.Length > 0) cb.Items.AddRange(items);
            return cb;
        }

        private static int ParseBid(string maDhon) =>
            int.Parse(maDhon.Replace("DP", "").TrimStart('0').PadLeft(1, '0'));

        private static string GenderToDisplay(string s)
        {
            if (s == "Nu")    return "Nữ";
            if (s == "Khac")  return "Khác";
            if (s == "Other") return "Khác";
            return s;
        }

        private static string GenderToStore(string s)
        {
            if (s == "Nữ")   return "Nu";
            if (s == "Khác") return "Khac";
            return s;
        }

        private void CenterFormPanel()
        {
            if (pnlForm == null) return;
            pnlForm.Left = (Width  - pnlForm.Width)  / 2;
            pnlForm.Top  = (Height - pnlForm.Height) / 2;
        }

        public void Reload() => LoadData();

        // ── Data ──────────────────────────────────────────────────────

        private void LoadData()
        {
            string kw  = txtSearch?.Text.Trim() ?? "";
            string sql = "SELECT b.bid, g.cname, g.mobile, g.nationality, r.roomNo, b.checkin, b.chekout " +
                         "FROM bookings b INNER JOIN guests g ON b.gid=g.gid " +
                         "INNER JOIN rooms r ON b.roomid=r.roomid";
            if (!string.IsNullOrEmpty(kw))
                sql += " WHERE g.cname LIKE N'%" + kw.Replace("'","''") + "%'" +
                       " OR g.mobile LIKE N'%" + kw.Replace("'","''") + "%'" +
                       " OR r.roomNo LIKE '%" + kw.Replace("'","''") + "%'";
            sql += " ORDER BY b.bid DESC";

            dgvCustomers.Rows.Clear();
            try
            {
                var ds = fn.GetData(sql);
                int stt = 1;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    string status = r["chekout"].ToString() == "YES" ? "Đã trả phòng" : "Đang ở";
                    int bid = Convert.ToInt32(r["bid"]);
                    string maDhon = "DP" + bid.ToString().PadLeft(3, '0');
                    dgvCustomers.Rows.Add(maDhon, stt++, r["cname"],
                        r["mobile"], r["nationality"], r["roomNo"], r["checkin"], status);
                }
                lblCount.Text = "Tổng: " + ds.Tables[0].Rows.Count + " lượt đặt phòng";
            }
            catch { }
        }

        private void DgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == dgvCustomers.Columns["colEdit"].Index)
            {
                int bid = ParseBid(dgvCustomers.Rows[e.RowIndex].Cells["colBid"].Value.ToString());
                ShowForm(bid);
            }
            else if (e.ColumnIndex == dgvCustomers.Columns["colDel"].Index)
            {
                int bid = ParseBid(dgvCustomers.Rows[e.RowIndex].Cells["colBid"].Value.ToString());
                DeleteBooking(bid, e.RowIndex);
            }
        }

        // ── Form open ─────────────────────────────────────────────────

        private void ShowForm(int bid)
        {
            _editingBid     = bid;
            _editingGuestId = -1;
            _editingRoomId  = -1;
            _selectedRoomId = -1;
            _selectedGuestId = -1;

            // Reset all fields
            fName.Text = ""; fMobile.Text = ""; fNationality.Text = "";
            fIdProof.Text = ""; fAddress.Text = ""; fPrice.Text = "";
            fGender.SelectedIndex   = -1;
            fBed.SelectedIndex      = -1;
            fRoomType.SelectedIndex = -1;
            fRoomNo.Items.Clear();
            fDob.Value     = DateTime.Now.AddYears(-20);
            fCheckin.Value = DateTime.Now;
            fGuestSearch.Text = ""; fGuestSearch.Items.Clear();
            lblGuestTag.Visible = false;

            // Show/hide guest search section (only for new bookings)
            bool isNew = (bid == -1);
            fGuestSearch.Visible  = isNew;
            lblGuestTag.Visible   = false;
            foreach (Control c in pnlForm.Controls)
                if (c.Top == 54 && c is Label l && l.Text.Contains("Tìm khách"))
                    l.Visible = isNew;

            if (isNew)
            {
                lblFormTitle.Text = "Đặt Phòng Mới";
            }
            else
            {
                lblFormTitle.Text = "Sửa Thông Tin Đặt Phòng";
                string sql = "SELECT b.bid, b.gid, g.cname, g.mobile, g.nationality, g.gender," +
                             " g.dob, g.idproof, g.address, b.checkin, b.roomid, r.roomNo, r.bed, r.roomType " +
                             "FROM bookings b INNER JOIN guests g ON b.gid=g.gid " +
                             "INNER JOIN rooms r ON b.roomid=r.roomid " +
                             "WHERE b.bid=" + bid;
                var ds = fn.GetData(sql);
                if (ds.Tables[0].Rows.Count == 0) return;
                var r = ds.Tables[0].Rows[0];

                _editingGuestId = Convert.ToInt32(r["gid"]);
                fName.Text        = r["cname"].ToString();
                fMobile.Text      = r["mobile"]?.ToString() ?? "";
                fNationality.Text = r["nationality"].ToString();
                fIdProof.Text     = r["idproof"].ToString();
                fAddress.Text     = r["address"].ToString();

                if (DateTime.TryParse(r["dob"].ToString(),     out var dob)) fDob.Value     = dob;
                if (DateTime.TryParse(r["checkin"].ToString(), out var cin)) fCheckin.Value = cin;

                fGender.SelectedItem = GenderToDisplay(r["gender"].ToString());

                _editingRoomId  = Convert.ToInt32(r["roomid"]);
                _selectedRoomId = _editingRoomId;

                string bed      = r["bed"].ToString();
                string roomType = r["roomType"].ToString();
                string roomNo   = r["roomNo"].ToString();

                fBed.SelectedItem      = bed;
                fRoomType.SelectedItem = roomType;
                LoadRoomNumbers(bed, roomType, _editingRoomId);
                fRoomNo.SelectedItem   = roomNo;
            }

            CenterFormPanel();
            pnlForm.Visible = true;
            pnlForm.BringToFront();
        }

        // ── Room cascades ─────────────────────────────────────────────

        private void LoadRoomNumbers(string bed, string roomType, int editingRoomId = -1)
        {
            fRoomNo.Items.Clear();
            fPrice.Text     = "";
            _selectedRoomId = -1;
            if (string.IsNullOrEmpty(bed) || string.IsNullOrEmpty(roomType)) return;

            string sql = editingRoomId > 0
                ? "SELECT roomNo FROM rooms WHERE bed='" + bed + "' AND roomType='" + roomType +
                  "' AND (booked='NO' OR roomid=" + editingRoomId + ")"
                : "SELECT roomNo FROM rooms WHERE bed='" + bed + "' AND roomType='" + roomType +
                  "' AND booked='NO'";
            try
            {
                var ds = fn.GetData(sql);
                foreach (DataRow row in ds.Tables[0].Rows)
                    fRoomNo.Items.Add(row["roomNo"].ToString());
            }
            catch { }
        }

        private void FBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            fRoomType.SelectedIndex = -1;
            fRoomNo.Items.Clear();
            fPrice.Text     = "";
            _selectedRoomId = -1;
        }

        private void FRoomType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string bed = fBed.SelectedItem?.ToString();
            string rt  = fRoomType.SelectedItem?.ToString();
            if (bed == null || rt == null) { fRoomNo.Items.Clear(); fPrice.Text = ""; return; }
            int editId = (_editingBid > 0) ? _editingRoomId : -1;
            LoadRoomNumbers(bed, rt, editId);
        }

        private void FRoomNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string rn = fRoomNo.SelectedItem?.ToString();
            if (rn == null) return;
            try
            {
                var ds = fn.GetData("SELECT roomid, price FROM rooms WHERE roomNo='" + rn + "'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    fPrice.Text     = ds.Tables[0].Rows[0]["price"].ToString();
                    _selectedRoomId = Convert.ToInt32(ds.Tables[0].Rows[0]["roomid"]);
                }
            }
            catch { }
        }

        // ── Save ──────────────────────────────────────────────────────

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(fName.Text) || string.IsNullOrWhiteSpace(fMobile.Text)
                || string.IsNullOrWhiteSpace(fNationality.Text) || fGender.SelectedItem == null
                || string.IsNullOrWhiteSpace(fIdProof.Text)     || string.IsNullOrWhiteSpace(fAddress.Text)
                || fBed.SelectedItem == null || fRoomType.SelectedItem == null
                || fRoomNo.SelectedItem == null || _selectedRoomId < 1)
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string mobile = fMobile.Text.Trim();
            if (!System.Text.RegularExpressions.Regex.IsMatch(mobile, @"^\d{9,11}$"))
            {
                MessageBox.Show("SĐT không hợp lệ (9-11 chữ số).", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name   = fName.Text.Trim().Replace("'", "''");
            string nation = fNationality.Text.Trim().Replace("'", "''");
            string gender = GenderToStore(fGender.SelectedItem.ToString());
            string dob    = fDob.Value.ToString("yyyy-MM-dd");
            string idp    = fIdProof.Text.Trim().Replace("'", "''");
            string addr   = fAddress.Text.Trim().Replace("'", "''");
            string cin    = fCheckin.Value.ToString("yyyy-MM-dd");

            if (_editingBid == -1)
            {
                // ── New booking ──────────────────────────────────────
                int gid;
                if (_selectedGuestId > 0)
                {
                    // Existing guest — update personal info in case it changed
                    gid = _selectedGuestId;
                    fn.ExecNonQuery(
                        "UPDATE guests SET cname=N'" + name + "',mobile='" + mobile +
                        "',nationality=N'" + nation + "',gender='" + gender + "',dob='" + dob +
                        "',idproof='" + idp + "',address=N'" + addr + "' WHERE gid=" + gid);
                }
                else
                {
                    // New guest
                    fn.ExecNonQuery(
                        "INSERT INTO guests (cname,mobile,nationality,gender,dob,idproof,address) " +
                        "VALUES (N'" + name + "','" + mobile + "',N'" + nation + "','" + gender +
                        "','" + dob + "','" + idp + "',N'" + addr + "')");
                    var dsGid = fn.GetData("SELECT MAX(gid) AS g FROM guests");
                    gid = Convert.ToInt32(dsGid.Tables[0].Rows[0]["g"]);
                }
                fn.SetData(
                    "INSERT INTO bookings (gid,roomid,checkin,chekout) VALUES (" +
                    gid + "," + _selectedRoomId + ",'" + cin + "','NO'); " +
                    "UPDATE rooms SET booked='YES' WHERE roomid=" + _selectedRoomId,
                    "Đặt phòng thành công!");
            }
            else
            {
                // ── Edit booking ─────────────────────────────────────
                fn.ExecNonQuery(
                    "UPDATE guests SET cname=N'" + name + "',mobile='" + mobile +
                    "',nationality=N'" + nation + "',gender='" + gender + "',dob='" + dob +
                    "',idproof='" + idp + "',address=N'" + addr + "' WHERE gid=" + _editingGuestId);

                bool roomChanged = (_selectedRoomId != _editingRoomId);
                string sqlBooking = "UPDATE bookings SET roomid=" + _selectedRoomId +
                                    ",checkin='" + cin + "' WHERE bid=" + _editingBid;
                if (roomChanged)
                    sqlBooking += "; UPDATE rooms SET booked='NO' WHERE roomid=" + _editingRoomId +
                                  "; UPDATE rooms SET booked='YES' WHERE roomid=" + _selectedRoomId;
                fn.SetData(sqlBooking, "Cập nhật thành công!");
            }

            pnlForm.Visible = false;
            LoadData();
        }

        // ── Delete ────────────────────────────────────────────────────

        private void DeleteBooking(int bid, int rowIndex)
        {
            string roomNo = dgvCustomers.Rows[rowIndex].Cells["colRoom"].Value?.ToString()   ?? "";
            string status = dgvCustomers.Rows[rowIndex].Cells["colStatus"].Value?.ToString() ?? "";

            var confirm = MessageBox.Show(
                "Xóa lượt đặt phòng " + roomNo + "? Thông tin cá nhân khách vẫn được giữ lại.",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;

            var ds = fn.GetData("SELECT roomid FROM bookings WHERE bid=" + bid);
            if (ds.Tables[0].Rows.Count == 0) return;
            int roomid = Convert.ToInt32(ds.Tables[0].Rows[0]["roomid"]);

            fn.ExecNonQuery("DELETE FROM customer_services WHERE bid=" + bid);
            fn.ExecNonQuery("DELETE FROM invoices WHERE bid=" + bid);

            string sql = "DELETE FROM bookings WHERE bid=" + bid;
            if (status != "Đã trả phòng")
                sql += "; UPDATE rooms SET booked='NO' WHERE roomid=" + roomid;
            fn.SetData(sql, "Xóa thành công!");
            LoadData();
        }
    }
}
