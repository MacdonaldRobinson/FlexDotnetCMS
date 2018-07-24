<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CreateAnAccount.ascx.cs" Inherits="WebApplication.Controls.CreateAnAccount" %>

<form runat="server">
	<div>
		<label>First Name</label>
		<asp:TextBox runat="server" ID="FirstName" />
	</div>
	<div>
		<label>Last Name</label>
		<asp:TextBox runat="server" ID="LastName" />
	</div>
	<div>
		<label>Email Address</label>
		<asp:TextBox runat="server" ID="EmailAddress" TextMode="Email"/>
	</div>
	<div>
		<label>Password</label>
		<asp:TextBox runat="server" ID="Password" TextMode="Password"/>
	</div>
	<div>
		<asp:Panel ID="ErrorPanel" runat="server" Visible="false" CssClass="alert alert-danger">			
			<asp:Literal ID="ServerMessage" runat="server"></asp:Literal>
		</asp:Panel>
	</div>
	<div>
		<asp:LinkButton Text="Sign Up" runat="server"  ID="Signup" OnClick="Signup_Click" />
	</div>
</form>