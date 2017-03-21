<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Chat.ascx.cs" Inherits="WebApplication.Controls.Widgets.Chat" %>

<script type="text/javascript">
    $(document).ready(function () {
        var getChatInterval = null;
        var getChatRoomsInterval = null;
        var getCheckSessionInterval = null;

        var chatUserId = "";
        var chatRoomId = 0;        
        var chatRoomName = "";
        var nickNameText = "";
        var roomMode = "<%= ChatRoomMode %>";
        var initialRoomMode = "<%= ChatRoomMode %>";
        
        var AlertMessages = $("#AlertMessages");
        var ChatScreen = $("#ChatScreen");
        var ChatArea = $("#ChatArea");
        var ChatMessage = $("#ChatMessage");
        var SendMessage = $("#SendMessage");
        var LoadChat = $("#LoadChat");
        var NickName = $("#NickName");  
        var LoginScreen = $("#LoginScreen");  
        var ClearChat = $("#ClearChat");          
        var ChatRoomUsers = $("#ChatRoomUsers");
        var PublicChatRooms = $("#PublicChatRooms");
        var PrivateChatRooms = $("#PrivateChatRooms");
        var AddPublicChatRoom = $("#AddPublicChatRoom");     
        var ChatRoomName = $("#ChatRoomName");                

        var webserviceUrl = "/Webservices/Chat.asmx";

        ShowAlertMessage("Please wait checking your session ...");
        LoginScreen.hide();
        ChatScreen.hide();

        CheckSession();

        $(".openChat").on("click", function () {
            $("#ChatWindow").slideToggle();
        });

        $(document).on("click", ".deleteRoom", function (e) {

            var id = $(this).attr("data-chatroomid");

            $.get(webserviceUrl + "/DeleteChatRoom?chatRoomId=" + id, function (data) {                
            })

            e.preventDefault();
            return false;
        });

        $(document).on("click", ".chatRoomEntry" , function(){
            var id = $(this).attr("data-chatroomid");            
            var name = $(this).text();
            var mode = $(this).attr("data-chatroommode");

            SwitchChatRoom(id, name, mode);
        });

        $(document).on("click", ".chatRoomUserEntry" , function(){
            if(initialRoomMode == "Public")
            {            
                var otherChatUserId = $(this).attr("data-chatroomuserid");

                $.get(webserviceUrl + "/CreatePrivateChatRoomWith?otherChatUserId=" + otherChatUserId, function (data) {                          
                    if(data != null)
                    {                  
                        SwitchChatRoom(data.ChatRoomID, data.ChatRoomName, "Private");
                    }
                });
            }
        });
        

        AddPublicChatRoom.on("click", function(){
            clearInterval()
            var newChatRoomName = prompt("Enter a name for the chatroom");

            if(newChatRoomName != null)
            {                
                SwitchChatRoom(0, newChatRoomName, "Public");
            }                       
        });

        ClearChat.on("click", function() {
            $.get(webserviceUrl + "/ClearChatRoom?chatRoomId=" + chatRoomId, function (data) {
                GetChatRoom(nickNameText);
            })
        });

        LoadChat.on("click", function () {
            nickNameText = NickName.val();

            ClearChatWindow();

            ChatArea.html("Loading Chat ...");

            StartChatInterval();         

            getChatRoomsInterval = setInterval(function () {
                GetChatRooms();
            }, 1000); 


            ShowChatScreen();
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

        function CheckSession()
        {
            $.get(webserviceUrl + "/CheckSession?roomMode=" + roomMode, function (data) {
                console.log(data);
                ShowAlertMessage("");                
                if (data == null || data.NickName == null || data.NickName == "") {                
                    TimeoutChat();
                }
                else{
                    ShowChatScreen();
                    NickName.val(data.NickName);
                    LoadChat[0].click();
                }
            });        
        }

        function ClearChatWindow()
        {
            ChatRoomUsers.text("");
            PublicChatRooms.text("");
            PrivateChatRooms.text("");
            ChatRoomName.text("");     
            ChatArea.text("");
        }

        function StartChatInterval()
        {
            getChatInterval = setInterval(function () {
                GetChatRoom(nickNameText);
            }, 1000); 
        }

        function SwitchChatRoom(id, name, mode)
        {
            console.log(arguments);
            console.log("Switching chat room ...");
            if(getChatInterval != null)
            {
                clearInterval(getChatInterval);
            }       
            
            chatRoomId = id;
            chatRoomName = name;            
            roomMode = mode;   

            StartChatInterval();
        }


        function SendChatMessage(chatRoomId, message) {
            message = encodeURI(message);
            $.post(webserviceUrl + "/SendMessage", { chatRoomId: chatRoomId, message: message } , function (data) {
                ChatMessage.val("");
                GetChatRoom(nickNameText);                
            })
        }


        function ShowAlertMessage(message)
        {
            AlertMessages.text(message);
            AlertMessages.show();
        }

        function ShowLoginScreen()
        {            
            LoginScreen.show();
            ChatScreen.hide();
        }

        function ShowChatScreen()
        {
            ShowAlertMessage("");
            LoginScreen.hide();
            ChatScreen.show();
        }

        function ScrollToBottom()
        {
            ChatArea.scrollTop(ChatArea[0].scrollHeight);
        }

        function TimeoutChat()
        {            
            chatRoomName="";
            chatRoomId="";
            clearInterval(getChatInterval);            
            clearInterval(getChatRoomsInterval);   
            ShowLoginScreen();
            ShowAlertMessage("You have been logged out due to inactivity");
        }

        function _GetChatRoom(roomMode, ChatRoomElem)
        {
            $.get(webserviceUrl + "/GetChatRooms?roomMode="+roomMode, function (data) {  
                var currentChatRoomsText = ChatRoomElem.html();
                var newChatRoomsText = "";                
                $(data).each(function (index, elem) {                              
                    var additionalClasses = "";

                    if(elem.ChatRoomID == chatRoomId)
                    {
                        additionalClasses = "active";
                    }                    
                    
                    if(elem.LastChatUserNickName != nickNameText && elem.LastChatUserNickName != null)
                    {
                        additionalClasses +=" newMessage";
                    }

                    newChatRoomsText = newChatRoomsText + '<span class="' + roomMode + 'ChatRoomEntry chatRoomEntry ' + additionalClasses + '" data-chatroomid="' + elem.ChatRoomID + '" data-chatroommode="' + elem.ChatRoomMode + '" data-lastchatusernickname="' + elem.LastChatUserNickName + '" data-chatmessagescount="' + elem.ChatMessagesCount + '">' + elem.ChatRoomName + '<span class="deleteRoom" data-chatroomid="' + elem.ChatRoomID + '">x</span></span>';                    
                });

                if (currentChatRoomsText != newChatRoomsText)
                {
                    ChatRoomElem.html("");
                    ChatRoomElem.html(newChatRoomsText);
                }                
            });
        }

        function GetChatRooms()
        {
            if(initialRoomMode == "Public")
            {
                _GetChatRoom("Public", PublicChatRooms);
                _GetChatRoom("Private", PrivateChatRooms);
            }
        }

        function GetChatRoom(nickname)
        {
            var scrollToBottom = false;            
            var maxScrollHeight = ChatArea.prop('scrollHeight') - ChatArea.innerHeight();

            if (ChatArea.scrollTop() == maxScrollHeight)
            {
                scrollToBottom = true;
            }

            $.get(webserviceUrl + "/GetOrCreateChatRoom?NickName=" + nickname + "&roomMode="+roomMode + "&chatRoomId="+chatRoomId+"&chatRoomName="+chatRoomName, function (data) {                
                
                if (data == null)
                {
                    TimeoutChat();
                    return;
                }                                                              

                chatRoomId = data.ChatRoomID;
                chatRoomName = data.ChatRoomName;

                ChatRoomName.text(chatRoomName);

                var currentChatRoomUsersText = ChatRoomUsers.html();
                var newChatRoomUsersText = "";

                $(data.CurrentUsers).each(function (index, elem) {     
                    newChatRoomUsersText = newChatRoomUsersText + '<span class="chatRoomUserEntry" data-chatroomuserid="'+elem.ChatUserID+'">'+elem.NickName+'</span>';                    
                });

                
                if (currentChatRoomUsersText != newChatRoomUsersText)
                {
                    ChatRoomUsers.html("");
                    ChatRoomUsers.html(newChatRoomUsersText);
                }


                var currentChatAreaText = ChatArea.html();
                var newChatAreaText = "";
                
                $(data.ChatMessages).each(function (index, elem) {                    
                    
                    if (elem.MessageMode == 1)
                    {
                        newChatAreaText = newChatAreaText + '<span class="chatMessageEntry"><span class="chatMessageEntryNickName">' + elem.ChatUser.NickName + '</span>' + decodeURI(elem.Message) + '</span>';
                    }
                    else if (elem.MessageMode == 0)
                    {
                        newChatAreaText = newChatAreaText + '<span class="chatMessageEntry systemMessage">' + elem.Message + '</span>';
                    }                          
                });

                if (currentChatAreaText != newChatAreaText)
                {
                    ChatArea.html("");
                    ChatArea.html(newChatAreaText);
                }

                if (scrollToBottom)
                {
                    ScrollToBottom();
                }

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
        height: 100%;
    }

    .deleteRoom {
        float: right;
        font-size: 14px;
        font-weight: bold;
        color:red;
        cursor: pointer;
    }

    #ChatTab {
        text-align:center;
        display: block;
        cursor:pointer;
        background-color: green;
        position:absolute;
        z-index:9999999999;
        bottom:0;
        right:0;
        padding: 5px;
        color: #fff;
    }

    #ChatWindowWrapper {
        border: 1px solid #F8CBB4;
        width: 100%;
        background-color: #F8CBB4;        
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;
        position: absolute;
        bottom:0;
        z-index: 99999999999;
    }

    #ChatWindow {
        display:none;
    }

        #ChatWindowWrapper.Private .SidePanel{
            display:none;
        }

        #ChatWindowWrapper.Private #ChatAreaWrapper{
            width:100%;
            min-height: 500px;
            margin:0;
        }

        #ChatWindowWrapper.Private #ChatAreaWrapper > label {
            display: none;
        }

        #ChatWindowWrapper.Private #ChatArea {
            height: 85%;
        }

    #LoginScreen, #ChatScreen, #AlertMessages {
        display:none;
    }

    #ChatScreen{
        height: 500px;
    }

    
    #ChatArea, .ChatInfoBox {        
        overflow-y:scroll;
        background-color: #F2F2F2;
        padding: 5px;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;
        height: 88%;
    }

    .chatMessageEntry, .chatRoomEntry, .chatRoomUserEntry{
        display: block;
        margin-bottom: 5px;
        background-color: #fff;
        padding: 5px;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;        
        font-size: 11px;
    }

    .chatMessageEntry.systemMessage{
        color: gray;
    }

    .chatMessageEntry .chatMessageEntryNickName{
        display:block;
        color: #DE8400;
    }

    #ChatRoomUsersWrapper, #ChatAreaWrapper {
       float: left;
    }

    #ChatRoomsWrapper {
        float:right;
    }

        #ChatRoomsWrapper > div {
            height: 50%;
        }

    #ChatAreaWrapper {        
        padding:0 5px;
        width: 68%;
    }

    #ChatRoomUsers{
        height: 95%;
    }

    #SendMessageWrapper {
        width: 100%;
    }

    #ChatMessage {
        width: 100%;
    }

    #ChatArea {
        height: 79%;
    }

    .SidePanel {
        width: 15%;        
    }

    .clear {
        clear:both;
    }

    .chatRoomEntry, .chatRoomUserEntry {
        cursor: pointer;        
    }

        .chatRoomEntry.active, .chatRoomUserEntry.active {
            background-color: #4C9689;
        }

    .newMessage::after{
        content:'(New Messages)';
        color: red;
        float: right;
        font-weight: bold;        
        background-color: yellow;
    }

    .flexContainer {
        display: flex;        
        flex-wrap:nowrap;
        justify-content:center;
        align-items: stretch;
        align-content:stretch;
        height: 100%;
    }

        .flexContainer .row {
            flex-direction:row;
        }

        .flexContainer .column {
            flex-direction:column;
        }

