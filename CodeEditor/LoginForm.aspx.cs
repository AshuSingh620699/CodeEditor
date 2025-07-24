using System;
using System.Data.SqlClient;

namespace CodeEditor
{
    public partial class userLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void LoginButton_Click(object sender, EventArgs e)
        {
            string usernameOrEmail = usernameEmail.Text.Trim();
            string userPassword = password.Text.Trim();
            userPassword = Hashing.Pass(userPassword);

            if (string.IsNullOrEmpty(usernameOrEmail) || string.IsNullOrEmpty(userPassword))
            {
                // Show error message if either field is empty
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Username/Email and Password are required.');", true);
                return;
            }

            // Define connection string
            string connectionString = "Server=Ashutosh\\SQLEXPRESS;Database=ASH;Integrated Security=True;";

            // Determine if the input is an email (contains '@') or username
            string query;
            SqlCommand cmd;

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                try
                {

                    if (usernameOrEmail.Contains("@"))
                    {
                        // Input is email
                        query = "SELECT ID, Password FROM Users WHERE Email = @UsernameOrEmail";
                    }
                    else
                    {
                        // Input is username
                        query = "SELECT ID, Password FROM Users WHERE Username = @UsernameOrEmail";
                    }

                    cmd = new SqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@UsernameOrEmail", usernameOrEmail);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        string storedPassword = reader["Password"].ToString();


                        // You can use a hashing algorithm like BCrypt to compare hashed passwords
                        if (storedPassword == userPassword) // Compare plain-text for simplicity (not recommended)
                        {
                            // Authentication success
                            Session["ID"] = reader["ID"].ToString();
                            Session["UsernameOrEmail"] = usernameOrEmail;
                            Response.Redirect("userDashboard.aspx");
                        }
                        else
                        {
                            // Password mismatch
                            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Invalid password.');", true);
                        }
                    }
                    else
                    {
                        // No user found with given username/email
                        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('User not found.');", true);
                    }
                }
                catch (Exception ex) {
                    lblmessage.ForeColor = System.Drawing.Color.Red;
                    lblmessage.Text = ex.Message;
                }
            }
        }
    }
}
