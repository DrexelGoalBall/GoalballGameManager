using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GoalballGameManager
{
    // Delegate for method to be called when LineReceive event is triggered
    public delegate void LineReceive(UserConnection sender, string Data);

    /// <summary>
    ///     Handles sending and receiving data through stream writer/reader for a client connection
    /// </summary>
    public class UserConnection
    {
        // Event to trigger when a line is received
        public event LineReceive LineReceived;

        // Constant read buffer size and the byte array read buffer
        const int READ_BUFFER_SIZE = 255;
        private byte[] readBuffer = new byte[READ_BUFFER_SIZE];

        // The TCPClient for the connection
        private TcpClient client;
        
        // The Name property uniquely identifies the user connection
        private string strName;
        public string Name
        {
            get
            {
                return strName;
            }
            set
            {
                strName = value;
            }
        }

        /// <summary>
        ///     Constructor to set the client and start read thread
        /// </summary>
        /// <param name="client"></param>
        public UserConnection(TcpClient client)
        {
            this.client = client;
            // Starts the asynchronous read thread to save data into readBuffer
            this.client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(StreamReceiver), null);
        }

        /// <summary>
        ///     Users StreamWriter to send a message to the user
        /// </summary>
        /// <param name="Data"></param>
        public void SendData(string Data)
        {
            // Ensure that no other threads try to use the stream at the same time
            lock (client.GetStream())
            {
                StreamWriter writer = new StreamWriter(client.GetStream());
                writer.Write(Data + (char)13 + (char)10);
                // Make sure all data is sent now
                writer.Flush();
            }
        }

        /// <summary>
        ///     Callback function for TcpClient.GetStream.Begin, starts an asynchronous read from a stream
        /// </summary>
        /// <param name="ar"></param>
        private void StreamReceiver(IAsyncResult ar)
        {
            int BytesRead;
            string strMessage;

            try
            {
                // Ensure that no other threads try to use the stream at the same time
                lock (client.GetStream())
                {
                    // Finish asynchronous read into readBuffer and get number of bytes read
                    BytesRead = client.GetStream().EndRead(ar);
                }

                // Convert the byte array the message was saved into, minus one for the carriage return (char 13)
                strMessage = Encoding.ASCII.GetString(readBuffer, 0, BytesRead - 1);
                LineReceived(this, strMessage);
                // Ensure that no other threads try to use the stream at the same time
                lock (client.GetStream())
                {
                    // Start a new asynchronous read into readBuffer
                    client.GetStream().BeginRead(readBuffer, 0, READ_BUFFER_SIZE, new AsyncCallback(StreamReceiver), null);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
    }
}
