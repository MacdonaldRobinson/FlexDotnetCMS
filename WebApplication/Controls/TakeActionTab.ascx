<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TakeActionTab.ascx.cs" Inherits="WebApplication.Controls.TakeActionTab" %>

<div id="takeActionTab">
    <div id="handle" class="floatLeft"></div>
    <div id="tabContent" class="floatLeft">
        <Site:GenerateNav runat="server" RootVirtualPath="~/take-action/" />
        <div class="share">
            <strong>Share:</strong>
            <span class='st_sharethis_large' displaytext='ShareThis'></span>
            <span class='st_facebook_large' displaytext='Facebook'></span>
        </div>
    </div>
</div>