using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quan_ly_KS
{
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Dashboard_Load(object sender, EventArgs e)
        {
            uC_AddRoom1.Visible = false;
            uC_CustomerRes1.Visible = false;
            uC_CheckOut1.Visible = false;
            uC_CustomerDetails1.Visible = false;
            uC_Emloyee1.Visible = false;
            uC_DichVu1.Visible = false;
            btnAddRoom.PerformClick();

        }

        private void btnAddRoom_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnAddRoom.Left + 50;
            uC_AddRoom1.Visible = true;
            uC_AddRoom1.BringToFront();
        }

        

        private void btnCustomerRes_Click(object sender, EventArgs e)
        {
            
            PanelMoving.Left = btnCustomerRes.Left + 60;
            uC_CustomerRes1.Visible = true;
            uC_CustomerRes1.BringToFront();

        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnCheckOut.Left + 50;
            
            uC_CheckOut1.Visible = true;
            uC_CheckOut1.BringToFront();
            uC_CheckOut1.ReloadData();

        }

        private void btnCustomerDetail_Click(object sender, EventArgs e)
        {
            PanelMoving.Left += btnCustomerDetail.Left + 10;
            uC_CustomerDetails1.Visible = true;
            uC_CustomerDetails1.BringToFront();
        }

        private void btnEmployee_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnEmployee.Left + 60;
            uC_Emloyee1.Visible = true;
            uC_Emloyee1.BringToFront();
        }

        private void btnDichVu_Click(object sender, EventArgs e)
        {
            PanelMoving.Left = btnDichVu.Left + 60;
            uC_DichVu1.Visible = true;
            uC_DichVu1.BringToFront();
        }
    }
}
