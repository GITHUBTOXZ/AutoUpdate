namespace AutoUpdateWindow
{
    partial class UpdaetManage
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.addBtn = new System.Windows.Forms.Button();
            this.delBtn = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.upGrid = new System.Windows.Forms.DataGridView();
            this.addBtnSite = new System.Windows.Forms.Button();
            this.downBtn = new System.Windows.Forms.Button();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // addBtn
            // 
            this.addBtn.Location = new System.Drawing.Point(93, 8);
            this.addBtn.Name = "addBtn";
            this.addBtn.Size = new System.Drawing.Size(75, 38);
            this.addBtn.TabIndex = 1;
            this.addBtn.Text = "添加新行";
            this.addBtn.UseVisualStyleBackColor = true;
            this.addBtn.Click += new System.EventHandler(this.addBtn_Click);
            // 
            // delBtn
            // 
            this.delBtn.Location = new System.Drawing.Point(174, 10);
            this.delBtn.Name = "delBtn";
            this.delBtn.Size = new System.Drawing.Size(75, 35);
            this.delBtn.TabIndex = 3;
            this.delBtn.Text = "移除";
            this.delBtn.UseVisualStyleBackColor = true;
            this.delBtn.Click += new System.EventHandler(this.delBtn_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.upGrid);
            this.groupBox2.Location = new System.Drawing.Point(12, 52);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(753, 541);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "更新列表";
            // 
            // upGrid
            // 
            this.upGrid.AllowUserToAddRows = false;
            this.upGrid.BackgroundColor = System.Drawing.Color.White;
            this.upGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.upGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.upGrid.Location = new System.Drawing.Point(3, 17);
            this.upGrid.Name = "upGrid";
            this.upGrid.RowHeadersVisible = false;
            this.upGrid.RowTemplate.Height = 23;
            this.upGrid.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.upGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.upGrid.Size = new System.Drawing.Size(747, 521);
            this.upGrid.TabIndex = 0;
            this.upGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.upGrid_DataError);
            this.upGrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.upGrid_RowPostPaint);
            // 
            // addBtnSite
            // 
            this.addBtnSite.Location = new System.Drawing.Point(12, 8);
            this.addBtnSite.Name = "addBtnSite";
            this.addBtnSite.Size = new System.Drawing.Size(75, 38);
            this.addBtnSite.TabIndex = 5;
            this.addBtnSite.Text = "添加站点";
            this.addBtnSite.UseVisualStyleBackColor = true;
            this.addBtnSite.Click += new System.EventHandler(this.addBtnSite_Click);
            // 
            // downBtn
            // 
            this.downBtn.Location = new System.Drawing.Point(255, 12);
            this.downBtn.Name = "downBtn";
            this.downBtn.Size = new System.Drawing.Size(75, 35);
            this.downBtn.TabIndex = 6;
            this.downBtn.Text = "下载";
            this.downBtn.UseVisualStyleBackColor = true;
            this.downBtn.Click += new System.EventHandler(this.downBtn_Click);
            // 
            // UpdaetManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(774, 596);
            this.Controls.Add(this.downBtn);
            this.Controls.Add(this.addBtnSite);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.delBtn);
            this.Controls.Add(this.addBtn);
            this.Name = "UpdaetManage";
            this.Text = "更新管理";
            this.Load += new System.EventHandler(this.UpdaetManage_Load);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.upGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button addBtn;
        private System.Windows.Forms.Button delBtn;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView upGrid;
        private System.Windows.Forms.Button addBtnSite;
        private System.Windows.Forms.Button downBtn;
    }
}