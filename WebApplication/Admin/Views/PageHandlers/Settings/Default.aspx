<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Views/MasterPages/Admin.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Admin.Views.PageHandlers.Settings.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h1>Settings</h1>
    <fieldset>
        <div id="tabs" class="tabs">
            <ul>
                <li><a href="#GlobalCodeInHead">Global Code In Head</a></li>
                <li><a href="#GlobalCodeInBody">Global Code In Body</a></li>
                <li><a href="#Other">Other</a></li>
            </ul>
            <div id="GlobalCodeInHead">
                <div>
                    <asp:TextBox runat="server" ID="GlobalCodeInHead" TextMode="MultiLine" CssClass="AceEditor" Height="600px"></asp:TextBox>
                </div>
            </div>
            <div id="GlobalCodeInBody">
                <div>
                    <asp:TextBox runat="server" ID="GlobalCodeInBody" TextMode="MultiLine" CssClass="AceEditor" Height="600px"></asp:TextBox>
                </div>
            </div>
            <div id="Other">
                <fieldset>
                    <div>
                        <label for="<%= DefaultLanguageSelector.ClientID %>">
                            Default Language:</label>
                        <div>
                            <asp:DropDownList runat="server" ID="DefaultLanguageSelector">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div>
                        <label for="<%= DefaultMasterPageSelector.ClientID %>">
                            Default Template:</label>
                        <div>
                            <asp:DropDownList runat="server" ID="DefaultMasterPageSelector">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div>
                        <label for="<%= MaxUploadFileSizePerFileInMB.ClientID %>">
                            Max Upload File Size Per File( in MB)</label>
                        <div>
                            <asp:TextBox runat="server" ID="MaxUploadFileSizePerFileInMB" />
                        </div>
                    </div>
                    <div>
                        <label for="<%= MaxRequestLengthInMB.ClientID %>">
                            Max Request Length( in MB) must be more then Max Upload File Size Per File</label>
                        <div>
                            <asp:TextBox runat="server" ID="MaxRequestLengthInMB" />
                        </div>
                    </div>
                    <div>
                        <label for="<%= EnableGlossaryTerms.ClientID %>">
                            <asp:CheckBox ID="EnableGlossaryTerms" runat="server" />
                            Enable Glossary Terms</label>
                    </div>
                    <div>
                        <label for="<%= OutputCacheDurationInSeconds.ClientID %>">
                            Output Cache Duration ( in Seconds)</label>
                        <div>
                            <asp:TextBox runat="server" ID="OutputCacheDurationInSeconds" />
                        </div>
                    </div>
                    <div>
                        <label for="<%= ShoppingCartTax.ClientID %>">
                            Tax</label>
                        <div>
                            <asp:TextBox runat="server" ID="ShoppingCartTax" />
                        </div>
                    </div>
                    <div>
                        <label for="<%= SiteOnlineAtDateTime.ClientID %>">
                            Site Online At DateTime</label>
                        <div>
                            <asp:TextBox runat="server" ID="SiteOnlineAtDateTime" CssClass="datetimepicker" />
                        </div>
                    </div>
                    <div>
                        <label for="<%= SiteOfflineAtDateTime.ClientID %>">
                            Site Offline At DateTime</label>
                        <div>
                            <asp:TextBox runat="server" ID="SiteOfflineAtDateTime" CssClass="datetimepicker" />
                        </div>
                    </div>
                    <div>
                        <label for="<%= SiteOfflineUrl.ClientID %>">
                            Site Offline Url</label>
                        <div>
                            <asp:TextBox runat="server" ID="SiteOfflineUrl"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        <label for="<%= PageNotFoundUrl.ClientID %>">
                            Page Not Found Url</label>
                        <div>
                            <asp:TextBox runat="server" ID="PageNotFoundUrl"></asp:TextBox>
                        </div>
                    </div>
                    <div>
                        Current time on the server is: <%= DateTime.Now.ToString() %>
                    </div>
                </fieldset>
            </div>
        </div>
        <asp:UpdatePanel runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div class="buttons">
                    <asp:LinkButton ID="Save" runat="server" OnClick="Save_OnClick" CssClass="SavePageButton">Save</asp:LinkButton>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </fieldset>
</asp:Content>
