using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EmployeeManagementSystem
{
    public partial class Dashboard : UserControl
    {
        string dbFileName = "employee.db";
        string connectionString = @"URI=file:" + Application.StartupPath + "\\employee.db";
        SQLiteConnection connect;
        private Chart statusChart;

        public Dashboard()
        {
            InitializeComponent();
            connect = new SQLiteConnection(connectionString);

            displayTE();
            displayAE();
            displayIE();

            InitializeChart();
            LoadChartData();
        }

        public void displayTE()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT COUNT(id) FROM employees WHERE delete_date IS NULL";

                    using (SQLiteCommand cmd = new SQLiteCommand(selectData, connect))
                    {
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            dashboard_TE.Text = count.ToString();
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        public void displayAE()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT COUNT(id) FROM employees WHERE status = @status " +
                        "AND delete_date IS NULL";

                    using (SQLiteCommand cmd = new SQLiteCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@status", "Active");
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            dashboard_AE.Text = count.ToString();
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        public void displayIE()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();

                    string selectData = "SELECT COUNT(id) FROM employees WHERE status = @status " +
                        "AND delete_date IS NULL";

                    using (SQLiteCommand cmd = new SQLiteCommand(selectData, connect))
                    {
                        cmd.Parameters.AddWithValue("@status", "Inactive");
                        SQLiteDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int count = Convert.ToInt32(reader[0]);
                            dashboard_IE.Text = count.ToString();
                        }
                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex, "Error Message"
                        , MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        private void InitializeChart()
        {
            statusChart = new Chart();

            // Chart boyutunu ve konumunu ayarla
            statusChart.Size = new System.Drawing.Size(400, 300);
            statusChart.Location = new System.Drawing.Point(20, 20);

            // Chart alanı oluşturma
            ChartArea chartArea = new ChartArea();
            chartArea.Name = "EmployeeStatus";
            statusChart.ChartAreas.Add(chartArea);

            // Pasta grafiği serisi oluşturma
            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            series.Name = "EmployeeStatus";
            series.ChartArea = "EmployeeStatus";

            // Yüzdeleri göstermek için format
            series.Label = "#PERCENT{P0}";
            series.LegendText = "#VALX";

            statusChart.Series.Add(series);

            // Legend ekleme
            Legend legend = new Legend();
            statusChart.Legends.Add(legend);

            // Chart'ı panel2'ye ekle
            panel2.Controls.Add(statusChart);
        }

        private void LoadChartData()
        {
            if (connect.State != ConnectionState.Open)
            {
                try
                {
                    connect.Open();
                    string query = @"
                        SELECT status, COUNT(*) as count 
                        FROM employees 
                        WHERE delete_date IS NULL 
                        GROUP BY status";

                    using (var command = new SQLiteCommand(query, connect))
                    using (var reader = command.ExecuteReader())
                    {
                        statusChart.Series["EmployeeStatus"].Points.Clear();

                        while (reader.Read())
                        {
                            string status = reader["status"].ToString();
                            int count = Convert.ToInt32(reader["count"]);

                            var point = statusChart.Series["EmployeeStatus"].Points.Add(count);
                            point.AxisLabel = status;
                            point.LegendText = status;

                            if (status.ToLower() == "active")
                            {
                                point.Color = System.Drawing.Color.FromArgb(0, 150, 136); // Yeşil
                            }
                            else
                            {
                                point.Color = System.Drawing.Color.FromArgb(229, 57, 53); // Kırmızı
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chart verisi yüklenirken hata oluştu: " + ex.Message,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    connect.Close();
                }
            }
        }

        public void RefreshData()
        {
            if (InvokeRequired)
            {
                Invoke((MethodInvoker)RefreshData);
                return;
            }

            displayTE();
            displayAE();
            displayIE();
            LoadChartData();
        }

        private void chart1_Click(object sender, EventArgs e)
        {
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}