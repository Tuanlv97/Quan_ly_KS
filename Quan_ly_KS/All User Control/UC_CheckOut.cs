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

        // List view
        private DataGridView dgvCustomers;
        private TextBox txtSearch;

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
            pnlList = new Panel
            {
                Location = Point.Empty,
                Size = new Size(1882, 852),
                BackColor = Color.White
            };

            // Title
            var lblTitle = new Label
            {
                Text = "Thanh Toán",
                Location = new Point(PAD, 16),
                Size = new Size(280, 44),
                Font = new Font("Century Gothic", 18F, FontStyle.Bold),
                ForeColor = C_DARK
            };

            // Search group (top-right)
            var lblSearchHdr = new Label
            {
                Text = "Tìm Kiếm",
                Location = new Point(1356, 14),
                AutoSize = true,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Italic),
                ForeColor = Color.FromArgb(120, 110, 160)
            };
            txtSearch = new TextBox
            {
                Location = new Point(1356, 36),
                Size = new Size(506, 32),
                Font = new Font("Segoe UI", 10.5F),
                BorderStyle = BorderStyle.FixedSingle,
                ForeColor = Color.Gray,
                Text = "Enter FullName"
            };
            txtSearch.GotFocus  += (s, e) => { if (txtSearch.Text == "Enter FullName") { txtSearch.Text = ""; txtSearch.ForeColor = Color.Black; } };
            txtSearch.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtSearch.Text)) { txtSearch.Text = "Enter FullName"; txtSearch.ForeColor = Color.Gray; } };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // Separator
            var sep = new Panel
            {
                Location = new Point(PAD, 78),
                Size = new Size(1882 - 2 * PAD, 2),
                BackColor = C_GRID
            };

            // Customer DataGridView
            dgvCustomers = new DataGridView
            {
                Location = new Point(PAD, 88),
                Size = new Size(1882 - 2 * PAD, 852 - 88 - PAD),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 10F),
                MultiSelect = false,
                ScrollBars = ScrollBars.Vertical,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ApplyGridStyle(dgvCustomers);

            // Hidden data columns (must come first for Rows.Add positional assignment)
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCid",      Visible = false, FillWeight = 1 });
            dgvCustomers.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPriceRaw", Visible = false, FillWeight = 1 });

            // Visible columns
            var cSTT = new DataGridViewTextBoxColumn { Name = "colSTT",   HeaderText = "STT",              FillWeight = 32,  MinimumWidth = 55 };
            cSTT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var cName  = new DataGridViewTextBoxColumn { Name = "colName",  HeaderText = "Tên Khách Hàng",   FillWeight = 250, MinimumWidth = 180 };
            var cPhone = new DataGridViewTextBoxColumn { Name = "colPhone", HeaderText = "Số Điện Thoại",    FillWeight = 130, MinimumWidth = 130 };
            cPhone.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var cCI    = new DataGridViewTextBoxColumn { Name = "colCI",    HeaderText = "Ngày Check-In",    FillWeight = 145, MinimumWidth = 140 };
            cCI.DefaultCellStyle.Alignment    = DataGridViewContentAlignment.MiddleCenter;
            var cRoom  = new DataGridViewTextBoxColumn { Name = "colRoom",  HeaderText = "Số Phòng",         FillWeight = 100, MinimumWidth = 100 };
            cRoom.DefaultCellStyle.Alignment  = DataGridViewContentAlignment.MiddleCenter;
            var cType  = new DataGridViewTextBoxColumn { Name = "colType",  HeaderText = "Loại Phòng",       FillWeight = 145, MinimumWidth = 120 };
            cType.DefaultCellStyle.Alignment  = DataGridViewContentAlignment.MiddleCenter;
            var cGia   = new DataGridViewTextBoxColumn { Name = "colGia",   HeaderText = "Giá / Đêm",        FillWeight = 140, MinimumWidth = 140 };
            cGia.DefaultCellStyle.Alignment   = DataGridViewContentAlignment.MiddleRight;

            // Xem button column — fixed width, not Fill
            var cXem = new DataGridViewButtonColumn
            {
                Name = "colXem", HeaderText = "",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None, Width = 115,
                SortMode = DataGridViewColumnSortMode.NotSortable,
                Text = "Xem", UseColumnTextForButtonValue = true
            };
            cXem.DefaultCellStyle.BackColor          = C_PURPLE;
            cXem.DefaultCellStyle.ForeColor          = Color.White;
            cXem.DefaultCellStyle.Font               = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            cXem.DefaultCellStyle.Alignment          = DataGridViewContentAlignment.MiddleCenter;
            cXem.DefaultCellStyle.SelectionBackColor = Color.FromArgb(78, 66, 210);
            cXem.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvCustomers.Columns.AddRange(new DataGridViewColumn[]
                { cSTT, cName, cPhone, cCI, cRoom, cType, cGia, cXem });

            dgvCustomers.CellClick += DgvCustomers_CellClick;

            pnlList.Controls.AddRange(new Control[] { lblTitle, lblSearchHdr, txtSearch, sep, dgvCustomers });
            Controls.Add(pnlList);
        }

        // ══ DETAIL VIEW ══════════════════════════════════════════════════════

        private void BuildDetailView()
        {
            pnlDetail = new Panel
            {
                Location = Point.Empty,
                Size = new Size(1882, 852),
                BackColor = Color.White,
                Visible = false
            };

            // ── Top bar ──────────────────────────────────────────────────────
            var btnBack = new Button
            {
                Text = "◄ Quay lại",
                Location = new Point(PAD, 14),
                Size = new Size(128, 38),
                BackColor = Color.FromArgb(234, 232, 255),
                ForeColor = C_PURPLE,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnBack.FlatAppearance.BorderColor = C_PURPLE;
            btnBack.FlatAppearance.BorderSize  = 1;
            btnBack.Click += (s, e) => ShowListView();

            var lblDetTitle = new Label
            {
                Text = "Thanh Toán",
                Location = new Point(162, 14),
                Size = new Size(300, 38),
                Font = new Font("Century Gothic", 16F, FontStyle.Bold),
                ForeColor = C_DARK
            };

            // ── Left panel — bill detail ──────────────────────────────────────
            var pnlLeft = new Panel
            {
                Location = new Point(PAD, TOPBAR),
                Size = new Size(LEFT_W, CONTENT_H),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Purple accent top bar inside left panel
            var accentL = new Panel { Location = new Point(0, 0), Size = new Size(LEFT_W, 5), BackColor = C_PURPLE };

            var lblBillTitle = new Label
            {
                Text = "Chi Tiết Dịch Vụ Đã Sử Dụng",
                Location = new Point(14, 12),
                Size = new Size(LEFT_W - 28, 34),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = C_DARK
            };

            int dgvBW = LEFT_W - 28;   // 1232 — full interior width của pnlLeft
            dgvBillDetail = new DataGridView
            {
                Location = new Point(14, 54),
                Size = new Size(dgvBW, 546),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9.5F),
                MultiSelect = false,
                ScrollBars = ScrollBars.Both,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            ApplyGridStyle(dgvBillDetail);

            // FillWeight kiểm soát tỷ lệ rộng: TenDV ngắn (FW=90), ThanhTien rộng hơn (FW=120)
            var bSTT = new DataGridViewTextBoxColumn { Name = "STT",       HeaderText = "STT",          FillWeight = 25,  MinimumWidth = 50  };
            bSTT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var bTen = new DataGridViewTextBoxColumn { Name = "TenDV",     HeaderText = "Tên Dịch Vụ",  FillWeight = 90,  MinimumWidth = 150 };
            var bDG  = new DataGridViewTextBoxColumn { Name = "DonGia",    HeaderText = "Đơn Giá",      FillWeight = 110, MinimumWidth = 120 };
            bDG.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            var bSL  = new DataGridViewTextBoxColumn { Name = "SoLuong",   HeaderText = "Số Lượng",     FillWeight = 80,  MinimumWidth = 90  };
            bSL.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var bDVT = new DataGridViewTextBoxColumn { Name = "DVTinh",    HeaderText = "ĐVT",          FillWeight = 90,  MinimumWidth = 90  };
            bDVT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var bTT  = new DataGridViewTextBoxColumn { Name = "ThanhTien", HeaderText = "Thành Tiền",   FillWeight = 120, MinimumWidth = 120 };
            bTT.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvBillDetail.Columns.AddRange(new DataGridViewColumn[] { bSTT, bTen, bDG, bSL, bDVT, bTT });

            int fy = 54 + 546 + 10;    // footer y = 610
            lblDVTotal = new Label
            {
                Location = new Point(14, fy),
                Size = new Size(dgvBW, 28),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = C_DARK, TextAlign = ContentAlignment.MiddleRight
            };
            lblPhongTotal = new Label
            {
                Location = new Point(14, fy + 34),
                Size = new Size(dgvBW, 28),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = C_DARK, TextAlign = ContentAlignment.MiddleRight
            };

            pnlLeft.Controls.AddRange(new Control[] { accentL, lblBillTitle, dgvBillDetail, lblDVTotal, lblPhongTotal });

            // ── Right panel — summary ─────────────────────────────────────────
            int rightX = PAD + LEFT_W + GAP;    // 1290
            var pnlRight = new Panel
            {
                Location = new Point(rightX, TOPBAR),
                Size = new Size(RIGHT_W, CONTENT_H),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Purple header bar
            var pnlHdr = new Panel { Location = new Point(0, 0), Size = new Size(RIGHT_W, 58), BackColor = C_PURPLE };
            pnlHdr.Controls.Add(new Label
            {
                Text = "Tóm Tắt Thanh Toán",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            });

            // Inner content width = RIGHT_W - 32 = 542
            int iw = RIGHT_W - 32;   // 542

            lblInfoCust = SumInfoLabel("Khách hàng: —", 70, iw);
            lblInfoRoom = SumInfoLabel("Số phòng: —",   106, iw);

            var lblDateLbl = new Label
            {
                Text = "Ngày Thanh Toán",
                Location = new Point(16, 148), Size = new Size(iw, 22),
                Font = new Font("Segoe UI", 9.5F), ForeColor = Color.FromArgb(120, 110, 160)
            };
            dtpCheckOut = new DateTimePicker
            {
                Location = new Point(16, 172), Size = new Size(iw, 36),
                Font = new Font("Segoe UI", 10.5F),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            dtpCheckOut.ValueChanged += (s, e) => { if (cId > 0) RecalcBill(); };

            var sep1 = new Panel { Location = new Point(16, 220), Size = new Size(iw, 1), BackColor = C_GRID };

            // Breakdown rows
            int ry = 232;
            var l1 = SumRowLabel("Tổng tiền phòng:",    ry);
            lblRoomTotalValue = SumRowValue("—", ry, iw); ry += 46;
            var l2 = SumRowLabel("Tổng tiền dịch vụ:",  ry);
            lblSvcTotalValue  = SumRowValue("—", ry, iw); ry += 46;
            var l3 = SumRowLabel("Thuế GTGT (10%):",    ry);
            lblVATValue       = SumRowValue("—", ry, iw); ry += 52;

            var sep2 = new Panel { Location = new Point(16, ry), Size = new Size(iw, 1), BackColor = C_GRID };
            ry += 14;

            var lblGrandLbl = new Label
            {
                Text = "TỔNG CỘNG THANH TOÁN",
                Location = new Point(16, ry), Size = new Size(iw, 28),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = C_DARK, TextAlign = ContentAlignment.MiddleCenter
            };
            ry += 36;

            lblGrandAmount = new Label
            {
                Text = "—",
                Location = new Point(16, ry), Size = new Size(iw, 58),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = C_PURPLE, TextAlign = ContentAlignment.MiddleCenter
            };
            ry += 66;

            var btnConfirm = new Button
            {
                Text = "XÁC NHẬN THANH TOÁN",
                Location = new Point(16, ry), Size = new Size(iw, 58),
                BackColor = Color.FromArgb(34, 168, 95),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += BtnConfirm_Click;

            pnlRight.Controls.AddRange(new Control[]
            {
                pnlHdr, lblInfoCust, lblInfoRoom,
                lblDateLbl, dtpCheckOut, sep1,
                l1, lblRoomTotalValue, l2, lblSvcTotalValue, l3, lblVATValue,
                sep2, lblGrandLbl, lblGrandAmount, btnConfirm
            });

            pnlDetail.Controls.AddRange(new Control[] { btnBack, lblDetTitle, pnlLeft, pnlRight });
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
            Text = text, Location = new Point(16, y), Size = new Size(260, 28),
            Font = new Font("Segoe UI", 10.5F), ForeColor = Color.FromArgb(80, 80, 110)
        };

        private static Label SumRowValue(string text, int y, int iw) => new Label
        {
            Text = text, Location = new Point(280, y), Size = new Size(iw - 264, 28),
            Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
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
                q = "SELECT customer.cid, customer.cname, customer.mobile, customer.checkin, " +
                    "rooms.roomNo, rooms.roomType, rooms.price " +
                    "FROM customer INNER JOIN rooms ON customer.roomid = rooms.roomid WHERE chekout = 'NO'";
            }
            else
            {
                string safe = filter.Replace("'", "''");
                q = "SELECT customer.cid, customer.cname, customer.mobile, customer.checkin, " +
                    "rooms.roomNo, rooms.roomType, rooms.price " +
                    "FROM customer INNER JOIN rooms ON customer.roomid = rooms.roomid " +
                    "WHERE chekout = 'NO' AND customer.cname LIKE '" + safe + "%'";
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
                    "WHERE cs.cid = " + cId + " ORDER BY cs.used_date");

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
            if (s == "Enter FullName") s = "";
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
            string q = "UPDATE customer SET chekout = 'YES', checkout = '" + cdate + "' WHERE cid = " + cId +
                       " UPDATE rooms SET booked = 'NO' WHERE roomNo = '" + currentRoomNo.Replace("'", "''") + "'";
            fn.SetData(q, "Check Out Thành Công.");
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
    }
}
