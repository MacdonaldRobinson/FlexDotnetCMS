<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommonIncludes.ascx.cs"
    Inherits="WebApplication.Controls.Header" %>
<%@ Import Namespace="WebApplication" %>

<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<meta name="viewport" content="user-scalable=no, initial-scale=1.0, maximum-scale=1.0, width=device-width">
<%--<link rel="apple-touch-icon" href="" />--%>

<%
    if (BasePage.CurrentMediaDetail != null)
    {
%><meta property="og:url" content="<%= BasePage.CurrentMediaDetail.AbsoluteUrl %>" />
<meta property="og:title" content="<%= BasePage.CurrentMediaDetail.Title %>" />
<meta property="og:site_name" content="<%= BasePage.CurrentWebsite.Title %>" />
<meta property="og:description" content="<%= StringHelper.StripExtraSpaces(StringHelper.StripHtmlTags(BasePage.CurrentMediaDetail.GetMetaDescription())) %>" /><%
    }
%>

<%= BasePage.GetSettings().GlobalCodeInHead %>