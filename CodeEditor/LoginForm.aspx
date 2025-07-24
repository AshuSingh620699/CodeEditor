<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LoginForm.aspx.cs" Inherits="CodeEditor.userLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="signUpstyle.css" />
</head>
<body>
    <form id="loginForm" runat="server">
        <div class="form-container">
            <h2>Login here !</h2>
            <div class="input-group">
                <label for="usernameEmail"></label>
                <asp:TextBox ID="usernameEmail" runat="server" CssClass="form-control" placeholder="Enter your username or email" />
                <div class="forgot-link">
                    <a href="forgot.aspx">forgotten password?</a>
                </div>
            </div>
            <div class="input-group">
                <label for="password"></label>
                <asp:TextBox ID="password" runat="server" TextMode="Password" CssClass="form-control" placeholder="Enter your password" />
            </div>
            <asp:Button ID="Button" runat="server" Text="Login" OnClick="LoginButton_Click" CssClass="login-button" />
            <asp:Label ID="lblmessage" runat="server"></asp:Label>
            <div class="signup-link">
                Don't have an account? <a href="signUp.aspx">Sign up</a>
            </div>
        </div>
    </form>
</body>
</html>
