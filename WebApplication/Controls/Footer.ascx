<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Footer.ascx.cs" Inherits="WebApplication.Controls.Footer" %>
<%@ Import Namespace="WebApplication" %>

<div id="footer">

    <div id="footerShare">
    </div>

    <div id="footerMenu" class="floatRight">
        <Site:GenerateFooterNav runat="server" />
    </div>
</div>