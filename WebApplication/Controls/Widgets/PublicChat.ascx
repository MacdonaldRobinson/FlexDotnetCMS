<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PublicChat.ascx.cs" Inherits="WebApplication.Controls.Widgets.PublicChat" %>

<script type="text/javascript">
    $(document).ready(function () {
        var chatInterval = null;
        var chatRoomId = null;        
        var nickNameText = "";
        
        var AlertMessages = $("#AlertMessages");
        var ChatAreaWrapper = $("#ChatAreaWrapper");
        var ChatArea = $("#ChatArea");
        var ChatMessage = $("#ChatMessage");
        var SendMessage = $("#SendMessage");
        var LoadChat = $("#LoadChat");
        var NickName = $("#NickName");  
        var FirstStep = $("#FirstStep");  

        var webserviceUrl = "/Webservices/Chat.asmx";

        AlertMessages.show();
        AlertMessages.text("Please wait checking your session ...");

        $.get(webserviceUrl+"/GetChatUserBySession", function (data) {

            if (data.NickName == null || data.NickName == "")
            {
                AlertMessages.text("");
                AlertMessages.hide();
                FirstStep.show();
            }
            else
            {
                AlertMessages.text("");
                AlertMessages.hide();
                NickName.val(data.NickName);
                LoadChat[0].click();
            }
        })

        LoadChat.on("click", function () {
            nickNameText = NickName.val();

            ChatArea.html("Loading Chat ...");

            chatInterval = setInterval(function () {
                GetPublicChat(nickNameText);
            }, 1000);            

            FirstStep.hide();
            ChatAreaWrapper.show();
        });

        ChatMessage.on("keypress", function (event) {
            event = event || window.event;
            if (event.keyCode == 13 || event.which == 13) {
                SendMessage.click();
            }
        });

        SendMessage.on("click", function () {
            var message = ChatMessage.val();
            SendChatMessage(chatRoomId, message);
        });

        function SendChatMessage(chatRoomId, message) {
            message = encodeURI(message);
            $.post(webserviceUrl + "/SendMessage", { chatRoomId: chatRoomId, message: message } , function (data) {
                ChatMessage.val("");
                GetPublicChat(nickNameText);
            })
        }

        function GetPublicChat(nickname)
        {
            ChatArea.scrollTop(ChatArea[0].scrollHeight);

            $.get(webserviceUrl + "/GetPublicChatRoom?NickName=" + nickname, function (data) {

                chatRoomId = data.ChatRoomID;

                ChatArea.html("");                

                $(data.ChatMessages).each(function (index, elem) {
                    
                    var currentText = ChatArea.html();
                    var newText = currentText;

                    if (elem.MessageMode == 1)
                    {
                        newText = newText + "<span class='chatMessageEntry'><span class='chatMessageEntryNickName'>" + elem.ChatUser.NickName + "</span>" + decodeURI(elem.Message) + "</span>";
                    }
                    else if (elem.MessageMode == 0)
                    {
                        newText = newText + "<span class='chatMessageEntry systemMessage'>" + elem.Message + "</span>";
                    }

                    if (currentText != newText)
                    {
                        ChatArea.html(newText);
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

    #ChatWindowWrapper {
        border: 1px solid #F8CBB4;
        width: 400px;
        background-color: #F8CBB4;
        padding: 10px;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;
    }

    #FirstStep, #ChatAreaWrapper, #AlertMessages {
        display:none;
    }

    #ChatAreaWrapper #ChatMessage, #ChatAreaWrapper #ChatArea {
        width: 100%;
    }

    #ChatAreaWrapper #ChatArea {
        height: 300px;
        overflow-y:scroll;
        background-color: #F2F2F2;
        padding: 5px;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;
    }

    .chatMessageEntry {
        display: block;
        margin-bottom: 5px;
        background-color: #fff;
        padding: 5px;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;        
        font-size: 12px;
    }

    .chatMessageEntry.systemMessage{
        color: gray;
    }

    .chatMessageEntry .chatMessageEntryNickName{
        display:block;
        color: #DE8400;
    }

</style>

<div id="ChatWindowWrapper"> 
    <div id="ChatWindow">
        <div id="AlertMessages"></div>

        <div id="FirstStep">
            <label>Enter your nickname:</label>
            <input type="text" id="NickName" />
            <input id="LoadChat" type="button" value="LoadChat" />
        </div>

        <div id="ChatAreaWrapper">
            <div class="field">
                <label>Welcome to LIVE Chat:</label>
                <div id="ChatArea"></div>
            </div>
            <div class="field">
                <textarea id="ChatMessage"></textarea><input id="SendMessage" type="button" value="Send Message" />
            </div>
        </div>
    </div>
</div>