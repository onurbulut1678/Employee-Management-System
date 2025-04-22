using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    class EmployeeData
    {
        // Properties
        public int ID { get; set; }
        public string EmployeeID { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public string Contact { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
        public int Salary { get; set; }
        public string Status { get; set; }

        // Connection string should be readonly since it won't change
        private readonly string connectionString;

        public EmployeeData()
        {
            connectionString = $"URI=file:{Application.StartupPath}\\employee.db";
        }

        public List<EmployeeData> employeeListData()
        {
            var listdata = new List<EmployeeData>();

            using (var connect = new SQLiteConnection(connectionString))
            {
                try
                {
                    connect.Open();
                    string selectData = @"SELECT id, employee_id, full_name, gender, 
                                        contact_number, position, image, salary, status 
                                        FROM employees 
                                        WHERE delete_date IS NULL";

                    using (var cmd = new SQLiteCommand(selectData, connect))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listdata.Add(new EmployeeData
                            {
                                ID = Convert.ToInt32(reader["id"]),
                                EmployeeID = reader["employee_id"].ToString(),
                                Name = reader["full_name"].ToString(),
                                Gender = reader["gender"].ToString(),
                                Contact = reader["contact_number"].ToString(),
                                Position = reader["position"].ToString(),
                                Image = reader["image"].ToString(),
                                Salary = Convert.ToInt32(reader["salary"]),
                                Status = reader["status"].ToString()
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading employee data: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return listdata;
        }

        public List<EmployeeData> salaryEmployeeListData()
        {
            var listdata = new List<EmployeeData>();

            using (var connect = new SQLiteConnection(connectionString))
            {
                try
                {
                    connect.Open();
                    string selectData = @"SELECT employee_id, full_name, position, salary 
                                        FROM employees 
                                        WHERE delete_date IS NULL";

                    using (var cmd = new SQLiteCommand(selectData, connect))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            listdata.Add(new EmployeeData
                            {
                                EmployeeID = reader["employee_id"].ToString(),
                                Name = reader["full_name"].ToString(),
                                Position = reader["position"].ToString(),
                                Salary = Convert.ToInt32(reader["salary"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading salary data: {ex.Message}",
                        "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return listdata;
        }
    }
}