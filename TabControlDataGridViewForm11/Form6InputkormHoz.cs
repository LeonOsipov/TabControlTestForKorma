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

/*  ВНИМАНИЕ ВЗАИМОДЕЙСТВИЕ С ТАБЛИЦЕЙ sprhozyaistv ВОЗМОЖНО ЗАМЕНА НА hozyaistva ИЛИ СМЕНА FOREIGN KEY */

namespace TabControlDataGridViewForm11
{
    public partial class Form6InputkormHoz : Form
    {
        int id_raion;
        int id_hozyaistva;
        private NpgsqlDataAdapter adapterCmb1 = null;
        //private NpgsqlDataAdapter adapterKorm = null;
        DataTable dtCmb1 = null;
        //DataTable dtKorm = null;
        private NpgsqlDataAdapter adapter = null;

        public Form6InputkormHoz()
        {
            InitializeComponent();

            textBox1.Text = Convert.ToString(DateTime.Now.Year);

            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            // Заполнение comboBox1
            adapterCmb1 = new NpgsqlDataAdapter("SELECT nameraion FROM raion", npgSqlConnection);
            dtCmb1 = new DataTable();
            // Заполняем DataTable
            adapterCmb1.Fill(dtCmb1);
            // Отображаем данные
            comboBox1.DataSource = dtCmb1;
            comboBox1.DisplayMember = "nameraion";

            npgSqlConnection.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid = agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();

            adapter = new NpgsqlDataAdapter("SELECT namehozyaistva, kodhozyaistva FROM sprhozyaistv WHERE nameraiona =" + "N" + "'" + comboBox1.Text + "'", npgSqlConnection);

            //Создаём объект DataTable
            dtCmb1 = new DataTable();
            // Заполняем DataTable
            adapter.Fill(dtCmb1);
            // Отображаем данные
            comboBox2.DataSource = dtCmb1;
            comboBox2.DisplayMember = "namehozyaistva";
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
            }
            reader.Close();

            //NpgsqlCommand commandSelectFromKorma2 = new NpgsqlCommand("SELECT id_hozyaistva FROM hozyaistva WHERE id_raion =" + "N" + "'" + comboBox1.Text + "'" + "AND hozyaistva.id_hozyaistva =" + id_raion, npgSqlConnection);
            NpgsqlCommand commandSelectFromKorma2 = new NpgsqlCommand("SELECT kodhozyaistva FROM sprhozyaistv WHERE sprhozyaistv.namehozyaistva =" + "N" + "'" + comboBox2.Text + "'" , npgSqlConnection);
            NpgsqlDataReader reader2 = commandSelectFromKorma2.ExecuteReader();
            while (reader2.Read())
            {
                id_hozyaistva = reader2.GetInt32(0);
            }
            reader.Close();
            NpgsqlCommand command = new NpgsqlCommand($"INSERT INTO vkormahoz (yearinput, raionid, hozyaistvoid, vsenohoz, vsiloshoz, vsenaghoz) VALUES (@yearinput, @raionid, @hozyaistvoid, @vsenohoz, @vsiloshoz, @vsenag)", npgSqlConnection);
            command.Parameters.AddWithValue("yearinput", int.Parse(textBox1.Text));
            command.Parameters.AddWithValue("raionid", id_raion);
            command.Parameters.AddWithValue("hozyaistvoid", id_hozyaistva);
            command.Parameters.AddWithValue("vsenohoz", int.Parse(textBox2.Text));
            command.Parameters.AddWithValue("vsiloshoz", int.Parse(textBox3.Text));
            command.Parameters.AddWithValue("vsenag", int.Parse(textBox4.Text));
            command.ExecuteNonQuery();
            MessageBox.Show("Данные сохранены!", "Уведомление о результатах", MessageBoxButtons.OK);
            npgSqlConnection.Close();
        }
    }
}
