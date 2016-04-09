<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactUs.ascx.cs" Inherits="WebApplication.Controls.EmailTemplates.ContactUs" %>

Name:
<asp:Literal ID="SenderName" runat="server"></asp:Literal><br />
Email:
<asp:Literal ID="SenderEmailAddress" runat="server"></asp:Literal><br />
Message:
<asp:Literal ID="Message" runat="server"></asp:Literal>