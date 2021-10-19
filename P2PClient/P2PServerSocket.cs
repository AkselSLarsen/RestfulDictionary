using RestfulDictionary.Interfaces;
using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace P2P {
    public class P2PServerSocket {
        private TcpListener Server;
        public FileRepository Repository;
        private static bool end;

        public P2PServerSocket(IPAddress ip, int port, FileRepository repository) {
            Server = new TcpListener(ip, port);
            Repository = repository;
        }

        public void Run() {
            Register(this);

            Server.Start();

            end = false;
            while (!end) {
                TcpClient peer = Server.AcceptTcpClient();
                Console.WriteLine("A peer has connected");
                Task.Run(() => P2PServices.ServicePeer(this, peer));
            }

            Deregister(this);
        }

        public static void Stop() {
            end = true;
        }

        public static Peer ToPeer(P2PServerSocket socket) {
            string[] IPAndPort = socket.Server.LocalEndpoint.ToString().Split(":");

            long? iPv4 = null;
            string iPv6 = null;
            int port = int.Parse(IPAndPort[1]);

            List<string> files = socket.Repository.GetFileNames();

            if (IPAndPort[0].Contains("[")) { //if we are using IPv6
                iPv6 = IPAndPort[0];
            } else { //else we assume we use IPv4
                iPv4 = Peer.IPv4FromString(IPAndPort[0]);
            }
            return new Peer(iPv4, iPv6, port, files);
        }

        public static string ToPeerJson(P2PServerSocket socket) {
            return ToPeer(socket).ToJson();

            //Example: {"iPv4":0,"iPv6":null,"port":32,"files":["file1","file2","file4"]}
        }

        private static void Register(P2PServerSocket Server) {
            WebAccessor.WriteJsonToUrl(WebAccessor.AddPeerUrl, P2PServerSocket.ToPeerJson(Server));

            foreach (FileEndPoint file in Server.Repository.ToFileEndPoints(IJsonAble<Peer>.FromJson(P2PServerSocket.ToPeerJson(Server)))) {
                WebAccessor.WriteJsonToUrl(WebAccessor.AddFileUrl, file.ToJson());
            }
        }

        private static void Deregister(P2PServerSocket Server) {
            Peer peer = ToPeer(Server);
            WebAccessor.WriteJsonToUrl(WebAccessor.RemovePeerUrl, peer.ToJson());

            foreach (FileEndPoint file in Server.Repository.ToFileEndPoints(IJsonAble<Peer>.FromJson(P2PServerSocket.ToPeerJson(Server)))) {
                WebAccessor.WriteJsonToUrl(WebAccessor.RemoveFileUrl, file.ToJson());
            }
        }
    }
}