using System.Windows;
using System.Net.Sockets;
using System.Net;
using System;
using System.Windows.Input;
using System.Windows.Media;
using System.IO.Ports;
using System.Windows.Threading;
using System.Threading;
using System.Collections;

namespace Turck_RS485_RW_TCP
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Mainwindow_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();//先停止线程,然后终止进程.  
            Environment.Exit(0);//直接终止进程.  
        }
        private static byte[] CRC(byte[] data, int len)
        {
            int crc = 0xFFFF;
            int poly = 0x8408;
            byte[] crcLH = new byte[2];
            for (int i = 0; i < len; i++)
            {
                crc = crc ^ (int)data[i];
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x0001) == 1)
                    {
                        crc = (crc >> 1) ^ poly;
                    }
                    else
                    {
                        crc = (crc >> 1);
                    }
                }
            }
            crc = ~crc;
            crcLH[0] = Convert.ToByte(crc & 0xff);
            crcLH[1] = Convert.ToByte((crc >> 8) & 0xff);
            return crcLH;
        }

        private byte[] transmit(byte[] data)
        {
            int tempNum = data.Length + 3;
            int realNum = data.Length + 5;
            byte[] temp = new byte[tempNum];
            byte[] realdata = new byte[realNum];
            temp[0] = 0xAA;
            temp[1] = (byte)realNum;
            temp[2] = (byte)realNum;
            for (int i = 0; i < data.Length; i++)
            {
                temp[i + 3] = data[i];
            }
            byte[] crcdata = CRC(temp, temp.Length);
            for (int i = 0; i < tempNum; i++)
            {
                realdata[i] = temp[i];
            }
            realdata[realNum - 2] = crcdata[0];
            realdata[realNum - 1] = crcdata[1];
            return realdata;
        }

        /// <summary>
        /// TCP口部分程序
        /// </summary>
        #region //TCP
        DispatcherTimer autoReadTimer = new System.Windows.Threading.DispatcherTimer();
        TcpClient client;
        NetworkStream sendStream;
        int tagsel = 0;
        byte[] receiveData;
        Thread _thread;
        bool listenStart = false;
        private static Thread _ComSend; //发送数据线程
        private static bool Sending = false;//正在发送数据状态字
        private SendStr sendStr = new SendStr(); //发送数据线程传递参数的结构体格式
        private struct SendStr
        {
            public byte[] SendData;
            public bool SendMode;
        }

        private void Send(SendStr sendSet)
        {
            txtdata.Clear();
            if (Sending == true) return;//如果当前正在发送，则取消本次发送，本句注释后，可能阻塞在ComSend的lock处
            _ComSend = new Thread(new ParameterizedThreadStart(ComSend)); //new发送线程
            _ComSend.Start(sendSet);//发送线程启动
        }

        private void ComSend(object obj)
        {
            lock(this) //由于send()中的if(Sending == true) return，所以这里不会产生阻塞，如果没有那句，多次启动该线程，会在此处排队
            {
                if (client != null)
                {
                    try
                    {
                        Sending = true;
                        SendStr temp = (SendStr)obj;
                        byte[] comSendData = temp.SendData;
                        sendStream.Write(comSendData, 0, comSendData.Length);
                        Sending = false;
                        //_ComSend.Abort();

                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("命令发送失败：" + err.Message, "提示");
                    }
                }
            }
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            if(txtip.Text.Trim()==string.Empty || txtport.Text.Trim()==string.Empty)
            {
                return;
            }
            IPAddress ip = IPAddress.Parse(txtip.Text);
            client = new TcpClient();
            try
            {
                client.Connect(ip, int.Parse(txtport.Text));
                sendStream = client.GetStream();
                listenStart = true;
                _thread = new Thread(ListenerServer);
                _thread.Start();
                if (client.Connected)
                {
                    btndisconnect.IsEnabled = true;
                    btnConnect.IsEnabled = false;
                    btntagselect.IsEnabled = true;
                    btnrfon.IsEnabled = true;
                    btnrfoff.IsEnabled = true;
                    btnread.IsEnabled = true;
                    btnwrite.IsEnabled = true;
                    chkautoread.IsEnabled = true;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("出现错误：" + err.Message, "提示");
            }

        }
        private void btndisconnect_Click(object sender, RoutedEventArgs e)
        {
            listenStart = false;
            _thread.Abort();
            client.Close();
            
            btnConnect.IsEnabled = true;
            btndisconnect.IsEnabled = false;
            btntagselect.IsEnabled = false;
            btnrfon.IsEnabled = false;
            btnrfoff.IsEnabled = false;
            btnread.IsEnabled = false;
            btnwrite.IsEnabled = false;
            chkautoread.IsEnabled = false;
        }

        private void ListenerServer()
        {
            //NetworkStream receiveStrem = client.GetStream();
            Thread.Sleep(30);
            while (listenStart)
            {
                try
                {
                    int readSize = 0;
                    byte[] buffer = new byte[3000];
                    if (sendStream.DataAvailable)
                    {
                        lock (sendStream)
                        {
                            readSize = sendStream.Read(buffer, 0, 3000);
                        }
                    }

                    if (readSize > 9)
                    {
                        receiveData = new byte[readSize];
                        string zz = string.Empty;
                        for (int i = 0; i < readSize; i++)
                        {
                            receiveData[i] = buffer[i];
                        }

                        for (int i = 0; i < (readSize - 9); i++)
                        {
                            if (receiveData[i] == 0xAA && receiveData[i + 3] == 0x68)
                            {
                                if ((tagsel == 0 && receiveData[i + 1] == 0x0B && receiveData[i + 2] == 0x0B /*&& receiveData[i + 9] != 0x00*/) ||
                                    (tagsel == 1 && receiveData[i + 1] == 0x0F && receiveData[i + 2] == 0x0F /*&& receiveData[i + 14] != 0x00*/))
                                {
                                    this.txtread0.Dispatcher.Invoke(new Action(delegate () { txtread0.Text = receiveData[i + 5].ToString(); }));
                                    this.txtread1.Dispatcher.Invoke(new Action(delegate () { txtread1.Text = receiveData[i + 6].ToString(); }));
                                    this.txtread2.Dispatcher.Invoke(new Action(delegate () { txtread2.Text = receiveData[i + 7].ToString(); }));
                                    this.txtread3.Dispatcher.Invoke(new Action(delegate () { txtread3.Text = receiveData[i + 8].ToString(); }));
                                    break;
                                }
                            }
                            else
                            {
                                this.txtread0.Dispatcher.Invoke(new Action(delegate () { txtread0.Text = ""; }));
                                this.txtread1.Dispatcher.Invoke(new Action(delegate () { txtread1.Text = ""; }));
                                this.txtread2.Dispatcher.Invoke(new Action(delegate () { txtread2.Text = ""; }));
                                this.txtread3.Dispatcher.Invoke(new Action(delegate () { txtread3.Text = ""; }));
                            }
                        }

                        foreach (byte a in receiveData)
                        {
                            if (Convert.ToString(a, 16).Length == 1)
                            {
                                zz = zz + "0" + Convert.ToString(a, 16).ToUpper() + " ";
                            }
                            else
                            {
                                zz = zz + Convert.ToString(a, 16).ToUpper() + " ";
                            }
                        }
                        this.txtdata.Dispatcher.Invoke(new Action(delegate () { txtdata.Text = zz; }));
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show("出现错误：" + err.Message, "提示");
                }
                //将缓存中的数据写入传输流
            } 
        }

        private void txtpl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)//只能输入数字  
            {
                e.Handled = false;
            }
            else e.Handled = true;
            if (e.Key == Key.Enter)//输入回车  
            {
                if (txtpl.Text.Length == 0 || Convert.ToInt32(txtpl.Text) == 0)//时间为空或时间等于0，设置为1000  
                {
                    txtpl.Text = "1000";
                }
            }
        }

        private void btnrfon_Click(object sender, RoutedEventArgs e)
        {
            byte[] cmdData = new byte[2];
            cmdData[0] = 0x48;
            cmdData[1] = 0x00;
            sendStr.SendData = transmit(cmdData);
            Send(sendStr);
        }

        private void btnrfoff_Click(object sender, RoutedEventArgs e)
        {
            byte[] cmdData = new byte[2];
            cmdData[0] = 0x49;
            cmdData[1] = 0x00;
            sendStr.SendData = transmit(cmdData);
            Send(sendStr);
        }

        private void btnread_Click(object sender, RoutedEventArgs e)
        {
            byte[] cmdData = new byte[4];
            cmdData[0] = 0x68;
            cmdData[1] = 0x00;
            cmdData[2] = 0x00;
            cmdData[3] = 0x00;
            sendStr.SendData = transmit(cmdData);
            Send(sendStr); 
        }

        private void btnwrite_Click(object sender, RoutedEventArgs e)
        {
            if(Convert.ToInt16(txtwrite0.Text)<=255 && Convert.ToInt16(txtwrite1.Text) <= 255 && Convert.ToInt16(txtwrite2.Text) <= 255 && Convert.ToInt16(txtwrite3.Text) <= 255)
            {
                byte[] cmdData;
                if (tagsel == 0)
                {
                    cmdData = new byte[8];
                }
                else
                {
                    cmdData = new byte[12];
                }
                cmdData[0] = 0x69;
                cmdData[1] = 0x00;
                cmdData[2] = 0x00;
                cmdData[3] = 0x00;
                cmdData[4] = Convert.ToByte(txtwrite0.Text, 10);
                cmdData[5] = Convert.ToByte(txtwrite1.Text, 10);
                cmdData[6] = Convert.ToByte(txtwrite2.Text, 10);
                cmdData[7] = Convert.ToByte(txtwrite3.Text, 10);
                sendStr.SendData = transmit(cmdData);
                Send(sendStr);
            }
        }

        private void btntagselect_Click(object sender, RoutedEventArgs e)
        {
            if(cbotagsel.Text == "B128")
            {
                tagsel = 0;
                byte[] cmdData = new byte[4] { 0x61, 0x00, 0x1B, 0x03 };
                sendStr.SendData = transmit(cmdData);
                Send(sendStr);
            }
            else if (cbotagsel.Text == "K2")
            {
                tagsel = 1;
                byte[] cmdData = new byte[4] { 0x61, 0x00, 0xF9, 0x07 };
                sendStr.SendData = transmit(cmdData);
                Send(sendStr);
            }
        }

        private void chkautoread_Click(object sender, RoutedEventArgs e)
        {
            if(chkautoread.IsChecked == true)
            {
                int autoTime = Convert.ToInt32(txtpl.Text);
                autoReadTimer.Interval = TimeSpan.FromMilliseconds(autoTime);
                autoReadTimer.Start();
                autoReadTimer.Tick += new EventHandler(autoReadTimer_Tick);
            }
            else
            {
                autoReadTimer.Tick -= new EventHandler(autoReadTimer_Tick);
                autoReadTimer.Stop();
            }
        }

        private void autoReadTimer_Tick(object sender, EventArgs e)
        {
            byte[] cmdData = new byte[4];
            cmdData[0] = 0x68;
            cmdData[1] = 0x00;
            cmdData[2] = 0x00;
            cmdData[3] = 0x00;
            sendStr.SendData = transmit(cmdData);
            Send(sendStr);
        }
        #endregion

        /// <summary>
        /// COM口部分程序
        /// </summary>
        DispatcherTimer autoReadTimerC = new System.Windows.Threading.DispatcherTimer();
        SerialPort port = new SerialPort();
        int tagselC = 0;
        private static Thread _ComSendC; //发送数据线程
        private static bool SendingC = false;//正在发送数据状态字
        Queue recQueue = new Queue();//接收数据过程中，接收数据线程与数据处理线程直接传递的队列，先进先出
        private SendStrC sendStrC = new SendStrC(); //发送数据线程传递参数的结构体格式

        private struct SendStrC 
        {
            public byte[] SendData;
            public bool SendMode;
        }

        private void Mainwindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadComPort();
            Thread _ComRec = new Thread(new ThreadStart(ComRec)); //查询串口接收数据线程声明
            _ComRec.Start();//启动线程
        }
        private void btnComRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadComPort();
        }

        private void LoadComPort()
        {
            string[] portName = SerialPort.GetPortNames();
            foreach (string str in portName)
            {
                cboComSel.Items.Add(str);
            }
            cboComSel.SelectedIndex = 0;
            port.ReadTimeout = 8000;//串口读超时8秒  
            port.WriteTimeout = 8000;//串口写超时8秒，在1ms自动发送数据时拔掉串口，写超时5秒后，会自动停止发送，如果无超时设定，这时程序假死  
            port.ReadBufferSize = 1024;//数据读缓存  
            port.WriteBufferSize = 1024;//数据写缓存
        }

        private void btnConnectC_Click(object sender, RoutedEventArgs e)
        {
            if (port.IsOpen)
            {
                port.Close();
            }
            try
            {
                port.PortName = cboComSel.Text;
                port.BaudRate = Convert.ToInt32(cboBaudSel.Text);
                port.Parity = Parity.None;
                port.DataBits = 8;
                port.StopBits = StopBits.One;
                port.Encoding = System.Text.Encoding.Unicode;
                port.Open();
                txtComStatus.Text = cboComSel.Text + "已连接";
                Brush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Green"));
                txtComStatus.Background = color;
                btnConnectC.IsEnabled = false;
                btnDisconnectC.IsEnabled = true;
                cboComSel.IsEnabled = false;
                cboBaudSel.IsEnabled = false;
                btntagselectC.IsEnabled = true;
                btnrfonC.IsEnabled = true;
                btnrfoffC.IsEnabled = true;
                btnreadC.IsEnabled = true;
                btnwriteC.IsEnabled = true;
                if (port.IsOpen)
                {
                    chkautoreadC.IsEnabled = true;
                }
                port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            }
            catch (Exception err)
            {
                MessageBox.Show("出现错误：" + err.Message, "提示");
            }
        }

        private void btnDisconnectC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(chkautoreadC.IsChecked == false)
                {
                    port.Close();
                    txtComStatus.Text = cboComSel.Text + "已断开";
                    Brush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("Red"));
                    txtComStatus.Background = color;
                    btnConnectC.IsEnabled = true;
                    btnDisconnectC.IsEnabled = false;
                    cboComSel.IsEnabled = true;
                    cboBaudSel.IsEnabled = true;
                    btntagselectC.IsEnabled = false;
                    btnrfonC.IsEnabled = false;
                    btnrfoffC.IsEnabled = false;
                    btnreadC.IsEnabled = false;
                    btnwriteC.IsEnabled = false;
                    chkautoreadC.IsEnabled = false;
                    port.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
                }
                else
                {
                    MessageBox.Show("请先关闭自动读取功能！", "出现错误");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show("出现错误：" + err.Message, "提示");
            }
        }

        private void SendC(SendStrC sendSetC)
        {
            txtdataC.Clear();
            ReceiveDataCClear();
            receiveDataCntC = 0;
            if (SendingC == true) return;//如果当前正在发送，则取消本次发送，本句注释后，可能阻塞在ComSend的lock处
            _ComSendC = new Thread(new ParameterizedThreadStart(ComSendC)); //new发送线程
            _ComSendC.Start(sendSetC);//发送线程启动
        }

        private void ComSendC(object objC)
        {
            lock (this) //由于send()中的if(Sending == true) return，所以这里不会产生阻塞，如果没有那句，多次启动该线程，会在此处排队
            {
                if (port != null)
                {
                    try
                    {
                        SendingC = true;
                        SendStrC tempC = (SendStrC)objC;
                        byte[] comSendDataC = tempC.SendData;
                        port.Write(comSendDataC, 0, comSendDataC.Length);
                        SendingC = false;
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show("命令发送失败：" + err.Message, "提示");
                    }
                }
            }
        }

        //串口接收事件
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            //Thread.Sleep(10);
            byte[] readdata;
            try
            {
                readdata = new byte[port.BytesToRead];
                port.Read(readdata, 0, readdata.Length);
                recQueue.Enqueue(readdata);//读取数据入列Enqueue（全局）
            }
            catch
            {
                MessageBox.Show("无法接收数据，原因未知！");
            }
        }

        byte[] receiveDataC = new byte[1000];
        int receiveDataCntC = 0;
        private void ReceiveDataCClear()
        {
            for (int i = 0; i < receiveDataC.Length; i++)
            {
                receiveDataC[i] = 0;
            }
        }

        void ComRec()//接收线程，窗口初始化中就开始启动运行
        {
            while(true)
            {
                if (recQueue.Count > 0)//当串口接收线程中有新的数据时候，队列中有新进的成员recQueue.Count > 0
                {
                    byte[] recBuffer = (byte[])recQueue.Dequeue();//出列Dequeue（全局）
                    for(int i=0;i<recBuffer.Length;i++)
                    {
                        receiveDataC[receiveDataCntC] = recBuffer[i];
                        receiveDataCntC++;
                    }
                    string zz = string.Empty;
                    foreach (byte a in recBuffer)
                    {
                        if (Convert.ToString(a, 16).Length == 1)
                        {
                            zz = zz + "0" + Convert.ToString(a, 16).ToUpper() + " ";
                        }
                        else
                        {
                            zz = zz + Convert.ToString(a, 16).ToUpper() + " ";
                        }
                    }
                    this.txtdataC.Dispatcher.Invoke(new Action(delegate () { txtdataC.Text += zz; }));

                    for (int i = 0; i < (receiveDataCntC - 3); i++)
                    {
                        if (receiveDataC[i] == 0xAA && receiveDataC[i + 3] == 0x68)
                        {
                            if ((tagselC == 0 && receiveDataC[i + 1] == 0x0B && receiveDataC[i + 2] == 0x0B && receiveDataC[i + 9] != 0x00) ||
                                    (tagselC == 1 && receiveDataC[i + 1] == 0x0F && receiveDataC[i + 2] == 0x0F && receiveDataC[i + 14] != 0x00))
                            {
                                this.txtreadC0.Dispatcher.Invoke(new Action(delegate () { txtreadC0.Text = receiveDataC[i + 5].ToString(); }));
                                this.txtreadC1.Dispatcher.Invoke(new Action(delegate () { txtreadC1.Text = receiveDataC[i + 6].ToString(); }));
                                this.txtreadC2.Dispatcher.Invoke(new Action(delegate () { txtreadC2.Text = receiveDataC[i + 7].ToString(); }));
                                this.txtreadC3.Dispatcher.Invoke(new Action(delegate () { txtreadC3.Text = receiveDataC[i + 8].ToString(); }));
                                break;
                            }
                        }
                        else
                        {
                            this.txtreadC0.Dispatcher.Invoke(new Action(delegate () { txtreadC0.Text = ""; }));
                            this.txtreadC1.Dispatcher.Invoke(new Action(delegate () { txtreadC1.Text = ""; }));
                            this.txtreadC2.Dispatcher.Invoke(new Action(delegate () { txtreadC2.Text = ""; }));
                            this.txtreadC3.Dispatcher.Invoke(new Action(delegate () { txtreadC3.Text = ""; }));
                        }
                    }
                }
                else
                {
                    Thread.Sleep(10);//如果不延时，一直查询，将占用CPU过高
                }
            }
        }
        private void txtplC_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.D0 && e.Key <= Key.D9 || e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9)//只能输入数字  
            {
                e.Handled = false;
            }
            else e.Handled = true;
            if (e.Key == Key.Enter)//输入回车  
            {
                if (txtpl.Text.Length == 0 || Convert.ToInt32(txtpl.Text) == 0)//时间为空或时间等于0，设置为1000  
                {
                    txtpl.Text = "1000";
                }
            }
        }

        private void chkautoreadC_Click(object sender, RoutedEventArgs e)
        {
            if (chkautoreadC.IsChecked == true)
            {
                int autoTimeC = Convert.ToInt32(txtplC.Text);
                autoReadTimerC.Interval = TimeSpan.FromMilliseconds(autoTimeC);
                autoReadTimerC.Start();
                autoReadTimerC.Tick += new EventHandler(autoReadTimerC_Tick);
            }
            else
            {
                autoReadTimerC.Tick -= new EventHandler(autoReadTimerC_Tick);
                autoReadTimerC.Stop();
            }
        }

        private void autoReadTimerC_Tick(object sender, EventArgs e)
        {
            byte[] cmdData = new byte[4];
            cmdData[0] = 0x68;
            cmdData[1] = 0x00;
            cmdData[2] = 0x00;
            cmdData[3] = 0x00;
            sendStrC.SendData = transmit(cmdData);
            SendC(sendStrC);
        }

        private void btnrfonC_Click(object sender, RoutedEventArgs e)
        {
            byte[] cmdData = new byte[2];
            cmdData[0] = 0x48;
            cmdData[1] = 0x00;
            sendStrC.SendData = transmit(cmdData);
            SendC(sendStrC);
        }

        private void btnrfoffC_Click(object sender, RoutedEventArgs e)
        {
            byte[] cmdData = new byte[2];
            cmdData[0] = 0x49;
            cmdData[1] = 0x00;
            sendStrC.SendData = transmit(cmdData);
            SendC(sendStrC);
        }

        private void btnreadC_Click(object sender, RoutedEventArgs e)
        {
            byte[] cmdData = new byte[4];
            cmdData[0] = 0x68;
            cmdData[1] = 0x00;
            cmdData[2] = 0x00;
            cmdData[3] = 0x00;
            sendStrC.SendData = transmit(cmdData);
            SendC(sendStrC);
        }

        private void btntagselectC_Click(object sender, RoutedEventArgs e)
        {
            if (cbotagselC.Text == "B128")
            {
                tagselC = 0;
                byte[] cmdData = new byte[4] { 0x61, 0x00, 0x1B, 0x03 };
                sendStrC.SendData = transmit(cmdData);
                SendC(sendStrC);
            }
            else if (cbotagselC.Text == "K2")
            {
                tagselC = 1;
                byte[] cmdData = new byte[4] { 0x61, 0x00, 0xF9, 0x07 };
                sendStrC.SendData = transmit(cmdData);
                SendC(sendStrC);
            }
        }

        private void btnwriteC_Click(object sender, RoutedEventArgs e)
        {
            if (Convert.ToInt16(txtwriteC0.Text) <= 255 && Convert.ToInt16(txtwriteC1.Text) <= 255 && Convert.ToInt16(txtwriteC2.Text) <= 255 && Convert.ToInt16(txtwriteC3.Text) <= 255)
            {
                byte[] cmdData;
                if (tagselC == 0)
                {
                    cmdData = new byte[8];
                }
                else
                {
                    cmdData = new byte[12];
                }
                cmdData[0] = 0x69;
                cmdData[1] = 0x00;
                cmdData[2] = 0x00;
                cmdData[3] = 0x00;
                cmdData[4] = Convert.ToByte(txtwriteC0.Text, 10);
                cmdData[5] = Convert.ToByte(txtwriteC1.Text, 10);
                cmdData[6] = Convert.ToByte(txtwriteC2.Text, 10);
                cmdData[7] = Convert.ToByte(txtwriteC3.Text, 10);
                sendStrC.SendData = transmit(cmdData);
                SendC(sendStrC);
            }
        }
    }
}
