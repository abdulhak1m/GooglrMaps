using System;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Google_Maps
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            KeyDown += (s, e) => { if (e.KeyValue == (char)Keys.Enter) Btn_Search_Click(btn_Search, null); };
        }

        void Btn_Search_Click(object sender, EventArgs e)
        {
            string line = "";
            string country;
            string region;
            string city;
            string latitude;
            string longitude;
            string timezona;
            try
            {
                using (WebClient wc = new WebClient())
                    line = wc.DownloadString($"http://free.ipwhois.io/xml/{txt_ipInfo.Text}");
                Match match = Regex.Match(line, "<country>(.*?)</country>(.*?)<region>(.*?)</region>(.*?)<city>(.*?)</city>(.*?)<latitude>(.*?)</latitude>(.*?)<longitude>-(.*?)</longitude>(.*?)<timezone>(.*?)</timezone>");
                country = match.Groups[1].Value;
                lbl_country.Text = country;
                region = match.Groups[3].Value;
                lbl_region.Text = region;
                city = match.Groups[5].Value;
                lbl_city.Text = city;
                latitude = match.Groups[7].Value;
                lbl_latitude.Text = latitude;
                longitude = match.Groups[9].Value;
                lbl_longitude.Text = longitude;
                timezona = match.Groups[11].Value;
                lbl_timezona.Text = timezona;
                               
                StringBuilder queryAddress = new StringBuilder();
                queryAddress.Append("https://www.google.ru/maps?q=");
                if (country != string.Empty)
                    queryAddress.Append(country + "," + "+");
                if (region != string.Empty)
                    queryAddress.Append(region + "," + "+");
                if (city != string.Empty)
                    queryAddress.Append(city + "," + "+");
                if (latitude != string.Empty)
                    queryAddress.Append(latitude + "," + "+");
                if (longitude != string.Empty)
                    queryAddress.Append(longitude + "," + "+");
                if (timezona != string.Empty)
                    queryAddress.Append(timezona + "," + "+");

                webBrowser1.Navigate(queryAddress.ToString());
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source, MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        void Txt_ipInfo_TextChanged(object sender, EventArgs e)
        {
            if(Regex.IsMatch(txt_ipInfo.Text, "[^0-9-.]"))
            {
                txt_ipInfo.Text = txt_ipInfo.Text.Remove(txt_ipInfo.Text.Length - 1);
                txt_ipInfo.SelectionStart = txt_ipInfo.TextLength;
            }
        }

        void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            notifyIcon1.Visible = false;
            WindowState = FormWindowState.Normal;
        }

        void Form1_Load(object sender, EventArgs e)
        {
            notifyIcon1.BalloonTipTitle = "Вы открыли приложение Google Maps";
            notifyIcon1.BalloonTipText = "Уведомление: вы свернули Google Maps";
            notifyIcon1.Text = "Google Maps";
        }

        void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(1000);
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        void ОткрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        void ЗакрытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
