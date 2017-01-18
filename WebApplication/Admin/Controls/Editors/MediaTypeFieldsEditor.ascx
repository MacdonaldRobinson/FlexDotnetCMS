<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MediaTypeFieldsEditor.ascx.cs" Inherits="WebApplication.Admin.Controls.Editors.MediaTypeFieldsEditor" %>

<script type="text/javascript">
    $(document).ready(function () {
        OnUpdatePanelRefreshComplete(function () {
            initAceEditors();
        });
    });
</script>

<asp:UpdatePanel runat="server">
    <ContentTemplate>
        <fieldset>
            <legend>Currently Created Fields</legend>
            <asp:GridView runat="server" ID="ItemList" AutoGenerateColumns="false" AllowPaging="false" OnPageIndexChanging="ItemList_PageIndexChanging">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" SortExpression="ID" />
                    <asp:BoundField DataField="FieldCode" HeaderText="FieldCode" SortExpression="FieldCode" />
                    <asp:BoundField DataField="FieldLabel" HeaderText="FieldLabel" SortExpression="FieldLabel" />
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
                <div>
                    <asp:DropDownList runat="server" ID="FieldTypeDropDown" AutoPostBack="true" AppendDataBoundItems="true" OnSelectedIndexChanged="FieldTypeDropDown_SelectedIndexChanged">
                        <asp:ListItem Text="--Select A Type--" Value="" />
                    </asp:DropDownList>
                </div>

                <div>
                    <label for="<%# AdminControl.ClientID %>">Admin Control:</label>
                    <asp:TextBox runat="server" ID="AdminControl" TextMode="MultiLine" class="AceEditor" />
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
                    <h3>Default Field Value</h3>
                    <div>
                        <asp:TextBox runat="server" ID="FieldValue" TextMode="MultiLine" />
                    </div>
                </div>
                <div class="buttons">
                    <asp:LinkButton Text="Save" runat="server" ID="Update" OnClick="Update_Click" CssClass="SaveFieldButton"/>
                    <asp:LinkButton Text="Cancel" runat="server" ID="Cancel" OnClick="Cancel_Click" />
                </div>
            </fieldset>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>