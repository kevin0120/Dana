using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Desoutter.ProcessControl.Plugin.v2.Interface;
using Desoutter.ProcessControl.Plugin.v2.Interface.AttributeParameter;
using Desoutter.ProcessControl.Plugin.v2.Interface.Model;
using Plugin.ViewModel;
using Plugin.View;
using GalaSoft.MvvmLight.CommandWpf;
using Plugin.Communication;
using System.Collections.Generic;

namespace Plugin
{
    [Plugin]
    public class Plugin : PluginBase
    {
        private MainViewModel _viewModel;
        private FrameworkElement _view;
        private TaskCompletionSource<StepResult> _taskCompletionSource;
        private CancellationTokenSource _tokenSource;

        // Declare the serial port
        private SerialClient _serialClient;

        private int _counter;

        public override FrameworkElement CreateControl()
        {
            if (_view != null) return _view;

            _view = new MainView();
            _viewModel = new MainViewModel
            {

                DisplayKeyboardCommand = new RelayCommand(DisplayKeyboard),
                ValidateCommand = new RelayCommand(ValidatePluginStep)

            };

            _view.DataContext = _viewModel;

            return _view;
        }

        public override bool HasToCreateControl()
        {
            return true;
        }

        public override StepResult ExecuteStep(object parameters)
        {
            // Fusion parameters
            var param = parameters as Parameters;
          
            // add custom plugin log
            InfinityService.Log.Info($"Fusion Parameter 1 : Serial Com Port = {param?.ComPortNumber}");

            _tokenSource = new CancellationTokenSource();
            _taskCompletionSource = new TaskCompletionSource<StepResult>();

            // Serial port
            _serialClient = new SerialClient(param?.ComPortNumber);
            _serialClient.OpenCom();
            _serialClient.SerialDataReceived += SerialDataReceived;

            Application.Current.Dispatcher.Invoke(() =>
            {
                // Display Fusion parameters
                _viewModel.SerialPortNumber = param?.ComPortNumber;

                _viewModel.KeyboardData = "Keyboard ---";
                _viewModel.SerialDataReceived = "Serial Data ---";

            });

            _taskCompletionSource.Task.Wait(_tokenSource.Token);
            return _taskCompletionSource.Task.Result;
        }

        private void ValidatePluginStep()
        {
            _taskCompletionSource.SetResult(new StepResult { Data = "OK", IsPassed = true });

            StopSerialCommunication();
        }

        private async void DisplayKeyboard()
        {
            _viewModel.KeyboardData = await InfinityService.ExternalInterface.DisplayKeyboardAsync("My Keyboard", "");

            InfinityService.Log.Info($"Keyboard Data = {_viewModel.KeyboardData}");
            InfinityService.Production.InsertTraceabilityLine(true, $"Keyboard Data = {_viewModel.KeyboardData}");

            _serialClient.Send(_viewModel.KeyboardData + "\r\n");
        }

        void SerialDataReceived(object sender, EventArgs e)
        {
            _viewModel.SerialDataReceived = _serialClient.DataReceived;

            InfinityService.Log.Info($"Serial Data Received = {_viewModel.SerialDataReceived}");
            InfinityService.Production.InsertTraceabilityLine(true, $"Serial Data Received = {_viewModel.SerialDataReceived}");
        }

        private void StopSerialCommunication()
        {
            _serialClient.CloseCom();
            _serialClient.SerialDataReceived -= SerialDataReceived;
            _serialClient.Dispose();

            InfinityService.Log.Info($"Serial Port {_viewModel.SerialPortNumber} Closed");
        }

        public override void CycleStart()
        {
            _counter++;

            _viewModel.CycleCounter = _counter.ToString();
        }

        public override void StepEndedPrematurely()
        {
            _taskCompletionSource.SetResult(new StepResult { Data = "Cycle Skipped/Scrapped", IsPassed = false });

            StopSerialCommunication();
        }
    }
}
