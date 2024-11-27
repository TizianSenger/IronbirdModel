
namespace TCP_Tester
{
    partial class TCP_Communication_Tester
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.modeSelector = new System.Windows.Forms.ComboBox();
            this.trackBarLC = new System.Windows.Forms.TrackBar();
            this.trackBarLS = new System.Windows.Forms.TrackBar();
            this.trackBarLI = new System.Windows.Forms.TrackBar();
            this.trackBarLO = new System.Windows.Forms.TrackBar();
            this.trackBarAB = new System.Windows.Forms.TrackBar();
            this.trackBarLE = new System.Windows.Forms.TrackBar();
            this.labelLC = new System.Windows.Forms.Label();
            this.labelLS = new System.Windows.Forms.Label();
            this.labelLI = new System.Windows.Forms.Label();
            this.labelLO = new System.Windows.Forms.Label();
            this.labelAB = new System.Windows.Forms.Label();
            this.labelLE = new System.Windows.Forms.Label();
            this.labelRE = new System.Windows.Forms.Label();
            this.labelFI = new System.Windows.Forms.Label();
            this.labelRO = new System.Windows.Forms.Label();
            this.labelRI = new System.Windows.Forms.Label();
            this.labelRS = new System.Windows.Forms.Label();
            this.labelRC = new System.Windows.Forms.Label();
            this.trackBarRE = new System.Windows.Forms.TrackBar();
            this.trackBarFI = new System.Windows.Forms.TrackBar();
            this.trackBarRO = new System.Windows.Forms.TrackBar();
            this.trackBarRI = new System.Windows.Forms.TrackBar();
            this.trackBarRS = new System.Windows.Forms.TrackBar();
            this.trackBarRC = new System.Windows.Forms.TrackBar();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.portSelector = new System.Windows.Forms.TextBox();
            this.ipSelector = new System.Windows.Forms.TextBox();
            this.clearButton = new System.Windows.Forms.Button();
            this.presetSelector = new System.Windows.Forms.ComboBox();
            this.disconButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRC)).BeginInit();
            this.SuspendLayout();
            // 
            // modeSelector
            // 
            this.modeSelector.FormattingEnabled = true;
            this.modeSelector.Location = new System.Drawing.Point(167, 9);
            this.modeSelector.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.modeSelector.Name = "modeSelector";
            this.modeSelector.Size = new System.Drawing.Size(92, 21);
            this.modeSelector.TabIndex = 3;
            this.modeSelector.Text = "Mode";
            this.modeSelector.SelectedIndexChanged += new System.EventHandler(this.modeSelector_SelectedIndexChanged);
            // 
            // trackBarLC
            // 
            this.trackBarLC.Location = new System.Drawing.Point(9, 50);
            this.trackBarLC.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarLC.Maximum = 180;
            this.trackBarLC.Name = "trackBarLC";
            this.trackBarLC.Size = new System.Drawing.Size(91, 45);
            this.trackBarLC.SmallChange = 5;
            this.trackBarLC.TabIndex = 4;
            this.trackBarLC.Scroll += new System.EventHandler(this.trackBarLC_Scroll);
            // 
            // trackBarLS
            // 
            this.trackBarLS.Location = new System.Drawing.Point(9, 101);
            this.trackBarLS.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarLS.Maximum = 180;
            this.trackBarLS.Name = "trackBarLS";
            this.trackBarLS.Size = new System.Drawing.Size(91, 45);
            this.trackBarLS.SmallChange = 5;
            this.trackBarLS.TabIndex = 6;
            this.trackBarLS.Scroll += new System.EventHandler(this.trackBarLS_Scroll);
            // 
            // trackBarLI
            // 
            this.trackBarLI.Location = new System.Drawing.Point(9, 151);
            this.trackBarLI.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarLI.Maximum = 180;
            this.trackBarLI.Name = "trackBarLI";
            this.trackBarLI.Size = new System.Drawing.Size(91, 45);
            this.trackBarLI.SmallChange = 5;
            this.trackBarLI.TabIndex = 8;
            this.trackBarLI.Scroll += new System.EventHandler(this.trackBarLI_Scroll);
            // 
            // trackBarLO
            // 
            this.trackBarLO.Location = new System.Drawing.Point(9, 202);
            this.trackBarLO.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarLO.Maximum = 180;
            this.trackBarLO.Name = "trackBarLO";
            this.trackBarLO.Size = new System.Drawing.Size(91, 45);
            this.trackBarLO.SmallChange = 5;
            this.trackBarLO.TabIndex = 10;
            this.trackBarLO.Scroll += new System.EventHandler(this.trackBarLO_Scroll);
            // 
            // trackBarAB
            // 
            this.trackBarAB.Location = new System.Drawing.Point(9, 252);
            this.trackBarAB.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarAB.Maximum = 180;
            this.trackBarAB.Name = "trackBarAB";
            this.trackBarAB.Size = new System.Drawing.Size(91, 45);
            this.trackBarAB.SmallChange = 5;
            this.trackBarAB.TabIndex = 12;
            this.trackBarAB.Scroll += new System.EventHandler(this.trackBarAB_Scroll);
            // 
            // trackBarLE
            // 
            this.trackBarLE.Location = new System.Drawing.Point(9, 302);
            this.trackBarLE.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarLE.Maximum = 180;
            this.trackBarLE.Name = "trackBarLE";
            this.trackBarLE.Size = new System.Drawing.Size(91, 45);
            this.trackBarLE.SmallChange = 5;
            this.trackBarLE.TabIndex = 14;
            this.trackBarLE.Scroll += new System.EventHandler(this.trackBarLE_Scroll);
            // 
            // labelLC
            // 
            this.labelLC.AutoSize = true;
            this.labelLC.Location = new System.Drawing.Point(104, 50);
            this.labelLC.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLC.Name = "labelLC";
            this.labelLC.Size = new System.Drawing.Size(62, 13);
            this.labelLC.TabIndex = 8;
            this.labelLC.Text = "Left Canard";
            // 
            // labelLS
            // 
            this.labelLS.AutoSize = true;
            this.labelLS.Location = new System.Drawing.Point(104, 101);
            this.labelLS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLS.Name = "labelLS";
            this.labelLS.Size = new System.Drawing.Size(46, 13);
            this.labelLS.TabIndex = 9;
            this.labelLS.Text = "Left Slat";
            // 
            // labelLI
            // 
            this.labelLI.AutoSize = true;
            this.labelLI.Location = new System.Drawing.Point(104, 151);
            this.labelLI.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLI.Name = "labelLI";
            this.labelLI.Size = new System.Drawing.Size(60, 13);
            this.labelLI.TabIndex = 10;
            this.labelLI.Text = "Left In Flap";
            // 
            // labelLO
            // 
            this.labelLO.AutoSize = true;
            this.labelLO.Location = new System.Drawing.Point(104, 202);
            this.labelLO.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLO.Name = "labelLO";
            this.labelLO.Size = new System.Drawing.Size(68, 13);
            this.labelLO.TabIndex = 11;
            this.labelLO.Text = "Left Out Flap";
            // 
            // labelAB
            // 
            this.labelAB.AutoSize = true;
            this.labelAB.Location = new System.Drawing.Point(104, 252);
            this.labelAB.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAB.Name = "labelAB";
            this.labelAB.Size = new System.Drawing.Size(50, 13);
            this.labelAB.TabIndex = 12;
            this.labelAB.Text = "Air Break";
            // 
            // labelLE
            // 
            this.labelLE.AutoSize = true;
            this.labelLE.Location = new System.Drawing.Point(104, 302);
            this.labelLE.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelLE.Name = "labelLE";
            this.labelLE.Size = new System.Drawing.Size(61, 13);
            this.labelLE.TabIndex = 13;
            this.labelLE.Text = "Left Engine";
            // 
            // labelRE
            // 
            this.labelRE.AutoSize = true;
            this.labelRE.Location = new System.Drawing.Point(298, 302);
            this.labelRE.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRE.Name = "labelRE";
            this.labelRE.Size = new System.Drawing.Size(68, 13);
            this.labelRE.TabIndex = 25;
            this.labelRE.Text = "Right Engine";
            // 
            // labelFI
            // 
            this.labelFI.AutoSize = true;
            this.labelFI.Location = new System.Drawing.Point(298, 252);
            this.labelFI.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelFI.Name = "labelFI";
            this.labelFI.Size = new System.Drawing.Size(21, 13);
            this.labelFI.TabIndex = 24;
            this.labelFI.Text = "Fin";
            // 
            // labelRO
            // 
            this.labelRO.AutoSize = true;
            this.labelRO.Location = new System.Drawing.Point(298, 202);
            this.labelRO.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRO.Name = "labelRO";
            this.labelRO.Size = new System.Drawing.Size(75, 13);
            this.labelRO.TabIndex = 23;
            this.labelRO.Text = "Right Out Flap";
            // 
            // labelRI
            // 
            this.labelRI.AutoSize = true;
            this.labelRI.Location = new System.Drawing.Point(298, 151);
            this.labelRI.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRI.Name = "labelRI";
            this.labelRI.Size = new System.Drawing.Size(67, 13);
            this.labelRI.TabIndex = 22;
            this.labelRI.Text = "Right In Flap";
            // 
            // labelRS
            // 
            this.labelRS.AutoSize = true;
            this.labelRS.Location = new System.Drawing.Point(298, 101);
            this.labelRS.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRS.Name = "labelRS";
            this.labelRS.Size = new System.Drawing.Size(53, 13);
            this.labelRS.TabIndex = 21;
            this.labelRS.Text = "Right Slat";
            // 
            // labelRC
            // 
            this.labelRC.AutoSize = true;
            this.labelRC.Location = new System.Drawing.Point(298, 50);
            this.labelRC.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelRC.Name = "labelRC";
            this.labelRC.Size = new System.Drawing.Size(69, 13);
            this.labelRC.TabIndex = 20;
            this.labelRC.Text = "Right Canard";
            // 
            // trackBarRE
            // 
            this.trackBarRE.Location = new System.Drawing.Point(203, 302);
            this.trackBarRE.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarRE.Maximum = 180;
            this.trackBarRE.Name = "trackBarRE";
            this.trackBarRE.Size = new System.Drawing.Size(91, 45);
            this.trackBarRE.SmallChange = 5;
            this.trackBarRE.TabIndex = 15;
            this.trackBarRE.Scroll += new System.EventHandler(this.trackBarRE_Scroll);
            // 
            // trackBarFI
            // 
            this.trackBarFI.Location = new System.Drawing.Point(203, 252);
            this.trackBarFI.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarFI.Maximum = 180;
            this.trackBarFI.Name = "trackBarFI";
            this.trackBarFI.Size = new System.Drawing.Size(91, 45);
            this.trackBarFI.SmallChange = 5;
            this.trackBarFI.TabIndex = 13;
            this.trackBarFI.Scroll += new System.EventHandler(this.trackBarFI_Scroll);
            // 
            // trackBarRO
            // 
            this.trackBarRO.Location = new System.Drawing.Point(203, 202);
            this.trackBarRO.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarRO.Maximum = 180;
            this.trackBarRO.Name = "trackBarRO";
            this.trackBarRO.Size = new System.Drawing.Size(91, 45);
            this.trackBarRO.SmallChange = 5;
            this.trackBarRO.TabIndex = 11;
            this.trackBarRO.Scroll += new System.EventHandler(this.trackBarRO_Scroll);
            // 
            // trackBarRI
            // 
            this.trackBarRI.Location = new System.Drawing.Point(203, 151);
            this.trackBarRI.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarRI.Maximum = 180;
            this.trackBarRI.Name = "trackBarRI";
            this.trackBarRI.Size = new System.Drawing.Size(91, 45);
            this.trackBarRI.SmallChange = 5;
            this.trackBarRI.TabIndex = 9;
            this.trackBarRI.Scroll += new System.EventHandler(this.trackBarRI_Scroll);
            // 
            // trackBarRS
            // 
            this.trackBarRS.Location = new System.Drawing.Point(203, 101);
            this.trackBarRS.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarRS.Maximum = 180;
            this.trackBarRS.Name = "trackBarRS";
            this.trackBarRS.Size = new System.Drawing.Size(91, 45);
            this.trackBarRS.SmallChange = 5;
            this.trackBarRS.TabIndex = 7;
            this.trackBarRS.Scroll += new System.EventHandler(this.trackBarRS_Scroll);
            // 
            // trackBarRC
            // 
            this.trackBarRC.Location = new System.Drawing.Point(203, 50);
            this.trackBarRC.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.trackBarRC.Maximum = 180;
            this.trackBarRC.Name = "trackBarRC";
            this.trackBarRC.Size = new System.Drawing.Size(91, 45);
            this.trackBarRC.SmallChange = 5;
            this.trackBarRC.TabIndex = 5;
            this.trackBarRC.Value = 5;
            this.trackBarRC.Scroll += new System.EventHandler(this.trackBarRC_Scroll);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(396, 9);
            this.statusTextBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(420, 308);
            this.statusTextBox.TabIndex = 26;
            this.statusTextBox.Text = "";
            // 
            // portSelector
            // 
            this.portSelector.Location = new System.Drawing.Point(89, 10);
            this.portSelector.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.portSelector.Name = "portSelector";
            this.portSelector.Size = new System.Drawing.Size(76, 20);
            this.portSelector.TabIndex = 2;
            this.portSelector.Text = "Port";
            // 
            // ipSelector
            // 
            this.ipSelector.Location = new System.Drawing.Point(10, 10);
            this.ipSelector.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.ipSelector.Name = "ipSelector";
            this.ipSelector.Size = new System.Drawing.Size(76, 20);
            this.ipSelector.TabIndex = 1;
            this.ipSelector.Text = "Server IP";
            // 
            // clearButton
            // 
            this.clearButton.Location = new System.Drawing.Point(396, 320);
            this.clearButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.clearButton.Name = "clearButton";
            this.clearButton.Size = new System.Drawing.Size(56, 19);
            this.clearButton.TabIndex = 16;
            this.clearButton.Text = "Löschen";
            this.clearButton.UseVisualStyleBackColor = true;
            this.clearButton.Click += new System.EventHandler(this.clearButton_Click);
            // 
            // presetSelector
            // 
            this.presetSelector.FormattingEnabled = true;
            this.presetSelector.Location = new System.Drawing.Point(261, 9);
            this.presetSelector.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.presetSelector.Name = "presetSelector";
            this.presetSelector.Size = new System.Drawing.Size(92, 21);
            this.presetSelector.TabIndex = 17;
            this.presetSelector.Text = "Preset";
            this.presetSelector.SelectedIndexChanged += new System.EventHandler(this.presetSelector_SelectedIndexChanged);
            // 
            // disconButton
            // 
            this.disconButton.Location = new System.Drawing.Point(456, 320);
            this.disconButton.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.disconButton.Name = "disconButton";
            this.disconButton.Size = new System.Drawing.Size(56, 19);
            this.disconButton.TabIndex = 27;
            this.disconButton.Text = "Trennen";
            this.disconButton.UseVisualStyleBackColor = true;
            this.disconButton.Click += new System.EventHandler(this.disconButton_Click);
            // 
            // TCP_Communication_Tester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 348);
            this.Controls.Add(this.disconButton);
            this.Controls.Add(this.presetSelector);
            this.Controls.Add(this.clearButton);
            this.Controls.Add(this.ipSelector);
            this.Controls.Add(this.portSelector);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.labelRE);
            this.Controls.Add(this.labelFI);
            this.Controls.Add(this.labelRO);
            this.Controls.Add(this.labelRI);
            this.Controls.Add(this.labelRS);
            this.Controls.Add(this.labelRC);
            this.Controls.Add(this.trackBarRE);
            this.Controls.Add(this.trackBarFI);
            this.Controls.Add(this.trackBarRO);
            this.Controls.Add(this.trackBarRI);
            this.Controls.Add(this.trackBarRS);
            this.Controls.Add(this.trackBarRC);
            this.Controls.Add(this.labelLE);
            this.Controls.Add(this.labelAB);
            this.Controls.Add(this.labelLO);
            this.Controls.Add(this.labelLI);
            this.Controls.Add(this.labelLS);
            this.Controls.Add(this.labelLC);
            this.Controls.Add(this.trackBarLE);
            this.Controls.Add(this.trackBarAB);
            this.Controls.Add(this.trackBarLO);
            this.Controls.Add(this.trackBarLI);
            this.Controls.Add(this.trackBarLS);
            this.Controls.Add(this.trackBarLC);
            this.Controls.Add(this.modeSelector);
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.Name = "TCP_Communication_Tester";
            this.Text = "TCP Communication Tester";
            this.Load += new System.EventHandler(this.TCP_Communication_Tester_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRC)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox modeSelector;
        private System.Windows.Forms.TrackBar trackBarLC;
        private System.Windows.Forms.TrackBar trackBarLS;
        private System.Windows.Forms.TrackBar trackBarLI;
        private System.Windows.Forms.TrackBar trackBarLO;
        private System.Windows.Forms.TrackBar trackBarAB;
        private System.Windows.Forms.TrackBar trackBarLE;
        private System.Windows.Forms.Label labelLC;
        private System.Windows.Forms.Label labelLS;
        private System.Windows.Forms.Label labelLI;
        private System.Windows.Forms.Label labelLO;
        private System.Windows.Forms.Label labelAB;
        private System.Windows.Forms.Label labelLE;
        private System.Windows.Forms.Label labelRE;
        private System.Windows.Forms.Label labelFI;
        private System.Windows.Forms.Label labelRO;
        private System.Windows.Forms.Label labelRI;
        private System.Windows.Forms.Label labelRS;
        private System.Windows.Forms.Label labelRC;
        private System.Windows.Forms.TrackBar trackBarRE;
        private System.Windows.Forms.TrackBar trackBarFI;
        private System.Windows.Forms.TrackBar trackBarRO;
        private System.Windows.Forms.TrackBar trackBarRI;
        private System.Windows.Forms.TrackBar trackBarRS;
        private System.Windows.Forms.TrackBar trackBarRC;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.TextBox portSelector;
        private System.Windows.Forms.TextBox ipSelector;
        private System.Windows.Forms.Button clearButton;
        private System.Windows.Forms.ComboBox presetSelector;
        private System.Windows.Forms.Button disconButton;
    }
}

