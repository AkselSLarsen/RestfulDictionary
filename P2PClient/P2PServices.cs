using RestfulDictionary.Interfaces;
using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace P2P {
    public abstract class P2PServices {
        public static void ServicePeer(P2PServerSocket server, TcpClient peer) {
            NetworkStream ns = peer.GetStream();
            StreamReader reader = new StreamReader(ns);
            StreamWriter writer = new StreamWriter(ns);

            string message = "";
            bool end = false;
            while (!end) {
                try {
                    message = ReadLines(reader);

                    if(message.ToLowerInvariant().StartsWith("get")) {
                        string fileName = message.Substring(3).Trim();
                        if(server.Repository.GetLocationForFile(fileName) != null) {

                            //writer.WriteLine("Sending file \"" + fileName + "\" now"); // Incase everything goes well, nothing besides the file should be sent.
                            
                            SendFile(peer, server.Repository.GetLocationForFile(fileName));
                            end = true;
                        } else {
                            writer.WriteLine("No file named " + fileName);
                            writer.Flush();
                        }
                    } else {
                        writer.WriteLine("Didn't understand, try again.");
                        writer.Flush();
                    }

                } catch (Exception e) {
                    Console.WriteLine(e.Message);
                    if (e != null) {
                        end = true;
                        writer.WriteLine("Server connection severed");
                        writer.Flush();
                    }
                }
            }
            peer.Close();
        }

        public static void GetFiles() {
            Console.WriteLine(WebAccessor.GetJsonFromUrl(WebAccessor.GetFilesUrl));
        }

        /// <summary>
        /// Either gets all peers if the filename is null or gets all peers with a file of the given filename.
        /// </summary>
        /// <param name="filename">A string containing the filename that all peers we are getting should have</param>
        public static void GetPeers(string filename = null) {
            if(filename == null) {
                Console.WriteLine(WebAccessor.GetJsonFromUrl(WebAccessor.GetPeersUrl));
            } else {
                Console.WriteLine(WebAccessor.GetJsonFromUrl(WebAccessor.GetPeersWithFileUrl(filename)));
            }
        }

        public static void DownloadFile(string message) {
#warning Not implemented            
        }

        /// <summary>
        /// Reads all lines from the reader and returns them in a single string.
        /// </summary>
        /// <param name="Reader">The reader to read from</param>
        /// <param name="re">An internal string used to concatenate the strings of the reader, for normal operations don't pass anything to this parameter</param>
        /// <returns>A single string containing all of the readers content</returns>
        public static string ReadLines(StreamReader Reader, string re = "") {
            re += Reader.ReadLine();

            if (Reader.Peek() >= 0) {
                re = ReadLines(Reader, re);
            }

            return re;
        }

        private static void SendFile(TcpClient peer, Uri uri) {
            if(File.Exists(uri.AbsolutePath)) {
                using (FileStream fileStream = File.OpenRead(uri.AbsolutePath)) {
                    if(fileStream.CanRead) {
                        fileStream.CopyTo(peer.GetStream());

                        //peer.Close(); Has to be done, but is taken care of after the code exits the while loop of the ServicePeer method.
                    
                    } else {
                        throw new Exception("File at " + uri.AbsolutePath + " wasn't readable");
                    }
                }
            } else {
                throw new Exception("File wasn't at " + uri.AbsolutePath);
            }
        }
    }
}