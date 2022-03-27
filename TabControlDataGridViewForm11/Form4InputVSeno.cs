using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TabControlDataGridViewForm11
{
    public partial class Form4InputVSeno : Form
    {
        int id_raion;
        int vsilos;

        private NpgsqlDataAdapter adapterCmb1 = null;
        DataTable dtCmb1 = null;
        //int dataYear = DateTime.Now.Year;

        //int raionid;

        public Form4InputVSeno()
        {
            InitializeComponent();

            textBox1.Text = Convert.ToString(DateTime.Now.Year);

            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();

            ////adapter = new NpgsqlDataAdapter("SELECT korm AS 'Наименование корма' FROM korma WHERE nameraiona =" + "N" + "'" + textBox1.Text + "'", npgSqlConnection);
            //adapter = new NpgsqlDataAdapter("SELECT korma.korm AS \"Наименование корма\" FROM korma", npgSqlConnection);
            //dt = new DataTable();
            //adapter.Fill(dt);
            //dataGridView1.DataSource = dt;

            adapterCmb1 = new NpgsqlDataAdapter("SELECT nameraion FROM raion", npgSqlConnection);
            dtCmb1 = new DataTable();
            // Заполняем DataTable
            adapterCmb1.Fill(dtCmb1);
            // Отображаем данные
            comboBox1.DataSource = dtCmb1;
            comboBox1.DisplayMember = "nameraion";
            npgSqlConnection.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            NpgsqlCommand commandSelectFromKorma = new NpgsqlCommand("SELECT id_raion FROM raion WHERE nameraion =" + "N" + "'" + comboBox1.Text + "'", npgSqlConnection);
            NpgsqlDataReader reader = commandSelectFromKorma.ExecuteReader();
            while (reader.Read())
            {
                id_raion = reader.GetInt32(0);
                //object kod = reader["kodhozyaistva"];
            }
            reader.Close();
            ////////////////////// Test
            /*NpgsqlCommand commandSelectFromKorm = new NpgsqlCommand("SELECT raionid, vsilos FROM vkormaraion WHERE nameraion =" + "N" + "'" + comboBox1.Text + "'", npgSqlConnection);
            NpgsqlDataReader reader2 = commandSelectFromKorm.ExecuteReader();
            while (reader2.Read())
            {
                raionid = reader2.GetInt32(0);
                vsilos = reader2.GetInt32(1);
                //object kod = reader["kodhozyaistva"];
            }*/
            ////////////////////// End Test
            // код сохранения объёма Сена, наименования района и года ввода объёма корма:
            NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO vkormaraion (vseno, yearinput, raionid) VALUES (@vseno, @yearinput, @raionid)", npgSqlConnection);
            command.Parameters.AddWithValue("vseno", int.Parse(textBox2.Text));
            command.Parameters.AddWithValue("yearinput", int.Parse(textBox1.Text));
            command.Parameters.AddWithValue("raionid", id_raion);
            command.ExecuteNonQuery();
            MessageBox.Show("Данные сохранены!", "Уведомление о результатах", MessageBoxButtons.OK);
            npgSqlConnection.Close();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            int yearInput = int.Parse(textBox1.Text);
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            adapterCmb1 = new NpgsqlDataAdapter("SELECT nameraion AS \"НАИМЕНОВАНИЕ РАЙОНА\", vseno AS \"ЗАГОТОВЛЕНО (Тонн)\" FROM raion INNER JOIN vkormaraion ON raion.id_raion = vkormaraion.raionid WHERE raion.nameraion =" + "N" + "'" + comboBox1.Text + "'" + "AND vkormaraion.yearinput =" + yearInput, npgSqlConnection);
            dtCmb1 = new DataTable();
            adapterCmb1.Fill(dtCmb1);
            dataGridView1.DataSource = dtCmb1;
            npgSqlConnection.Close();
        }
    }
}
