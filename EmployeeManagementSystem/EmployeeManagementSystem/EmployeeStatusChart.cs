using System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace EmployeeManagementSystem
{
    public partial class EmployeeStatusChart : UserControl
    {
        private readonly string connectionString;
        private Chart statusChart;

        public EmployeeStatusChart()
        {
            InitializeComponent();
            connectionString = $"URI=file:{Application.StartupPath}\\employee.db";
            InitializeChart();
            LoadData();
        }

        public void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // EmployeeStatusChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "EmployeeStatusChart";
            this.Size = new System.Drawing.Size(500, 400);
            this.ResumeLayout(false);
        }

        private void InitializeChart()
        {
            statusChart = new Chart();
            statusChart.Dock = DockStyle.Fill;

            // Chart alanı oluşturma
            ChartArea chartArea = new ChartArea();
            statusChart.ChartAreas.Add(chartArea);

            // Pasta grafiği serisi oluşturma
            Series series = new Series();
            series.ChartType = SeriesChartType.Pie;
            series.Name = "EmployeeStatus";

            // Yüzdeleri göstermek için format
            series.Label = "#PERCENT{P0}";
            series.LegendText = "#VALX";

            statusChart.Series.Add(series);

            // Legend ekleme
            Legend legend = new Legend();
            statusChart.Legends.Add(legend);

            this.Controls.Add(statusChart);
        }

        public void LoadData()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"
                        SELECT status, COUNT(*) as count 
                        FROM employees 
                        WHERE delete_date IS NULL 
                        GROUP BY status";

                    using (var command = new SQLiteCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        // Mevcut verileri temizle
                        statusChart.Series["EmployeeStatus"].Points.Clear();

                        // Verileri ekle
                        while (reader.Read())
                        {
                            string status = reader["status"].ToString();
                            int count = Convert.ToInt32(reader["count"]);

                            var point = statusChart.Series["EmployeeStatus"].Points.Add(count);
                            point.AxisLabel = status;
                            point.LegendText = status;

                            // Duruma göre renk ataması
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
                    MessageBox.Show("Veri yüklenirken hata oluştu: " + ex.Message,
                        "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Verileri yenilemek için public method
        public void RefreshData()
        {
            LoadData();
        }
    }
}