</style>

<div id="ChatWindowWrapper" class="<%= ChatRoomMode %>"> 
    <div id="ChatWindow">
        <div id="AlertMessages"></div>

        <div id="LoginScreen">
            <label>Enter your nickname:</label>
            <input type="text" id="NickName" />
            <input id="LoadChat" type="button" value="LoadChat" />
        </div>

        <div id="ChatScreen">
            <div class="field">                
                <div class="flexContainer row">        
                    <div id="ChatRoomUsersWrapper" class="SidePanel">
                        <label>Chat Room Users:</label>
                        <div id="ChatRoomUsers" class="ChatInfoBox"></div>
                    </div>                    
                    <div id="ChatAreaWrapper">
                        <label>Chat Room Name: <span id="ChatRoomName"></span></label>
                        <div id="ChatArea"></div>
                        <div class="field" id="SendMessageWrapper">
                            <textarea id="ChatMessage"></textarea>
                            <input id="SendMessage" type="button" value="Send Message" />
                            <input id="ClearChat" type="button" value="Clear Chat" />
                        </div>                    
                    </div>
                    <div id="ChatRoomsWrapper" class="SidePanel flexContainer column">
                        <div id="PublicChatRoomsWrapper">
                            <label>Public Chatrooms: <a href="javascript:void(0)" id="AddPublicChatRoom">Add</a></label>
                            <div id="PublicChatRooms" class="ChatInfoBox"></div>
                        </div>
                        <div id="PrivateChatRoomsWrapper">
                            <label>Private Chatrooms:</label>
                            <div id="PrivateChatRooms" class="ChatInfoBox"></div>
                        </div>
                    </div>                      
                </div>                
            </div>
        </div>
    </div>        
</div>

<a id="ChatTab" class="openChat">Live Chat</a>