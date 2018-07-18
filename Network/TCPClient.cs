using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace DataManager.Network
{
    /// <summary>
    /// Class to send messages to server
    /// </summary>
    public class TCPClient
    {
        /// <summary>
        /// Server IP
        /// </summary>
        public static String ServerAddress = String.Empty;

        /// <summary>
        /// Server Port
        /// </summary>
        public static Int32 ServerPort = 0;

        /// <summary>
        /// Sends message to server via TCP Client
        /// </summary>
        /// <param name="message">Message to send</param>
        /// <param name="server">server address, can be empty string if ServerPort and ServerAddress is set</param>
        /// <param name="port">server address, can be empty string if ServerPort and ServerAddress is set</param>
        /// <param name="success">Message send successfully</param>
        /// <returns>Response from Server</returns>
        public static String sendMessage(String message, String server, out bool success)
        {
            success = false;

            String response = "";


            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer 
            // connected to the same address as specified by the server, port
            // combination.

            TcpClient client = new TcpClient(ServerAddress, ServerPort);

            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

            NetworkStream stream = client.GetStream();

            // Send the message to the connected TcpServer. 
            stream.Write(data, 0, data.Length);


            // Receive the TcpServer.response.

            // Buffer to store the response bytes.
            data = new Byte[2048];


            // Read the first batch of the TcpServer response bytes.
            Int32 bytes = stream.Read(data, 0, data.Length);
            response = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

            // Close everything.
            stream.Close();
            client.Close();

            success = true;

            return response;
        }
    }
}
