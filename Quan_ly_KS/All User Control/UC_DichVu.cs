using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_DichVu : UserControl
    {
        function fn = new function();
        private int editingId = -1;

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

            LoadData();
        }

        private void SetupDataGridView()
        {
            dgvServices.Columns.Clear();
            dgvServices.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSTT", HeaderText = "STT", Width = 60 });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colId", HeaderText = "Mã dịch vụ", Width = 120 });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colName", HeaderText = "Tên dịch vụ", MinimumWidth = 200 });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrice", HeaderText = "Giá dịch vụ", Width = 180 });
            dgvServices.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus", HeaderText = "Trạng thái", Width = 180 });

            dgvServices.Columns.Add(new DataGridViewButtonColumn { Name = "colView", HeaderText = "Xem", Text = "👁", UseColumnTextForButtonValue = true, Width = 70 });
            dgvServices.Columns.Add(new DataGridViewButtonColumn { Name = "colEdit", HeaderText = "Sửa", Text = "✏", UseColumnTextForButtonValue = true, Width = 70 });
            dgvServices.Columns.Add(new DataGridViewButtonColumn { Name = "colDelete", HeaderText = "Xóa", Text = "🗑", UseColumnTextForButtonValue = true, Width = 70 });

            dgvServices.Columns["colName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvServices.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvServices.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(132, 112, 255);
            dgvServices.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvServices.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvServices.EnableHeadersVisualStyles = false;
            dgvServices.RowTemplate.Height = 45;
            dgvServices.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvServices.DefaultCellStyle.SelectionBackColor = Color.FromArgb(200, 191, 255);
            dgvServices.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvServices.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 244, 255);
        }

        public void LoadData()
        {
            string search = txtSearch.Text.Trim();
            string statusFilter = cmbFilterStatus.SelectedItem?.ToString();

            string sql = "SELECT sid, serviceName, price, status FROM services WHERE 1=1";
            if (!string.IsNullOrEmpty(search))
                sql += " AND serviceName LIKE '%" + search.Replace("'", "''") + "%'";
            if (statusFilter != "Tất cả trạng thái" && !string.IsNullOrEmpty(statusFilter))
                sql += " AND status = '" + statusFilter + "'";
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

            lblCount.Text = string.Format("Hiển thị {0} dịch vụ", dgvServices.Rows.Count);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            editingId = -1;
            txtServiceName.Clear();
            txtPrice.Clear();
            cmbFormStatus.SelectedIndex = 0;
            lblFormTitle.Text = "Thêm Dịch Vụ";
            pnlForm.Visible = true;
            pnlForm.BringToFront();
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

            if (e.ColumnIndex == dgvServices.Columns["colView"].Index)
            {
                DataSet ds = fn.GetData("SELECT * FROM services WHERE sid=" + sid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    MessageBox.Show(
                        string.Format("Mã DV: {0}\nTên: {1}\nGiá: {2:N0} VND\nTrạng thái: {3}",
                            row["sid"], row["serviceName"], row["price"], row["status"]),
                        "Chi tiết dịch vụ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if (e.ColumnIndex == dgvServices.Columns["colEdit"].Index)
            {
                DataSet ds = fn.GetData("SELECT * FROM services WHERE sid=" + sid);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    editingId = sid;
                    txtServiceName.Text = row["serviceName"].ToString();
                    txtPrice.Text = row["price"].ToString();
                    cmbFormStatus.SelectedItem = row["status"].ToString();
                    lblFormTitle.Text = "Cập Nhật Dịch Vụ";
                    pnlForm.Visible = true;
                    pnlForm.BringToFront();
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
