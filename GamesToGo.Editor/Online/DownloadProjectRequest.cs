using System.IO;
using osu.Framework.IO.Network;
using osu.Framework.Platform;

namespace GamesToGo.Editor.Online
{
    public class DownloadProjectRequest : APIRequest
    {
        private readonly int projectID;
        private readonly Storage store;
        private readonly string filename;

        public DownloadProjectRequest(int id, string hash, Storage store)
        {
            projectID = id;
            filename = Path.Combine("download", $"{hash}.zip");
            this.store = store;
            base.Success += onSuccess;
        }

        protected override WebRequest CreateWebRequest()
        {
            var fullPath = store.GetFullPath(filename, true);
            if (store.Exists(filename))
                File.Delete(fullPath);
            using (File.Create(fullPath))
            { }

            var request = new FileWebRequest(fullPath, Uri);
            request.DownloadProgress += request_Progress;
            return request;
        }

        private void request_Progress(long current, long total) => API.Schedule(() => Progressed?.Invoke((float)current / total));

        private void onSuccess()
        {
            Success?.Invoke(filename);
        }

        public event APIProgressHandler Progressed;

        public new event APISuccessHandler<string> Success;

        protected override string Target => @$"Games/DownloadProject/{projectID}";
    }
}
