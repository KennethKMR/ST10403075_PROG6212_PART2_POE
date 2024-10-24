using System.Windows;
using System.Windows.Controls.Primitives;

namespace ST10403075_PROG6212_POE_PART2
{
    public partial class ManagerCoordinatorView : Window
    {
        // Sample Data Model for Claim
        public class Claim
        {
            public string ClaimID { get; set; }
            public double ClaimAmount { get; set; }
            public string SupportingDocument { get; set; }
            public string Status { get; set; } // Pending, Approved, Rejected
            public string SubmittedDate { get; set; }
            public string LecturerID { get; set; }
        }

        public ManagerCoordinatorView()
        {
            InitializeComponent();
            LoadClaims();
        }

        private void LoadClaims()
        {
            // Sample data; replace with actual data retrieval logic
            ClaimsListView.Items.Add(new Claim { ClaimID = "C001", Status = "Pending" });
            ClaimsListView.Items.Add(new Claim { ClaimID = "C002", Status = "Pending" });
        }

        private void ClaimsListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (ClaimsListView.SelectedItem is Claim selectedClaim)
            {
                ShowClaimDetails(selectedClaim);
            }
        }

        private void ShowClaimDetails(Claim claim)
        {
            ClaimDetailsPanel.Visibility = Visibility.Visible;
            ClaimIDText.Text = $"Claim ID: {claim.ClaimID}";
            ClaimAmountText.Text = $"Claim Amount: {claim.ClaimAmount}";
            SupportingDocText.Text = $"Supporting Document: {claim.SupportingDocument}";
            StatusText.Text = $"Status: {claim.Status}";
            SubmittedDateText.Text = $"Submitted Date: {claim.SubmittedDate}";
            LecturerIDText.Text = $"Lecturer ID: {claim.LecturerID}";

            // Keep track of the selected claim (if needed for approving/rejecting)
            ApproveButton.Tag = claim;
            RejectButton.Tag = claim;
        }

        private void ApproveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApproveButton.Tag is Claim claim)
            {
                claim.Status = "Approved";
                MessageBox.Show($"Claim {claim.ClaimID} approved.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshClaimListView();
                ClaimDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void RejectButton_Click(object sender, RoutedEventArgs e)
        {
            if (RejectButton.Tag is Claim claim)
            {
                claim.Status = "Rejected";
                MessageBox.Show($"Claim {claim.ClaimID} rejected.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                RefreshClaimListView();
                ClaimDetailsPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void DownloadDocButton_Click(object sender, RoutedEventArgs e)
        {
            // Implement logic to download the supporting document
            MessageBox.Show("Downloading document...", "Download", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void RefreshClaimListView()
        {
            // Refresh list to show updated status; you may need to reload data
            ClaimsListView.Items.Refresh();
        }
    }
}