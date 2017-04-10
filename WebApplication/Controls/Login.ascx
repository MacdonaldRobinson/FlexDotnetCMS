<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="WebApplication.Controls.Login" %>

<asp:Panel runat="server" DefaultButton="Submit">
    <div>
        Username: <asp:TextBox runat="server" ID="Username"/>
    </div>    
    <div>
        Password: <asp:TextBox runat="server" ID="Password" TextMode="Password"/>
    </div>    
    <div>
        <asp:LinkButton Text="Login" runat="server" ID="Submit" OnClick="Submit_Click"/>
    </div>
</asp:Panel>

