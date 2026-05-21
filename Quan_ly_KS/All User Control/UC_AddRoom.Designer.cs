namespace Quan_ly_KS.All_User_Control
{
    partial class UC_AddRoom
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
            this.lblTitle         = new System.Windows.Forms.Label();
            this.lblBreadcrumb    = new System.Windows.Forms.Label();
            this.lblCount         = new System.Windows.Forms.Label();
            this.btnAdd           = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch        = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbFilterStatus  = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dgvRooms         = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlForm          = new System.Windows.Forms.Panel();
            this.lblFormTitle     = new System.Windows.Forms.Label();
            this.lblRoomNo        = new System.Windows.Forms.Label();
            this.txtFormRoomNo    = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblRoomType      = new System.Windows.Forms.Label();
            this.cmbFormRoomType  = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblBed           = new System.Windows.Forms.Label();
            this.cmbFormBed       = new Guna.UI2.WinForms.Guna2ComboBox();
            this.lblPrice         = new System.Windows.Forms.Label();
            this.txtFormPrice     = new Guna.UI2.WinForms.Guna2TextBox();
            this.lblFormStatus    = new System.Windows.Forms.Label();
            this.cmbFormStatus    = new Guna.UI2.WinForms.Guna2ComboBox();
            this.btnSave          = new Guna.UI2.WinForms.Guna2Button();
            this.btnCancel        = new Guna.UI2.WinForms.Guna2Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).BeginInit();
            this.pnlForm.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize  = true;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblTitle.Location  = new System.Drawing.Point(20, 18);
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.Text      = "Danh Sách Phòng";

            // lblBreadcrumb
            this.lblBreadcrumb.AutoSize  = true;
            this.lblBreadcrumb.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBreadcrumb.ForeColor = System.Drawing.Color.Gray;
            this.lblBreadcrumb.Location  = new System.Drawing.Point(22, 62);
            this.lblBreadcrumb.Name      = "lblBreadcrumb";
            this.lblBreadcrumb.Text      = "Quản lý phòng  /  Danh sách phòng";

            // lblCount
            this.lblCount.AutoSize  = true;
            this.lblCount.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblCount.ForeColor = System.Drawing.Color.Gray;
            this.lblCount.Location  = new System.Drawing.Point(22, 795);
            this.lblCount.Name      = "lblCount";
            this.lblCount.Text      = "";

            // btnAdd
            this.btnAdd.BorderRadius                    = 8;
            this.btnAdd.DisabledState.BorderColor       = System.Drawing.Color.DarkGray;
            this.btnAdd.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAdd.DisabledState.FillColor         = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnAdd.DisabledState.ForeColor         = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnAdd.FillColor                       = System.Drawing.Color.SlateBlue;
            this.btnAdd.Font                            = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor                       = System.Drawing.Color.White;
            this.btnAdd.Location                        = new System.Drawing.Point(1640, 22);
            this.btnAdd.Name                            = "btnAdd";
            this.btnAdd.Size                            = new System.Drawing.Size(200, 45);
            this.btnAdd.Text                            = "+ Thêm Phòng";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // txtSearch
            this.txtSearch.BorderRadius                    = 8;
            this.txtSearch.DisabledState.BorderColor       = System.Drawing.Color.FromArgb(208, 208, 208);
            this.txtSearch.DisabledState.FillColor         = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtSearch.DisabledState.ForeColor         = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtSearch.FillColor                       = System.Drawing.Color.WhiteSmoke;
            this.txtSearch.Font                            = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location                        = new System.Drawing.Point(1150, 26);
            this.txtSearch.Name                            = "txtSearch";
            this.txtSearch.PlaceholderText                 = "Tìm kiếm số phòng...";
            this.txtSearch.Size                            = new System.Drawing.Size(250, 40);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // cmbFilterStatus
            this.cmbFilterStatus.BorderRadius                    = 8;
            this.cmbFilterStatus.DisabledState.BorderColor       = System.Drawing.Color.FromArgb(208, 208, 208);
            this.cmbFilterStatus.DisabledState.FillColor         = System.Drawing.Color.FromArgb(226, 226, 226);
            this.cmbFilterStatus.DisabledState.ForeColor         = System.Drawing.Color.FromArgb(138, 138, 138);
            this.cmbFilterStatus.FillColor                       = System.Drawing.Color.WhiteSmoke;
            this.cmbFilterStatus.Font                            = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFilterStatus.Location                        = new System.Drawing.Point(1415, 26);
            this.cmbFilterStatus.Name                            = "cmbFilterStatus";
            this.cmbFilterStatus.Size                            = new System.Drawing.Size(210, 40);
            this.cmbFilterStatus.SelectedIndexChanged += new System.EventHandler(this.cmbFilterStatus_SelectedIndexChanged);

            // dgvRooms
            this.dgvRooms.AllowUserToAddRows                  = false;
            this.dgvRooms.AllowUserToDeleteRows               = false;
            this.dgvRooms.BackgroundColor                     = System.Drawing.Color.White;
            this.dgvRooms.BorderStyle                         = System.Windows.Forms.BorderStyle.None;
            this.dgvRooms.ColumnHeadersHeightSizeMode         = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRooms.Font                                = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvRooms.Location                            = new System.Drawing.Point(20, 90);
            this.dgvRooms.Name                                = "dgvRooms";
            this.dgvRooms.RowHeadersVisible                   = false;
            this.dgvRooms.SelectionMode                       = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRooms.Size                                = new System.Drawing.Size(1840, 695);
            this.dgvRooms.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRooms_CellContentClick);

            // pnlForm (overlay, centered: x=(1882-600)/2=641, y=(852-555)/2=148)
            this.pnlForm.BackColor    = System.Drawing.Color.White;
            this.pnlForm.BorderStyle  = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlForm.Controls.Add(this.lblFormTitle);
            this.pnlForm.Controls.Add(this.lblRoomNo);
            this.pnlForm.Controls.Add(this.txtFormRoomNo);
            this.pnlForm.Controls.Add(this.lblRoomType);
            this.pnlForm.Controls.Add(this.cmbFormRoomType);
            this.pnlForm.Controls.Add(this.lblBed);
            this.pnlForm.Controls.Add(this.cmbFormBed);
            this.pnlForm.Controls.Add(this.lblPrice);
            this.pnlForm.Controls.Add(this.txtFormPrice);
            this.pnlForm.Controls.Add(this.lblFormStatus);
            this.pnlForm.Controls.Add(this.cmbFormStatus);
            this.pnlForm.Controls.Add(this.btnSave);
            this.pnlForm.Controls.Add(this.btnCancel);
            this.pnlForm.Location = new System.Drawing.Point(641, 148);
            this.pnlForm.Name     = "pnlForm";
            this.pnlForm.Size     = new System.Drawing.Size(600, 555);
            this.pnlForm.Visible  = false;

            // lblFormTitle
            this.lblFormTitle.AutoSize  = true;
            this.lblFormTitle.Font      = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblFormTitle.ForeColor = System.Drawing.Color.SlateBlue;
            this.lblFormTitle.Location  = new System.Drawing.Point(20, 20);
            this.lblFormTitle.Name      = "lblFormTitle";
            this.lblFormTitle.Text      = "Thêm Phòng";

            // --- Field 1: Số Phòng (label y=78, input y=103) ---
            this.lblRoomNo.AutoSize  = true;
            this.lblRoomNo.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRoomNo.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblRoomNo.Location  = new System.Drawing.Point(20, 78);
            this.lblRoomNo.Name      = "lblRoomNo";
            this.lblRoomNo.Text      = "Số Phòng";

            this.txtFormRoomNo.BorderRadius              = 8;
            this.txtFormRoomNo.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.txtFormRoomNo.DisabledState.FillColor   = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtFormRoomNo.DisabledState.ForeColor   = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtFormRoomNo.FillColor                 = System.Drawing.Color.WhiteSmoke;
            this.txtFormRoomNo.Font                      = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFormRoomNo.Location                  = new System.Drawing.Point(20, 103);
            this.txtFormRoomNo.Name                      = "txtFormRoomNo";
            this.txtFormRoomNo.PlaceholderText           = "Nhập số phòng (VD: 101)";
            this.txtFormRoomNo.Size                      = new System.Drawing.Size(555, 40);

            // --- Field 2: Loại Phòng (label y=160, combo y=185) ---
            this.lblRoomType.AutoSize  = true;
            this.lblRoomType.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRoomType.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblRoomType.Location  = new System.Drawing.Point(20, 160);
            this.lblRoomType.Name      = "lblRoomType";
            this.lblRoomType.Text      = "Loại Phòng";

            this.cmbFormRoomType.BorderRadius              = 8;
            this.cmbFormRoomType.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.cmbFormRoomType.DisabledState.FillColor   = System.Drawing.Color.FromArgb(226, 226, 226);
            this.cmbFormRoomType.DisabledState.ForeColor   = System.Drawing.Color.FromArgb(138, 138, 138);
            this.cmbFormRoomType.FillColor                 = System.Drawing.Color.WhiteSmoke;
            this.cmbFormRoomType.Font                      = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormRoomType.Location                  = new System.Drawing.Point(20, 185);
            this.cmbFormRoomType.Name                      = "cmbFormRoomType";
            this.cmbFormRoomType.Size                      = new System.Drawing.Size(555, 40);

            // --- Field 3: Loại Giường (label y=242, combo y=267) ---
            this.lblBed.AutoSize  = true;
            this.lblBed.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBed.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblBed.Location  = new System.Drawing.Point(20, 242);
            this.lblBed.Name      = "lblBed";
            this.lblBed.Text      = "Loại Giường";

            this.cmbFormBed.BorderRadius              = 8;
            this.cmbFormBed.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.cmbFormBed.DisabledState.FillColor   = System.Drawing.Color.FromArgb(226, 226, 226);
            this.cmbFormBed.DisabledState.ForeColor   = System.Drawing.Color.FromArgb(138, 138, 138);
            this.cmbFormBed.FillColor                 = System.Drawing.Color.WhiteSmoke;
            this.cmbFormBed.Font                      = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormBed.Location                  = new System.Drawing.Point(20, 267);
            this.cmbFormBed.Name                      = "cmbFormBed";
            this.cmbFormBed.Size                      = new System.Drawing.Size(555, 40);

            // --- Field 4: Giá Phòng (label y=324, input y=349) ---
            this.lblPrice.AutoSize  = true;
            this.lblPrice.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblPrice.Location  = new System.Drawing.Point(20, 324);
            this.lblPrice.Name      = "lblPrice";
            this.lblPrice.Text      = "Giá Phòng (VNĐ)";

            this.txtFormPrice.BorderRadius              = 8;
            this.txtFormPrice.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.txtFormPrice.DisabledState.FillColor   = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtFormPrice.DisabledState.ForeColor   = System.Drawing.Color.FromArgb(138, 138, 138);
            this.txtFormPrice.FillColor                 = System.Drawing.Color.WhiteSmoke;
            this.txtFormPrice.Font                      = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFormPrice.Location                  = new System.Drawing.Point(20, 349);
            this.txtFormPrice.Name                      = "txtFormPrice";
            this.txtFormPrice.PlaceholderText           = "Nhập giá (VD: 500000)";
            this.txtFormPrice.Size                      = new System.Drawing.Size(555, 40);

            // --- Field 5: Trạng Thái (label y=406, combo y=431) ---
            this.lblFormStatus.AutoSize  = true;
            this.lblFormStatus.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFormStatus.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblFormStatus.Location  = new System.Drawing.Point(20, 406);
            this.lblFormStatus.Name      = "lblFormStatus";
            this.lblFormStatus.Text      = "Trạng Thái";

            this.cmbFormStatus.BorderRadius              = 8;
            this.cmbFormStatus.DisabledState.BorderColor = System.Drawing.Color.FromArgb(208, 208, 208);
            this.cmbFormStatus.DisabledState.FillColor   = System.Drawing.Color.FromArgb(226, 226, 226);
            this.cmbFormStatus.DisabledState.ForeColor   = System.Drawing.Color.FromArgb(138, 138, 138);
            this.cmbFormStatus.FillColor                 = System.Drawing.Color.WhiteSmoke;
            this.cmbFormStatus.Font                      = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormStatus.Location                  = new System.Drawing.Point(20, 431);
            this.cmbFormStatus.Name                      = "cmbFormStatus";
            this.cmbFormStatus.Size                      = new System.Drawing.Size(555, 40);

            // btnSave
            this.btnSave.BorderRadius                    = 8;
            this.btnSave.DisabledState.BorderColor       = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSave.DisabledState.FillColor         = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnSave.DisabledState.ForeColor         = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnSave.FillColor                       = System.Drawing.Color.SlateBlue;
            this.btnSave.Font                            = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor                       = System.Drawing.Color.White;
            this.btnSave.Location                        = new System.Drawing.Point(20, 490);
            this.btnSave.Name                            = "btnSave";
            this.btnSave.Size                            = new System.Drawing.Size(160, 45);
            this.btnSave.Text                            = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            // btnCancel
            this.btnCancel.BorderRadius                    = 8;
            this.btnCancel.DisabledState.BorderColor       = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCancel.DisabledState.FillColor         = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnCancel.DisabledState.ForeColor         = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnCancel.FillColor                       = System.Drawing.Color.FromArgb(220, 220, 220);
            this.btnCancel.Font                            = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor                       = System.Drawing.Color.FromArgb(80, 80, 80);
            this.btnCancel.Location                        = new System.Drawing.Point(200, 490);
            this.btnCancel.Name                            = "btnCancel";
            this.btnCancel.Size                            = new System.Drawing.Size(160, 45);
            this.btnCancel.Text                            = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // UC_AddRoom
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.White;
            this.Controls.Add(this.lblCount);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.dgvRooms);
            this.Controls.Add(this.cmbFilterStatus);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblBreadcrumb);
            this.Controls.Add(this.lblTitle);
            this.Name = "UC_AddRoom";
            this.Size = new System.Drawing.Size(1882, 852);
            this.Load += new System.EventHandler(this.UC_AddRoom_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label               lblTitle;
        private System.Windows.Forms.Label               lblBreadcrumb;
        private System.Windows.Forms.Label               lblCount;
        private Guna.UI2.WinForms.Guna2Button            btnAdd;
        private Guna.UI2.WinForms.Guna2TextBox           txtSearch;
        private Guna.UI2.WinForms.Guna2ComboBox          cmbFilterStatus;
        private Guna.UI2.WinForms.Guna2DataGridView      dgvRooms;
        private System.Windows.Forms.Panel               pnlForm;
        private System.Windows.Forms.Label               lblFormTitle;
        private System.Windows.Forms.Label               lblRoomNo;
        private Guna.UI2.WinForms.Guna2TextBox           txtFormRoomNo;
        private System.Windows.Forms.Label               lblRoomType;
        private Guna.UI2.WinForms.Guna2ComboBox          cmbFormRoomType;
        private System.Windows.Forms.Label               lblBed;
        private Guna.UI2.WinForms.Guna2ComboBox          cmbFormBed;
        private System.Windows.Forms.Label               lblPrice;
        private Guna.UI2.WinForms.Guna2TextBox           txtFormPrice;
        private System.Windows.Forms.Label               lblFormStatus;
        private Guna.UI2.WinForms.Guna2ComboBox          cmbFormStatus;
        private Guna.UI2.WinForms.Guna2Button            btnSave;
        private Guna.UI2.WinForms.Guna2Button            btnCancel;
    }
}
