﻿using RestfulDictionary.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RestfulDictionary.Model {
    public class FileEndPoint : IJsonAble<FileEndPoint> {
        private Peer _peer;
        private string _fileName;

        public Peer Peer {
            get { return _peer; }
            set { _peer = value; }
        }

        public string FileName {
            get { return _fileName; }
            set { _fileName = value; }
        }

        public FileEndPoint() {

        }

        public FileEndPoint(Peer peer, string fileName) {
            Peer = peer;
            FileName = fileName;
        }

        public string ToJson() {
            return JsonSerializer.Serialize(this, typeof(FileEndPoint));
        }

        public override bool Equals(object obj) {
            if (obj != null && GetType() == obj.GetType()) {
                FileEndPoint file = (FileEndPoint)obj;
                if (this.Peer == file.Peer && this.FileName == file.FileName) {
                    return true;
                }
            }
            return false;
        }
    }
}
