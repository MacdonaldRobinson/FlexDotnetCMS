<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailChipSignupForm.ascx.cs" Inherits="WebApplication.Controls.MailChipSignupForm" %>
<div class="signup-top">
    <asp:TextBox runat="server" ID="EmailAddress" Placeholder="Your Email Address" />
    <asp:LinkButton runat="server" ID="Submit" OnClick="Submit_Click" class="progress-button" data-style="fill" data-horizontal>Sign Up</asp:LinkButton>
</div>
<div class="signup-bottom">
    <asp:Panel runat="server" ID="SuccessPanel" Visible="false" CssClass="success-panel">
        <i class="fa fa-check"></i>
        <div class="success-message">
            A confirmation email has been sent to the email address provided
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="ErrorPanel" Visible="false" CssClass="error-panel">
        <i class="fa fa-times"></i>
        <div class="error-message">
            Something went wrong! Please enter a valid email address and try again.
        </div>
    </asp:Panel>
</div>