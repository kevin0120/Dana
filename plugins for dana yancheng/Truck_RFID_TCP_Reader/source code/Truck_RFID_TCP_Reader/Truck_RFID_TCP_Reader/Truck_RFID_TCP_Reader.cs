using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using Desoutter.ProcessControl.Plugin.v2.Interface;
using Desoutter.ProcessControl.Plugin.v2.Interface.AttributeParameter;
using Desoutter.ProcessControl.Plugin.v2.Interface.Model;


namespace Truck_RFID_TCP_Reader
{
    [Plugin]
    public class Plugin : PluginBase
    {
        TcpClient client;
        NetworkStream sendStream;
        bool listenStart = false;
        private static Thread _ComSend; //发送数据线程
        private static bool Sending = false;//正在发送数据状态字
        private SendStr sendStr = new SendStr(); //发送数据线程传递参数的结构体格式
        private string val = string.Empty;
        private string val_return = string.Empty;
        private struct SendStr
        {
            public byte[] SendData;
        }
        public override FrameworkElement CreateControl()
        {
            throw new System.NotImplementedException();
        }

        public override StepResult ExecuteStep(object parameters)
        {
            var param = parameters as Parameters;
            //connect
            Connect(param.IP.Trim(), param.Port.Trim());
            // reader request
            byte[] cmdData = new byte[4];
            cmdData[0] = 0x68;
            cmdData[1] = 0x00;
            cmdData[2] = 0x00;
            cmdData[3] = 0x0F;
            sendStr.SendData = Transmit(cmdData);
            Send(sendStr);

            // new thread to get read reply
            listenStart = true;
            //_thread = new Thread(ListenerServer);
            //_thread.Start();
            if (client.Client != null)
            {
               ReadProc();
            }

            val_return = val;
            val = string.Empty;

            return new StepResult { Data = val_return, IsPassed = true };
        }

        public override bool HasToCreateControl()
        {
            //       throw new System.NotImplementedException();
            return false;
        }


        private byte[] Transmit(byte[] data)
        {


            int tempNum = data.Length + 3;
            int realNum = data.Length + 5;
            byte[] temp = new byte[tempNum];
            byte[] realdata = new byte[realNum];
            temp[0] = 0xAA;
            temp[1] = (byte)realNum;
            temp[2] = (byte)realNum;
            temp.CopyTo(realdata, 0);
            data.CopyTo(realdata, 3);
            byte[] crcdata = CRC(realdata.Take(tempNum).ToArray(), temp.Length);
            crcdata.CopyTo(realdata, realNum - 2);
            return realdata;
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
        private void Send(SendStr sendSet)
        {
            if (Sending == true) return;//如果当前正在发送，则取消本次发送，本句注释后，可能阻塞在ComSend的lock处
            _ComSend = new Thread(new ParameterizedThreadStart(ComSend)); //new发送线程
            _ComSend.Start(sendSet);//发送线程启动
        }

        private void ComSend(object obj)
        {
            lock (this) //由于send()中的if(Sending == true) return，所以这里不会产生阻塞，如果没有那句，多次启动该线程，会在此处排队
            {
                if (client.Client != null)
                {
                    try
                    {
                        while (sendStream.CanWrite) {
                            lock (sendStream)
                            {
                                Sending = true;
                                SendStr temp = (SendStr)obj;
                                byte[] comSendData = temp.SendData;
                                sendStream.Write(comSendData, 0, comSendData.Length);
                                Sending = false;
                            }
                            Thread.Sleep(500);
                        }
                        //_ComSend.Abort();
                    }
                    catch (Exception err)
                    {
                        client.Close();
                       // MessageBox.Show("ComSend 命令发送失败：" + err.Message, "提示");
                    }
                }
            }
        }

        private void Connect(string ipAdress, string port)
        {
            IPAddress ip = IPAddress.Parse(ipAdress);

            client = new TcpClient();

            if (client.Connected)
            {
                return;
            }

            try
            {
                client.Connect(ip, int.Parse(port));
                sendStream = client.GetStream();

            }
            catch (Exception err)
            {
                client.Close();
               // MessageBox.Show("Connect 出现错误：" + err.Message, "提示");
            }

        }

        public void theout(object source, System.Timers.ElapsedEventArgs e)
        {
            listenStart = false;
            if (client.Connected) { 
            //MessageBox.Show("该工步使用的读rfid插件已经超时10s！！！，发射器可能不在合理范围之内，结束工步","提示");
            }
        }


        private void ReadProc()
        {
            //NetworkStream receiveStrem = client.GetStream();
            System.Timers.Timer t = new System.Timers.Timer(10000);//实例化Timer类，设置间隔时间为1000毫秒；

            t.Elapsed += new System.Timers.ElapsedEventHandler(theout);//到达时间的时候执行事件；

            t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)；

            t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；


            Thread.Sleep(30);
            while (listenStart)
            {
                try
                {
                    int readSize = 0;
                    byte[] buffer = new byte[3000];

                    while (sendStream.DataAvailable)
                    {
                        lock (sendStream)
                        {
                            readSize = sendStream.Read(buffer, 0, 3000);
                            //Console.WriteLine(readSize);

                        }

                        //string Text1 = string.Empty;
                        //for (int i = 0; i < 100; i++)
                        //{
                        //    Text1 += "0x" + buffer[i].ToString("X02") + " ";
                        //}

                        ////MessageBox.Show("收到字节：" + Text1);
                        //Console.WriteLine("收到字节1：" + Text1);

                    while (true) { 
                    if (buffer[0] == 0xAA && buffer[3] == 0x68 && buffer[1] == 0x47)
                    {

                        byte[] a = buffer.Skip(5).Take(64).ToArray();
                        val = Encoding.ASCII.GetString(a, 0, 64).Trim();
                        client.Close();
                        Console.WriteLine(val);
                        return;
                    } else if (buffer[0] == 0xAA && buffer[3] == 0x68 && buffer[1] == 0x07)
                        {
                            buffer = buffer.Skip(7).ToArray();
                            //Console.WriteLine("收到字节2：" + string.Join(",", buffer));

                        }
                        else if (buffer[0] == 0xAA && buffer[3] == 0x68 && buffer[1] == 0x0A)
                        {
                            buffer = buffer.Skip(10).ToArray();
                            //Console.WriteLine("收到字节2：" + string.Join(",", buffer));
                        }

                        else
                        {
                          
                           //Console.WriteLine("收到字节3：" + string.Join(",", buffer));
                            break;
                        }
                        //Console.WriteLine("收到字节4：" + string.Join(",", buffer))；
                    }
                        break;

                    }


                }
                catch (Exception err)
                {
                    client.Close();

                   // MessageBox.Show("ReadProc 出现错误：" + err.Message, "提示");
                    return;
                }
                //将缓存中的数据写入传输流
            }
            client.Close();
            return;
        }

        public void Reader(string ip, string port)
        {
            //connect
            Connect(ip, port);

            // reader request
            byte[] cmdData = new byte[4];
            cmdData[0] = 0x68;
            cmdData[1] = 0x00;
            cmdData[2] = 0x00;
            cmdData[3] = 0x0F;
            sendStr.SendData = Transmit(cmdData);
            Send(sendStr);

            // new thread to get read reply
            listenStart = true;
            //_thread = new Thread(ReadProc);
            //_thread.Start();
            if (client.Client != null)
            {
                ReadProc();
            }

            if (listenStart)
            {
                Console.WriteLine("Sucess");
            }
            else
            {
                Console.WriteLine("Fail");
            }

        }

    }
}





