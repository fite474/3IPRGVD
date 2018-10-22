namespace ClientApp.Gui
{
    partial class PatientTestStart
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
            this.startTestButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // startTestButton
            // 
            this.startTestButton.Location = new System.Drawing.Point(335, 293);
            this.startTestButton.Name = "startTestButton";
            this.startTestButton.Size = new System.Drawing.Size(75, 23);
            this.startTestButton.TabIndex = 0;
            this.startTestButton.Text = "Run Test";
            this.startTestButton.UseVisualStyleBackColor = true;
            this.startTestButton.Click += new System.EventHandler(this.startTestButton_Click);
            // 
            // PatientGui
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.startTestButton);
            this.Name = "PatientGui";
            this.Text = "PatientGui";
            this.Load += new System.EventHandler(this.PatientGui_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button startTestButton;
    }
}