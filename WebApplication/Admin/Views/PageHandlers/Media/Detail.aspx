<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master"
    AutoEventWireup="True" CodeBehind="Detail.aspx.cs" Inherits="WebApplication.Admin.MediaArticle.Detail" ValidateRequest="false" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="MediaDetailPanel" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var baseUrl = '<%= URIHelper.BaseUrl %>';

        $(document).ready(function(){
            $(document).on('click', 'a.preview', function(event){
                return UpdatePreview(event);
            });

            UpdatePreviewUrl($("#VirtualPathHolder a").attr("href"));

            function UpdatePreview(event)
            {
                var href = $(event.target).attr("href");
                return UpdatePreviewUrl(href);
            }

            function UpdatePreviewUrl(url)
            {
                if(url != undefined && url.indexOf(baseUrl) != -1){
                    $("#PreviewPanel").attr("src", url);
                }
                return false;
            }
        });
    </script>

    <div class="split-pane fixed-left">
        <div class="split-pane-component" id="left-component">
            <link href="/Admin/Styles/mediaDetails.css" rel="stylesheet" />
            <link href="/Admin/Styles/admin.css" rel="stylesheet" />
            <script type="text/javascript">
                var selectedMediaId = <%=(SelectedMedia != null) ? SelectedMedia.ID : 0 %>;
                var selectedMediaDetailId = <%=(SelectedMediaDetail != null) ? SelectedMediaDetail.ID : 0 %>;
            </script>

            <asp:Panel ID="HistoryPanel" runat="server" Visible="false" CssClass="HistoryPanel">
                You are currently viewing history version (
                <asp:Literal ID="HistoryVersionNumber" runat="server"></asp:Literal>
                )
                <asp:LinkButton ID="ViewCurrentVersion" runat="server" OnClick="ViewCurrentVersion_OnClick">Click Here</asp:LinkButton>
                to view the current LIVE version<br />
            </asp:Panel>
            <h1>
                <asp:Literal ID="SectionTitle" runat="server"></asp:Literal></h1>

            <asp:PlaceHolder runat="server" ID="PanelsPlaceHolder" />

            <div class="clear"></div>

            <asp:Panel ID="SavePanel" runat="server" Visible="false" CssClass="SavePanel buttons">
                <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick" Text="Save" />
                <asp:LinkButton ID="SaveAsDraft" runat="server" OnClick="Save_OnClick" Text="Save As Draft" CommandArgument="SaveAsDraft" />
                <asp:LinkButton ID="LoadLatestDraft" runat="server" OnClick="LoadLatestDraft_OnClick" Text="Load Latest Draft" Visible="false" />
                <asp:LinkButton ID="SaveAndPublish" runat="server" OnClick="SaveAndPublish_OnClick" Text="Save And Publish" CommandArgument="SaveAndPublish" Visible="false" />
                <asp:LinkButton ID="PublishNow" runat="server" OnClick="PublishNow_OnClick" Visible="false" Text="Publish Now" />
                <asp:LinkButton runat="server" ID="PublishLive" OnClick="PublishLive_OnClick" Text="Publish LIVE" Visible="false" />
                <asp:HyperLink ID="EditMediaType" runat="server" Text="Edit Media Type" Visible="false" CssClass="colorbox iframe" />
            </asp:Panel>
        </div>
        <div class="split-pane-divider" id="my-divider"></div>
        <div class="split-pane-component" id="right-component">
            <iframe id="PreviewPanel" runat="server" clientidmode="Static"></iframe>
        </div>

        <asp:Panel runat="server" ID="RemovePreviewPanelScript" Visible="false">
            <script type="text/javascript">
                $(document).ready(function(){
                    $("#my-divider").hide();
                    $("#right-component").hide();
                    $("#left-component").width("100%");
                    $(".SavePanel").css("top", "0");
                });
            </script>
        </asp:Panel>
    </div>
</asp:Content>