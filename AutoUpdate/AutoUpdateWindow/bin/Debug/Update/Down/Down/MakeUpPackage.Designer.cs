namespace Down
{
    partial class MakeUpPackage
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.makePath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.serviceDownUrl = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.version = new System.Windows.Forms.TextBox();
            this.browse_btn = new System.Windows.Forms.Button();
            this.make_btn = new System.Windows.Forms.Button();
            this.exit_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "更新包制作路径:";
            // 
            // makePath
            // 
            this.makePath.Enabled = false;
            this.makePath.Location = new System.Drawing.Point(107, 12);
            this.makePath.Name = "makePath";
            this.makePath.Size = new System.Drawing.Size(199, 21);
            this.makePath.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "服务器下载地址:";
            // 
            // serviceDownUrl
            // 
            this.serviceDownUrl.Location = new System.Drawing.Point(107, 49);
            this.serviceDownUrl.Name = "serviceDownUrl";
            this.serviceDownUrl.Size = new System.Drawing.Size(280, 21);
            this.serviceDownUrl.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(30, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "软件版本号:";
            // 
            // version
            // 
            this.version.Location = new System.Drawing.Point(107, 87);
            this.version.Name = "version";
            this.version.Size = new System.Drawing.Size(280, 21);
            this.version.TabIndex = 5;
            // 
            // browse_btn
            // 
            this.browse_btn.Location = new System.Drawing.Point(312, 12);
            this.browse_btn.Name = "browse_btn";
            this.browse_btn.Size = new System.Drawing.Size(75, 23);
            this.browse_btn.TabIndex = 6;
            this.browse_btn.Text = "浏览";
            this.browse_btn.UseVisualStyleBackColor = true;
            this.browse_btn.Click += new System.EventHandler(this.browse_btn_Click);
            // 
            // make_btn
            // 
            this.make_btn.Location = new System.Drawing.Point(221, 138);
            this.make_btn.Name = "make_btn";
            this.make_btn.Size = new System.Drawing.Size(75, 23);
            this.make_btn.TabIndex = 7;
            this.make_btn.Text = "制作";
            this.make_btn.UseVisualStyleBackColor = true;
            this.make_btn.Click += new System.EventHandler(this.make_btn_Click);
            // 
            // exit_btn
            // 
            this.exit_btn.Location = new System.Drawing.Point(312, 138);
            this.exit_btn.Name = "exit_btn";
            this.exit_btn.Size = new System.Drawing.Size(75, 23);
            this.exit_btn.TabIndex = 8;
            this.exit_btn.Text = "退出";
            this.exit_btn.UseVisualStyleBackColor = true;
            this.exit_btn.Click += new System.EventHandler(this.exit_btn_Click);
            // 
            // MakeUpPackage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(389, 163);
            this.Controls.Add(this.exit_btn);
            this.Controls.Add(this.make_btn);
            this.Controls.Add(this.browse_btn);
            this.Controls.Add(this.version);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.serviceDownUrl);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.makePath);
            this.Controls.Add(this.label1);
            this.Name = "MakeUpPackage";
            this.Text = "制作更新包";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox makePath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox serviceDownUrl;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox version;
        private System.Windows.Forms.Button browse_btn;
        private System.Windows.Forms.Button make_btn;
        private System.Windows.Forms.Button exit_btn;
    }
}

