using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Guna.UI2.WinForms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_Dashboard : UserControl
    {
        private readonly function fn = new function();
        private readonly ToolTip toolTip = new ToolTip();

        private Label lblCheckIn;
        private Label lblCheckOut;
        private Label lblEmptyRooms;
        private Label lblRevenue;
        private DataGridView dgvRecentInvoices;
        private DataGridView dgvRecentGuests;
        private FlowLayoutPanel flpRoomMap;
        private Chart chartWeekly;
        private Chart chartMonthly;

        public UC_Dashboard()
        {
            InitializeComponent();
            BuildUI();
        }

        private void BuildUI()
        {
            this.BackColor = Color.FromArgb(245, 245, 250);

            // Top bar
            var pnlTopBar = new Panel
            {
                Dock = DockStyle.Top,
                Height = 70,
                BackColor = Color.White,
                Padding = new Padding(16, 0, 16, 0)
            };

            var lblTitle = new Label
            {
                Text = "Dashboad Tổng Quan Thống Kê",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                AutoSize = false,
                Location = new Point(16, 10),
                Size = new Size(420, 50),
                TextAlign = ContentAlignment.MiddleLeft
            };

            var txtSearch = new Guna2TextBox
            {
                PlaceholderText = "Tìm kiếm nhân viên...",
                Location = new Point(450, 18),
                Size = new Size(260, 36),
                BorderRadius = 8,
                Font = new Font("Segoe UI", 10F)
            };

            var btnAddEmployee = new Guna2Button
            {
                Text = "+  Thêm Nhân Viên",
                Location = new Point(730, 16),
                Size = new Size(170, 38),
                BorderRadius = 10,
                FillColor = Color.FromArgb(132, 112, 255),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold)
            };
            btnAddEmployee.Click += (s, e) => (FindForm() as Dashboard)?.ShowEmployeeTab();

            pnlTopBar.Controls.Add(lblTitle);
            pnlTopBar.Controls.Add(txtSearch);
            pnlTopBar.Controls.Add(btnAddEmployee);

            // Stats row
            var flpStats = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 115,
                BackColor = Color.FromArgb(245, 245, 250),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(12, 10, 12, 5)
            };

            lblCheckIn = new Label();
            lblCheckOut = new Label();
            lblEmptyRooms = new Label();
            lblRevenue = new Label();

            flpStats.Controls.Add(MakeStatCard("Hôm nay: Check-in", Color.FromArgb(33, 150, 243), lblCheckIn));
            flpStats.Controls.Add(MakeStatCard("Hôm nay: Check-out", Color.FromArgb(76, 175, 80), lblCheckOut));
            flpStats.Controls.Add(MakeStatCard("Phòng trống", Color.FromArgb(255, 152, 0), lblEmptyRooms));
            flpStats.Controls.Add(MakeStatCard("Doanh thu hôm nay", Color.FromArgb(132, 112, 255), lblRevenue));

            flpStats.Resize += (s, e) =>
            {
                int cardW = (flpStats.ClientSize.Width - 24 - 30) / 4;
                foreach (Control c in flpStats.Controls)
                    c.Width = cardW;
            };

            // Main 3-column layout
            var tblMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 3,
                RowCount = 1,
                Padding = new Padding(12, 8, 12, 12)
            };
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 37F));
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38F));
            tblMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tblMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            tblMain.Controls.Add(BuildLeftPanel(), 0, 0);
            tblMain.Controls.Add(BuildMidPanel(), 1, 0);
            tblMain.Controls.Add(BuildRightPanel(), 2, 0);

            // Add in reverse Dock order
            this.Controls.Add(tblMain);
            this.Controls.Add(flpStats);
            this.Controls.Add(pnlTopBar);
        }

        private Panel MakeStatCard(string title, Color accentColor, Label valueLbl)
        {
            var card = new Panel
            {
                BackColor = Color.White,
                Height = 90,
                Width = 400,
                Margin = new Padding(0, 0, 10, 0)
            };

            var accent = new Panel
            {
                Dock = DockStyle.Left,
                Width = 5,
                BackColor = accentColor
            };

            var lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 9F),
                ForeColor = Color.Gray,
                AutoSize = false,
                Location = new Point(14, 14),
                Size = new Size(320, 22)
            };

            valueLbl.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            valueLbl.ForeColor = accentColor;
            valueLbl.AutoSize = false;
            valueLbl.Location = new Point(14, 38);
            valueLbl.Size = new Size(320, 40);
            valueLbl.Text = "—";

            card.Controls.Add(accent);
            card.Controls.Add(lblTitle);
            card.Controls.Add(valueLbl);
            return card;
        }

        private Control BuildLeftPanel()
        {
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(0, 0, 8, 0)
            };
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));

            // Row 0: Recent invoices
            var pnlInv = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };

            var hdrInv = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
            var lblInvTitle = new Label
            {
                Text = "Giao dịch gần đây",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                AutoSize = true,
                Location = new Point(0, 10)
            };
            var btnAddTx = new Guna2Button
            {
                Text = "+  Thêm Giao Dịch",
                Size = new Size(160, 32),
                BorderRadius = 8,
                FillColor = Color.FromArgb(33, 150, 243),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(200, 6)
            };
            btnAddTx.Click += (s, e) => (FindForm() as Dashboard)?.ShowBookingTab();
            hdrInv.Controls.Add(lblInvTitle);
            hdrInv.Controls.Add(btnAddTx);
            hdrInv.Resize += (s, e) => btnAddTx.Left = hdrInv.Width - btnAddTx.Width - 2;

            dgvRecentInvoices = MakeGrid(new[] { "STT", "Mã HĐ", "Số tiền (đ)", "Ngày" });
            dgvRecentInvoices.Dock = DockStyle.Fill;

            pnlInv.Controls.Add(dgvRecentInvoices);
            pnlInv.Controls.Add(hdrInv);

            // Row 1: Recent check-in guests
            var pnlGuest = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(12) };
            pnlGuest.Margin = new Padding(0, 8, 0, 0);

            var hdrGuest = new Panel { Dock = DockStyle.Top, Height = 44, BackColor = Color.White };
            var lblGuestTitle = new Label
            {
                Text = "Khách hàng vừa check-in",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                AutoSize = true,
                Location = new Point(0, 10)
            };
            hdrGuest.Controls.Add(lblGuestTitle);

            dgvRecentGuests = MakeGrid(new[] { "STT", "Họ và Tên", "Giới Tính", "Số Điện Thoại" });
            dgvRecentGuests.Dock = DockStyle.Fill;

            pnlGuest.Controls.Add(dgvRecentGuests);
            pnlGuest.Controls.Add(hdrGuest);

            tbl.Controls.Add(pnlInv, 0, 0);
            tbl.Controls.Add(pnlGuest, 0, 1);
            return tbl;
        }

        private Control BuildMidPanel()
        {
            var pnl = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(12),
                Margin = new Padding(4, 0, 4, 0)
            };

            // Title
            var lblTitle = new Label
            {
                Text = "Bản đồ phòng",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                Dock = DockStyle.Top,
                Height = 30
            };

            // Legend
            var pnlLegend = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                Height = 30,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                BackColor = Color.White
            };
            pnlLegend.Controls.Add(MakeLegendItem("Trống", Color.FromArgb(76, 175, 80)));
            pnlLegend.Controls.Add(MakeLegendItem("Có khách", Color.FromArgb(33, 150, 243)));
            pnlLegend.Controls.Add(MakeLegendItem("Bẩn", Color.FromArgb(255, 152, 0)));
            pnlLegend.Controls.Add(MakeLegendItem("Bảo trì", Color.FromArgb(244, 67, 54)));

            // Room map grid
            flpRoomMap = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true,
                BackColor = Color.FromArgb(245, 245, 250),
                Padding = new Padding(4)
            };

            pnl.Controls.Add(flpRoomMap);
            pnl.Controls.Add(pnlLegend);
            pnl.Controls.Add(lblTitle);
            return pnl;
        }

        private Panel MakeLegendItem(string text, Color color)
        {
            var pnl = new Panel { Width = 90, Height = 24, BackColor = Color.Transparent, Margin = new Padding(0, 0, 8, 0) };
            var dot = new Panel { Width = 14, Height = 14, BackColor = color, Location = new Point(0, 5) };
            var lbl = new Label { Text = text, Location = new Point(18, 2), AutoSize = true, Font = new Font("Segoe UI", 8.5F) };
            pnl.Controls.Add(dot);
            pnl.Controls.Add(lbl);
            return pnl;
        }

        private Control BuildRightPanel()
        {
            var tbl = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                Margin = new Padding(8, 0, 0, 0)
            };
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

            chartWeekly = BuildBarChart("Doanh thu theo tuần", Color.FromArgb(132, 112, 255));
            chartWeekly.Dock = DockStyle.Fill;
            var pnlW = WrapChart(chartWeekly);

            chartMonthly = BuildBarChart("Lượng khách theo tháng", Color.FromArgb(33, 150, 243));
            chartMonthly.Dock = DockStyle.Fill;
            var pnlM = WrapChart(chartMonthly);
            pnlM.Margin = new Padding(0, 8, 0, 0);

            tbl.Controls.Add(pnlW, 0, 0);
            tbl.Controls.Add(pnlM, 0, 1);
            return tbl;
        }

        private Panel WrapChart(Chart chart)
        {
            var pnl = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(4) };
            chart.Dock = DockStyle.Fill;
            pnl.Controls.Add(chart);
            return pnl;
        }

        private Chart BuildBarChart(string title, Color barColor)
        {
            var chart = new Chart();
            chart.BackColor = Color.White;

            var ca = new ChartArea("main");
            ca.BackColor = Color.White;
            ca.AxisX.LabelStyle.Font = new Font("Segoe UI", 7.5F);
            ca.AxisX.MajorGrid.Enabled = false;
            ca.AxisY.LabelStyle.Font = new Font("Segoe UI", 7.5F);
            ca.AxisY.LabelStyle.Format = "{0:N0}";
            ca.AxisY.MajorGrid.LineColor = Color.FromArgb(230, 230, 235);
            chart.ChartAreas.Add(ca);

            var t = new Title(title);
            t.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            t.ForeColor = Color.FromArgb(50, 50, 90);
            chart.Titles.Add(t);

            var s = new Series { ChartType = SeriesChartType.Column, Color = barColor, IsValueShownAsLabel = false };
            chart.Series.Add(s);
            return chart;
        }

        private DataGridView MakeGrid(string[] columns)
        {
            var dgv = new DataGridView
            {
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BorderStyle = BorderStyle.None,
                BackgroundColor = Color.White,
                GridColor = Color.FromArgb(240, 240, 245),
                Font = new Font("Segoe UI", 9.5F),
                RowTemplate = { Height = 38 },
                EnableHeadersVisualStyles = false
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(132, 112, 255);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 38;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 200, 255);
            dgv.DefaultCellStyle.SelectionForeColor = Color.FromArgb(50, 50, 90);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 247, 255);

            foreach (var col in columns)
                dgv.Columns.Add(col.Replace(" ", "_"), col);

            return dgv;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (Visible) LoadAllData();
        }

        public void LoadAllData()
        {
            LoadStats();
            LoadRecentInvoices();
            LoadRecentGuests();
            LoadRoomMap();
            LoadWeeklyChart();
            LoadMonthlyChart();
        }

        private void LoadStats()
        {
            try
            {
                var ds = fn.GetData("SELECT COUNT(*) FROM bookings WHERE TRY_CAST(checkin AS DATE) = CAST(GETDATE() AS DATE)");
                lblCheckIn.Text = ds.Tables[0].Rows[0][0].ToString();
            }
            catch { lblCheckIn.Text = "—"; }

            try
            {
                var ds = fn.GetData("SELECT COUNT(*) FROM bookings WHERE TRY_CAST(checkout AS DATE) = CAST(GETDATE() AS DATE)");
                lblCheckOut.Text = ds.Tables[0].Rows[0][0].ToString();
            }
            catch { lblCheckOut.Text = "—"; }

            try
            {
                var ds = fn.GetData("SELECT COUNT(*) FROM rooms WHERE status = N'Trống'");
                lblEmptyRooms.Text = ds.Tables[0].Rows[0][0].ToString();
            }
            catch { lblEmptyRooms.Text = "—"; }

            try
            {
                var ds = fn.GetData("SELECT ISNULL(SUM(totalAmount), 0) FROM invoices WHERE CAST(createdDate AS DATE) = CAST(GETDATE() AS DATE)");
                long rev = Convert.ToInt64(ds.Tables[0].Rows[0][0]);
                lblRevenue.Text = string.Format("{0:N0} đ", rev);
            }
            catch { lblRevenue.Text = "—"; }
        }

        private void LoadRecentInvoices()
        {
            dgvRecentInvoices.Rows.Clear();
            try
            {
                var ds = fn.GetData("SELECT TOP 5 invoiceNo, totalAmount, CONVERT(NVARCHAR(10), createdDate, 103) FROM invoices ORDER BY createdDate DESC");
                int stt = 1;
                foreach (DataRow r in ds.Tables[0].Rows)
                    dgvRecentInvoices.Rows.Add(stt++, r[0], string.Format("{0:N0}", r[1]), r[2]);
            }
            catch { }
        }

        private void LoadRecentGuests()
        {
            dgvRecentGuests.Rows.Clear();
            try
            {
                string sql = @"SELECT TOP 5 g.cname, g.gender, g.mobile
                               FROM bookings b JOIN guests g ON b.gid = g.gid
                               WHERE TRY_CAST(b.checkin AS DATE) = CAST(GETDATE() AS DATE)";
                var ds = fn.GetData(sql);
                int stt = 1;
                foreach (DataRow r in ds.Tables[0].Rows)
                    dgvRecentGuests.Rows.Add(stt++, r[0], r[1], r[2]);
            }
            catch { }
        }

        private void LoadRoomMap()
        {
            flpRoomMap.Controls.Clear();
            try
            {
                var ds = fn.GetData("SELECT roomNo, status FROM rooms ORDER BY roomNo");
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    string roomNo = r["roomNo"].ToString();
                    string status = r["status"].ToString();

                    var cell = new Panel
                    {
                        Width = 72,
                        Height = 52,
                        Margin = new Padding(4),
                        BackColor = StatusToColor(status),
                        Cursor = Cursors.Default
                    };
                    var lbl = new Label
                    {
                        Text = roomNo,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter,
                        Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                        ForeColor = Color.White
                    };
                    cell.Controls.Add(lbl);
                    toolTip.SetToolTip(cell, roomNo + " — " + status);
                    toolTip.SetToolTip(lbl, roomNo + " — " + status);
                    flpRoomMap.Controls.Add(cell);
                }
            }
            catch { }
        }

        private static Color StatusToColor(string status)
        {
            switch (status)
            {
                case "Có khách": return Color.FromArgb(33, 150, 243);
                case "Bẩn":     return Color.FromArgb(255, 152, 0);
                case "Bảo trì": return Color.FromArgb(244, 67, 54);
                default:        return Color.FromArgb(76, 175, 80);
            }
        }

        private void LoadWeeklyChart()
        {
            chartWeekly.Series[0].Points.Clear();
            var dict = new Dictionary<DateTime, long>();
            for (int i = 6; i >= 0; i--)
                dict[DateTime.Today.AddDays(-i)] = 0;

            try
            {
                string sql = @"SELECT CAST(createdDate AS DATE) d, ISNULL(SUM(totalAmount),0) total
                               FROM invoices
                               WHERE createdDate >= DATEADD(DAY,-6,CAST(GETDATE() AS DATE))
                               GROUP BY CAST(createdDate AS DATE) ORDER BY d";
                var ds = fn.GetData(sql);
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    if (DateTime.TryParse(r["d"].ToString(), out DateTime d) && dict.ContainsKey(d))
                        dict[d] = Convert.ToInt64(r["total"]);
                }
            }
            catch { }

            foreach (var kv in dict)
            {
                int idx = chartWeekly.Series[0].Points.AddY(kv.Value);
                chartWeekly.Series[0].Points[idx].AxisLabel = kv.Key.ToString("dd/MM");
            }
        }

        private void LoadMonthlyChart()
        {
            chartMonthly.Series[0].Points.Clear();
            int[] counts = new int[12];

            try
            {
                string sql = @"SELECT MONTH(checkin) m, COUNT(*) cnt
                               FROM bookings WHERE YEAR(checkin) = YEAR(GETDATE())
                               GROUP BY MONTH(checkin) ORDER BY m";
                var ds = fn.GetData(sql);
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    int m = Convert.ToInt32(r["m"]);
                    if (m >= 1 && m <= 12) counts[m - 1] = Convert.ToInt32(r["cnt"]);
                }
            }
            catch { }

            string[] months = { "T1", "T2", "T3", "T4", "T5", "T6", "T7", "T8", "T9", "T10", "T11", "T12" };
            for (int i = 0; i < 12; i++)
            {
                int idx = chartMonthly.Series[0].Points.AddY(counts[i]);
                chartMonthly.Series[0].Points[idx].AxisLabel = months[i];
            }
        }
    }
}
