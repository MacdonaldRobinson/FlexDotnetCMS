<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AdvancedSettingsTab.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.AdvancedSettingsTab" %>


<fieldset>
    <div>
        <p>
            <label for="<%= Handler.ClientID %>">
                Handler</label>
            <Admin:FileSelector ID="Handler" runat="server" DirPath="~/Views/PageHandlers" />
        </p>
    </div>
    <div id="MasterPageSelectorHolder">
        <label for="<%= MasterPageSelector.ClientID %>">
            Site Template:
        </label>
        <Site:MasterPageSelector ID="MasterPageSelector" runat="server" />
    </div>
    <div>
        <p>
            <asp:CheckBox ID="EnableCaching" runat="server" /><label for="<%= EnableCaching.ClientID %>">
                Enable Caching</label>
        </p>
    </div>
    <div>
        <p>
            <label class="exception" for="<%= MediaDetailID.ClientID %>">
                Media Detail ID:</label>
            <asp:Literal ID="MediaDetailID" runat="server"></asp:Literal>
        </p>
    </div>
    <div>
        <p>
            <label class="exception" for="<%= MediaID.ClientID %>">
                Media  ID:</label>
            <asp:Literal ID="MediaID" runat="server"></asp:Literal>
        </p>
    </div>
    <div>
        <p>
            <label class="exception" for="<%= MediaTypeID.ClientID %>">
                Media Type ID:</label>
            <asp:Literal ID="MediaTypeID" runat="server"></asp:Literal>
        </p>
    </div>

    <div>
        <p>
            <label class="exception" for="<%= MediaType.ClientID %>">
                Media Type:</label>
            <asp:Literal ID="MediaType" runat="server"></asp:Literal>
        </p>
    </div>

    <div>
        <p>
            <label class="exception" for="<%= MediaTypes.ClientID %>">
                You can change this item to the following Media Types:</label><br />
            <asp:DropDownList runat="server" ID="MediaTypes">
            </asp:DropDownList>
            <asp:LinkButton Text="Change" ID="MediaTypeChange" runat="server" OnClick="MediaTypeChange_Click" />
        </p>
    </div>

    <div>
        <p>
            <label class="exception" for="<%= OrderIndex.ClientID %>">
                Order Index:</label>
            <asp:Literal ID="OrderIndex" runat="server"></asp:Literal>
        </p>
    </div>
    <div>
        <p>
            <label class="exception" for="<%= CreatedByUser.ClientID %>">
                Created By User:</label>
            <asp:Literal ID="CreatedByUser" runat="server"></asp:Literal>
        </p>
    </div>
    <div>
        <p>
            <label class="exception" for="<%= LastModifiedByUser.ClientID %>">
                Last Modified By User:</label>
            <asp:Literal ID="LastModifiedByUser" runat="server"></asp:Literal>
        </p>
    </div>
</fieldset>