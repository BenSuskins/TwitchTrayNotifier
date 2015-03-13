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
#endregion
///Twitch Live stream notifier program
///By Ben Suskins 12/03/15
///Version 0.1
///Known Issues:
///-Doesn't Work
namespace TwitchTrayNotifier
{
    public partial class SettingsForm : Form
    {
        #region Global Variables
        NotifyIcon twitchActiveTray;
        Icon twitchIcon;
        Thread twitchLiveWorkerThread;
        string Username;
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
            MenuItem progNameMenuItem = new MenuItem("Twitch Live Notifier - By Ben Suskins 2015 V0.1");
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

            //Start up thread to check if channel live
            twitchLiveWorkerThread = new Thread(new ThreadStart(TwitchChannelLiveThread));
            twitchLiveWorkerThread.Start();

        }


        #endregion

        #region Context Menu, Quit / Settings Functions
        //When settings menu item is clicked allow them to enter twitch details
        void settingsMenuItem_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;

        }
        //When quit menu item is clicked exit the program and dispose of tray icons
        void quitMenuItem_Click(object sender, EventArgs e)
        {
            twitchIcon.Dispose();
            this.Close();
        }
        #endregion

        #region Thread to check if live
        private void TwitchChannelLiveThread()
        {

        }
        #endregion

        #region Username storage / Application
        //Take the username from textbox and store in file for use when program closed and save it
        private void btnSave_Click(object sender, EventArgs e)
        {
            //On click take txtUsername and save to file
            Username = txtUsername.Text;
            MessageBox.Show("Your twitch username is: " + Username);
            this.WindowState = FormWindowState.Minimized;
                
        }
        #endregion

    }
}
