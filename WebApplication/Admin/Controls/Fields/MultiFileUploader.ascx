<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiFileUploader.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.MultiFileUploader" %>

<asp:Panel runat="server" ID="AdminPanel" CssClass="MultiFileUploader">

    <asp:UpdatePanel runat="server" ID="AdminUpdatePanel" class="AdminUpdatePanel">
        <ContentTemplate>
            <fieldset>
                <div>
                    <asp:LinkButton Text="Add New Item" runat="server" ID="AddItem" OnClick="AddItem_Click"/>
                </div>
                <asp:Panel runat="server" DefaultButton="SearchItems" id="SearchPanel" Visible="false">
                    <asp:TextBox runat="server" ID="SearchText" style="display:inline; width: 200px;" placeholder="Search ..." />
                    <asp:LinkButton ID="SearchItems" Text="Search" runat="server" OnClick="SearchItems_Click"/>
                </asp:Panel>
                <asp:Panel runat="server" ID="MultiItemUploaderPanel">
                    <asp:FileUpload ID="MultiFileUpload" runat="server" AllowMultiple="true" CssClass="MultiFileUpload" />
                    <div class="dvPreview" runat="server"></div>
                    <div class="buttons UploadFilesNowButtons" style="display:none;">
                        <asp:LinkButton runat="server" ID="UploadFilesNow" OnClick="UploadFilesNow_Click" CssClass="button">Upload Now</asp:LinkButton>
                        <div class="clear"></div>
                    </div>
                    <div class="UploadedItems" id="UploadedItems" runat="server">
                        <asp:ListView runat="server" ID="Values" ItemType="FrameworkLibrary.FieldAssociation">
                            <LayoutTemplate>
                                <fieldset>
                                    <h3>Items</h3>
                                    <ul class="sortable">
                                        <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                                    </ul>
                                    <div class="clear"></div>
                                </fieldset>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li class="item" data-id="<%# Item.ID %>">
                                    <div>
                                        <a href="<%# (Item.MediaDetail != null) ? WebApplication.BasePage.GetRedirectToMediaDetailUrl(Item.MediaDetail.MediaTypeID, Item.MediaDetail.MediaID, Item.MediaDetail.Media.ParentMediaID, Item.MediaDetail.HistoryVersionNumber)+"&masterFilePath=/Admin/Views/MasterPages/Popup.Master" : "#" %>" class="colorbox iframe EditImage" data-id="<%# Item.ID %>" data-OnColorboxClose="RefreshAdminUpdatePanel('<%= AdminUpdatePanel.ClientID %>')">Edit</a> |
                                        <a href="javascript:void(0)" class="DeleteImage" data-id="<%# Item.ID %>">Delete</a>
                                    </div>
                                    <a>
                                        <img src="<%# (Item.MediaDetail != null) ? URIHelper.ConvertToAbsUrl(Item.MediaDetail.RenderField("PathToFile", false)) : "#" %>" alt="<%# (Item.MediaDetail != null) ? Item.MediaDetail.RenderField("SectionTitle", false) : "" %>" />
                                        <div style="max-width:100px;">
                                            <%# (Item.MediaDetail != null) ? Item.MediaDetail.SectionTitle : "" %>
                                        </div>
                                    </a>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>
                        <Site:Pager runat="server" id="Pager" PagedControlID="Values" PageSize="12" />
                    </div>
                </asp:Panel>

                <asp:TextBox runat="server" CssClass="ItemsToDelete" ID="ItemsToDelete" Text="[]" Style="display: none;" />
                <asp:TextBox runat="server" CssClass="ReorderItems" ID="ReorderItems" Text="[]" Style="display: none;"/>

                <asp:GridView runat="server" ID="FieldItems" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="ItemList_PageIndexChanging" ItemType="FrameworkLibrary.FieldAssociation" Visible="false" Width="100%">
                    <Columns>
                        <asp:BoundField DataField="MediaDetail.SectionTitle" HeaderText="SectionTitle" SortExpression="MediaDetail.SectionTitle" />
                        <asp:BoundField DataField="MediaDetail.CreatedByUser.Username" HeaderText="CreatedByUser" SortExpression="MediaDetail.CreatedByUser.Username" />
                        <asp:BoundField DataField="MediaDetail.LastUpdatedByUser.Username" HeaderText="LastUpdatedByUser" SortExpression="MediaDetail.LastUpdatedByUser.Username" />
                        <asp:BoundField DataField="MediaDetail.DateLastModified" HeaderText="DateLastModified" SortExpression="MediaDetail.DateLastModified" />
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <div class="item">
                                    <a href="<%# (Item.MediaDetail != null) ? WebApplication.BasePage.GetRedirectToMediaDetailUrl(Item.MediaDetail.MediaTypeID, Item.MediaDetail.MediaID, Item.MediaDetail.Media.ParentMediaID, Item.MediaDetail.HistoryVersionNumber)+"&masterFilePath=/Admin/Views/MasterPages/Popup.Master" : "#" %>" class="colorbox iframe EditImage" data-id="<%# Item.ID %>" data-OnColorboxClose="RefreshAdminUpdatePanel('<%= AdminUpdatePanel.ClientID %>')">Edit</a> |
                                    <a href="javascript:void(0)" class="DeleteImage" data-id="<%# Item.ID %>">Delete</a>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </fieldset>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="UploadFilesNow" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>