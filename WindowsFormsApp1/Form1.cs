using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp = null;
            foreach (var item in PrintNameColumn())
            {
                dataGridView1.Columns.Add(item.ToString(), item.ToString());
            }
            for (int i = 0; i < 3; i++)
            {
                dataGridView1.Rows.Add();
            }
            foreach (var item in PrintValueColumn())
            {
                dataGridView1.Rows[1].Cells[2].Value = item.ToString();
            }
            

        }
        string[] PrintValueColumn()
        {
            using (SqlConnection connection = new SqlConnection($@"Data Source={textBox1.Text}; Initial Catalog={textBox3.Text}; Integrated Security=SSPI; Encrypt = False;"))
            {
                try
                {
                    // Открываем подключение
                    connection.Open();
                    string sqlExpression = $"SELECT * FROM Cars";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        string[] arr = new string[reader.FieldCount];
                        for (int i = 0; i < arr.Length; i++) arr[i] = reader.GetValue(i).ToString();
                        return arr;
                    }
                    MessageBox.Show("Пустое значение");
                    return null;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                finally
                {
                    // закрываем подключение
                    connection.Close();
                    MessageBox.Show("Подключение закрыто...");
                }
            }
        }

        string[] PrintNameColumn()
        {
            using (SqlConnection connection = new SqlConnection($@"Data Source={textBox1.Text}; Initial Catalog={textBox3.Text}; Integrated Security=SSPI; Encrypt = False;"))
            {
                try
                {
                    // Открываем подключение
                    connection.Open();
                    MessageBox.Show("Подключение открыто");
                    string sqlExpression = $"SELECT * FROM Cars";
                    SqlCommand command = new SqlCommand(sqlExpression, connection);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        //MessageBox.Show(reader.FieldCount.ToString());
                        string[] arr = new string[reader.FieldCount];

                        for (int i = 0; i < arr.Length; i++) arr[i] = reader.GetName(i);
                        return arr;
                    }
                    MessageBox.Show("Пустое значение");
                    return null;
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return null;
                }
                finally
                {
                    // закрываем подключение
                    connection.Close();
                    MessageBox.Show("Подключение закрыто...");
                }
                /*connection.Open();
                string sqlExpression = $"SELECT * FROM Cars";
                SqlCommand command = new SqlCommand(sqlExpression, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows) // если есть данные
                {
                    MessageBox.Show(reader.FieldCount.ToString());
                    string[] arr = new string[reader.FieldCount];

                    for (int i = 0; i < arr.Length; i++) arr[i] = reader.GetName(i);
                    return arr;
                }
                MessageBox.Show("Пустое значение");
                return null;*/
            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            creadDB();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            deleteDB();
        }

        public async void creadDB()
        {
            using (SqlConnection connection = new SqlConnection($@"Data Source={textBox1.Text}; Database=master; Integrated Security=SSPI; Encrypt = False;"))
            {
                await connection.OpenAsync();   // открываем подключение

                SqlCommand command = new SqlCommand();
                // определяем выполняемую команду
                command.CommandText = $"CREATE DATABASE {textBox3.Text}";
                // определяем используемое подключение
                command.Connection = connection;
                // выполняем команду
                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception)
                {
                    MessageBox.Show("DB уже существует!");
                    return;
                }
                MessageBox.Show("База данных создана");

                command.CommandText = $"USE {textBox3.Text}";
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();

                command.CommandText = $"CREATE TABLE Cars (Id INT PRIMARY KEY IDENTITY," +
                                    "Модель NVARCHAR(100) NOT NULL," +
                                    "Марка NVARCHAR(100) NOT NULL," +
                                    "Тип_кузова NVARCHAR(100) NOT NULL," +
                                    "Объёем_двигателя INT NOT NULL," +
                                    "Цена_мин INT NOT NULL," +
                                    "Цена_макс INT NOT NULL," +
                                    "Расположение_руля NVARCHAR(100) NOT NULL," +
                                    "Тип_коробки_переключения_передач NVARCHAR(100) NOT NULL)";
                command.Connection = connection;
                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception)
                {
                    MessageBox.Show("Таблица уже существует!");
                    return;
                }
                MessageBox.Show("Таблица создана");

                command.CommandText = $"INSERT INTO Cars (Модель, Марка, Тип_кузова, Объёем_двигателя, Цена_мин, Цена_макс, Расположение_руля, Тип_коробки_переключения_передач) " +
                                                $"VALUES ('Matiz', 'Deawoo', 'Карлик', 85, 75000, 80000, 'Левое', 'Ручная')";
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();
                command.CommandText = $"INSERT INTO Cars (Модель, Марка, Тип_кузова, Объёем_двигателя, Цена_мин, Цена_макс, Расположение_руля, Тип_коробки_переключения_передач) " +
                                $"VALUES ('X5', 'BMW', 'ХэчБэк', 285, 375000, 580000, 'Левое', 'Автомат')";
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();
                command.CommandText = $"INSERT INTO Cars (Модель, Марка, Тип_кузова, Объёем_двигателя, Цена_мин, Цена_макс, Расположение_руля, Тип_коробки_переключения_передач) " +
                                $"VALUES ('Q7', 'Audi', 'Пикап', 185, 475000, 880000, 'Левое', 'Робот')";
                command.Connection = connection;
                await command.ExecuteNonQueryAsync();
            }
        }

        public async void deleteDB()
        {
            using (SqlConnection connection = new SqlConnection($@"Data Source={textBox1.Text}; Database=master; Integrated Security=SSPI; Encrypt = False;"))
            {
                await connection.OpenAsync();   // открываем подключение

                SqlCommand command = new SqlCommand();
                // определяем выполняемую команду
                command.CommandText = $"DROP DATABASE {textBox3.Text}";
                // определяем используемое подключение
                command.Connection = connection;
                // выполняем команду

                try
                {
                    await command.ExecuteNonQueryAsync();
                }
                catch (Exception)
                {
                    MessageBox.Show("DB не существует!");
                    return;
                }
                MessageBox.Show("База данных удалена");
            }
        }
    }
}
