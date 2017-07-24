<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociationCheckBoxList.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.AssociationCheckBoxList" %>

<asp:Panel runat="server" ID="AdminPanel">

    <%--<script type="text/javascript">
        $(document).ready(function(){
            $("[class='RadioButtonList'] input").click(function () {                
                $(this).closest("[class='RadioButtonList']").find("input").not(this).prop("checked", false);
            })
        });
    </script>--%>

    <asp:CheckBoxList runat="server" ID="CheckBoxList" RepeatLayout="UnorderedList" Visible="false">
    </asp:CheckBoxList>

    <asp:RadioButtonList runat="server" ID="RadioButtonList" RepeatLayout="UnorderedList" Visible="false">
    </asp:RadioButtonList>

    <asp:HiddenField ID="Values" runat="server" />
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>