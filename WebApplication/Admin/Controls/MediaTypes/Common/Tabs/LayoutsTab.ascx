<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LayoutsTab.ascx.cs" Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.LayoutsTab" %>

<script type="text/javascript">
    $(document).ready(function () {
        check();

        $(".UseMediaTypeLayouts input[type=checkbox]").on("change", function () {
            check();
        });

        function check() {
            if ($(".UseMediaTypeLayouts input[type=checkbox]").prop("checked")) {
                $("#ItemLayouts").hide();
            }
            else {
                $("#ItemLayouts").show();
            }
        }

    });
</script>

<fieldset>
    <div>
        <label class="exception" for="<%= UseMediaTypeLayouts.ClientID %>">
            <asp:CheckBox runat="server" ID="UseMediaTypeLayouts" CssClass="UseMediaTypeLayouts" />
            Use Media Type Layouts
            <br />
            <em>NOTE: If this is checked then the media type layouts will be used instead of the layouts below</em>
        </label>
    </div>
    <div id="ItemLayouts">
        <div>
            <label class="exception" for="<%= MainLayout.ClientID %>">
                Main Layout:</label>
            <asp:TextBox runat="server" ID="MainLayout" TextMode="MultiLine" Height="300px" CssClass="AceEditor" />
        </div>
        <div>
            <label class="exception" for="<%= SummaryLayout.ClientID %>">
                Summary Layout:</label>
            <asp:TextBox runat="server" ID="SummaryLayout" TextMode="MultiLine" Height="150px" CssClass="AceEditor" />
        </div>
        <div>
            <label class="exception" for="<%= FeaturedLayout.ClientID %>">
                Featured Layout:</label>
            <asp:TextBox runat="server" ID="FeaturedLayout" TextMode="MultiLine" Height="150px" CssClass="AceEditor" />
        </div>
    </div>
</fieldset>