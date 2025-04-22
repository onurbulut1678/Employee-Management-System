using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Windows.Forms;

namespace EmployeeManagementSystem
{
    class SalaryData
    {
        public string EmployeeID { get; set; } // 0
        public string Name { get; set; } // 1
        public string Gender { get; set; } // 2
        public string Contact { get; set; } // 3
        public string Position { get; set; } // 4
        public int Salary { get; set; } // 5

        private readonly string connectionString;

        public SalaryData()
        {
            // SQLite bağlantı dizesi
            connectionString = $"Data Source={Application.StartupPath}\\employee.db;Version=3;";
        }

        public List<SalaryData> salaryEmployeeListData()
        {
            List<SalaryData> listdata = new List<SalaryData>();

            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string selectData = "SELECT * FROM employees WHERE status = 'Active' AND delete_date IS NULL";

                    using (var cmd = new SQLiteCommand(selectData, connection))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                SalaryData sd = new SalaryData
                                {
                                    EmployeeID = reader["employee_id"].ToString(),
                                    Name = reader["full_name"].ToString(),
                                    Gender = reader["gender"].ToString(),
                                    Contact = reader["contact_number"].ToString(),
                                    Position = reader["position"].ToString(),
                                    Salary = Convert.ToInt32(reader["salary"])
                                };

                                listdata.Add(sd);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return listdata;
        }
    }
}
