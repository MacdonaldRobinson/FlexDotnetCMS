using System;

namespace FrameworkLibrary
{
    public class ChatUser
    {
        public Guid ChatUserID { get; } = Guid.NewGuid();
        public string SessionID { get; set; }
        public string NickName { get; set; }
        public long LoggedInUserID { get; set; }
        public DateTime LastChatMessageDateTime { get; set; } = DateTime.Now;

        private TimeSpan _inactiveDuration = TimeSpan.FromMinutes(10);

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

        public bool IsActive
        {
            get
            {
                return DateTime.Now < (LastChatMessageDateTime + _inactiveDuration);
            }            
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
