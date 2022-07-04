using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace MySqlAccess
{
    public partial class Welcome : Form
    {
        public Welcome()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string server = this.ipbox.Text;
            string database = this.dbbox.Text;
            string username = this.usernamebox.Text;
            string password = this.pswbox.Text;
            string port = this.portbox.Text;
            try
            {
                MySqlConnection conn = new MySqlConnection($"server={server};database={database};username={username};password={password};port={port}");
                conn.Open();
                if (conn.State == ConnectionState.Open)
                {
                    foreach (Control control in Controls)
                    {
                        if (control.GetType() == typeof(MdiClient))
                        {
                            continue;
                        }
                        control.Visible = false;
                    }
                    Table table = new Table(conn, database, this)
                    {
                        MdiParent = this
                    };
                    table.Show();

                    if(!FileIO.SaveFile(ip: server, port: port, username: username,
                        psw: password, dbname: database, msg: out string msg))
                    {
                        Trace.WriteLine(msg);
                        MessageBox.Show(msg);
                        conn.Close();
                        Close();
                    }


                }
                else if (conn.State == ConnectionState.Closed)
                {

                    conn.Close();
                    MessageBox.Show("Database not connected.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private Size m_szInit;//初始窗体大小
        private Dictionary<Control, Rectangle> m_dicSize
            = new Dictionary<Control, Rectangle>();

        protected override void OnLoad(EventArgs e)
        {
            m_szInit = this.Size;//获取初始大小
            this.GetInitSize(this);
            base.OnLoad(e);
        }

        private void GetInitSize(Control ctrl)
        {
            foreach (Control c in ctrl.Controls)
            {
                m_dicSize.Add(c, new Rectangle(c.Location, c.Size));
                this.GetInitSize(c);
            }
        }

        protected override void OnResize(EventArgs e)
        {
            //计算当前大小和初始大小的比例
            float fx = (float)this.Width / m_szInit.Width;
            float fy = (float)this.Height / m_szInit.Height;
            foreach (var v in m_dicSize)
            {
                v.Key.Left = (int)(v.Value.Left * fx);
                v.Key.Top = (int)(v.Value.Top * fy);
                v.Key.Width = (int)(v.Value.Width * fx);
                v.Key.Height = (int)(v.Value.Height * fy);
            }
            base.OnResize(e);
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            if(FileIO.ReadFile(out string ip, out string port,
            out string username, out string psw, out string dbname, out string msg))
            {
                ipbox.Text = ip;
                portbox.Text = port;
                usernamebox.Text = username;
                pswbox.Text = psw;
                dbbox.Text = dbname;
            }
            else
            {
                Trace.WriteLine(msg);
                MessageBox.Show(msg);
            }
            this.WindowState = FormWindowState.Maximized;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("mailto:937030036zx@gmail.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/937030036");

        }

        private void KeyDownEnter(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(sender, e);
            }
        }
    }
}
