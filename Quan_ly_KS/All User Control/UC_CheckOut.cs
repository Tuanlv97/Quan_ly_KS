using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_CheckOut : UserControl
    {
        private readonly function fn = new function();
        private int cId;
        private long currentRoomPrice;
        private DateTime currentCheckinDate;
        private string currentRoomNo;

        // Panels
        private Panel pnlList;
        private Panel pnlDetail;
        private Panel pnlLeft;

        // List view
        private DataGridView dgvCustomers;
        private TextBox txtSearch;
        private Label lblListCount;

        // Detail view
        private DataGridView dgvBillDetail;
        private Label lblDVTotal;
        private Label lblPhongTotal;
        private Label lblInfoCust;
        private Label lblInfoRoom;
        private DateTimePicker dtpCheckOut;
        private Label lblRoomTotalValue;
        private Label lblSvcTotalValue;
        private Label lblVATValue;
        private Label lblGrandAmount;
        private long _grandTotal = 0;

        // Theme colors
        private static readonly Color C_PURPLE = Color.FromArgb(100, 88, 255);
        private static readonly Color C_DARK   = Color.FromArgb(50, 50, 90);
        private static readonly Color C_GRID   = Color.FromArgb(220, 218, 255);
        private static readonly Color C_ALTROW = Color.FromArgb(248, 247, 255);

        // Layout constants (UC is 1882 x 852)
        private const int PAD   = 18;
        private const int TOPBAR = 62;
        private const int LEFT_W = 1260;
        private const int GAP    = 12;
        // rightW = 1882 - 2*18 - 1260 - 12 = 574
        private const int RIGHT_W = 574;
        // contentH = 852 - TOPBAR - PAD = 772
        private const int CONTENT_H = 772;

        // ─────────────────────────────────────────────────────────────────────

        public UC_CheckOut()
        {
            InitializeComponent();
            HideDesignerControls();
            BuildListView();
            BuildDetailView();
            ShowListView();
        }

        private void HideDesignerControls()
        {
            label1.Visible = false; label2.Visible = false;
            label3.Visible = false; label4.Visible = false; label5.Visible = false;
            txtName.Visible = false; txtCName.Visible = false;
            txtRoom.Visible = false; txtCheckOutDate.Visible = false;
            btnCheckOut.Visible = false;
            guna2DataGridView1.Visible = false;
        }

        // ══ LIST VIEW ════════════════════════════════════════════════════════

        private void BuildListView()
        {
            pnlList = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            // ── Header (Dock Top) ─────────────────────────────────────────────
            var pnlHeader = new Panel {
                Dock = DockStyle.Top, Height = 88, BackColor = Color.White,
                Padding = new Padding(24, 0, 24, 0)
            };
            var lblTitle = new Label {
                Text = "Thanh Toán",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 50),
                AutoSize = true, Left = 0, Top = 14
            };
            lblListCount = new Label {
                Font = new Font("Segoe UI", 9.5F), ForeColor = Color.Gray,
                AutoSize = true, Left = 0, Top = 52
            };

            // Rounded pill search box
            const int SW = 240, SH = 40;
            var pnlSearch = new Panel { Width = SW, Height = SH, Top = 24, BackColor = Color.Transparent };
            pnlSearch.Paint += (s, pe) => {
                var g = pe.Graphics;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                var ctrl = (Control)s;
                float r = ctrl.Height / 2f;
                var rect = new RectangleF(0.5f, 0.5f, ctrl.Width - 1, ctrl.Height - 1);
                using (var path = new System.Drawing.Drawing2D.GraphicsPath())
                using (var fill   = new SolidBrush(Color.WhiteSmoke))
                using (var border = new Pen(Color.FromArgb(200, 200, 200), 1f))
                {
                    path.AddArc(rect.X,           rect.Y,            r*2, r*2, 180, 90);
                    path.AddArc(rect.Right - r*2, rect.Y,            r*2, r*2, 270, 90);
                    path.AddArc(rect.Right - r*2, rect.Bottom - r*2, r*2, r*2,   0, 90);
                    path.AddArc(rect.X,           rect.Bottom - r*2, r*2, r*2,  90, 90);
                    path.CloseFigure();
                    g.FillPath(fill, path);
                    g.DrawPath(border, path);
                }
            };
            txtSearch = new TextBox {
                Left = 14, Top = (SH - 22) / 2,
                Width = SW - 28, Height = 22,
                BorderStyle = BorderStyle.None,
                BackColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(160, 150, 190),
                Text = "Tìm kiếm..."
            };
            txtSearch.GotFocus  += (s, e) => { if (txtSearch.Text == "Tìm kiếm...") { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; } };
            txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "Tìm kiếm..."; txtSearch.ForeColor = Color.FromArgb(160, 150, 190); } };
            txtSearch.TextChanged += TxtSearch_TextChanged;
            pnlSearch.Controls.Add(txtSearch);

            pnlHeader.Controls.AddRange(new Control[] { lblTitle, lblListCount, pnlSearch });
            Action layoutHeader = () => {
                if (pnlHeader.Width < 200) return;
                pnlSearch.Left = pnlHeader.Width - pnlSearch.Width - 24;
            };
            pnlHeader.Resize        += (s, e) => layoutHeader();
            pnlHeader.HandleCreated += (s, e) => layoutHeader();

            // ── Separator ─────────────────────────────────────────────────────
            var sep = new Panel {
                Dock = DockStyle.Top, Height = 1,
                BackColor = Color.FromArgb(220, 220, 220)
            };

            // ── Grid panel (Dock Fill) ─────────────────────────────────────────
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
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                MultiSelect = false, ScrollBars = ScrollBars.Vertical
            };
            dgvCustomers.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvCustomers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvCustomers.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgvCustomers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvCustomers.ColumnHeadersHeight = 42;
            dgvCustomers.RowTemplate.Height  = 40;

            // Hidden columns
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCid",      Visible = false, FillWeight = 1 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPriceRaw", Visible = false, FillWeight = 1 });

            var cSTT = new DataGridViewTextBoxColumn { Name = "colSTT",   HeaderText = "STT",            FillWeight = 32,  MinimumWidth = 55  };
            cSTT.DefaultCellStyle.Alignment   = DataGridViewContentAlignment.MiddleCenter;
            var cName  = new DataGridViewTextBoxColumn { Name = "colName",  HeaderText = "Tên Khách Hàng", FillWeight = 250, MinimumWidth = 180 };
            var cPhone = new DataGridViewTextBoxColumn { Name = "colPhone", HeaderText = "Số Điện Thoại",  FillWeight = 130, MinimumWidth = 130 };
            cPhone.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var cCI    = new DataGridViewTextBoxColumn { Name = "colCI",    HeaderText = "Ngày Check-In",  FillWeight = 145, MinimumWidth = 140 };
            cCI.DefaultCellStyle.Alignment    = DataGridViewContentAlignment.MiddleCenter;
            var cRoom  = new DataGridViewTextBoxColumn { Name = "colRoom",  HeaderText = "Số Phòng",       FillWeight = 100, MinimumWidth = 100 };
            cRoom.DefaultCellStyle.Alignment  = DataGridViewContentAlignment.MiddleCenter;
            var cType  = new DataGridViewTextBoxColumn { Name = "colType",  HeaderText = "Loại Phòng",     FillWeight = 145, MinimumWidth = 120 };
            cType.DefaultCellStyle.Alignment  = DataGridViewContentAlignment.MiddleCenter;
            var cGia   = new DataGridViewTextBoxColumn { Name = "colGia",   HeaderText = "Giá / Đêm",      FillWeight = 140, MinimumWidth = 140 };
            cGia.DefaultCellStyle.Alignment   = DataGridViewContentAlignment.MiddleRight;
            var cXem = new DataGridViewButtonColumn { Name = "colXem", HeaderText = "", Text = "👁", UseColumnTextForButtonValue = true, FillWeight = 38 };

            dgvCustomers.Columns.AddRange(new DataGridViewColumn[] { cSTT, cName, cPhone, cCI, cRoom, cType, cGia, cXem });
            dgvCustomers.CellClick += DgvCustomers_CellClick;
            pnlGrid.Controls.Add(dgvCustomers);

            // Thứ tự Add: Fill trước, Top sau (innermost → outermost)
            pnlList.Controls.Add(pnlGrid);
            pnlList.Controls.Add(sep);
            pnlList.Controls.Add(pnlHeader);
            Controls.Add(pnlList);
        }

        // ══ DETAIL VIEW ══════════════════════════════════════════════════════

        private void BuildDetailView()
        {
            pnlDetail = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Visible = false };

            // ── Top bar ────────────────────────────────────────────────────────
            var pnlTopBar = new Panel { Dock = DockStyle.Top, Height = 54, BackColor = Color.White };
            var btnBack = new Button {
                Text = " Quay lại",
                Location = new Point(PAD, 9), Size = new Size(126, 34),
                BackColor = Color.FromArgb(234, 232, 255), ForeColor = C_PURPLE,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand,
                Image = CreateBackArrowIcon(16, C_PURPLE),
                ImageAlign = ContentAlignment.MiddleLeft,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Padding = new Padding(6, 0, 8, 0)
            };
            btnBack.FlatAppearance.BorderColor        = C_PURPLE;
            btnBack.FlatAppearance.BorderSize         = 1;
            btnBack.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 207, 255);
            btnBack.FlatAppearance.MouseDownBackColor = Color.FromArgb(190, 185, 255);
            btnBack.Click += (s, e) => ShowListView();
            var lblDetTitle = new Label {
                Text = "Thanh Toán", Location = new Point(162, 9), Size = new Size(300, 34),
                Font = new Font("Century Gothic", 14F, FontStyle.Bold), ForeColor = C_DARK
            };
            pnlTopBar.Controls.AddRange(new Control[] { btnBack, lblDetTitle });

            // ── Content ────────────────────────────────────────────────────────
            var pnlContent = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(PAD, 6, PAD, PAD) };

            // ── Right panel (compact summary, 306px) ───────────────────────────
            const int R_W = 306;
            int iw = R_W - 28;   // 278 usable

            var pnlRight = new Panel { Dock = DockStyle.Right, Width = R_W, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            var pnlHdr = new Panel { Location = new Point(0, 0), Size = new Size(R_W, 40), BackColor = C_PURPLE };
            pnlHdr.Controls.Add(new Label {
                Text = "Tóm Tắt Thanh Toán", Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.White, TextAlign = ContentAlignment.MiddleCenter
            });

            lblInfoCust = new Label { Text = "Khách hàng: —", Location = new Point(14, 47), Size = new Size(iw, 20), Font = new Font("Segoe UI", 9.5F), ForeColor = C_DARK };
            lblInfoRoom = new Label { Text = "Số phòng: —",   Location = new Point(14, 69), Size = new Size(iw, 20), Font = new Font("Segoe UI", 9.5F), ForeColor = C_DARK };

            var lblDateLbl = new Label { Text = "Ngày Thanh Toán", Location = new Point(14, 97), Size = new Size(iw, 17), Font = new Font("Segoe UI", 8.5F), ForeColor = Color.FromArgb(120, 110, 160) };
            dtpCheckOut = new DateTimePicker { Location = new Point(14, 116), Size = new Size(iw, 26), Font = new Font("Segoe UI", 9.5F), Format = DateTimePickerFormat.Short, Value = DateTime.Today };
            dtpCheckOut.ValueChanged += (s, e) => { if (cId > 0) RecalcBill(); };

            var sep1 = new Panel { Location = new Point(14, 150), Size = new Size(iw, 1), BackColor = C_GRID };

            int ry = 158;
            int lw = 152, vx = 154, vw = iw - 154;

            var l1 = MakeMoneyLabel("Tổng tiền phòng:",   ry, lw);
            lblRoomTotalValue = MakeMoneyValue("—", ry, vx, vw); ry += 32;
            var l2 = MakeMoneyLabel("Tổng tiền dịch vụ:", ry, lw);
            lblSvcTotalValue  = MakeMoneyValue("—", ry, vx, vw); ry += 32;
            var l3 = MakeMoneyLabel("Thuế GTGT (10%):",   ry, lw);
            lblVATValue       = MakeMoneyValue("—", ry, vx, vw); ry += 36;

            var sep2 = new Panel { Location = new Point(14, ry), Size = new Size(iw, 1), BackColor = C_GRID };
            ry += 9;

            var lblGrandLbl = new Label {
                Text = "TỔNG CỘNG THANH TOÁN",
                Location = new Point(14, ry), Size = new Size(iw, 20),
                Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                ForeColor = C_DARK, TextAlign = ContentAlignment.MiddleCenter
            };
            ry += 24;

            lblGrandAmount = new Label {
                Text = "—",
                Location = new Point(14, ry), Size = new Size(iw, 42),
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = C_PURPLE, TextAlign = ContentAlignment.MiddleCenter
            };
            ry += 46;

            var btnConfirm = new Button {
                Text = "XÁC NHẬN THANH TOÁN",
                Location = new Point(14, ry), Size = new Size(iw, 38),
                BackColor = Color.FromArgb(34, 168, 95), ForeColor = Color.White,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;

            pnlRight.Controls.AddRange(new Control[] {
                pnlHdr, lblInfoCust, lblInfoRoom, lblDateLbl, dtpCheckOut, sep1,
                l1, lblRoomTotalValue, l2, lblSvcTotalValue, l3, lblVATValue,
                sep2, lblGrandLbl, lblGrandAmount, btnConfirm
            });

            // ── Left panel (service detail list) ───────────────────────────────
            pnlLeft = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };

            var accentL    = new Panel { Dock = DockStyle.Top, Height = 4, BackColor = C_PURPLE };
            var pnlBillHead = new Panel { Dock = DockStyle.Top, Height = 40, BackColor = Color.White };
            pnlBillHead.Controls.Add(new Label {
                Text = "Chi Tiết Dịch Vụ Đã Sử Dụng",
                Location = new Point(14, 6), AutoSize = false, Size = new Size(600, 28),
                Font = new Font("Segoe UI", 11F, FontStyle.Bold), ForeColor = C_DARK
            });

            var pnlFooterLeft = new Panel { Dock = DockStyle.Bottom, Height = 52, BackColor = Color.White };
            lblPhongTotal = new Label { Dock = DockStyle.Bottom, Height = 24, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = C_DARK, TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(0, 0, 12, 0) };
            lblDVTotal    = new Label { Dock = DockStyle.Bottom, Height = 24, Font = new Font("Segoe UI", 9.5F, FontStyle.Bold), ForeColor = C_DARK, TextAlign = ContentAlignment.MiddleRight, Padding = new Padding(0, 0, 12, 0) };
            pnlFooterLeft.Controls.AddRange(new Control[] { lblPhongTotal, lblDVTotal });

            dgvBillDetail = new DataGridView {
                Dock = DockStyle.Fill,
                ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White, BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9F), MultiSelect = false,
                ScrollBars = ScrollBars.Vertical,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
            };
            ApplyGridStyle(dgvBillDetail);
            dgvBillDetail.RowTemplate.Height = 30;

            var bSTT = new DataGridViewTextBoxColumn { Name = "STT",       HeaderText = "STT"         };
            bSTT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var bTen = new DataGridViewTextBoxColumn { Name = "TenDV",     HeaderText = "Tên Dịch Vụ" };
            var bDG  = new DataGridViewTextBoxColumn { Name = "DonGia",    HeaderText = "Đơn Giá"     };
            bDG.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            var bSL  = new DataGridViewTextBoxColumn { Name = "SoLuong",   HeaderText = "Số Lượng"    };
            bSL.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var bDVT = new DataGridViewTextBoxColumn { Name = "DVTinh",    HeaderText = "ĐVT"         };
            bDVT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var bTT  = new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành Tiền"  };
            bTT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBillDetail.Columns.AddRange(new DataGridViewColumn[] { bSTT, bTen, bDG, bSL, bDVT, bTT });
            // Explicit fixed widths — đủ rộng cho tất cả 6 cột trên màn hình 1366px
            dgvBillDetail.Columns[0].Width = 60;   // STT
            dgvBillDetail.Columns[1].Width = 290;  // Tên Dịch Vụ
            dgvBillDetail.Columns[2].Width = 210;  // Đơn Giá
            dgvBillDetail.Columns[3].Width = 115;  // Số Lượng
            dgvBillDetail.Columns[4].Width = 95;   // ĐVT
            dgvBillDetail.Columns[5].Width = 200;  // Thành Tiền

            pnlLeft.Controls.Add(dgvBillDetail);
            pnlLeft.Controls.Add(pnlFooterLeft);
            pnlLeft.Controls.Add(pnlBillHead);
            pnlLeft.Controls.Add(accentL);

            var pnlGap = new Panel { Dock = DockStyle.Right, Width = 10, BackColor = Color.White };
            pnlContent.Controls.Add(pnlRight);
            pnlContent.Controls.Add(pnlGap);
            pnlContent.Controls.Add(pnlLeft);

            pnlDetail.Controls.Add(pnlContent);
            pnlDetail.Controls.Add(pnlTopBar);
            Controls.Add(pnlDetail);
        }

        // ── Helper label factories ────────────────────────────────────────────

        private static void ApplyGridStyle(DataGridView dgv)
        {
            dgv.ColumnHeadersDefaultCellStyle.BackColor = C_PURPLE;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersHeight = 44;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgv.DefaultCellStyle.SelectionForeColor = C_DARK;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = C_ALTROW;
            dgv.GridColor = C_GRID;
            dgv.RowTemplate.Height = 38;
        }

        private static Label SumInfoLabel(string text, int y, int w) => new Label
        {
            Text = text, Location = new Point(16, y), Size = new Size(w, 28),
            Font = new Font("Segoe UI", 10.5F), ForeColor = C_DARK
        };

        private static Label SumRowLabel(string text, int y) => new Label
        {
            Text = text, Location = new Point(16, y), Size = new Size(150, 28),
            Font = new Font("Segoe UI", 10.5F), ForeColor = Color.FromArgb(80, 80, 110)
        };

        private static Label SumRowValue(string text, int y, int iw) => new Label
        {
            Text = text, Location = new Point(160, y), Size = new Size(iw - 160 - 16, 28),
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
            ForeColor = C_DARK, TextAlign = ContentAlignment.MiddleRight
        };

        private static Label MakeMoneyLabel(string text, int y, int w) => new Label {
            Text = text, Location = new Point(14, y), Size = new Size(w, 22),
            Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(80, 80, 110)
        };

        private static Label MakeMoneyValue(string text, int y, int x, int w) => new Label {
            Text = text, Location = new Point(x, y), Size = new Size(w, 22),
            Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
            ForeColor = C_DARK, TextAlign = ContentAlignment.MiddleRight
        };

        // ══ NAVIGATION ═══════════════════════════════════════════════════════

        private void ShowListView()
        {
            pnlDetail.Visible = false;
            pnlList.Visible   = true;
        }

        private void ShowDetailView()
        {
            pnlList.Visible   = false;
            pnlDetail.Visible = true;
        }

        // ══ DATA ══════════════════════════════════════════════════════════════

        private void LoadCustomerGrid(string filter = "")
        {
            string q;
            if (string.IsNullOrEmpty(filter))
            {
                q = "SELECT b.bid AS cid, g.cname, g.mobile, b.checkin, " +
                    "r.roomNo, r.roomType, r.price " +
                    "FROM bookings b INNER JOIN guests g ON b.gid=g.gid " +
                    "INNER JOIN rooms r ON b.roomid=r.roomid WHERE b.chekout='NO'";
            }
            else
            {
                string safe = filter.Replace("'", "''");
                q = "SELECT b.bid AS cid, g.cname, g.mobile, b.checkin, " +
                    "r.roomNo, r.roomType, r.price " +
                    "FROM bookings b INNER JOIN guests g ON b.gid=g.gid " +
                    "INNER JOIN rooms r ON b.roomid=r.roomid " +
                    "WHERE b.chekout='NO' AND g.cname LIKE N'" + safe + "%'";
            }

            DataSet ds = fn.GetData(q);
            dgvCustomers.Rows.Clear();
            int i = 1;
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                string checkin = "";
                if (DateTime.TryParse(dr["checkin"]?.ToString(), out DateTime dt))
                    checkin = dt.ToString("dd/MM/yyyy");

                // Columns order: colCid(0), colPriceRaw(1), colSTT(2), colName(3),
                //                colPhone(4), colCI(5), colRoom(6), colType(7), colGia(8), colXem(9-button)
                dgvCustomers.Rows.Add(
                    dr["cid"],
                    dr["price"],
                    i++,
                    dr["cname"],
                    dr["mobile"],
                    checkin,
                    dr["roomNo"],
                    dr["roomType"],
                    string.Format("{0:N0} đ", dr["price"])
                );
            }
            if (lblListCount != null)
                lblListCount.Text = "Tổng: " + (i - 1) + " khách đang ở";
        }

        private void RecalcBill()
        {
            try
            {
                int nights = Math.Max(1, (dtpCheckOut.Value.Date - currentCheckinDate.Date).Days);
                long roomTotal = currentRoomPrice * nights;

                DataSet dsSvc = fn.GetData(
                    "SELECT s.serviceName, cs.quantity, s.price, cs.quantity * s.price AS subtotal " +
                    "FROM customer_services cs INNER JOIN services s ON cs.sid = s.sid " +
                    "WHERE cs.bid = " + cId + " ORDER BY cs.used_date");

                long svcTotal = 0;
                dgvBillDetail.Rows.Clear();

                // Row 1: room charge
                dgvBillDetail.Rows.Add(
                    1,
                    string.Format("Tiền Phòng ({0})", currentRoomNo),
                    string.Format("{0:N0} đ/đêm", currentRoomPrice),
                    nights, "đêm",
                    string.Format("{0:N0} đ", roomTotal));

                // Subsequent rows: services
                int idx = 2;
                foreach (DataRow dr in dsSvc.Tables[0].Rows)
                {
                    long sub = Convert.ToInt64(dr["subtotal"]);
                    svcTotal += sub;
                    dgvBillDetail.Rows.Add(
                        idx++,
                        dr["serviceName"].ToString(),
                        string.Format("{0:N0} đ", dr["price"]),
                        dr["quantity"], "lần",
                        string.Format("{0:N0} đ", sub));
                }

                long vatAmount  = (long)Math.Round((roomTotal + svcTotal) * 0.10);
                long grandTotal = roomTotal + svcTotal + vatAmount;
                _grandTotal = grandTotal;

                lblDVTotal.Text    = string.Format("Tổng Tiền Dịch Vụ:  {0:N0} đ", svcTotal);
                lblPhongTotal.Text = string.Format("Tổng Tiền Phòng:  {0:N0} đ",   roomTotal);
                lblRoomTotalValue.Text = string.Format("{0:N0} đ", roomTotal);
                lblSvcTotalValue.Text  = string.Format("{0:N0} đ", svcTotal);
                lblVATValue.Text       = string.Format("{0:N0} đ", vatAmount);
                lblGrandAmount.Text    = string.Format("{0:N0} đ", grandTotal);
            }
            catch { }
        }

        // ══ EVENTS ═══════════════════════════════════════════════════════════

        private void UC_CheckOut_Load(object sender, EventArgs e)
        {
            LoadCustomerGrid();
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            string s = txtSearch.Text.Trim();
            if (s == "Tìm kiếm...") s = "";
            LoadCustomerGrid(s);
        }

        private void DgvCustomers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (dgvCustomers.Columns[e.ColumnIndex].Name != "colXem") return;

            var row = dgvCustomers.Rows[e.RowIndex];
            if (row.Cells["colCid"].Value == null) return;

            cId              = Convert.ToInt32(row.Cells["colCid"].Value);
            currentRoomPrice = Convert.ToInt64(row.Cells["colPriceRaw"].Value);
            currentRoomNo    = row.Cells["colRoom"].Value?.ToString() ?? "";

            string checkinStr = row.Cells["colCI"].Value?.ToString() ?? "";
            if (!DateTime.TryParseExact(checkinStr, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out currentCheckinDate))
                currentCheckinDate = DateTime.Today;

            lblInfoCust.Text = "Khách hàng:  " + (row.Cells["colName"].Value?.ToString() ?? "");
            lblInfoRoom.Text = "Số phòng:  "   + currentRoomNo;
            dtpCheckOut.Value = DateTime.Today;

            ShowDetailView();
            RecalcBill();
        }

        private void BtnConfirm_Click(object sender, EventArgs e)
        {
            if (cId <= 0)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để check out.", "Thông Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (MessageBox.Show("Bạn có chắc chắn muốn thanh toán?", "Xác Nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                return;

            string cdate = dtpCheckOut.Value.ToString("MM/dd/yyyy");
            string q = "UPDATE bookings SET chekout='YES', checkout='" + cdate + "' WHERE bid=" + cId +
                       "; UPDATE rooms SET booked='NO' WHERE roomNo='" + currentRoomNo.Replace("'", "''") + "'";
            fn.SetData(q, "Check Out Thành Công.");

            // Lưu hoá đơn
            try
            {
                string eidValue = Session.EmployeeId > 0 ? Session.EmployeeId.ToString() : "NULL";
                string insertInv =
                    "INSERT INTO invoices (invoiceNo, bid, createdDate, totalAmount, status, eid) " +
                    "VALUES (" +
                    "  N'HD' + RIGHT('000000' + CAST((SELECT ISNULL(MAX(invoiceId),0)+1 FROM invoices) AS NVARCHAR(10)), 6)," +
                    "  " + cId + "," +
                    "  GETDATE()," +
                    "  " + _grandTotal + "," +
                    "  N'Đã thanh toán'," +
                    "  " + eidValue +
                    ")";
                fn.ExecNonQuery(insertInv);
            }
            catch { }

            cId = 0;
            ShowListView();
            LoadCustomerGrid();
        }

        // ══ PUBLIC API (called by Dashboard) ═════════════════════════════════

        public void ClearAll()
        {
            cId = 0;
            ShowListView();
            dgvBillDetail?.Rows.Clear();
            if (lblDVTotal    != null) lblDVTotal.Text    = "";
            if (lblPhongTotal != null) lblPhongTotal.Text = "";
            if (lblRoomTotalValue != null) lblRoomTotalValue.Text = "—";
            if (lblSvcTotalValue  != null) lblSvcTotalValue.Text  = "—";
            if (lblVATValue       != null) lblVATValue.Text       = "—";
            if (lblGrandAmount    != null) lblGrandAmount.Text    = "—";
        }

        public void clearAll() => ClearAll();

        public void ReloadData() => LoadCustomerGrid();

        // ══ DESIGNER STUBS (hooked in .Designer.cs, must exist) ══════════════

        private void txtName_TextChanged(object sender, EventArgs e) { }
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void btnCheckOut_Click(object sender, EventArgs e) { }
        private void btnCheckOut_Leave(object sender, EventArgs e) { }

        // ══ UI HELPERS ════════════════════════════════════════════════════════

        private static Bitmap CreateBackArrowIcon(int size, Color color)
        {
            var bmp = new Bitmap(size, size);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                float stroke = Math.Max(2f, size * 0.13f);
                using (var pen = new Pen(color, stroke))
                {
                    pen.StartCap  = System.Drawing.Drawing2D.LineCap.Round;
                    pen.EndCap    = System.Drawing.Drawing2D.LineCap.Round;
                    pen.LineJoin  = System.Drawing.Drawing2D.LineJoin.Round;
                    float tip  = size * 0.18f;
                    float mid  = size * 0.50f;
                    float tail = size * 0.82f;
                    float arm  = size * 0.32f;
                    g.DrawLine(pen, tail, mid, tip,       mid);        // shaft
                    g.DrawLine(pen, tip,  mid, tip + arm, mid - arm);  // head top
                    g.DrawLine(pen, tip,  mid, tip + arm, mid + arm);  // head bottom
                }
            }
            return bmp;
        }

    }
}
