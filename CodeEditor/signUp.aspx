<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signUp.aspx.cs" Inherits="CodeEditor.signUp" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="signUpstyle.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
         <div class="form-container">
     <h2>Sign Up here !</h2>
     <div class="input-group">
         <label for="username"><i class="fas fa-user"></i></label>
         <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Username" required="true"></asp:TextBox>
     </div>

     <div class="input-group">
         <label for="email"><i class="fas fa-envelope"></i></label>
         <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Email" required="true"></asp:TextBox>
     </div>

     <div class="input-group">
         <label for="password"><i class="fas fa-lock"></i></label>
         <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Password" required="true"></asp:TextBox>
     </div>

     <div class="input-group">
         <label for="confirm_password"><i class="fas fa-lock"></i></label>
         <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm Password" required="true"></asp:TextBox>
     </div>

     <asp:Button ID="Button" runat="server" Text="Register" CssClass="btn" OnClick="Register_Click" />
     <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
             <div class="login-link">
    Already have an account? <a href="LoginForm.aspx">login</a>
</div>
 </div>
    </form>
</body>
</html>
