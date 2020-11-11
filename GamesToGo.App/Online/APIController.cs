using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using Newtonsoft.Json;
using osu.Framework.Bindables;
using osu.Framework.Graphics;

namespace GamesToGo.App.Online
{
    public class APIController : Component
    {
        public string Token;
        private string password;
        private string username;
        private AddUserRequest registrableUser;

        public static string UserAgent { get; } = "gtg";

        public static string Endpoint => @"https://gamestogo.company";

        private readonly Queue<APIRequest> queue = new Queue<APIRequest>();

        public Bindable<User> LocalUser { get; } = new Bindable<User>();

        private APIState State
        {
            get;
            set;
        }

        private bool isLoggedIn => LocalUser.Value?.ID > 0;
        private bool hasLogin => hasValidToken || !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password);

        private bool hasValidToken => !string.IsNullOrEmpty(Token);

        private readonly CancellationTokenSource cancellationToken = new CancellationTokenSource();

        public APIController()
        {
            var thread = new Thread(run)
            {
                Name = "APIController",
                IsBackground = true,
            };

            thread.Start();
        }

        public new void Schedule(Action action) => base.Schedule(action);

        private int failureCount;

        private void run()
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                switch (State)
                {
                    case APIState.Failing:
                        //todo: replace this with a ping request.
                        Thread.Sleep(5000);

                        if (!isLoggedIn) goto case APIState.Connecting;

                        break;

                    case APIState.Offline:
                    case APIState.Connecting:

                        if (registrableUser != null)
                        {
                            handleRequest(registrableUser);
                            registrableUser = null;
                            continue;
                        }

                        if (!hasLogin)
                        {
                            State = APIState.Offline;
                            Thread.Sleep(50);
                            continue;
                        }

                        State = APIState.Connecting;

                        int userID = 0;
                        if (!hasValidToken && (userID = authenticate()) < 0)
                        {
                            Token = null;
                            continue;
                        }

                        var userReq = new GetUserRequest(userID);
                        userReq.Success += u =>
                        {
                            LocalUser.Value = u;

                            failureCount = 0;

                            State = APIState.Online;
                        };

                        if (!handleRequest(userReq))
                        {
                            if (State == APIState.Connecting)
                                State = APIState.Failing;
                            continue;
                        }

                        while (State > APIState.Offline && State < APIState.Online)
                            Thread.Sleep(500);

                        break;
                }

                if (!hasValidToken)
                {
                    Logout();
                    continue;
                }

                while (true)
                {
                    APIRequest req;

                    lock (queue)
                    {
                        if (queue.Count == 0) break;

                        req = queue.Dequeue();
                    }

                    handleRequest(req);
                }

                Thread.Sleep(50);
            }
        }

        private bool handleRequest(APIRequest req)
        {
            try
            {
                req.Perform(this);

                if (isLoggedIn) State = APIState.Online;

                failureCount = 0;
                return true;
            }
            catch (WebException we)
            {
                handleWebException(we);
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private void handleWebException(WebException we)
        {
            HttpStatusCode statusCode = (we.Response as HttpWebResponse)?.StatusCode ?? (we.Status == WebExceptionStatus.UnknownError ? HttpStatusCode.NotAcceptable : HttpStatusCode.RequestTimeout);

            switch (we.Message)
            {
                case "Unauthorized":
                case "Forbidden":
                    statusCode = HttpStatusCode.Unauthorized;
                    break;
            }

            switch (statusCode)
            {
                case HttpStatusCode.Unauthorized:
                    Logout();
                    return;

                case HttpStatusCode.RequestTimeout:
                    failureCount++;

                    if (failureCount < 3) return;

                    if (State != APIState.Online) return;
                    State = APIState.Failing;
                    flushQueue();

                    return;
            }
        }

        private int authenticate()
        {
            if (string.IsNullOrEmpty(username)) return -1;
            if (string.IsNullOrEmpty(password)) return -1;

            using var req = new AccessRequest(username, password)
            {
                Url = $@"{Endpoint}/api/Login",
                Method = HttpMethod.Get,
            };

            try
            {
                req.Perform();
            }
            catch
            {
                return -1;
            }

            Token = req.ResponseObject.Token;
            return req.ResponseObject.ID;
        }

        public void Queue(APIRequest request)
        {
            lock (queue) queue.Enqueue(request);
        }

        private void flushQueue(bool failOldRequests = true)
        {
            lock (queue)
            {
                var oldQueueRequests = queue.ToArray();

                queue.Clear();

                if (!failOldRequests)
                    return;

                foreach (var req in oldQueueRequests)
                    req.Fail(new WebException(@"Disconnected from server"));
            }
        }
        public void Login(string user, string pass)
        {
            username = user;
            password = pass;
        }

        public void Register(AddUserRequest userRequest)
        {
            registrableUser = userRequest;
        }

        public void Logout()
        {
            flushQueue();

            Token = null;
            password = null;
            username = null;

            State = APIState.Offline;
        }

        private class AccessRequest : BaseJsonWebRequest<AuthToken>
        {
            private readonly string username;
            private readonly string password;

            public AccessRequest(string username, string password)
            {
                this.username = username;
                this.password = password;
            }
            protected override void PrePerform()
            {
                AddParameter("username", username);
                AddParameter("pass", password);

                base.PrePerform();
            }
        }
    }

    public class AuthToken
    {
        [JsonProperty(@"token")]
        public string Token;

        [JsonProperty(@"id")]
        public int ID;
    }

    public enum APIState
    {
        Offline,
        Failing,
        Connecting,
        Online,
    }
}
