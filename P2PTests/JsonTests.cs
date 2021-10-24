using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestfulDictionary.Interfaces;
using RestfulDictionary.Model;
using System.Collections.Generic;

namespace P2PTests {
    [TestClass]
    public class JsonTests {
        [TestMethod]
        public void ConvertToJson() {
            Peer peer = new Peer(527, "[2AB1::F437]", 2121, new List<string>());

            string expectedJson = "{\"IPv4\":527,\"IPv6\":\"[2AB1::F437]\",\"Port\":2121,\"Files\":[]}";

            Assert.AreEqual(expectedJson, peer.ToJson());
        }

        [TestMethod]
        public void ConvertFromJson() {
            string json = "{\"IPv4\":527,\"IPv6\":\"[2AB1::F437]\",\"Port\":2121,\"Files\":[]}";

            Peer expectedPeer = new Peer(527, "[2AB1::F437]", 2121, new List<string>());

            Assert.AreEqual(expectedPeer, IJsonAble<Peer>.FromJson(json));
        }

        [TestMethod]
        public void ConvertFromJsonArray() {
            string json = "[{\"IPv4\":527,\"IPv6\":\"[2AB1::F437]\",\"Port\":2121,\"Files\":[]}]";

            Peer expectedPeer = new Peer(527, "[2AB1::F437]", 2121, new List<string>());

            Assert.AreEqual(expectedPeer, IJsonAble<Peer>.FromJsonArray(json)[0]);
        }
    }
}
