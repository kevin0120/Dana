using Desoutter.ProcessControl.Plugin.v2.Interface.AttributeParameter;

namespace Plugin
{
    [PluginParameters]
    public class Parameters
    {
        [PluginParameter("Com Port", "Com Port Number")]
        public string ComPortNumber { get; set; }
    }
}
