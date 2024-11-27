
namespace EF_SerialCommunication_Tester
{
    partial class CommunicationTester
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
            this.portSelector = new System.Windows.Forms.ComboBox();
            this.baudSelector = new System.Windows.Forms.ComboBox();
            this.statusTextBox = new System.Windows.Forms.RichTextBox();
            this.trackBarLC = new System.Windows.Forms.TrackBar();
            this.trackBarRC = new System.Windows.Forms.TrackBar();
            this.trackBarLS = new System.Windows.Forms.TrackBar();
            this.trackBarRS = new System.Windows.Forms.TrackBar();
            this.trackBarLO = new System.Windows.Forms.TrackBar();
            this.trackBarRO = new System.Windows.Forms.TrackBar();
            this.trackBarLI = new System.Windows.Forms.TrackBar();
            this.trackBarRI = new System.Windows.Forms.TrackBar();
            this.trackBarLE = new System.Windows.Forms.TrackBar();
            this.trackBarRE = new System.Windows.Forms.TrackBar();
            this.trackBarAB = new System.Windows.Forms.TrackBar();
            this.trackBarFI = new System.Windows.Forms.TrackBar();
            this.LabelLC = new System.Windows.Forms.Label();
            this.labelRC = new System.Windows.Forms.Label();
            this.labelLS = new System.Windows.Forms.Label();
            this.labelRS = new System.Windows.Forms.Label();
            this.labelLO = new System.Windows.Forms.Label();
            this.labelRO = new System.Windows.Forms.Label();
            this.labelLI = new System.Windows.Forms.Label();
            this.labelRI = new System.Windows.Forms.Label();
            this.labelLE = new System.Windows.Forms.Label();
            this.labelRE = new System.Windows.Forms.Label();
            this.labelAB = new System.Windows.Forms.Label();
            this.labelFI = new System.Windows.Forms.Label();
            this.modeSelector = new System.Windows.Forms.ComboBox();
            this.presetSelector = new System.Windows.Forms.ComboBox();
            this.LEDselector = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRI)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFI)).BeginInit();
            this.SuspendLayout();
            // 
            // portSelector
            // 
            this.portSelector.FormattingEnabled = true;
            this.portSelector.Location = new System.Drawing.Point(12, 12);
            this.portSelector.Name = "portSelector";
            this.portSelector.Size = new System.Drawing.Size(121, 24);
            this.portSelector.TabIndex = 0;
            this.portSelector.Text = "COM-Port";
            this.portSelector.SelectedIndexChanged += new System.EventHandler(this.portSelector_SelectedIndexChanged);
            // 
            // baudSelector
            // 
            this.baudSelector.FormattingEnabled = true;
            this.baudSelector.Location = new System.Drawing.Point(139, 12);
            this.baudSelector.Name = "baudSelector";
            this.baudSelector.Size = new System.Drawing.Size(121, 24);
            this.baudSelector.TabIndex = 1;
            this.baudSelector.Text = "Baudrate";
            this.baudSelector.SelectedIndexChanged += new System.EventHandler(this.baudSelector_SelectedIndexChanged);
            // 
            // statusTextBox
            // 
            this.statusTextBox.Location = new System.Drawing.Point(626, 12);
            this.statusTextBox.Name = "statusTextBox";
            this.statusTextBox.Size = new System.Drawing.Size(463, 528);
            this.statusTextBox.TabIndex = 2;
            this.statusTextBox.Text = "";
            // 
            // trackBarLC
            // 
            this.trackBarLC.Location = new System.Drawing.Point(12, 58);
            this.trackBarLC.Maximum = 255;
            this.trackBarLC.Name = "trackBarLC";
            this.trackBarLC.Size = new System.Drawing.Size(121, 56);
            this.trackBarLC.SmallChange = 5;
            this.trackBarLC.TabIndex = 5;
            this.trackBarLC.Scroll += new System.EventHandler(this.trackBarLC_Scroll);
            // 
            // trackBarRC
            // 
            this.trackBarRC.Location = new System.Drawing.Point(12, 120);
            this.trackBarRC.Maximum = 255;
            this.trackBarRC.Name = "trackBarRC";
            this.trackBarRC.Size = new System.Drawing.Size(121, 56);
            this.trackBarRC.SmallChange = 5;
            this.trackBarRC.TabIndex = 6;
            this.trackBarRC.Scroll += new System.EventHandler(this.trackBarRC_Scroll);
            // 
            // trackBarLS
            // 
            this.trackBarLS.Location = new System.Drawing.Point(12, 182);
            this.trackBarLS.Maximum = 255;
            this.trackBarLS.Name = "trackBarLS";
            this.trackBarLS.Size = new System.Drawing.Size(121, 56);
            this.trackBarLS.SmallChange = 5;
            this.trackBarLS.TabIndex = 7;
            this.trackBarLS.Scroll += new System.EventHandler(this.trackBarLS_Scroll);
            // 
            // trackBarRS
            // 
            this.trackBarRS.Location = new System.Drawing.Point(12, 244);
            this.trackBarRS.Maximum = 255;
            this.trackBarRS.Name = "trackBarRS";
            this.trackBarRS.Size = new System.Drawing.Size(121, 56);
            this.trackBarRS.SmallChange = 5;
            this.trackBarRS.TabIndex = 8;
            this.trackBarRS.Scroll += new System.EventHandler(this.trackBarRS_Scroll);
            // 
            // trackBarLO
            // 
            this.trackBarLO.Location = new System.Drawing.Point(12, 306);
            this.trackBarLO.Maximum = 255;
            this.trackBarLO.Name = "trackBarLO";
            this.trackBarLO.Size = new System.Drawing.Size(121, 56);
            this.trackBarLO.SmallChange = 5;
            this.trackBarLO.TabIndex = 9;
            this.trackBarLO.Scroll += new System.EventHandler(this.trackBarLO_Scroll);
            // 
            // trackBarRO
            // 
            this.trackBarRO.Location = new System.Drawing.Point(12, 368);
            this.trackBarRO.Maximum = 255;
            this.trackBarRO.Name = "trackBarRO";
            this.trackBarRO.Size = new System.Drawing.Size(121, 56);
            this.trackBarRO.SmallChange = 5;
            this.trackBarRO.TabIndex = 10;
            this.trackBarRO.Scroll += new System.EventHandler(this.trackBarRO_Scroll);
            // 
            // trackBarLI
            // 
            this.trackBarLI.Location = new System.Drawing.Point(334, 58);
            this.trackBarLI.Maximum = 255;
            this.trackBarLI.Name = "trackBarLI";
            this.trackBarLI.Size = new System.Drawing.Size(121, 56);
            this.trackBarLI.SmallChange = 5;
            this.trackBarLI.TabIndex = 11;
            this.trackBarLI.Scroll += new System.EventHandler(this.trackBarLI_Scroll);
            // 
            // trackBarRI
            // 
            this.trackBarRI.Location = new System.Drawing.Point(334, 120);
            this.trackBarRI.Maximum = 255;
            this.trackBarRI.Name = "trackBarRI";
            this.trackBarRI.Size = new System.Drawing.Size(121, 56);
            this.trackBarRI.SmallChange = 5;
            this.trackBarRI.TabIndex = 12;
            this.trackBarRI.Scroll += new System.EventHandler(this.trackBarRI_Scroll);
            // 
            // trackBarLE
            // 
            this.trackBarLE.Location = new System.Drawing.Point(334, 182);
            this.trackBarLE.Maximum = 255;
            this.trackBarLE.Name = "trackBarLE";
            this.trackBarLE.Size = new System.Drawing.Size(121, 56);
            this.trackBarLE.SmallChange = 5;
            this.trackBarLE.TabIndex = 13;
            this.trackBarLE.Scroll += new System.EventHandler(this.trackBarLE_Scroll);
            // 
            // trackBarRE
            // 
            this.trackBarRE.Location = new System.Drawing.Point(334, 244);
            this.trackBarRE.Maximum = 255;
            this.trackBarRE.Name = "trackBarRE";
            this.trackBarRE.Size = new System.Drawing.Size(121, 56);
            this.trackBarRE.SmallChange = 5;
            this.trackBarRE.TabIndex = 14;
            this.trackBarRE.Scroll += new System.EventHandler(this.trackBarRE_Scroll);
            // 
            // trackBarAB
            // 
            this.trackBarAB.Location = new System.Drawing.Point(334, 306);
            this.trackBarAB.Maximum = 255;
            this.trackBarAB.Name = "trackBarAB";
            this.trackBarAB.Size = new System.Drawing.Size(121, 56);
            this.trackBarAB.SmallChange = 5;
            this.trackBarAB.TabIndex = 15;
            this.trackBarAB.Scroll += new System.EventHandler(this.trackBarAB_Scroll);
            // 
            // trackBarFI
            // 
            this.trackBarFI.Location = new System.Drawing.Point(334, 368);
            this.trackBarFI.Maximum = 255;
            this.trackBarFI.Name = "trackBarFI";
            this.trackBarFI.Size = new System.Drawing.Size(121, 56);
            this.trackBarFI.SmallChange = 5;
            this.trackBarFI.TabIndex = 16;
            this.trackBarFI.Scroll += new System.EventHandler(this.trackBarFI_Scroll);
            // 
            // LabelLC
            // 
            this.LabelLC.AutoSize = true;
            this.LabelLC.Location = new System.Drawing.Point(139, 58);
            this.LabelLC.Name = "LabelLC";
            this.LabelLC.Size = new System.Drawing.Size(82, 17);
            this.LabelLC.TabIndex = 17;
            this.LabelLC.Text = "Left Canard";
            // 
            // labelRC
            // 
            this.labelRC.AutoSize = true;
            this.labelRC.Location = new System.Drawing.Point(139, 120);
            this.labelRC.Name = "labelRC";
            this.labelRC.Size = new System.Drawing.Size(91, 17);
            this.labelRC.TabIndex = 18;
            this.labelRC.Text = "Right Canard";
            // 
            // labelLS
            // 
            this.labelLS.AutoSize = true;
            this.labelLS.Location = new System.Drawing.Point(139, 182);
            this.labelLS.Name = "labelLS";
            this.labelLS.Size = new System.Drawing.Size(60, 17);
            this.labelLS.TabIndex = 19;
            this.labelLS.Text = "Left Slat";
            // 
            // labelRS
            // 
            this.labelRS.AutoSize = true;
            this.labelRS.Location = new System.Drawing.Point(139, 244);
            this.labelRS.Name = "labelRS";
            this.labelRS.Size = new System.Drawing.Size(69, 17);
            this.labelRS.TabIndex = 20;
            this.labelRS.Text = "Right Slat";
            // 
            // labelLO
            // 
            this.labelLO.AutoSize = true;
            this.labelLO.Location = new System.Drawing.Point(139, 306);
            this.labelLO.Name = "labelLO";
            this.labelLO.Size = new System.Drawing.Size(127, 17);
            this.labelLO.TabIndex = 21;
            this.labelLO.Text = "Left Outboard Flap";
            // 
            // labelRO
            // 
            this.labelRO.AutoSize = true;
            this.labelRO.Location = new System.Drawing.Point(139, 368);
            this.labelRO.Name = "labelRO";
            this.labelRO.Size = new System.Drawing.Size(136, 17);
            this.labelRO.TabIndex = 22;
            this.labelRO.Text = "Right Outboard Flap";
            // 
            // labelLI
            // 
            this.labelLI.AutoSize = true;
            this.labelLI.Location = new System.Drawing.Point(461, 58);
            this.labelLI.Name = "labelLI";
            this.labelLI.Size = new System.Drawing.Size(115, 17);
            this.labelLI.TabIndex = 23;
            this.labelLI.Text = "Left Inboard Flap";
            // 
            // labelRI
            // 
            this.labelRI.AutoSize = true;
            this.labelRI.Location = new System.Drawing.Point(461, 120);
            this.labelRI.Name = "labelRI";
            this.labelRI.Size = new System.Drawing.Size(124, 17);
            this.labelRI.TabIndex = 24;
            this.labelRI.Text = "Right Inboard Flap";
            // 
            // labelLE
            // 
            this.labelLE.AutoSize = true;
            this.labelLE.Location = new System.Drawing.Point(461, 182);
            this.labelLE.Name = "labelLE";
            this.labelLE.Size = new System.Drawing.Size(80, 17);
            this.labelLE.TabIndex = 25;
            this.labelLE.Text = "Left Engine";
            // 
            // labelRE
            // 
            this.labelRE.AutoSize = true;
            this.labelRE.Location = new System.Drawing.Point(461, 244);
            this.labelRE.Name = "labelRE";
            this.labelRE.Size = new System.Drawing.Size(89, 17);
            this.labelRE.TabIndex = 26;
            this.labelRE.Text = "Right Engine";
            // 
            // labelAB
            // 
            this.labelAB.AutoSize = true;
            this.labelAB.Location = new System.Drawing.Point(461, 306);
            this.labelAB.Name = "labelAB";
            this.labelAB.Size = new System.Drawing.Size(66, 17);
            this.labelAB.TabIndex = 27;
            this.labelAB.Text = "Air Break";
            // 
            // labelFI
            // 
            this.labelFI.AutoSize = true;
            this.labelFI.Location = new System.Drawing.Point(461, 368);
            this.labelFI.Name = "labelFI";
            this.labelFI.Size = new System.Drawing.Size(43, 17);
            this.labelFI.TabIndex = 28;
            this.labelFI.Text = "Finne";
            // 
            // modeSelector
            // 
            this.modeSelector.FormattingEnabled = true;
            this.modeSelector.Location = new System.Drawing.Point(266, 12);
            this.modeSelector.Name = "modeSelector";
            this.modeSelector.Size = new System.Drawing.Size(121, 24);
            this.modeSelector.TabIndex = 30;
            this.modeSelector.Text = "Mode";
            this.modeSelector.SelectedIndexChanged += new System.EventHandler(this.modeSelector_SelectedIndexChanged);
            // 
            // presetSelector
            // 
            this.presetSelector.FormattingEnabled = true;
            this.presetSelector.Location = new System.Drawing.Point(393, 12);
            this.presetSelector.Name = "presetSelector";
            this.presetSelector.Size = new System.Drawing.Size(121, 24);
            this.presetSelector.TabIndex = 31;
            this.presetSelector.Text = "Preset";
            this.presetSelector.SelectedIndexChanged += new System.EventHandler(this.presetSelector_SelectedIndexChanged);
            // 
            // LEDselector
            // 
            this.LEDselector.FormattingEnabled = true;
            this.LEDselector.Location = new System.Drawing.Point(12, 514);
            this.LEDselector.Name = "LEDselector";
            this.LEDselector.Size = new System.Drawing.Size(121, 24);
            this.LEDselector.TabIndex = 32;
            this.LEDselector.Text = "LED Modus";
            this.LEDselector.SelectedIndexChanged += new System.EventHandler(this.LEDselector_SelectedIndexChanged_1);
            // 
            // CommunicationTester
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1101, 552);
            this.Controls.Add(this.LEDselector);
            this.Controls.Add(this.presetSelector);
            this.Controls.Add(this.modeSelector);
            this.Controls.Add(this.labelFI);
            this.Controls.Add(this.labelAB);
            this.Controls.Add(this.labelRE);
            this.Controls.Add(this.labelLE);
            this.Controls.Add(this.labelRI);
            this.Controls.Add(this.labelLI);
            this.Controls.Add(this.labelRO);
            this.Controls.Add(this.labelLO);
            this.Controls.Add(this.labelRS);
            this.Controls.Add(this.labelLS);
            this.Controls.Add(this.labelRC);
            this.Controls.Add(this.LabelLC);
            this.Controls.Add(this.trackBarFI);
            this.Controls.Add(this.trackBarAB);
            this.Controls.Add(this.trackBarRE);
            this.Controls.Add(this.trackBarLE);
            this.Controls.Add(this.trackBarRI);
            this.Controls.Add(this.trackBarLI);
            this.Controls.Add(this.trackBarRO);
            this.Controls.Add(this.trackBarLO);
            this.Controls.Add(this.trackBarRS);
            this.Controls.Add(this.trackBarLS);
            this.Controls.Add(this.trackBarRC);
            this.Controls.Add(this.trackBarLC);
            this.Controls.Add(this.statusTextBox);
            this.Controls.Add(this.baudSelector);
            this.Controls.Add(this.portSelector);
            this.Name = "CommunicationTester";
            this.Text = "EF Model Serial-Communication Tester";
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRI)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarLE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarRE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarAB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFI)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox portSelector;
        private System.Windows.Forms.ComboBox baudSelector;
        private System.Windows.Forms.RichTextBox statusTextBox;
        private System.Windows.Forms.TrackBar trackBarLC;
        private System.Windows.Forms.TrackBar trackBarRC;
        private System.Windows.Forms.TrackBar trackBarLS;
        private System.Windows.Forms.TrackBar trackBarRS;
        private System.Windows.Forms.TrackBar trackBarLO;
        private System.Windows.Forms.TrackBar trackBarRO;
        private System.Windows.Forms.TrackBar trackBarLI;
        private System.Windows.Forms.TrackBar trackBarRI;
        private System.Windows.Forms.TrackBar trackBarLE;
        private System.Windows.Forms.TrackBar trackBarRE;
        private System.Windows.Forms.TrackBar trackBarAB;
        private System.Windows.Forms.TrackBar trackBarFI;
        private System.Windows.Forms.Label LabelLC;
        private System.Windows.Forms.Label labelRC;
        private System.Windows.Forms.Label labelLS;
        private System.Windows.Forms.Label labelRS;
        private System.Windows.Forms.Label labelLO;
        private System.Windows.Forms.Label labelRO;
        private System.Windows.Forms.Label labelLI;
        private System.Windows.Forms.Label labelRI;
        private System.Windows.Forms.Label labelLE;
        private System.Windows.Forms.Label labelRE;
        private System.Windows.Forms.Label labelAB;
        private System.Windows.Forms.Label labelFI;
        private System.Windows.Forms.ComboBox modeSelector;
        private System.Windows.Forms.ComboBox presetSelector;
        private System.Windows.Forms.ComboBox LEDselector;
    }
}

