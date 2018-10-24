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
            this.faseLabel = new System.Windows.Forms.Label();
            this.currentHearbeatLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(580, 72);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(35, 13);
            this.timeLabel.TabIndex = 0;
            this.timeLabel.Text = "label1";
            // 
            // instructionLabelRPM
            // 
            this.instructionLabelRPM.AutoSize = true;
            this.instructionLabelRPM.Location = new System.Drawing.Point(160, 72);
            this.instructionLabelRPM.Name = "instructionLabelRPM";
            this.instructionLabelRPM.Size = new System.Drawing.Size(35, 13);
            this.instructionLabelRPM.TabIndex = 1;
            this.instructionLabelRPM.Text = "label1";
            // 
            // faseLabel
            // 
            this.faseLabel.AutoSize = true;
            this.faseLabel.Location = new System.Drawing.Point(340, 166);
            this.faseLabel.Name = "faseLabel";
            this.faseLabel.Size = new System.Drawing.Size(35, 13);
            this.faseLabel.TabIndex = 2;
            this.faseLabel.Text = "label1";
            this.faseLabel.Click += new System.EventHandler(this.faseLabel_Click);
            // 
            // currentHearbeatLabel
            // 
            this.currentHearbeatLabel.AutoSize = true;
            this.currentHearbeatLabel.Location = new System.Drawing.Point(163, 284);
            this.currentHearbeatLabel.Name = "currentHearbeatLabel";
            this.currentHearbeatLabel.Size = new System.Drawing.Size(35, 13);
            this.currentHearbeatLabel.TabIndex = 3;
            this.currentHearbeatLabel.Text = "label1";
            this.currentHearbeatLabel.Click += new System.EventHandler(this.currentHearbeatLabel_Click);
            // 
            // PatientTestInstructions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.currentHearbeatLabel);
            this.Controls.Add(this.faseLabel);
            this.Controls.Add(this.instructionLabelRPM);
            this.Controls.Add(this.timeLabel);
            this.Name = "PatientTestInstructions";
            this.Text = "PatientTestInstructions";
            this.Load += new System.EventHandler(this.PatientTestInstructions_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.Label instructionLabelRPM;
        private System.Windows.Forms.Label faseLabel;
        private System.Windows.Forms.Label currentHearbeatLabel;
    }
}