using System.Windows;
using Desoutter.ProcessControl.Plugin.v2.Interface;
using Desoutter.ProcessControl.Plugin.v2.Interface.AttributeParameter;
using Desoutter.ProcessControl.Plugin.v2.Interface.Model;

namespace EchoSnStatus
{
    [Plugin]
    public class Echo : PluginBase
    {
        DataAcess da = new DataAcess();
        public override FrameworkElement CreateControl()
        {
            return null;
        }

        public override bool HasToCreateControl()
        {
            return false;
        }

        public override StepResult ExecuteStep(object parameters)
        {
            var param = parameters as Parameters;
            da.UpdateSNandProStauts(param.ConnectString, param.ProID, param.SN, param.Status);

            return new StepResult { Data = "I have done update sn status successfully!", IsPassed = true };
        }

    }
}
