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
            this.pnlHeader        = new System.Windows.Forms.Panel();
            this.pnlGrid          = new System.Windows.Forms.Panel();
            this.lblTitle         = new System.Windows.Forms.Label();
            this.lblBreadcrumb    = new System.Windows.Forms.Label();
            this.btnAdd           = new Guna.UI2.WinForms.Guna2Button();
            this.txtSearch        = new Guna.UI2.WinForms.Guna2TextBox();
            this.cmbFilterStatus  = new Guna.UI2.WinForms.Guna2ComboBox();
            this.dgvRooms         = new Guna.UI2.WinForms.Guna2DataGridView();
            this.pnlForm          = new Guna.UI2.WinForms.Guna2Panel();
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

            // lblTitle (inside pnlHeader)
            this.lblTitle.AutoSize  = true;
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(50, 50, 50);
            this.lblTitle.Location  = new System.Drawing.Point(20, 10);
            this.lblTitle.Name      = "lblTitle";
            this.lblTitle.Text      = "Danh Sách Phòng";
            this.lblTitle.Anchor    = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;

            // lblBreadcrumb (inside pnlHeader, below title — updated in LoadData to show count)
            this.lblBreadcrumb.AutoSize  = true;
            this.lblBreadcrumb.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBreadcrumb.ForeColor = System.Drawing.Color.Gray;
            this.lblBreadcrumb.Location  = new System.Drawing.Point(20, 52);
            this.lblBreadcrumb.Name      = "lblBreadcrumb";
            this.lblBreadcrumb.Text      = "Quản lý phòng  /  Danh sách phòng";
            this.lblBreadcrumb.Anchor    = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Top;

            // btnAdd
            this.btnAdd.BorderRadius              = 8;
            this.btnAdd.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAdd.DisabledState.FillColor   = System.Drawing.Color.FromArgb(169, 169, 169);
            this.btnAdd.DisabledState.ForeColor   = System.Drawing.Color.FromArgb(141, 141, 141);
            this.btnAdd.FillColor                 = System.Drawing.Color.SlateBlue;
            this.btnAdd.Font                      = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnAdd.ForeColor                 = System.Drawing.Color.White;
            this.btnAdd.Location                  = new System.Drawing.Point(20, 22);
            this.btnAdd.Name                      = "btnAdd";
            this.btnAdd.Size                      = new System.Drawing.Size(200, 45);
            this.btnAdd.Top                       = 22;
            this.btnAdd.Text                      = "+ Thêm Phòng";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);

            // cmbFilterStatus
            this.cmbFilterStatus.BorderRadius            = 8;
            this.cmbFilterStatus.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.cmbFilterStatus.FillColor               = System.Drawing.Color.WhiteSmoke;
            this.cmbFilterStatus.Font                    = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFilterStatus.Location                = new System.Drawing.Point(20, 26);
            this.cmbFilterStatus.Name                    = "cmbFilterStatus";
            this.cmbFilterStatus.Size                    = new System.Drawing.Size(210, 40);
            this.cmbFilterStatus.SelectedIndexChanged += new System.EventHandler(this.cmbFilterStatus_SelectedIndexChanged);

            // txtSearch
            this.txtSearch.BorderRadius            = 8;
            this.txtSearch.DisabledState.FillColor = System.Drawing.Color.FromArgb(226, 226, 226);
            this.txtSearch.FillColor               = System.Drawing.Color.WhiteSmoke;
            this.txtSearch.Font                    = new System.Drawing.Font("Segoe UI", 10F);
            this.txtSearch.Location                = new System.Drawing.Point(20, 26);
            this.txtSearch.Name                    = "txtSearch";
            this.txtSearch.PlaceholderText         = "Tìm kiếm số phòng...";
            this.txtSearch.Size                    = new System.Drawing.Size(220, 40);
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);

            // ── pnlGrid (Dock=Fill) ───────────────────────────────────
            this.pnlGrid.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Padding   = new System.Windows.Forms.Padding(20, 10, 20, 16);
            this.pnlGrid.BackColor = System.Drawing.Color.White;
            this.pnlGrid.Controls.Add(this.dgvRooms);

            // dgvRooms (Dock=Fill inside pnlGrid)
            this.dgvRooms.AllowUserToAddRows          = false;
            this.dgvRooms.AllowUserToDeleteRows       = false;
            this.dgvRooms.AllowUserToResizeRows       = false;
            this.dgvRooms.BackgroundColor             = System.Drawing.Color.White;
            this.dgvRooms.BorderStyle                 = System.Windows.Forms.BorderStyle.None;
            this.dgvRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvRooms.Dock                        = System.Windows.Forms.DockStyle.Fill;
            this.dgvRooms.Font                        = new System.Drawing.Font("Segoe UI", 10F);
            this.dgvRooms.Name                        = "dgvRooms";
            this.dgvRooms.RowHeadersVisible           = false;
            this.dgvRooms.SelectionMode               = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRooms.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRooms_CellContentClick);

            // ── pnlForm (overlay, centered via CenterFormPanel) ───────
            this.pnlForm.FillColor       = System.Drawing.Color.White;
            this.pnlForm.BorderRadius    = 12;
            this.pnlForm.BorderColor     = System.Drawing.Color.FromArgb(100, 132, 112, 255);
            this.pnlForm.BorderThickness = 2;
            this.pnlForm.Name            = "pnlForm";
            this.pnlForm.Size            = new System.Drawing.Size(500, 472);
            this.pnlForm.Visible         = false;
            this.pnlForm.Controls.AddRange(new System.Windows.Forms.Control[] {
                this.lblFormTitle, this.lblRoomNo, this.txtFormRoomNo,
                this.lblRoomType, this.cmbFormRoomType,
                this.lblBed, this.cmbFormBed,
                this.lblPrice, this.txtFormPrice,
                this.lblFormStatus, this.cmbFormStatus,
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
            this.lblFormTitle.Text      = "Thêm Phòng";

            // Số Phòng  (field 0: label top=48, input top=65)
            this.lblRoomNo.AutoSize  = true;
            this.lblRoomNo.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRoomNo.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblRoomNo.Location  = new System.Drawing.Point(18, 48);
            this.lblRoomNo.Name      = "lblRoomNo";
            this.lblRoomNo.Text      = "Số Phòng";

            this.txtFormRoomNo.BorderRadius    = 8;
            this.txtFormRoomNo.FillColor       = System.Drawing.Color.WhiteSmoke;
            this.txtFormRoomNo.Font            = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFormRoomNo.Location        = new System.Drawing.Point(18, 65);
            this.txtFormRoomNo.Name            = "txtFormRoomNo";
            this.txtFormRoomNo.PlaceholderText = "Nhập số phòng (VD: 101)";
            this.txtFormRoomNo.Size            = new System.Drawing.Size(464, 38);

            // Loại Phòng  (field 1: label top=121, input top=138)
            this.lblRoomType.AutoSize  = true;
            this.lblRoomType.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblRoomType.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblRoomType.Location  = new System.Drawing.Point(18, 121);
            this.lblRoomType.Name      = "lblRoomType";
            this.lblRoomType.Text      = "Loại Phòng";

            this.cmbFormRoomType.BorderRadius = 8;
            this.cmbFormRoomType.FillColor    = System.Drawing.Color.WhiteSmoke;
            this.cmbFormRoomType.Font         = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormRoomType.Location     = new System.Drawing.Point(18, 138);
            this.cmbFormRoomType.Name         = "cmbFormRoomType";
            this.cmbFormRoomType.Size         = new System.Drawing.Size(464, 38);

            // Loại Giường  (field 2: label top=194, input top=211)
            this.lblBed.AutoSize  = true;
            this.lblBed.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblBed.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblBed.Location  = new System.Drawing.Point(18, 194);
            this.lblBed.Name      = "lblBed";
            this.lblBed.Text      = "Loại Giường";

            this.cmbFormBed.BorderRadius = 8;
            this.cmbFormBed.FillColor    = System.Drawing.Color.WhiteSmoke;
            this.cmbFormBed.Font         = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormBed.Location     = new System.Drawing.Point(18, 211);
            this.cmbFormBed.Name         = "cmbFormBed";
            this.cmbFormBed.Size         = new System.Drawing.Size(464, 38);

            // Giá Phòng  (field 3: label top=267, input top=284)
            this.lblPrice.AutoSize  = true;
            this.lblPrice.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPrice.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblPrice.Location  = new System.Drawing.Point(18, 267);
            this.lblPrice.Name      = "lblPrice";
            this.lblPrice.Text      = "Giá Phòng (VNĐ)";

            this.txtFormPrice.BorderRadius    = 8;
            this.txtFormPrice.FillColor       = System.Drawing.Color.WhiteSmoke;
            this.txtFormPrice.Font            = new System.Drawing.Font("Segoe UI", 10F);
            this.txtFormPrice.Location        = new System.Drawing.Point(18, 284);
            this.txtFormPrice.Name            = "txtFormPrice";
            this.txtFormPrice.PlaceholderText = "Nhập giá (VD: 500000)";
            this.txtFormPrice.Size            = new System.Drawing.Size(464, 38);

            // Trạng Thái  (field 4: label top=340, input top=357)
            this.lblFormStatus.AutoSize  = true;
            this.lblFormStatus.Font      = new System.Drawing.Font("Segoe UI", 10F);
            this.lblFormStatus.ForeColor = System.Drawing.Color.FromArgb(60, 60, 60);
            this.lblFormStatus.Location  = new System.Drawing.Point(18, 340);
            this.lblFormStatus.Name      = "lblFormStatus";
            this.lblFormStatus.Text      = "Trạng Thái";

            this.cmbFormStatus.BorderRadius = 8;
            this.cmbFormStatus.FillColor    = System.Drawing.Color.WhiteSmoke;
            this.cmbFormStatus.Font         = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbFormStatus.Location     = new System.Drawing.Point(18, 357);
            this.cmbFormStatus.Name         = "cmbFormStatus";
            this.cmbFormStatus.Size         = new System.Drawing.Size(464, 38);

            // btnSave / btnCancel  (btnY = 357+38+18 = 413)
            this.btnSave.BorderRadius = 8;
            this.btnSave.FillColor    = System.Drawing.Color.SlateBlue;
            this.btnSave.Font         = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor    = System.Drawing.Color.White;
            this.btnSave.Location     = new System.Drawing.Point(18, 413);
            this.btnSave.Name         = "btnSave";
            this.btnSave.Size         = new System.Drawing.Size(150, 40);
            this.btnSave.Text         = "Lưu";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.btnCancel.BorderRadius = 8;
            this.btnCancel.FillColor    = System.Drawing.Color.FromArgb(220, 220, 220);
            this.btnCancel.Font         = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor    = System.Drawing.Color.FromArgb(80, 80, 80);
            this.btnCancel.Location     = new System.Drawing.Point(178, 413);
            this.btnCancel.Name         = "btnCancel";
            this.btnCancel.Size         = new System.Drawing.Size(150, 40);
            this.btnCancel.Text         = "Hủy";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // ── UC_AddRoom root ───────────────────────────────────────
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode       = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor           = System.Drawing.Color.White;
            this.Name                = "UC_AddRoom";
            this.Size                = new System.Drawing.Size(1882, 852);
            this.Load               += new System.EventHandler(this.UC_AddRoom_Load);

            // Add in correct z-order: Fill first, then Top, then overlay
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlForm);

            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel               pnlHeader;
        private System.Windows.Forms.Panel               pnlGrid;
        private System.Windows.Forms.Label               lblTitle;
        private System.Windows.Forms.Label               lblBreadcrumb;
        private Guna.UI2.WinForms.Guna2Button            btnAdd;
        private Guna.UI2.WinForms.Guna2TextBox           txtSearch;
        private Guna.UI2.WinForms.Guna2ComboBox          cmbFilterStatus;
        private Guna.UI2.WinForms.Guna2DataGridView      dgvRooms;
        private Guna.UI2.WinForms.Guna2Panel              pnlForm;
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
