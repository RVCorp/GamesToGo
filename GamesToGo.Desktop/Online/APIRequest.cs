using System;
using osu.Framework.IO.Network;
using osu.Framework.Logging;

namespace GamesToGo.Desktop.Online
{
    public abstract class APIRequest<T> : APIRequest where T : class
    {
        protected override WebRequest CreateWebRequest() => new BaseJsonWebRequest<T>(Uri);

        public T Result { get; private set; }

        /// <summary>
        /// Invoked on successful completion of an API request.
        /// This will be scheduled to the API's internal scheduler (run on update thread automatically).
        /// </summary>
        public new event APISuccessHandler<T> Success;

        protected override void PostProcess()
        {
            base.PostProcess();
            Result = ((BaseJsonWebRequest<T>)WebRequest)?.ResponseObject;
        }

        internal void TriggerSuccess(T result)
        {
            if (Result != null)
                throw new InvalidOperationException("Attempted to trigger success more than once");

            Result = result;

            TriggerSuccess();
        }

        internal override void TriggerSuccess()
        {
            base.TriggerSuccess();
            Success?.Invoke(Result);
        }
    }
    public abstract class APIRequest
    {
        protected abstract string Target { get; }

        protected virtual WebRequest CreateWebRequest() => new BaseWebRequest(Uri);

        protected virtual string Uri => $@"{API.Endpoint}/api/{Target}";

        protected APIController API;
        protected WebRequest WebRequest;

        protected User User { get; private set; }

        public event APISuccessHandler Success;

        public event APIFailureHandler Failure;

        private bool cancelled;

        private Action pendingFailure;

        public void Perform(APIController api)
        {
            API = api;
            User = api.LocalUser.Value;

            if (checkAndScheduleFailure())
                return;

            WebRequest = CreateWebRequest();
            WebRequest.Failed += Fail;
            WebRequest.AllowRetryOnTimeout = false;
            WebRequest.AddHeader("Authorization", $"Bearer {API.Token}");

            if (checkAndScheduleFailure())
                return;

            if (!WebRequest.Aborted) // could have been aborted by a Cancel() call
            {
                Logger.Log($@"Performing request {this}", LoggingTarget.Network);
                WebRequest.Perform();
            }

            if (checkAndScheduleFailure())
                return;

            PostProcess();

            API.Schedule(delegate
            {
                if (cancelled) return;

                TriggerSuccess();
            });
        }

        /// <summary>
        /// Perform any post-processing actions after a successful request.
        /// </summary>
        protected virtual void PostProcess()
        {
        }

        internal virtual void TriggerSuccess()
        {
            Success?.Invoke();
        }

        public void Cancel() => Fail(new OperationCanceledException(@"Request cancelled"));

        public void Fail(Exception e)
        {
            if (WebRequest?.Completed == true)
                return;

            if (cancelled)
                return;

            cancelled = true;
            WebRequest?.Abort();

            string responseString = WebRequest?.GetResponseString();

            Logger.Log($@"Failing request {this} ({e})", LoggingTarget.Network);
            pendingFailure = () => Failure?.Invoke(e);
            checkAndScheduleFailure();
        }

        private bool checkAndScheduleFailure()
        {
            if (API == null || pendingFailure == null) return cancelled;

            API.Schedule(pendingFailure);
            pendingFailure = null;
            return true;
        }
    }

    public delegate void APIFailureHandler(Exception e);

    public delegate void APISuccessHandler();

    public delegate void APIProgressHandler(long current, long total);

    public delegate void APISuccessHandler<in T>(T content);
}
