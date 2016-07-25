<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommentsTab.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.CommentsTab" %>

<asp:UpdatePanel runat="server" ID="CommentsListTabs">
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
                        <Site:CommentsList ID="ApprovedCommentsList" runat="server" />
                    </div>
                </fieldset>
            </div>
            <div id="RejectedComments">
                <fieldset>
                    <div class="CommentContainer">
                        <Site:CommentsList ID="RejectedCommentsList" runat="server" />
                    </div>
                </fieldset>
            </div>
            <div id="PendingComments">
                <fieldset>
                    <div class="CommentContainer">
                        <Site:CommentsList ID="PendingCommentsList" runat="server" />
                    </div>
                </fieldset>
            </div>
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
