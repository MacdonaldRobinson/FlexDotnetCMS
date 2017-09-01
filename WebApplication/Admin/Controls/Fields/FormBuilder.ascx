<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FormBuilder.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.FormBuilder" %>

<asp:Panel runat="server" ID="AdminPanel">

<%  var uniqueId = Guid.NewGuid(); %>

<div id="tabs" class="tabs">
  <ul>
    <li><a href="#form-builder-<%=uniqueId %>">Form Builder</a></li>
    <li><a href="#form-submissions-<%=uniqueId %>">Form Submissions</a></li>
      <li><a href="#form-settings-<%=uniqueId %>">Form Settings</a></li>
  </ul>
  <div id="form-builder-<%=uniqueId %>">
    <script src="/Scripts/formBuilder-master/dist/form-builder.min.js"></script>
    <script>
        jQuery(function ($) {

            var fields = [
                {
                    label: "Email",
                    type: "text",
                    subtype: "email",
                    icon: "✉"
                }
            ];

            formBuilderOptions = {
                fields: fields,
                onSave: function () {
                    $("#<%= FormBuilderData.ClientID %>").val(formBuilder.formData);
                }
            }

            var formBuilder = $("#<%= FormBuilderWrapper.ClientID %>").formBuilder(formBuilderOptions);

            var currentVal = $("#<%= FormBuilderData.ClientID %>").val();

            formBuilder.promise.then(formBuilder => {
                formBuilder.actions.setData($("#<%= FormBuilderData.ClientID %>").val());
            });
            
        });
    </script>
    <div id="FormBuilderWrapper" runat="server"></div>

    <asp:HiddenField ID="FormBuilderData" runat="server" />    
  </div>
  <div id="form-submissions-<%=uniqueId %>">
      <asp:UpdatePanel runat="server" ID="FormSubmissionsWrapper">
          <Triggers>
              <asp:PostBackTrigger ControlID="ExportCSV" />
          </Triggers>
          <ContentTemplate>

              <div class="buttons">
                <asp:LinkButton ID="ExportCSV" Text="Export CSV" runat="server" OnClick="ExportCSV_Click"/>
                <asp:LinkButton ID="ClearAllSubmissions" Text="Clear All Submissions" runat="server" OnClick="ClearAllSubmissions_Click"/>
                  <div class="clear"></div>
              </div>
              <asp:GridView runat="server" ID="FormSubmissions" PageSize="10" AllowPaging="true" OnPageIndexChanging="FormSubmissions_PageIndexChanging">
              </asp:GridView>

          </ContentTemplate>
      </asp:UpdatePanel>
  </div>
    <div id="form-settings-<%=uniqueId %>">
        <asp:UpdatePanel runat="server" ID="UpdatePanel1" class="fieldsContainer">
            <ContentTemplate>
                <div>
                    <label>Email Address To Send Form Submissions To</label>
                    <asp:TextBox runat="server" ID="EmailAddress" />
                </div>            
                <div>
                    <label>Email Template Media ID</label>
                    <asp:TextBox runat="server" ID="EmailTemplateMediaID" />
                </div>
                <div class="buttons">
                    <asp:LinkButton Text="Save Settings" runat="server" ID="SaveSettings" OnClick="SaveSettings_Click"/>
                    <div class="clear"></div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>


</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>