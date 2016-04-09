<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoResponder.ascx.cs" Inherits="WebApplication.Controls.EmailTemplates.CreateAccount.AutoResponder" %>

<h2><%=GetHeading() %></h2>
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

<h3>Login Credentials</h3>
<div>
    <a href='<%=URIHelper.BaseUrl %>login'>Click Here</a> to Login
</div>
<div>
    <label>User Name:</label>
    <%= User.UserName %>
</div>
<div>
    <label>Password:</label>
    <%= GetPassword() %>
</div>