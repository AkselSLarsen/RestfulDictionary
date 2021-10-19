using RestfulDictionary.Interfaces;
using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace P2P {
    public class P2PDuelSocket {
        public P2PServerSocket Server;

        public P2PDuelSocket(IPAddress ip, int port, FileRepository repository) {
            Server = new P2PServerSocket(ip, port, repository);
        }

        public void Run() {
            Server.Run();

            P2PClientSocket.ClientTextUI();

            Console.ReadLine();
        }


        
    }
}