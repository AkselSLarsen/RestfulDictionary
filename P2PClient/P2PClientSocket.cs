using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace P2P {
    public class P2PClientSocket {
        public TcpClient Client;
        public StreamReader Reader;
        public StreamWriter Writer;

        public P2PClientSocket(IPAddress host, int port) {
            Client = new TcpClient(host.ToString(), port);
            NetworkStream ns = Client.GetStream();
            Reader = new StreamReader(ns);
            Writer = new StreamWriter(ns);
        }
        
        public static void ClientTextUI() {
            Console.WriteLine("Write\n\"GetFiles\" or \"GF\" to list all files.\n" +
                "\"GetPeers\" or \"GP\" to list all peers.\n" +
                "\"GetPeersWithFile\" or \"GPWF\" followed by a space and the file's name to get all peers with the file.\n" +
                "\"DownloadFile\" or \"DF\" to download a file from a random peer.\n" +
                "\"DownloadFileFromPeer\" or \"DFFP\" followed by the ip and port divided by spaces, of the peer you want to download the file from.\n" +
                "Or write Q to exit the program.");

            Boolean end = false;
            while (!end) {
                try {
                    string message = Console.ReadLine();

                    if (message.ToLowerInvariant().StartsWith("q")) {
                        P2PServerSocket.Stop();
                        end = true;
                        Console.Clear();
                        Console.WriteLine("Program is shutting down, please wait.");

                    } else if (message.ToLowerInvariant().StartsWith("getpeerswithfile") || message.ToLowerInvariant().StartsWith("gpwf")) {
                        P2PServices.GetPeers(message.Split(" ")[1]);

                    } else if (message.ToLowerInvariant().StartsWith("getfiles") || message.ToLowerInvariant().StartsWith("gf")) {
                        P2PServices.GetFiles();

                    } else if (message.ToLowerInvariant().StartsWith("getpeers") || message.ToLowerInvariant().StartsWith("gp")) {
                        P2PServices.GetPeers();

                    } else if (message.ToLowerInvariant().StartsWith("downloadfilefrompeer") || message.ToLowerInvariant().StartsWith("dffp")) {
                        P2PServices.DownloadFile(message);

                    } else if (message.ToLowerInvariant().StartsWith("downloadfile") || message.ToLowerInvariant().StartsWith("df")) {
                        P2PServices.DownloadFile(message);
                    
                    } else {
                        Console.WriteLine("Command \"" + message + "\" was not understood.");
                    }
                } catch (Exception e) {
                    Console.WriteLine(e.Message);                    
                    end = true;
                }
            }
        }
    }
}