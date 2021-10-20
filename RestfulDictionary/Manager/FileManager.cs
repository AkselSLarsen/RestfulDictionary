using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulDictionary.Manager {
    public abstract class FileManager {
        private static readonly List<FileEndPoint> Data = new List<FileEndPoint>();

        public static List<FileEndPoint> GetAll() {
            return new List<FileEndPoint>(Data);
        }

        public static void Add(FileEndPoint newFile) {
            Delete(newFile);

            Data.Add(newFile);
        }

        public static FileEndPoint Delete(FileEndPoint file) {
            bool deleted = Data.Remove(file);
            return deleted ? file : null;
        }

        public static List<FileEndPoint> NameToFiles(string fileName) {
            List<FileEndPoint> re = new List<FileEndPoint>();
            foreach(FileEndPoint file in Data) {
                if(file.FileName == fileName) {
                    re.Add(file);
                }
            }
            return re;
        }

        public static List<FileEndPoint> Get(string filename) {
            List<FileEndPoint> re = new List<FileEndPoint>();
            foreach (FileEndPoint file in Data) {
                if (file.FileName == filename) {
                    re.Add(file);
                }
            }
            return re;
        }

        public static List<FileEndPoint> Get(Peer peer) {
            List<FileEndPoint> re = new List<FileEndPoint>();
            foreach (FileEndPoint file in Data) {
                if(file.Peer.Equals(peer)) {
                    re.Add(file);
                }
            }
            return re;
        }

        public static FileEndPoint Get(string filename, Peer peer) {
            FileEndPoint re = null;
            foreach (FileEndPoint file in Data) {
                if (file.Peer.Equals(peer)) {
                    if(file.FileName == filename) {
                        re = file;
                    }
                }
            }
            return re;
        }
    }
}