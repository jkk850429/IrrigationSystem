namespace Farm_IO_Control
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblShowSystemTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblLinkStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.picLinkState = new System.Windows.Forms.PictureBox();
            this.btnSettingConfig = new System.Windows.Forms.Button();
            this.btnReadConfig = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.txtGroupCounterDatetime = new System.Windows.Forms.TextBox();
            this.txtGroupCounterEmergency = new System.Windows.Forms.TextBox();
            this.btnSetDeviceSystemTime = new System.Windows.Forms.Button();
            this.btnReadSystemTime = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRunningOrder = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.close = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.upload = new System.Windows.Forms.Button();
            this.emergency = new System.Windows.Forms.Button();
            this.search = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.edit = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLinkState)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblShowSystemTime,
            this.lblLinkStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 790);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1445, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblShowSystemTime
            // 
            this.lblShowSystemTime.BackColor = System.Drawing.Color.White;
            this.lblShowSystemTime.Name = "lblShowSystemTime";
            this.lblShowSystemTime.Size = new System.Drawing.Size(128, 17);
            this.lblShowSystemTime.Text = "toolStripStatusLabel1";
            // 
            // lblLinkStatus
            // 
            this.lblLinkStatus.BackColor = System.Drawing.Color.White;
            this.lblLinkStatus.Name = "lblLinkStatus";
            this.lblLinkStatus.Size = new System.Drawing.Size(128, 17);
            this.lblLinkStatus.Text = "toolStripStatusLabel1";
            // 
            // btnConnect
            // 
            this.btnConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnConnect.Location = new System.Drawing.Point(30, 22);
            this.btnConnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(104, 28);
            this.btnConnect.TabIndex = 1;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new System.Drawing.Point(157, 22);
            this.btnDisconnect.Margin = new System.Windows.Forms.Padding(4);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(88, 28);
            this.btnDisconnect.TabIndex = 2;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // picLinkState
            // 
            this.picLinkState.Image = ((System.Drawing.Image)(resources.GetObject("picLinkState.Image")));
            this.picLinkState.Location = new System.Drawing.Point(252, 20);
            this.picLinkState.Name = "picLinkState";
            this.picLinkState.Size = new System.Drawing.Size(81, 30);
            this.picLinkState.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picLinkState.TabIndex = 4;
            this.picLinkState.TabStop = false;
            // 
            // btnSettingConfig
            // 
            this.btnSettingConfig.Location = new System.Drawing.Point(717, 490);
            this.btnSettingConfig.Name = "btnSettingConfig";
            this.btnSettingConfig.Size = new System.Drawing.Size(75, 23);
            this.btnSettingConfig.TabIndex = 8;
            this.btnSettingConfig.Text = "設定";
            this.btnSettingConfig.UseVisualStyleBackColor = true;
            // 
            // btnReadConfig
            // 
            this.btnReadConfig.Location = new System.Drawing.Point(245, 754);
            this.btnReadConfig.Name = "btnReadConfig";
            this.btnReadConfig.Size = new System.Drawing.Size(75, 23);
            this.btnReadConfig.TabIndex = 9;
            this.btnReadConfig.Text = "讀取";
            this.btnReadConfig.UseVisualStyleBackColor = true;
            this.btnReadConfig.Click += new System.EventHandler(this.btnReadConfig_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(714, 529);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(162, 17);
            this.label25.TabIndex = 10;
            this.label25.Text = "Datetime Group counter :";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(714, 557);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(173, 17);
            this.label28.TabIndex = 11;
            this.label28.Text = "Emergency Group counter :";
            // 
            // txtGroupCounterDatetime
            // 
            this.txtGroupCounterDatetime.Location = new System.Drawing.Point(882, 529);
            this.txtGroupCounterDatetime.Name = "txtGroupCounterDatetime";
            this.txtGroupCounterDatetime.ReadOnly = true;
            this.txtGroupCounterDatetime.Size = new System.Drawing.Size(36, 25);
            this.txtGroupCounterDatetime.TabIndex = 12;
            this.txtGroupCounterDatetime.Text = "2";
            // 
            // txtGroupCounterEmergency
            // 
            this.txtGroupCounterEmergency.Location = new System.Drawing.Point(882, 560);
            this.txtGroupCounterEmergency.Name = "txtGroupCounterEmergency";
            this.txtGroupCounterEmergency.ReadOnly = true;
            this.txtGroupCounterEmergency.Size = new System.Drawing.Size(36, 25);
            this.txtGroupCounterEmergency.TabIndex = 13;
            this.txtGroupCounterEmergency.Text = "0";
            // 
            // btnSetDeviceSystemTime
            // 
            this.btnSetDeviceSystemTime.Location = new System.Drawing.Point(338, 754);
            this.btnSetDeviceSystemTime.Name = "btnSetDeviceSystemTime";
            this.btnSetDeviceSystemTime.Size = new System.Drawing.Size(114, 23);
            this.btnSetDeviceSystemTime.TabIndex = 14;
            this.btnSetDeviceSystemTime.Text = "設定設備時間";
            this.btnSetDeviceSystemTime.UseVisualStyleBackColor = true;
            this.btnSetDeviceSystemTime.Click += new System.EventHandler(this.btnSetDeviceSystemTime_Click);
            // 
            // btnReadSystemTime
            // 
            this.btnReadSystemTime.Location = new System.Drawing.Point(467, 754);
            this.btnReadSystemTime.Name = "btnReadSystemTime";
            this.btnReadSystemTime.Size = new System.Drawing.Size(104, 23);
            this.btnReadSystemTime.TabIndex = 15;
            this.btnReadSystemTime.Text = "讀取設備時間";
            this.btnReadSystemTime.UseVisualStyleBackColor = true;
            this.btnReadSystemTime.Click += new System.EventHandler(this.btnReadSystemTime_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(714, 585);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 17);
            this.label15.TabIndex = 16;
            this.label15.Text = "Running Order : ";
            // 
            // txtRunningOrder
            // 
            this.txtRunningOrder.Location = new System.Drawing.Point(817, 582);
            this.txtRunningOrder.Name = "txtRunningOrder";
            this.txtRunningOrder.Size = new System.Drawing.Size(36, 25);
            this.txtRunningOrder.TabIndex = 17;
            this.txtRunningOrder.Text = "0";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.Silver;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.Raised;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.GridColor = System.Drawing.Color.Black;
            this.dataGridView1.Location = new System.Drawing.Point(236, 112);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(1197, 636);
            this.dataGridView1.TabIndex = 91;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.panel2.Controls.Add(this.close);
            this.panel2.Controls.Add(this.btnConnect);
            this.panel2.Controls.Add(this.btnDisconnect);
            this.panel2.Controls.Add(this.picLinkState);
            this.panel2.Location = new System.Drawing.Point(228, 30);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1217, 76);
            this.panel2.TabIndex = 90;
            // 
            // close
            // 
            this.close.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.close.FlatAppearance.BorderSize = 0;
            this.close.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.close.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.close.Image = ((System.Drawing.Image)(resources.GetObject("close.Image")));
            this.close.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.close.Location = new System.Drawing.Point(1129, 12);
            this.close.Name = "close";
            this.close.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.close.Size = new System.Drawing.Size(76, 52);
            this.close.TabIndex = 0;
            this.close.UseVisualStyleBackColor = false;
            this.close.Click += new System.EventHandler(this.close_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.panel1.Controls.Add(this.upload);
            this.panel1.Controls.Add(this.emergency);
            this.panel1.Controls.Add(this.search);
            this.panel1.Controls.Add(this.delete);
            this.panel1.Controls.Add(this.edit);
            this.panel1.Controls.Add(this.add);
            this.panel1.Location = new System.Drawing.Point(0, 30);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(228, 760);
            this.panel1.TabIndex = 89;
            // 
            // upload
            // 
            this.upload.FlatAppearance.BorderSize = 0;
            this.upload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.upload.Font = new System.Drawing.Font("微軟正黑體", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.upload.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.upload.Image = ((System.Drawing.Image)(resources.GetObject("upload.Image")));
            this.upload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.upload.Location = new System.Drawing.Point(2, 651);
            this.upload.Name = "upload";
            this.upload.Size = new System.Drawing.Size(225, 69);
            this.upload.TabIndex = 8;
            this.upload.Text = "     上傳";
            this.upload.UseVisualStyleBackColor = true;
            this.upload.Click += new System.EventHandler(this.upload_Click);
            // 
            // emergency
            // 
            this.emergency.FlatAppearance.BorderSize = 0;
            this.emergency.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.emergency.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.emergency.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.emergency.Image = ((System.Drawing.Image)(resources.GetObject("emergency.Image")));
            this.emergency.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.emergency.Location = new System.Drawing.Point(1, 434);
            this.emergency.Name = "emergency";
            this.emergency.Size = new System.Drawing.Size(225, 69);
            this.emergency.TabIndex = 6;
            this.emergency.Text = "    緊急";
            this.emergency.UseVisualStyleBackColor = true;
            this.emergency.Click += new System.EventHandler(this.emergency_Click);
            // 
            // search
            // 
            this.search.FlatAppearance.BorderSize = 0;
            this.search.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.search.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.search.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.search.Image = ((System.Drawing.Image)(resources.GetObject("search.Image")));
            this.search.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.search.Location = new System.Drawing.Point(2, 332);
            this.search.Name = "search";
            this.search.Size = new System.Drawing.Size(227, 69);
            this.search.TabIndex = 5;
            this.search.Text = "   查詢";
            this.search.UseVisualStyleBackColor = true;
            this.search.Click += new System.EventHandler(this.search_Click);
            // 
            // delete
            // 
            this.delete.FlatAppearance.BorderSize = 0;
            this.delete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.delete.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.delete.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.delete.Image = ((System.Drawing.Image)(resources.GetObject("delete.Image")));
            this.delete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.delete.Location = new System.Drawing.Point(2, 231);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(227, 69);
            this.delete.TabIndex = 4;
            this.delete.Text = "   刪除";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // edit
            // 
            this.edit.FlatAppearance.BorderSize = 0;
            this.edit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.edit.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.edit.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.edit.Image = ((System.Drawing.Image)(resources.GetObject("edit.Image")));
            this.edit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.edit.Location = new System.Drawing.Point(1, 122);
            this.edit.Name = "edit";
            this.edit.Size = new System.Drawing.Size(228, 76);
            this.edit.TabIndex = 3;
            this.edit.Text = "   編輯";
            this.edit.UseVisualStyleBackColor = true;
            this.edit.Click += new System.EventHandler(this.edit_Click);
            // 
            // add
            // 
            this.add.FlatAppearance.BorderSize = 0;
            this.add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.add.Font = new System.Drawing.Font("微軟正黑體", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.add.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.add.Image = ((System.Drawing.Image)(resources.GetObject("add.Image")));
            this.add.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.add.Location = new System.Drawing.Point(0, 25);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(227, 70);
            this.add.TabIndex = 2;
            this.add.Text = "   新增";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.ForeColor = System.Drawing.Color.Firebrick;
            this.label1.Location = new System.Drawing.Point(1265, 762);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(168, 21);
            this.label1.TabIndex = 5;
            this.label1.Text = "註:每8秒刷新一次頁面";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gray;
            this.ClientSize = new System.Drawing.Size(1445, 812);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtRunningOrder);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.btnReadSystemTime);
            this.Controls.Add(this.btnSetDeviceSystemTime);
            this.Controls.Add(this.txtGroupCounterEmergency);
            this.Controls.Add(this.txtGroupCounterDatetime);
            this.Controls.Add(this.label28);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.btnReadConfig);
            this.Controls.Add(this.btnSettingConfig);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("微軟正黑體", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLinkState)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblShowSystemTime;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.PictureBox picLinkState;
        private System.Windows.Forms.ToolStripStatusLabel lblLinkStatus;
        private System.Windows.Forms.Button btnSettingConfig;
        private System.Windows.Forms.Button btnReadConfig;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtGroupCounterDatetime;
        private System.Windows.Forms.TextBox txtGroupCounterEmergency;
        private System.Windows.Forms.Button btnSetDeviceSystemTime;
        private System.Windows.Forms.Button btnReadSystemTime;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRunningOrder;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button close;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button upload;
        private System.Windows.Forms.Button emergency;
        private System.Windows.Forms.Button search;
        private System.Windows.Forms.Button delete;
        private System.Windows.Forms.Button edit;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label1;
    }
}

