# TwitchTrayNofitier
This application uses C# in Visual Studio. It creates a tray notification when a designated twitch.tv channel goes live.

This application makes use of background threads which update once a minute, it then checks using the [Twitch API](https://github.com/justintv/Twitch-API)to check if a channel is indeed live. This is done by checking the json file for a string that indicates the selected channel is live, if it is a notification is given to the user.

This application runs mainly in the background with a task menu icon available to change settings such as the channel you want to be notified about.

Planned Features:

-Enter own twitch channel then the program will find your followers and then check them one by one rather than just one selected channel.
