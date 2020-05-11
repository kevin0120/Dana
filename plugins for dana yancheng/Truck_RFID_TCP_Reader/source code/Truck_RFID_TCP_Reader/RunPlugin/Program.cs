using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Truck_RFID_TCP_Reader;

namespace RunPlugin
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("hello world!".Skip(0).Take(12));
            byte[] test = Encoding.ASCII.GetBytes("hello world!");
            string val = Encoding.ASCII.GetString(test, 0, 12).Trim();
            Console.WriteLine(val);

            byte[] a = test.Skip(2).Take(8).ToArray();

            Console.WriteLine(Encoding.ASCII.GetString(a, 0, 8).Trim());

            Console.WriteLine(a.ToString());

            string Text=string.Empty;
            for (int i = 0; i < test.Length; i++)
            {
                Text += "0x" + test[i].ToString("X02") + " ";
            }

            Console.WriteLine(Text);

            //SetParameter();
            Plugin plug = new Plugin();
            plug.Reader("192.168.0.123", "3000");
            Console.ReadKey();
        }





    }
}
