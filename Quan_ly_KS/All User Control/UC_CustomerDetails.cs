using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_CustomerDetails : UserControl
    {
        private function fn = new function();
        private int selectedCid = -1;
        private bool _searchLock = false; // guard against re-entrant TextChanged

        // List panel
        private Panel pnlList;
        private ComboBox cmbSearch;
        private DataGridView dgvList;

        // Detail panel
        private Panel pnlDetail;

        // Customer info value labels
        private Label lblCNameVal, lblGenderVal, lblDobVal, lblMobileVal;
        private Label lblNationalityVal, lblIdTypeVal, lblIdNoVal, lblAddressVal;

        // Room info value labels
        private Label lblRoomNoVal, lblRoomTypeVal, lblPriceVal, lblCheckinVal;
        private Label lblCheckoutVal, lblNightsVal, lblStaffVal, lblStatusVal;

        // Services
        private DataGridView dgvServices;
        private Label lblServiceTotal;

        // Notes
        private TextBox txtNotes;

        // Invoices
        private DataGridView dgvInvoices;
        private Label lblInvoiceTotal;

        public UC_CustomerDetails()
        {
            InitializeComponent();
            BuildListPanel();
            BuildDetailPanel();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Reload();
        }

        public void Reload()
        {
            LoadSearchItems();
            LoadListData();
        }

        private void LoadSearchItems()
        {
            _searchLock = true;
            try
            {
                cmbSearch.Items.Clear();
                DataSet ds = fn.GetData("SELECT DISTINCT cname FROM guests ORDER BY cname");
                foreach (DataRow r in ds.Tables[0].Rows)
                    cmbSearch.Items.Add(r["cname"].ToString());
            }
            catch { }
            finally { _searchLock = false; }
        }

        // =====================================================================
        // LIST PANEL
        // =====================================================================

        private void BuildListPanel()
        {
            pnlList = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 246, 250),
                Padding = new Padding(30, 20, 30, 20)
            };
            this.Controls.Add(pnlList);

            var card = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };
            pnlList.Controls.Add(card);

            // Header row: title (left) + search (right)
            var pnlTop = new Panel { Dock = DockStyle.Top, Height = 110, BackColor = Color.White };
            card.Controls.Add(pnlTop);
            card.Controls.SetChildIndex(pnlTop, 0);

            pnlTop.Controls.Add(new Label
            {
                Text = "Thông Tin Chi Tiết Khách Hàng",
                Font = new Font("Century Gothic", 18, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                AutoSize = true,
                Location = new Point(20, 22)
            });

            // Search panel — anchored right
            var pnlSearch = new Panel { Size = new Size(370, 85), BackColor = Color.Transparent };
            pnlTop.Controls.Add(pnlSearch);
            // Keep right edge 20px from pnlTop right
            Action repoSearch = () => pnlSearch.Left = pnlTop.ClientSize.Width - pnlSearch.Width - 20;
            pnlTop.Resize += (s, e) => repoSearch();
            pnlSearch.Top = 14;
            pnlTop.HandleCreated += (s, e) => repoSearch();

            pnlSearch.Controls.Add(new Label
            {
                Text = "Tìm Kiếm",
                Font = new Font("Segoe UI", 10, FontStyle.Italic),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(0, 2)
            });

            cmbSearch = new ComboBox
            {
                Location = new Point(0, 28),
                Size = new Size(370, 32),
                Font = new Font("Segoe UI", 11),
                DropDownStyle = ComboBoxStyle.DropDown,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.Gray
            };
            cmbSearch.Text = "Chọn hoặc nhập tên khách hàng";
            cmbSearch.Enter += (s, e) =>
            {
                if (cmbSearch.Text == "Chọn hoặc nhập tên khách hàng")
                {
                    _searchLock = true;
                    cmbSearch.Text = "";
                    cmbSearch.ForeColor = Color.Black;
                    _searchLock = false;
                }
            };
            cmbSearch.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(cmbSearch.Text))
                {
                    _searchLock = true;
                    cmbSearch.Text = "Chọn hoặc nhập tên khách hàng";
                    cmbSearch.ForeColor = Color.Gray;
                    _searchLock = false;
                    LoadListData(); // khôi phục toàn bộ danh sách
                }
            };
            cmbSearch.TextChanged += (s, e) =>
            {
                if (_searchLock) return;
                string txt = cmbSearch.Text.Trim();
                if (txt != "Chọn hoặc nhập tên khách hàng")
                    LoadListData(txt);
            };
            pnlSearch.Controls.Add(cmbSearch);

            pnlTop.Controls.Add(new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 1,
                BackColor = Color.FromArgb(231, 229, 255)
            });

            // DataGridView fills rest of card
            dgvList = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                GridColor = Color.FromArgb(231, 229, 255),
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            StyleHeader(dgvList, 45, 55);
            dgvList.DefaultCellStyle.Font = new Font("Arial", 10);
            dgvList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCid", Visible = false });

            var colName = new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Tên Khách Hàng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            };
            colName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colName.DefaultCellStyle.Padding = new Padding(10, 0, 0, 0);
            colName.DefaultCellStyle.Font = new Font("Arial", 10);
            dgvList.Columns.Add(colName);

            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRoom",    HeaderText = "Số Phòng",       Width = 160 });
            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCheckin",  HeaderText = "Ngày Check-in",  Width = 200 });
            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCheckout", HeaderText = "Ngày Check-out", Width = 200 });
            dgvList.Columns.Add(new DataGridViewButtonColumn  { Name = "colDetail",   HeaderText = "Chi Tiết", Text = "Xem",
                UseColumnTextForButtonValue = true, Width = 120 });

            dgvList.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                if (e.ColumnIndex == dgvList.Columns["colDetail"].Index)
                {
                    e.CellStyle.BackColor = Color.FromArgb(100, 88, 255);
                    e.CellStyle.ForeColor = Color.White;
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
                else if (e.RowIndex % 2 == 1)
                    e.CellStyle.BackColor = Color.FromArgb(250, 249, 255);
            };
            dgvList.CellContentClick += DgvList_CellContentClick;

            card.Controls.Add(dgvList);
        }

        private void LoadListData(string filter = "")
        {
            string sql = "SELECT b.bid AS cid, g.cname, r.roomNo, b.checkin, b.checkout" +
                         " FROM bookings b INNER JOIN guests g ON b.gid=g.gid INNER JOIN rooms r ON b.roomid=r.roomid";
            if (!string.IsNullOrEmpty(filter))
                sql += " WHERE g.cname LIKE N'%" + filter.Replace("'", "''") + "%'";
            sql += " ORDER BY g.cname";

            try
            {
                DataSet ds = fn.GetData(sql);
                dgvList.Rows.Clear();
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    string ci = r["checkin"]  != DBNull.Value ? Convert.ToDateTime(r["checkin"]).ToString("dd/MM/yyyy")  : "—";
                    string co = r["checkout"] != DBNull.Value ? Convert.ToDateTime(r["checkout"]).ToString("dd/MM/yyyy") : "—";
                    dgvList.Rows.Add(r["cid"], r["cname"], r["roomNo"], ci, co, "Xem");
                }
            }
            catch { }
        }

        private void DgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == dgvList.Columns["colDetail"].Index)
            {
                selectedCid = Convert.ToInt32(dgvList.Rows[e.RowIndex].Cells["colCid"].Value);
                ShowDetailPanel(selectedCid);
            }
        }

        // =====================================================================
        // DETAIL PANEL
        // =====================================================================

        private void BuildDetailPanel()
        {
            pnlDetail = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(245, 246, 250), Visible = false };
            this.Controls.Add(pnlDetail);

            // ── Fixed header ─────────────────────────────────────────────────
            var pnlHeader = new Panel { Dock = DockStyle.Top, Height = 75, BackColor = Color.White };
            pnlDetail.Controls.Add(pnlHeader);

            var btnBackTop = new Button
            {
                Text = "← Chi Tiết Khách Hàng",
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                AutoSize = true,
                Location = new Point(30, 10),
                Cursor = Cursors.Hand
            };
            btnBackTop.FlatAppearance.BorderSize = 0;
            btnBackTop.Click += (s, e) => ShowListPanel();
            pnlHeader.Controls.Add(btnBackTop);

            pnlHeader.Controls.Add(new Label
            {
                Text = "Danh sách khách hàng  /  Chi tiết khách hàng",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(34, 50)
            });

            var btnPrint = new Button
            {
                Text = "In thông tin",
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(100, 88, 255),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(140, 36),
                Cursor = Cursors.Hand,
                Top = 20
            };
            btnPrint.FlatAppearance.BorderColor = Color.FromArgb(100, 88, 255);
            pnlHeader.Controls.Add(btnPrint);
            Action repoBtn = () => btnPrint.Left = pnlHeader.ClientSize.Width - btnPrint.Width - 20;
            pnlHeader.Resize += (s, e) => repoBtn();
            pnlHeader.HandleCreated += (s, e) => repoBtn();

            // ── Scrollable content ───────────────────────────────────────────
            var pnlScroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.FromArgb(245, 246, 250) };
            pnlDetail.Controls.Add(pnlScroll);
            pnlDetail.Controls.SetChildIndex(pnlHeader, 0);
            pnlDetail.Controls.SetChildIndex(pnlScroll, 1);

            // ── Customer + Room info (side by side via TableLayoutPanel) ─────
            var tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 255,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 15, 20, 0),
                Margin = new Padding(0)
            };
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));

            // Customer info card
            var pnlCust = CreateCard("Thông tin khách hàng");
            pnlCust.Dock = DockStyle.Fill;
            pnlCust.Margin = new Padding(0, 0, 6, 0);

            var btnEdit = new Button
            {
                Text = "Chỉnh sửa",
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.FromArgb(100, 88, 255),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(100, 28),
                Top = 10,
                Cursor = Cursors.Hand
            };
            btnEdit.FlatAppearance.BorderColor = Color.FromArgb(100, 88, 255);
            pnlCust.Controls.Add(btnEdit);
            Action repoEdit = () => btnEdit.Left = pnlCust.Width - btnEdit.Width - 10;
            pnlCust.Resize += (s, e) => repoEdit();
            pnlCust.HandleCreated += (s, e) => repoEdit();

            BuildCustInfoFields(pnlCust);

            // Room info card
            var pnlRoom = CreateCard("Thông tin thuê phòng");
            pnlRoom.Dock = DockStyle.Fill;
            pnlRoom.Margin = new Padding(6, 0, 0, 0);
            BuildRoomInfoFields(pnlRoom);

            tlp.Controls.Add(pnlCust, 0, 0);
            tlp.Controls.Add(pnlRoom, 1, 0);
            pnlScroll.Controls.Add(tlp);

            // ── Bottom section: Services (left 57%) + Invoices (right 43%) side by side ──
            var tlpBottom = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 445,
                ColumnCount = 2,
                RowCount = 1,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 0),
                Margin = new Padding(0)
            };
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 57));
            tlpBottom.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 43));
            tlpBottom.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            pnlScroll.Controls.Add(tlpBottom);

            // ── Left column: services card + notes card stacked ───────────────
            var pnlLeft = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
                Margin = new Padding(0, 0, 6, 0)
            };

            var cardSvc = CreateCard("Dịch vụ sử dụng");
            cardSvc.Dock = DockStyle.Top;
            cardSvc.Height = 295;

            dgvServices = MakeGrid();
            dgvServices.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvServices.Location = new Point(10, 44);
            dgvServices.Height = 190;
            AddGridCols(dgvServices, new[]
            {
                ("stt",      "STT",           55,  false),
                ("svcName",  "Tên dịch vụ",   0,   true),
                ("donGia",   "Đơn giá",       140, false),
                ("soLuong",  "Số lượng",       90, false),
                ("thanhTien","Thành tiền",    140, false),
                ("ngaySd",   "Ngày sử dụng", 150, false)
            });
            cardSvc.Controls.Add(dgvServices);
            cardSvc.Resize += (s, e) => dgvServices.Width = cardSvc.Width - 20;

            lblServiceTotal = new Label
            {
                Text = "Tổng cộng dịch vụ:  0 VND",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 88, 255),
                AutoSize = true,
                Location = new Point(10, 250)
            };
            cardSvc.Controls.Add(lblServiceTotal);
            cardSvc.Resize += (s, e) => { if (lblServiceTotal.Width > 0) lblServiceTotal.Left = cardSvc.Width - lblServiceTotal.Width - 20; };

            var spacerLeft = new Panel { Dock = DockStyle.Top, Height = 8, BackColor = Color.Transparent };

            var cardNotes = CreateCard("Ghi chú");
            cardNotes.Dock = DockStyle.Top;
            cardNotes.Height = 128;

            txtNotes = new TextBox
            {
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(10, 44),
                Height = 65,
                Multiline = true,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.FromArgb(252, 251, 255),
                ForeColor = Color.FromArgb(100, 100, 120)
            };
            cardNotes.Controls.Add(txtNotes);
            cardNotes.Resize += (s, e) => txtNotes.Width = cardNotes.Width - 20;

            // Add to pnlLeft — SetChildIndex ensures top-to-bottom order
            pnlLeft.Controls.Add(cardNotes);
            pnlLeft.Controls.Add(spacerLeft);
            pnlLeft.Controls.Add(cardSvc);
            pnlLeft.Controls.SetChildIndex(cardSvc,   0);
            pnlLeft.Controls.SetChildIndex(spacerLeft,1);
            pnlLeft.Controls.SetChildIndex(cardNotes, 2);

            // ── Right column: invoices card (fills full height) ───────────────
            var cardInv = CreateCard("Hóa đơn");
            cardInv.Dock = DockStyle.Fill;
            cardInv.Margin = new Padding(6, 0, 0, 0);

            dgvInvoices = MakeGrid();
            dgvInvoices.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            dgvInvoices.Location = new Point(10, 44);
            dgvInvoices.Height = 330;
            AddGridCols(dgvInvoices, new[]
            {
                ("hdId",     "Số hóa đơn",  0,   true),
                ("hdDate",   "Ngày lập",    120, false),
                ("hdTotal",  "Tổng tiền",   130, false),
                ("hdStatus", "Trạng thái",  130, false),
                ("hdAction", "Thao tác",     80, false)
            });
            cardInv.Controls.Add(dgvInvoices);

            lblInvoiceTotal = new Label
            {
                Text = "Tổng cộng:  0 VND",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                AutoSize = true,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            cardInv.Controls.Add(lblInvoiceTotal);

            Action resizeInv = () =>
            {
                dgvInvoices.Width = cardInv.Width - 20;
                if (lblInvoiceTotal.Width > 0)
                {
                    lblInvoiceTotal.Left = cardInv.Width - lblInvoiceTotal.Width - 20;
                    lblInvoiceTotal.Top  = cardInv.Height - lblInvoiceTotal.Height - 12;
                }
            };
            cardInv.Resize       += (s, e) => resizeInv();
            cardInv.HandleCreated += (s, e) => resizeInv();

            tlpBottom.Controls.Add(pnlLeft, 0, 0);
            tlpBottom.Controls.Add(cardInv, 1, 0);

            // ── Action buttons ────────────────────────────────────────────────
            var wrapAct = WrapPanel(75);
            pnlScroll.Controls.Add(wrapAct);

            var btnDel = new Button
            {
                Text = "Xóa khách hàng",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(220, 53, 69),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(185, 44),
                Location = new Point(20, 16),
                Cursor = Cursors.Hand
            };
            btnDel.FlatAppearance.BorderSize = 0;
            btnDel.Click += BtnDelete_Click;
            wrapAct.Controls.Add(btnDel);

            var btnBackList = new Button
            {
                Text = "Quay lại danh sách",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(100, 88, 255),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(210, 44),
                Location = new Point(215, 16),
                Cursor = Cursors.Hand
            };
            btnBackList.FlatAppearance.BorderSize = 0;
            btnBackList.Click += (s, e) => ShowListPanel();
            wrapAct.Controls.Add(btnBackList);

            // Stack Dock=Top panels from top to bottom
            pnlScroll.Controls.SetChildIndex(tlp,       0);
            pnlScroll.Controls.SetChildIndex(tlpBottom, 1);
            pnlScroll.Controls.SetChildIndex(wrapAct,   2);
        }

        // ── Build info fields ─────────────────────────────────────────────────

        private void BuildCustInfoFields(Panel panel)
        {
            int[] ys = { 50, 90, 130, 170 };
            string[] leftKeys  = { "Họ và tên:",     "Giới tính:", "Ngày sinh:",    "Số điện thoại:" };
            string[] rightKeys = { "Quốc tịch:",     "Loại định danh:", "Số định danh:", "Địa chỉ:" };

            // Left-column key labels (fixed at x=15)
            foreach (var item in new[] { (0, leftKeys[0]), (1, leftKeys[1]), (2, leftKeys[2]), (3, leftKeys[3]) })
                AddKey(panel, 15, ys[item.Item1], item.Item2);

            // Left-column value labels (fixed at x=130)
            lblCNameVal        = KeyVal(panel, 130, ys[0]);
            lblGenderVal       = KeyVal(panel, 130, ys[1]);
            lblDobVal          = KeyVal(panel, 130, ys[2]);
            lblMobileVal       = KeyVal(panel, 130, ys[3]);

            // Right-column key labels (x repositioned by SizeChanged)
            var rk0 = AddKey(panel, 0, ys[0], rightKeys[0]);
            var rk1 = AddKey(panel, 0, ys[1], rightKeys[1]);
            var rk2 = AddKey(panel, 0, ys[2], rightKeys[2]);
            var rk3 = AddKey(panel, 0, ys[3], rightKeys[3]);

            // Right-column value labels (x repositioned by SizeChanged)
            lblNationalityVal = KeyVal(panel, 0, ys[0]);
            lblIdTypeVal      = KeyVal(panel, 0, ys[1]);
            lblIdNoVal        = KeyVal(panel, 0, ys[2]);
            lblAddressVal     = KeyVal(panel, 0, ys[3]);

            var rv0 = lblNationalityVal; var rv1 = lblIdTypeVal;
            var rv2 = lblIdNoVal;        var rv3 = lblAddressVal;

            Action layout = () =>
            {
                if (panel.Width < 50) return;
                int hx = panel.Width / 2 + 5;
                rk0.Left = hx; rv0.Left = hx + 130;
                rk1.Left = hx; rv1.Left = hx + 130;
                rk2.Left = hx; rv2.Left = hx + 130;
                rk3.Left = hx; rv3.Left = hx + 130;
            };
            panel.Resize       += (s, e) => layout();
            panel.HandleCreated += (s, e) => layout();
        }

        private void BuildRoomInfoFields(Panel panel)
        {
            int[] ys = { 50, 90, 130, 170 };
            string[] leftKeys  = { "Số phòng:",     "Loại phòng:",    "Giá phòng/đêm:", "Ngày check-in:" };
            string[] rightKeys = { "Ngày check-out:", "Số đêm:",       "Nhân viên lập:", "Tình trạng:" };

            foreach (var item in new[] { (0, leftKeys[0]), (1, leftKeys[1]), (2, leftKeys[2]), (3, leftKeys[3]) })
                AddKey(panel, 15, ys[item.Item1], item.Item2);

            lblRoomNoVal   = KeyVal(panel, 135, ys[0]);
            lblRoomTypeVal = KeyVal(panel, 135, ys[1]);
            lblPriceVal    = KeyVal(panel, 135, ys[2]);
            lblCheckinVal  = KeyVal(panel, 135, ys[3]);

            var rk0 = AddKey(panel, 0, ys[0], rightKeys[0]);
            var rk1 = AddKey(panel, 0, ys[1], rightKeys[1]);
            var rk2 = AddKey(panel, 0, ys[2], rightKeys[2]);
            var rk3 = AddKey(panel, 0, ys[3], rightKeys[3]);

            lblCheckoutVal = KeyVal(panel, 0, ys[0]);
            lblNightsVal   = KeyVal(panel, 0, ys[1]);
            lblStaffVal    = KeyVal(panel, 0, ys[2]);

            lblStatusVal = new Label
            {
                Text = "  Đang thuê  ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.MediumSeaGreen,
                AutoSize = true,
                Location = new Point(0, ys[3]),
                Padding = new Padding(4, 2, 4, 2)
            };
            panel.Controls.Add(lblStatusVal);

            var rv0 = lblCheckoutVal; var rv1 = lblNightsVal;
            var rv2 = lblStaffVal;    var rv3 = lblStatusVal;

            Action layout = () =>
            {
                if (panel.Width < 50) return;
                int hx = panel.Width / 2 + 5;
                rk0.Left = hx; rv0.Left = hx + 145;
                rk1.Left = hx; rv1.Left = hx + 145;
                rk2.Left = hx; rv2.Left = hx + 145;
                rk3.Left = hx; rv3.Left = hx + 145;
            };
            panel.Resize       += (s, e) => layout();
            panel.HandleCreated += (s, e) => layout();
        }

        // ── Layout helpers ────────────────────────────────────────────────────

        private Panel WrapPanel(int height)
        {
            return new Panel
            {
                Dock = DockStyle.Top,
                Height = height,
                BackColor = Color.Transparent,
                Padding = new Padding(20, 10, 20, 0)
            };
        }

        private Panel CreateCard(string title)
        {
            var pnl = new Panel { BackColor = Color.White };

            pnl.Controls.Add(new Label
            {
                Text = "◆",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 88, 255),
                AutoSize = true,
                Location = new Point(10, 14)
            });
            pnl.Controls.Add(new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                AutoSize = true,
                Location = new Point(28, 14)
            });

            var sep = new Panel
            {
                Location = new Point(0, 40),
                Height = 1,
                BackColor = Color.FromArgb(231, 229, 255),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            pnl.Controls.Add(sep);
            pnl.Resize += (s, e) => sep.Width = pnl.Width;

            return pnl;
        }

        private DataGridView MakeGrid()
        {
            var dgv = new DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(231, 229, 255),
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };
            StyleHeader(dgv, 35, 38);
            dgv.DefaultCellStyle.Font = new Font("Arial", 9);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 191, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.Black;
            return dgv;
        }

        private void AddGridCols(DataGridView dgv, (string name, string header, int width, bool fill)[] cols)
        {
            foreach (var (name, header, width, fill) in cols)
            {
                if (fill)
                    dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = name, HeaderText = header, AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
                else
                    dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = name, HeaderText = header, Width = width });
            }
        }

        private void StyleHeader(DataGridView dgv, int headerH, int rowH)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = headerH;
            dgv.RowTemplate.Height = rowH;
        }

        private Label AddKey(Panel panel, int x, int y, string text)
        {
            var lbl = new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(x, y)
            };
            panel.Controls.Add(lbl);
            return lbl;
        }

        private Label KeyVal(Panel panel, int x, int y)
        {
            var lbl = new Label
            {
                Text = "—",
                Font = new Font("Arial", 9),
                ForeColor = Color.FromArgb(50, 50, 90),
                AutoSize = true,
                Location = new Point(x, y)
            };
            panel.Controls.Add(lbl);
            return lbl;
        }

        // ── Panel switching ───────────────────────────────────────────────────

        private void ShowDetailPanel(int cid)
        {
            pnlList.Visible = false;
            pnlDetail.Visible = true;
            LoadDetailData(cid);
        }

        private void ShowListPanel()
        {
            pnlDetail.Visible = false;
            pnlList.Visible = true;
            // Reset search và reload toàn bộ danh sách
            _searchLock = true;
            cmbSearch.Text = "Chọn hoặc nhập tên khách hàng";
            cmbSearch.ForeColor = Color.Gray;
            _searchLock = false;
            LoadListData();
        }

        // ── Load detail data ──────────────────────────────────────────────────

        private void LoadDetailData(int cid)
        {
            string sql =
                "SELECT b.bid, g.cname, g.gender, g.dob, g.mobile, g.nationality," +
                " g.idproof, g.address, b.checkin, b.checkout, b.chekout," +
                " r.roomNo, r.roomType, r.price" +
                " FROM bookings b INNER JOIN guests g ON b.gid=g.gid INNER JOIN rooms r ON b.roomid=r.roomid" +
                " WHERE b.bid = " + cid;
            try
            {
                DataSet ds = fn.GetData(sql);
                if (ds.Tables[0].Rows.Count == 0) return;
                DataRow r = ds.Tables[0].Rows[0];

                lblCNameVal.Text       = r["cname"].ToString();
                lblGenderVal.Text      = r["gender"].ToString();
                lblDobVal.Text         = r["dob"] != DBNull.Value ? Convert.ToDateTime(r["dob"]).ToString("dd/MM/yyyy") : "—";
                lblMobileVal.Text      = r["mobile"].ToString();
                lblNationalityVal.Text = r["nationality"].ToString();
                lblIdTypeVal.Text      = "CCCD";
                lblIdNoVal.Text        = r["idproof"].ToString();
                lblAddressVal.Text     = r["address"].ToString();

                lblRoomNoVal.Text      = r["roomNo"].ToString();
                lblRoomTypeVal.Text    = r["roomType"].ToString();
                lblPriceVal.Text       = string.Format("{0:N0} VND", r["price"]);
                lblCheckinVal.Text     = r["checkin"] != DBNull.Value ? Convert.ToDateTime(r["checkin"]).ToString("dd/MM/yyyy") : "—";
                lblStaffVal.Text       = "—";

                bool co = r["chekout"] != DBNull.Value && r["chekout"].ToString() == "YES";
                if (r["checkout"] != DBNull.Value)
                {
                    DateTime cout = Convert.ToDateTime(r["checkout"]);
                    lblCheckoutVal.Text = cout.ToString("dd/MM/yyyy");
                    if (r["checkin"] != DBNull.Value)
                        lblNightsVal.Text = (cout - Convert.ToDateTime(r["checkin"])).Days + " đêm";
                }
                else
                {
                    lblCheckoutVal.Text = "Chưa trả";
                    if (r["checkin"] != DBNull.Value)
                        lblNightsVal.Text = (DateTime.Today - Convert.ToDateTime(r["checkin"])).Days + " đêm";
                }

                lblStatusVal.Text      = co ? "  Đã trả phòng  " : "  Đang thuê  ";
                lblStatusVal.BackColor = co ? Color.FromArgb(220, 53, 69) : Color.MediumSeaGreen;
            }
            catch { }

            LoadServicesData(cid);
            LoadInvoicesData(cid);
            txtNotes.Text = "";
        }

        private void LoadServicesData(int cid)
        {
            dgvServices.Rows.Clear();
            long total = 0;
            try
            {
                DataSet ds = fn.GetData(
                    "SELECT s.serviceName, s.price, cs.quantity," +
                    " (s.price * cs.quantity) AS tt, cs.used_date" +
                    " FROM customer_services cs INNER JOIN services s ON cs.sid = s.sid" +
                    " WHERE cs.bid = " + cid + " ORDER BY cs.used_date");
                int stt = 1;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    long tt = Convert.ToInt64(r["tt"]);
                    total += tt;
                    string ud = r["used_date"] != DBNull.Value ? Convert.ToDateTime(r["used_date"]).ToString("dd/MM/yyyy") : "—";
                    dgvServices.Rows.Add(stt++, r["serviceName"],
                        string.Format("{0:N0} VND", r["price"]),
                        r["quantity"],
                        string.Format("{0:N0} VND", tt), ud);
                }
            }
            catch { }
            lblServiceTotal.Text = string.Format("Tổng cộng dịch vụ:  {0:N0} VND", total);
        }

        private void LoadInvoicesData(int cid)
        {
            dgvInvoices.Rows.Clear();
            long total = 0;
            try
            {
                DataSet ds = fn.GetData(
                    "SELECT invoiceNo, createdDate, totalAmount, status" +
                    " FROM invoices WHERE bid = " + cid + " ORDER BY createdDate");
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    long amt = Convert.ToInt64(r["totalAmount"]);
                    total += amt;
                    string dt = r["createdDate"] != DBNull.Value ? Convert.ToDateTime(r["createdDate"]).ToString("dd/MM/yyyy") : "—";
                    int idx = dgvInvoices.Rows.Add(r["invoiceNo"], dt,
                        string.Format("{0:N0} VND", amt), r["status"], "Xem");
                    StyleStatusCell(dgvInvoices.Rows[idx].Cells["hdStatus"], r["status"].ToString());
                }
            }
            catch { }
            lblInvoiceTotal.Text = string.Format("Tổng cộng:  {0:N0} VND", total);
        }

        private void StyleStatusCell(DataGridViewCell cell, string status)
        {
            cell.Style.ForeColor = Color.White;
            cell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            cell.Style.BackColor = status.Contains("thanh toán") ? Color.FromArgb(40, 167, 69)
                                 : status.Contains("Chưa")       ? Color.FromArgb(255, 140, 0)
                                 : Color.SlateGray;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (selectedCid < 0) return;
            if (MessageBox.Show("Bạn có chắc muốn xóa khách hàng này?", "Xác nhận xóa",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;
            try
            {
                DataSet ds = fn.GetData("SELECT roomid FROM bookings WHERE bid = " + selectedCid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int roomId = Convert.ToInt32(ds.Tables[0].Rows[0]["roomid"]);
                    fn.ExecNonQuery("DELETE FROM customer_services WHERE bid = " + selectedCid);
                    fn.ExecNonQuery("DELETE FROM invoices WHERE bid = " + selectedCid);
                    fn.SetData(
                        "DELETE FROM bookings WHERE bid = " + selectedCid +
                        "; UPDATE rooms SET booked='NO' WHERE roomid = " + roomId,
                        "Đã xóa lượt đặt phòng thành công!");
                }
                ShowListPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
