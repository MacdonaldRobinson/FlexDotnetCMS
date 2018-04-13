<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Search.ascx.cs" Inherits="WebApplication.Controls.Search" %>
<asp:Panel runat="server" DefaultButton="SearchBtn">
    <asp:TextBox runat="server" ID="SearchTerms" ClientIDMode="Static" placeholder="Search" CssClass="search-bar"></asp:TextBox>
    <asp:LinkButton runat="server" OnClientClick="return SearchBtn_OnClientClick()" CssClass="search-button fa fa-search" ID="SearchBtn" />

    <script type="text/javascript">
        function SearchBtn_OnClientClick()
        {
            window.location.href = "/search/?q=" + $(".search-bar").val()
            return false;
        }
    </script>
	
</asp:Panel>