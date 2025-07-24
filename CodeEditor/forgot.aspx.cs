using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodeEditor
{
    public partial class forgot : System.Web.UI.Page
    {
        private static string OTP = "";
        static string constr = "Server=Ashutosh\\SQLEXPRESS;Database=ASH;Integrated Security=True;";
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void SendOTPButton_Click(object sender, EventArgs e)
        {
            string usernameOrEmail = usernameEmail.Text.Trim();

            // Check if the username/email exists in the database
            if (IsUserExists(usernameOrEmail))
            {
                // Generate OTP
                string generatedOTP = GenerateOTP();

                // Store OTP in session
                Session["OTP"] = generatedOTP;

                // Send OTP to the user's email
                SendEmail(usernameOrEmail, "Your OTP for Password Reset", $"Your OTP is: {generatedOTP}");

                // Show OTP and New Password fields
                panel1.Visible = false;
                Step2.Visible = true;

                lblMessage.Text = "OTP sent to your email. Please check your inbox.";
                lblMessage.Visible = true;
            }
            else
            {
                lblMessage.Text = "Username or email not found.";
                lblMessage.Visible = true;
            }
        }

        protected void btnResetPassword_Click(object sender, EventArgs e)
        {
            string otp = txtOTP.Text.Trim();
            string newPassword = txtNewPassword.Text.Trim();
            string email = usernameEmail.Text.Trim();

            savePassword(email, newPassword);

            // Retrieve OTP from session
            if (Session["OTP"] == null)
            {
                lblMessage.Text = "OTP has expired or is not available. Please request a new OTP.";
                lblMessage.Visible = true;
                return;
            }

            string generatedOTP = Session["OTP"].ToString();

            // Validate OTP
            if (otp == generatedOTP)
            {
                // Hash the new password
                string hashedPassword = Hashing.Pass(newPassword);

                // Update the password in the database
                string query = "UPDATE users SET password = @newPassword WHERE Email = @Email";
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@newPassword", hashedPassword);
                        cmd.Parameters.AddWithValue("@Email", email);

                        conn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        if (rows > 0)
                        {
                            lblMessage.Text = "Password reset successfully! You can now log in with your new password.";
                            lblMessage.Visible = true;

                            // Clear the OTP from the session
                            Session["OTP"] = null;

                            // Redirect to login page after 3 seconds
                            Response.AddHeader("REFRESH", "3;URL=LoginForm.aspx");
                        }
                        else
                        {
                            lblMessage.Text = "Something went wrong. Please try again.";
                            lblMessage.Visible = true;
                        }
                    }
                }
            }
            else
            {
                lblMessage.Text = "Invalid OTP. Please try again.";
                lblMessage.Visible = true;
            }
        }

        private void savePassword(string email, string newPassword)
        {
            string user = "";
            if (email.Contains("@"))
            {
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string query = "select Username from Users where Email =  @email";
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@email", email);
                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user = reader["Username"].ToString();
                    }
                    con.Close();
                }
            }
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "update passwords set password = @password where username = @user";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@user", user);
                cmd.Parameters.AddWithValue("@password", newPassword);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        protected bool IsUserExists(string usernameOrEmail) {
            bool exists = false;
            string constr = "Server=Ashutosh\\SQLEXPRESS;Database=ASH;Integrated Security=True;";
            string query = "select count(*) from users where Email = @user";
            if (usernameOrEmail != null)
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@user", usernameOrEmail);
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            exists = true;
                        }
                    }
                }
            }
            return exists;
        }
        private void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(toEmail);
                mail.From = new MailAddress("razzputashu9821@gmail.com"); // Replace with your email
                mail.Subject = subject;
                mail.Body = body;

                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // Use your SMTP server
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("razzputashu9821@gmail.com", "hhbhvyuccctgeavx"), // Replace with your credentials
                    EnableSsl = true
                };

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                lblMessage.Text = "Error sending email: " + ex.Message;
                lblMessage.Visible = true;
            }
        }

        private string GenerateOTP()
        {
            Random random = new Random();
            return random.Next(100000, 999999).ToString();
        }
    }
}