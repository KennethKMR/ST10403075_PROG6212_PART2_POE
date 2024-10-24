using System;
using System.Diagnostics.Contracts;
using System.Windows;

namespace ST10403075_PROG6212_POE_PART2
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SubmitClaim_Click(object sender, RoutedEventArgs e)
        {
            // Collect data from input fields
            string contractId = ContractID.Text;
            double hoursWorked;
            double hourlyRate;
            bool isHoursValid = double.TryParse(HoursWorked.Text, out hoursWorked);
            bool isRateValid = double.TryParse(HourlyRate.Text, out hourlyRate);
            DateTime? claimDate = ClaimDate.SelectedDate;

            if (string.IsNullOrEmpty(contractId) || !isHoursValid || !isRateValid || !claimDate.HasValue)
            {
                MessageBox.Show("Please enter valid data for all fields.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Calculate Claim Amount
            double claimAmount = hoursWorked * hourlyRate;
            ClaimAmount.Text = claimAmount.ToString("C");
            ClaimStatus.Text = "Submitted";

            // Logic to save the claim to the database can be added here
            MessageBox.Show($"Claim for {contractId} has been submitted successfully.\nTotal Amount: {claimAmount:C}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ResetForm_Click(object sender, RoutedEventArgs e)
        {
            // Clear all input fields
            ContractID.Clear();
            HoursWorked.Clear();
            HourlyRate.Clear();
            ClaimAmount.Clear();
            ClaimDate.SelectedDate = null;
            ClaimStatus.Text = "Pending";
        }
    }
}