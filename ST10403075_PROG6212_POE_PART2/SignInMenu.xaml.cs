using System;
using System.Data.SqlClient;
using System.Windows;

namespace ST10403075_PROG6212_POE_PART2
{
    public partial class SignInMenu : Window
    {
        public SignInMenu()
        {
            InitializeComponent();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            // Get input values
            string userID = IDTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(userID) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in both fields.");
                return;
            }

            // Check credentials in all three tables
            string connectionString = "Server=labG9AEB3\\SQLEXPRESS;Database=ST10403075_PROG6212_PART2_POE;Trusted_Connection=True;";
            string queryLecturer = "SELECT COUNT(1) FROM Lecturer WHERE lecturerID = @ID AND Password = @Password";
            string queryCoordinator = "SELECT COUNT(1) FROM ProgrammeCoordinator WHERE coordinatorID = @ID AND Password = @Password";
            string queryManager = "SELECT COUNT(1) FROM AcademicManager WHERE managerID = @ID AND Password = @Password";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Check Lecturer table
                    bool isLecturer = CheckCredentials(connection, queryLecturer, userID, password);

                    // Check ProgrammeCoordinator table
                    bool isCoordinator = CheckCredentials(connection, queryCoordinator, userID, password);

                    // Check AcademicManager table
                    bool isManager = CheckCredentials(connection, queryManager, userID, password);

                    // Redirect users based on their role
                    if (isCoordinator || isManager)
                    {
                        // Redirect to Manager/Coordinator View
                        ManagerCoordinatorView managerView = new ManagerCoordinatorView();
                        managerView.Show();
                        this.Close();
                    }
                    else if (isLecturer)
                    {
                        // Redirect to MainWindow
                        MainWindow mainWindow = new MainWindow();
                        mainWindow.Show();
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Unrecognized ID or Password");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Helper method to check credentials
        private bool CheckCredentials(SqlConnection connection, string query, string userID, string password)
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", userID);
            command.Parameters.AddWithValue("@Password", password);

            int count = Convert.ToInt32(command.ExecuteScalar());
            return count == 1;
        }
        private void NeedAccount_Click(object sender, RoutedEventArgs e)
        {
            // Redirect to LecturerSignUp window
            LecturerSignUp signUpWindow = new LecturerSignUp();
            signUpWindow.Show();
            this.Close(); // Close the SignInMenu window
        }
    }
}