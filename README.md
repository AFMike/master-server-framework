# Barebones Master Server Framework for Unity

[![](http://i.imgur.com/9SrutM0.png)](https://www.assetstore.unity3d.com/#!/content/71391)

[Asset Store Link](https://www.assetstore.unity3d.com/#!/content/71391)

## What Is This?

This is a repository of **Unity Asset** - The older version **Master Server Framework (2.0.4)**

## About Master Server Framework

Develop, run and debug your back-end server like a regular Unity game, within editor. No tedious multi-language setups, no third party services with monthly subscriptions, no cap on CCU or traffic, host on win and linux VPS, full source code - even if I die, you move forward. Based on socket connections (think Photon Server, just without caps and monthly payments).

Read ([wiki](https://github.com/alvyxaz/barebones-masterserver/wiki)) for more info. The older version **Master Server Framework (2.0.4)**.

**Discord Channel [https://discord.gg/raFXg83](https://discord.gg/raFXg83)**

---

Master Server Framework is designed to kickstart your back-end server development. It contains solutions to some of the common problems, such as:

-   **Authentication**
-   Persistent and synchronized player profiles
-   Lobby (game server registration / lookup)
-   Chat - with both private and public messages

Run your back-end servers within Unity:

-   Super fast development - servers can be started and debugged in unity’s editor, together with your game.
-   No need to share DLL’s, compile multiple projects, or develop in multiple languages.
-   **No third-party services** - develop offline, host wherever you want.
-   No issues with concurrency / race conditions

**Networking API**: framework is based on a thin layer of abstraction on top of **Websocket** protocol. It makes communication between your servers and clients a breeze. You can start as many **socket servers** and clients as you want.  

If you’re working on a rooms-based game, you can utilize **Spawner Server**. It can start game servers within your VPS'es / dedicated servers, automatically, on user’s request. It means that even **browser** players can create rooms! Players will no longer be required to host games themselves.  

Framework encourages you to run your game servers on your own VPS'es / dedicated servers. You can have games hosted on player-devices, but framework doesn't try to solve NAT issues, and usual connectivity problems will still be there, unless you host games on your own VPS / Dedicated server** Other Features:

-   **Unlimited CCU**
-   Full Source Code
-   Host on Windows and Linux platforms
-   Written in C#, no Reflection / AOT code used

Supported client platforms:  
WebGL, Windows, Mac, Android (iOS not yet tested)  

**Please note that this is a framework and not a complete project.** You’ll still need to make a game, and be a able to write C# code to utilize the framework.
