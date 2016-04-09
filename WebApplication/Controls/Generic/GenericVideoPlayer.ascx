<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenericVideoPlayer.ascx.cs"
    Inherits="WebApplication.Controls.Generic.GenericVideoPlayer" %>
<script type="text/javascript">
    $(document).ready(function () {
        var flashVideoUrl = '<%= this.FlashVideoUrl %>';
        var htmlVideoUrl = '<%= this.HtmlVideoUrl %>';
        var downloadVideoUrl = '<%= this.DownloadVideoUrl %>';
        var playerSwfUrl = '<%=this.PlayerSwfUrl %>';

        jwplayer('<%= MediaPlayer.ClientID %>').setup({
            file: flashVideoUrl,
            image: '<%= this.PreviewImageUrl %>',
            autostart: '<%= this.AutoStart %>',
            width: '<%=this.Width %>',
            height: '<%=this.Height %>',
            'modes': [
                        { type: 'flash', src: playerSwfUrl },
                        {
                            type: 'html5',
                            config: {
                                'file': htmlVideoUrl,
                                'provider': 'video'
                            }
                        },
                        {
                            type: 'download',
                            config: {
                                'file': downloadVideoUrl,
                                'provider': 'video'
                            }
                        }
                    ]
        });
    });
</script>
<asp:Panel ID="MediaPlayer" runat="server">
</asp:Panel>