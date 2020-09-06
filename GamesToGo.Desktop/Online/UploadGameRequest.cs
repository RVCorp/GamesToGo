using System.IO;
using System.Linq;
using System.Net.Http;
using GamesToGo.Desktop.Project;
using Ionic.Zip;
using Newtonsoft.Json;
using osu.Framework.IO.Network;
using osu.Framework.Platform;

namespace GamesToGo.Desktop.Online
{
    public class UploadGameRequest : APIRequest<UploadGameResult>
    {
        private ProjectInfo project;
        private Storage store;

        public UploadGameRequest(ProjectInfo project, Storage store)
        {
            this.project = project;
            this.store = store.GetStorageForDirectory("files");
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            using ZipFile file = new ZipFile($"{project.File.NewName}.zip");
            using (var projectFileMemoryStream = new MemoryStream())
            {
                using (var projectFileStream = store.GetStream(project.File.NewName))
                    projectFileStream.CopyTo(projectFileMemoryStream);
                file.AddEntry(project.File.NewName, projectFileMemoryStream.ToArray());
            }
            foreach (var projectFile in project.Relations.Select(fr => fr.File))
            {
                using (var fileMemoryStream = new MemoryStream())
                {
                    using (var fileStream = store.GetStream(projectFile.NewName))
                        fileStream.CopyTo(fileMemoryStream);
                    file.AddEntry(projectFile.NewName, fileMemoryStream.ToArray());
                }
            }

            using (var ms = new MemoryStream())
            {
                file.Save(ms);

                ms.Seek(0, SeekOrigin.Begin);
                ms.Flush();

                req.Method = HttpMethod.Post;
                req.AddParameter("ID", (project.OnlineProjectID == 0 ? -1 : project.OnlineProjectID).ToString());
                req.AddParameter("Name", project.Name);
                req.AddParameter("description", project.Description);
                req.AddParameter("minP", project.MinNumberPlayers.ToString());
                req.AddParameter("maxP", project.MaxNumberPlayers.ToString());
                req.AddParameter("imageName", project.ImageRelation?.File?.NewName ?? "null");
                req.AddParameter("LastEdited", project.LastEdited.ToUniversalTime().ToString("yyyyMMddHHmmssfff"));
                req.AddParameter("FileName", project.File.NewName);
                req.AddFile(@"File", ms.ToArray());
            }

            return req;
        }
        protected override string Target => "Games/UploadFile";
    }

    public class UploadGameResult
    {
        [JsonProperty(@"id")]
        public int OnlineID { get; set; }
    }
}
