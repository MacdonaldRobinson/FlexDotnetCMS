<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Welcome to your site editor: <span>Lets get started</span></h1>
    <div class="box">
        <div class="leftArrow">
            <h2>Edit a page in your site</h2>
            <p>On the left you will see our navigation tree. On that tree are the labels of all the pages in your site. Clicking on a page will open up the editor for that page. If you see a + beside a page it means that there are more pages inside and clicking on it will expand that label.</p>
        </div>
        <h2>Create a new page</h2>
        <p>On the left navigation simply right-click on the section you would like to create a page within. Then from the menu that appears select “create child” and follow the instructions that appear. If you want to move you pages around you can move up or down using the same menu.</p>
        <hr />
        <div class="fixedMinSize">
            <div class="floatLeft recentEdits">
                <h2>Recent Edits</h2>

                <asp:GridView runat="server" ID="RecentEditsList" AutoGenerateColumns="false" CssClass="DataTable" OnDataBound="RecentEditsList_DataBound">
                    <Columns>
                        <asp:BoundField DataField="LinkTitle" HeaderText="Title"/>
                        <asp:BoundField DataField="DateLastModified" HeaderText="Date Last Modified"/>
                       <asp:TemplateField HeaderText="Usename">
                            <ItemTemplate>
                                <%# ((IMediaDetail)Container.DataItem).LastUpdatedByUser.UserName %>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <a href="<%# GetAdminUrl( (IMediaDetail)Container.DataItem ) %>">Edit This Page</a>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="floatLeft feeds">
            </div>
            <div class="clear"></div>
        </div>
    </div>
</asp:Content>