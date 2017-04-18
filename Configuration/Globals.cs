using System;
using System.Web.Configuration;

namespace PowerUp.RemoteAgent.Configuration
{
    public static class Globals
    {
        public static readonly AgentConfig Settings = GetConfig();

        private static AgentConfig GetConfig()
        {
            var agentSection = WebConfigurationManager.GetSection("Agent") as AgentConfig;
            if (agentSection != null)
                return agentSection;

            throw new Exception("Invalid config - check \"Agent\" is in app/web.config and check for invalid keys");
        }
    }
}