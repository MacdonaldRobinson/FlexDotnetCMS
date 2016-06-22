<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiFileUploader.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.MultiFileUploader" %>

<asp:Panel runat="server" ID="AdminPanel" CssClass="MultiFileUploader">

    <asp:UpdatePanel runat="server" ID="AdminUpdatePanel" class="AdminUpdatePanel">
        <ContentTemplate>
            <fieldset>
                <div>
                    <asp:LinkButton Text="Add New Item" runat="server" ID="AddFieldFile" OnClick="AddFieldFile_Click"/>
                </div>
                <asp:FileUpload ID="MultiFileUpload" runat="server" AllowMultiple="true" CssClass="MultiFileUpload" />
                <div class="dvPreview" runat="server"></div>
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
                            <li class="item" data-fieldfileid="<%# Item.ID %>">
                                <div>
                                    <a href="<%# (Item.MediaDetail != null) ? WebApplication.BasePage.GetRedirectToMediaDetailUrl(Item.MediaDetail.MediaTypeID, Item.MediaDetail.MediaID, Item.MediaDetail.ParentMediaID, Item.MediaDetail.HistoryVersionNumber) + "&masterFilePath=~/Admin/Views/MasterPages/Popup.Master" : "#" %>" target="_blank" class="colorbox iframe EditImage" data-id="<%# Item.ID %>" data-OnColorboxClose="RefreshAdminUpdatePanel('<%= AdminUpdatePanel.ClientID %>')">Edit</a> |
                                    <a href="javascript:void(0)" class="DeleteImage" data-id="<%# Item.ID %>">Delete</a>
                                </div>
                                <a>
                                    <img src="<%# URIHelper.ConvertToAbsUrl(Item.MediaDetail.PathToFile) %>" alt="<%# Item.MediaDetail.SectionTitle %>" />
                                    <div style="max-width:100px;">
                                        <%# Item.MediaDetail.SectionTitle %>
                                    </div>
                                </a>
                            </li>
                        </ItemTemplate>
                    </asp:ListView>
                    <Site:Pager runat="server" PagedControlID="Values" PageSize="12" />
                    <asp:TextBox runat="server" CssClass="FilesToDelete" ID="FilesToDelete" Text="[]" Style="display: none;" />
                    <asp:TextBox runat="server" CssClass="ReorderFiles" ID="ReorderFiles" Text="[]" Style="display: none;"/>
                </div>
            </fieldset>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>