<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsTab.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.CommentsTab" %>


<script>
    function UpdateCommentsTab()
    {
        RefreshUpdatePanel("CommentsListTabs");
    }
</script>

<asp:UpdatePanel runat="server" ID="CommentsListTabs" ClientIDMode="Static">
    <ContentTemplate>
        <div id="tabs" class="tabs">
            <ul>
                <li><a href="#ApprovedComments">Approved Comments(<asp:Literal ID="ApprovedCommentsCount" runat="server" />)</a></li>
                <li><a href="#RejectedComments">Rejected Comments(<asp:Literal ID="RejectedCommentsCount" runat="server" />)</a></li>
                <li><a href="#PendingComments">Pending Comments(<asp:Literal ID="PendingCommentsCount" runat="server" />)</a></li>
            </ul>
            <div id="ApprovedComments">
                <fieldset>
                    <div class="CommentContainer">
                        <Admin:MediaComments ID="ApprovedCommentsList" runat="server" Status="Approved" />
                    </div>
                </fieldset>
            </div>
            <div id="RejectedComments">
                <fieldset>
                    <div class="CommentContainer">
                        <Admin:MediaComments ID="RejectedCommentsList" runat="server" Status="Rejected" />
                    </div>
                </fieldset>
            </div>
            <div id="PendingComments">
                <fieldset>
                    <div class="CommentContainer">
                        <Admin:MediaComments ID="PendingCommentsList" runat="server" Status="Pending" />
                    </div>
                </fieldset>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
