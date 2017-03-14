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

        $(document).on("click", ".chatRoomEntry" , function(){
            var id = $(this).attr("data-chatroomid");               
            SwitchChatRoom(id);
        });

        $(document).on("click", ".chatRoomUserEntry" , function(){
            var otherChatUserId = $(this).attr("data-chatroomuserid");

            $.get(webserviceUrl + "/CreatePrivateChatRoomWith?otherChatUserId=" + otherChatUserId, function (data) {                          
                if(data != null)
                {
                    SwitchChatRoom(data.ChatRoomID);
                }
            })
        });
        

        AddPublicChatRoom.on("click", function(){
            clearInterval()
            var newChatRoomName = prompt("Enter a name for the chatroom");

            if(newChatRoomName != null)
            {
                chatRoomName = newChatRoomName;
                chatRoomId = 0;
                roomMode = "Public";                
                SwitchChatRoom(chatRoomId);
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
                ShowAlertMessage("");                
                if (data == null || data.NickName == null || data.NickName == "") {                
                    TimeoutChat();
                }
                else{
                    ShowChatScreen();
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

        function SwitchChatRoom(id)
        {
            console.log("Switching chat room ...");
            if(getChatInterval != null)
            {
                clearInterval(getChatInterval);
            }       
            
            chatRoomId = id;

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

                    console.log(elem);
                    
                    if(elem.LastChatUserNickName != nickNameText && elem.LastChatUserNickName != null)
                    {
                        additionalClasses +=" newMessage";
                    }

                    newChatRoomsText = newChatRoomsText + '<span class="'+roomMode+'ChatRoomEntry chatRoomEntry '+additionalClasses+'" data-chatroomid="'+elem.ChatRoomID+'" data-chatroommode="'+elem.ChatRoomMode+'" data-lastchatusernickname="'+elem.LastChatUserNickName+'" data-chatmessagescount="'+elem.ChatMessagesCount+'">'+elem.ChatRoomName+'</span>';                    
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
            _GetChatRoom("Public", PublicChatRooms);
            _GetChatRoom("Private", PrivateChatRooms);
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
    }

    #ChatWindowWrapper {
        border: 1px solid #F8CBB4;
        width: 100%;
        background-color: #F8CBB4;
        padding: 10px;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;
    }

    #LoginScreen, #ChatScreen, #AlertMessages {
        display:none;
    }

    
    #ChatArea, .ChatInfoBox {
        height: 300px;
        overflow-y:scroll;
        background-color: #F2F2F2;
        padding: 5px;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;
    }

    .chatMessageEntry, .chatRoomEntry, .chatRoomUserEntry{
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

    #ChatRoomUsersWrapper, #ChatRoomsWrapper, #ChatAreaWrapper {
       float: left;
    }

    #ChatAreaWrapper {        
        margin-left: 10px;
        margin-right: 10px;
        width: 68%;
    }

    #ChatRoomUsers{
        height: 500px;
    }

    #SendMessageWrapper {
        width: 100%;
    }

    #ChatMessage {
        width: 100%;
    }

    #ChatArea {
        height: 555px;
    }

    .SidePanel {
        width: 15%;
        max-width:300px;
        min-width:200px;
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

    @keyframes Glow {
        from {background-color: #4C9689;}
        to {background-color: yellow;}
    }

    .chatRoomEntry.newMessage {
        animation-name: Glow;
        animation-duration: 1s;
        animation-timing-function: linear;        
        animation-iteration-count: infinite;
        animation-direction: alternate;
        background-color: red;
    }

</style>

<div id="ChatWindowWrapper"> 
    <div id="ChatWindow">
        <div id="AlertMessages"></div>

        <div id="LoginScreen">
            <label>Enter your nickname:</label>
            <input type="text" id="NickName" />
            <input id="LoadChat" type="button" value="LoadChat" />
        </div>

        <div id="ChatScreen">
            <div class="field">
                <label>Welcome to LIVE Chat</label>
                <div>        
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
                    <div id="ChatRoomsWrapper" class="SidePanel">
                        <div id="PublicChatRoomsWrapper">
                            <label>Public Chatrooms: <a href="javascript:void(0)" id="AddPublicChatRoom">Add</a></label>
                            <div id="PublicChatRooms" class="ChatInfoBox"></div>
                        </div>
                        <div id="PrivateChatRoomsWrapper">
                            <label>Private Chatrooms:</label>
                            <div id="PrivateChatRooms" class="ChatInfoBox"></div>
                        </div>
                    </div>  
                    <div class="clear"></div>
                </div>                
            </div>
        </div>
    </div>
</div>