using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulDictionary.Manager {
    public abstract class PeerManager {
        private static readonly List<Peer> Data = new List<Peer>
        {
            new Peer(0, 32, new List<string>() { "file1","file2","file4" }),
            new Peer(int.MaxValue, 27757, new List<string>() { "file4","file2","file3" })
        };

        public static List<Peer> GetAll() {
            return new List<Peer>(Data);
        }

        public static Peer Get(string ipv4, string ipv6, int port) {
            Peer peer = new Peer();
            try {
                peer.IPv4 = uint.Parse(ipv4);
                peer.Port = port;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            try {
                peer.IPv4FromString(ipv4);
                peer.Port = port;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            try {
                peer.IPv6 = long.Parse(ipv6);
                peer.Port = port;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            try {
                peer.IPv6FromString(ipv6);
                peer.Port = port;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }

            bool exists = Data.Contains(peer);

            if(exists) {
                foreach(Peer re in Data) {
                    if(re.Equals(peer)) {
                        return re;
                    }
                }
            }
            return null;
        }

        public static List<Peer> Get(string filename) {
            List<Peer> re = new List<Peer>();
            foreach(Peer peer in Data) {
                if(peer.Files.Contains(filename)) {
                    re.Add(peer);
                }
            }
            return re;
        }

        public static void Add(Peer newPeer) {
            Delete(newPeer);

            Data.Add(newPeer);
        }

        public static Peer Delete(Peer peer) {
            bool deleted = Data.Remove(peer);
            return deleted ? peer : null;
        }

        public static List<FileEndPoint> GetFilesOfPeer(Peer peer) {
            List<FileEndPoint> re = new List<FileEndPoint>();
            foreach (FileEndPoint file in FileManager.GetAll()) {
                if(file.Peer == peer) {
                    re.Add(file);
                }
            }
            return re;
        }

        [Obsolete]
        public static Peer Update(Peer updatedPeer) {
            Peer deletedPeer = Delete(updatedPeer);

            if(deletedPeer != null) {
                Add(updatedPeer);

                return deletedPeer;
            }

            return null;
        }

    }
}