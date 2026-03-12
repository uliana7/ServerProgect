using System;
using System.Windows.Forms;

namespace ChatClient;

public partial class ConnectForm : Form
{
    public ConnectForm()
    {
        InitializeComponent();

        numericUpDownPort.Minimum = 1;
        numericUpDownPort.Maximum = 65535;
        numericUpDownPort.Value = 13000;

        textBoxIp.Text = "127.0.0.1";
        textBoxLogin.Text = "Ivan";
    }

    private void buttonConnect_Click(object sender, EventArgs e)
    {
        string ip = textBoxIp.Text.Trim();
        int port = (int)numericUpDownPort.Value;
        string login = textBoxLogin.Text.Trim();

        if (string.IsNullOrWhiteSpace(ip) || string.IsNullOrWhiteSpace(login))
        {
            MessageBox.Show("Введите IP и логин.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        ChatForm chatForm = new ChatForm(ip, port, login);
        chatForm.FormClosed += (_, _) => Show();

        Hide();
        chatForm.Show();
    }
}