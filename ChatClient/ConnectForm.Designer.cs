namespace ChatClient
{
    partial class ConnectForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            labelIp = new Label();
            labelPort = new Label();
            labelLogin = new Label();
            textBoxIp = new TextBox();
            numericUpDownPort = new NumericUpDown();
            textBoxLogin = new TextBox();
            buttonConnect = new Button();
            ((System.ComponentModel.ISupportInitialize)numericUpDownPort).BeginInit();
            SuspendLayout();
            // 
            // labelIp
            // 
            labelIp.AutoSize = true;
            labelIp.Location = new Point(22, 22);
            labelIp.Name = "labelIp";
            labelIp.Size = new Size(17, 15);
            labelIp.TabIndex = 0;
            labelIp.Text = "IP";
            // 
            // labelPort
            // 
            labelPort.AutoSize = true;
            labelPort.Location = new Point(22, 62);
            labelPort.Name = "labelPort";
            labelPort.Size = new Size(29, 15);
            labelPort.TabIndex = 1;
            labelPort.Text = "Port";
            // 
            // labelLogin
            // 
            labelLogin.AutoSize = true;
            labelLogin.Location = new Point(22, 102);
            labelLogin.Name = "labelLogin";
            labelLogin.Size = new Size(37, 15);
            labelLogin.TabIndex = 2;
            labelLogin.Text = "Login";
            // 
            // textBoxIp
            // 
            textBoxIp.Location = new Point(90, 19);
            textBoxIp.Name = "textBoxIp";
            textBoxIp.Size = new Size(220, 23);
            textBoxIp.TabIndex = 3;
            // 
            // numericUpDownPort
            // 
            numericUpDownPort.Location = new Point(90, 59);
            numericUpDownPort.Name = "numericUpDownPort";
            numericUpDownPort.Size = new Size(220, 23);
            numericUpDownPort.TabIndex = 4;
            // 
            // textBoxLogin
            // 
            textBoxLogin.Location = new Point(90, 99);
            textBoxLogin.Name = "textBoxLogin";
            textBoxLogin.Size = new Size(220, 23);
            textBoxLogin.TabIndex = 5;
            // 
            // buttonConnect
            // 
            buttonConnect.Location = new Point(90, 139);
            buttonConnect.Name = "buttonConnect";
            buttonConnect.Size = new Size(220, 32);
            buttonConnect.TabIndex = 6;
            buttonConnect.Text = "Подключиться";
            buttonConnect.UseVisualStyleBackColor = true;
            buttonConnect.Click += buttonConnect_Click;
            // 
            // ConnectForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(350, 200);
            Controls.Add(buttonConnect);
            Controls.Add(textBoxLogin);
            Controls.Add(numericUpDownPort);
            Controls.Add(textBoxIp);
            Controls.Add(labelLogin);
            Controls.Add(labelPort);
            Controls.Add(labelIp);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            Name = "ConnectForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Подключение";
            ((System.ComponentModel.ISupportInitialize)numericUpDownPort).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label labelIp;
        private Label labelPort;
        private Label labelLogin;
        private TextBox textBoxIp;
        private NumericUpDown numericUpDownPort;
        private TextBox textBoxLogin;
        private Button buttonConnect;
    }
}