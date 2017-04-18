using System.Configuration;

namespace PowerUp.RemoteAgent.Configuration
{
    public class SecurityElement : ConfigurationElement
    {
        [ConfigurationProperty("key", DefaultValue = "7662E26F-7A40-4299-9193-F5BBA8067612")]
        public virtual string Key
        {
            get { return (string)base["key"]; }
            set { base["key"] = value; }
        }
    }
}