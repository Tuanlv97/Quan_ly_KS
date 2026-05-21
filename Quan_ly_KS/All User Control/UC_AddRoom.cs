using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_AddRoom : UserControl
    {
        private function fn = new function();
        private int editingId = -1;

        public UC_AddRoom()
        {
            InitializeComponent();
        }

        private void UC_AddRoom_Load(object sender, EventArgs e)
        {
            SetupGrid();

            cmbFilterStatus.Items.Clear();
            cmbFilterStatus.Items.AddRange(new string[] { "Tất cả trạng thái", "Trống", "Có khách", "Bẩn", "Bảo trì" });
            cmbFilterStatus.SelectedIndex = 0;

            cmbFormStatus.Items.Clear();
            cmbFormStatus.Items.AddRange(new string[] { "Trống", "Có khách", "Bẩn", "Bảo trì" });
            cmbFormStatus.SelectedIndex = 0;

            cmbFormRoomType.Items.Clear();
            cmbFormRoomType.Items.AddRange(new string[] { "Ac", "Non-Ac" });
            cmbFormRoomType.SelectedIndex = 0;

            cmbFormBed.Items.Clear();
            cmbFormBed.Items.AddRange(new string[] { "Single", "Double", "Triple" });
            cmbFormBed.SelectedIndex = 0;

            LoadData();
        }

        private void SetupGrid()
        {
            dgvRooms.Columns.Clear();
            dgvRooms.EditMode = DataGridViewEditMode.EditProgrammatically;

            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRawId",    HeaderText = "",                Visible = false });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSTT",      HeaderText = "STT",             Width = 60 });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRoomNo",   HeaderText = "Số Phòng",        MinimumWidth = 150 });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn { Name = "colRoomType", HeaderText = "Loại Phòng",      Width = 160 });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn { Name = "colBed",      HeaderText = "Loại Giường",     Width = 160 });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn { Name = "colPrice",    HeaderText = "Giá Phòng (VNĐ)", Width = 220 });
            dgvRooms.Columns.Add(new DataGridViewTextBoxColumn { Name = "colStatus",   HeaderText = "Trạng Thái",      Width = 160 });
            dgvRooms.Columns.Add(new DataGridViewButtonColumn  { Name = "colEdit",     HeaderText = "Sửa",   Text = "✏", UseColumnTextForButtonValue = true, Width = 70 });
            dgvRooms.Columns.Add(new DataGridViewButtonColumn  { Name = "colDelete",   HeaderText = "Xóa",   Text = "🗑", UseColumnTextForButtonValue = true, Width = 70 });

            dgvRooms.Columns["colRoomNo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dgvRooms.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRooms.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(132, 112, 255);
            dgvRooms.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRooms.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRooms.EnableHeadersVisualStyles               = false;
            dgvRooms.RowTemplate.Height                      = 45;
            dgvRooms.DefaultCellStyle.Font                   = new Font("Segoe UI", 10);
            dgvRooms.DefaultCellStyle.SelectionBackColor     = Color.FromArgb(200, 191, 255);
            dgvRooms.DefaultCellStyle.SelectionForeColor     = Color.Black;
            dgvRooms.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 244, 255);
        }

        public void LoadData()
        {
            string search       = txtSearch.Text.Trim();
            string statusFilter = cmbFilterStatus.SelectedItem?.ToString();

            string sql = "SELECT roomid, roomNo, roomType, bed, price, status FROM rooms WHERE 1=1";
            if (!string.IsNullOrEmpty(search))
                sql += " AND roomNo LIKE N'%" + search.Replace("'", "''") + "%'";
            if (statusFilter != "Tất cả trạng thái" && !string.IsNullOrEmpty(statusFilter))
                sql += " AND status = N'" + statusFilter.Replace("'", "''") + "'";
            sql += " ORDER BY roomNo";

            try
            {
                DataSet ds = fn.GetData(sql);
                dgvRooms.Rows.Clear();

                int stt = 1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    int idx = dgvRooms.Rows.Add(
                        row["roomid"],
                        stt++,
                        row["roomNo"],
                        row["roomType"],
                        row["bed"],
                        string.Format("{0:N0} VNĐ", row["price"]),
                        row["status"],
                        "✏", "🗑"
                    );

                    // Màu badge theo trạng thái
                    var statusCell = dgvRooms.Rows[idx].Cells["colStatus"];
                    string status  = row["status"].ToString();
                    switch (status)
                    {
                        case "Trống":
                            statusCell.Style.BackColor = Color.MediumSeaGreen;
                            statusCell.Style.ForeColor = Color.White;
                            statusCell.Style.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
                            break;
                        case "Có khách":
                            statusCell.Style.BackColor = Color.DodgerBlue;
                            statusCell.Style.ForeColor = Color.White;
                            statusCell.Style.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
                            break;
                        case "Bẩn":
                            statusCell.Style.BackColor = Color.DarkOrange;
                            statusCell.Style.ForeColor = Color.White;
                            statusCell.Style.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
                            break;
                        case "Bảo trì":
                            statusCell.Style.BackColor = Color.Crimson;
                            statusCell.Style.ForeColor = Color.White;
                            statusCell.Style.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
                            break;
                    }
                }

                lblCount.Text = string.Format("Hiển thị {0} phòng", dgvRooms.Rows.Count);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            editingId = -1;
            txtFormRoomNo.Clear();
            txtFormPrice.Clear();
            cmbFormRoomType.SelectedIndex = 0;
            cmbFormBed.SelectedIndex      = 0;
            cmbFormStatus.SelectedIndex   = 0;
            lblFormTitle.Text             = "Thêm Phòng";
            pnlForm.Visible = true;
            pnlForm.BringToFront();
            txtFormRoomNo.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string roomNo   = txtFormRoomNo.Text.Trim();
            string roomType = cmbFormRoomType.SelectedItem?.ToString() ?? "";
            string bed      = cmbFormBed.SelectedItem?.ToString() ?? "";
            string priceStr = txtFormPrice.Text.Trim();
            string status   = cmbFormStatus.SelectedItem?.ToString() ?? "Trống";

            if (string.IsNullOrEmpty(roomNo) || string.IsNullOrEmpty(priceStr))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!long.TryParse(priceStr, out long price) || price <= 0)
            {
                MessageBox.Show("Giá phòng phải là số nguyên dương!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string safeRoomNo = roomNo.Replace("'", "''");
            string sql, message;

            if (editingId == -1)
            {
                sql = string.Format(
                    "INSERT INTO rooms (roomNo,roomType,bed,price,status) VALUES (N'{0}',N'{1}',N'{2}',{3},N'{4}')",
                    safeRoomNo, roomType, bed, price, status);
                message = "Thêm phòng thành công!";
            }
            else
            {
                sql = string.Format(
                    "UPDATE rooms SET roomNo=N'{0}',roomType=N'{1}',bed=N'{2}',price={3},status=N'{4}' WHERE roomid={5}",
                    safeRoomNo, roomType, bed, price, status, editingId);
                message = "Cập nhật phòng thành công!";
            }

            try
            {
                fn.SetData(sql, message);
                pnlForm.Visible = false;
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlForm.Visible = false;
        }

        private void dgvRooms_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int roomId = Convert.ToInt32(dgvRooms.Rows[e.RowIndex].Cells["colRawId"].Value);

            if (e.ColumnIndex == dgvRooms.Columns["colEdit"].Index)
            {
                try
                {
                    DataSet ds = fn.GetData("SELECT * FROM rooms WHERE roomid=" + roomId);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DataRow row   = ds.Tables[0].Rows[0];
                        editingId     = roomId;
                        txtFormRoomNo.Text            = row["roomNo"].ToString();
                        txtFormPrice.Text             = row["price"].ToString();
                        cmbFormRoomType.SelectedItem  = row["roomType"].ToString();
                        cmbFormBed.SelectedItem       = row["bed"].ToString();
                        string curStatus              = row["status"].ToString();
                        cmbFormStatus.SelectedItem    = cmbFormStatus.Items.Contains(curStatus) ? curStatus : "Trống";
                        lblFormTitle.Text             = "Cập Nhật Phòng";
                        pnlForm.Visible = true;
                        pnlForm.BringToFront();
                        txtFormRoomNo.Focus();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.ColumnIndex == dgvRooms.Columns["colDelete"].Index)
            {
                string roomNo = dgvRooms.Rows[e.RowIndex].Cells["colRoomNo"].Value?.ToString();
                if (MessageBox.Show(
                        string.Format("Bạn có chắc muốn xóa phòng '{0}'?", roomNo),
                        "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question
                    ) == DialogResult.Yes)
                {
                    try
                    {
                        fn.SetData("DELETE FROM rooms WHERE roomid=" + roomId, "Xóa phòng thành công!");
                        LoadData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

        // Refresh khi người dùng quay lại màn hình này
        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible && dgvRooms.Columns.Count > 0)
            {
                pnlForm.Visible = false;
                LoadData();
            }
        }
    }
}
