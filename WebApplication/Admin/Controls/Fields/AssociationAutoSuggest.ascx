<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociationAutoSuggest.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.AssociationAutoSuggest" %>

<asp:Panel ID="AdminPanel" runat="server">
    <script type="text/javascript">
        jQuery(document).ready(function () {

            jQuery("#<%= TagSelectorPanel.ClientID %> .autoSuggestTags").autoSuggest(<%= this.GetDataJson() %>, {resultsHighlight: true, startText:'Start Typing', selectedItemProp: "name", searchObjProps: "name", selectedValuesProp: "value", preFill: <%= GetCurrentAutoSuggestItems() %> });

            $(document).click(function () {
                var values = jQuery("#<%= TagSelectorPanel.ClientID %> .as-original .as-values").val();
                var valSplit = values.split(",");

                var ids = new Array();
                for (var i=0; i< valSplit.length; i++) {
                    if(valSplit[i] !="")
                    {
                        ids.push(Number(valSplit[i]));
                    }
                }
                
                values = JSON.stringify(ids);
                jQuery("#<%= Values.ClientID %>").attr('value', values);
            });

        });
    </script>
    <asp:Panel ID="TagSelectorPanel" runat="server">
        <asp:TextBox ID="ItemsList" runat="server" CssClass="autoSuggestTags"></asp:TextBox>
        <asp:HiddenField ID="Values" runat="server" />
    </asp:Panel>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>
