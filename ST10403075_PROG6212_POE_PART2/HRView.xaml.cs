using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ST10403075_PROG6212_POE_PART2
{

    public partial class HRView : Window
    {
        public HRView()
        {
            InitializeComponent();
        }

        private void GenerateReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string connectionString = "Server=labG9AEB3\\SQLEXPRESS;Database=ST10403075_PROG6212_PART2_POE;Trusted_Connection=True;";
                string reportData = "Approved Claims Report\n\n";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    string query = "SELECT c.claimID, c.lecturerID, c.HourlyRate" +
                                   "(c.HoursWorked * c.HourlyRate) AS claimAmount " + "FROM Claims c " +
                                    "INNER JOIN Lecturer u ON c.lecturerID = u.lecturerID " + "WHERE c.ClaimStatus = 'Approved'";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            reportData += $"Claim ID: {reader["claimID"]}\n" +
                                          $"Lecturer ID: {reader["lecturerID"]}\n" ;                                       
                        }
                    }
                }

                // Prompt user to save the report
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                    Title = "Save Report",
                    FileName = "ApprovedClaimsReport.txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, reportData);
                    MessageBox.Show($"Report saved successfully at: {saveFileDialog.FileName}");
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void SearchLecturer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string lecturerID = IDTextBox.Text;

                if (string.IsNullOrEmpty(lecturerID))
                {
                    MessageBox.Show("Please enter an ID to search.");
                    return;
                }

                string connectionString = "Server=labG9AEB3\\SQLEXPRESS;Database=ST10403075_PROG6212_PART2_POE;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT Name, FROM Lecturer WHERE ID = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@lecturerID", lecturerID);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                txtFirstName.Text = reader["FirstName"].ToString();
                                
                            }
                            else
                            {
                                MessageBox.Show("User not found.");
                                ResetLecturerFields();
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void UpdateLecturer_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string firstName = txtFirstName.Text;
                string LecturerID = IDTextBox.Text;


                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(LecturerID))
                {
                    MessageBox.Show("Please fill in all fields.");
                    return;
                }

                string connectionString = "Server=labG9AEB3\\SQLEXPRESS;Database=ST10403075_PROG6212_PART2_POE;Trusted_Connection=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Lecturer SET Name = @Name WHERE lecturerID = @lecturerID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@lecturerID", IDTextBox);
                        cmd.Parameters.AddWithValue("@FirstName", firstName);
                        
 

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show(" Information updated successfully!");
                }
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show($"Database error: {sqlEx.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ResetLecturerFields()
        {
            txtFirstName.Text = "";
            IDTextBox.Text = "";
        }

    }
}
