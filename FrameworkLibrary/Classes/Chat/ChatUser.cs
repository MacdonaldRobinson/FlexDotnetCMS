using System;

namespace FrameworkLibrary
{
    public class ChatUser
    {
        public string SessionID { get; set; }
        public string NickName { get; set; }
        public long LoggedInUserID { get; set; }
        public DateTime LastChatMessageDateTime { get; set; }

        public ChatUser(string sessionId, string nickName, long loggedInUserId = 0)
        {
            SessionID = sessionId;
            NickName = nickName;
            LoggedInUserID = loggedInUserId;
        }

        public ChatUser(string sessionId)
        {
            SessionID = sessionId;
        }

        public User GetLoggedInUser()
        {
            if (LoggedInUserID == 0)
                return null;

            var user = UsersMapper.GetByID(LoggedInUserID);
            return user;
        }

    }
}
