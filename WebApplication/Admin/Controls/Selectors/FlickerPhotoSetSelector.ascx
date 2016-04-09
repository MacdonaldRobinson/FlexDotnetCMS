<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlickerPhotoSetSelector.ascx.cs" Inherits="WebApplication.Admin.Controls.Selectors.FlickerPhotoSetSelector" %>
<%--<telerik:RadComboBox ID="FlickerPhotoSets" runat="server" OnItemDataBound="FlickerPhotoSets_OnItemDataBound" AppendDataBoundItems="true">
    <Items>
        <telerik:RadComboBoxItem Text=" --- Select a PhotoSet ---" Value="" runat="server" />
    </Items>
</telerik:RadComboBox>--%>

<asp:DropDownList runat="server" AppendDataBoundItems="true" ID="FlickerPhotoSets" OnItemDataBound="FlickerPhotoSets_OnItemDataBound" AutoPostBack="true">
    <asp:ListItem Text="--- Select a PhotoSet ---" Value="" />
</asp:DropDownList>