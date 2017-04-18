using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Amazon.CloudFront.Model;
using Amazon.S3;
using Amazon.S3.Model;
using Azon.Helpers.Extensions;
using Nancy;
using Nancy.ModelBinding;
using PowerUp.RemoteAgent.Configuration;

namespace PowerUp.RemoteAgent.Web
{
    public class IndexModule : NancyModule
    {
        // private ILog _log;

        public IndexModule()
        {
            Get["/"] = _ =>
            {
                var response = (Response)"Listening...";
                response.ContentType = "text/plain";
                return response;
            };

            Post["/", true] = async (x, ct) =>
            {
                DeployResult result = await DoDeploy(x, ct);
                var response = (Response)result.Content;
                if (!result.Succeeded)
                    response.StatusCode = HttpStatusCode.InternalServerError;

                response.ContentType = "text/plain";
                return response;
            };
        }

        private async Task<DeployResult> DoDeploy(dynamic parameters, CancellationToken ct)
        {
            var publishRequest = this.Bind<PublishRequest>();

            if (!publishRequest.SecurityKey.Equals(Globals.Settings.Security.Key))
                throw new InvalidArgumentException("Security Key");

            var archive = await Task.Run(() => DownloadFile(publishRequest), ct);
            var extracted = await Task.Run(() => ExtractFile(archive), ct);
            var output = await Task.Run(() => Deploy(publishRequest, extracted), ct);

            return output;
        }

        private DeployResult Deploy(PublishRequest publishRequest, DirectoryInfo directory)
        {
            var deploymentScriptFilename = publishRequest.DeploymentScriptFilename;
            if (string.IsNullOrWhiteSpace(deploymentScriptFilename))
            {
                deploymentScriptFilename = "deploy.bat";
            }
            var args = "/C {0} {1}".FormatWith(deploymentScriptFilename, publishRequest.DeploymentProfile);

            //Note AppPool must run as an account with elevated permissions for this to work
            var processStartInfo = new ProcessStartInfo("cmd.exe")
            {
                WorkingDirectory = directory.FullName,
                Arguments = args,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var proc = Process.Start(processStartInfo);
            var output = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            return new DeployResult
            {
                Content = output,
                Succeeded = proc.ExitCode == 0
            };
        }

        private DirectoryInfo ExtractFile(FileInfo archiveInfo)
        {
            var dirToExtract = Path.Combine(Globals.Settings.RemoteAgent.DeployTemp, archiveInfo.Name);
            var directory = EnsureFolder(dirToExtract);

            using (var zipFileToOpen = new FileStream(archiveInfo.FullName, FileMode.Open))
            using (var archive = new ZipArchive(zipFileToOpen, ZipArchiveMode.Read))
            {
                archive.ExtractToDirectory(dirToExtract);
            }

            return directory;
        }

        private FileInfo DownloadFile(PublishRequest publishRequest)
        {
            var bucketName = publishRequest.BucketName;
            var config = Globals.Settings.Proxy.UseProxy
                ? new AmazonS3Config
                {
                    ProxyHost = Globals.Settings.Proxy.Host,
                    ProxyPort = int.Parse(Globals.Settings.Proxy.Port)
                }
                : new AmazonS3Config();


            var key = publishRequest.Filename.TrimStart(new[] { '/' });
            var temp = Path.GetTempFileName();

            using (var client = new AmazonS3Client(publishRequest.AccessKey, publishRequest.SecretKey, config))
            {
                var getObjectRequest = new GetObjectRequest().WithBucketName(bucketName).WithKey(key);

                using (S3Response getObjectResponse = client.GetObject(getObjectRequest))
                using (var fs = new FileStream(temp, FileMode.OpenOrCreate))
                using (var s = getObjectResponse.ResponseStream)
                {
                    s.CopyTo(fs);
                }
            }

            return RenameFile(new FileInfo(temp), key);
        }

        private FileInfo RenameFile(FileInfo file, string newFileName)
        {
            var newPathAndFileName = Path.Combine(file.Directory.FullName, newFileName);

            if (File.Exists(newPathAndFileName))
                File.Delete(newPathAndFileName);

            file.MoveTo(newPathAndFileName);

            return new FileInfo(newPathAndFileName);
        }

        private DirectoryInfo EnsureFolder(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            if (dirInfo.Exists)
                Directory.Delete(path, true);

            Directory.CreateDirectory(path);
            return new DirectoryInfo(path);
        }

        private class DeployResult
        {
            public bool Succeeded { get; set; }
            public string Content { get; set; }
        }
    }
}