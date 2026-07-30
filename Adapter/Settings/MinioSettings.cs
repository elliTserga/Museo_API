using System;
using System.Collections.Generic;
using System.Text;

namespace Adapter.Settings
{
    public class MinioSettings
    {

        public string Endpoint { get; set; } = string.Empty;

        public string AccessKey { get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;

        public string BucketName { get; set; } = string.Empty;

        public bool UseSSL { get; set; }

    }
}
