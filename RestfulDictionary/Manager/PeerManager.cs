using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulDictionary.Manager {
    public abstract class PeerManager {
        private static readonly List<Peer> Data = new List<Peer>();

        public static List<Peer> GetAll() {
            return new List<Peer>(Data);
        }

        public static Peer Get(string ipv4, string ipv6, int port) {
            Peer peer = new Peer();
            try {
                peer.IPv4 = long.Parse(ipv4);
                peer.Port = port;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            try {
                peer.IPv4 = Peer.IPv4FromString(ipv4);
                peer.Port = port;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            try {
                peer.IPv6 = ipv6;
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

            if (deleted) {
                foreach (FileEndPoint file in GetFilesOfPeer(peer)) {
                    FileEndPoint deletedfile = FileManager.Delete(file);
                    if (deletedfile == null) {
                        throw new Exception("Tried to delete file that wasn't there. As this operation is automated, it will never be caused by human error.");
                    }
                }
            }

            return deleted ? peer : null;
        }

        public static List<FileEndPoint> GetFilesOfPeer(Peer peer) {
            List<FileEndPoint> re = new List<FileEndPoint>();
            foreach (FileEndPoint file in FileManager.GetAll()) {
                if(file.Peer.Equals(peer)) {
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