using System.Windows.Input;
using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Desoutter.ProcessControl.Plugin.v2.Interface.Model;

namespace Plugin.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private string _serialPortData;
        public string SerialPortData
        {
            get { return _serialPortData; }
            set { Set(ref _serialPortData, value); }
        }

        private string _serialPortNumber;
        public string SerialPortNumber
        {
            get { return _serialPortNumber; }
            set { Set(ref _serialPortNumber, value); }
        }

        private string _serialDataReceived;
        public string SerialDataReceived
        {
            get { return _serialDataReceived; }
            set { Set(ref _serialDataReceived, value); }
        }

        private string _keyboardData;
        public string KeyboardData
        {
            get { return _keyboardData; }
            set { Set(ref _keyboardData, value); }
        }

        private string _cycleCounter;
        public string CycleCounter
        {
            get { return _cycleCounter; }
            set { Set(ref _cycleCounter, value); }
        }

        private ICommand _validateCommand;
        public ICommand ValidateCommand
        {
            get { return _validateCommand; }
            set { Set(ref _validateCommand, value); }
        }

        private ICommand _displayKeyboardCommand;
        public ICommand DisplayKeyboardCommand
        {
            get { return _displayKeyboardCommand; }
            set { Set(ref _displayKeyboardCommand, value); }
        }
    }
}
