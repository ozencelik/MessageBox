namespace MessageBox.Core.Infrastructure
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Users
        {
            public const string Delete = Base + "/users";

            public const string Get = Base + "/users";

            public const string Login = Base + "/users/login";

            public const string Register = Base + "/users/register";

            public const string Update = Base + "/users";
        }

        public static class ActivityLogs
        {
            public const string Get = Base + "/activity-logs/{activity-log-id}";

            public const string GetAll = Base + "/activity-logs";
        }
        
        public static class Logs
        {
            public const string Get = Base + "/logs/{log-id}";

            public const string GetAll = Base + "/logs";
        }

        public static class Messages
        {
            public const string Delete = Base + "/messages/{message-id}";

            public const string Get = Base + "/messages/{message-id}";

            public const string GetAll = Base + "/messages";

            public const string GetAllUnRead = Base + "/messages/unread";

            public const string Send = Base + "/messages";

            public const string Update = Base + "/messages/{message-id}";
        }

        public static class BlockedUsers
        {
            public const string Get = Base + "/blocked-users";

            public const string GetAllBlockedUser = Base + "/blocked-users";

            public const string Block = Base + "/blocked-users/block";

            public const string UnBlock = Base + "/blocked-users/un-block";
        }
    }
}
