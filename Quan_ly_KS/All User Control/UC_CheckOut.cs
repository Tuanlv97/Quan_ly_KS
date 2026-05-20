using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_CheckOut : UserControl
    {
        function fn = new function();
        String query;
        private Label lblServicesSummary;

        public UC_CheckOut()
        {
            InitializeComponent();
            lblServicesSummary = new Label {
                Text = "",
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.SlateBlue,
                Location = new Point(60, 593),
                Size = new Size(1732, 22),
                TextAlign = ContentAlignment.MiddleRight,
                BackColor = Color.White
            };
            this.Controls.Add(lblServicesSummary);
        }

        private void UC_CheckOut_Load(object sender, EventArgs e)
        {
            query = "select customer.cid, customer.cname, customer.mobile, customer.nationality, customer.dob,customer.gender, customer.idproof, customer.address, customer.checkin, rooms.roomNo, rooms.roomType, rooms.bed, rooms.price from customer inner join rooms on customer.roomid = rooms.roomid where chekout = 'NO' ";
            DataSet ds = fn.GetData(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
        }

        //private void txtName_TextChanged(object sender, EventArgs e)
        //{
        //    query = "select customer.cid, customer.cname, customer.mobile, customer.nationality, customer.dob,customer.gender , customer.idproof, customer.address, customer.checkin, rooms.roomNo, rooms.roomType, rooms.bed, rooms.price from customer inner join rooms on customer.roomid = rooms.roomid where cname like '" + txtName + "%' and chekout = 'NO'";
        //    DataSet ds = fn.GetData(query);
        //    guna2DataGridView1.DataSource = ds.Tables[0];
        //}
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            string searchText = txtName.Text.Trim();

            if (!string.IsNullOrEmpty(searchText))
            {
                query = "select customer.cid, customer.cname, customer.mobile, customer.nationality, customer.dob, " +
                        "customer.gender, customer.idproof, customer.address, customer.checkin, rooms.roomNo, " +
                        "rooms.roomType, rooms.bed, rooms.price " +
                        "from customer inner join rooms on customer.roomid = rooms.roomid " +
                        "where customer.cname like '" + searchText + "%' and chekout = 'NO'";
            }
            else
            {
                // Nếu text rỗng thì load lại toàn bộ dữ liệu gốc
                query = "select customer.cid, customer.cname, customer.mobile, customer.nationality, customer.dob, " +
                        "customer.gender, customer.idproof, customer.address, customer.checkin, rooms.roomNo, " +
                        "rooms.roomType, rooms.bed, rooms.price " +
                        "from customer inner join rooms on customer.roomid = rooms.roomid " +
                        "where chekout = 'NO'";
            }

            DataSet ds = fn.GetData(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
        }

        int cId;
        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
            //guna2DataGridView1.DataSource = ds.Tables[0];
            if (guna2DataGridView1.Rows[e.RowIndex].Cells[e.RowIndex].Value != null)
            {
                cId = int.Parse(guna2DataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                txtCName.Text = guna2DataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtRoom.Text = guna2DataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                // Calculate and show total
                try {
                    long roomPrice = Convert.ToInt64(guna2DataGridView1.Rows[e.RowIndex].Cells[12].Value);
                    var dsSvc = fn.GetData("SELECT ISNULL(SUM(cs.quantity * s.price), 0) AS svc_total FROM customer_services cs INNER JOIN services s ON cs.sid=s.sid WHERE cs.cid=" + cId);
                    long svcTotal = Convert.ToInt64(dsSvc.Tables[0].Rows[0]["svc_total"]);
                    long grandTotal = roomPrice + svcTotal;
                    lblServicesSummary.Text = string.Format("Tiền phòng: {0:N0} đ   |   Dịch vụ: {1:N0} đ   |   Tổng thanh toán: {2:N0} đ", roomPrice, svcTotal, grandTotal);
                } catch { lblServicesSummary.Text = ""; }
                //guna2DataGridView1.CurrentRow.Selected = true;
                //txtCId.Text = guna2DataGridView1.Rows[e.RowIndex].Cells["cid"].Value.ToString();
                //query = "select customer.cid, customer.cname, customer.mobile, customer.nationality, customer.dob,customer.gender, customer.idproof, customer.address, customer.checkin, rooms.roomNo, rooms.roomType, rooms.bed, rooms.price from customer inner join rooms on customer.roomid = rooms.roomid where chekout = 'NO' ";
                //DataSet ds = fn.GetData(query);
                //guna2DataGridView1.DataSource = ds.Tables[0];
            }
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            if (txtCName.Text != "")
            {
                if (MessageBox.Show("Ban co chac chan khong?", "Xac Nhan", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    String cdate = txtCheckOutDate.Text;
                    query = "update customer set chekout = 'YES', checkout = '" + cdate + "' where cid = " + cId + " update rooms set booked = 'NO' where roomNo = '" + txtRoom.Text + "'";
                    fn.SetData(query, "Check Out Thanh Cong.");
                    UC_CheckOut_Load(this, null);
                    clearAll();
                }
            }
            else
            {
                MessageBox.Show("Vui long chon khach hang de check out.", "Thong Bao", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void clearAll()
        {
            txtRoom.Clear();
            txtCName.Clear();
            txtName.Clear();
            txtCheckOutDate.ResetText();
            if (lblServicesSummary != null) lblServicesSummary.Text = "";
        }

        private void btnCheckOut_Leave(object sender, EventArgs e)
        {
            clearAll();
        }

        public void ReloadData()
        {
            string query = "select customer.cid, customer.cname, customer.mobile, customer.nationality, customer.dob, " +
                           "customer.gender, customer.idproof, customer.address, customer.checkin, rooms.roomNo, " +
                           "rooms.roomType, rooms.bed, rooms.price " +
                           "from customer inner join rooms on customer.roomid = rooms.roomid " +
                           "where chekout = 'NO'";

            DataSet ds = fn.GetData(query);
            guna2DataGridView1.DataSource = ds.Tables[0];
        }

    }
}
