#region Imports
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Net;
#endregion

///Twitch Live stream notifier program
///By Ben Suskins 12/03/15
///Version 0.5
///Known Issues:
///-Only shows tells you if the channel you entered in settings form is live
///-Need to Assign an array to all the channels that you follow then cycle through them all in the thread

#region Program
namespace TwitchTrayNotifier
{
    public partial class SettingsForm : Form
    {
        #region Global Variables
        //Notification tray icon variable
        NotifyIcon twitchActiveTray;
        //Tray icon location
        Icon twitchIcon;
        //Thread to check if a channel goes live
        Thread twitchLiveWorkerThread;
        //Store the username that was entered in Settings form
        string Username = "Bannanser";
        //Array to store all the channels followed
        string[] Follows;
        #endregion

        #region Form Hiding & Setting up tray notification
        public SettingsForm()
        {
            InitializeComponent();
            //Allow for pressing 'enter' to save username
            this.AcceptButton = btnSave;
            //Set up the icon image
            twitchIcon = new Icon("twitch.ico");

            //Make notification tray icon active
            twitchActiveTray = new NotifyIcon();
            twitchActiveTray.Icon = twitchIcon;
            twitchActiveTray.Visible = true;

            //Create context menu items to the notification tray icon
            MenuItem progNameMenuItem = new MenuItem("Twitch Live Notifier - By Ben Suskins 2015 V0.5");
            MenuItem breakMenuItem = new MenuItem("-");
            MenuItem settingsMenuItem = new MenuItem("Settings");
            MenuItem quitMenuItem = new MenuItem("Quit");

            //Add context menu items to the tray icon
            ContextMenu contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(progNameMenuItem);
            contextMenu.MenuItems.Add(breakMenuItem);
            contextMenu.MenuItems.Add(settingsMenuItem);
            contextMenu.MenuItems.Add(quitMenuItem);
            twitchActiveTray.ContextMenu = contextMenu;

            //Allow the buttons to have a function when clicked
            settingsMenuItem.Click += settingsMenuItem_Click;
            quitMenuItem.Click += quitMenuItem_Click;

            //Minimize the form and then stop it from showing in task bar
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            //Creates array for follows with 9 entries
            Follows = new string[9];

            // Read the file that stores username and sets sUsername to the value stored.
            System.IO.StreamReader myFile = new System.IO.StreamReader("E:\\username.txt"); //Set to actual location for the text document
            Username = myFile.ReadToEnd();
            myFile.Close();
            txtUsername.Text = Username;

            //Start up thread to check if channel live
            twitchLiveWorkerThread = new Thread(new ThreadStart(TwitchChannelLiveThread));
            twitchLiveWorkerThread.Start();

        }


        #endregion

        #region Context Menu, Quit / Settings Functions
        //When settingsMenuItem clicked show settings form
        void settingsMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

        }
        //When quitMenuItem clicked close application and dispose of notification tray icon
        void quitMenuItem_Click(object sender, EventArgs e)
        {
            twitchIcon.Dispose();
            this.Close();
        }
        #endregion

        #region Thread to check if live
        private void TwitchChannelLiveThread()
        {
            try
            {
                //Thread runs repeatedly as it has now way to end
                while (true)
                {
                    //Checks to see if user is live
                    string sUrl = "https://api.twitch.tv/kraken/streams/" + Username;
                    HttpWebRequest wRequest = (HttpWebRequest)HttpWebRequest.Create(sUrl);
                    wRequest.ContentType = "application/json";
                    wRequest.Accept = "application/vnd.twitchtv.v3+json";
                    wRequest.Method = "GET";

                    dynamic wResponse = wRequest.GetResponse().GetResponseStream();
                    StreamReader reader = new StreamReader(wResponse);
                    dynamic res = reader.ReadToEnd();
                    reader.Close();
                    wResponse.Close();

                    //If they are live shows a balloon notification saying said user is live
                    if (res.Contains("display_name"))
                    {
                        //Set a BalloonTip to show which channel is live
                        twitchActiveTray.BalloonTipTitle = "A channel is now live";
                        twitchActiveTray.BalloonTipText = Username + " is now live";
                        twitchActiveTray.ShowBalloonTip(10000);
                    }

                    //Sleep for 1 minute then check if live again
                    Thread.Sleep(60000);
                }
            }
            catch (ThreadAbortException tbe)
            {


            }
        }
        #endregion

        #region Username storage / Application
        //Take the username from textbox and store in file for use when program closed and save it
        private void btnSave_Click(object sender, EventArgs e)
        {
            //Declare Username equal to the text box on the Settings form
            Username = txtUsername.Text;

            //Check that the user entered a value for username
            if (Username != "")
            {
                //Notify user that username is stored, save it to file and then minimize the program
                MessageBox.Show("Your twitch username is: " + Username
                + " and is stored for future use.");
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;

                //Write the username they entered to file for later use
                System.IO.StreamWriter myFile = new System.IO.StreamWriter("E:\\username.txt"); //Need to set actual location for file
                myFile.WriteLine(Username);
                myFile.Close();
                this.Hide();
            }
            else
            {
                //Let the user know that the username isn't valid
                MessageBox.Show("The username you entered isn't valid, please try again");
                txtUsername.Text = "";
            }

        }
        #endregion

    }
}
#endregion
