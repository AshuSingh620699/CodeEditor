using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

namespace CodeEditor
{
    public partial class signUp : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void Register_Click(object sender, EventArgs e)
        {
            string userN = txtUsername.Text;
            string email = txtEmail.Text;
            string password = txtPassword.Text;
            string Cpassword = txtConfirmPassword.Text;

            if (string.IsNullOrWhiteSpace(userN))
            {
                lblMessage.Text = "Username cannot be empty.";
                return;
            }
            if (string.IsNullOrWhiteSpace(email))
            {
                lblMessage.Text = "Email cannot be empty.";
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                lblMessage.Text = "Password cannot be empty.";
                return;
            }
            if (string.IsNullOrWhiteSpace(Cpassword))
            {
                lblMessage.Text = "Confirm Password cannot be empty.";
                return;
            }
            if (password.Length < 8)
            {
                lblMessage.Text = "passwords must contain atleast 8 alphabets or symbols.";
                return;

            }
            if (password != Cpassword)
            {
                lblMessage.Text = "Passwords do not match.";
                return;
            }

            string pass = password;
            password = Hashing.Pass(password);


            // Define connection string
            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string selectquery = "SELECT COUNT(*) FROM Users WHERE Username = @Username OR Email = @Email";
                using (SqlCommand Scmd = new SqlCommand(selectquery, connection))
                {
                    Scmd.Parameters.AddWithValue("@Username", userN);
                    Scmd.Parameters.AddWithValue("@Email", email);
                    try
                    {
                        connection.Open();

                        int user = (int)Scmd.ExecuteScalar();

                        if (user > 0)
                        {
                            lblMessage.ForeColor = System.Drawing.Color.Red;
                            lblMessage.Text = "Username or email already exists.";
                            return;
                        }
                    }
                    catch (SqlException ex)
                    {
                        lblMessage.ForeColor = System.Drawing.Color.Red;
                        lblMessage.Text = "Error checking user existence: " + ex.Message;
                        return;
                    }
                }

                string query = "INSERT INTO Users (Username, Email, Password) VALUES (@Username, @Email, @Password)";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", userN);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);
                    try
                    {
                        cmd.ExecuteNonQuery();
                        lblMessage.ForeColor = System.Drawing.Color.Green;
                        lblMessage.Text = "Registration successful!";

                        // Call the email-sending method
                        SendSignUpEmail(email, userN);
                    }
                    catch (SqlException ex)
                    {
                        lblMessage.Text = "Registration failed: " + ex.Message;
                    }
                }
            }
            savePassword(userN, pass);
        }
        private void savePassword(string usernameOrEmail, string userPassword)
        {

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "insert into passwords (username, password) values (@username, @password)";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@username", usernameOrEmail);
                cmd.Parameters.AddWithValue("@password", userPassword);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        private void SendSignUpEmail(string toEmail, string userName)
        {
            try
            {
                // Email configuration
                string fromEmail = "razzputashu9821@gmail.com";
                string fromPassword = "hhbhvyuccctgeavx";
                string subject = "Welcome to Code Editor!";
                string body = $@"
                    <html>
                    <body>
                        <h1>Welcome to Code Editor!</h1>
                        <p>Dear {userName},</p>
                        <p>Thank you for signing up for Code Editor. We're excited to have you on board!</p>
                        <p>Best Regards,<br/>The Code Editor Team</p>
                    </body>
                    </html>";

                // Create the email message
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(fromEmail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;

                // Configure the SMTP client
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential(fromEmail, fromPassword),
                    EnableSsl = true
                };

                // Send the email
                smtpClient.Send(mail);
            }
            catch (Exception ex)
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Registration successful, but failed to send email: " + ex.Message;
            }
        }
    }
}