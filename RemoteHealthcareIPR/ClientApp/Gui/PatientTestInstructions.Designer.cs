namespace PatientApp.Gui
{
    partial class PatientTestInstructions
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
            this.timeLabel = new System.Windows.Forms.Label();
            this.instructionLabelRPM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(753, 66);
            this.timeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(46, 17);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "label1";
            // 
            // instructionLabelRPM
            // 
            this.instructionLabelRPM.AutoSize = true;
            this.instructionLabelRPM.Location = new System.Drawing.Point(213, 89);
            this.instructionLabelRPM.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.instructionLabelRPM.Name = "instructionLabelRPM";
            this.instructionLabelRPM.Size = new System.Drawing.Size(46, 17);
            this.instructionLabelRPM.TabIndex = 1;
            this.instructionLabelRPM.Text = "label1";
            // 
            // PatientTestInstructions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.instructionLabelRPM);
            this.Controls.Add(this.timeLabel);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PatientTestInstructions";
            this.Text = "PatientTestInstructions";
            this.Load += new System.EventHandler(this.PatientTestInstructions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label instructionLabelRPM;
    }
}