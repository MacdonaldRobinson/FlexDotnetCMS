<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.AdminTools.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Admin Tools</h2>
    <br />
    <fieldset>
        <legend>Caching and History Tools</legend>
        <div>
            <h3>Clear All Cache</h3>
            This tool will clear all the data cache stored by the application<br />
            <div class="buttons">
                <asp:LinkButton ID="ClearAllCache" runat="server" Text="Clear All Cache Now" OnClick="ClearAllCache_OnClick" />
                <div class="clear"></div>
            </div>
        </div>
        <div>
            <h3>Clear All Cache & Delete All History Versions</h3>
            This tool will delete all history versions currently saved in the database and will clear all the cache at the same time<br />
            <div class="buttons">
                <asp:LinkButton ID="DeleteAllHistoryAndClearAllCache" runat="server" Text="Delete All History And Clear All Cache" OnClick="DeleteAllHistoryAndClearAllCache_Click" />
                <asp:CheckBox ID="DeleteSavedDrafts" runat="server" />
                Delete any saved Drafts as well
                <div class="clear"></div>
            </div>
        </div>
    </fieldset>

    <%--    <fieldset>
        <legend>Database Tools</legend>
        <p>
            <h3>Backup Database</h3>
            This tool will create a backup of the current database an place it under (
            <asp:Literal ID="DBBackupPath" runat="server"></asp:Literal>
            )<br />
            <div class="buttons">
                <asp:LinkButton ID="BackupNow" runat="server" Text="Backup Now" OnClick="BackupNow_OnClick" />
            </div>
        </p>
    </fieldset>--%>

    <fieldset>
        <legend>Email Log</legend>
        <p>All emails sent by the system are shown here</p>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <asp:GridView runat="server" ID="EmailLog" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="EmailLog_PageIndexChanging" OnSorting="EmailLog_Sorting">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                        <asp:BoundField DataField="SenderName" HeaderText="SenderName" SortExpression="SenderName" />
                        <asp:BoundField DataField="SenderEmailAddress" HeaderText="SenderEmailAddress" SortExpression="SenderEmailAddress" />
                        <asp:BoundField DataField="ToEmailAddresses" HeaderText="ToEmailAddresses" SortExpression="ToEmailAddresses" />
                        <asp:BoundField DataField="FromEmailAddress" HeaderText="FromEmailAddress" SortExpression="FromEmailAddress" />
                        <asp:BoundField DataField="Subject" HeaderText="Subject" SortExpression="Subject" />
                        <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message" />
                        <asp:BoundField DataField="RequestUrl" HeaderText="RequestUrl" SortExpression="RequestUrl" />
                        <asp:BoundField DataField="ServerMessage" HeaderText="ServerMessage" SortExpression="ServerMessage" />
                        <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                        <asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />
                    </Columns>
                </asp:GridView>

            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>

    <fieldset>
        <legend>Error Log</legend>
        <p>Recent 10 exceptions are shown here, inorder to view a full list and more details <a href="/elmah.axd" target="_blank">click here</a></p>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

                <asp:GridView runat="server" ID="ErrorLog" AutoGenerateColumns="false" AllowPaging="true" OnPageIndexChanging="ErrorLog_PageIndexChanging" OnSorting="ErrorLog_Sorting">
                    <Columns>
                        <asp:BoundField DataField="HostName" HeaderText="HostName" SortExpression="HostName" />
                        <asp:BoundField DataField="Type" HeaderText="Type" SortExpression="Type" />
                        <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message" />
                    </Columns>
                </asp:GridView>

                <%--<telerik:RadGrid ID="ErrorLog" runat="server" OnNeedDataSource="ErrorLog_NeedDataSource" AutoGenerateColumns="True" GridLines="None"
                    AllowPaging="true" AllowSorting="true" PageSize="10"
                    AllowCustomPaging="False" AllowFilteringByColumn="false">
                    <ClientSettings ReorderColumnsOnClient="True" EnablePostBackOnRowClick="false">
                        <Selecting AllowRowSelect="true" />
                    </ClientSettings>
                    <GroupingSettings CaseSensitive="False"></GroupingSettings>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" />

                    <MasterTableView AutoGenerateColumns="False" DataKeyNames="HostName" ClientDataKeyNames="HostName">
                        <RowIndicatorColumn Visible="False">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn Visible="False" Resizable="False">
                            <HeaderStyle Width="20px"></HeaderStyle>
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridBoundColumn DataField="HostName" HeaderText="HostName" SortExpression="HostName" UniqueName="HostName"
                                AutoPostBackOnFilter="true" FilterControlWidth="20px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Type" HeaderText="Type" SortExpression="Type"
                                UniqueName="Type" AutoPostBackOnFilter="true" FilterControlWidth="75px">
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="Message" HeaderText="Message" SortExpression="Message"
                                UniqueName="Message" AutoPostBackOnFilter="true" FilterControlWidth="75px">
                            </telerik:GridBoundColumn>
                        </Columns>
                        <EditFormSettings>
                            <PopUpSettings ScrollBars="None"></PopUpSettings>
                        </EditFormSettings>
                    </MasterTableView>
                </telerik:RadGrid>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>