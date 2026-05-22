using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
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
                GridColor = Color.FromArgb(210, 208, 240),
                EnableHeadersVisualStyles = false,
                CellBorderStyle = DataGridViewCellBorderStyle.Single,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };
            // Header styling — set directly to avoid override from shared StyleHeader
            dgvList.ColumnHeadersHeight = 46;
            dgvList.ColumnHeadersDefaultCellStyle.BackColor          = Color.FromArgb(100, 88, 255);
            dgvList.ColumnHeadersDefaultCellStyle.ForeColor          = Color.White;
            dgvList.ColumnHeadersDefaultCellStyle.Font               = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvList.ColumnHeadersDefaultCellStyle.Alignment          = DataGridViewContentAlignment.MiddleCenter;
            dgvList.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.FromArgb(100, 88, 255);
            dgvList.RowTemplate.Height         = 48;
            dgvList.DefaultCellStyle.Font      = new Font("Segoe UI", 10);
            dgvList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvList.DefaultCellStyle.Padding   = new Padding(4, 0, 4, 0);

            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCid",     Visible = false });
            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colChekout", Visible = false });

            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStt", HeaderText = "STT", Width = 55 });

            var colName = new DataGridViewTextBoxColumn
            {
                Name = "colName",
                HeaderText = "Tên Khách Hàng",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 160
            };
            colName.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            colName.DefaultCellStyle.Padding   = new Padding(12, 0, 0, 0);
            colName.DefaultCellStyle.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvList.Columns.Add(colName);

            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRoom",    HeaderText = "Số Phòng",      Width = 100 });
            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCheckin",  HeaderText = "Ngày Check-in", Width = 145 });
            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCheckout", HeaderText = "Ngày Check-out",Width = 145 });
            dgvList.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus",   HeaderText = "Trạng Thái",   Width = 130 });
            dgvList.Columns.Add(new DataGridViewButtonColumn  { Name = "colDetail",   HeaderText = "",
                Text = "👁  Xem", UseColumnTextForButtonValue = true, Width = 130 });

            dgvList.CellFormatting += (s, e) =>
            {
                if (e.RowIndex < 0) return;
                int detailIdx  = dgvList.Columns["colDetail"].Index;
                int statusIdx  = dgvList.Columns["colStatus"].Index;
                int chekoutIdx = dgvList.Columns["colChekout"].Index;

                if (e.ColumnIndex == detailIdx)
                {
                    e.CellStyle.BackColor          = Color.FromArgb(100, 88, 255);
                    e.CellStyle.ForeColor          = Color.White;
                    e.CellStyle.Font               = new Font("Segoe UI", 10, FontStyle.Bold);
                    e.CellStyle.SelectionBackColor = Color.FromArgb(75, 63, 200);
                    e.CellStyle.Alignment          = DataGridViewContentAlignment.MiddleCenter;
                }
                else if (e.ColumnIndex == statusIdx)
                {
                    string chekout = dgvList.Rows[e.RowIndex].Cells["colChekout"].Value?.ToString() ?? "";
                    if (chekout == "YES")
                    {
                        e.CellStyle.ForeColor  = Color.FromArgb(220, 53, 69);
                        e.CellStyle.Font       = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
                    else
                    {
                        e.CellStyle.ForeColor  = Color.FromArgb(40, 167, 69);
                        e.CellStyle.Font       = new Font("Segoe UI", 9, FontStyle.Bold);
                    }
                }
                else if (e.RowIndex % 2 == 1)
                    e.CellStyle.BackColor = Color.FromArgb(248, 247, 255);
            };
            dgvList.CellContentClick += DgvList_CellContentClick;

            card.Controls.Add(dgvList);
            // dgvList must be at index 0 so dock layout processes pnlTop (index 1) first,
            // which lets pnlTop claim the top 110px before dgvList fills the remainder.
            card.Controls.SetChildIndex(dgvList, 0);
        }

        private void LoadListData(string filter = "")
        {
            string sql = "SELECT b.bid AS cid, g.cname, r.roomNo, b.checkin, b.checkout, b.chekout" +
                         " FROM bookings b INNER JOIN guests g ON b.gid=g.gid INNER JOIN rooms r ON b.roomid=r.roomid";
            if (!string.IsNullOrEmpty(filter))
                sql += " WHERE g.cname LIKE N'%" + filter.Replace("'", "''") + "%'";
            sql += " ORDER BY g.cname";

            try
            {
                DataSet ds = fn.GetData(sql);
                dgvList.Rows.Clear();
                int stt = 1;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    string ci     = r["checkin"]  != DBNull.Value ? Convert.ToDateTime(r["checkin"]).ToString("dd/MM/yyyy")  : "—";
                    string co     = r["checkout"] != DBNull.Value ? Convert.ToDateTime(r["checkout"]).ToString("dd/MM/yyyy") : "—";
                    bool checkedOut = r["chekout"].ToString() == "YES";
                    string status = checkedOut ? "Đã trả phòng" : "Đang ở";
                    dgvList.Rows.Add(r["cid"], r["chekout"], stt++, r["cname"], r["roomNo"], ci, co, status, "👁 Xem");
                }
            }
            catch { }
        }

        private void DgvList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex == dgvList.Columns["colDetail"].Index)
            {
                if (int.TryParse(dgvList.Rows[e.RowIndex].Cells["colCid"].Value?.ToString(), out int bid))
                {
                    selectedCid = bid;
                    ShowDetailPanel(selectedCid);
                }
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
                Text = "Xuất PDF / In",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 88, 255),
                BackColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Size = new Size(160, 38),
                Cursor = Cursors.Hand,
                Top = 18
            };
            btnPrint.FlatAppearance.BorderSize = 0;
            btnPrint.FlatAppearance.MouseOverBackColor = Color.FromArgb(243, 241, 255);
            btnPrint.FlatAppearance.MouseDownBackColor = Color.FromArgb(226, 220, 255);
            ApplyRoundedStyle(btnPrint, Color.FromArgb(100, 88, 255), 10);
            btnPrint.Click += BtnPrint_Click;
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
            Action repoSvcTotal = () => { if (cardSvc.Width > 0 && lblServiceTotal.Width > 0) lblServiceTotal.Left = cardSvc.Width - lblServiceTotal.Width - 20; };
            cardSvc.Resize            += (s, e) => repoSvcTotal();
            lblServiceTotal.SizeChanged += (s, e) => repoSvcTotal();

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

            // Left-column value labels (x=170 to clear longest key at high DPI)
            lblCNameVal        = KeyVal(panel, 170, ys[0]);
            lblGenderVal       = KeyVal(panel, 170, ys[1]);
            lblDobVal          = KeyVal(panel, 170, ys[2]);
            lblMobileVal       = KeyVal(panel, 170, ys[3]);

            // Right-column key labels — start hidden to prevent flash at x=0 before layout
            var rk0 = AddKey(panel, 0, ys[0], rightKeys[0]); rk0.Visible = false;
            var rk1 = AddKey(panel, 0, ys[1], rightKeys[1]); rk1.Visible = false;
            var rk2 = AddKey(panel, 0, ys[2], rightKeys[2]); rk2.Visible = false;
            var rk3 = AddKey(panel, 0, ys[3], rightKeys[3]); rk3.Visible = false;

            // Right-column value labels — also hidden until layout runs
            lblNationalityVal = KeyVal(panel, 0, ys[0]); lblNationalityVal.Visible = false;
            lblIdTypeVal      = KeyVal(panel, 0, ys[1]); lblIdTypeVal.Visible      = false;
            lblIdNoVal        = KeyVal(panel, 0, ys[2]); lblIdNoVal.Visible        = false;
            lblAddressVal     = KeyVal(panel, 0, ys[3]); lblAddressVal.Visible     = false;

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
                rk0.Visible = rk1.Visible = rk2.Visible = rk3.Visible = true;
                rv0.Visible = rv1.Visible = rv2.Visible = rv3.Visible = true;
            };
            panel.Resize        += (s, e) => layout();
            panel.HandleCreated += (s, e) => layout();
        }

        private void BuildRoomInfoFields(Panel panel)
        {
            int[] ys = { 50, 90, 130, 170 };
            string[] leftKeys  = { "Số phòng:",     "Loại phòng:",    "Giá phòng/đêm:", "Ngày check-in:" };
            string[] rightKeys = { "Ngày check-out:", "Số đêm:",       "Nhân viên lập:", "Tình trạng:" };

            foreach (var item in new[] { (0, leftKeys[0]), (1, leftKeys[1]), (2, leftKeys[2]), (3, leftKeys[3]) })
                AddKey(panel, 15, ys[item.Item1], item.Item2);

            lblRoomNoVal   = KeyVal(panel, 170, ys[0]);
            lblRoomTypeVal = KeyVal(panel, 170, ys[1]);
            lblPriceVal    = KeyVal(panel, 170, ys[2]);
            lblCheckinVal  = KeyVal(panel, 170, ys[3]);

            var rk0 = AddKey(panel, 0, ys[0], rightKeys[0]); rk0.Visible = false;
            var rk1 = AddKey(panel, 0, ys[1], rightKeys[1]); rk1.Visible = false;
            var rk2 = AddKey(panel, 0, ys[2], rightKeys[2]); rk2.Visible = false;
            var rk3 = AddKey(panel, 0, ys[3], rightKeys[3]); rk3.Visible = false;

            lblCheckoutVal = KeyVal(panel, 0, ys[0]); lblCheckoutVal.Visible = false;
            lblNightsVal   = KeyVal(panel, 0, ys[1]); lblNightsVal.Visible   = false;
            lblStaffVal    = KeyVal(panel, 0, ys[2]); lblStaffVal.Visible    = false;

            lblStatusVal = new Label
            {
                Text = "  Đang thuê  ",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.MediumSeaGreen,
                AutoSize = true,
                Location = new Point(0, ys[3]),
                Padding = new Padding(4, 2, 4, 2),
                Visible = false
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
                rk0.Visible = rk1.Visible = rk2.Visible = rk3.Visible = true;
                rv0.Visible = rv1.Visible = rv2.Visible = rv3.Visible = true;
            };
            panel.Resize        += (s, e) => layout();
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
                " r.roomNo, r.roomType, r.price," +
                " (SELECT TOP 1 e.ename FROM invoices inv INNER JOIN employee e ON e.eid = inv.eid WHERE inv.bid = b.bid) AS staffName" +
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
                lblStaffVal.Text       = r["staffName"] != DBNull.Value ? r["staffName"].ToString() : "—";

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
                        "; UPDATE rooms SET status=N'Trống' WHERE roomid = " + roomId,
                        "Đã xóa lượt đặt phòng thành công!");
                }
                ShowListPanel();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Rounded button styling ────────────────────────────────────────────

        private void ApplyRoundedStyle(Button btn, Color accentColor, int radius)
        {
            Action updateRegion = () =>
            {
                if (btn.Width <= 0 || btn.Height <= 0) return;
                using (var p = GetRoundedPath(new Rectangle(0, 0, btn.Width, btn.Height), radius))
                    btn.Region = new Region(p);
            };

            btn.Paint += (s, pe) =>
            {
                pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                var rect = new Rectangle(1, 1, btn.Width - 3, btn.Height - 3);
                using (var p = GetRoundedPath(rect, Math.Max(1, radius - 1)))
                using (var pen = new Pen(accentColor, 2f))
                    pe.Graphics.DrawPath(pen, p);
            };

            btn.Resize += (s, e) => updateRegion();
            if (btn.IsHandleCreated)
                updateRegion();
            else
                btn.HandleCreated += (s, e) => updateRegion();
        }

        private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
        {
            var path = new GraphicsPath();
            int d = Math.Max(1, radius * 2);
            path.AddArc(rect.X, rect.Y, d, d, 180, 90);
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
            path.CloseFigure();
            return path;
        }

        // ── Print / PDF ───────────────────────────────────────────────────────

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (selectedCid < 0) return;

            var doc = new PrintDocument();
            doc.DefaultPageSettings.Margins = new Margins(60, 60, 50, 50);
            doc.PrintPage += PrintInvoicePage;

            using (var dlg = new PrintDialog { Document = doc })
            {
                if (dlg.ShowDialog(this.FindForm()) == DialogResult.OK)
                    doc.Print();
            }
            doc.Dispose();
        }

        private void PrintInvoicePage(object sender, PrintPageEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float x = e.MarginBounds.Left;
            float y = e.MarginBounds.Top;
            float W = e.MarginBounds.Width;

            var purple   = Color.FromArgb(100, 88, 255);
            var darkText = Color.FromArgb(30,  30,  60);
            var grayText = Color.Gray;

            var fTitle   = new Font("Segoe UI", 20, FontStyle.Bold);
            var fSub     = new Font("Segoe UI",  9);
            var fBanner  = new Font("Segoe UI", 13, FontStyle.Bold);
            var fSection = new Font("Segoe UI",  9, FontStyle.Bold | FontStyle.Underline);
            var fKey     = new Font("Segoe UI",  9, FontStyle.Bold);
            var fVal     = new Font("Segoe UI",  9);
            var fSmall   = new Font("Segoe UI",  8);
            var fTotal   = new Font("Segoe UI", 12, FontStyle.Bold);
            var fGrand   = new Font("Segoe UI", 14, FontStyle.Bold);

            var sfC = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            var sfR = new StringFormat { Alignment = StringAlignment.Far,    LineAlignment = StringAlignment.Center };

            float rh = 20f;

            // ── Accent bar ────────────────────────────────────────────────────
            g.FillRectangle(new SolidBrush(purple), x, y, W, 4);
            y += 12;

            // ── Hotel header ──────────────────────────────────────────────────
            g.DrawString("KHÁCH SẠN QUẢN LÝ", fTitle, new SolidBrush(purple),
                new RectangleF(x, y, W, 34), sfC);
            y += 36;
            g.DrawString("123 Đường ABC, TP.HCM  |  Tel: 028 1234 5678  |  Email: hotel@ks.com",
                fSub, new SolidBrush(grayText), new RectangleF(x, y, W, 16), sfC);
            y += 24;

            // ── Title banner ──────────────────────────────────────────────────
            g.FillRectangle(new SolidBrush(purple), x, y, W, 28);
            g.DrawString("HÓA ĐƠN DỊCH VỤ KHÁCH SẠN", fBanner, Brushes.White,
                new RectangleF(x, y, W, 28), sfC);
            y += 36;
            g.DrawString("Ngày in: " + DateTime.Now.ToString("HH:mm  dd/MM/yyyy"),
                fSmall, new SolidBrush(grayText), new RectangleF(x, y, W, 14), sfR);
            y += 20;

            // ── Thông tin khách hàng ──────────────────────────────────────────
            y = PrintSectionTitle(g, "THÔNG TIN KHÁCH HÀNG", x, y, W, purple, fSection);
            float c1 = x, c2 = x + 110, c3 = x + W * 0.5f + 5, c4 = x + W * 0.5f + 120;
            y = PrintInfoRow(g, "Họ và tên:",     lblCNameVal.Text,     "Quốc tịch:",      lblNationalityVal.Text, c1, c2, c3, c4, y, rh, fKey, fVal, darkText, grayText);
            y = PrintInfoRow(g, "Giới tính:",     lblGenderVal.Text,    "Loại định danh:", lblIdTypeVal.Text,      c1, c2, c3, c4, y, rh, fKey, fVal, darkText, grayText);
            y = PrintInfoRow(g, "Ngày sinh:",     lblDobVal.Text,       "Số định danh:",   lblIdNoVal.Text,        c1, c2, c3, c4, y, rh, fKey, fVal, darkText, grayText);
            y = PrintInfoRow(g, "Số điện thoại:", lblMobileVal.Text,    "Địa chỉ:",        lblAddressVal.Text,     c1, c2, c3, c4, y, rh, fKey, fVal, darkText, grayText);
            y += 5;

            // ── Thông tin thuê phòng ──────────────────────────────────────────
            y = PrintSectionTitle(g, "THÔNG TIN THUÊ PHÒNG", x, y, W, purple, fSection);
            c2 = x + 120; c4 = x + W * 0.5f + 145;
            y = PrintInfoRow(g, "Số phòng:",      lblRoomNoVal.Text,   "Ngày check-out:", lblCheckoutVal.Text,         c1, c2, c3, c4, y, rh, fKey, fVal, darkText, grayText);
            y = PrintInfoRow(g, "Loại phòng:",    lblRoomTypeVal.Text, "Số đêm:",         lblNightsVal.Text,           c1, c2, c3, c4, y, rh, fKey, fVal, darkText, grayText);
            y = PrintInfoRow(g, "Giá phòng/đêm:", lblPriceVal.Text,    "Nhân viên:",      lblStaffVal.Text,            c1, c2, c3, c4, y, rh, fKey, fVal, darkText, grayText);
            y = PrintInfoRow(g, "Ngày check-in:", lblCheckinVal.Text,  "Tình trạng:",     lblStatusVal.Text.Trim(),    c1, c2, c3, c4, y, rh, fKey, fVal, darkText, grayText);
            y += 5;

            // ── Dịch vụ sử dụng ──────────────────────────────────────────────
            if (dgvServices.Rows.Count > 0)
            {
                y = PrintSectionTitle(g, "DỊCH VỤ SỬ DỤNG", x, y, W, purple, fSection);

                float[] cw = { 35, W * 0.28f, 110, 60, 110, 100 };
                string[] heads = { "STT", "Tên dịch vụ", "Đơn giá", "SL", "Thành tiền", "Ngày sử dụng" };

                g.FillRectangle(new SolidBrush(Color.FromArgb(235, 232, 255)), x, y, W, 22);
                float cx = x;
                for (int i = 0; i < heads.Length; i++)
                {
                    g.DrawString(heads[i], fKey, new SolidBrush(purple),
                        new RectangleF(cx + 2, y, cw[i] - 4, 22), sfC);
                    cx += cw[i];
                }
                y += 22;

                for (int row = 0; row < dgvServices.Rows.Count; row++)
                {
                    if (row % 2 == 1)
                        g.FillRectangle(new SolidBrush(Color.FromArgb(250, 249, 255)), x, y, W, 18);
                    cx = x;
                    for (int col = 0; col < Math.Min(dgvServices.Columns.Count, cw.Length); col++)
                    {
                        string v = dgvServices.Rows[row].Cells[col].Value?.ToString() ?? "";
                        g.DrawString(v, fSmall, new SolidBrush(darkText),
                            new RectangleF(cx + 2, y, cw[col] - 4, 18), sfC);
                        cx += cw[col];
                    }
                    y += 18;
                }

                g.DrawLine(new Pen(Color.FromArgb(200, 195, 255), 1f), x, y, x + W, y);
                y += 4;
                g.DrawString(lblServiceTotal.Text, fKey, new SolidBrush(purple),
                    new RectangleF(x, y, W, 18), sfR);
                y += 22;
            }

            // ── Tổng kết ──────────────────────────────────────────────────────
            y = PrintSectionTitle(g, "TỔNG KẾT", x, y, W, purple, fSection);

            if (int.TryParse(lblNightsVal.Text.Split(' ')[0].Trim(), out int nights) &&
                long.TryParse(lblPriceVal.Text.Replace(" VND", "").Replace(",", "").Trim(), out long pricePerNight))
            {
                long roomCost = pricePerNight * nights;
                g.DrawString("Tiền phòng (" + nights + " đêm × " + lblPriceVal.Text + "):",
                    fKey, new SolidBrush(grayText), x + 5, y);
                g.DrawString(string.Format("{0:N0} VND", roomCost), fVal,
                    new SolidBrush(darkText), new RectangleF(x, y, W, rh), sfR);
                y += rh;
            }

            string svcTotal = lblServiceTotal.Text.Replace("Tổng cộng dịch vụ:  ", "").Trim();
            g.DrawString("Tiền dịch vụ:", fKey, new SolidBrush(grayText), x + 5, y);
            g.DrawString(svcTotal, fVal, new SolidBrush(darkText),
                new RectangleF(x, y, W, rh), sfR);
            y += rh;

            g.DrawLine(new Pen(Color.FromArgb(200, 195, 255), 1f), x, y, x + W, y);
            y += 4;

            g.FillRectangle(new SolidBrush(Color.FromArgb(235, 232, 255)), x, y, W, 30);
            g.DrawString("TỔNG CỘNG:", fTotal, new SolidBrush(purple), x + 8, y + 4);
            string grandTotal = lblInvoiceTotal.Text.Replace("Tổng cộng:  ", "").Trim();
            g.DrawString(grandTotal, fGrand, new SolidBrush(purple),
                new RectangleF(x, y, W - 5, 30), sfR);
            y += 40;

            // ── Footer ────────────────────────────────────────────────────────
            g.DrawLine(new Pen(Color.FromArgb(200, 195, 255), 1f), x, y + 8, x + W, y + 8);
            y += 18;
            g.DrawString("Cảm ơn quý khách đã sử dụng dịch vụ tại khách sạn!", fSub,
                new SolidBrush(grayText), new RectangleF(x, y, W, 16), sfC);
            y += 30;

            float sig = W / 3f;
            g.DrawString("Khách hàng ký tên", fSmall, new SolidBrush(grayText),
                new RectangleF(x, y, sig, 14), sfC);
            g.DrawString("Nhân viên lập hóa đơn", fSmall, new SolidBrush(grayText),
                new RectangleF(x + W * 2f / 3f, y, sig, 14), sfC);

            foreach (var f in new Font[] { fTitle, fSub, fBanner, fSection, fKey, fVal, fSmall, fTotal, fGrand })
                f.Dispose();

            e.HasMorePages = false;
        }

        private float PrintSectionTitle(Graphics g, string title, float x, float y, float W,
            Color color, Font font)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(248, 246, 255)), x, y, W, 20);
            g.DrawLine(new Pen(color, 2f), x, y + 20, x + W, y + 20);
            g.DrawString(title, font, new SolidBrush(color), x + 4, y + 2);
            return y + 26;
        }

        private float PrintInfoRow(Graphics g,
            string k1, string v1, string k2, string v2,
            float c1, float c2, float c3, float c4,
            float y, float h, Font fKey, Font fVal,
            Color textColor, Color keyColor)
        {
            g.DrawString(k1, fKey, new SolidBrush(keyColor),  c1, y);
            g.DrawString(v1, fVal, new SolidBrush(textColor), c2, y);
            g.DrawString(k2, fKey, new SolidBrush(keyColor),  c3, y);
            g.DrawString(v2, fVal, new SolidBrush(textColor), c4, y);
            return y + h;
        }
    }
}
