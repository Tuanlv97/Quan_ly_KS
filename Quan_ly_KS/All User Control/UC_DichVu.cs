using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_DichVu : UserControl
    {
        function fn = new function();
        private int editingId = -1;
        private bool _viewMode = false;
        private Label btnX;

        public UC_DichVu()
        {
            InitializeComponent();
        }

        private void UC_DichVu_Load(object sender, EventArgs e)
        {
            SetupDataGridView();

            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.AddRange(new string[] { "Tất cả trạng thái", "Đang cung cấp", "Ngừng cung cấp" });
            cmbFilterStatus.SelectedIndex = 0;

            cmbFormStatus.Items.Clear();
            cmbFormStatus.Items.AddRange(new string[] { "Đang cung cấp", "Ngừng cung cấp" });
            cmbFormStatus.SelectedIndex = 0;

            Action layoutHeader = () => {
                if (pnlHeader.Width < 300) return;
                btnAdd.Left          = pnlHeader.Width - btnAdd.Width - 24;
                cmbFilterStatus.Left = btnAdd.Left - cmbFilterStatus.Width - 12;
                txtSearch.Left       = cmbFilterStatus.Left - txtSearch.Width - 12;
            };
            pnlHeader.Resize        += (s, ev) => layoutHeader();
            pnlHeader.HandleCreated += (s, ev) => layoutHeader();

            this.Resize += (s, ev) => CenterFormPanel();

            // Nút X đóng form — góc trên phải
            btnX = new Label {
                Text      = "X",
                Size      = new Size(28, 28),
                Top       = 8,
                Left      = pnlForm.Width - 28 - 8,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(80, 80, 90),
                BackColor = Color.FromArgb(225, 225, 232),
                Font      = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor    = Cursors.Hand
            };
            btnX.Click += (s, ev) => pnlForm.Visible = false;
            pnlForm.Controls.Add(btnX);

            LoadData();
            CenterFormPanel();
            layoutHeader();
        }

        private void CenterFormPanel()
        {
            if (pnlForm == null) return;
            pnlForm.Left = (Width  - pnlForm.Width)  / 2;
            pnlForm.Top  = (Height - pnlForm.Height) / 2;
        }

        private void SetupDataGridView()
        {
            dgvServices.Columns.Clear();
            dgvServices.EditMode            = DataGridViewEditMode.EditProgrammatically;
            dgvServices.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSTT",    HeaderText = "STT",         FillWeight = 45  });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId",     HeaderText = "Mã dịch vụ", FillWeight = 80  });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName",   HeaderText = "Tên dịch vụ", FillWeight = 200 });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrice",  HeaderText = "Giá dịch vụ", FillWeight = 130 });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Trạng thái",  FillWeight = 130 });
            dgvServices.Columns.Add(new DataGridViewButtonColumn  { Name = "colView",   HeaderText = "", Text = "👁", UseColumnTextForButtonValue = true, FillWeight = 45 });
            dgvServices.Columns.Add(new DataGridViewButtonColumn  { Name = "colEdit",   HeaderText = "", Text = "✏", UseColumnTextForButtonValue = true, FillWeight = 45 });
            dgvServices.Columns.Add(new DataGridViewButtonColumn  { Name = "colDelete", HeaderText = "", Text = "🗑", UseColumnTextForButtonValue = true, FillWeight = 45 });

            dgvServices.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvServices.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(132, 112, 255);
            dgvServices.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvServices.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvServices.ColumnHeadersHeight                     = 42;
            dgvServices.EnableHeadersVisualStyles               = false;
            dgvServices.RowTemplate.Height                      = 45;
            dgvServices.DefaultCellStyle.Font                   = new Font("Segoe UI", 10);
            dgvServices.DefaultCellStyle.SelectionBackColor     = Color.FromArgb(200, 191, 255);
            dgvServices.DefaultCellStyle.SelectionForeColor     = Color.Black;
            dgvServices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 244, 255);
        }

        public void LoadData()
        {
            string search = txtSearch.Text.Trim();
            string statusFilter = cmbFilterStatus.SelectedItem?.ToString();

            string sql = "SELECT sid, serviceName, price, status FROM services WHERE 1=1";
            if (!string.IsNullOrEmpty(search))
                sql += " AND serviceName LIKE N'%" + search.Replace("'", "''") + "%'";
            if (statusFilter != "Tất cả trạng thái" && !string.IsNullOrEmpty(statusFilter))
                sql += " AND status = N'" + statusFilter.Replace("'", "''") + "'";
            sql += " ORDER BY sid";

            DataSet ds = fn.GetData(sql);
            dgvServices.Rows.Clear();

            int stt = 1;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                int idx = dgvServices.Rows.Add(
                    stt++,
                    row["sid"],
                    row["serviceName"],
                    string.Format("{0:N0} VND", row["price"]),
                    row["status"],
                    "👁", "✏", "🗑"
                );

                var statusCell = dgvServices.Rows[idx].Cells["colStatus"];
                if (row["status"].ToString() == "Đang cung cấp")
                {
                    statusCell.Style.ForeColor = Color.White;
                    statusCell.Style.BackColor = Color.MediumSeaGreen;
                    statusCell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
                else
                {
                    statusCell.Style.ForeColor = Color.White;
                    statusCell.Style.BackColor = Color.OrangeRed;
                    statusCell.Style.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            }

            lblBreadcrumb.Text = string.Format("Quản lý dịch vụ  /  Danh sách dịch vụ   –   {0} dịch vụ", dgvServices.Rows.Count);
        }

        private void OpenForm(bool viewOnly)
        {
            _viewMode = viewOnly;
            txtServiceName.Enabled = !viewOnly;
            txtPrice.Enabled       = !viewOnly;
            cmbFormStatus.Enabled  = !viewOnly;
            btnSave.Visible        = !viewOnly;
            btnCancel.Text         = viewOnly ? "Đóng" : "Hủy";
            // Xem: nút Đóng sát phải; Sửa/Thêm: nút Hủy cạnh Lưu
            btnCancel.Left = viewOnly
                ? pnlForm.Width - btnCancel.Width - 18
                : 178;
            CenterFormPanel();
            pnlForm.Visible = true;
            pnlForm.BringToFront();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            editingId = -1;
            txtServiceName.Clear();
            txtPrice.Clear();
            cmbFormStatus.SelectedIndex = 0;
            lblFormTitle.Text = "Thêm Dịch Vụ";
            OpenForm(viewOnly: false);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtServiceName.Text) || string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!long.TryParse(txtPrice.Text.Trim(), out long price) || price <= 0)
            {
                MessageBox.Show("Giá dịch vụ phải là số nguyên dương!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string name = txtServiceName.Text.Trim().Replace("'", "''");
            string status = cmbFormStatus.SelectedItem.ToString();
            string sql, message;

            if (editingId == -1)
            {
                sql = string.Format("INSERT INTO services (serviceName, price, status) VALUES (N'{0}', {1}, N'{2}')", name, price, status);
                message = "Thêm dịch vụ thành công!";
            }
            else
            {
                sql = string.Format("UPDATE services SET serviceName=N'{0}', price={1}, status=N'{2}' WHERE sid={3}", name, price, status, editingId);
                message = "Cập nhật dịch vụ thành công!";
            }

            fn.SetData(sql, message);
            pnlForm.Visible = false;
            LoadData();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
        }

        private void dgvServices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int sid = Convert.ToInt32(dgvServices.Rows[e.RowIndex].Cells["colId"].Value);

            if (e.ColumnIndex == dgvServices.Columns["colView"].Index ||
                e.ColumnIndex == dgvServices.Columns["colEdit"].Index)
            {
                bool isView = e.ColumnIndex == dgvServices.Columns["colView"].Index;
                DataSet ds = fn.GetData("SELECT * FROM services WHERE sid=" + sid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    editingId = isView ? -1 : sid;
                    txtServiceName.Text        = row["serviceName"].ToString();
                    txtPrice.Text              = row["price"].ToString();
                    cmbFormStatus.SelectedItem = row["status"].ToString();
                    lblFormTitle.Text          = isView ? "Chi Tiết Dịch Vụ" : "Cập Nhật Dịch Vụ";
                    OpenForm(viewOnly: isView);
                }
            }
            else if (e.ColumnIndex == dgvServices.Columns["colDelete"].Index)
            {
                if (MessageBox.Show("Bạn có chắc muốn xóa dịch vụ này?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    fn.SetData("DELETE FROM services WHERE sid=" + sid, "Xóa dịch vụ thành công!");
                    LoadData();
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cmbFilterStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}
