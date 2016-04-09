<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="WeatherWidget.ascx.cs" Inherits="WebApplication.Controls.WeatherWidget" %>

<div class="weatherWidget">
    <div class="currentQuery transparentBackground">
        <div>
            <strong>
                <asp:Literal runat="server" ID="CityQuery"></asp:Literal></strong><br />
            <asp:Literal runat="server" ID="ObservationTime"></asp:Literal>
        </div>
        <div class="currentDetails">
            <asp:Image runat="server" ID="Icon" />
            <div class="currentTemp">
                <asp:Literal runat="server" ID="Temp"></asp:Literal>&deg;C
            </div>
            <div>
                <asp:Literal runat="server" ID="WeatherDesc"></asp:Literal>
            </div>
            <div class="clear"></div>
        </div>
    </div>
    <div>
        <asp:ListView runat="server" ID="WeatherList" OnItemDataBound="WeatherList_OnItemDataBound">
            <LayoutTemplate>
                <asp:PlaceHolder runat="server" ID="itemPlaceHolder"></asp:PlaceHolder>
            </LayoutTemplate>
            <ItemTemplate>
                <div class="weather transparentBackground">
                    <div class="iconWrapper">
                        <asp:Image runat="server" ID="Icon" />
                    </div>
                    <div>
                        <asp:Literal runat="server" ID="MinTemp"></asp:Literal>&deg;C
                    </div>
                    <div>
                        <asp:Literal runat="server" ID="MaxTemp"></asp:Literal>&deg;C
                    </div>
                    <div>
                        <asp:Literal runat="server" ID="Date"></asp:Literal>
                    </div>
                </div>
            </ItemTemplate>
        </asp:ListView>
        <div class="clear"></div>
    </div>
    <div class="clear"></div>
</div>