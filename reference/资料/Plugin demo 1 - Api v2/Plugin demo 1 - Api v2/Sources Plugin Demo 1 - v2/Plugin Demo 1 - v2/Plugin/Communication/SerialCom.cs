using System;
using System.IO.Ports;
using Plugin.Events;

namespace Plugin.Communication
{
    public class SerialClient : IDisposable
    {
        #region Defines

        private string _port;
        private int _baudRate;
        private int _dataBits;
        private Parity _parity;
        private StopBits _stopBits; 

        private SerialPort _serialPort;

        private string _dataReceived;

        #endregion

        #region CustomEvent

        public event EventHandler SerialDataReceived;

        #endregion

        #region Constructors

        public SerialClient(string port)
        {
            _port = port;
            _baudRate = 9600;
            _dataBits = 8;
            _parity = Parity.None;
            _stopBits = StopBits.One;

            _serialPort = new SerialPort($"COM{_port}", _baudRate, _parity, _dataBits, _stopBits);
            _serialPort.ReadTimeout = -1;
            _serialPort.WriteTimeout = -1;
            _serialPort.Handshake = Handshake.None;

            _serialPort.DataReceived += new SerialDataReceivedEventHandler(ReceivedData);
        }

        #endregion

        #region Properties
        public string Port
        {
            get { return _port; }
        }
        public int BaudRate
        {
            get { return _baudRate; }
        }
        public int DataBits
        {
            get { return _dataBits; }
        }
        public string DataReceived
        {
            get { return _dataReceived; }
        }
        #endregion

        #region Methods
        #region Port Control
        public void OpenCom()
        {
            try
            {           
                if (!(_serialPort.IsOpen))
                {                 
                    _serialPort.Open();
                }    
            }
            catch (Exception ex)
            {
                //return false;
                throw;
            }
        }

        public bool PortIsOpen()
        {
            return _serialPort.IsOpen;
        }

        public void CloseCom()
        {
            if (_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        public void ResetCom()
        {
            CloseCom();
            OpenCom();
        }

        #endregion

        #region Send/Receive

        public void Send(byte[] packet)
        {
            try
            {
                _serialPort.Write(packet, 0, packet.Length);
            }
            catch (Exception ex)
            {
                throw;
            }      
        }

        public void Send(string packet)
        {
            try
            {
                _serialPort.Write(packet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        void ReceivedData(object sender, SerialDataReceivedEventArgs e)
        {
            _dataReceived = _serialPort.ReadLine();

            SerialDataReceived.Raise(this, new EventArgs());
        }

        #endregion

        #region IDisposable Methods
        public void Dispose()
        {
            CloseCom();

            if (_serialPort != null)
            {
                _serialPort.Dispose();
                _serialPort = null;
            }
        }
        #endregion

        #endregion
    }
}
