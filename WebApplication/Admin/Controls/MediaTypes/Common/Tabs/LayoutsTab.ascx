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
    <div id="ItemLayouts" class="accordian opened">
        <h3>Main Layout</h3>
        <div>
            <asp:TextBox runat="server" ID="MainLayout" TextMode="MultiLine" Height="500px" CssClass="AceEditor CanAttachToBrowserPanel"/>
        </div>
        <h3>Summary Layout</h3>
        <div>
            <asp:TextBox runat="server" ID="SummaryLayout" TextMode="MultiLine" Height="400px" CssClass="AceEditor" />
        </div>
        <h3>Featured Layout</h3>
        <div>
            <asp:TextBox runat="server" ID="FeaturedLayout" TextMode="MultiLine" Height="400px" CssClass="AceEditor" />
        </div>
    </div>

</fieldset>