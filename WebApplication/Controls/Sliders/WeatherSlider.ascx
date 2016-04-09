<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeatherSlider.ascx.cs" Inherits="WebApplication.Controls.Sliders.WeatherSlider" %>

<script type="text/javascript">
    $(document).ready(function () {
        $('.carousel').carousel();
    });
</script>

<asp:ListView runat="server" ID="Cities" OnItemDataBound="Cities_OnItemDataBound">
    <LayoutTemplate>
        <div class="carousel slide" id="weatherWidgetSlider">
            <div class="carousel-inner">
                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
            </div>
            <%--<a data-slide="prev" href="#weatherWidgetSlider" class="left carousel-control">‹</a>
            <a data-slide="next" href="#weatherWidgetSlider" class="right carousel-control">›</a>--%>
        </div>
    </LayoutTemplate>
    <ItemTemplate>
        <div class="item">
            <Site:WeatherWidget runat="server" NumberOfDaysToPull="3" ID="WeatherWidget" />
        </div>
    </ItemTemplate>
</asp:ListView>