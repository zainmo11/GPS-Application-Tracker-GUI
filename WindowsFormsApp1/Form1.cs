using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;
using System.Web.Script.Serialization;
using GMap.NET;
using System.Threading;
using System.Timers;

namespace WindowsFormsApp1
{
    
    public partial class Form1 : Form
    {
        private GMap.NET.WindowsForms.GMapControl gmap;
        // Create a TCP/IP socket.
        public static Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        static string[] substrings;
        bool isDragEnabled = false;
        private GMap.NET.WindowsForms.GMapOverlay markerOverlay;
        private GMap.NET.WindowsForms.Markers.GMarkerGoogle marker;
        private System.Timers.Timer timer;
        int zoom=20;
        public Form1()
        {
            InitializeComponent();
            gmap = new GMap.NET.WindowsForms.GMapControl();
            gmap.MapProvider = GMap.NET.MapProviders.GMapProviders.GoogleMap;
            gmap.Dock = DockStyle.Fill;
            gmap.MapProvider = GMap.NET.MapProviders.BingMapProvider.Instance;
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            gmap.ShowCenter = false;
            gmap.MinZoom = 1;
            gmap.MaxZoom = 20;
            Guna.UI2.WinForms.Guna2Panel guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            guna2Panel1.Location = new Point(400, 157);
            guna2Panel1.Size = new Size(291, 251);
            guna2Panel1.Controls.Add(gmap);
            this.Controls.Add(guna2Panel1);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.Text = "GPS App";
            gmap.DragButton = MouseButtons.Left;

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

          
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {

        }

        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            // Replace these with the IP address and port number of your server
            string serverIP = guna2TextBox7.Text;
            int serverPort = int.Parse(guna2TextBox8.Text);

            clientSocket.Connect(serverIP, serverPort);
            // Replace this with the message you want to send
            string message = "appl";

            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            clientSocket.Send(messageBytes);
            bool isConnected = ServerConnectivityChecker.IsServerConnected(serverIP, serverPort);

            

            timer = new System.Timers.Timer(1000);
            timer.Elapsed += TimerElapsed;
            timer.Start();


        }
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {

            string message = "getd";
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            clientSocket.Send(messageBytes);

            byte[] buffer = new byte[1024];
            int bytesReceived = clientSocket.Receive(buffer);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
            substrings = response.Split(',');
            Invoke((MethodInvoker)delegate {
                guna2TextBox6.Text = substrings[0];
                guna2TextBox5.Text = substrings[1];
                guna2TextBox4.Text = substrings[2];
                guna2TextBox3.Text = substrings[5];
                if (substrings[6][0] == '1')
                {
                    guna2TextBox9.Text = "Yes";
                }
                else
                {
                    guna2TextBox9.Text = "NO";
                }
                gmap.Position = new PointLatLng(Convert.ToDouble(substrings[3]), Convert.ToDouble(substrings[4]));
                gmap.Zoom = zoom;
                gmap.Update();
                gmap.Refresh();
                if (markerOverlay == null)
                {
                    markerOverlay = new GMap.NET.WindowsForms.GMapOverlay("marker1");
                    gmap.Overlays.Add(markerOverlay);
                }
                if (marker != null)
                {
                    // Remove existing marker if it already exists
                    markerOverlay.Markers.Remove(marker);
                }
                marker = new GMap.NET.WindowsForms.Markers.GMarkerGoogle(new PointLatLng(Convert.ToDouble(substrings[3]), Convert.ToDouble(substrings[4])), GMap.NET.WindowsForms.Markers.GMarkerGoogleType.red_small);
                markerOverlay.Markers.Add(marker);

            });
        }
        private void timerUpdateValues_Tick(object sender, EventArgs e)
        {
            // Send a request to the server to get the values of time, speed, and distance
            // and update the TextBoxes
            string response = SendRequestToServer("getd");
            string[] values = response.Split(',');
            guna2TextBox6.Text = values[0];
            guna2TextBox5.Text = values[1];
            guna2TextBox4.Text = values[2];
            guna2TextBox3.Text = values[3];
        }

        private string SendRequestToServer(string v)
        {
            throw new NotImplementedException();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton3_Click(object sender, EventArgs e)
        {
            zoom -= 1;
            gmap.Update();
            gmap.Refresh();
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {
            // Get the text from textbox1 and textbox2
            string text1 = guna2TextBox1.Text;
            string text2 = guna2TextBox2.Text;

            byte[] messageBytes = Encoding.ASCII.GetBytes("gpsd");
            clientSocket.Send(messageBytes);
            // Convert the text to bytes
            byte[] data = Encoding.ASCII.GetBytes(text1 + "," + text2);
            byte[] buffer = new byte[1024];
            int bytesReceived = clientSocket.Receive(buffer);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
            // Convert the message to a byte array and send it to the server.
            if (response == "1")
            {
                clientSocket.Send(data);
            }
        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }


        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox6_TextChanged(object sender, EventArgs e)
        {
             
        }

        private void guna2TextBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton2_Click(object sender, EventArgs e)
        {
            try
            {


                /*// Send a message to the server
                byte[] messageBytes = Encoding.ASCII.GetBytes("getd");
                clientSocket.Send(messageBytes);

                // Receive a response from the server
                byte[] buffer = new byte[1024];
                int bytesReceived = clientSocket.Receive(buffer);
                string response = Encoding.ASCII.GetString(buffer, 0, bytesReceived);

                // Deserialize the response
                string[] substrings = response.Split(',');

                string latitude = substrings[0];
                string longitude = substrings[1];*/
                guna2TextBox1.Text = substrings[3];
                guna2TextBox2.Text = substrings[4];
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
   

        
    }

        private void guna2GradientButton5_Click_1(object sender, EventArgs e)
        {
            zoom += 1;
            gmap.Update();
            gmap.Refresh();
        }

        private void guna2TextBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox9_TextChanged(object sender, EventArgs e)
        {
            string serverIP = guna2TextBox7.Text;
            int serverPort = int.Parse(guna2TextBox8.Text);
        }
        public class ServerConnectivityChecker
        {
            public static bool IsServerConnected(string serverIpAddress, int serverPort)
            {
                try
                {
                    using (Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        clientSocket.Connect(serverIpAddress, serverPort);
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }
        }

        private void guna2GradientButton6_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
           
            // Toggle the drag state
            isDragEnabled = !isDragEnabled;

            // Set the appropriate value for the DragButton property
            if (isDragEnabled)
            {
                gmap.DragButton = MouseButtons.Left;
            }
            else
            {
                gmap.DragButton = MouseButtons.None;
            }
        }
    }
    }
