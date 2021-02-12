using System.IO;
using GamesToGo.Common.Online.Requests;
using osu.Framework.IO.Network;
using osu.Framework.Platform;

namespace GamesToGo.Game.Online.Requests
{
    public class DownloadSpecificFile : APIRequest
    {
        private readonly string hash;
        private readonly string filename;
        private readonly Storage store;

        public DownloadSpecificFile(string hash, Storage store)
        {
            this.hash = hash;
            filename = Path.Combine("files", $"{hash.ToUpper()}");
            this.store = store;
            base.Success += onSuccess;
        }

        protected override WebRequest CreateWebRequest()
        {
            var fullPath = store.GetFullPath(filename, true);
            if(store.Exists(filename))
                File.Delete(fullPath);
            using (File.Create(fullPath))
            { }

            var request = new FileWebRequest(fullPath, Uri);

            return request;
        }

        private void onSuccess()
        {
            Success?.Invoke(filename);
        }

        public new event APISuccessHandler<string> Success;

        protected override string Target => $"Games/DownloadFile/{hash}";
    }
}
