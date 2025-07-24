using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Web.UI.WebControls;

namespace CodeEditor
{
    public partial class userDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["UsernameOrEmail"] == null)
            {
                Response.Redirect("LoginForm.aspx");
            }
            else
            {
                Label1.Text = Session["UsernameOrEmail"].ToString();

                if (!IsPostBack)
                {
                    LoadSavedProjects();
                    LoadProjects();
                }
            }
            Label2.Visible = false;
        }
        protected void btnShowMore_Click(object sender, EventArgs e)
        {
            // Get current offset from hidden field and increase by page size (5)
            int offset = int.Parse(hfOffset.Value);
            offset += 5;
            hfOffset.Value = offset.ToString();

            LoadProjects(offset);
            LoadSavedProjects(offset);
        }
        protected void btnShowLess_Click(object sender, EventArgs e)
        {
            int offset = int.Parse(hfOffset.Value);
            offset -= 5;
            hfOffset.Value = offset.ToString();

            LoadProjects(offset);
            LoadSavedProjects(offset);
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("LoginForm.aspx");
        }

        private void LoadProjects(int offset = 0, int pageSize = 5)
        {
            int page = pageSize + offset;
            offset = 0;
            int initialPage = offset;

            if (page == 0)
            {
                Label2.Visible=true;
                Label2.ForeColor = System.Drawing.Color.Red;
                Label2.Text = "Cannot Show Less.";
                page = 5;
            }
            if (Session["ID"] != null)
            {
                string connStr = "Data Source=Ashutosh\\SQLEXPRESS;Initial Catalog=ASH;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"
                        SELECT ProjectID, ProjectName, Language, LastEdited 
                        FROM Projects 
                        WHERE ID = @ID 
                        ORDER BY ProjectName
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", Session["ID"]);
                    cmd.Parameters.AddWithValue("@Offset", initialPage);
                    cmd.Parameters.AddWithValue("@PageSize", page);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    rptProjects.DataSource = reader;
                    rptProjects.DataBind();
                    conn.Close();
                }
            }
        }

        private void LoadSavedProjects(int offset = 0, int pageSize = 5)
        {
            int page = pageSize + offset;
            offset = 0;
            int initialPage = offset;

            if (page == 0)
            {
                Label2.Visible = true;
                Label2.ForeColor = System.Drawing.Color.Red;
                Label2.Text = "Cannot Show Less.";
                page = 5;
            }
            if (Session["ID"] != null)
            {
                string connStr = "Data Source=Ashutosh\\SQLEXPRESS;Initial Catalog=ASH;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"SELECT ProjectName, LastEdited FROM Projects WHERE ID = @ID ORDER BY ProjectName
                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", Session["ID"]);
                    cmd.Parameters.AddWithValue("@Offset", initialPage);
                    cmd.Parameters.AddWithValue("@PageSize", page);
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    Repeater1.DataSource = reader;
                    Repeater1.DataBind();
                    conn.Close();
                }
            }
        }

        protected void EditProject(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            int projectId = Convert.ToInt32(btn.CommandArgument);
            Response.Redirect("Projects.aspx?projectId=" + projectId);
        }

        protected void DeleteProject(object sender, EventArgs e)
        {
            Button delbtn = (Button)sender;
            int ProjectId = Convert.ToInt32(delbtn.CommandArgument);

            using (SqlConnection con = new SqlConnection("Data Source = Ashutosh\\SQLEXPRESS; Initial Catalog = ASH; Integrated Security = True"))
            {
                string Query = "DELETE FROM PROJECTS WHERE ProjectID = @ProjectID";
                SqlCommand sql = new SqlCommand(Query, con);
                sql.Parameters.AddWithValue("@ProjectID", ProjectId);
                con.Open();
                sql.ExecuteNonQuery();
                con.Close();
            }

            // Rebind the data to update the UI
            LoadSavedProjects();
            LoadProjects();
        }
        protected void CreateNewProject(object sender, EventArgs e)
        {

            Response.Redirect("Projects.aspx");
        }

        private string ExecuteCode(string language, string code, string input)
        {
            string result = "";
            string tempFilePath = Server.MapPath("~/Temp/");

            // Ensure the Temp directory exists
            if (!Directory.Exists(tempFilePath))
            {
                Directory.CreateDirectory(tempFilePath);
            }

            // Generate unique file names
            string codeFileName = Path.Combine(tempFilePath, $"{Guid.NewGuid()}.{GetFileExtension(language)}");
            string inputFileName = Path.Combine(tempFilePath, $"{Guid.NewGuid()}.txt");

            // Write code to file
            File.WriteAllText(codeFileName, code);
            File.WriteAllText(inputFileName, input);

            string fileName = "";
            string args = "";
            string outputFileName = "";

            // Define execution parameters based on the language
            switch (language)
            {
                case "javascript":
                    fileName = @"C:\Program Files\nodejs\node.exe"; // Use the full path to node.exe
                    args = codeFileName;
                    break;
                case "java":
                    fileName = @"C:\Users\razzp\AppData\Local\Programs\Eclipse Adoptium\jdk-17.0.10.7-hotspot\bin\javac.exe"; // Use the full path to javac.exe
                    outputFileName = codeFileName.Replace(".java", "");
                    args = codeFileName;
                    break;
                case "python":
                    fileName = "python"; // Use the full path to python.exe
                    args = codeFileName;
                    break;
                case "cpp":
                    fileName = @"C:\MinGW\bin\cpp.exe"; // Use the full path to g++.exe
                    outputFileName = codeFileName.Replace(".cpp", ".exe");
                    args = $"-o {outputFileName} {codeFileName}";
                    break;
            }

            // Execute the code
            ProcessStartInfo processStartInfo = new ProcessStartInfo(fileName, args)
            {
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process { StartInfo = processStartInfo })
            {
                process.Start();
                process.WaitForExit();

                result = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();
            }

            // If Java or C++, run the compiled file
            if (language == "java" || language == "cpp")
            {
                if (File.Exists(outputFileName))
                {
                    processStartInfo = new ProcessStartInfo(language == "java" ? "java" : outputFileName, outputFileName)
                    {
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        RedirectStandardInput = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    using (Process process = new Process { StartInfo = processStartInfo })
                    {
                        process.Start();

                        // Write input to the process
                        if (!string.IsNullOrEmpty(input))
                        {
                            using (StreamWriter writer = process.StandardInput)
                            {
                                writer.WriteLine(input);
                            }
                        }

                        result = process.StandardOutput.ReadToEnd() + process.StandardError.ReadToEnd();

                        process.WaitForExit();
                    }

                    // Clean up the compiled file
                    File.Delete(outputFileName);
                }
            }

            // Clean up temporary files
            File.Delete(codeFileName);
            File.Delete(inputFileName);

            return result;
        }

        private string GetFileExtension(string language)
        {
            switch (language)
            {
                case "javascript":
                    return "js";
                case "java":
                    return "java";
                case "python":
                    return "py";
                case "cpp":
                    return "cpp";
                default:
                    throw new ArgumentException("Unsupported language");
            }
        }
    }
}
