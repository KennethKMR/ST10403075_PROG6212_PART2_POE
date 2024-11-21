using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows;

namespace ST10403075_PROG6212_POE_PART2
{
    public partial class MainWindow : Window
    {
        private byte[] pdfData; // To store the uploaded PDF

        public MainWindow()
        {
            InitializeComponent();
        }

        private void CalculateClaimAmount_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(HoursWorkedTextBox.Text, out decimal hoursWorked) &&
                decimal.TryParse(HourlyRateTextBox.Text, out decimal hourlyRate))
            {
                decimal claimAmount = hoursWorked * hourlyRate;
                ClaimAmountTextBox.Text = claimAmount.ToString("F2"); // Format to two decimal places
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for hours worked and hourly rate.");
            }
        }

        private void UploadPDF_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "PDF Files (*.pdf)|*.pdf"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                if (fileInfo.Length <= 1048576) // 1 MB limit
                {
                    pdfData = File.ReadAllBytes(openFileDialog.FileName);
                }
                else
                {
                    MessageBox.Show("File is too large. Please upload a PDF no larger than 1 MB.");
                }
            }
        }

        private void SubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=labG9AEB3\\SQLEXPRESS;Database=ST10403075_PROG6212_PART2_POE;Trusted_Connection=True;";

            if (decimal.TryParse(HoursWorkedTextBox.Text, out decimal hoursWorked) &&
                decimal.TryParse(HourlyRateTextBox.Text, out decimal hourlyRate) &&
                DateTime.TryParse(SubmittedDatePicker.Text, out DateTime submittedDate))
            {
                decimal claimAmount = hoursWorked * hourlyRate;

                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = @"
                            INSERT INTO Claims (submittedDate, claimAmount, hourlyRate, hoursWorked, supportingDocument, lecturerID)
                            VALUES (@submittedDate, @claimAmount, @hourlyRate, @hoursWorked, @supportingDocument, @lecturerID)";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@submittedDate", submittedDate);
                            command.Parameters.AddWithValue("@claimAmount", claimAmount);
                            command.Parameters.AddWithValue("@hourlyRate", hourlyRate);
                            command.Parameters.AddWithValue("@hoursWorked", hoursWorked);
                            command.Parameters.AddWithValue("@supportingDocument", pdfData ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@lecturerID", IDTextBox.Text); // User-provided lecturer ID

                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Claim submitted successfully.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please enter valid data.");
            }
        }
    }
}