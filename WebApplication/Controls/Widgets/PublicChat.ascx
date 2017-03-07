<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublicChat.ascx.cs" Inherits="WebApplication.Controls.Widgets.PublicChat" %>

<script type="text/javascript">
    $(document).ready(function () {
        var chatInterval = null;
        var chatRoomId = null;

        $("#LoadChat").on("click", function () {
            var nickName = $("#NickName").val();

            $("#ChatMessages").text("Loading Chat ...");

            chatInterval = setInterval(function () {
                GetPublicChat(nickName);
            }, 1000);            

            $("#FirstStep").hide();
            $("#ChatArea").show();
        });

        $("#ChatMessage").on("keypress", function (event) {
            event = event || window.event;
            if (event.keyCode == 13 || event.which == 13) {
                $("#SendMessage").click();
            }
        })

        $("#SendMessage").on("click", function () {
            var message = $("#ChatMessage").val();
            SendChatMessage(chatRoomId, message);
        });

        function SendChatMessage(chatRoomId, message) {
            $.get("/Webservices/Chat.asmx/SendMessage?chatRoomId=" + chatRoomId + "&message=" + message, function (data) {
                console.log(data);
            })
        }

        function GetPublicChat(nickname)
        {
            $.get("/Webservices/Chat.asmx/GetPublicChatRoom?NickName=" + nickname, function (data) {
                chatRoomId = data.ChatRoomID;

                $("#ChatMessages").val("");

                $(data.ChatMessages).each(function (index, elem) {
                    var currentText = $("#ChatMessages").val();
                    var newText = currentText + elem.ChatUser.NickName+": " + elem.Message +"\r\n";

                    if (currentText != newText)
                    {
                        $("#ChatMessages").val(newText);
                    }                    
                });
            })
        }

    });
</script>

<style type="text/css">
    label {
        display: block;
        font-weight:bold;
    }

    .field {
        margin-bottom: 10px;
    }

    #ChatArea {
        display:none;
    }

    #ChatArea input[type=text], #ChatArea textarea {
        width: 100%;
    }

    #ChatArea textarea {
        height: 200px;
    }

</style>

<div id="FirstStep">
    <label>Enter your nickname:</label>
    <input type="text" id="NickName" />
    <input id="LoadChat" type="button" value="LoadChat" />
</div>

<div id="ChatArea">
    <div class="field">
        <label>Chat Messages:</label>
        <textarea id="ChatMessages" readonly="readonly"></textarea>
    </div>
    <div class="field">
        <input type="text" id="ChatMessage" /><input id="SendMessage" type="button" value="Send Message" />
    </div>
</div>
