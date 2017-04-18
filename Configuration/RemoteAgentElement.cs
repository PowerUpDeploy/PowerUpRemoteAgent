using System.Configuration;

namespace PowerUp.RemoteAgent.Configuration
{
    public class RemoteAgentElement : ConfigurationElement
    {
        [ConfigurationProperty("deployTemp", DefaultValue = "C:\\DeployTemp")]
        public virtual string DeployTemp
        {
            get { return (string)base["deployTemp"]; }
            set { base["deployTemp"] = value; }
        }
    }
}