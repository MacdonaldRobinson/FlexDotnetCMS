<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FileSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.Fields.FileSelector" %>

<asp:Panel runat="server" ID="AdminPanel">
    <script type="text/javascript">
        $(document).ready(function () {
            $(document).on("keyup", "#<%=SelectedFile.ClientID%>", function () {
                var src = $(this).val().replace("~/", "/");
                $("#<%=SelectedImage.ClientID%>").attr("src", src + "?height=200&mode=min");
            });

            $("#<%=SelectedImage.ClientID%>").load(function () {
                var src = $(this).attr("src");

                if (src.indexOf("?height") == -1)
                {
                    $(this).attr("src", src + "?height=200&mode=min");
                }
            });
        });
    </script>

    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <a class="colorbox iframe" href="/Scripts/tinyfilemanager.net/dialog.aspx?profile=notinymce&targetId=<%= SelectedFile.ClientID %>&currpath=<%= DirPath %>">Select File</a>
                <asp:TextBox ID="SelectedFile" runat="server" CssClass="Control"></asp:TextBox>
            </div>
            <div>
                <asp:Image ID="SelectedImage" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>