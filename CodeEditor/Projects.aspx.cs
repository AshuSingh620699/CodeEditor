using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;

namespace CodeEditor
{
    public partial class Projects : System.Web.UI.Page
    {

        private static readonly string JDoodleClientID = Environment.GetEnvironmentVariable("JDOODLE_CLIENT_ID");
        private static readonly string JDoodleClientSecret = Environment.GetEnvironmentVariable("JDOODLE_CLIENT_SECRET");


        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsernameOrEmail"] == null)
            {
                Response.Redirect("LoginForm.aspx");
            }
            if (!IsPostBack)
            {
                string projectId = Request.QueryString["projectId"];
                if (!string.IsNullOrEmpty(projectId))
                {
                    loadProject(projectId);
                }
            }
            if (hfShowConsole.Value == "true")
            {
                terminal.Visible = true;
                // Optionally register JS to show console again
                ScriptManager.RegisterStartupScript(this, GetType(), "ShowConsole", "document.getElementById('consoleSection').style.display='block';", true);
            }


        }

        protected async void RunButton_Click(object sender, EventArgs e)
        {
            terminal.Value = "Running code...\n";
            string code = editor.Value;
            string input = InputBox.Text;
            var language = ddlLanguage.SelectedValue;
            await ExecuteCodeAsync(code, input); // Initiate code execution

        }

        [WebMethod]
        protected async Task ExecuteCodeAsync(string code, string input)
        {
            string language = ddlLanguage.SelectedValue;
            var apiUrl = "https://api.jdoodle.com/v1/execute";
            System.Diagnostics.Debug.WriteLine("Client ID: " + JDoodleClientID);
            System.Diagnostics.Debug.WriteLine("Secret: " + JDoodleClientSecret);


            var requestData = new
            {
                clientId = JDoodleClientID,
                clientSecret = JDoodleClientSecret,
                script = code,
                language = language,
                versionIndex = 3,
                stdin = input ?? "" ,// Ensure no versionIndex is passed here

            };

            string jsonRequest = JsonConvert.SerializeObject(requestData);

            // Log the request to see the payload being sent
            Console.WriteLine("Request Data: " + jsonRequest);

            using (var httpClient = new HttpClient())
            {
                try
                {
                    var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        dynamic result = JsonConvert.DeserializeObject(jsonResponse);
                        AppendToConsole(result.output.ToString());
                    }
                    else
                    {
                        var errorResponse = await response.Content.ReadAsStringAsync();
                        AppendToConsole($"Error: {response.StatusCode}. Details: {errorResponse}");
                    }
                }
                catch (HttpRequestException httpEx)
                {
                    AppendToConsole($"HttpRequestException: {httpEx.Message}");
                }
                catch (Exception ex)
                {
                    AppendToConsole($"Exception: {ex.Message}");
                }
            }
        }


        private void AppendToConsole(string text)
        {
            terminal.Value += $"{text}";
        }

        protected void loadProject(string projectID)
        {
            int projectId = Convert.ToInt32(projectID);
            string connStr = "Data Source=Ashutosh\\SQLEXPRESS;Initial Catalog=ASH;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                string query = "SELECT ProjectName, Language, Code FROM Projects WHERE ProjectID = @ProjectID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ProjectID", projectId);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtProjectName.Text = reader["ProjectName"].ToString();
                    ddlLanguage.SelectedValue = reader["Language"].ToString();
                    editor.Value = reader["Code"].ToString();
                    hfProjectID.Value = projectId.ToString();
                }
                conn.Close();
            }
        }

        protected void btnSaveProject_Click(object sender, EventArgs e)
        {
            string projectName = txtProjectName.Text;
            string language = ddlLanguage.SelectedValue;
            string code = editor.Value;
            string connStr = "Data Source=Ashutosh\\SQLEXPRESS;Initial Catalog=ASH;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connStr))
            {
                SqlCommand cmd;
                if (!string.IsNullOrEmpty(hfProjectID.Value)) // Edit existing project
                {
                    int projectId = Convert.ToInt32(hfProjectID.Value);
                    string query = "UPDATE Projects SET ProjectName = @ProjectName, Language = @Language, Code = @Code WHERE ProjectID = @ProjectID";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ProjectID", projectId);
                }
                else // Create new project
                {
                    string query = "INSERT INTO Projects (ID, ProjectName, Language, Code, LastEdited) VALUES (@ID, @ProjectName, @Language, @Code, @LastEdited)";
                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", Session["ID"]);
                }

                cmd.Parameters.AddWithValue("@ProjectName", projectName);
                cmd.Parameters.AddWithValue("@Language", language);
                cmd.Parameters.AddWithValue("@Code", code);
                cmd.Parameters.AddWithValue("@LastEdited", DateTime.Now);

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Project saved successfully!');", true);
        }
    }
}
