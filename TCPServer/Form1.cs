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


namespace TCPServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
        /* Χρησιμοποιούμε τη βιβλιοθήκη  SimpleTcp όπου της εγκατασήσαμε μέσω 
           του Visual Studio. Υποστηρίζει λειτουργίες σύνδεσης,ανταλλαγής 
           δεδομένων, αποσύνδεσης κτλ.*/

        SimpleTcpServer server; 

        // Το κουμπί btnStart ενεργοποιεί τον server.
        private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            txtInfo.Text += $"Starting...{Environment.NewLine}";
            btnSend.Enabled = true;
        }

        // Το αρχικό παράθυρο της εφαρμογής.
        private void Form1_Load(object sender, EventArgs e)
        {
            btnSend.Enabled = false;
            server = new SimpleTcpServer(txtIP.Text);
            server.Events.ClientConnected += Events_ClientConnected;
            server.Events.ClientDisconnected += Events_ClientDisconnected;
            server.Events.DataReceived += Events_DataReceived;
        }

        // Τα δεδομένα 
        private void Events_DataReceived(object sender, DataReceivedFromClientEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort}: {Encoding.UTF8.GetString(e.Data)}{Environment.NewLine}";
            });

        }

        // Αποσύνδεση Client
        private void Events_ClientDisconnected(object sender, ClientDisconnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                txtInfo.Text += $"{e.IpPort} disconnected.{Environment.NewLine}";
                lstClientIP.Items.Remove(e.IpPort);
            });
        }

        // Σύνδεση Client
        private void Events_ClientConnected(object sender, ClientConnectedEventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
                {
                    txtInfo.Text += $"{e.IpPort} connected.{Environment.NewLine}";
                    lstClientIP.Items.Add(e.IpPort);
                });
        }


        // Αποστολή μηνύματος με χρήση του πλήκτρου Send.
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (server.IsListening) // Ελέγχουμε αν έχει αρχίσει ο server
            {
                // Το μήνυμα δεν στέλνεται αν δεν περιέχει χαρακτήρες.
                if (!string.IsNullOrEmpty(txtMessage.Text) && lstClientIP.SelectedItem != null)
                //lstClientIP.SelectedItem != null: Ελέγχει αν ο χρήστης έχει επιλέξει κάποιον client
                {
                    server.Send(lstClientIP.SelectedItem.ToString(), txtMessage.Text);
                    txtInfo.Text += $"Server: {txtMessage.Text}{Environment.NewLine}";
                    txtMessage.Text = string.Empty;
                }
            }
        }


        // Αποθήκευση συνεδρίας LOG file.
        private void btnLog_Click(object sender, EventArgs e)
        {
            TextWriter txt = new StreamWriter("C:\\server-client\\log.txt");
            txt.Write(txtInfo.Text);
            txt.Close();
        }

        // Εμφάνιση αναδυόμενου παράθυρου με κάποιες πληροφορίες.
        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Giorgos Tsichlakis E14195" + Environment.NewLine + "Δίκτυα Υπολογιστών 2" + Environment.NewLine + "Δεκέμβριος 2020");
        }

        /*
        private void btnManual_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"c:\instructions.pdf");
        }
        */
    }
}
