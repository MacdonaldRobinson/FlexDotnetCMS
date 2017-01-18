<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaFieldsEditor.ascx.cs" Inherits="WebApplication.Admin.Controls.Editors.MediaFieldsEditor" %>

<script type="text/javascript">
    $(document).ready(function () {
        BindReOrder();

        check($("#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>"));

        $(document).on("change", "#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>", function () {
            check($("#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>"));
        });

        OnUpdatePanelRefreshComplete(function (event) {
            check($("#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>"));

            $("#<%= UseMediaTypeFieldFrontEndLayout.ClientID %>").click(function () {
                check(this);
            });

            initAccordians();
            BindReOrder();
            BindScrollMagic();
            initAceEditors();

            if (event != undefined && event._postBackSettings != undefined && event._postBackSettings.asyncTarget != undefined)
            {
                if (event._postBackSettings.asyncTarget.indexOf("$MediaFieldsEditor") !=-1)
                {
                    ReloadPreviewPanel();

                    if (event._postBackSettings.asyncTarget.indexOf("$Delete") != -1)
                    {
                        if (confirm("Successfully Updated, would you like to reload the page?")) {
                            window.location.reload();
                        }
                    }
                    else if(event._postBackSettings.asyncTarget.indexOf("$Update") != -1)
                    {
                        if($("#<%= Update.ClientID%>").text().indexOf("Add") != -1)
                        {
                            window.location.reload();
                        }
                    }
                }

            }
        });

        function check(elem) {
            if ($(elem).is(":checked")) {
                $("#FrontEndLayoutWrapper").hide();
            }
            else {
                $("#FrontEndLayoutWrapper").show();
            }
        }

        BindScrollMagic();

    });

    $(document).ajaxComplete(function () {
        BindReOrder();
        BindScrollMagic();
        initAccordians();
    });

    function BindReOrder()
    {
        BindGridViewSortable("#<%=ItemList.ClientID%>", "/Admin/Views/MasterPages/Webservice.asmx/ReOrderMediaFields", "<%= MediaFieldsUpdatePanel.ClientID%>", function () {
            window.location.href = window.location.href;
        });
    }

</script>

<style type="text/css">
    fieldset {
        position: relative;
    }
</style>

<asp:UpdatePanel runat="server" ID="MediaFieldsUpdatePanel" UpdateMode="Conditional">
    <ContentTemplate>
        <fieldset>
            <legend>Currently Created Fields</legend>
            <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="false" CssClass="DragDropGrid" OnPageIndexChanging="ItemList_PageIndexChanging" PageSize="20">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="OrderIndex" HeaderText="OrderIndex" SortExpression="OrderIndex" />
                    <asp:BoundField DataField="FieldCode" HeaderText="FieldCode" SortExpression="FieldCode" />
                    <asp:BoundField DataField="FieldLabel" HeaderText="FieldLabel" SortExpression="FieldLabel" />
                    <asp:BoundField DataField="GroupName" HeaderText="GroupName" SortExpression="GroupName" />
                    <asp:BoundField DataField="MediaTypeFieldID" HeaderText="MediaTypeFieldID" SortExpression="MediaTypeFieldID" />
                    <asp:BoundField DataField="DateCreated" HeaderText="DateCreated" SortExpression="DateCreated" />
                    <asp:BoundField DataField="DateLastModified" HeaderText="DateLastModified" SortExpression="DateLastModified" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:LinkButton ID="Edit" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Edit_Click">Edit</asp:LinkButton> |
                            <asp:LinkButton ID="Delete" runat="server" CommandArgument='<%# Eval("ID") %>' OnClick="Delete_Click" OnClientClick="return confirm('Are you sure you want to perminently delete this field? you will loose all data that has been assigned to this field.')">Delete</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </fieldset>
        <asp:Panel runat="server">
            <fieldset>
                <div style="margin-bottom:40px;">
                    <div id="SaveFields" class="buttons">
                        <asp:LinkButton Text="Save" runat="server" ID="Update" OnClick="Update_Click" CssClass="SaveFieldButton" />
                        <asp:LinkButton Text="Cancel" runat="server" ID="Cancel" OnClick="Cancel_Click" />
                    </div>
                    <div class="clear"></div>
                </div>
                <h2>
                    <asp:Literal ID="FieldDetailsTitle" runat="server" /></h2>
                <asp:HiddenField ID="FieldID" runat="server" Value="0" />
                <div>
                    <label for="<%# FieldCode.ClientID %>">Field Code:</label>
                <asp:TextBox runat="server" ID="FieldCode" />
                </div>
                <div>
                    <label for="<%# FieldLabel.ClientID %>">Field Label:</label>
                <asp:TextBox runat="server" ID="FieldLabel" />
                </div>
                <div>
                    <asp:CheckBox runat="server" ID="RenderLabelAfterControl" /> <label for="<%# RenderLabelAfterControl.ClientID %>">Render Label After Control</label>
                </div>
                <div>
                    <label for="<%# GroupName.ClientID %>">Group Name:</label>
                <asp:TextBox runat="server" ID="GroupName" />
                </div>
                <div id="AssociateWithMediaTypeFieldWrapper" runat="server" visible="false">
                    <asp:CheckBox runat="server" ID="AssociateWithMediaTypeField" /> <label for="<%# AssociateWithMediaTypeField.ClientID %>">Associate With Media Type Field</label>
                </div>
                <div>
                    <asp:DropDownList runat="server" ID="FieldTypeDropDown" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="FieldTypeDropDown_SelectedIndexChanged">
                        <asp:ListItem Text="--Select A Type--" Value="" />
                    </asp:DropDownList>
                </div>
                <div>
                    <label for="<%# AdminControl.ClientID %>">Admin Control:</label>
                <asp:TextBox runat="server" ID="AdminControl" TextMode="MultiLine" class="AceEditor" Height="300" />
                </div>
                <div id="UseMediaTypeFieldFrontEndLayoutWrapper" runat="server" visible="false">
                    <label>
                        <asp:CheckBox ID="UseMediaTypeFieldFrontEndLayout" runat="server" />
                        <label for="<%# UseMediaTypeFieldFrontEndLayout.ClientID %>">Use Media Type Field FrontEnd Layout</label>
                    </label>
                </div>

                <div class="accordian opened">
                    <h3>Front End Layout</h3>
                    <div>
                        <asp:TextBox runat="server" ID="FrontEndLayout" TextMode="MultiLine" class="AceEditor" Height="400"/>
                    </div>
                    <h3>Get Admin Control Value</h3>
                    <div>
                        <asp:TextBox runat="server" ID="GetAdminControlValue" TextMode="MultiLine" class="AceEditor" Height="200"/>
                    </div>
                    <h3>Set Admin Control Value</h3>
                    <div>
                        <asp:TextBox runat="server" ID="SetAdminControlValue" TextMode="MultiLine" class="AceEditor" Height="200"/>
                    </div>
                    <h3>Field Value</h3>
                    <div>
                        <asp:TextBox runat="server" ID="FieldValue" TextMode="MultiLine" />
                    </div>
                </div>
            </fieldset>
        </asp:Panel>
    </ContentTemplate>
    <Triggers>
        <asp:AsyncPostBackTrigger ControlID="FieldTypeDropDown" EventName="SelectedIndexChanged" />
    </Triggers>
</asp:UpdatePanel>