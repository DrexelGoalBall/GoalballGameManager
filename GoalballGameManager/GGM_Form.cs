using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GoalballGameManager
{
    /// <summary>
    ///     The main form for the application that listenings for user connections and updates the UI accordingly
    /// </summary>
    public partial class GGM_Form : Form
    {
        #region Variables

        // Dictionary containing all of the users connected to this application
        private Dictionary<string, UserConnection> clients = new Dictionary<string, UserConnection>();
        // Dictionary containing all of the games currently hosted on the server
        private Dictionary<int, Process> games = new Dictionary<int, Process>();

        // The port in which clients must make the initial connection to
        private const int MASTER_PORT = 7000;
        // The first port number to host games on
        private const int START_PORT = 7001;
        // The number of ports available to host on (therefore, the number of allowed games)
        private int numPorts = 10;
        
        // TCP Listener and corresponding thread
        private TcpListener listener;
        private Thread listenerThread;

        // Commands that can be received
        private const string RCV_CONNECT_GGM = "CONNECT";
        private const string RCV_DISCONNECT_GGM = "DISCONNECT";
        private const string RCV_REQUESTUSERS = "REQUESTUSERS";
        private const string RCV_REQUESTGAMES = "REQUESTGAMES";
        private const string RCV_CREATEGAME = "CREATEGAME";
        //private const string RCV_ENDGAME = "ENDGAME";
        private const string RCV_CHAT = "CHAT";

        // Commands that can be sent
        private const string SND_JOIN_GGM = "JOIN";
        private const string SND_REFUSE = "REFUSE";
        private const string SND_LISTUSERS = "LISTUSERS";
        private const string SND_LISTGAMES = "LISTGAMES";
        private const string SND_PORT = "PORT";
        private const string SND_CHAT = "CHAT";
        private const string SND_BROADCAST = "BROAD";

        #endregion Variables

        #region Initialization

        /// <summary>
        ///     Default constructor to initialize components
        /// </summary>
        public GGM_Form()
        {
            //This call is required by the Windows Form Designer.
            InitializeComponent();
        }

        #endregion Initialization

        #region Connection

        /// <summary>
        ///     Check for valid incoming connection and send back a JOIN request
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="sender"></param>
        private void ConnectUser(string userName, UserConnection sender)
        {
            string uName = userName;
            //int duplicate = 1;

            //while (clients.ContainsKey(uName))
            //{
            //    uName = userName + "_" + duplicate;
            //    duplicate++;
            //}

            if (clients.ContainsKey(uName))
            {
                ReplyToSender(SND_REFUSE, sender);
            }
            else
            {
                sender.Name = uName;
                // Update the list and UI to show the new client
                clients.Add(sender.Name, sender);
                this.Invoke((MethodInvoker)(() => lstPlayers.Items.Add(sender.Name)));
                // Send a JOIN to sender
                ReplyToSender(SND_JOIN_GGM, sender);
                UpdateStatus(string.Format("{0} has joined.", sender.Name));
                // Notify all other clients that sender joined
                SendToClients(string.Format("{0}|{1} has joined.", SND_CHAT, sender.Name), sender);
            }
        }

        /// <summary>
        ///     Client has left, so remove all references from the application
        /// </summary>
        /// <param name="sender"></param>
        private void DisconnectUser(UserConnection sender)
        {
            // Update the list and UI to show the loss of the client
            clients.Remove(sender.Name);
            this.Invoke((MethodInvoker)(() => lstPlayers.Items.Remove(sender.Name)));
            UpdateStatus(string.Format("{0} has left.", sender.Name));
            // Notify all other clients that sender joined
            SendToClients(string.Format("{0}|{1} has left.", SND_CHAT, sender.Name), sender);
        }

        #endregion Connection

        #region Listening

        /// <summary>
        ///     Background listener thread to allow reading incoming messages without lagging the user interface
        /// </summary>
        private void DoListen()
        {
            try
            {
                // Listen for new connections
                listener = new TcpListener(System.Net.IPAddress.Any, MASTER_PORT);
                listener.Start();

                do
                {
                    // Create a new user connection using TcpClient
                    UserConnection client = new UserConnection(listener.AcceptTcpClient());
                    // Create an event handler to allow the UserConnection to communicate with the window
                    client.LineReceived += new LineReceive(OnLineReceived);
                    UpdateStatus("new connection found: waiting for log-in...");
                } while (true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary>
        ///     Event handler when UserConnection receives a full line
        ///     Parses the command and parameters to take the appropriate action
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="data"></param>
        private void OnLineReceived(UserConnection sender, string data)
        {
            // Split on carriage return
            string[] dataArray = data.Split((char)13);
            // Loop through all of the commands
            for (int i = 0; i < dataArray.Length; i++)
            {
                // For debugging, write every command received
                Debug.WriteLine(dataArray[0]);

                // Split on '|' and determine the command received (command = 0, info = 1)
                string[] commandAndInfo = dataArray[i].Split((char)124);
                switch (commandAndInfo[0])
                {
                    case RCV_CONNECT_GGM:
                        ConnectUser(commandAndInfo[1], sender);
                        break;
                    case RCV_DISCONNECT_GGM:
                        DisconnectUser(sender);
                        break;
                    case RCV_REQUESTUSERS:
                        ListUsers(sender);
                        break;
                    case RCV_REQUESTGAMES:
                        ListGames(sender);
                        break;
                    case RCV_CREATEGAME:
                        CreateGame(sender);
                        break;
                    //case RCV_ENDGAME:
                    //    EndGame();
                    //    break;
                    case RCV_CHAT:
                        SendChat(commandAndInfo[1], sender);
                        break;
                    default:
                        // Message is junk do nothing with it
                        break;
                }
            }
        }

        #endregion Listening

        #region Sending

        /// <summary>
        ///     Concatenate all the client names and send them to the user who requested user list
        /// </summary>
        /// <param name="sender"></param>
        private void ListUsers(UserConnection sender)
        {
            // Add the command to send list of users first
            string strUserList = SND_LISTUSERS;

            // Loop through all client dictionary keys and add them to the string
            foreach (string clientName in clients.Keys)
            {
                strUserList = strUserList + "|" + clientName;
            }

            UpdateStatus(string.Format("Sending {0} the list of users online.", sender.Name));
            // Send the list to the sender
            ReplyToSender(strUserList, sender);
        }

        /// <summary>
        ///     Concatenate all the game ports and send them to the user who requested
        /// </summary>
        /// <param name="sender"></param>
        private void ListGames(UserConnection sender)
        {
            // Add the command to send list of games first
            string strGameList = SND_LISTGAMES;

            // Loop through all game dictionary key values and add them to the string
            foreach (int portNumber in games.Keys)
            {
                strGameList = strGameList + "|" + portNumber;
            }

            UpdateStatus(string.Format("Sending {0} the list of games.", sender.Name));
            // Send the list to the sender
            ReplyToSender(strGameList, sender);
        }

        /// <summary>
        ///     Sends a message to all clients from the servers perspective
        /// </summary>
        /// <param name="strMessage"></param>
        private void Broadcast(string strMessage)
        {
            // Loop through all connected users and send them the message
            foreach (UserConnection userConn in clients.Values)
            {
                userConn.SendData(strMessage);
            }
        }

        /// <summary>
        ///     Sends a chat message from given sender to all other clients
        /// </summary>
        /// <param name="message"></param>
        /// <param name="sender"></param>
        private void SendChat(string message, UserConnection sender)
        {
            UpdateStatus(string.Format("{0}: {1}", sender.Name, message));
            // Notify all other clients of senders message
            SendToClients(string.Format("{0}|{1}: {2}", SND_CHAT, sender.Name, message), sender);
        }

        /// <summary>
        ///     Sends a message to all attached clients except the sender
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="sender"></param>
        private void SendToClients(string strMessage, UserConnection sender)
        {
            // Loop through all connected users and send them the message
            foreach (UserConnection userConn in clients.Values)
            {
                // Do not send to the sender
                if (userConn.Name != sender.Name)
                {
                    userConn.SendData(strMessage);
                }
            }
        }

        /// <summary>
        ///     Sends a response to the sender
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="sender"></param>
        private void ReplyToSender(string strMessage, UserConnection sender)
        {
            sender.SendData(strMessage);
        }

        #endregion Sending

        #region Game

        /// <summary>
        ///     Starts a game and tells the sender to join it
        /// </summary>
        /// <param name="sender"></param>
        private void CreateGame(UserConnection sender)
        {
            // Determine if there are any available ports
            int portNumber = -1;
            for (int i = START_PORT; i < START_PORT + numPorts; i++)
            {
                if (!games.ContainsKey(i))
                {
                    portNumber = i;
                    break;
                }
            }

            Debug.WriteLine(portNumber);
            
            // Check if an avilable port was found
            if (portNumber == -1)
            {
                ReplyToSender(string.Format("{0}|No ports available.", SND_CHAT), sender);
            }
            else
            {
                // Create the process for Goalball game
                Process process = new Process();
                process.StartInfo.FileName = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + "\\Goalball.exe";
                process.StartInfo.Arguments = "+port:" + portNumber;
                process.EnableRaisingEvents = true;
                process.Exited += new EventHandler(process_Exited);
                if (process.Start())
                {
                    // Add new game process to list of open games
                    games.Add(portNumber, process);
                    this.Invoke((MethodInvoker)(() => lstGames.Items.Add(portNumber)));
                    // Tell the sender the port to join the game
                    ReplyToSender(string.Format("{0}|{1}", SND_PORT, portNumber), sender);
                }
            }
        }

        /// <summary>
        ///     Event called when game process is closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void process_Exited(object sender, EventArgs e)
        {
            // Get the process that was closed
            Process proc = (Process)sender;
            foreach (int portNum in games.Keys)
            {
                // Find and remove the corresponding items in the game and UI list
                if (proc.Id == games[portNum].Id)
                {
                    games.Remove(portNum);
                    this.Invoke((MethodInvoker)(() => lstGames.Items.Remove(portNum)));
                    break;
                }
            }
        }

        #endregion Game

        #region Status

        /// <summary>
        ///     Displays the provided message as the newest status    
        /// </summary>
        /// <param name="statusMessage"></param>
        private void UpdateStatus(string statusMessage)
        {
            this.Invoke((MethodInvoker)(() => lstStatus.Items.Add(statusMessage)));
        }

        #endregion Status

        #region UI Events

        /// <summary>
        ///     Start the background listener thread
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Load(object sender, EventArgs e)
        {
            listenerThread = new Thread(new ThreadStart(DoListen));
            listenerThread.Start();
            UpdateStatus("Listener started");
        }

        /// <summary>
        ///     Stop the listener when the application closes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmMain_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            listener.Stop();
        }

        /// <summary>
        ///     Sends the contents of the Broadcast textbox to all clients
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBroadcast_Click(object sender, System.EventArgs e)
        {
            // Check if there is a message to send
            if (txtBroadcast.Text.Equals(string.Empty))
            {
                UpdateStatus(string.Format("Broadcasting: {0}", txtBroadcast.Text));
                Broadcast(string.Format("{0}|{1}", SND_BROADCAST, txtBroadcast.Text));
                // Clear the textbox
                txtBroadcast.Text = string.Empty;
            }
        }

        #endregion UI Events
    }
}
