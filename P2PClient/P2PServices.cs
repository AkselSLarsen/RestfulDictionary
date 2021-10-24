using RestfulDictionary.Interfaces;
using RestfulDictionary.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
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

        /// <summary>
        /// If the message is split by one space then the peer we get the file from is a random peer with the file.
        /// If the message is split by three spaces, then we get the file from the specified peer.
        /// </summary>
        /// <param name="message"></param>
        public static void DownloadFile(string message) {

            string file = AskForFile();

            string[] splitMessage = message.Split(" ");

            if (splitMessage.Length == 1) {
                string peersAsJson = WebAccessor.GetJsonFromUrl(WebAccessor.GetPeersWithFileUrl(file));

#warning Delete debug info
                Console.WriteLine("_______________________________1");
                Console.WriteLine(peersAsJson);

                Peer[] peers = IJsonAble<Peer>.FromJsonArray(peersAsJson);

#warning Delete debug info
                Console.WriteLine("_______________________________2");
                foreach(Peer p in peers) { Console.WriteLine(p.ToString()); }

                DownloadFileFromPeer(file, peers[new Random().Next(0, peers.Length)]);
            } else if(splitMessage.Length == 3) {
                DownloadFileFromPeer(file, new Peer(splitMessage[1], int.Parse(splitMessage[2])));
            } else { // if something goes wrong
                throw new ArgumentException("message parameter must be a string with either two or no spaces");
            }
        }

        private static string AskForFile() {
            Console.WriteLine("Please write the name of the desired file.");
            return Console.ReadLine();
        }

        private static void DownloadFileFromPeer(string filename, Peer peer) {
            P2PClientSocket client = null;

#warning Delete debug info
            Console.WriteLine("_______________________________3");
            Console.WriteLine(peer.IPv4);

            if (peer.IPv4 != null) {
                client = new P2PClientSocket(IPAddress.Parse(Peer.IPv4AsString(peer.IPv4)), peer.Port);
            } else {
                client = new P2PClientSocket(IPAddress.Parse(peer.IPv6), peer.Port);
            }

#warning Delete debug info
            Console.WriteLine("_______________________________4");

            client.Writer.WriteLine("get" + filename);
            client.Writer.Flush();

            ReceiveFile(client.Client, filename);
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

        private static void ReceiveFile(TcpClient client, string filename) {
            NetworkStream ns = client.GetStream();

            string directory = Directory.GetCurrentDirectory();

            while (!directory.EndsWith("Simple P2P")) {
                int i = directory.LastIndexOf("\\");
                directory = directory.Remove(i);
            }

            directory += "\\downloads\\";

            FileStream file = File.Create(directory + filename);

            using (Stream stream = file) {
                ns.CopyTo(stream);
            }
            client.Close();
        }
    }
}