<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginSuccess.ascx.cs" Inherits="WebApplication.Controls.LoginSuccess" %>
Welcome&nbsp;
<asp:LoginName ID="LoginName" runat="Server" />
&nbsp;
<br />
<br />
<asp:LoginStatus ID="LoginStatus" CssClass="btn btn-primary customBtn" runat="Server" OnLoggedOut="LoginStatus_OnLoggedOut" />