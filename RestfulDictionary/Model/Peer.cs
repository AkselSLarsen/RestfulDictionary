using RestfulDictionary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestfulDictionary.Model {
    public class Peer : IJsonAble<Peer> {
        private uint? _ipv4;
        private long? _ipv6;
        private int _port;
        private List<string> _files;

        public uint? IPv4 {
            get { return _ipv4; }
            set { _ipv4 = value; }
        }

        public long? IPv6 {
            get { return _ipv6; }
            set { _ipv6 = value; }
        }

        public int Port {
            get { return _port; }
            set { _port = value; }
        }

        public List<string> Files {
            get { return _files; }
            set { _files = value; }
        }

        public Peer() {

        }

        public Peer(uint ip, int port) {
            IPv4 = ip;
            Port = port;
        }

        public Peer(long ip, int port) {
            IPv6 = ip;
            Port = port;
        }

        public Peer(uint ip, int port, List<string> files) {
            IPv4 = ip;
            Port = port;
            Files = files;
        }

        public Peer(long ip, int port, List<string> files) {
            IPv6 = ip;
            Port = port;
            Files = files;
        }

        public string IPv4AsString() {
            string re = "";

            re += (_ipv4 % uint.MaxValue) + ".";
            re += (_ipv4 % (256 * 256 * 256)) + ".";
            re += (_ipv4 % (256 * 256)) + ".";
            re += (_ipv4 % 256);

            return re;
        }

        public void IPv4FromString(string value) {
            if(!String.IsNullOrEmpty(value)) {
                long re = 0;
                string[] values = value.Split(".");
                for (int i = 0; i < values.Length; i++) {
                    try {
                        re += uint.Parse(values[i]) * (i + 1);
                    } catch (Exception e) {
                        Console.WriteLine(e);
                    }
                }
                _ipv4 = (uint)re;
            }
        }

        public string IPv6AsString() {
            throw new NotImplementedException();
        }

        public void IPv6FromString(string value) {
            throw new NotImplementedException();
        }

        public string ToJson() {
            return JsonSerializer.Serialize(this, typeof(Peer));
        }

        public override bool Equals(object obj) {
            if (obj != null && GetType() == obj.GetType()) {
                Peer peer = (Peer)obj;
                if ((this.IPv4 == peer.IPv4 && this.Port == peer.Port) || (this.IPv6 == peer.IPv6 && this.Port == peer.Port)) {
                    return true;
                }
            }
            return false;
        }
    }
}
