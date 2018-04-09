namespace WokItEasy
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
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button31 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.button32 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(734, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(393, 39);
            this.label1.TabIndex = 0;
            this.label1.Text = "Przyjmowanie zamówień";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 31;
            this.listBox1.Location = new System.Drawing.Point(3, 9);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(313, 562);
            this.listBox1.TabIndex = 32;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            this.listBox1.SelectedValueChanged += new System.EventHandler(this.listBox1_SelectedValueChanged);
            // 
            // button31
            // 
            this.button31.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button31.Location = new System.Drawing.Point(3, 588);
            this.button31.Name = "button31";
            this.button31.Size = new System.Drawing.Size(313, 165);
            this.button31.TabIndex = 33;
            this.button31.Tag = "";
            this.button31.Text = "Zatwierdź";
            this.button31.UseVisualStyleBackColor = true;
            this.button31.Click += new System.EventHandler(this.button31_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(12, 760);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 48);
            this.label2.TabIndex = 34;
            this.label2.Text = "0";
            // 
            // button32
            // 
            this.button32.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button32.Location = new System.Drawing.Point(3, 823);
            this.button32.Name = "button32";
            this.button32.Size = new System.Drawing.Size(313, 165);
            this.button32.TabIndex = 35;
            this.button32.Text = "Online";
            this.button32.UseVisualStyleBackColor = true;
            this.button32.Click += new System.EventHandler(this.button32_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(342, 100);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 888);
            this.button1.TabIndex = 36;
            this.button1.Text = "<<<<";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1425, 100);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 888);
            this.button2.TabIndex = 37;
            this.button2.Text = ">>>>";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Visible = false;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1500, 1000);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button32);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button31);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form2_Load);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form2_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button31;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button32;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}