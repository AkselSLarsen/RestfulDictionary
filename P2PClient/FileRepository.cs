using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace P2P {
    public class FileRepository {
        private Dictionary<string, string> _fileLocationMap;

        public FileRepository() {
            _fileLocationMap = new Dictionary<string, string>();
        }

        public FileRepository(Dictionary<string, string> fileLocationMap) {
            _fileLocationMap = fileLocationMap;
        }

        public FileRepository(string directory) {
            _fileLocationMap = new Dictionary<string, string>();

            while (!directory.EndsWith("Simple P2P")) {
                int i = directory.LastIndexOf("\\");
                directory = directory.Remove(i);
            }

            directory += "\\files";

            foreach(string file in Directory.GetFiles(directory)) {
                _fileLocationMap.Add(file.Replace(directory + "\\", ""), file);
            }
        }

        public bool AddFileWithLocation(string fileName, string location) {
            if(_fileLocationMap[fileName] == null) {
                _fileLocationMap.Add(fileName, location);
                return true;
            }
            return false;
        }

        public string GetLocationForFile(string fileName) {
            return _fileLocationMap[fileName];
        }

        public List<string> GetFileNames() {
            return new List<string>(_fileLocationMap.Keys);
        }

        public List<FileEndPoint> ToFileEndPoints(Peer peer) {
            List<FileEndPoint> re = new List<FileEndPoint>();

            foreach(string file in _fileLocationMap.Keys) {
                re.Add(new FileEndPoint(peer, file));
            }

            return re;
        }
    }
}
