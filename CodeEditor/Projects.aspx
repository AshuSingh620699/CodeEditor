<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Projects.aspx.cs" Inherits="CodeEditor.Projects" Async="true" ValidateRequest="false" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Full-Screen Code Editor</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/codemirror.min.css" />
    <script src="https://unpkg.com/xterm/lib/xterm.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/codemirror.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/mode/javascript/javascript.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/mode/python/python.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/mode/clike/clike.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/codemirror/5.65.5/addon/edit/closebrackets.min.js"></script>
    <style>
        body, html {
            margin: 0;
            padding: 0;
            height: 100%;
            overflow: hidden;
            font-family: Arial, sans-serif;
        }

        #editorContainer {
            position: absolute;
            top: 0;
            bottom: 0;
            left: 0;
            right: 0;
            display: flex;
            flex-direction: column;
            z-index: 10;
        }

        #editorWrapper {
            flex: 1;
            overflow-y: auto; /* Enable scrolling inside the editor */
            border: 1px solid #ddd;
        }

        #controls {
            height: 25px;
            display: flex;
            justify-content: flex-end;
            align-items: center;
            padding: 10px;
            background-color: #f8f8f8;
            border-top: 1px solid #ddd;
        }

        .button {
            padding: 8px 16px;
            margin-left: 10px;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            font-size: 16px;
            margin: 10px 5px;
        }

        .execute-button {
            background-color: #4CAF50;
        }

        .save-button {
            background-color: #2196F3;
        }

        .cancel-button {
            background-color: #f44336;
        }

        .button:hover {
            opacity: 0.9;
        }

        /* Modal Overlay */
        .modal-overlay {
            display: none; /* Initially hidden */
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            z-index: 100;
        }

        /* Modal Content Styling */
        .modal-content {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            padding: 20px;
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.2);
            width: 400px;
            max-width: 90%;
            z-index: 1001;
        }

            /* Title Styling */
            .modal-content h3 {
                font-size: 1.6em;
                color: #333;
                margin-bottom: 20px;
            }

        /* Form Input Styling */
        .form-group {
            margin-bottom: 20px;
        }

        .enhanced-input {
            padding: 12px 20px; /* Increase padding for larger input */
            border-radius: 10px; /* Rounded corners */
            border: 1px solid #ccc;
            width: 80%;
            font-size: 1rem;
            transition: border-color 0.3s ease, box-shadow 0.3s ease;
            margin: 8px 0;
        }

            /* Focus styling for inputs */
            .enhanced-input:focus {
                border-color: #007bff;
                box-shadow: 0 0 8px rgba(0, 123, 255, 0.5);
                outline: none;
            }

        /* Error Message Styling */
        .text-danger {
            font-size: 0.875rem;
            color: red;
        }


        #consoleSection {
            display: none;
            background-color: #1e1e1e;
            color: #d4d4d4;
            height: 200px;
            overflow-y: auto;
            padding: 10px;
            font-family: monospace;
            position: relative;
        }

        #closeConsole {
            position: absolute;
            top: 10px;
            right: 10px;
            color: #fff;
            background: #444;
            border-radius: 50%;
            width: 25px;
            height: 25px;
            display: flex;
            align-items: center;
            justify-content: center;
            cursor: pointer;
            font-weight: bold;
            font-size: 16px;
        }

            #closeConsole:hover {
                background: #ff4444;
            }
             #terminal{
                 background:#1e1e1e;
                 width:98%;
                 height:100%;
                 color:#fff;
                 border:none;
             }
             #InputBox{
                 width:98%;
                 height:30px;
                 background-color:#1e1e1e;
                 color:#fff;
                 margin:5px 0;
                 border:none;
             }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfShowConsole" runat="server" />

        <div id="editorContainer">

            <!-- Controls with Execute, Save, and Cancel Buttons -->
            <div id="controls">
                <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="button cancel-button" OnClientClick="goBack(); return false;" />
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="button save-button" OnClientClick="showSaveSection(); return false;" />
                <asp:Button ID="btnExecute" runat="server" Text="Execute" CssClass="button execute-button" OnClientClick="executeCode()" OnClick="RunButton_Click" />
            </div>

            <!-- CodeMirror Wrapper -->
            <div id="editorWrapper">
                <textarea id="editor" name="code" runat="server">// Write your code here</textarea>
            </div>

            <div id="consoleSection">
                <div id="closeConsole" onclick="closeConsole()">×</div>
                <asp:TextBox ID="InputBox" runat="server" CssClass="input-box" placeholder="Enter input and press Enter" AutoPostBack="true"></asp:TextBox>
               <textarea id="terminal" name="term" runat="server"></textarea>
            </div>

            <!-- Hidden Section for Project Details -->
            <div id="projectDetailsSection" class="modal-overlay">
                <div class="modal-content">
                    <asp:HiddenField ID="hfProjectID" runat="server" />
                    <h3 class="text-center mb-4">Enter Project Details</h3>

                    <!-- Project Name Field -->
                    <div class="form-group">
                        <asp:Label ID="lblProjectName" runat="server" Text="Project Name:" AssociatedControlID="txtProjectName" CssClass="control-label" />
                        <asp:TextBox ID="txtProjectName" runat="server" CssClass="form-control enhanced-input" placeholder="Enter Project Name" Required="true" />
                        <span class="text-danger" id="projectNameError" style="display: none;">Please enter a project name.</span>
                    </div>

                    <!-- Project Language Field -->
                    <div class="form-group">
                        <asp:Label ID="lblddlLanguage" runat="server" Text="Project Language:" AssociatedControlID="ddlLanguage" CssClass="control-label" />
                        <asp:DropDownList ID="ddlLanguage" runat="server" CssClass="form-control enhanced-input">
                            <asp:ListItem Value="" Text="Select Language" />
                            <asp:ListItem Value="c">C</asp:ListItem>
                            <asp:ListItem Value="python3">Python</asp:ListItem>
                            <asp:ListItem Value="cpp">C++</asp:ListItem>
                        </asp:DropDownList>
                        <span class="text-danger" id="languageError" style="display: none;">Please select a project language.</span>
                    </div>

                    <!-- Save and Cancel Buttons -->
                    <div class="form-group text-center">
                        <asp:Button ID="btnSaveProject" runat="server" Text="Save" CssClass="button save-button" OnClick="btnSaveProject_Click" />
                        <asp:Button ID="btnCancelProject" runat="server" Text="Cancel" CssClass="button cancel-button" OnClientClick="hideSaveSection(); return false;" />
                    </div>
                </div>
            </div>

        </div>

        <!-- Initialize CodeMirror as Full-Screen Editor with Internal Scrolling -->
        <script>
            var language = document.getElementById('<%= ddlLanguage.ClientID %>')
            var editor = CodeMirror.fromTextArea(document.getElementById("editor"), {
                lineNumbers: true,
                mode: "javascript",  // Default mode
                theme: "default",
                autoCloseBrackets: true,
                lineWrapping: true,
                tabSize: 4,
                matchBrackets: true
            });

            // Update editor mode when language is selected
            function updateEditorMode() {
                var selectedLanguage = language.value;
                var mode;
                switch (selectedLanguage) {
                    case "javascript": mode = "javascript"; break;
                    case "c": mode = "text/x-csrc"; break;
                    case "cpp": mode = "text/x-c++src"; break;
                    case "python3": mode = "python"; break;
                    default: mode = "javascript";
                }
                editor.setOption("mode", mode);
            }

            language.addEventListener("change", updateEditorMode);


            function goBack() {
                if (confirm("Are you sure you want to cancel?")) {
                    window.history.back();
                }
            }

            function showSaveSection() {
                document.getElementById('projectDetailsSection').style.display = 'block';
            }

            function hideSaveSection() {
                // Hide the hidden section and overlay, remove blur from main content
                document.getElementById('projectDetailsSection').style.display = 'none';
            }
            function executeCode() {
                document.getElementById('<%= hfShowConsole.ClientID %>').value = 'true';
            }
            // Make the editor take up the entire #editorWrapper area
            editor.setSize("100%", "100%");

            function closeConsole() {
                document.getElementById("consoleSection").style.display = "none";
            }
        </script>
        
    </form>
</body>
</html>

