using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace P2P {
    public class FileRepository {
        private Dictionary<string, Uri> _fileLocationMap;

        public FileRepository() {
            _fileLocationMap = new Dictionary<string, Uri>();
        }

        public FileRepository(Dictionary<string, Uri> fileLocationMap) {
            _fileLocationMap = fileLocationMap;
        }

        public bool AddFileWithLocation(string fileName, Uri location) {
            if(_fileLocationMap[fileName] == null) {
                _fileLocationMap.Add(fileName, location);
                return true;
            }
            return false;
        }

        public Uri GetLocationForFile(string fileName) {
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
