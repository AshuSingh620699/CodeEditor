<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="forgot.aspx.cs" Inherits="CodeEditor.forgot" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="signUpstyle.css" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="form-container">
            <asp:Panel ID="panel1" runat="server">
                <h2>forgot password !</h2>
                <div class="input-group">
                    <label for="usernameEmail"></label>
                    <asp:TextBox ID="usernameEmail" runat="server" CssClass="form-control" placeholder="Enter your username or email" />
                    <asp:RequiredFieldValidator ID="rfvUsernameOrEmail" runat="server" ControlToValidate="usernameEmail" ErrorMessage="Username or Email is required." CssClass="error"></asp:RequiredFieldValidator>
                </div>
                <asp:Button ID="Button" runat="server" Text="Send OTP" OnClick="SendOTPButton_Click" CssClass="login-button" />
            </asp:Panel>

             <asp:Panel ID="Step2" runat="server" Visible="false">
                <!-- Step 2: OTP -->
                <div class="input-group">
                    <label for="txtOTP"></label>
                    <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control" Placeholder="Enter OTP"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvOTP" runat="server" ControlToValidate="txtOTP" ErrorMessage="OTP is required." CssClass="error"></asp:RequiredFieldValidator>
                </div>

                <!-- Step 3: New Password -->
                <div class="input-group">
                    <label for="txtNewPassword"></label>
                    <asp:TextBox ID="txtNewPassword" runat="server" CssClass="form-control" TextMode="Password" Placeholder="Enter new password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvNewPassword" runat="server" ControlToValidate="txtNewPassword" ErrorMessage="Password is required." CssClass="error"></asp:RequiredFieldValidator>
                </div>

                <asp:Button ID="Button1" runat="server" Text="Reset Password" OnClick="btnResetPassword_Click" CssClass="btn-primary" />
            </asp:Panel>

            <asp:Label ID="lblMessage" runat="server" CssClass="success-message" Visible="false"></asp:Label>

        </div>
    </form>
</body>
</html>
