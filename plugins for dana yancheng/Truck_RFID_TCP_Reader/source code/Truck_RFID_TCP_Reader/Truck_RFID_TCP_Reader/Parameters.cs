using Desoutter.ProcessControl.Plugin.v2.Interface.AttributeParameter;

namespace Truck_RFID_TCP_Reader
{
    [PluginParameters]
    class Parameters
    {
        [PluginParameter("IP", "ip address of rfid ")]
        public string IP { get; set; }

        [PluginParameter("Port", "port number of rfid")]
        public string Port { get; set; }

    }
}
