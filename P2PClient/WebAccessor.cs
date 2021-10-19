using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace P2P {
    public abstract class WebAccessor {
        public static string GetPeersUrl = Program.URLRoot + "";
        public static string AddPeerUrl = Program.URLRoot + "";
        public static string RemovePeerUrl = Program.URLRoot + "";

        public static string GetFilesUrl = Program.URLRoot + "files";
        public static string AddFileUrl = Program.URLRoot + "files";
        public static string RemoveFileUrl = Program.URLRoot + "files";

        public static string GetPeerUrl(Peer peer) {
            return Program.URLRoot + "/peer?ipv4=" + peer.IPv4 + "&ipv6=" + peer.IPv6 + "&port=" + peer.Port;
        }

        public static string GetPeersWithFileUrl(string filename) {
            return Program.URLRoot + "/peer?filename=" + filename;
        }

        public static string GetFilesByNameUrl(string filename) {
            return Program.URLRoot + "/files/byname?filename=" + filename;
        }

        public static string GetFilesOfPeerUrl(Peer peer) {
            return Program.URLRoot + "/files/bypeer?ipv4=" + peer.IPv4 + "&ipv6=" + peer.IPv6 + "&port=" + peer.Port;
        }

        public static string ReadJsonFromUrl(string Url) {
            try {
                return new WebClient().DownloadString(Url);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        public static bool WriteJsonToUrl(string Url, string Json) {
            try {
                new WebClient().UploadString(Url, Json);
                return true;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return false;
        }

    }
}
