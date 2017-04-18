namespace PowerUp.RemoteAgent.Configuration
{
    public class PublishRequest
    {
        public string SecurityKey { get; set; }
        public string BucketName { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
        public string Filename { get; set; }
        public string DeploymentProfile { get; set; }
        public string DeploymentScriptFilename { get; set; }
    }
}
