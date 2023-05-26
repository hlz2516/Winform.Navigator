namespace demo.页面缓存
{
    partial class Form2
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btn_back = new System.Windows.Forms.Button();
            this.btn_backback = new System.Windows.Forms.Button();
            this.btn_page1 = new System.Windows.Forms.Button();
            this.btn_page2 = new System.Windows.Forms.Button();
            this.btn_page3 = new System.Windows.Forms.Button();
            this.navigator1 = new Navigators.Navigator();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.navigator1);
            this.panel1.Location = new System.Drawing.Point(12, 89);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(951, 479);
            this.panel1.TabIndex = 0;
            // 
            // btn_back
            // 
            this.btn_back.Location = new System.Drawing.Point(56, 29);
            this.btn_back.Name = "btn_back";
            this.btn_back.Size = new System.Drawing.Size(100, 30);
            this.btn_back.TabIndex = 1;
            this.btn_back.Text = "返回上一页";
            this.btn_back.UseVisualStyleBackColor = true;
            this.btn_back.Click += new System.EventHandler(this.btn_back_Click);
            // 
            // btn_backback
            // 
            this.btn_backback.Location = new System.Drawing.Point(193, 29);
            this.btn_backback.Name = "btn_backback";
            this.btn_backback.Size = new System.Drawing.Size(100, 30);
            this.btn_backback.TabIndex = 2;
            this.btn_backback.Text = "撤销返回";
            this.btn_backback.UseVisualStyleBackColor = true;
            this.btn_backback.Click += new System.EventHandler(this.btn_backback_Click);
            // 
            // btn_page1
            // 
            this.btn_page1.Location = new System.Drawing.Point(438, 29);
            this.btn_page1.Name = "btn_page1";
            this.btn_page1.Size = new System.Drawing.Size(100, 30);
            this.btn_page1.TabIndex = 3;
            this.btn_page1.Text = "页面一";
            this.btn_page1.UseVisualStyleBackColor = true;
            this.btn_page1.Click += new System.EventHandler(this.btn_page1_Click);
            // 
            // btn_page2
            // 
            this.btn_page2.Location = new System.Drawing.Point(591, 29);
            this.btn_page2.Name = "btn_page2";
            this.btn_page2.Size = new System.Drawing.Size(100, 30);
            this.btn_page2.TabIndex = 4;
            this.btn_page2.Text = "页面二";
            this.btn_page2.UseVisualStyleBackColor = true;
            this.btn_page2.Click += new System.EventHandler(this.btn_page2_Click);
            // 
            // btn_page3
            // 
            this.btn_page3.Location = new System.Drawing.Point(751, 29);
            this.btn_page3.Name = "btn_page3";
            this.btn_page3.Size = new System.Drawing.Size(100, 30);
            this.btn_page3.TabIndex = 5;
            this.btn_page3.Text = "页面三";
            this.btn_page3.UseVisualStyleBackColor = true;
            this.btn_page3.Click += new System.EventHandler(this.btn_page3_Click);
            // 
            // navigator1
            // 
            this.navigator1.Location = new System.Drawing.Point(3, 411);
            this.navigator1.Name = "navigator1";
            this.navigator1.Size = new System.Drawing.Size(65, 65);
            this.navigator1.TabIndex = 6;
            this.navigator1.Text = "navigator1";
            this.navigator1.Visible = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(975, 580);
            this.Controls.Add(this.btn_page3);
            this.Controls.Add(this.btn_page2);
            this.Controls.Add(this.btn_page1);
            this.Controls.Add(this.btn_backback);
            this.Controls.Add(this.btn_back);
            this.Controls.Add(this.panel1);
            this.Name = "Form2";
            this.Text = "Form2";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btn_back;
        private System.Windows.Forms.Button btn_backback;
        private System.Windows.Forms.Button btn_page1;
        private System.Windows.Forms.Button btn_page2;
        private System.Windows.Forms.Button btn_page3;
        private Navigators.Navigator navigator1;
    }
}