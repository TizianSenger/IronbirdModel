
namespace JoystickApp
{
    partial class MainForm
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
            this.lblXValue = new System.Windows.Forms.Label();
            this.lblYValue = new System.Windows.Forms.Label();
            this.progressBarXLeft = new System.Windows.Forms.ProgressBar();
            this.progressBarXRight = new System.Windows.Forms.ProgressBar();
            this.progressBarYUp = new System.Windows.Forms.ProgressBar();
            this.progressBarYDown = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.progressBarThrottle = new System.Windows.Forms.ProgressBar();
            this.lblThrottleValue = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.chkButtonState1 = new System.Windows.Forms.CheckBox();
            this.chkButtonState2 = new System.Windows.Forms.CheckBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.progressBarRzLeft = new System.Windows.Forms.ProgressBar();
            this.progressBarRzRight = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblRZValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblXValue
            // 
            this.lblXValue.AutoSize = true;
            this.lblXValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXValue.Location = new System.Drawing.Point(12, 103);
            this.lblXValue.Name = "lblXValue";
            this.lblXValue.Size = new System.Drawing.Size(46, 17);
            this.lblXValue.TabIndex = 0;
            this.lblXValue.Text = "label1";
            // 
            // lblYValue
            // 
            this.lblYValue.AutoSize = true;
            this.lblYValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYValue.Location = new System.Drawing.Point(12, 57);
            this.lblYValue.Name = "lblYValue";
            this.lblYValue.Size = new System.Drawing.Size(46, 17);
            this.lblYValue.TabIndex = 1;
            this.lblYValue.Text = "label1";
            // 
            // progressBarXLeft
            // 
            this.progressBarXLeft.Location = new System.Drawing.Point(502, 100);
            this.progressBarXLeft.Name = "progressBarXLeft";
            this.progressBarXLeft.Size = new System.Drawing.Size(100, 23);
            this.progressBarXLeft.TabIndex = 2;
            // 
            // progressBarXRight
            // 
            this.progressBarXRight.Location = new System.Drawing.Point(262, 100);
            this.progressBarXRight.Name = "progressBarXRight";
            this.progressBarXRight.Size = new System.Drawing.Size(100, 23);
            this.progressBarXRight.TabIndex = 3;
            // 
            // progressBarYUp
            // 
            this.progressBarYUp.Location = new System.Drawing.Point(502, 54);
            this.progressBarYUp.Name = "progressBarYUp";
            this.progressBarYUp.Size = new System.Drawing.Size(100, 23);
            this.progressBarYUp.TabIndex = 4;
            // 
            // progressBarYDown
            // 
            this.progressBarYDown.Location = new System.Drawing.Point(262, 54);
            this.progressBarYDown.Name = "progressBarYDown";
            this.progressBarYDown.Size = new System.Drawing.Size(100, 23);
            this.progressBarYDown.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(440, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "DOWN";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(224, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "UP";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(444, 103);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "RIGTH";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(208, 103);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(44, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "LEFT";
            // 
            // progressBarThrottle
            // 
            this.progressBarThrottle.Location = new System.Drawing.Point(262, 210);
            this.progressBarThrottle.Name = "progressBarThrottle";
            this.progressBarThrottle.Size = new System.Drawing.Size(100, 23);
            this.progressBarThrottle.TabIndex = 10;
            // 
            // lblThrottleValue
            // 
            this.lblThrottleValue.AutoSize = true;
            this.lblThrottleValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblThrottleValue.Location = new System.Drawing.Point(12, 213);
            this.lblThrottleValue.Name = "lblThrottleValue";
            this.lblThrottleValue.Size = new System.Drawing.Size(46, 17);
            this.lblThrottleValue.TabIndex = 11;
            this.lblThrottleValue.Text = "label1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(166, 213);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 17);
            this.label5.TabIndex = 12;
            this.label5.Text = "THROTTLE";
            // 
            // chkButtonState1
            // 
            this.chkButtonState1.AutoSize = true;
            this.chkButtonState1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkButtonState1.Location = new System.Drawing.Point(454, 213);
            this.chkButtonState1.Name = "chkButtonState1";
            this.chkButtonState1.Size = new System.Drawing.Size(148, 21);
            this.chkButtonState1.TabIndex = 13;
            this.chkButtonState1.Text = "TRIGGER FRONT";
            this.chkButtonState1.UseVisualStyleBackColor = true;
            // 
            // chkButtonState2
            // 
            this.chkButtonState2.AutoSize = true;
            this.chkButtonState2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkButtonState2.Location = new System.Drawing.Point(454, 251);
            this.chkButtonState2.Name = "chkButtonState2";
            this.chkButtonState2.Size = new System.Drawing.Size(173, 21);
            this.chkButtonState2.TabIndex = 14;
            this.chkButtonState2.Text = "SHOULDER BUTTON";
            this.chkButtonState2.UseVisualStyleBackColor = true;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(15, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 15;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(145, 10);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 16;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // progressBarRzLeft
            // 
            this.progressBarRzLeft.Location = new System.Drawing.Point(502, 156);
            this.progressBarRzLeft.Name = "progressBarRzLeft";
            this.progressBarRzLeft.Size = new System.Drawing.Size(100, 23);
            this.progressBarRzLeft.TabIndex = 17;
            // 
            // progressBarRzRight
            // 
            this.progressBarRzRight.Location = new System.Drawing.Point(262, 156);
            this.progressBarRzRight.Name = "progressBarRzRight";
            this.progressBarRzRight.Size = new System.Drawing.Size(100, 23);
            this.progressBarRzRight.TabIndex = 18;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(166, 162);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(80, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "ROT LEFT";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(408, 162);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(88, 17);
            this.label7.TabIndex = 20;
            this.label7.Text = "ROT RIGHT";
            // 
            // lblRZValue
            // 
            this.lblRZValue.AutoSize = true;
            this.lblRZValue.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRZValue.Location = new System.Drawing.Point(12, 162);
            this.lblRZValue.Name = "lblRZValue";
            this.lblRZValue.Size = new System.Drawing.Size(46, 17);
            this.lblRZValue.TabIndex = 22;
            this.lblRZValue.Text = "label1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblRZValue);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBarRzRight);
            this.Controls.Add(this.progressBarRzLeft);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.chkButtonState2);
            this.Controls.Add(this.chkButtonState1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblThrottleValue);
            this.Controls.Add(this.progressBarThrottle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBarYDown);
            this.Controls.Add(this.progressBarYUp);
            this.Controls.Add(this.progressBarXRight);
            this.Controls.Add(this.progressBarXLeft);
            this.Controls.Add(this.lblYValue);
            this.Controls.Add(this.lblXValue);
            this.Name = "MainForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblXValue;
        private System.Windows.Forms.Label lblYValue;
        private System.Windows.Forms.ProgressBar progressBarXLeft;
        private System.Windows.Forms.ProgressBar progressBarXRight;
        private System.Windows.Forms.ProgressBar progressBarYUp;
        private System.Windows.Forms.ProgressBar progressBarYDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ProgressBar progressBarThrottle;
        private System.Windows.Forms.Label lblThrottleValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chkButtonState1;
        private System.Windows.Forms.CheckBox chkButtonState2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.ProgressBar progressBarRzLeft;
        private System.Windows.Forms.ProgressBar progressBarRzRight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblRZValue;
    }
}

