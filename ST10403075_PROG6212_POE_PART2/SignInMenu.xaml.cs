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
            string lecturerID = IDTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(lecturerID) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in both fields.");
                return;
            }

            // Check credentials in the database
            string connectionString = "Server=labG9AEB3\\SQLEXPRESS;Database=ST10403075_PROG6212_PART2_POE;Trusted_Connection=True;";
            string query = "SELECT COUNT(1) FROM Lecturer WHERE lecturerID = @lecturerID AND Password = @Password";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@lecturerID", lecturerID);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    if (count == 1)
                    {
                        MessageBox.Show("Welcome");
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

        private void NeedAccount_Click(object sender, RoutedEventArgs e)
        {
            // Redirect to LecturerSignUp window
            LecturerSignUp signUpWindow = new LecturerSignUp();
            signUpWindow.Show();
            this.Close();
        }
    }
}