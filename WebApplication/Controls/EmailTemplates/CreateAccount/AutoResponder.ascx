<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AutoResponder.ascx.cs" Inherits="WebApplication.Controls.EmailTemplates.CreateAccount.AutoResponder" %>

<style>
	div {
		margin-bottom: 10px;
	}
	label {
		display: block;
		margin-bottom: 5px;
	}
</style>

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
    <label>Organization Name:</label>
    <%= User.OrganizationName %>
</div>

<div>
    <label>Number Of Employees:</label>
    <%= User.NumberOfEmployees %>
</div>

<div>
    <label>Your Role:</label>
    <%= User.YourRole %>
</div>

<div>
    <label>Industry:</label>
    <%= User.Industry %>
</div>

<div>
    <label>How did you hear about Alberta Healthy Workplaces?</label>
    <%= User.HowDidYouHear %>
</div>

<div>
    <label>I would like to recieve email updates about the program and upcoming workplace health initiatives? <%= User.WouldLikeToRecieveEmailUpdates %></label>    
</div>

<h3>Login Credentials</h3>
<div>
    <a href='<%=URIHelper.BaseUrl %>login'>Click Here</a> to Login
</div>
<div>
    <label>User Name:</label>
    <%= User.UserName %>
</div>