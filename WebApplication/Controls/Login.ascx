<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="WebApplication.Controls.Login" %>

<asp:Panel runat="server" DefaultButton="Submit">

    <asp:Panel runat="server" Visible="false" ID="ErrorPanel" CssClass="alert alert-danger">        
        <i class="fa fa-exclamation-triangle" aria-hidden="true"></i> Invalid username or password. Please check your credentials and try again.
    </asp:Panel>

    <div>
        Username: <asp:TextBox runat="server" ID="Username" required/>
    </div>    
    <div>
        Password: <asp:TextBox runat="server" ID="Password" TextMode="Password" required/>
    </div>    
    <div>
        <asp:LinkButton Text="Login" runat="server" ID="Submit" OnClick="Submit_Click"/>
    </div>
</asp:Panel>

