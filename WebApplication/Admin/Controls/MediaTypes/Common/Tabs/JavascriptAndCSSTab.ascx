<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JavascriptAndCSSTab.ascx.cs" Inherits="WebApplication.Admin.Controls.MediaTypes.Common.Tabs.JavascriptAndCSSTab" %>

<fieldset id="AccordianHolder">
    <div class="accordian opened">
        <h3>Javascript</h3>
        <div>
            <asp:TextBox runat="server" ID="Javascript" CssClass="AceEditor" Height="600px" TextMode="MultiLine"></asp:TextBox>
        </div>
        <h3>CSS</h3>
        <div>
            <asp:TextBox runat="server" ID="CSS" CssClass="AceEditor" Height="600px" TextMode="MultiLine"></asp:TextBox>
        </div>
    </div>
</fieldset>