<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="CommonPanel.ascx.cs"
    Inherits="WebApplication.Admin.Controls.MediaTypes.CommonPanel" %>
<%@ Register Src="~/Admin/Controls/MediaTypes/Common/Tabs/PublishSettingsTab.ascx" TagPrefix="Admin" TagName="PublishSettingsTab" %>
<%@ Register Src="~/Admin/Controls/MediaTypes/Common/Tabs/SEOSettingsTab.ascx" TagPrefix="Admin" TagName="SEOSettingsTab" %>

<script type="text/javascript">

    $(document).ready(function () {

        $(".LinkTitle").on("blur", function () {
            CopyToOtherFields($(this).val(), this);
        });

        $(".LinkTitle").on("focus", function () {
            ClearFieldsIfSameAs($(this).val(), this);
        });
    });

    function ClearFieldsIfSameAs(value, objReference) {
        if ($(".MediaTitle").val() == value)
            $(".MediaTitle").val("");

        if ($(".SectionTitle").val() == value)
            $(".SectionTitle").val("");

        if ($(".field.SectionTitle input:not(:hidden)").val() == value)
            $(".field.SectionTitle input:not(:hidden)").val("");

        for (edId in tinymce.editors) {
            var editor = tinymce.editors[edId];
            var content = editor.getContent();

            if (content == value || content == "<p>" + value + "</p>")
            {
                editor.setContent("");
                editor.save();
            }
        }

        //console.log(tinymce.getInstanceById('title'));

    }

    function CopyToOtherFields(value, objReference) {
        if ($(".MediaTitle").val() == "")
            $(".MediaTitle").val(value);

        if ($(".SectionTitle").val() == "")
            $(".SectionTitle").val(value);

        if ($(".field.SectionTitle input:not(:hidden)").val() == "")
            $(".field.SectionTitle input:not(:hidden)").val(value);


        for (edId in tinymce.editors) {
            var editor = tinymce.editors[edId];
            var content = editor.getContent();

            if (content == "")
            {
                editor.setContent("<p>" + value + "</p>");
                editor.save();
            }
        }


        //tinymce.get('ShortDescription').setContent(value);
    }
</script>
<style type="text/css">
    .field {
        margin-bottom: 15px;
        padding-bottom: 15px;
        border-bottom: 1px solid #ccc;
    }
</style>

<div id="tabs" class="tabs">
    <ul>
        <li><a href="#Main">Main</a></li>
        <asp:ListView runat="server" ID="Tabs">
            <ItemTemplate>
                <li><a href="#<%# StringHelper.CreateSlug((string)Container.DataItem) %>"><%# Container.DataItem %></a></li>
            </ItemTemplate>
        </asp:ListView>
    </ul>
    <div id="Main">
        <fieldset id="MainFields">
            <div id="VirtualPathHolder">
                <label class="exception" for="<%= VirtualPath.ClientID %>">
                    Virtual Path:</label>
                <asp:HyperLink ID="VirtualPath" runat="server" Target="_blank"></asp:HyperLink>
            </div>
            <div id="LinkTitleHolder">
                <label for="<%= LinkTitle.ClientID %>">
                    Link Title</label>
                <asp:TextBox ID="LinkTitle" runat="server" CssClass="LinkTitle"></asp:TextBox>
            </div>
            <div id="TagsHolder">
                <label>Tags</label>
                <Admin:TagsSelector ID="TagsSelector" runat="server"/>
            </div>

            <asp:PlaceHolder runat="server" ID="PanelFieldsPlaceHolder" />
        </fieldset>
              <asp:ListView runat="server" ID="FieldGroupTabContents" OnItemDataBound="FieldGroupTabContents_ItemDataBound">
                    <LayoutTemplate>
                        <fieldset id="FieldsHolder">
                            <div class="accordian opened">
                                <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                            </div>
                        </fieldset>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <h3><%# ((Eval("Key") == null || Eval("Key").ToString() == "") ? "Main Fields" : Eval("Key")) %></h3>
                        <div>
                            <asp:ListView runat="server" ID="MediaFieldsList" OnItemDataBound="MediaFieldsList_ItemDataBound" ItemType="FrameworkLibrary.MediaDetailField">
                                <LayoutTemplate>
                                    <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                                </LayoutTemplate>
                                <ItemTemplate>
                                    <asp:Panel runat="server" CssClass="field" ID="FieldWrapper">
                                        <asp:HiddenField ID="FieldID" Value="0" runat="server" />
                                        <%# (!Item.RenderLabelAfterControl) ? "<label data-fieldcode='" + Item.FieldCode + "' data-fieldid=" + Item.ID + ">" + Item.FieldLabel + "</label> <i class='fa fa-question-circle tooltip' aria-hidden='true' title='" + Item.FieldDescription + "<p>Usage Example:</p><p>"+ Item.UsageExample +"</p>'></i>" : "" %>
                                            <asp:PlaceHolder runat="server" ID="DynamicField" />
                                        <%# (Item.RenderLabelAfterControl) ? "<label data-fieldcode='"+Item.FieldCode+"' data-fieldid="+Item.ID+">"+Item.FieldLabel+"</label> <i class='fa fa-question-circle tooltip' aria-hidden='true' title='"+Item.FieldDescription+"'></i><p>Usage Example:</p><p>"+ Item.UsageExample +"</p>'" : "" %>
                                    </asp:Panel>
                                </ItemTemplate>
                            </asp:ListView>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
        <fieldset id="AccordianHolder">
            <div class="accordian closed">
                <h3>SEO Settings</h3>
                <div>
                    <Admin:SEOSettingsTab runat="server" ID="SEOSettingsTab" />
                </div>
                <h3>Publish Settings</h3>
                <div>
                    <Admin:PublishSettingsTab runat="server" ID="PublishSettingsTab" />
                </div>
            </div>
        </fieldset>

    </div>
    <div id="TabPanelsHolder">
        <asp:ListView runat="server" ID="TabPanels" OnItemDataBound="TabPanels_ItemDataBound">
            <ItemTemplate>
                <div id="<%# StringHelper.CreateSlug((string)Eval("Name")) %>">
                    <asp:PlaceHolder runat="server" ID="TabControlHolder" />
                </div>
            </ItemTemplate>
        </asp:ListView>
    </div>
</div>