using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quan_ly_KS.All_User_Control
{
    public partial class UC_CustomerRes : UserControl
    {
        function fn = new function();
        String query;
        public UC_CustomerRes()
        {
            InitializeComponent();
        }

        public void setComboBox(String query, ComboBox combo)
        {
            SqlDataReader sdr = fn.GetForCombo(query);
            while (sdr.Read())
            {
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    combo.Items.Add(sdr.GetString(i));
                }
            }
            sdr.Close();
        }
        private void UC_CustomerRes_Load(object sender, EventArgs e)
        {
            
        }

        private void txtRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRoomNo.Items.Clear();
            query = "select roomNo from rooms where bed = '" + txtBed.Text + "' and roomType =  '" + txtRoom.Text + "' and booked = 'NO' ";
            setComboBox(query, txtRoomNo);
        }

        private void txtBed_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtRoom.SelectedIndex = -1;
            txtRoomNo.Items.Clear();
            txtPrice.Clear();
        }
        int rid;
        private void txtRoomNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            query = "select price, roomid from rooms where roomNo = '" + txtRoomNo.Text + "'";
            DataSet ds = fn.GetData(query);
            txtPrice.Text = ds.Tables[0].Rows[0][0].ToString();

            rid = int.Parse(ds.Tables[0].Rows[0][1].ToString());
        }

        private void btnAllotCustomer_Click(object sender, EventArgs e)
        {
            if (txtName.Text != "" && txtContact.Text != "" && txtNationality.Text != "" && txtGender.Text != ""
                && txtBed.Text != "" && txtRoom.Text != "" && txtRoomNo.Text != "" && txtPrice.Text != "" && txtIdProff.Text != ""
                && txtDob.Text != "" && txtAddress.Text != "" && txtRoom.Text != "" && txtCheckin.Text != "")
            {
                String cname = txtName.Text;
                Int64 mobile = Int64.Parse(txtContact.Text);
                String nationality = txtNationality.Text;
                String gender = txtGender.Text;
                String dob = txtDob.Text;
                String idproof = txtIdProff.Text;
                String address = txtAddress.Text;
                String checkin = txtCheckin.Text;
                //String bed = txtBed.Text;
                //String roomtype = txtRoom.Text;
                //String roomno = txtRoomNo.Text;
                //Int64 price = Int64.Parse(txtPrice.Text);

                query = "insert into customer (cname, mobile, nationality, gender, dob, idproof, address, checkin, roomid) values ('" + cname + "', " + mobile + ", '" + nationality + "', '" + gender + "', '" + dob + "', '" + idproof + "', '" + address + "', '" + checkin + "', '" + rid + "') update rooms set booked = 'YES' where roomNo = '" + txtRoomNo.Text + "'";

                fn.SetData(query, "Customer Alloted Room Successfully.");
                clearAll();

            }
            else
            {
                MessageBox.Show(" Vui lòng điền đầy đủ thông tin. ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void clearAll()
        {
            txtName.Clear();
            txtContact.Clear();
            txtNationality.Clear();
            txtGender.SelectedIndex = -1;
            txtDob.Value = DateTime.Now;
            txtIdProff.Clear();
            txtAddress.Clear();
            txtCheckin.Value = DateTime.Now;
            txtBed.SelectedIndex = -1;
            txtRoom.SelectedIndex = -1;
            txtRoomNo.Items.Clear();
            txtPrice.Clear();
        }

        private void UC_CustomerRes_Leave(object sender, EventArgs e)
        {
            clearAll();
        }

        
    }
}
