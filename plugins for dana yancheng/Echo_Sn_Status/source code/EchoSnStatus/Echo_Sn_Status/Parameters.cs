using Desoutter.ProcessControl.Plugin.v2.Interface.AttributeParameter;

namespace EchoSnStatus
{
    [PluginParameters]
    public class Parameters
    {
        [PluginParameter("Connect string", "Data Source=;Initial Catalog=;Persist Security Info=True;User ID=;Password=")]
        public string ConnectString { get; set; }

        [PluginParameter("PRO ID", "id of proNumber in database")]
        public int ProID { get; set; }

        [PluginParameter("SN", "")]
        public string SN { get; set; }

        [PluginParameter("Status", "0:In progress 1:Completed ")]
        public int Status { get; set; } 
    }
}
