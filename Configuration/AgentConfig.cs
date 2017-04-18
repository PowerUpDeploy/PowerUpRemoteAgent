using System.Configuration;

namespace PowerUp.RemoteAgent.Configuration
{
    public class AgentConfig : ConfigurationSection
    {
        [ConfigurationProperty("buildDateStamp", DefaultValue = "N/A")]
        public virtual string BuildDateStamp
        {
            get { return (string)base["buildDateStamp"]; }
            set { base["buildDateStamp"] = value; }
        }

        [ConfigurationProperty("buildNumber", DefaultValue = "0")]
        public virtual int BuildNumber
        {
            get { return (int)base["buildNumber"]; }
            set { base["buildNumber"] = value; }
        }

        [ConfigurationProperty("environment", DefaultValue = "development")]
        public virtual string Environment
        {
            get { return (string)base["environment"]; }
            set { base["environment"] = value; }
        }

        [ConfigurationProperty("proxy", IsRequired = false)]
        public virtual ProxyElement Proxy
        {
            get { return (ProxyElement)base["proxy"]; }
            set { base["proxy"] = value; }
        }

        [ConfigurationProperty("remoteAgent", IsRequired = false)]
        public virtual RemoteAgentElement RemoteAgent
        {
            get { return (RemoteAgentElement)base["remoteAgent"]; }
            set { base["remoteAgent"] = value; }
        }

        [ConfigurationProperty("security", IsRequired = false)]
        public virtual SecurityElement Security
        {
            get { return (SecurityElement)base["security"]; }
            set { base["security"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}
