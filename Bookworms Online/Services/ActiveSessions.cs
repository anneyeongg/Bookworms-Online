using System.Collections.Generic;

namespace Bookworms_Online.Services
{
    public class ActiveSessions
    {
        private static readonly Dictionary<string, string> UserSessionMap = new Dictionary<string, string>();

        public static bool IsSessionActive(string userId, string sessionId)
        {
            if (userId == null || sessionId == null)
            {
                return false; // Handle the case where userId or sessionId is null
            }

            lock (UserSessionMap)
            {
                if (UserSessionMap.TryGetValue(userId, out var activeSessionId))
                {
                    return activeSessionId == sessionId;
                }

                return false;
            }
        }


        public static void AddActiveSession(string userId, string sessionId)
        {
            lock (UserSessionMap)
            {
                UserSessionMap[userId] = sessionId;
            }
        }

        public static void RemoveActiveSession(string userId)
        {
            lock (UserSessionMap)
            {
                if (UserSessionMap.ContainsKey(userId))
                {
                    UserSessionMap.Remove(userId);
                }
            }
        }
    }
}
