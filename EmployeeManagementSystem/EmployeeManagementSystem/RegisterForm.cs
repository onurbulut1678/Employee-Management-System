using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
namespace EmployeeManagementSystem
{
    public partial class RegisterForm : Form
    {
        string dbFileName = "employee.db";
        string connectionString = @"URI=file:" + Application.StartupPath + "\\employee.db";
        SQLiteConnection connection;

        public RegisterForm()
        {
            InitializeComponent();
            connection = new SQLiteConnection(connectionString);
        }

        private void createDataBase()
        {
            if (!File.Exists(dbFileName))
            {
                SQLiteConnection.CreateFile(dbFileName);
                using (var connection = new SQLiteConnection(@"Data Source=" + dbFileName))
                {
                    connection.Open();
                    string sql1 = "CREATE TABLE users(id INTEGER PRIMARY KEY AUTOINCREMENT,username TEXT NULL,password TEXT NULL,date_register DATE NULL)";
                    SQLiteCommand cmd = new SQLiteCommand(sql1, connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            else
            {
                Debug.WriteLine("Database is already created");
            }
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {
            createDataBase();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void signup_loginBtn_Click(object sender, EventArgs e)
        {
            Form1 loginForm = new Form1();
            loginForm.Show();
            this.Hide();
        }

        private void signup_showPass_CheckedChanged(object sender, EventArgs e)
        {
            signup_password.PasswordChar = signup_showPass.Checked ? '\0' : '*';
        }

        private void signup_btn_Click(object sender, EventArgs e)
        {
            if (signup_username.Text == "" || signup_password.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connection.State != ConnectionState.Open)
                {
                    try
                    {
                        connection.Open();

                        string selectUsername = "SELECT COUNT(id) FROM users WHERE username = @user";
                        using (var checkUser = new SQLiteCommand(selectUsername, connection))
                        {
                            checkUser.Parameters.AddWithValue("@user", signup_username.Text.Trim());
                            int count = Convert.ToInt32(checkUser.ExecuteScalar());

                            if (count >= 1)
                            {
                                MessageBox.Show(signup_username.Text.Trim() + " is already taken", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else
                            {
                                DateTime today = DateTime.Today;

                                string query = "INSERT INTO users (username, password, date_register) VALUES (@username, @password, @dateReg)";
                                using (var command = new SQLiteCommand(query, connection))
                                {
                                    command.Parameters.AddWithValue("@username", signup_username.Text.Trim());
                                    command.Parameters.AddWithValue("@password", signup_password.Text.Trim());
                                    command.Parameters.AddWithValue("@dateReg", today);

                                    command.ExecuteNonQuery();

                                    MessageBox.Show("User registered successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    Form1 loginForm = new Form1();
                                    loginForm.Show();
                                    this.Hide();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }
    }
}
