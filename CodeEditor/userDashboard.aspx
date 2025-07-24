<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userDashboard.aspx.cs" Inherits="CodeEditor.userDashboard" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>DashBoard</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/codemirror.min.css"/>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/codemirror.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/mode/javascript/javascript.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/mode/python/python.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/mode/clike/clike.min.js"></script>
    <link rel="stylesheet" href="StyleSheet1.css" />
    <!--<link rel="stylesheet" href="DashBoardStyleSheet2.css" />-->

    <style>
        .CodeMirror-line{
            padding-left:40px !important;
        }
         .editor-container {
            display: flex;
            flex-direction: column;
            width: 100%;
         }
        .code-editor {
            height: 400px;
            border: 1px solid #ccc;
            margin-bottom: 20px;
        }
        .console-output {
            height: 200px;
            border: 1px solid #ccc;
            background-color: #1e1e1e;
            color: #fff;
            padding: 10px;
            white-space: pre-wrap;
        }
        .execute-btn {
            padding: 10px;
            background-color: #4CAF50;
            color: white;
            cursor: pointer;
        }
        input#btnShowMore,#btnShowLess{
            background:transparent;
            border-radius:10px;
            border:2px solid #8361be;
            padding:3px 10px;
            font-weight:500;
            cursor:pointer;
            font-size:15px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <nav class="navbar">
            <div class="logo">Code Editor</div>
            <ul class="nav-links">
                <li><a href="#">Dashboard</a></li>
                <li><a href="#">New Project</a></li>
            </ul>
            <asp:Button ID="btnLogout" runat="server" cssClass="redBtn" Text="Logout" OnClick="btnLogout_Click" />
        </nav>

        <div class="dashboard">
            <aside class="sidebar">
                <div id="side">
                <h3>Saved Projects</h3>
                <asp:Repeater ID="Repeater1" runat="server">
                    <ItemTemplate>
                        <div class="scard">
                            <h4><%# Eval("ProjectName") %></h4>
                            <p>Last Edited: <%# Eval("LastEdited", "{0:MMMM dd, yyyy}") %></p>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                <asp:Button id="Button" class="button-51" runat="server" Text="Create New Project" OnClick="CreateNewProject"/>
                </div>
            </aside>

            <main class="main-content">
                <section id="section">
                    <h1>Welcome back, <asp:Label ID="Label1" runat="server" Text=""></asp:Label></h1>
                    <p>Here are your most recent projects.</p>
                </section>
                <div id="dashboardSection">
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                    <asp:Repeater ID="rptProjects" runat="server">
                        <HeaderTemplate><h3>Recent Projects</h3></HeaderTemplate>
                        <ItemTemplate>
                            <div class="project-card">
                                <h4><%# Eval("ProjectName") %></h4>
                                <p>Language: <%# Eval("Language") %></p>
                                <p>Last Edited: <%# Eval("LastEdited") %></p>
                                <asp:Button runat="server" Text="Edit" CommandArgument='<%# Eval("ProjectID") %>' OnClick="EditProject" CssClass="edtbtn" />
                                <asp:Button runat="server" Text="Delete" CommandArgument='<%# Eval("ProjectID") %>' OnClick="DeleteProject" CssClass="edtbtn" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>

                    <asp:HiddenField ID="hfOffset" runat="server" Value="0" />

                    <asp:Button ID="btnShowMore" runat="server" Text="Show More ++ " OnClick="btnShowMore_Click" CssClass="show-more-button" />
                    <asp:Button ID="btnShowLess" runat="server" Text="Show Less -- " OnClick="btnShowLess_Click" CssClass="show-less-button" />
                </div>
                </main>
            </div>
            <!-- Include jQuery for AJAX functionality -->
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

            <script>

        // Function to show the new project form
            function showNewProjectForm() {
            document.getElementById('dashboardSection').style.display = 'none';
            document.getElementById('section').style.display = 'none';
            document.getElementById('editor').style.display = 'block';
            
        }

        // Function to reload the dashboard and reset the editor
        function reloadDashboard() {
            document.getElementById('dashboardSection').style.display = 'block';
            document.getElementById('section').style.display = 'block';
            document.getElementById('editor').style.display = 'none';
            editor.setValue(''); // Clear CodeMirror editor
        }        
            </script>
        
    </form>
</body>
</html>
