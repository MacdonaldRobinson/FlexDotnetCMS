<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenericSlideShow.ascx.cs"
    Inherits="WebApplication.Controls.Generic.GenericSlideShow" %>
<asp:Panel ID="SlidesPanel" runat="server" Style="max-width: 100%; max-height: 100%">
    <script type="text/javascript">
        $(window).load(function () {
            sizeToFit();
            $('#<%= this.ClientID+"_slider" %>').show();
            var slider = $('#<%= this.ClientID+"_slider" %>').bxSlider({
                mode: '<%= this.Mode %>',
                speed: <%= this.Speed %>,
                infiniteLoop: <%= this.InfiniteLoop %>,
                controls: <%= this.Controls %>,
                prevText: '<%= this.PrevText %>',
                prevImage: '<%= this.PrevImage %>',
                nextText: '<%= this.NextText %>',
                nextImage: '<%= this.NextImage %>',
                hideControlOnEnd: <%= this.HideControlOnEnd %>,
                captions: <%= this.Captions %>,
                auto: <%= this.Auto %>,
                pause: <%= this.Pause %>,
                pager: <%= this.Pager %>,
                pagerType: '<%= this.PagerType %>',
                displaySlideQty: <%= this.DisplaySlideQty %>,
                moveSlideQty: <%= this.MoveSlideQty %>,
                autoControls: <%= this.AutoControls %>,
                touchEnabled: 'false',
                preventDefaultSwipeX: 'false',
                preventDefaultSwipeY: 'false',
                adaptiveHeight: true,
                onSliderLoad: function(currentIndex){
                    $('#<%= this.ClientID+"_slider" %> li:nth-child(2)').addClass("current");

                    updateAlternativeImageViewer();
                },
                onSlideAfter: function ($slideElement, oldIndex, newIndex){
                    $slideElement.parent().find("li").removeClass("current");
                    $slideElement.addClass("current");

                    updateAlternativeImageViewer();
                }

            });

            function updateAlternativeImageViewer(){
                var img = $('#<%= this.ClientID+"_slider" %> li.current img');

                $('#<%= AlternativeImageViewerID %>').fadeOut(100, function(){
                    $('#<%= AlternativeImageViewerID %>').attr("src", img.attr("data-alt-image"));
                });
                $('#<%= AlternativeImageViewerID %>').fadeIn();
            }

            //$('#<%= this.ClientID+"_slider" %> .popup[rel="group"][disabled!="disabled"]').colorbox({ slideshow: true });
        });

        $(window).resize(function () {
            sizeToFit();
        });

        function sizeToFit  () {
            $('.bx-wrapper, .bx-window').css({ width: "100%" });
            $('.bx-window img').each(function () {
                var actualImageWidth = $(this).width();
                var containerWidth = $('.bx-window').width();
                var difference = actualImageWidth - containerWidth;

                if (difference < containerWidth)
                    $(this).css("margin-left", (difference - (difference * 2)) / 2);
            });
        }
    </script>
    <style type="text/css">
        .bx-wrapper img {
            width: auto;
            margin: 0 auto;
        }

        .slider {
            margin: 0;
            padding: 0;
        }
    </style>
    <ul id='<%= this.ClientID %>_slider' class="slider" style="display: none">
        <asp:ListView ID="SlidesList" runat="server" OnItemDataBound="SlidesList_OnItemDataBound">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceholder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <li>
                    <asp:Panel ID="ImageSliderPanel" runat="server">
                        <asp:HyperLink ID="Link" runat="server" rel="group">
                            <asp:Panel ID="NonFlashPanel" runat="server">
                                <asp:Image ID="Image" runat="server" CssClass="img-responsive" />
                            </asp:Panel>
                        </asp:HyperLink>
                    </asp:Panel>
                    <asp:Panel ID="ContentSliderPanel" runat="server" Visible="false">
                    </asp:Panel>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </ul>
</asp:Panel>