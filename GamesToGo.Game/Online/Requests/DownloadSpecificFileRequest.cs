using System.IO;
using GamesToGo.Common.Online.Requests;
using osu.Framework.IO.Network;
using osu.Framework.Platform;

namespace GamesToGo.Game.Online.Requests
{
    public class DownloadSpecificFileRequest : APIRequest
    {
        private readonly string hash;
        private readonly string filename;
        private readonly Storage store;

        public DownloadSpecificFileRequest(string hash, Storage store)
        {
            this.hash = hash;
            filename = Path.Combine("files", $"{hash.ToUpper()}");
            this.store = store;
        }

        protected override WebRequest CreateWebRequest()
        {
            var fullPath = store.GetFullPath(filename, true);
            if(store.Exists(filename))
                File.Delete(fullPath);
            using (File.Create(fullPath))
            { }

            var request = new FileWebRequest(fullPath, Uri);
            request.DownloadProgress += request_Progress;
            return request;
        }

        private void request_Progress(long current, long total) => API.Schedule(() => Progressed?.Invoke((float)current / total));

        public event APIProgressHandler Progressed;

        protected override string Target => $"Games/DownloadFile/{hash}";
    }
}
