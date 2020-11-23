using System;
using osu.Framework.IO.Network;
using osu.Framework.Logging;

namespace GamesToGo.Editor.Online
{
    public abstract class APIRequest<T> : APIRequest where T : class
    {
        protected override WebRequest CreateWebRequest() => new BaseJsonWebRequest<T>(Uri);

        private T result { get; set; }

        /// <summary>
        /// Invoked on successful completion of an API request.
        /// This will be scheduled to the API's internal scheduler (run on update thread automatically).
        /// </summary>
        public new event APISuccessHandler<T> Success;

        protected override void PostProcess()
        {
            base.PostProcess();
            result = ((BaseJsonWebRequest<T>)WebRequest)?.ResponseObject;
        }

        internal override void TriggerSuccess()
        {
            base.TriggerSuccess();
            Success?.Invoke(result);
        }
    }

    public abstract class APIRequest
    {
        protected abstract string Target { get; }

        protected virtual WebRequest CreateWebRequest() => new BaseWebRequest(Uri);

        protected string Uri => $@"{APIController.Endpoint}/api/{Target}";

        protected APIController API;
        protected WebRequest WebRequest;

        public event APISuccessHandler Success;

        public event APIFailureHandler Failure;

        private bool cancelled;

        private Action pendingFailure;

        public void Perform(APIController api)
        {
            API = api;

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

        public void Fail(Exception e)
        {
            if (WebRequest?.Completed == true)
                return;

            if (cancelled)
                return;

            cancelled = true;
            WebRequest?.Abort();

            WebRequest?.GetResponseString();

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

    public delegate void APIProgressHandler(float current);

    public delegate void APISuccessHandler<in T>(T content);
}
