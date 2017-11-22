<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Fields.FileSelector" %>

<asp:Panel runat="server" ID="AdminPanel">
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).on("keyup", "#<%=SelectedFile.ClientID%>", function () {
                var src = $(this).val().replace("~/", "/");
                $("#<%=SelectedImage.ClientID%>").attr("src", src);
            });
        });
    </script>
    <style>
        .SelectedImage {
            background-color: lightgray;
        }
    </style>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <a class="colorbox iframe" href="/Scripts/tinyfilemanager.net/dialog.aspx?profile=notinymce&targetId=<%= SelectedFile.ClientID %>&currpath=<%= DirPath %>">Select File</a>
                <asp:TextBox ID="SelectedFile" runat="server" CssClass="Control"></asp:TextBox>
            </div>
            <div>
                <asp:Image ID="SelectedImage" runat="server" CssClass="size-200 SelectedImage"/>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>