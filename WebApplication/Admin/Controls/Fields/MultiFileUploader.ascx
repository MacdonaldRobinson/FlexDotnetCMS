<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultiFileUploader.ascx.cs" Inherits="WebApplication.Admin.Controls.Fields.MultiFileUploader" %>

<asp:Panel runat="server" ID="AdminPanel">
    <fieldset>
        <asp:FileUpload ID="MultiFileUpload" runat="server" AllowMultiple="true" />
        <div id="dvPreview" runat="server"></div>
        <div class="UploadedItems" id="UploadedItems" runat="server">
            <asp:ListView runat="server" ID="Values" ItemType="FrameworkLibrary.FieldFile">
                <LayoutTemplate>
                    <fieldset>
                        <h3>Uploaded Files</h3>
                        <ul class="sortable">
                            <asp:PlaceHolder runat="server" ID="itemPlaceHolder" />
                        </ul>
                        <div class="clear"></div>
                    </fieldset>
                </LayoutTemplate>
                <ItemTemplate>
                    <li class="item" data-fieldfileid="<%# Item.ID %>">
                        <a href="<%# URIHelper.ConvertToAbsUrl(Item.PathToFile) %>" target="_blank">
                        <img src="<%# URIHelper.ConvertToAbsUrl(Item.PathToFile) %>?width=200&mode=max" alt="<%# Item.Name %>" />
                        </a>
                        <div>
                            <a href="/Admin/Views/PageHandlers/FieldFiles/Detail.aspx?id=<%# Item.ID %>" target="_blank" class="colorbox iframe EditImage" data-id="<%# Item.ID %>">Edit</a> |
                            <a href="javascript:void(0)" class="DeleteImage" data-id="<%# Item.ID %>">Delete</a>
                        </div>
                    </li>
                </ItemTemplate>
            </asp:ListView>
            <asp:TextBox runat="server" CssClass="FilesToDelete" ID="FilesToDelete" Text="[]" Style="display: none;" />
            <asp:TextBox runat="server" CssClass="ReorderFiles" ID="ReorderFiles" Text="[]" Style="display: none;"/>
        </div>
    </fieldset>
    <style type="text/css">
        img.MarkedAsDeleted {
            opacity: 0.5;
        }

        .UploadedItems .item {
            float: left;
            margin-left: 5px;
            text-align: center;
        }

            .UploadedItems .item img {
                height: 100px;
                width: 100px;
            }
    </style>
    <script language="javascript" type="text/javascript">
        $(function () {

            $("#<%= UploadedItems.ClientID%> img").error(function () {
                $(this).attr("src", "/media/images/icons/File.jpg");
            });

            $("#<%= UploadedItems.ClientID%> .DeleteImage").click(function () {
                var parentItem = $(this).parents(".item");
                var ImagesToDelete = $("#<%= FilesToDelete.ClientID %>");
                var image = parentItem.find("img");
                var fieldFileId = $(this).attr('data-id');

                var ImagesToDeleteJson = JSON.parse(ImagesToDelete.val());
                var src = image.attr("src");

                if (!image.hasClass("MarkedAsDeleted")) {
                    image.addClass("MarkedAsDeleted");

                    if (ImagesToDeleteJson.indexOf(fieldFileId) == -1) {
                        ImagesToDeleteJson.push(fieldFileId);
                    }

                    ImagesToDelete.val(JSON.stringify(ImagesToDeleteJson));

                    $(this).text("UnDelete");
                }
                else {
                    image.removeClass("MarkedAsDeleted");

                    var index = ImagesToDeleteJson.indexOf(fieldFileId)

                    if (index != -1) {
                        ImagesToDeleteJson.splice(index, 1);
                        //ImagesToDeleteJson.push(fieldFileId);
                    }

                    ImagesToDelete.val(JSON.stringify(ImagesToDeleteJson));

                    $(this).text("Delete");
                }
            });

            $(".sortable").sortable({
                update: function (event, ui) {
                    var arr = new Array();
                    $(this).children("li").each(function () {
                        var id = $(this).attr("data-fieldfileid");
                        arr.push(id);
                    });

                    $("#<%=ReorderFiles.ClientID%>").val(JSON.stringify(arr));
                }
            });

            $("#<%= MultiFileUpload.ClientID %>").change(function () {
                if (typeof (FileReader) != "undefined") {
                    var dvPreview = $("#<%=dvPreview.ClientID%>");
                    dvPreview.html("");
                    var regex = /^([a-zA-Z0-9\s_\\.\-:])+(.jpg|.jpeg|.gif|.png|.bmp|.pdf|.csv|.docx|.doc)$/;
                    $($(this)[0].files).each(function () {
                        var file = $(this);
                        if (regex.test(file[0].name.toLowerCase())) {
                            var reader = new FileReader();
                            reader.onload = function (e) {

                                if (file[0].type.indexOf("image") != -1) {
                                    var img = $("<img />");
                                    img.attr("style", "width: 100px; height: 100px;");
                                    img.attr("src", e.target.result);
                                    dvPreview.append(img);
                                }
                                else {
                                    var link = $("<a>" + file[0].name + "</a>");
                                    link.attr("href", e.target.result);
                                    dvPreview.append(link);
                                }
                            }
                            reader.readAsDataURL(file[0]);
                        } else {
                            alert(file[0].name + " is not a valid image file.");
                            dvPreview.html("");
                            return false;
                        }
                    });
                } else {
                    alert("This browser does not support HTML5 FileReader.");
                }
            });
        });
    </script>
</asp:Panel>
<asp:Panel runat="server" ID="FrontEndPanel">
    <asp:PlaceHolder runat="server" ID="DynamicContent" />
</asp:Panel>