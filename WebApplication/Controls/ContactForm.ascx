<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactForm.ascx.cs" Inherits="WebApplication.Controls.ContactForm" %>
<asp:Panel ID="FormPanel" runat="server">

    <div class="form">
        <h4>
            <asp:Literal ID="TitleLiteral" runat="server" Text="nothing"></asp:Literal></h4>

        <asp:ValidationSummary ID="ValidationSummary" ClientIDMode="Static" HeaderText="There were some errors" runat="server" ValidationGroup="SubmitForm" />

        <div>
            <label for="Name">Name:</label><asp:TextBox ID="Name" ClientIDMode="Static" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredField" runat="server" ControlToValidate="Name" ValidationGroup="SubmitForm" Display="None" ErrorMessage="Name Is Required"></asp:RequiredFieldValidator>
        </div>
        <div>
            <label for="Email">Email Address:</label><asp:TextBox ID="Email" ClientIDMode="Static" runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="Email" ValidationGroup="SubmitForm" Display="None" ErrorMessage="Email Is Required"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator
                ID="RegularExpressionValidator1"
                runat="server"
                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                ControlToValidate="Email"
                ErrorMessage="Email Address is invalid" Display="None">
            </asp:RegularExpressionValidator>
        </div>
        <div>
            <label for="Message">Message:</label><asp:TextBox ID="Message" Rows="10" ClientIDMode="Static" runat="server" TextMode="MultiLine"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="Message" ValidationGroup="SubmitForm" Display="None" ErrorMessage="Message Is Required"></asp:RequiredFieldValidator>
        </div>
        <div>
            <asp:Button ID="Submit" runat="server" OnClick="Submit_OnClick" Text="Submit" ValidationGroup="SubmitForm" />
        </div>
    </div>
</asp:Panel>