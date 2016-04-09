<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateBreadCrumb.ascx.cs" Inherits="WebApplication.Controls.GenerateBreadCrumb" %>
<div id="BreadCrumb">
    <Site:generatenav id="GenerateNav" runat="server" renderrootmedia="false" renderhiddenmediatypes="true" dividerstring="" rootulclasses="breadcrumb" isbreadcrumbmenu="True" displayprotectedsections="true" />
</div>