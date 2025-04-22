using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    public partial class Form1 : Form
    {
        string dbFileName = "employee.db";
        string connectionString = @"URI=file:" + Application.StartupPath + "\\employee.db";
        SQLiteConnection connection;

        public Form1()
        {
            InitializeComponent();
            connection = new SQLiteConnection(connectionString);
        }

        private void exit_Click(object sender, EventArgs e)
        {
            Application.Exit(); // Uygulamadan çıkış 
        }

        private void login_signupBtn_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Show();
            this.Hide();
        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            login_password.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (login_username.Text == "" || login_password.Text == "")
            {
                MessageBox.Show("Please fill all blank fields", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (connection.State == ConnectionState.Closed)
                {
                    try
                    {
                        connection.Open();
                        string selectData = "SELECT * FROM users WHERE username = @username AND password = @password";

                        using (var command = new SQLiteCommand(selectData, connection))
                        {
                            command.Parameters.AddWithValue("@username", login_username.Text.Trim());
                            command.Parameters.AddWithValue("@password", login_password.Text.Trim());

                            SQLiteDataAdapter adapter = new SQLiteDataAdapter(command);
                            DataTable table = new DataTable();
                            adapter.Fill(table);

                            if (table.Rows.Count >= 1)
                            {
                                MessageBox.Show("Login successfully!", "Information Message", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                MainForm mainForm = new MainForm();
                                mainForm.Show();
                                this.Hide();
                            }
                            else
                            {
                                MessageBox.Show("Incorrect Username/Password", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
