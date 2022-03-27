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

// Заполнение combBox из Базы Данных без привязки 

namespace TabControlDataGridViewForm11
{
    public partial class Form2DataInComboBoxFromDB : Form
    {
        //NpgsqlDataReader reader;
        //NpgsqlConnection npgSqlConnection = null;
        private NpgsqlDataAdapter adapter = null;
        //DataSet ds = null;
        DataTable dt = null;
        int kodhoz;
       
        public Form2DataInComboBoxFromDB()
        {
            InitializeComponent();
        }

        private void Form2DataInComboBoxFromDB_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "db_kormaDataSet.raion". При необходимости она может быть перемещена или удалена.
            this.raionTableAdapter.Fill(this.db_kormaDataSet.raion);

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();

            adapter = new NpgsqlDataAdapter("SELECT namehozyaistva, kodhozyaistva FROM sprhozyaistv WHERE nameraiona =" + "N" + "'" + comboBox1.Text + "'", npgSqlConnection);

            // Создаем объект Dataset
            //ds = new DataSet();
            // Заполняем Dataset
            //adapter.Fill(ds);
            // Отображаем данные
            //comboBox2.DataSource = ds.Tables[0];
            //comboBox2.DisplayMember = "namehozyaistva";
            //comboBox2.ValueMember = "kodhozyaistva";
            //comboBox2.DataSource = ds.Tables["sprhozyaistv"];

            //Создаём объект DataTable
            dt = new DataTable();
            // Заполняем DataTable
            adapter.Fill(dt);
            // Отображаем данные
            comboBox2.DataSource = dt;
            comboBox2.DisplayMember = "namehozyaistva";
            //comboBox2.ValueMember = "kodhozyaistva";
            //textBox1.Text = comboBox2.ValueMember.ToString();
            //textBox1.Text = comboBox2.SelectedIndex.ToString();

            //NpgsqlCommand sc = new NpgsqlCommand("SELECT namehozyaistva, kodhozyaistva FROM sprhozyaistv WHERE nameraiona =" + "N" + "'" + comboBox1.Text + "'", npgSqlConnection);
            ////NpgsqlDataReader reader;
            //reader = sc.ExecuteReader();
            //DataTable dt = new DataTable();
            //dt.Columns.Add("namehozyaistva", typeof(string));
            //dt.Columns.Add("kodhozyaistva", typeof(string));
            //dt.Load(reader);
            //comboBox1.ValueMember = "kodhozyaistva";
            //comboBox1.DisplayMember = "namehozyaistva";
            //comboBox1.DataSource = dt;

            npgSqlConnection.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int kod;
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            /***************************************/
            NpgsqlCommand commandSelectFromKorma = new NpgsqlCommand("SELECT kodhozyaistva FROM sprhozyaistv WHERE nameraiona =" + "N" + "'" + comboBox1.Text + "'" + " AND namehozyaistva =" + "N" + "'" + comboBox2.Text + "'", npgSqlConnection);
            NpgsqlDataReader reader = commandSelectFromKorma.ExecuteReader();
            while (reader.Read())
            {
                kodhoz = reader.GetInt32(0);
                //object kod = reader["kodhozyaistva"];
            }
            reader.Close();
            /***************************************
            adapter = new NpgsqlDataAdapter("SELECT kodhozyaistva FROM sprhozyaistv WHERE nameraiona =" + "N" + "'" + comboBox1.Text + "'" + " AND namehozyaistva =" + "N" + "'" + comboBox2.Text + "'", npgSqlConnection);
            dt = new DataTable();
            // Заполняем DataTable
            adapter.Fill(dt);
            // Отображаем данные
            textBox1.Text = dt.ToString();
            //comboBox2.DisplayMember = "namehozyaistva";
            /**************************************/
            //textBox1.Text = comboBox2.SelectedIndex.ToString();
            //textBox1.Text = comboBox2.Items.ToString();
            textBox1.Text = kodhoz.ToString();

            npgSqlConnection.Close();
        }
    }
}
