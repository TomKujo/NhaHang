namespace GUI
{
    partial class ucThucDon
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.SplitContainer splitContainer1;

        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Label lblCatHeader;
        private System.Windows.Forms.DataGridView dgvCategory;
        private System.Windows.Forms.FlowLayoutPanel flpCatActions;
        private System.Windows.Forms.Button btnAddCat;
        private System.Windows.Forms.Button btnEditCat;
        private System.Windows.Forms.Button btnDelCat;

        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Label lblDishHeader;
        private System.Windows.Forms.DataGridView dgvDish;
        private System.Windows.Forms.FlowLayoutPanel flpDishActions;
        private System.Windows.Forms.Button btnAddDish;
        private System.Windows.Forms.Button btnEditDish;
        private System.Windows.Forms.Button btnDelDish;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.dgvCategory = new System.Windows.Forms.DataGridView();
            this.flpCatActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddCat = new System.Windows.Forms.Button();
            this.btnEditCat = new System.Windows.Forms.Button();
            this.btnDelCat = new System.Windows.Forms.Button();
            this.lblCatHeader = new System.Windows.Forms.Label();
            this.pnlRight = new System.Windows.Forms.Panel();
            this.dgvDish = new System.Windows.Forms.DataGridView();
            this.flpDishActions = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAddDish = new System.Windows.Forms.Button();
            this.btnEditDish = new System.Windows.Forms.Button();
            this.btnDelDish = new System.Windows.Forms.Button();
            this.lblDishHeader = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategory)).BeginInit();
            this.flpCatActions.SuspendLayout();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDish)).BeginInit();
            this.flpDishActions.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pnlLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnlRight);
            this.splitContainer1.Size = new System.Drawing.Size(1000, 600);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.TabIndex = 0;
            // 
            // pnlLeft
            // 
            this.pnlLeft.Controls.Add(this.dgvCategory);
            this.pnlLeft.Controls.Add(this.flpCatActions);
            this.pnlLeft.Controls.Add(this.lblCatHeader);
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.Padding = new System.Windows.Forms.Padding(10);
            this.pnlLeft.Location = new System.Drawing.Point(0, 0);
            this.pnlLeft.Name = "pnlLeft";
            this.pnlLeft.Size = new System.Drawing.Size(350, 600);
            this.pnlLeft.TabIndex = 0;
            // 
            // dgvCategory
            // 
            this.dgvCategory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCategory.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCategory.Location = new System.Drawing.Point(10, 50);
            this.dgvCategory.Name = "dgvCategory";
            this.dgvCategory.RowHeadersWidth = 51;
            this.dgvCategory.Size = new System.Drawing.Size(330, 490);
            this.dgvCategory.TabIndex = 2;
            // 
            // flpCatActions
            // 
            this.flpCatActions.Controls.Add(this.btnAddCat);
            this.flpCatActions.Controls.Add(this.btnEditCat);
            this.flpCatActions.Controls.Add(this.btnDelCat);
            this.flpCatActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpCatActions.Location = new System.Drawing.Point(10, 540);
            this.flpCatActions.Name = "flpCatActions";
            this.flpCatActions.Size = new System.Drawing.Size(330, 50);
            this.flpCatActions.TabIndex = 1;
            // 
            // btnAddCat
            // 
            this.btnAddCat.Location = new System.Drawing.Point(3, 3);
            this.btnAddCat.Name = "btnAddCat";
            this.btnAddCat.Size = new System.Drawing.Size(100, 35);
            this.btnAddCat.TabIndex = 0;
            this.btnAddCat.Text = "Thêm TĐ";
            this.btnAddCat.UseVisualStyleBackColor = true;
            // 
            // btnEditCat
            // 
            this.btnEditCat.Location = new System.Drawing.Point(109, 3);
            this.btnEditCat.Name = "btnEditCat";
            this.btnEditCat.Size = new System.Drawing.Size(100, 35);
            this.btnEditCat.TabIndex = 1;
            this.btnEditCat.Text = "Sửa TĐ";
            this.btnEditCat.UseVisualStyleBackColor = true;
            // 
            // btnDelCat
            // 
            this.btnDelCat.Location = new System.Drawing.Point(215, 3);
            this.btnDelCat.Name = "btnDelCat";
            this.btnDelCat.Size = new System.Drawing.Size(100, 35);
            this.btnDelCat.TabIndex = 2;
            this.btnDelCat.Text = "Xóa TĐ";
            this.btnDelCat.UseVisualStyleBackColor = true;
            // 
            // lblCatHeader
            // 
            this.lblCatHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblCatHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblCatHeader.Location = new System.Drawing.Point(10, 10);
            this.lblCatHeader.Name = "lblCatHeader";
            this.lblCatHeader.Size = new System.Drawing.Size(330, 40);
            this.lblCatHeader.TabIndex = 0;
            this.lblCatHeader.Text = "DANH MỤC THỰC ĐƠN";
            this.lblCatHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlRight
            // 
            this.pnlRight.Controls.Add(this.dgvDish);
            this.pnlRight.Controls.Add(this.flpDishActions);
            this.pnlRight.Controls.Add(this.lblDishHeader);
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Padding = new System.Windows.Forms.Padding(10);
            this.pnlRight.Location = new System.Drawing.Point(0, 0);
            this.pnlRight.Name = "pnlRight";
            this.pnlRight.Size = new System.Drawing.Size(646, 600);
            this.pnlRight.TabIndex = 0;
            // 
            // dgvDish
            // 
            this.dgvDish.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDish.Location = new System.Drawing.Point(10, 50);
            this.dgvDish.Name = "dgvDish";
            this.dgvDish.RowHeadersWidth = 51;
            this.dgvDish.Size = new System.Drawing.Size(626, 490);
            this.dgvDish.TabIndex = 2;
            // 
            // flpDishActions
            // 
            this.flpDishActions.Controls.Add(this.btnAddDish);
            this.flpDishActions.Controls.Add(this.btnEditDish);
            this.flpDishActions.Controls.Add(this.btnDelDish);
            this.flpDishActions.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.flpDishActions.Location = new System.Drawing.Point(10, 540);
            this.flpDishActions.Name = "flpDishActions";
            this.flpDishActions.Size = new System.Drawing.Size(626, 50);
            this.flpDishActions.TabIndex = 1;
            // 
            // btnAddDish
            // 
            this.btnAddDish.Location = new System.Drawing.Point(3, 3);
            this.btnAddDish.Name = "btnAddDish";
            this.btnAddDish.Size = new System.Drawing.Size(100, 35);
            this.btnAddDish.TabIndex = 0;
            this.btnAddDish.Text = "Thêm Món";
            this.btnAddDish.UseVisualStyleBackColor = true;
            // 
            // btnEditDish
            // 
            this.btnEditDish.Location = new System.Drawing.Point(109, 3);
            this.btnEditDish.Name = "btnEditDish";
            this.btnEditDish.Size = new System.Drawing.Size(100, 35);
            this.btnEditDish.TabIndex = 1;
            this.btnEditDish.Text = "Sửa Món";
            this.btnEditDish.UseVisualStyleBackColor = true;
            // 
            // btnDelDish
            // 
            this.btnDelDish.Location = new System.Drawing.Point(215, 3);
            this.btnDelDish.Name = "btnDelDish";
            this.btnDelDish.Size = new System.Drawing.Size(100, 35);
            this.btnDelDish.TabIndex = 2;
            this.btnDelDish.Text = "Xóa Món";
            this.btnDelDish.UseVisualStyleBackColor = true;
            // 
            // lblDishHeader
            // 
            this.lblDishHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblDishHeader.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblDishHeader.Location = new System.Drawing.Point(10, 10);
            this.lblDishHeader.Name = "lblDishHeader";
            this.lblDishHeader.Size = new System.Drawing.Size(626, 40);
            this.lblDishHeader.TabIndex = 0;
            this.lblDishHeader.Text = "DANH SÁCH MÓN ĂN";
            this.lblDishHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ucThucDon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "ucThucDon";
            this.Size = new System.Drawing.Size(1000, 600);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCategory)).EndInit();
            this.flpCatActions.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDish)).EndInit();
            this.flpDishActions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}