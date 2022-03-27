using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace TabControlDataGridViewForm11
{
    public partial class Form1 : Form
    {
        private NpgsqlDataAdapter adapter = null;
        private NpgsqlDataAdapter adapter1 = null;
        DataSet ds = new DataSet();//null;
        DataSet ds1 = new DataSet();
        //DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)// Отображение данных из БД в dataGridView
        {
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid=agro_user;Pwd=123456;";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            //string command = "Select * From spr_korm";
            //adapter = new NpgsqlDataAdapter(/*command*/"SELECT * FROM t_sprkorm", npgSqlConnection);
            adapter = new NpgsqlDataAdapter("SELECT name_korm AS \"Корм\", sv_n AS \"Сухое_вещество_н\", syroi_protein AS \"Сырой_протеин\", syroi_jir AS \"Сырой_жир\", krahmal_n AS \"Крахмал_н\", kormId AS \"ID_корма(автозаполнение)\" FROM t_sprkorm", npgSqlConnection);
            adapter1 = new NpgsqlDataAdapter("SELECT sugar_n AS \"Сахар_н\", syraya_zola AS \"Сырая_зола\", kormId AS \"ID_корма(автозаполнение)\" FROM t_sprkorm", npgSqlConnection);
            // Создаем объект Dataset
            ds = new DataSet();
            ds1 = new DataSet();
            // Заполняем Dataset
            adapter.Fill(ds);
            adapter1.Fill(ds1);
            // Отображаем данные
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView2.DataSource = ds1.Tables[0];
            //dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystroke;
            dataGridView1.Columns["ID_корма(автозаполнение)"].ReadOnly = true;// Закрытие колонки  kormId для редактирования при выводе в dataGridView
            dataGridView2.Columns["ID_корма(автозаполнение)"].Visible = false;// Неотображение столбца "ID_корма", т.к. этот столбец нужен только для команды UPDATE(без ключевого столбца команда не сработает - вылет программы с ошибкой) 
            //dataGridView2.Columns["ID_корма(автозаполнение)"].ReadOnly = true;
            //adapter.Fill(dt);
            //dataGridView1.DataSource = dt;
            npgSqlConnection.Close();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e) // Событие сохранения или внесения данных в dataGridView
        {
            String connectionString = "Server=192.168.2.4; Port=5432; Database=db_korma; Uid=agro_user; Pwd = 123456; ";
            NpgsqlConnection npgSqlConnection = new NpgsqlConnection(connectionString);
            npgSqlConnection.Open();
            //adapter.Update((DataTable)dataGridView1.DataSource);
            //adapter.Update(ds.Tables[0]);//adapter.Update(ds);
            //try
            //{
            //adapter = new NpgsqlDataAdapter(command, connection);
            /////////////////////////////////////////////////////////////////////////
            //adapter = new NpgsqlDataAdapter("UPDATE spr_korm;", connectionString);
            NpgsqlCommandBuilder cmdBuilder = new NpgsqlCommandBuilder(adapter);
            NpgsqlCommandBuilder cmdBuilder1 = new NpgsqlCommandBuilder(adapter1);

            adapter.UpdateCommand = cmdBuilder.GetUpdateCommand();
            adapter1.UpdateCommand = cmdBuilder1.GetUpdateCommand();

            adapter.Update((DataTable)dataGridView1.DataSource);
            adapter1.Update((DataTable)dataGridView2.DataSource);

            MessageBox.Show("Изменения в базе данных выполнены!", "Уведомление о результатах",MessageBoxButtons.OK);
            npgSqlConnection.Close();
            /////////////////////////////////////////////////////////////////////////
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Изменения в базе данных выполнить не удалось!",
            //      "Уведомление о результатах", MessageBoxButtons.OK);
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2DataInComboBoxFromDB Form2 = new Form2DataInComboBoxFromDB();
            Form2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form3InputVkorma Form3 = new Form3InputVkorma();
            Form3.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form4InputVSeno Form4 = new Form4InputVSeno();
            Form4.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form5InputVSilos Form5 = new Form5InputVSilos();
            Form5.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form6InputkormHoz Form6 = new Form6InputkormHoz();
            Form6.Show();
        }
    }
}
