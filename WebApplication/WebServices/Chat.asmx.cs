using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using FrameworkLibrary;
using System.Web.Script.Services;

namespace WebApplication.Services
{
    /// <summary>
    /// Summary description for Chat
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
    [System.Web.Script.Services.ScriptService]
    public class Chat : BaseService
    {
        private string _chatRoomName = "General";
        private ChatManager ChatManager { get; } = ChatManager.GetInstance();

        [WebMethod(EnableSession = true)]
        public void GetChatRooms(RoomMode roomMode = RoomMode.Public)
        {
            IEnumerable<IChatRoom> chatRooms = new List<IChatRoom>();

            if(roomMode == RoomMode.Private)
            {
                chatRooms = ChatManager.GetChatRooms(roomMode);

                if (FrameworkSettings.CurrentUser == null)
                {
                    chatRooms = chatRooms.Where(i => i.CurrentUsers.Any(j => j.SessionID == Session.SessionID));
                }                
            }
            else
            {
                chatRooms = ChatManager.GetChatRooms(roomMode);
            }

            WriteJSON(StringHelper.ObjectToJson(chatRooms.Select(i=>new { i.ChatRoomID, i.ChatRoomMode, i.ChatRoomName, i.ChatMessagesCount, i.LastChatUserNickName })));
        }

        [WebMethod(EnableSession = true)]
        public void CheckSession(RoomMode roomMode)
        {            
            var chatUser = ChatManager.ChatUsers.FirstOrDefault(i=>i.SessionID == Session.SessionID);
            WriteJSON(StringHelper.ObjectToJson(chatUser));
        }

        [WebMethod(EnableSession = true)]
        public void CreatePrivateChatRoomWith(string otherChatUserId)
        {
            var me = ChatManager.ChatUsers.FirstOrDefault(i=>i.SessionID == Session.SessionID);
            var chatUserId = new Guid(otherChatUserId);
            var otherUser = ChatManager.ChatUsers.FirstOrDefault(i=>i.ChatUserID == chatUserId);
            IChatRoom chatroom = null;

            if(me != null && otherUser != null && me.ChatUserID != otherUser.ChatUserID && me.NickName != otherUser.NickName)
            {
                var foundChatRoom = ChatManager.GetChatRoomWithUsers(RoomMode.Private, new List<ChatUser>() { me, otherUser });

                chatroom = ChatManager.GetOrCreateChatRoom(RoomMode.Private, me.NickName+","+otherUser.NickName, me);
                chatroom.JoinChatRoom(otherUser);
            }

            WriteJSON(StringHelper.ObjectToJson(chatroom));
        }

        [WebMethod(EnableSession = true)]
        public void GetOrCreateChatRoom(string nickName, string chatRoomId, string chatRoomName = "", RoomMode roomMode = RoomMode.Public)
        {
            long userId = 0;
            if (FrameworkSettings.CurrentUser != null)
            {
                userId = FrameworkSettings.CurrentUser.ID;
            }

            var chatUser = new ChatUser(Session.SessionID, nickName, userId);
            IChatRoom chatRoom = null;

            if(chatRoomId != "" && chatRoomId != "0" && chatRoomId != "null")
            {
                chatRoom = ChatManager.GetChatRoomByID(new Guid(chatRoomId));

                if (chatRoom != null && chatRoom.ChatRoomMode != roomMode)
                {
                    chatRoom = null;
                }
            }

            if(chatRoom == null)
            {
                if(chatRoomName == "")
                {
                    if (roomMode == RoomMode.Public)
                    {
                        chatRoomName = _chatRoomName;
                    }
                    else if(roomMode == RoomMode.Private)
                    {
                        if (nickName != "")
                        {
                            chatRoomName = nickName;
                        }
                        else
                        {
                            chatRoomName = Session.SessionID;                            
                        }
                    }
                }

                chatRoom = ChatManager.GetOrCreateChatRoom(roomMode, chatRoomName, chatUser);
            }
            else
            {
                var foundUserInChatRoom = chatRoom.GetUserInChatRoom(chatUser);

                if(foundUserInChatRoom == null)
                {
                    var foundUserWithNickName = chatRoom.CurrentUsers.FirstOrDefault(i=>i.NickName == chatUser.NickName);

                    if(foundUserWithNickName == null)
                    {
                        chatRoom.JoinChatRoom(chatUser);
                    }
                    else
                    {
                        chatRoom = null;
                    }
                }     
                else if(!foundUserInChatRoom.IsActive)
                {
                    chatRoom.RemoveUser(foundUserInChatRoom);

                    var chatRooms = ChatManager.GetChatRooms(roomMode).ToList();

                    foreach (var room in chatRooms)
                    {
                        room.RemoveUser(foundUserInChatRoom);

                        if(room.CurrentUsers.Count == 0)
                        {
                            ChatManager.DeleteChatRoom(room.ChatRoomID);
                        }
                    }

                    chatRoom = null;
                }
            }

            WriteJSON(StringHelper.ObjectToJson(chatRoom));
        }

        [WebMethod(EnableSession = true)]
        public void ClearChatRoom(string chatRoomId)
        {
            var chatUser = new ChatUser(Session.SessionID);
            var chatRoom = ChatManager.GetChatRoomByID(new Guid(chatRoomId));
            var foundUser = chatRoom.GetUserInChatRoom(chatUser);

            if(foundUser != null)
            {
                chatRoom.DeleteChatMessages();
            }
        }

        [ScriptMethod(UseHttpGet = false)]
        [WebMethod(EnableSession = true)]
        public void SendMessage(string chatRoomId, string message)
        {
            IChatRoom chatRoom = null;

            if(chatRoomId != "" && chatRoomId != "0" && chatRoomId != "null")
            {
                chatRoom = ChatManager.GetChatRoomByID(new Guid(chatRoomId));

                if(chatRoom != null)
                {
                    var chatUser = new ChatUser(Session.SessionID);

                    if (chatRoom.GetUserInChatRoom(chatUser) != null)
                    {
                        chatRoom.AddMessage(chatUser, message);
                    }
                }
            }

            WriteJSON(StringHelper.ObjectToJson(chatRoom));
        }
    }
}
