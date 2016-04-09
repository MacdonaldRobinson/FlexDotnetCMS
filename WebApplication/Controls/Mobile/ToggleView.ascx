<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ToggleView.ascx.cs" Inherits="WebApplication.Controls.Mobile.ToggleView" %>

<script type="text/javascript">
    $(".toggleView").live("click", function () {
        var js = $(".ui-radio-off").parents(".ui-radio").find("input").attr("onclick");
        if (js != 'undefined') {
            eval(js);
        }
    });
</script>
<asp:RadioButtonList ID="ToggleViewOptions" runat="server" RepeatLayout="UnorderedList" AutoPostBack="true" OnSelectedIndexChanged="ToggleViewOptions_OnSelectedIndexChanged" CssClass="toggleView">
    <asp:ListItem Text="HTML" Value="HTML"></asp:ListItem>
    <asp:ListItem Text="Mobile" Value="Mobile"></asp:ListItem>
</asp:RadioButtonList>