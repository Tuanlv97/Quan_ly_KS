using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_DichVuKhach : UserControl
    {
        private readonly function fn = new function();
        private int _editingId      = -1;
        private int _selectedBid    = -1;
        private int _selectedSid    = -1;
        private long _unitPrice     = 0;

        private readonly List<int>  _bidList   = new List<int>();
        private readonly List<int>  _sidList   = new List<int>();
        private readonly List<long> _priceList = new List<long>();

        private DataGridView        dgv;
        private Guna2TextBox        txtSearch;
        private Label               lblCount;
        private Guna2Panel          pnlForm;
        private Label               lblFormTitle;

        private Guna2ComboBox       fCustomer, fService;
        private Guna2TextBox        fQty, fUnitPrice, fTotal;
        private Guna2DateTimePicker fUsedDate;

        public UC_DichVuKhach()
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
                Text = "Dịch Vụ Khách Sử Dụng",
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
                Text = "+  Thêm Dịch Vụ", Width = 200, Height = 45, Top = 13,
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
            dgv = new DataGridView {
                Dock = DockStyle.Fill, BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None, RowHeadersVisible = false,
                AllowUserToAddRows = false, AllowUserToResizeRows = false,
                ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 10F), GridColor = Color.FromArgb(230, 230, 230),
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            dgv.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(50, 50, 50);
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgv.ColumnHeadersHeight = 42;
            dgv.RowTemplate.Height  = 40;

            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStt",     HeaderText = "STT",           FillWeight = 40  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId",      HeaderText = "Mã",            FillWeight = 55  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colMaKH",    HeaderText = "Mã KH",         FillWeight = 55  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName",    HeaderText = "Tên Khách",     FillWeight = 140 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRoom",    HeaderText = "Số Phòng",      FillWeight = 70  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colService", HeaderText = "Tên Dịch Vụ",  FillWeight = 150 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colQty",     HeaderText = "SL",            FillWeight = 45  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrice",   HeaderText = "Đơn Giá",      FillWeight = 90  });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colTotal",   HeaderText = "Thành Tiền",   FillWeight = 100 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDate",    HeaderText = "Ngày Sử Dụng", FillWeight = 100 });
            dgv.Columns.Add(new DataGridViewButtonColumn  { Name = "colEdit",    HeaderText = "", Text = "✏",  UseColumnTextForButtonValue = true, FillWeight = 38 });
            dgv.Columns.Add(new DataGridViewButtonColumn  { Name = "colDel",     HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 38 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colChekout", HeaderText = "", FillWeight = 1, Visible = false });

            dgv.CellClick += Dgv_CellClick;
            pnlGrid.Controls.Add(dgv);

            // ── Overlay form ──────────────────────────────────────────
            BuildFormPanel();

            // ── Wire root events ──────────────────────────────────────
            btnAdd.Click          += (s, e) => ShowForm(-1);
            txtSearch.TextChanged += (s, e) => LoadData();
            this.SizeChanged      += (s, e) => CenterFormPanel();
            this.HandleCreated    += (s, e) => { LoadData(); CenterFormPanel(); };

            this.Controls.Add(pnlGrid);
            this.Controls.Add(sep);
            this.Controls.Add(pnlHeader);
            this.Controls.Add(pnlForm);
        }

        private void BuildFormPanel()
        {
            const int formW = 500, lx = 18, fw = 464, inputH = 35;
            const int startY = 48, l2i = 17, fGap = 56, btnGap = 14;
            int lastInputBot = startY + 5 * fGap + l2i + inputH; // 380
            int btnY  = lastInputBot + btnGap;                   // 394
            int formH = btnY + 40 + 16;                          // 450

            pnlForm = new Guna2Panel {
                Width = formW, Height = formH,
                FillColor = Color.White,
                BorderRadius = 12,
                BorderColor = Color.FromArgb(100, 132, 112, 255),
                BorderThickness = 2,
                Visible = false
            };
            pnlForm.ShadowDecoration.Enabled = true;
            pnlForm.ShadowDecoration.Color   = Color.FromArgb(55, 132, 112, 255);
            pnlForm.ShadowDecoration.Depth   = 14;

            var btnX = new Label {
                Text = "X", Size = new Size(28, 28),
                Top = 8, Left = formW - 28 - 8,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(80, 80, 90),
                BackColor = Color.FromArgb(225, 225, 232),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnX.Click += (s, ev) => pnlForm.Visible = false;
            pnlForm.Controls.Add(btnX);

            lblFormTitle = new Label {
                Text = "Thêm Dịch Vụ Sử Dụng",
                Font = new Font("Segoe UI", 13F, FontStyle.Bold),
                ForeColor = Color.SlateBlue, AutoSize = true, Left = lx, Top = 16
            };
            pnlForm.Controls.Add(lblFormTitle);

            AddLbl(pnlForm, lx, startY + 0 * fGap, "Khách Hàng");
            fCustomer = new Guna2ComboBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList,
                Left = lx, Top = startY + 0 * fGap + l2i, Width = fw, Height = inputH
            };

            AddLbl(pnlForm, lx, startY + 1 * fGap, "Dịch Vụ");
            fService = new Guna2ComboBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke,
                Font = new Font("Segoe UI", 10F), DropDownStyle = ComboBoxStyle.DropDownList,
                Left = lx, Top = startY + 1 * fGap + l2i, Width = fw, Height = inputH
            };

            AddLbl(pnlForm, lx, startY + 2 * fGap, "Số Lượng");
            fQty = new Guna2TextBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke, Font = new Font("Segoe UI", 10F),
                PlaceholderText = "Nhập số lượng...",
                Left = lx, Top = startY + 2 * fGap + l2i, Width = fw, Height = inputH
            };

            AddLbl(pnlForm, lx, startY + 3 * fGap, "Ngày Sử Dụng");
            fUsedDate = new Guna2DateTimePicker {
                FillColor = Color.WhiteSmoke, Font = new Font("Segoe UI", 10F),
                Format = DateTimePickerFormat.Short, Value = DateTime.Now,
                Left = lx, Top = startY + 3 * fGap + l2i, Width = fw, Height = inputH
            };

            AddLbl(pnlForm, lx, startY + 4 * fGap, "Đơn Giá");
            fUnitPrice = new Guna2TextBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke, Font = new Font("Segoe UI", 10F),
                ReadOnly = true, Left = lx, Top = startY + 4 * fGap + l2i, Width = fw, Height = inputH
            };

            AddLbl(pnlForm, lx, startY + 5 * fGap, "Thành Tiền");
            fTotal = new Guna2TextBox {
                BorderRadius = 8, FillColor = Color.WhiteSmoke, Font = new Font("Segoe UI", 10F),
                ReadOnly = true, Left = lx, Top = startY + 5 * fGap + l2i, Width = fw, Height = inputH
            };

            var btnSave = new Guna2Button {
                Text = "Lưu", Left = lx, Top = btnY, Width = 150, Height = 40,
                BorderRadius = 8, FillColor = Color.SlateBlue, ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand
            };
            var btnCancel = new Guna2Button {
                Text = "Hủy", Left = lx + 160, Top = btnY, Width = 150, Height = 40,
                BorderRadius = 8, FillColor = Color.FromArgb(180, 180, 180), ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), Cursor = Cursors.Hand
            };

            btnSave.Click   += BtnSave_Click;
            btnCancel.Click += (s, e) => pnlForm.Visible = false;

            fService.SelectedIndexChanged += FService_SelectedIndexChanged;
            fQty.TextChanged              += (s, e) => UpdateTotal();

            pnlForm.Controls.AddRange(new Control[] {
                fCustomer, fService, fQty, fUsedDate, fUnitPrice, fTotal,
                btnSave, btnCancel
            });
        }

        // ── Helpers ───────────────────────────────────────────────────

        private static void AddLbl(Control parent, int x, int y, string text)
        {
            parent.Controls.Add(new Label {
                Text = text, AutoSize = true, Left = x, Top = y,
                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                ForeColor = Color.FromArgb(80, 80, 80)
            });
        }

        private void CenterFormPanel()
        {
            if (pnlForm == null) return;
            pnlForm.Left = (Width  - pnlForm.Width)  / 2;
            pnlForm.Top  = (Height - pnlForm.Height) / 2;
        }

        public void Reload() => LoadData();

        // ── Combos ────────────────────────────────────────────────────

        private void LoadCustomerCombo(int editingBid = -1)
        {
            fCustomer.Items.Clear();
            _bidList.Clear();
            string sql = "SELECT b.bid, g.cname, r.roomNo FROM bookings b " +
                         "INNER JOIN guests g ON b.gid=g.gid " +
                         "INNER JOIN rooms r ON b.roomid=r.roomid " +
                         (editingBid > 0
                             ? "WHERE b.chekout='NO' OR b.bid=" + editingBid
                             : "WHERE b.chekout='NO'") +
                         " ORDER BY g.cname";
            try
            {
                var ds = fn.GetData(sql);
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    int bid = Convert.ToInt32(r["bid"]);
                    string maDhon = "DP" + bid.ToString().PadLeft(3, '0');
                    fCustomer.Items.Add(maDhon + " - " + r["cname"] + " (Phòng " + r["roomNo"] + ")");
                    _bidList.Add(bid);
                }
            }
            catch { }
        }

        private void LoadServiceCombo()
        {
            fService.Items.Clear();
            _sidList.Clear();
            _priceList.Clear();
            try
            {
                var ds = fn.GetData("SELECT sid, serviceName, price FROM services ORDER BY serviceName");
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    _sidList.Add(Convert.ToInt32(r["sid"]));
                    long price = Convert.ToInt64(r["price"]);
                    _priceList.Add(price);
                    fService.Items.Add(r["serviceName"] + "  (" + price.ToString("N0") + " đ)");
                }
            }
            catch { }
        }

        private void FService_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = fService.SelectedIndex;
            if (idx < 0 || idx >= _priceList.Count) { fUnitPrice.Text = ""; fTotal.Text = ""; return; }
            _unitPrice = _priceList[idx];
            fUnitPrice.Text = _unitPrice.ToString("N0") + " đ";
            UpdateTotal();
        }

        private void UpdateTotal()
        {
            if (_unitPrice > 0 && int.TryParse(fQty.Text.Trim(), out int qty) && qty > 0)
                fTotal.Text = (_unitPrice * qty).ToString("N0") + " đ";
            else
                fTotal.Text = "";
        }

        // ── Data ──────────────────────────────────────────────────────

        private void LoadData()
        {
            string kw  = txtSearch?.Text.Trim() ?? "";
            string sql = "SELECT cs.id, b.bid, g.cname, r.roomNo, s.serviceName, " +
                         "cs.quantity, s.price, cs.quantity*s.price AS total, cs.used_date, b.chekout " +
                         "FROM customer_services cs " +
                         "INNER JOIN bookings b ON cs.bid=b.bid " +
                         "INNER JOIN guests g ON b.gid=g.gid " +
                         "INNER JOIN rooms r ON b.roomid=r.roomid " +
                         "INNER JOIN services s ON cs.sid=s.sid";
            if (!string.IsNullOrEmpty(kw))
                sql += " WHERE g.cname LIKE N'%" + kw + "%'" +
                       " OR s.serviceName LIKE N'%" + kw + "%'" +
                       " OR r.roomNo LIKE '%" + kw + "%'";
            sql += " ORDER BY cs.id DESC";

            dgv.Rows.Clear();
            try
            {
                var ds = fn.GetData(sql);
                int stt = 1;
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    int rawId = Convert.ToInt32(r["id"]);
                    int bid   = Convert.ToInt32(r["bid"]);
                    string maDV = "DV" + rawId.ToString().PadLeft(3, '0');
                    string maKH = "DP" + bid.ToString().PadLeft(3, '0');
                    long price = Convert.ToInt64(r["price"]);
                    long total = Convert.ToInt64(r["total"]);
                    bool checkedOut = r["chekout"].ToString() == "YES";
                    int rowIdx = dgv.Rows.Add(stt++, maDV, maKH, r["cname"], r["roomNo"],
                        r["serviceName"], r["quantity"],
                        price.ToString("N0") + " đ",
                        total.ToString("N0") + " đ",
                        r["used_date"]);
                    dgv.Rows[rowIdx].Cells["colChekout"].Value = r["chekout"];

                    if (checkedOut)
                    {
                        var row = dgv.Rows[rowIdx];
                        row.DefaultCellStyle.ForeColor  = Color.Gray;
                        row.DefaultCellStyle.BackColor  = Color.FromArgb(245, 245, 245);
                        row.DefaultCellStyle.Font       = new Font("Segoe UI", 10F, FontStyle.Italic);
                        row.Cells["colEdit"].Style.ForeColor = Color.LightGray;
                        row.Cells["colEdit"].Style.BackColor = Color.FromArgb(230, 230, 230);
                        row.Cells["colDel"].Style.ForeColor  = Color.LightGray;
                        row.Cells["colDel"].Style.BackColor  = Color.FromArgb(230, 230, 230);
                    }
                }
                lblCount.Text = "Tổng: " + ds.Tables[0].Rows.Count + " bản ghi";
            }
            catch { }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            bool isEditCol = e.ColumnIndex == dgv.Columns["colEdit"].Index;
            bool isDelCol  = e.ColumnIndex == dgv.Columns["colDel"].Index;
            if (!isEditCol && !isDelCol) return;

            bool checkedOut = dgv.Rows[e.RowIndex].Cells["colChekout"].Value?.ToString() == "YES";
            if (checkedOut)
            {
                MessageBox.Show("Khách đã trả phòng. Không thể sửa hoặc xóa dịch vụ.",
                    "Không được phép", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = ParseMaDV(dgv.Rows[e.RowIndex].Cells["colId"].Value.ToString());
            if (isEditCol) ShowForm(id);
            else           DeleteRecord(id, e.RowIndex);
        }

        private static int ParseMaDV(string maDV) =>
            int.Parse(maDV.Replace("DV", "").TrimStart('0').PadLeft(1, '0'));

        // ── Form open ─────────────────────────────────────────────────

        private void ShowForm(int id)
        {
            _editingId   = id;
            _selectedBid = -1;
            _selectedSid = -1;
            _unitPrice   = 0;

            LoadServiceCombo();

            // Reset
            fQty.Text       = "";
            fUnitPrice.Text = "";
            fTotal.Text     = "";
            fService.SelectedIndex  = -1;
            fCustomer.SelectedIndex = -1;
            fUsedDate.Value = DateTime.Now;

            if (id == -1)
            {
                lblFormTitle.Text = "Thêm Dịch Vụ Sử Dụng";
                LoadCustomerCombo();
            }
            else
            {
                lblFormTitle.Text = "Sửa Dịch Vụ Sử Dụng";
                string sql = "SELECT cs.id, cs.bid, cs.sid, cs.quantity, cs.used_date, s.price " +
                             "FROM customer_services cs " +
                             "INNER JOIN services s ON cs.sid=s.sid WHERE cs.id=" + id;
                var ds = fn.GetData(sql);
                if (ds.Tables[0].Rows.Count == 0) return;
                var r = ds.Tables[0].Rows[0];

                int bid = Convert.ToInt32(r["bid"]);
                int sid = Convert.ToInt32(r["sid"]);
                LoadCustomerCombo(bid);

                // Select booking
                int bidIdx = _bidList.IndexOf(bid);
                if (bidIdx >= 0) fCustomer.SelectedIndex = bidIdx;

                // Select service
                int sidIdx = _sidList.IndexOf(sid);
                if (sidIdx >= 0) fService.SelectedIndex = sidIdx;

                fQty.Text = r["quantity"].ToString();
                if (DateTime.TryParse(r["used_date"].ToString(), out var ud)) fUsedDate.Value = ud;
            }

            CenterFormPanel();
            pnlForm.Visible = true;
            pnlForm.BringToFront();
        }

        // ── Save ──────────────────────────────────────────────────────

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (fCustomer.SelectedIndex < 0 || fCustomer.SelectedIndex >= _bidList.Count)
            { MessageBox.Show("Vui lòng chọn khách hàng.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (fService.SelectedIndex < 0 || fService.SelectedIndex >= _sidList.Count)
            { MessageBox.Show("Vui lòng chọn dịch vụ.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!int.TryParse(fQty.Text.Trim(), out int qty) || qty < 1)
            { MessageBox.Show("Số lượng phải là số nguyên dương.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            int  bid  = _bidList[fCustomer.SelectedIndex];
            int  sid  = _sidList[fService.SelectedIndex];
            string dt = fUsedDate.Value.ToString("yyyy-MM-dd");

            string sql;
            if (_editingId == -1)
                sql = "INSERT INTO customer_services (bid,sid,quantity,used_date) VALUES (" +
                      bid + "," + sid + "," + qty + ",'" + dt + "')";
            else
                sql = "UPDATE customer_services SET bid=" + bid + ",sid=" + sid +
                      ",quantity=" + qty + ",used_date='" + dt + "' WHERE id=" + _editingId;

            fn.SetData(sql, _editingId == -1 ? "Thêm thành công!" : "Cập nhật thành công!");
            pnlForm.Visible = false;
            LoadData();
        }

        // ── Delete ────────────────────────────────────────────────────

        private void DeleteRecord(int id, int rowIndex)
        {
            string service = dgv.Rows[rowIndex].Cells["colService"].Value?.ToString() ?? "";
            string name    = dgv.Rows[rowIndex].Cells["colName"].Value?.ToString()    ?? "";
            var confirm = MessageBox.Show(
                "Xóa dịch vụ \"" + service + "\" của khách " + name + "?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes) return;
            fn.SetData("DELETE FROM customer_services WHERE id=" + id, "Xóa thành công!");
            LoadData();
        }
    }
}
