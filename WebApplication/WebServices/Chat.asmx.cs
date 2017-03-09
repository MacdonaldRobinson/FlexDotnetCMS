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
        private string _chatRoomName = "Public Chat Room";
        private ChatManager ChatManager { get; } = ChatManager.GetInstance();

        [WebMethod(EnableSession = true)]
        public void GetChatUserBySession()
        {
            var chatUser = new ChatUser(Session.SessionID);
            var publicChatRoom = ChatManager.GetOrCreateChatRoom(RoomMode.Public, _chatRoomName, chatUser);

            if (publicChatRoom != null)
            {
                var foundUser = publicChatRoom.GetUserInChatRoom(chatUser);

                if (foundUser != null)
                {
                    chatUser = foundUser;
                }
            }

            WriteJSON(StringHelper.ObjectToJson(chatUser));
        }

        [WebMethod(EnableSession = true)]
        public void GetPublicChatRoom(string nickName)
        {
            long userId = 0;
            if (FrameworkSettings.CurrentUser != null)
            {
                userId = FrameworkSettings.CurrentUser.ID;
            }

            var chatUser = new ChatUser(Session.SessionID, nickName, userId);
            var publicChatRoom = ChatManager.GetOrCreateChatRoom(RoomMode.Public, _chatRoomName, chatUser);

            WriteJSON(StringHelper.ObjectToJson(publicChatRoom));
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
            var chatRoom = ChatManager.GetChatRoomByID(new Guid(chatRoomId));
            var chatUser = new ChatUser(Session.SessionID);

            if (chatRoom.GetUserInChatRoom(chatUser) != null)
            {
                chatRoom.AddMessage(chatUser, message);
            }

            WriteJSON(StringHelper.ObjectToJson(chatRoom));
        }
    }
}
