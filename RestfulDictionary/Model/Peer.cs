﻿using RestfulDictionary.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestfulDictionary.Model {
    public class Peer : IJsonAble<Peer> {
        private long? _ipv4;
        private string? _ipv6;
        private int _port;
        private List<string> _files;

        public long? IPv4 {
            get { return _ipv4; }
            set { _ipv4 = value; }
        }

        public string? IPv6 {
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

        public Peer(long? ip, int port) {
            IPv4 = ip;
            Port = port;
        }

        public Peer(string ip, int port) {
            if(IPv4FromString(ip) >= 0) {
                IPv4 = IPv4FromString(ip);
            } else {
                IPv6 = ip;
            }
            Port = port;
        }

        public Peer(long? ip, int port, List<string> files) {
            IPv4 = ip;
            Port = port;
            Files = files;
        }

        public Peer(string ip, int port, List<string> files) {
            if (IPv4FromString(ip) >= 0) {
                IPv4 = IPv4FromString(ip);
            } else {
                IPv6 = ip;
            }
            Port = port;
            Files = files;
        }

        public Peer(long? ipv4, string ipv6, int port, List<string> files) {
            IPv4 = ipv4;
            IPv6 = ipv6;
            Port = port;
            Files = files;
        }

        public static string IPv4AsString(long ip) {
            string re = "";

            re += (ip % (256L * 256L * 256L * 256L) + ".");
            re += (ip % (256 * 256 * 256)) + ".";
            re += (ip % (256 * 256)) + ".";
            re += (ip % 256);

            return re;
        }

        public static long IPv4FromString(string value) {
            if(!String.IsNullOrEmpty(value)) {
                long re = 0;
                try {
                    string[] values = value.Split(".");
                    for (int i = 0; i < values.Length; i++) {
                        re += long.Parse(values[i]) * (i + 1);
                    }
                    return re;
                } catch(Exception e) {
                    Console.WriteLine(e);
                }
            }
            return -1;
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
