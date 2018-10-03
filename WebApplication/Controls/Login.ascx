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
				<label>Email</label><asp:TextBox runat="server" ID="Username" required CssClass="" />
			</div>    
			<div class="password__con">
				<label>Password </label>
				<span class="password-toggle">
					<span class="password-show">Show <i class="fa fa-eye"></i></span>
					
					<span class="password-hide">Hide <i class="fa fa-eye-slash"></i></span>
				</span>
				<asp:TextBox runat="server" ID="Password" TextMode="Password" required/>
				
			</div>    
			<div>
				<asp:LinkButton Text="Sign in" runat="server" ID="LoginButton" OnClick="LoginButton_Click" CssClass="button button--bluedark" />
			</div>
			<div>				
				<asp:LinkButton runat="server" ID="ForgotMyPassword" OnClick="ForgotMyPassword_Click" CssClass="link--grey">Forgot my password</asp:LinkButton>
			</div>
		</asp:Panel>
		<asp:Panel runat="server" ID="ForgotMyPasswordPanel" Visible="false">	
			<div>
				Please enter your email address:
				<asp:TextBox ID="EmailAddress" runat="server" />
			</div>
			<div>
				<asp:LinkButton ID="LinkButton1" runat="server" OnClick="BackToLoginScreen_Click" CssClass="button button--bluedark ghost">Back</asp:LinkButton>
				<asp:LinkButton runat="server" ID="ForgotPasswordSend" OnClick="ForgotPasswordSend_Click" CssClass="button button--bluedark">Send</asp:LinkButton>
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