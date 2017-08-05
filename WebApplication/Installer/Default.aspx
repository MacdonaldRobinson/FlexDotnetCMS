<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WebApplication.Installer.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        ul {
            list-style-type: none;
        }

        html, body, form, div {
            margin: 0;
            padding: 0;
        }

        .wrapper {
            margin-left: 20px;
        }

        #progressWrapper
        {
            position: fixed;
            text-align: center;
            height: 100%;
            width: 100%;
            top: 0;
            right: 0;
            left: 0;
            z-index: 9999999;
            background-color: #000000;
            opacity: 0.7;
            color: #fff;
        }

        #progressWrapper #loading{
            position: relative;
            top: 50%;
            transform: translateY(-50%);
        }

        label {
            font-weight: bold;
        }

        select {
            margin-bottom: 10px;
        }

        .step{
            border-bottom: 1px solid #000000;
            padding-bottom:20px;
        }

            .step > div {
                margin-left: 40px;
            }

                .step > div > div {
                    margin-bottom: 10px;
                }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <asp:UpdateProgress id="updateProgress" runat="server">
            <ProgressTemplate>
                <div id="progressWrapper">
                    <div id="loading">
                        <img src="Images/loading-gear.gif" />
                    </div>
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>

        <asp:UpdatePanel runat="server">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="ConnectionStringKeyName"  EventName="SelectedIndexChanged"/>
            </Triggers>
            <ContentTemplate>
                <div class="wrapper">

                    <asp:Panel runat="server" ID="InstallerDisabledPanel" Visible="false">
                        <h3>Installer has been disabled</h3>
                    </asp:Panel>
                    <asp:Panel runat="server" ID="InstallerPanel" Visible="false">
                        <h1>Installer</h1>
                        <div>
                            <i>NOTE: Please make sure that your web.config file has write permissions, after this process is complete you can remove write permissions</i>
                        </div>
                        <div class="step">
                            <h3>Step 1: Select your server environment</h3>
                            <asp:DropDownList runat="server" ID="ConnectionStringKeyName" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="ConnectionStringKeyName_SelectedIndexChanged">
                                <asp:ListItem Text="-- Select Server Type --" Value="" />
                                <asp:ListItem Text="Dev" Value="Dev" />
                                <asp:ListItem Text="Stage" Value="Stage" />
                                <asp:ListItem Text="Prod" Value="Prod" />
                            </asp:DropDownList>
                        </div>
                        <div class="step">
                            <h3>Step 2: Connection String Settings</h3>
                            <div>
                                <div>
                                    <label>Data Source ( Host or IP )</label><br />
                                    <asp:TextBox runat="server" ID="DataSource" />
                                </div>
                                <div>
                                    <label>Attach Db Filename</label><br />
                                    <asp:TextBox runat="server" ID="AttachDBFilename" />
                                </div>
                                <div>
                                    <label>DatabaseName</label><br />
                                    <asp:TextBox runat="server" ID="DatabaseName" />
                                </div>
                                <div>
                                    <label>Username</label><br />
                                    <asp:TextBox runat="server" ID="UserID" />
                                </div>
                                <div>
                                    <label>Password</label><br />
                                    <asp:TextBox runat="server" ID="Password" />
                                </div>
                                <div>
                                    <label><asp:CheckBox ID="IntegratedSecurity" runat="server" />Integrated Security</label>
                                </div>
                                <div>
                                    <asp:LinkButton Text="Save Login" runat="server" ID="SaveLogin" OnClick="SaveLogin_Click" />
                                </div>
                            </div>
                        </div>
                        <div class="step">
                            <h3>Step 3: Attempt connecting using the connection string settings provided</h3>
                            <asp:LinkButton Text="Attempt Connecting" runat="server" ID="Attempt" OnClick="Attempt_Click" />
                        </div>
                        <asp:PlaceHolder runat="server" ID="ExecutePanel" Visible="false">
                            <div class="step">
                                <h3>Step 4: Select SQL file to execute</h3>
                                <div>
                                    <div>
                                        <i>Note: Based on the script that is being executed, all data in the existing database will be lost, it is best that you backup the database prior to executing any scripts</i>
                                    </div>
                                    <asp:DropDownList runat="server" ID="SqlFiles">
                                    </asp:DropDownList><br />
                                    <asp:LinkButton Text="Execute SQL File" runat="server" ID="ExecuteSQLFile" OnClick="ExecuteSQL_Click" OnClientClick="return confirm('Are you sure you want to execute the selected sql script against the database? All existing data in the database will be lost.')" />
                                </div>
                            </div>
                            <div class="step">
                                <h3>Step 5: CMS Admin Login Credentials</h3>
                                <div>
                                    <div>
                                        <label>Username: admin</label><br /><br />
                                    </div>
                                    <div>
                                        <label>Email Address:</label><br />
                                        <asp:TextBox runat="server" ID="CMSEmailAddress" />
                                    </div>
                                    <div>
                                        <label>Password:</label><br />
                                        <asp:TextBox runat="server" ID="CMSPassword" />
                                    </div>
                                    <div>
                                        <label>First Name:</label><br />
                                        <asp:TextBox runat="server" ID="FirstName" />
                                    </div>
                                    <div>
                                        <label>Last Name:</label><br />
                                        <asp:TextBox runat="server" ID="LastName" />
                                    </div>
                                    <div>
                                        <asp:LinkButton Text="Update" runat="server" ID="UpdateCMSAdminLogin" OnClick="UpdateCMSAdminLogin_Click" />
                                    </div>
                                </div>
                            </div>
                            <asp:PlaceHolder runat="server" ID="DisableInstallerPanel" Visible="false">
                                <div class="step">
                                    <h3>Step 6: Disable Installer</h3>
                                    <asp:LinkButton Text="Disable Installer" runat="server" ID="DisableInstaller" OnClick="DisableInstaller_Click" />
                                </div>
                            </asp:PlaceHolder>
                        </asp:PlaceHolder>
                        <p>
                            <fieldset>
                                <legend>Messages from the server</legend>
                                <asp:Literal runat="server" ID="Messages" />
                            </fieldset>
                        </p>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>