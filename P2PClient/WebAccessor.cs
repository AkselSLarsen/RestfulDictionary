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

        public static string GetFilesUrl = Program.URLRoot + "/files";
        public static string AddFileUrl = Program.URLRoot + "/files";

        public static string GetPeerUrl(Peer peer) {
            return Program.URLRoot + "/peer?ipv4=" + peer.IPv4 + "&ipv6=" + peer.IPv6 + "&port=" + peer.Port;
        }

        public static string GetPeersWithFileUrl(string filename) {
            return Program.URLRoot + "/peer/fromfile?filename=" + filename;
        }

        public static string RemovePeerUrl(Peer peer) {
            return Program.URLRoot + "?ipv4=" + peer.IPv4 + "&ipv6=" + peer.IPv6 + "&port=" + peer.Port;
        }

        public static string GetFilesByNameUrl(string filename) {
            return Program.URLRoot + "/files/byname?filename=" + filename;
        }

        public static string GetFilesOfPeerUrl(Peer peer) {
            return Program.URLRoot + "/files/bypeer?ipv4=" + peer.IPv4 + "&ipv6=" + peer.IPv6 + "&port=" + peer.Port;
        }


        public static string RemoveFileUrl(string filename, Peer peer) {
            return Program.URLRoot + "/files?filename=" + filename + "&ipv4=" + peer.IPv4 + "&ipv6=" + peer.IPv6 + "&port=" + peer.Port;
        }

        public static string GetJsonFromUrl(string Url) {
            try {
                return WebAccess.GetAsync(Url).Result.Content.ReadAsStringAsync().Result;
            } catch (Exception e) {
                Console.WriteLine(e.Message + "\nFailed the get request to \"" + Url + "\".");
            }
            return "";
        }

        public static string PostJsonToUrl(string Url, string Json) {
            try {
                HttpContent content = new StringContent(Json, Encoding.UTF8, "application/json");
                return WebAccess.PostAsync(Url, content).Result.Content.ReadAsStringAsync().Result;
            } catch (Exception e) {
                Console.WriteLine(e.Message + "\nFailed the post request to \"" + Url + "\" with \"" + Json + "\" in body.");
            }
            return null;
        }

        public static string DeleteJsonFromUrl(string Url) {
            try {
               return WebAccess.DeleteAsync(Url).Result.Content.ReadAsStringAsync().Result;
            } catch (Exception e) {
                Console.WriteLine(e.Message + "\nFailed the delete request to \"" + Url + "\".");
            }
            return null;
        }

    }
}
