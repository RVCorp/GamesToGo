using System;
using System.Collections.Generic;
using System.Text;
using osu.Framework.IO.Network;

namespace GamesToGo.Desktop.Online
{
    class UploadUserImageRequest : APIRequest
    {
        private string name;
        private byte[] Image;

        public UploadUserImageRequest(byte[] image)
        {
            name = "salchipapa";
            Image = image;
        }

        protected override WebRequest CreateWebRequest()
        {
            var req = base.CreateWebRequest();
            req.AddParameter("Name", name);
            req.AddFile("File", Image);
            return req;
        }
        protected override string Target => "Users/UploadImage";
    }
}
