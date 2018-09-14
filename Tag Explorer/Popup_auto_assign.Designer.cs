namespace Tag_Explorer
{
    partial class Popup_auto_assign
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
            this.ApiMWMin = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.ApiMWMax = new System.Windows.Forms.NumericUpDown();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.SupMWMin = new System.Windows.Forms.NumericUpDown();
            this.SupMWMax = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.SupMMin = new System.Windows.Forms.NumericUpDown();
            this.SupMMax = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.ApiMMax = new System.Windows.Forms.NumericUpDown();
            this.ApiMMin = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ApiMWMin)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ApiMWMax)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SupMWMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SupMWMax)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SupMMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SupMMax)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ApiMMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApiMMin)).BeginInit();
            this.SuspendLayout();
            // 
            // ApiMWMin
            // 
            this.ApiMWMin.Location = new System.Drawing.Point(15, 23);
            this.ApiMWMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ApiMWMin.Name = "ApiMWMin";
            this.ApiMWMin.Size = new System.Drawing.Size(96, 20);
            this.ApiMWMin.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Plage d\'adresses API %MW";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.OliveDrab;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.ApiMWMax);
            this.panel1.Controls.Add(this.ApiMWMin);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(277, 50);
            this.panel1.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(117, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "----->";
            // 
            // ApiMWMax
            // 
            this.ApiMWMax.Location = new System.Drawing.Point(155, 23);
            this.ApiMWMax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ApiMWMax.Name = "ApiMWMax";
            this.ApiMWMax.Size = new System.Drawing.Size(96, 20);
            this.ApiMWMax.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.OliveDrab;
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.SupMWMin);
            this.panel2.Controls.Add(this.SupMWMax);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(295, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(277, 50);
            this.panel2.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(117, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(28, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "----->";
            // 
            // SupMWMin
            // 
            this.SupMWMin.Location = new System.Drawing.Point(15, 23);
            this.SupMWMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SupMWMin.Name = "SupMWMin";
            this.SupMWMin.Size = new System.Drawing.Size(96, 20);
            this.SupMWMin.TabIndex = 2;
            // 
            // SupMWMax
            // 
            this.SupMWMax.Location = new System.Drawing.Point(151, 23);
            this.SupMWMax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SupMWMax.Name = "SupMWMax";
            this.SupMWMax.Size = new System.Drawing.Size(96, 20);
            this.SupMWMax.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(176, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Plage d\'adresses Supervision %MW";
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(253, 140);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(52, 24);
            this.ConfirmButton.TabIndex = 5;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.OliveDrab;
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.SupMMin);
            this.panel3.Controls.Add(this.SupMMax);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Location = new System.Drawing.Point(294, 68);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(277, 50);
            this.panel3.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(117, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "----->";
            // 
            // SupMMin
            // 
            this.SupMMin.Location = new System.Drawing.Point(15, 23);
            this.SupMMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SupMMin.Name = "SupMMin";
            this.SupMMin.Size = new System.Drawing.Size(96, 20);
            this.SupMMin.TabIndex = 2;
            // 
            // SupMMax
            // 
            this.SupMMax.Location = new System.Drawing.Point(151, 23);
            this.SupMMax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.SupMMax.Name = "SupMMax";
            this.SupMMax.Size = new System.Drawing.Size(96, 20);
            this.SupMMax.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(165, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Plage d\'adresses Supervision %M";
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.OliveDrab;
            this.panel4.Controls.Add(this.label7);
            this.panel4.Controls.Add(this.ApiMMax);
            this.panel4.Controls.Add(this.ApiMMin);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Location = new System.Drawing.Point(11, 68);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(277, 50);
            this.panel4.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(117, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(28, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "----->";
            // 
            // ApiMMax
            // 
            this.ApiMMax.Location = new System.Drawing.Point(155, 23);
            this.ApiMMax.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ApiMMax.Name = "ApiMMax";
            this.ApiMMax.Size = new System.Drawing.Size(96, 20);
            this.ApiMMax.TabIndex = 2;
            // 
            // ApiMMin
            // 
            this.ApiMMin.Location = new System.Drawing.Point(15, 23);
            this.ApiMMin.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ApiMMin.Name = "ApiMMin";
            this.ApiMMin.Size = new System.Drawing.Size(96, 20);
            this.ApiMMin.TabIndex = 0;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 7);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(127, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Plage d\'adresses API %M";
            // 
            // Popup_auto_assign
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 176);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Popup_auto_assign";
            this.Text = "Popup_auto_assign";
            ((System.ComponentModel.ISupportInitialize)(this.ApiMWMin)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ApiMWMax)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SupMWMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SupMWMax)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.SupMMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SupMMax)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ApiMMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ApiMMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown ApiMWMin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown ApiMWMax;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown SupMWMin;
        private System.Windows.Forms.NumericUpDown SupMWMax;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown SupMMin;
        private System.Windows.Forms.NumericUpDown SupMMax;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown ApiMMax;
        private System.Windows.Forms.NumericUpDown ApiMMin;
        private System.Windows.Forms.Label label8;
    }
}