using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace DataManager.Network
{
    /// <summary>
    /// Multithreaded TCP Data Receiver to achieve data and rend response
    /// </summary>
    public class TCPServer
    {
        public delegate String ClientMessageHandler(String message);

        #region classMembers

        public ClientMessageHandler OnClientMessage = null;

        private ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        private int portNumber;

        private static object synchClientObject;

        #endregion

        public TCPServer(int portNumber)
        {
            this.portNumber = portNumber;
            synchClientObject = new object();
        }

        #region Connection Functions

        public void start()
        {
            try
            {

                IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), portNumber);
                TcpListener listener = new TcpListener(endpoint);
                listener.Start();

                Console.WriteLine("TCP Listener started ... ");

                while (true)
                {
                    tcpClientConnected.Reset();
                    listener.BeginAcceptTcpClient(new AsyncCallback(ProcessIncomingConnection), listener);
                    tcpClientConnected.WaitOne();
                }

            }
            catch (Exception)
            { }
        }

        private void ProcessIncomingData(object obj)
        {
            lock (synchClientObject)
            {

                TcpClient client = (TcpClient)obj;
                StringBuilder sb = new StringBuilder();

                String response = "";

                NetworkStream stream = client.GetStream();

                if (stream.CanRead)
                {
                    byte[] myReadBuffer = new byte[2048];

                    int numberOfBytesRead = 0;

                    // Incoming message may be larger than the buffer size.
                    do
                    {
                        numberOfBytesRead = stream.Read(myReadBuffer, 0, myReadBuffer.Length);

                        sb.AppendFormat("{0}", Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead));

                    }
                    while (stream.DataAvailable);
                }

                String request = sb.ToString();

                if (OnClientMessage != null)
                {
                    response = OnClientMessage(request);
                }

                if (String.IsNullOrEmpty(response))
                    response = "NO REPLY " + '\0';

                stream.Write(Encoding.ASCII.GetBytes(response), 0, response.Length);

                
            }
        }

        private void ProcessIncomingConnection(IAsyncResult ar)
        {
            try
            {

                TcpListener listener = (TcpListener)ar.AsyncState;
                TcpClient client = listener.EndAcceptTcpClient(ar);

                ThreadPool.QueueUserWorkItem(ProcessIncomingData, client);
                tcpClientConnected.Set();

            }
            catch(Exception)
            { }
        }

        #endregion

        
    }
}
