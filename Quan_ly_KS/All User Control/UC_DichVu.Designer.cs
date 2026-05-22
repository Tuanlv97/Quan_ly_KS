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
            this.pnlHeader       = new System.Windows.Forms.Panel();
            this.pnlGrid         = new System.Windows.Forms.Panel();
            this.lblTitle        = new System.Windows.Forms.Label();
            this.lblBreadcrumb   = new System.Windows.Forms.Label();
            this.btnAdd          = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch       = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbFilterStatus = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dgvServices     = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlForm         = new Guna.UI2.WinForms.Guna2Panel();
            this.lblFormTitle    = new System.Windows.Forms.Label();
            this.lblName         = new System.Windows.Forms.Label();
            this.txtServiceName  = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblPrice        = new System.Windows.Forms.Label();
            this.txtPrice        = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblStatus       = new System.Windows.Forms.Label();
            this.cmbFormStatus   = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnSave         = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel       = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();

            // ── pnlHeader (Dock=Top) ──────────────────────────────────
            this.pnlHeader.Dock      = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height    = 90;
            this.pnlHeader.BackColor = System.Drawing.Color.White;
            this.pnlHeader.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblTitle, this.lblBreadcrumb,
                this.txtSearch, this.cmbFilterStatus, this.btnAdd
            });

            // lblTitle
            this.lblTitle.AutoSize  = true;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblTitle.Location  = new System.Drawing.Point(20, 10);
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.Text      = "Danh Sách Dịch Vụ";
            this.lblTitle.Anchor    = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;

            // lblBreadcrumb (updated in LoadData to include count)
            this.lblBreadcrumb.AutoSize  = true;
            this.lblBreadcrumb.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBreadcrumb.ForeColor = System.Drawing.Color.Gray;
            this.lblBreadcrumb.Location  = new System.Drawing.Point(20, 52);
            this.lblBreadcrumb.Name      = "lblBreadcrumb";
            this.lblBreadcrumb.Text      = "Quản lý dịch vụ  /  Danh sách dịch vụ";
            this.lblBreadcrumb.Anchor    = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;

            // btnAdd
            this.btnAdd.BorderRadius            = 8;
            this.btnAdd.DisabledState.FillColor = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnAdd.DisabledState.ForeColor = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnAdd.FillColor               = System.Drawing.Color.SlateBlue;
            this.btnAdd.Font                    = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor               = System.Drawing.Color.White;
            this.btnAdd.Location                = new System.Drawing.Point(20, 22);
            this.btnAdd.Name                    = "btnAdd";
            this.btnAdd.Size                    = new System.Drawing.Size(200, 45);
            this.btnAdd.Text                    = "+ Thêm dịch vụ";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // cmbFilterStatus
            this.cmbFilterStatus.BorderRadius = 8;
            this.cmbFilterStatus.FillColor    = System.Drawing.Color.WhiteSmoke;
            this.cmbFilterStatus.Font         = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFilterStatus.Location     = new System.Drawing.Point(20, 26);
            this.cmbFilterStatus.Name         = "cmbFilterStatus";
            this.cmbFilterStatus.Size         = new System.Drawing.Size(210, 40);
            this.cmbFilterStatus.SelectedIndexChanged += new System.EventHandler(this.cmbFilterStatus_SelectedIndexChanged);

            // txtSearch
            this.txtSearch.BorderRadius    = 8;
            this.txtSearch.FillColor       = System.Drawing.Color.WhiteSmoke;
            this.txtSearch.Font            = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location        = new System.Drawing.Point(20, 26);
            this.txtSearch.Name            = "txtSearch";
            this.txtSearch.PlaceholderText = "Tìm kiếm dịch vụ...";
            this.txtSearch.Size            = new System.Drawing.Size(220, 40);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // ── pnlGrid (Dock=Fill) ───────────────────────────────────
            this.pnlGrid.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Padding   = new System.Windows.Forms.Padding(20, 10, 20, 16);
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.Controls.Add(this.dgvServices);

            // dgvServices (Dock=Fill inside pnlGrid)
            this.dgvServices.AllowUserToAddRows          = false;
            this.dgvServices.AllowUserToDeleteRows       = false;
            this.dgvServices.AllowUserToResizeRows       = false;
            this.dgvServices.BackgroundColor             = System.Drawing.Color.White;
            this.dgvServices.BorderStyle                 = System.Windows.Forms.BorderStyle.None;
            this.dgvServices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvServices.Dock                        = System.Windows.Forms.DockStyle.Fill;
            this.dgvServices.Font                        = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvServices.Name                        = "dgvServices";
            this.dgvServices.RowHeadersVisible           = false;
            this.dgvServices.SelectionMode               = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvServices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvServices_CellContentClick);

            // ── pnlForm (overlay, centered via CenterFormPanel) ───────
            // 3 fields: formH = 48+17+2*56+35+14+40+16 = 282
            this.pnlForm.FillColor       = System.Drawing.Color.White;
            this.pnlForm.BorderRadius    = 12;
            this.pnlForm.BorderColor     = System.Drawing.Color.FromArgb(100, 132, 112, 255);
            this.pnlForm.BorderThickness = 2;
            this.pnlForm.Name            = "pnlForm";
            this.pnlForm.Size            = new System.Drawing.Size(500, 282);
            this.pnlForm.Visible         = false;
            this.pnlForm.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblFormTitle, this.lblName, this.txtServiceName,
                this.lblPrice, this.txtPrice,
                this.lblStatus, this.cmbFormStatus,
                this.btnSave, this.btnCancel
            });
            this.pnlForm.ShadowDecoration.Enabled = true;
            this.pnlForm.ShadowDecoration.Color   = System.Drawing.Color.FromArgb(55, 132, 112, 255);
            this.pnlForm.ShadowDecoration.Depth   = 14;

            // lblFormTitle
            this.lblFormTitle.AutoSize  = true;
            this.lblFormTitle.Font      = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblFormTitle.Location  = new System.Drawing.Point(18, 16);
            this.lblFormTitle.Name      = "lblFormTitle";
            this.lblFormTitle.Text      = "Thêm Dịch Vụ";

            // Tên dịch vụ  (field 0: label top=48, input top=65)
            this.lblName.AutoSize  = true;
            this.lblName.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblName.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblName.Location  = new System.Drawing.Point(18, 48);
            this.lblName.Name      = "lblName";
            this.lblName.Text      = "Tên dịch vụ";

            this.txtServiceName.BorderRadius    = 8;
            this.txtServiceName.FillColor       = System.Drawing.Color.WhiteSmoke;
            this.txtServiceName.Font            = new System.Drawing.Font("Segoe UI", 10F);
            this.txtServiceName.Location        = new System.Drawing.Point(18, 65);
            this.txtServiceName.Name            = "txtServiceName";
            this.txtServiceName.PlaceholderText = "Nhập tên dịch vụ";
            this.txtServiceName.Size            = new System.Drawing.Size(464, 35);

            // Giá dịch vụ  (field 1: label top=104, input top=121)
            this.lblPrice.AutoSize  = true;
            this.lblPrice.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblPrice.Location  = new System.Drawing.Point(18, 104);
            this.lblPrice.Name      = "lblPrice";
            this.lblPrice.Text      = "Giá dịch vụ (VNĐ)";

            this.txtPrice.BorderRadius    = 8;
            this.txtPrice.FillColor       = System.Drawing.Color.WhiteSmoke;
            this.txtPrice.Font            = new System.Drawing.Font("Segoe UI", 10F);
            this.txtPrice.Location        = new System.Drawing.Point(18, 121);
            this.txtPrice.Name            = "txtPrice";
            this.txtPrice.PlaceholderText = "Nhập giá (VD: 50000)";
            this.txtPrice.Size            = new System.Drawing.Size(464, 35);

            // Trạng thái  (field 2: label top=160, input top=177)
            this.lblStatus.AutoSize  = true;
            this.lblStatus.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblStatus.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblStatus.Location  = new System.Drawing.Point(18, 160);
            this.lblStatus.Name      = "lblStatus";
            this.lblStatus.Text      = "Trạng thái";

            this.cmbFormStatus.BorderRadius = 8;
            this.cmbFormStatus.FillColor    = System.Drawing.Color.WhiteSmoke;
            this.cmbFormStatus.Font         = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormStatus.Location     = new System.Drawing.Point(18, 177);
            this.cmbFormStatus.Name         = "cmbFormStatus";
            this.cmbFormStatus.Size         = new System.Drawing.Size(464, 35);

            // Buttons  (btnY = 177+35+14 = 226)
            this.btnSave.BorderRadius = 8;
            this.btnSave.FillColor    = System.Drawing.Color.SlateBlue;
            this.btnSave.Font         = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor    = System.Drawing.Color.White;
            this.btnSave.Location     = new System.Drawing.Point(18, 226);
            this.btnSave.Name         = "btnSave";
            this.btnSave.Size         = new System.Drawing.Size(150, 40);
            this.btnSave.Text         = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.BorderRadius = 8;
            this.btnCancel.FillColor    = System.Drawing.Color.FromArgb(220, 220, 220);
            this.btnCancel.Font         = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor    = System.Drawing.Color.FromArgb(80, 80, 80);
            this.btnCancel.Location     = new System.Drawing.Point(178, 226);
            this.btnCancel.Name         = "btnCancel";
            this.btnCancel.Size         = new System.Drawing.Size(150, 40);
            this.btnCancel.Text         = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ── UC_DichVu root ────────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.White;
            this.Name                = "UC_DichVu";
            this.Size                = new System.Drawing.Size(1882, 852);
            this.Load               += new System.EventHandler(this.UC_DichVu_Load);

            // Fill first → Top → overlay last
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlForm);

            ((System.ComponentModel.ISupportInitialize)(this.dgvServices)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel              pnlHeader;
        private System.Windows.Forms.Panel              pnlGrid;
        private System.Windows.Forms.Label              lblTitle;
        private System.Windows.Forms.Label              lblBreadcrumb;
        private Guna.UI2.WinForms.Guna2Button           btnAdd;
        private Guna.UI2.WinForms.Guna2TextBox          txtSearch;
        private Guna.UI2.WinForms.Guna2ComboBox         cmbFilterStatus;
        private Guna.UI2.WinForms.Guna2DataGridView     dgvServices;
        private Guna.UI2.WinForms.Guna2Panel             pnlForm;
        private System.Windows.Forms.Label              lblFormTitle;
        private System.Windows.Forms.Label              lblName;
        private Guna.UI2.WinForms.Guna2TextBox          txtServiceName;
        private System.Windows.Forms.Label              lblPrice;
        private Guna.UI2.WinForms.Guna2TextBox          txtPrice;
        private System.Windows.Forms.Label              lblStatus;
        private Guna.UI2.WinForms.Guna2ComboBox         cmbFormStatus;
        private Guna.UI2.WinForms.Guna2Button           btnSave;
        private Guna.UI2.WinForms.Guna2Button           btnCancel;
    }
}
