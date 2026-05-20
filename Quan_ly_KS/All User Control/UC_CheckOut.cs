using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_CheckOut : UserControl
    {
        function fn = new function();
        string query;
        int cId;

        // State
        private long currentRoomPrice;
        private DateTime currentCheckinDate;
        private string currentRoomNo;
        private bool hasSelection;

        // Left bill panel
        private Panel pnlLeft;
        private DataGridView dgvBillDetail;
        private Label lblDVTotal;
        private Label lblPhongTotal;

        // Right summary card
        private Panel pnlRight;
        private Label lblInfoCust;
        private Label lblInfoRoom;
        private DateTimePicker dtpCheckOut;
        private Label lblRoomTotalValue;
        private Label lblSvcTotalValue;
        private Label lblVATValue;
        private Label lblGrandAmount;
        private Button btnConfirm;

        public UC_CheckOut()
        {
            InitializeComponent();
            HideOldControls();
            BuildLeftPanel();
            BuildRightPanel();
        }

        private void HideOldControls()
        {
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            txtCName.Visible = false;
            txtRoom.Visible = false;
            txtCheckOutDate.Visible = false;
            btnCheckOut.Visible = false;
        }

        private void BuildLeftPanel()
        {
            pnlLeft = new Panel
            {
                Location = new Point(15, 200),
                Size = new Size(1290, 600),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblTitle = new Label
            {
                Text = "Chi Tiết Dịch Vụ Đã Sử Dụng",
                Location = new Point(12, 12),
                Size = new Size(900, 32),
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90)
            };

            dgvBillDetail = new DataGridView
            {
                Location = new Point(12, 52),
                Size = new Size(1264, 420),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                Font = new Font("Segoe UI", 9.5F),
                MultiSelect = false
            };
            dgvBillDetail.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(100, 88, 255);
            dgvBillDetail.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvBillDetail.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvBillDetail.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvBillDetail.EnableHeadersVisualStyles = false;
            dgvBillDetail.DefaultCellStyle.SelectionBackColor = Color.FromArgb(231, 229, 255);
            dgvBillDetail.DefaultCellStyle.SelectionForeColor = Color.FromArgb(50, 50, 90);
            dgvBillDetail.GridColor = Color.FromArgb(220, 218, 255);
            dgvBillDetail.RowTemplate.Height = 30;
            dgvBillDetail.ColumnHeadersHeight = 38;

            // Columns summing to 1264
            dgvBillDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT", HeaderText = "STT", Width = 50,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvBillDetail.Columns.Add(new DataGridViewTextBoxColumn { Name = "TenDV", HeaderText = "Tên Dịch Vụ / Hàng Hóa", Width = 430 });
            dgvBillDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonGia", HeaderText = "Đơn Giá", Width = 200,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });
            dgvBillDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoLuong", HeaderText = "Số Lượng", Width = 120,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvBillDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DVTinh", HeaderText = "Đơn Vị Tính", Width = 154,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });
            dgvBillDetail.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "ThanhTien", HeaderText = "Thành Tiền", Width = 310,
                DefaultCellStyle = { Alignment = DataGridViewContentAlignment.MiddleRight }
            });

            lblDVTotal = new Label
            {
                Location = new Point(12, 486),
                Size = new Size(1264, 28),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                TextAlign = ContentAlignment.MiddleRight,
                Text = ""
            };
            lblPhongTotal = new Label
            {
                Location = new Point(12, 524),
                Size = new Size(1264, 28),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                TextAlign = ContentAlignment.MiddleRight,
                Text = ""
            };

            pnlLeft.Controls.AddRange(new Control[] { lblTitle, dgvBillDetail, lblDVTotal, lblPhongTotal });
            this.Controls.Add(pnlLeft);
        }

        private void BuildRightPanel()
        {
            pnlRight = new Panel
            {
                Location = new Point(1320, 48),
                Size = new Size(555, 756),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Purple header
            var pnlHeader = new Panel
            {
                Location = new Point(0, 0),
                Size = new Size(555, 65),
                BackColor = Color.FromArgb(100, 88, 255)
            };
            var lblCardTitle = new Label
            {
                Text = "Tóm Tắt Thanh Toán",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter
            };
            pnlHeader.Controls.Add(lblCardTitle);

            lblInfoCust = new Label
            {
                Location = new Point(16, 76),
                Size = new Size(523, 28),
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(50, 50, 90),
                Text = "Khách hàng: —"
            };
            lblInfoRoom = new Label
            {
                Location = new Point(16, 112),
                Size = new Size(523, 28),
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(50, 50, 90),
                Text = "Số phòng: —"
            };

            var lblDateLabel = new Label
            {
                Text = "Ngày Thanh Toán",
                Location = new Point(16, 152),
                Size = new Size(523, 24),
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gray
            };
            dtpCheckOut = new DateTimePicker
            {
                Location = new Point(16, 180),
                Size = new Size(523, 36),
                Font = new Font("Segoe UI", 11F),
                Format = DateTimePickerFormat.Short,
                Value = DateTime.Today
            };
            dtpCheckOut.ValueChanged += (s, e) => { if (hasSelection) RecalcBill(); };

            var sep1 = new Panel
            {
                Location = new Point(16, 230),
                Size = new Size(523, 1),
                BackColor = Color.FromArgb(210, 208, 255)
            };

            // Breakdown rows
            int y = 242;
            var lblR1 = MakeLabel("Tổng tiền phòng:", y);
            lblRoomTotalValue = MakeValueLabel("—", y); y += 50;
            var lblR2 = MakeLabel("Tổng tiền dịch vụ:", y);
            lblSvcTotalValue = MakeValueLabel("—", y); y += 50;
            var lblR3 = MakeLabel("Thuế GTGT (10%):", y);
            lblVATValue = MakeValueLabel("—", y); y += 55;

            var sep2 = new Panel
            {
                Location = new Point(16, y),
                Size = new Size(523, 1),
                BackColor = Color.FromArgb(210, 208, 255)
            };
            y += 14;

            var lblGrandLabel = new Label
            {
                Text = "TỔNG CỘNG THANH TOÁN",
                Location = new Point(16, y),
                Size = new Size(523, 28),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                TextAlign = ContentAlignment.MiddleCenter
            };
            y += 36;
            lblGrandAmount = new Label
            {
                Text = "—",
                Location = new Point(16, y),
                Size = new Size(523, 60),
                Font = new Font("Segoe UI", 20F, FontStyle.Bold),
                ForeColor = Color.FromArgb(100, 88, 255),
                TextAlign = ContentAlignment.MiddleCenter
            };
            y += 68;

            btnConfirm = new Button
            {
                Text = "XÁC NHẬN THANH TOÁN",
                Location = new Point(16, y),
                Size = new Size(523, 62),
                BackColor = Color.FromArgb(34, 168, 95),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleCenter
            };
            btnConfirm.FlatAppearance.BorderSize = 0;
            btnConfirm.Click += btnConfirm_Click;

            pnlRight.Controls.AddRange(new Control[] {
                pnlHeader, lblInfoCust, lblInfoRoom,
                lblDateLabel, dtpCheckOut, sep1,
                lblR1, lblRoomTotalValue,
                lblR2, lblSvcTotalValue,
                lblR3, lblVATValue,
                sep2, lblGrandLabel, lblGrandAmount, btnConfirm
            });
            this.Controls.Add(pnlRight);
        }

        private Label MakeLabel(string text, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(16, y),
                Size = new Size(280, 28),
                Font = new Font("Segoe UI", 10.5F),
                ForeColor = Color.FromArgb(80, 80, 110)
            };
        }

        private Label MakeValueLabel(string text, int y)
        {
            return new Label
            {
                Text = text,
                Location = new Point(300, y),
                Size = new Size(239, 28),
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                ForeColor = Color.FromArgb(50, 50, 90),
                TextAlign = ContentAlignment.MiddleRight
            };
        }

        private void UC_CheckOut_Load(object sender, EventArgs e)
        {
            LoadCustomerGrid();
        }

        private void LoadCustomerGrid()
        {
            query = "select customer.cid, customer.cname, customer.mobile, customer.checkin, " +
                    "rooms.roomNo, rooms.roomType, rooms.price " +
                    "from customer inner join rooms on customer.roomid = rooms.roomid where chekout = 'NO'";
            DataSet ds = fn.GetData(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string s = txtName.Text.Trim();
            query = string.IsNullOrEmpty(s)
                ? "select customer.cid, customer.cname, customer.mobile, customer.checkin, " +
                  "rooms.roomNo, rooms.roomType, rooms.price " +
                  "from customer inner join rooms on customer.roomid = rooms.roomid where chekout = 'NO'"
                : "select customer.cid, customer.cname, customer.mobile, customer.checkin, " +
                  "rooms.roomNo, rooms.roomType, rooms.price " +
                  "from customer inner join rooms on customer.roomid = rooms.roomid " +
                  "where customer.cname like '" + s + "%' and chekout = 'NO'";

            DataSet ds = fn.GetData(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
            guna2DataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var row = guna2DataGridView1.Rows[e.RowIndex];
            if (row.Cells[0].Value == null) return;

            int selectedCid = int.Parse(row.Cells[0].Value.ToString());
            DataSet dsCustomer = fn.GetData(
                "select customer.cid, customer.cname, customer.checkin, rooms.roomNo, " +
                "rooms.roomType, rooms.price " +
                "from customer inner join rooms on customer.roomid = rooms.roomid " +
                "where customer.cid = " + selectedCid);

            if (dsCustomer.Tables[0].Rows.Count == 0) return;
            DataRow dr = dsCustomer.Tables[0].Rows[0];

            cId = Convert.ToInt32(dr["cid"]);
            currentRoomPrice = Convert.ToInt64(dr["price"]);
            currentRoomNo = dr["roomNo"].ToString();

            string checkinStr = dr["checkin"]?.ToString() ?? "";
            if (!DateTime.TryParse(checkinStr, out currentCheckinDate))
                currentCheckinDate = DateTime.Today;

            lblInfoCust.Text = "Khách hàng:  " + dr["cname"].ToString();
            lblInfoRoom.Text = "Số phòng:  " + currentRoomNo;

            hasSelection = true;
            RecalcBill();
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

                dgvBillDetail.Rows.Add(
                    1,
                    string.Format("Tiền Phòng ({0})", currentRoomNo),
                    string.Format("{0:N0} đ/đêm", currentRoomPrice),
                    nights, "đêm",
                    string.Format("{0:N0} đ", roomTotal));

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

                long vatAmount = (long)Math.Round((roomTotal + svcTotal) * 0.10);
                long grandTotal = roomTotal + svcTotal + vatAmount;

                lblDVTotal.Text    = string.Format("Tổng Tiền Dịch Vụ:  {0:N0} đ", svcTotal);
                lblPhongTotal.Text = string.Format("Tổng Tiền Phòng:  {0:N0} đ", roomTotal);
                lblRoomTotalValue.Text = string.Format("{0:N0} đ", roomTotal);
                lblSvcTotalValue.Text  = string.Format("{0:N0} đ", svcTotal);
                lblVATValue.Text       = string.Format("{0:N0} đ", vatAmount);
                lblGrandAmount.Text    = string.Format("{0:N0} đ", grandTotal);
            }
            catch { }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (!hasSelection)
            {
                MessageBox.Show("Vui lòng chọn khách hàng để check out.", "Thông Báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (MessageBox.Show("Bạn có chắc chắn muốn thanh toán?", "Xác Nhận",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                string cdate = dtpCheckOut.Value.ToString("MM/dd/yyyy");
                string q = "update customer set chekout = 'YES', checkout = '" + cdate + "' where cid = " + cId +
                           " update rooms set booked = 'NO' where roomNo = '" + currentRoomNo + "'";
                fn.SetData(q, "Check Out Thành Công.");
                LoadCustomerGrid();
                ClearAll();
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e) { }
        private void btnCheckOut_Leave(object sender, EventArgs e) { }

        public void clearAll() => ClearAll();

        public void ClearAll()
        {
            txtName.Clear();
            hasSelection = false;
            lblInfoCust.Text = "Khách hàng: —";
            lblInfoRoom.Text = "Số phòng: —";
            dgvBillDetail.Rows.Clear();
            lblDVTotal.Text = "";
            lblPhongTotal.Text = "";
            lblRoomTotalValue.Text = "—";
            lblSvcTotalValue.Text  = "—";
            lblVATValue.Text       = "—";
            lblGrandAmount.Text    = "—";
        }

        public void ReloadData() => LoadCustomerGrid();
    }
}
