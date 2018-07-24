<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="WebApplication.Controls.Login" %>

<asp:ScriptManager runat="server"></asp:ScriptManager>

<asp:UpdatePanel runat="server" UpdateMode="Conditional">	
	<ContentTemplate>
		<asp:HiddenField ID="StoredMode" runat="server"/>
		<asp:Panel runat="server" DefaultButton="LoginButton" ID="LoginPanel">

			<asp:Panel runat="server" Visible="false" ID="ErrorPanel" CssClass="alert alert-danger">        
				<i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Invalid username or password. Please check your credentials and try again.
			</asp:Panel>

			<div>
				Username: <asp:TextBox runat="server" ID="Username" required/>
			</div>    
			<div>
				Password: <asp:TextBox runat="server" ID="Password" TextMode="Password" required/>
			</div>    
			<div>
				<asp:LinkButton Text="Login" runat="server" ID="LoginButton" OnClick="LoginButton_Click"/>
			</div>
			<div>				
				<asp:LinkButton runat="server" ID="ForgotMyPassword" OnClick="ForgotMyPassword_Click">Forgot my password</asp:LinkButton>
			</div>
		</asp:Panel>
		<asp:Panel runat="server" ID="ForgotMyPasswordPanel" Visible="false">	
			<div>
				Please enter your email address:
				<asp:TextBox ID="EmailAddress" runat="server" />
			</div>
			<div>
				<asp:LinkButton ID="LinkButton1" runat="server" OnClick="BackToLoginScreen_Click">Back</asp:LinkButton>
				<asp:LinkButton runat="server" ID="ForgotPasswordSend" OnClick="ForgotPasswordSend_Click">Send</asp:LinkButton>
			</div>
			<div>
				<asp:Literal ID="ServerMessage" runat="server"></asp:Literal>
			</div>
		</asp:Panel>
		<asp:Panel runat="server" ID="ResetPasswordPanel" Visible="false">
			<div>
				Enter the reset code:
				<asp:TextBox ID="ResetCode" runat="server"/>
			</div>
			<div>
				New Password:
				<asp:TextBox ID="NewPassword" runat="server"></asp:TextBox>
			</div>
			<div>
				<asp:LinkButton ID="LinkButton2" runat="server" OnClick="BackToLoginScreen_Click">Back</asp:LinkButton>
				<asp:LinkButton Text="Reset Password" runat="server" ID="ResetPassword" OnClick="ResetPassword_Click"/>
			</div>
			<div>
				<asp:Literal ID="ResetServerMessage" runat="server"></asp:Literal>
			</div>
		</asp:Panel>
	</ContentTemplate>

</asp:UpdatePanel>