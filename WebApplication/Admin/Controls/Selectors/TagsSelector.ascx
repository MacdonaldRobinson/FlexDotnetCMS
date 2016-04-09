<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagsSelector.ascx.cs"
    Inherits="WebApplication.Admin.Controls.TagsSelector" %>
<script type="text/javascript">
    jQuery(document).ready(function () {
        var data = {TagsJSON};
        var preFillData = {CurrentItemTagsJSON};

        jQuery("#<%= TagSelectorPanel.ClientID %> .autoSuggestTags").autoSuggest(data, { selectedItemProp: "name", searchObjProps: "name", selectedValuesProp: "id", preFill: preFillData });

        jQuery("#<%= TagSelectorPanel.ClientID %> .autoSuggestTags").blur(function(){
            jQuery("#<%= TagValues.ClientID %>").attr('value', jQuery("#<%= TagSelectorPanel.ClientID %> .as-original .as-values").val());
        });
    });
</script>
<asp:Panel ID="TagSelectorPanel" runat="server">
    <asp:TextBox ID="Tags" runat="server" CssClass="autoSuggestTags"></asp:TextBox>
    <asp:HiddenField ID="TagValues" runat="server" />
</asp:Panel>