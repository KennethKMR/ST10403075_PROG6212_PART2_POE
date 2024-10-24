using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace ST10403075_PROG6212_POE_PART2
{
    public partial class MainWindow : Window
    {
        private string uploadedDocumentPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Calculate Claim Amount button click
        private void CalculateClaimAmount_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(HoursWorkedTextBox.Text, out decimal hoursWorked) &&
                decimal.TryParse(HourlyRateTextBox.Text, out decimal hourlyRate))
            {
                decimal claimAmount = hoursWorked * hourlyRate;
                ClaimAmountTextBox.Text = claimAmount.ToString("F2");
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for Hours Worked and Hourly Rate.");
            }
        }

        // Upload PDF button click
        private void UploadPDF_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PDF files (*.pdf)|*.pdf",
                Title = "Select Supporting Document"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileInfo fileInfo = new FileInfo(openFileDialog.FileName);
                if (fileInfo.Length > 1 * 1024 * 1024)
                {
                    MessageBox.Show("File size exceeds 1MB limit. Please select a smaller file.");
                }
                else
                {
                    uploadedDocumentPath = openFileDialog.FileName;
                    MessageBox.Show("PDF uploaded successfully.");
                }
            }
        }

        // Submit Claim button click
        private void SubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            string lecturerID = LecturerIDTextBox.Text;
            string hoursWorked = HoursWorkedTextBox.Text;
            string hourlyRate = HourlyRateTextBox.Text;
            string claimAmount = ClaimAmountTextBox.Text;
            DateTime? claimDate = ClaimDatePicker.SelectedDate;

            if (string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(hoursWorked) ||
                string.IsNullOrEmpty(hourlyRate) || string.IsNullOrEmpty(claimAmount) || claimDate == null)
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Database insertion logic here
            MessageBox.Show("Claim submitted successfully!");
        }
    }
}