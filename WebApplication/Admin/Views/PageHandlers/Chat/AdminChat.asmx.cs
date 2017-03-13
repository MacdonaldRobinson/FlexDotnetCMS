using FrameworkLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace WebApplication.Admin.Views.PageHandlers.Chat
{
    /// <summary>
    /// Summary description for AdminChat
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class AdminChat : WebApplication.Services.BaseService
    {
        private ChatManager ChatManager { get; } = ChatManager.GetInstance();

        [WebMethod(EnableSession = true)]
        public void GetChatRooms(RoomMode roomMode = RoomMode.Public)
        {
            IEnumerable<IChatRoom> chatRooms = new List<IChatRoom>();

            if(roomMode == RoomMode.Private)
            {
                if(FrameworkSettings.CurrentUser != null)
                {
                    chatRooms = ChatManager.GetChatRooms(roomMode);
                }
            }
            else
            {
                chatRooms = ChatManager.GetChatRooms(roomMode);
            }

            WriteJSON(StringHelper.ObjectToJson(chatRooms));
        }

        [WebMethod(EnableSession = true)]
        public void JoinChatRoom(string chatRoomId)
        {
            var chatRoom = ChatManager.GetChatRoomByID(new Guid(chatRoomId));
            var chatUser = new ChatUser(Session.SessionID);

            if(FrameworkSettings.CurrentUser != null)
            {
                chatUser = new ChatUser(Session.SessionID, FrameworkSettings.CurrentUser.FirstName + " " +FrameworkSettings.CurrentUser.LastName, FrameworkSettings.CurrentUser.ID);                
            }

            if(chatRoom.ChatRoomMode == RoomMode.Private)
            {
                if(FrameworkSettings.CurrentUser != null)
                {
                    chatRoom.JoinChatRoom(chatUser);
                }
            }            
            else
            {
                chatRoom.JoinChatRoom(chatUser);
            }
        }

    }
}
