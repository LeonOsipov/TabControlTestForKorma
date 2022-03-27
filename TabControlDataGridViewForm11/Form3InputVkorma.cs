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
    public partial class Form3InputVkorma : Form
    {
        int dataYear = DateTime.Now.Year;
        private NpgsqlDataAdapter adapter = null;
        private NpgsqlDataAdapter adapterCmb1 = null;
        DataTable dt = null;
        DataTable dtCmb1 = null;
        int id_korm;

        public Form3InputVkorma()
        {
            InitializeComponent();
            //textBox1.Text = Convert.ToString(dataYear);
            textBox1.Text = Convert.ToString(DateTime.Now.Year);

            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();

            ////adapter = new NpgsqlDataAdapter("SELECT korm AS 'Наименование корма' FROM korma WHERE nameraiona =" + "N" + "'" + textBox1.Text + "'", npgSqlConnection);
            //adapter = new NpgsqlDataAdapter("SELECT korma.korm AS \"Наименование корма\" FROM korma", npgSqlConnection);
            //dt = new DataTable();
            //adapter.Fill(dt);
            //dataGridView1.DataSource = dt;

            adapterCmb1 = new NpgsqlDataAdapter("SELECT korm FROM korma", npgSqlConnection);
            dtCmb1 = new DataTable();
            // Заполняем DataTable
            adapterCmb1.Fill(dtCmb1);
            // Отображаем данные
            comboBox1.DataSource = dtCmb1;
            comboBox1.DisplayMember = "korm";

            npgSqlConnection.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            //NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            //npgSqlConnection.Open();

            //adapterCmb1 = new NpgsqlDataAdapter("SELECT korm FROM korma", npgSqlConnection);
            //dtCmb1 = new DataTable();
            //adapterCmb1.Fill(dtCmb1);
            //comboBox1.DataSource = dtCmb1;
            //comboBox1.DisplayMember = "korm";
            //npgSqlConnection.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            NpgsqlCommand commandSelectFromKorma = new NpgsqlCommand("SELECT id_korm FROM korma WHERE korm =" + "N" + "'" + comboBox1.Text + "'" , npgSqlConnection);
            NpgsqlDataReader reader = commandSelectFromKorma.ExecuteReader();
            while (reader.Read())
            {
                id_korm = reader.GetInt32(0);
                //object kod = reader["kodhozyaistva"];
            }
            reader.Close();
            // код сохранения объёма корма, наименования корма и года ввода объёма корма:
            NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO vkormayear (vkormayear, yearinput, typekormid) VALUES (@vkormayear, @yearinput, @typekormid)", npgSqlConnection);
            command.Parameters.AddWithValue("vkormayear", int.Parse(textBox2.Text));
            command.Parameters.AddWithValue("yearinput", dataYear);
            command.Parameters.AddWithValue("typekormid", id_korm);
            command.ExecuteNonQuery();
            MessageBox.Show("Данные сохрананы!", "Уведомление о результатах", MessageBoxButtons.OK);
            npgSqlConnection.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int yearInput = int.Parse(textBox1.Text);
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            adapter = new NpgsqlDataAdapter("SELECT korm AS \"НАИМЕНОВАНИЕ КОРМА\", vkormayear AS \"ЗАГОТОВЛЕНО (Тонн)\" FROM korma INNER JOIN vkormayear ON korma.id_korm = vkormayear.typekormid WHERE korma.korm =" + "N" + "'" + comboBox1.Text + "'" + "AND vkormayear.yearinput =" + yearInput, npgSqlConnection);
            dt = new DataTable();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            npgSqlConnection.Close();
        }
    }
}
