namespace ChatClient
{
    partial class ChatForm
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
            splitContainerMain = new SplitContainer();
            richTextBoxChat = new RichTextBox();
            listBoxUsers = new ListBox();
            panelBottom = new Panel();
            buttonSend = new Button();
            textBoxMessage = new TextBox();
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).BeginInit();
            splitContainerMain.Panel1.SuspendLayout();
            splitContainerMain.Panel2.SuspendLayout();
            splitContainerMain.SuspendLayout();
            panelBottom.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainerMain
            // 
            splitContainerMain.Dock = DockStyle.Fill;
            splitContainerMain.Location = new Point(0, 0);
            splitContainerMain.Margin = new Padding(3, 4, 3, 4);
            splitContainerMain.Name = "splitContainerMain";
            // 
            // splitContainerMain.Panel1
            // 
            splitContainerMain.Panel1.Controls.Add(richTextBoxChat);
            // 
            // splitContainerMain.Panel2
            // 
            splitContainerMain.Panel2.Controls.Add(listBoxUsers);
            splitContainerMain.Size = new Size(1029, 693);
            splitContainerMain.SplitterDistance = 743;
            splitContainerMain.SplitterWidth = 5;
            splitContainerMain.TabIndex = 0;
            // 
            // richTextBoxChat
            // 
            richTextBoxChat.Dock = DockStyle.Fill;
            richTextBoxChat.Location = new Point(0, 0);
            richTextBoxChat.Margin = new Padding(3, 4, 3, 4);
            richTextBoxChat.Name = "richTextBoxChat";
            richTextBoxChat.ReadOnly = true;
            richTextBoxChat.Size = new Size(743, 693);
            richTextBoxChat.TabIndex = 0;
            richTextBoxChat.Text = "";
            // 
            // listBoxUsers
            // 
            listBoxUsers.Dock = DockStyle.Fill;
            listBoxUsers.FormattingEnabled = true;
            listBoxUsers.Location = new Point(0, 0);
            listBoxUsers.Margin = new Padding(3, 4, 3, 4);
            listBoxUsers.Name = "listBoxUsers";
            listBoxUsers.Size = new Size(281, 693);
            listBoxUsers.TabIndex = 0;
            // 
            // panelBottom
            // 
            panelBottom.Controls.Add(buttonSend);
            panelBottom.Controls.Add(textBoxMessage);
            panelBottom.Dock = DockStyle.Bottom;
            panelBottom.Location = new Point(0, 693);
            panelBottom.Margin = new Padding(3, 4, 3, 4);
            panelBottom.Name = "panelBottom";
            panelBottom.Padding = new Padding(11, 13, 11, 13);
            panelBottom.Size = new Size(1029, 67);
            panelBottom.TabIndex = 1;
            // 
            // buttonSend
            // 
            buttonSend.Dock = DockStyle.Right;
            buttonSend.Location = new Point(858, 13);
            buttonSend.Margin = new Padding(3, 4, 3, 4);
            buttonSend.Name = "buttonSend";
            buttonSend.Size = new Size(160, 41);
            buttonSend.TabIndex = 1;
            buttonSend.Text = "Отправить";
            buttonSend.UseVisualStyleBackColor = true;
            buttonSend.Click += buttonSend_Click;
            // 
            // textBoxMessage
            // 
            textBoxMessage.Dock = DockStyle.Fill;
            textBoxMessage.Location = new Point(11, 13);
            textBoxMessage.Margin = new Padding(3, 4, 3, 4);
            textBoxMessage.Name = "textBoxMessage";
            textBoxMessage.Size = new Size(1007, 27);
            textBoxMessage.TabIndex = 0;
            textBoxMessage.KeyDown += textBoxMessage_KeyDown;
            // 
            // ChatForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1029, 760);
            Controls.Add(splitContainerMain);
            Controls.Add(panelBottom);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ChatForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Чат";
            splitContainerMain.Panel1.ResumeLayout(false);
            splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainerMain).EndInit();
            splitContainerMain.ResumeLayout(false);
            panelBottom.ResumeLayout(false);
            panelBottom.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainerMain;
        private RichTextBox richTextBoxChat;
        private ListBox listBoxUsers;
        private Panel panelBottom;
        private Button buttonSend;
        private TextBox textBoxMessage;
    }
}