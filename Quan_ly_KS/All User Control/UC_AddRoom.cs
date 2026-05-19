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
    
    public partial class UC_AddRoom : UserControl
    {
        function fn = new function();
        String sql;
        public UC_AddRoom()
        {
            InitializeComponent();
        }

        private void UC_AddRoom_Load(object sender, EventArgs e)
        {
            sql = "select * from rooms";
            DataSet ds = fn.GetData(sql);

            DataGridView1.DataSource = ds.Tables[0];
        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            if (txtRoomNo.Text != "" && txtRoomType.Text != "" && txtBed.Text != "" && txtPrice.Text != "")
            {
                String roomNo = txtRoomNo.Text;
                String roomType = txtRoomType.Text;
                String bed = txtBed.Text;
                Int64 price = Int64.Parse(txtPrice.Text);
                sql = "insert into rooms (roomNo,roomType,bed,price) values ('" + roomNo + "','" + roomType + "','" + bed + "'," + price + ")";
                fn.SetData(sql, "Room Added.");
                UC_AddRoom_Load(this, null);
                clearAll();
            }
            else
            {
                MessageBox.Show("Please fill all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public void clearAll()
        {
            txtRoomNo.Clear();
            txtRoomType.SelectedIndex = -1;
            txtBed.SelectedIndex = -1;
            txtPrice.Clear();
        }

        private void UC_AddRoom_Leave(object sender, EventArgs e)
        {
            clearAll();
        }

        private void UC_AddRoom_Enter(object sender, EventArgs e)
        {
            UC_AddRoom_Load(this, null);
        }
    }
}
