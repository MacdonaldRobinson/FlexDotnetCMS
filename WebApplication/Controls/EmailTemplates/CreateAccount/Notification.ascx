<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Notification.ascx.cs" Inherits="WebApplication.Controls.EmailTemplates.CreateAccount.Notification" %>

<h2>New Account was Created</h2>
<div>
    <label>First Name:</label>
    <%= User.FirstName %>
</div>
<div>
    <label>Last Name:</label>
    <%= User.LastName %>
</div>
<div>
    <label>Email Address:</label>
    <%= User.EmailAddress %>
</div>
<div>
    <label>Category:</label>
    <%= User.Roles.ElementAt(0).Name %>
</div>