using System;
using System.Data.SqlClient;
using System.Windows;

namespace ST10403075_PROG6212_POE_PART2
{
    public partial class LecturerSignUp : Window
    {
        public LecturerSignUp()
        {
            InitializeComponent();
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            // Get input values
            string name = NameTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please fill in all fields.");
                return;
            }

            // Generate random lecturerID
            Random random = new Random();
            int lecturerID = random.Next(10000, 99999);

            // Insert into database
            string connectionString = "Server=labG9AEB3\\SQLEXPRESS;Database=ST10403075_PROG6212_PART2_POE;Trusted_Connection=True;";
            string query = "INSERT INTO Lecturer (lecturerID, Name, Password) VALUES (@lecturerID, @Name, @Password)";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@lecturerID", lecturerID);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Password", password);

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show($"Sign-up successful! Your Lecturer ID is {lecturerID}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
