using System.Configuration;

namespace PowerUp.RemoteAgent.Configuration
{
    public class ProxyElement : ConfigurationElement
    {
        [ConfigurationProperty("host", DefaultValue = "")]
        public virtual string Host
        {
            get { return (string)base["host"]; }
            set { base["host"] = value; }
        }

        [ConfigurationProperty("port", DefaultValue = "80")]
        public virtual string Port
        {
            get { return (string)base["port"]; }
            set { base["port"] = value; }
        }

        [ConfigurationProperty("username", DefaultValue = "")]
        public virtual string Username
        {
            get { return (string)base["username"]; }
            set { base["username"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "")]
        public virtual string Password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }

        [ConfigurationProperty("useProxy", DefaultValue = false)]
        public virtual bool UseProxy
        {
            get { return (bool)base["useProxy"]; }
            set { base["useProxy"] = value; }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}