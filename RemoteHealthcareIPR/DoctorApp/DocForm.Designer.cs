namespace DoctorApp
{
    partial class DocForm
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.button1 = new System.Windows.Forms.Button();
            this.rpmLabel = new System.Windows.Forms.Label();
            this.hearthbeatLabel = new System.Windows.Forms.Label();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.LoadClientData = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(46, 347);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(153, 62);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // rpmLabel
            // 
            this.rpmLabel.AutoSize = true;
            this.rpmLabel.Location = new System.Drawing.Point(43, 53);
            this.rpmLabel.Name = "rpmLabel";
            this.rpmLabel.Size = new System.Drawing.Size(40, 17);
            this.rpmLabel.TabIndex = 1;
            this.rpmLabel.Text = "rpm: ";
            this.rpmLabel.Click += new System.EventHandler(this.label1_Click);
            // 
            // hearthbeatLabel
            // 
            this.hearthbeatLabel.AutoSize = true;
            this.hearthbeatLabel.Location = new System.Drawing.Point(43, 110);
            this.hearthbeatLabel.Name = "hearthbeatLabel";
            this.hearthbeatLabel.Size = new System.Drawing.Size(87, 17);
            this.hearthbeatLabel.TabIndex = 2;
            this.hearthbeatLabel.Text = "Hearthbeat: ";
            this.hearthbeatLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(254, 53);
            this.chart1.Margin = new System.Windows.Forms.Padding(4);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series1.Legend = "Legend1";
            series1.Name = "heartbeat";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            series2.Legend = "Legend1";
            series2.Name = "rpm";
            this.chart1.Series.Add(series1);
            this.chart1.Series.Add(series2);
            this.chart1.Size = new System.Drawing.Size(1021, 369);
            this.chart1.TabIndex = 4;
            this.chart1.Text = "chart1";
            // 
            // LoadClientData
            // 
            this.LoadClientData.Location = new System.Drawing.Point(46, 249);
            this.LoadClientData.Name = "LoadClientData";
            this.LoadClientData.Size = new System.Drawing.Size(153, 63);
            this.LoadClientData.TabIndex = 5;
            this.LoadClientData.Text = "Load Client Data";
            this.LoadClientData.UseVisualStyleBackColor = true;
            this.LoadClientData.Click += new System.EventHandler(this.button2_Click);
            // 
            // DocForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 469);
            this.Controls.Add(this.LoadClientData);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.hearthbeatLabel);
            this.Controls.Add(this.rpmLabel);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "DocForm";
            this.Text = "DocForm";
            this.Load += new System.EventHandler(this.DocForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label rpmLabel;
        private System.Windows.Forms.Label hearthbeatLabel;
        //private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Button LoadClientData;
    }
}