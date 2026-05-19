namespace Quan_ly_KS.All_User_Control
{
    partial class UC_DichVu
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblBreadcrumb = new System.Windows.Forms.Label();
            this.lblCount = new System.Windows.Forms.Label();
            this.btnAdd = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbFilterStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dgvServices = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.txtServiceName = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblPrice = new System.Windows.Forms.Label();
            this.txtPrice = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.cmbFormStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnSave = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).BeginInit();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblTitle.Location = new System.Drawing.Point(20, 18);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Text = "Danh Sách Dịch Vụ";

            // lblBreadcrumb
            this.lblBreadcrumb.AutoSize = true;
            this.lblBreadcrumb.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBreadcrumb.ForeColor = System.Drawing.Color.Gray;
            this.lblBreadcrumb.Location = new System.Drawing.Point(22, 62);
            this.lblBreadcrumb.Name = "lblBreadcrumb";
            this.lblBreadcrumb.Text = "Quản lý dịch vụ  /  Danh sách dịch vụ";

            // lblCount
            this.lblCount.AutoSize = true;
            this.lblCount.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCount.ForeColor = System.Drawing.Color.Gray;
            this.lblCount.Location = new System.Drawing.Point(22, 795);
            this.lblCount.Name = "lblCount";
            this.lblCount.Text = "";

            // btnAdd
            this.btnAdd.BorderRadius = 8;
            this.btnAdd.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAdd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAdd.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnAdd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnAdd.FillColor = System.Drawing.Color.SlateBlue;
            this.btnAdd.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.Location = new System.Drawing.Point(1640, 22);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(200, 45);
            this.btnAdd.Text = "+ Thêm dịch vụ";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // txtSearch
            this.txtSearch.BorderRadius = 8;
            this.txtSearch.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtSearch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtSearch.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location = new System.Drawing.Point(1150, 26);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm kiếm dịch vụ...";
            this.txtSearch.Size = new System.Drawing.Size(250, 40);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // cmbFilterStatus
            this.cmbFilterStatus.BorderRadius = 8;
            this.cmbFilterStatus.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.cmbFilterStatus.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.cmbFilterStatus.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.cmbFilterStatus.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbFilterStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFilterStatus.Location = new System.Drawing.Point(1415, 26);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(210, 40);
            this.cmbFilterStatus.SelectedIndexChanged += new System.EventHandler(this.cmbFilterStatus_SelectedIndexChanged);

            // dgvServices
            this.dgvServices.AllowUserToAddRows = false;
            this.dgvServices.AllowUserToDeleteRows = false;
            this.dgvServices.BackgroundColor = System.Drawing.Color.White;
            this.dgvServices.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvServices.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvServices.Location = new System.Drawing.Point(20, 90);
            this.dgvServices.Name = "dgvServices";
            this.dgvServices.RowHeadersVisible = false;
            this.dgvServices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServices.Size = new System.Drawing.Size(1840, 695);
            this.dgvServices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvServices_CellContentClick);

            // pnlForm
            this.pnlForm.BackColor = System.Drawing.Color.White;
            this.pnlForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlForm.Controls.Add(this.lblFormTitle);
            this.pnlForm.Controls.Add(this.lblName);
            this.pnlForm.Controls.Add(this.txtServiceName);
            this.pnlForm.Controls.Add(this.lblPrice);
            this.pnlForm.Controls.Add(this.txtPrice);
            this.pnlForm.Controls.Add(this.lblStatus);
            this.pnlForm.Controls.Add(this.cmbFormStatus);
            this.pnlForm.Controls.Add(this.btnSave);
            this.pnlForm.Controls.Add(this.btnCancel);
            this.pnlForm.Location = new System.Drawing.Point(641, 180);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Size = new System.Drawing.Size(600, 410);
            this.pnlForm.Visible = false;

            // lblFormTitle
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblFormTitle.Location = new System.Drawing.Point(20, 20);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Text = "Thêm Dịch Vụ";

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblName.Location = new System.Drawing.Point(20, 78);
            this.lblName.Name = "lblName";
            this.lblName.Text = "Tên dịch vụ";

            // txtServiceName
            this.txtServiceName.BorderRadius = 8;
            this.txtServiceName.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.txtServiceName.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtServiceName.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtServiceName.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtServiceName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtServiceName.Location = new System.Drawing.Point(20, 103);
            this.txtServiceName.Name = "txtServiceName";
            this.txtServiceName.PlaceholderText = "Nhập tên dịch vụ";
            this.txtServiceName.Size = new System.Drawing.Size(555, 40);

            // lblPrice
            this.lblPrice.AutoSize = true;
            this.lblPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblPrice.Location = new System.Drawing.Point(20, 160);
            this.lblPrice.Name = "lblPrice";
            this.lblPrice.Text = "Giá dịch vụ (VND)";

            // txtPrice
            this.txtPrice.BorderRadius = 8;
            this.txtPrice.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.txtPrice.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtPrice.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtPrice.FillColor = System.Drawing.Color.WhiteSmoke;
            this.txtPrice.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPrice.Location = new System.Drawing.Point(20, 185);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.PlaceholderText = "Nhập giá (VD: 50000)";
            this.txtPrice.Size = new System.Drawing.Size(555, 40);

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblStatus.Location = new System.Drawing.Point(20, 242);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Text = "Trạng thái";

            // cmbFormStatus
            this.cmbFormStatus.BorderRadius = 8;
            this.cmbFormStatus.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.cmbFormStatus.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.cmbFormStatus.DisabledState.ForeColor = System.Drawing.Color.FromArgb(138, 138, 138);
            this.cmbFormStatus.FillColor = System.Drawing.Color.WhiteSmoke;
            this.cmbFormStatus.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormStatus.Location = new System.Drawing.Point(20, 267);
            this.cmbFormStatus.Name = "cmbFormStatus";
            this.cmbFormStatus.Size = new System.Drawing.Size(555, 40);

            // btnSave
            this.btnSave.BorderRadius = 8;
            this.btnSave.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnSave.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnSave.FillColor = System.Drawing.Color.SlateBlue;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(20, 340);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 45);
            this.btnSave.Text = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.BorderRadius = 8;
            this.btnCancel.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnCancel.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnCancel.FillColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(80, 80, 80);
            this.btnCancel.Location = new System.Drawing.Point(200, 340);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(160, 45);
            this.btnCancel.Text = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // UC_DichVu
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.dgvServices);
            this.Controls.Add(this.cmbFilterStatus);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblBreadcrumb);
            this.Controls.Add(this.lblTitle);
            this.Name = "UC_DichVu";
            this.Size = new System.Drawing.Size(1882, 852);
            this.Load += new System.EventHandler(this.UC_DichVu_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).EndInit();
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblBreadcrumb;
        private System.Windows.Forms.Label lblCount;
        private Guna.UI2.WinForms.Guna2Button btnAdd;
        private Guna.UI2.WinForms.Guna2TextBox txtSearch;
        private Guna.UI2.WinForms.Guna2ComboBox cmbFilterStatus;
        private Guna.UI2.WinForms.Guna2DataGridView dgvServices;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblName;
        private Guna.UI2.WinForms.Guna2TextBox txtServiceName;
        private System.Windows.Forms.Label lblPrice;
        private Guna.UI2.WinForms.Guna2TextBox txtPrice;
        private System.Windows.Forms.Label lblStatus;
        private Guna.UI2.WinForms.Guna2ComboBox cmbFormStatus;
        private Guna.UI2.WinForms.Guna2Button btnSave;
        private Guna.UI2.WinForms.Guna2Button btnCancel;
    }
}
