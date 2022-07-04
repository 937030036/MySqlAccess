using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace MySqlAccess
{
    public partial class Table : Form
    {

        private Welcome Parentobj { get; set; }
        private string Dbname { get; set; }
        private readonly List<string> tables_name;
        public MySqlConnection MySqlConnection { get; private set; }
        public Table(MySqlConnection mySqlConnection, string dbname, Welcome welcome)
        {
            InitializeComponent();
            MySqlConnection = mySqlConnection;
            tables_name = new List<string>();
            Dbname = dbname;
            Parentobj = welcome;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string name = this.listBox1.SelectedItem.ToString();
            string sql = $"select * from {name}";
            MySqlConnection.Open();
            MySqlCommand mySqlCommand = new MySqlCommand(sql, MySqlConnection);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            var columns = Enumerable.Range(0, mySqlDataReader.FieldCount).Select(mySqlDataReader.GetName).ToList();

            DataTable dataTable = new DataTable(name);

            foreach (var col in columns)
            {
                dataTable.Columns.Add(col, Type.GetType("System.String"));
            }

            while (mySqlDataReader.Read())
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (var col in columns)
                {
                    try
                    {
                        dataRow[col] = mySqlDataReader.GetString(col);
                    }
                    catch (System.Data.SqlTypes.SqlNullValueException)
                    {
                        dataRow[col] = "Null";
                    }
                    catch (MySql.Data.Types.MySqlConversionException ex)
                    {
                        MessageBox.Show(ex.StackTrace);
                        Close();
                    }
                }
                dataTable.Rows.Add(dataRow);
            }

            MySqlConnection.Close();
            this.dataGridView1.DataSource = dataTable;
        }

        private void OnLoad(object sender, EventArgs e)
        {
            string sql = "show tables";
            MySqlCommand mySqlCommand = new MySqlCommand(sql, MySqlConnection);
            MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader();
            string col = "Tables_in_" + Dbname;
            while (mySqlDataReader.Read())
            {
                tables_name.Add(mySqlDataReader.GetString(col));
            }
            MySqlConnection.Close();

            foreach (string name in tables_name)
            {
                this.listBox1.Items.Add(name);
            }
            this.WindowState = FormWindowState.Normal;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();

            foreach (Control control in Parentobj.Controls)
            {
                control.Visible = true;
            }
        }


    }
}
