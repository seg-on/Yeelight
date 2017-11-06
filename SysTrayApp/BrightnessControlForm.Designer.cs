namespace YeelightTray
{
    partial class BrightnessControlForm
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
            this.pbBrightness = new System.Windows.Forms.ProgressBar();
            this.lBrightness = new System.Windows.Forms.Label();
            this.cbColourTemperature = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // pbBrightness
            // 
            this.pbBrightness.ForeColor = System.Drawing.SystemColors.WindowText;
            this.pbBrightness.Location = new System.Drawing.Point(10, 10);
            this.pbBrightness.Name = "pbBrightness";
            this.pbBrightness.Size = new System.Drawing.Size(200, 25);
            this.pbBrightness.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbBrightness.TabIndex = 0;
            // 
            // lBrightness
            // 
            this.lBrightness.AutoSize = true;
            this.lBrightness.Location = new System.Drawing.Point(100, 16);
            this.lBrightness.Name = "lBrightness";
            this.lBrightness.Size = new System.Drawing.Size(21, 13);
            this.lBrightness.TabIndex = 1;
            this.lBrightness.Text = "0%";
            // 
            // cbColourTemperature
            // 
            this.cbColourTemperature.FormattingEnabled = true;
            this.cbColourTemperature.Location = new System.Drawing.Point(10, 41);
            this.cbColourTemperature.Name = "cbColourTemperature";
            this.cbColourTemperature.Size = new System.Drawing.Size(200, 21);
            this.cbColourTemperature.TabIndex = 2;
            this.cbColourTemperature.SelectedIndexChanged += new System.EventHandler(this.cbColourTemperature_SelectedIndexChanged);
            // 
            // BrightnessControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 70);
            this.Controls.Add(this.cbColourTemperature);
            this.Controls.Add(this.lBrightness);
            this.Controls.Add(this.pbBrightness);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "BrightnessControlForm";
            this.Text = "BrightnessControlForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar pbBrightness;
        private System.Windows.Forms.Label lBrightness;
        private System.Windows.Forms.ComboBox cbColourTemperature;
    }
}