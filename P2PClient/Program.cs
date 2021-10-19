using System;
using System.Net;

namespace P2P {
    public class Program {
        public static IPAddress IP = Dns.GetHostEntry(Dns.GetHostName()).AddressList[1]; //IPAddress.Any
        public static int Port = 2121;

        public static string URLRoot = "http://localhost:43393/";

        public static void Main() {

            P2PDuelSocket socket = new P2PDuelSocket(IP, Port, new FileRepository());
            socket.Run();

            //Console.WriteLine(P2PServerSocket.ToPeerJson(socket.Server));
        }
    }
}