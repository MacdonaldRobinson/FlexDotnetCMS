<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FlickerGallery.ascx.cs" Inherits="WebApplication.Controls.Generic.FrickerGallery" %>

<style>
    /* This rule is read by Galleria to define the gallery height: */
    #galleria {
        height: 320px;
    }
</style>

<script type="text/javascript">
    Galleria.loadTheme('{BaseUrl}Scripts/galleria/themes/classic/galleria.classic.min.js');

    // Initialize Galleria
    Galleria.run('#galleria', {
        flickr: 'set:<%=FlickerPhotoSetID %>',
        flickrOptions: {
            sort: 'interestingness-desc'
        }
    });
</script>

<div id="galleria"></div>