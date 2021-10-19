using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace P2P {
    public abstract class WebAccessor {
        private static readonly HttpClient WebAccess = new HttpClient();

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
                return WebAccess.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return "";
        }

        public static string WriteJsonToUrl(string Url, string Json) {
            try {
                HttpContent content = new StringContent(Json);
                return WebAccess.PostAsync(Url, content).Result.Content.ReadAsStringAsync().Result;
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
            return null;
        }

    }
}
