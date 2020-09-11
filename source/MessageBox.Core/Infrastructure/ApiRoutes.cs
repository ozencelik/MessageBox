﻿namespace MessageBox.Core.Infrastructure
{
    public static class ApiRoutes
    {
        public const string Root = "api";

        public const string Version = "v1";

        public const string Base = Root + "/" + Version;

        public static class Users
        {
            public const string Create = Base + "/users";

            public const string Delete = Base + "/users/{userId}";

            public const string Get = Base + "/users/{userId}";

            public const string GetAll = Base + "/users";

            public const string Login = Base + "/users/login";

            public const string Register = Base + "/users/register";

            public const string Update = Base + "/users/{userId}";
        }

        public static class Logs
        {
            public const string Get = Base + "/logs/{logId}";

            public const string GetAll = Base + "/logs";
        }

        public static class Messages
        {
            public const string Get = Base + "/messages/{messageId}";

            public const string GetAll = Base + "/messages";

            public const string GetAllUnRead = Base + "/messages/unread";

            public const string Send = Base + "/messages";
        }

        public static class BlockedUsers
        {
            public const string Create = Base + "/blockedusers";

            public const string Delete = Base + "/blockedusers/{userId}";

            public const string Get = Base + "/blockedusers/{userId}";

            public const string GetAllBlockingUser = Base + "/blockedusers/blockingusers";

            public const string GetAllBlockedUser = Base + "/blockedusers";

            public const string Block = Base + "/blockedusers/block";
        }
    }
}
