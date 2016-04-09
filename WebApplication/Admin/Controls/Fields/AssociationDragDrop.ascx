<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssociationDragDrop.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.AssociationDragDrop" %>

<asp:Panel runat="server" ID="AdminPanel">
    <style type="text/css">
        .dropZone {
            background-color: #bfbfbf;
            padding: 5px;
        }

            .dropZone li {
                background-color: #808080;
                padding: 3px;
                margin: 3px;
            }

            .dropZone .delete {
                color: #e62626;
                cursor: pointer;
                font-size: 20px;
                font-weight: bold;
                margin: 10px;
            }

        .hidden {
            display: none;
        }
    </style>

    <script type="text/javascript">
        $(document).ready(function () {
            UpdateULFromValues();

            $(document).on("click", "#<%=UL.ClientID%> li a.delete", function () {
                $(this).parents("li").remove();
                UpdateValuesFromUL($("#<%=UL.ClientID%>")[0]);
            });

            $("#<%=UL.ClientID%>").bind("DOMSubtreeModified", function () {
                UpdateValuesFromUL(this);
            });

            $("#<%=UL.ClientID%>").sortable({
                update: function (event, ui) {
                    UpdateValuesFromUL($("#<%=UL.ClientID%>")[0]);
                }
            });

            function UpdateULFromValues() {
                var json = eval($("#<%= Values.ClientID%>").val());
                /*var valsArray = vals.split(",");

                for (item of valsArray) {
                    console.log(item);
                }*/

                $(json).each(function () {
                    //console.log(this);

                    $("#<%=UL.ClientID%>").append("<li data-mediadetailid='" + this.id + "'><a class='delete'>x</a><span class='text'>" + this.name + "</span></li>");

                });
            }

            function UpdateValuesFromUL(elem) {
                //console.log(elem);
                var arr = new Array();

                $(elem).children("li:not(.hidden)").each(function () {
                    var mediadetailid = $(this).attr("data-mediadetailid");
                    var name = $(this).children("span.text").text();

                    if (name != "")
                    {
                        var obj = new Object();
                        obj.name = name;
                        obj.id = mediadetailid;

                        arr.push(obj);
                    }

                });

                var jsonString = JSON.stringify(arr);

                $("#<%= Values.ClientID%>").val(jsonString);
            }

        });
    </script>

    <ul id="UL" runat="server" class="dropZone sortable">
        <li class="hidden">
            <asp:HiddenField runat="server" ID="Values" />
        </li>
    </ul>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>