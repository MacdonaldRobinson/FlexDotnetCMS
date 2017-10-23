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
            <h3>Delete All History Versions</h3>
            This tool will delete all history versions currently saved in the database<br />
            <div class="buttons">
                <asp:LinkButton ID="DeleteAllHistory" runat="server" Text="Delete All History" OnClick="DeleteAllHistory_Click" />
                <asp:CheckBox ID="DeleteSavedDrafts" runat="server" />
                Delete any saved Drafts as well
                <div class="clear"></div>
            </div>
        </div>
    </fieldset>

    <fieldset>
        <legend>Deployment Tools</legend>
        <label><strong>Select the remote environment:</strong></label>

        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <asp:DropDownList runat="server" ID="DeployToEnvironment">
                </asp:DropDownList>

                <asp:LinkButton Text="Check" runat="server" ID="Deploy" OnClick="Deploy_Click" />  
        
                <asp:ListView runat="server" ItemType="" ID="DeployMessages">
                    <LayoutTemplate>
                        <ul id="DeployMessages">
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                        </ul>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <li>                            
                            <strong><%# Eval("Key") %></strong>

                        <asp:ListView runat="server" ItemType="FrameworkLibrary.Return" ID="Messages" DataSource='<%# Eval("Value") %>'>
                            <LayoutTemplate>
                                <ul class="SubMessage">
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                                </ul>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <li>                                    
                                    <%# Eval("Error.Exception.Message") %>
                                </li>
                            </ItemTemplate>
                        </asp:ListView>


                        </li>
                    </ItemTemplate>
                </asp:ListView>
            </ContentTemplate>
        </asp:UpdatePanel>

        <!--<p>
            <h3>Backup Database</h3>
            This tool will create a backup of the current database an place it under (
            <asp:Literal ID="DBBackupPath" runat="server"></asp:Literal>
            )<br />
            <div class="buttons">
                <asp:LinkButton ID="BackupNow" runat="server" Text="Backup Now" OnClick="BackupNow_OnClick" />
            </div>
        </p>-->
    </fieldset>

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
                    </Columns>
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>