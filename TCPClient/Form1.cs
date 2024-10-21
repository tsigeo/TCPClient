using SimpleTcp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace TCPClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;
        private void label1_Click(object sender, EventArgs e)
        {

        }

        // Το πλήκτρο Send για την αποστολή μηνύματος.
        private void btnSend_Click(object sender, EventArgs e)
        {
            if(client.IsConnected)
            {
                if(!string.IsNullOrEmpty(txtMessage.Text))
                {
                    client.Send(txtMessage.Text);
                    txtInfo.Text += $"Me: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }

        // Το πλήκτρο Connect για την συνδεση του client στο server.
        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                client.Connect();
                btnSend.Enabled = true;
                btnConnect.Enabled = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Αρχικό παράθυρο εφαρμογής
        private void Form1_Load(object sender, EventArgs e)
        {
            client = new(txtIP.Text);
            client.Events.Connected += Events_Connected;
            client.Events.DataReceived += Events_DataReceived;
            client.Events.Disconnected += Events_Disconnected;
            btnSend.Enabled = false;
        }

        private void Events_Connected(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server connected.{Environment.NewLine}";
            });

        }

        private void Events_DataReceived(object sender, DataReceivedFromServerEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"Server: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });
        }
    
        private void Events_Disconnected(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate 
            {
                txtInfo.Text += $"Server disconnected.{Environment.NewLine}";
            });
            
        }

        // Αποθήκευση συνεδρίας LOG file.
        private void btnLog_Click(object sender, EventArgs e)
        {
                TextWriter txt = new StreamWriter("C:\\server-client\\log.txt");
                txt.Write(txtInfo.Text);
                txt.Close();
        }

        // Εμφάνιση αναδυόμενου παράθυρου με κάποιες πληροφορίες.
        private void btnInfo_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Giorgos Tsichlakis E14195" + Environment.NewLine + "Δίκτυα Υπολογιστών 2" + Environment.NewLine + "Δεκέμβριος 2020");
        }
    }
}
