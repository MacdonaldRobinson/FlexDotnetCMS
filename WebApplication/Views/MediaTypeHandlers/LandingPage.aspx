<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LandingPage.aspx.cs" Inherits="WebApplication.Views.MediaTypeHandlers.LandingPage" %>

<%@ Import Namespace="WebApplication" %>
<!DOCTYPE html>
<html lang="en-US" class="no-js">
<head runat="server">
    <Site:CommonIncludes ID="CommonIncludes" runat="server" />
    <asp:PlaceHolder ID="CssIncludes" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="JsIncludes" runat="server"></asp:PlaceHolder>
    <asp:PlaceHolder ID="MetaIncludes" runat="server"></asp:PlaceHolder>
</head>
<body>
    <%= FrontEndBasePage.GetSettings().GlobalCodeInBody %>

    <form id="form2" runat="server">
        <asp:ScriptManager runat="server" />

        <div class="landing-page">
            <asp:PlaceHolder runat="server" ID="DynamicContent" />
        </div>
    </form>
</body>
</html>