## Updates Log

### Msf 3.8.2

​Mirror fixes and features.

- Renamed all Character classes to PlayerChatacter.
- Fixed error in MirrorRoomClient when system tryes to assign roomId.
- Fixed bug in MirrorRoomClient. When client gets access to room and when access failed. You now can go back to offlineScene or stop player.
- Fixed bug in MirrorRoomClientStarter when it is destroyed it will be successfully unregistered fomr connection listener.
- Added information output to MirrorRoomPlayer.
- Added ProfileFactory method to MirrorRoomServer. This will help you initialize player's profile info on the server side.
- Added GetRoomPlayerByMirrorPeer method to MirrorRoomServer. This will help you to find MirrorRoomPlayer by Mirror peer id.
- Added GetRoomPlayerByMsfPeer method to MirrorRoomServer. This will help you to find MirrorRoomPlayer by Msf peer id.
- Added GetRoomPlayerByUsername method to MirrorRoomServer. This will help you to find MirrorRoomPlayer by Msf username.

MSF fixes and features.

- ProfilesManager class is now extendable.
- Fixed bug in ObservableProperty. When you set new property value it is now checked if value is the same or not to optimize networking logic.
- Another bugs and errors fixes.

Shared fixes and features.

- CreateNewRoomView now has Password field.
- GameItem outputs the information about if room has password or does not.
- GamesListView now chack if room has password or does not and shows dialog box to enter the password if it exists.

Tools fixes and features.

- Added UIProgressProperty that will help you to create HUD components such as Health bar, Stamina bar etc.

### Msf 3.8.1

​Mirror fixes and features.

- Now the MirrorRoomServer can load player profile on server. This will help you to use server side player profile and change in mirror room server.​
- Fixed bug in CharacterMovement and CharacterTdMovement scripts.
- Another fixes

### Msf 3.8.0

Added Top down character controller. 

- CharacterTdLook - is responsible for controlling top down camera and character angle. It also useful for root motion animator controller to control movement animation axis.
- CharacterTdMovement - is responsible for character movement. It also takes into account data from ​CharacterTdLook  to move character according to angle of the character.

### Msf 3.7.0

Added Mirror FPS character controller:

- CharacterBehaviour - generic class.
- CharacterMovement - is responsible for character movement.
- CharacterInput - is responsible for using input system of Unity and control character.
- CharacterFpsLook - fps mode camera controller.
- CharacterAvatar - is responsible for network avatar of character.

### Msf 3.6.3

Made some changes and improvements to MirrorRoomServer and MirrorRoomClient

### Msf 3.6.2

Added systems:
- AccountsBehaviour - is responsible for managing of user account​
- MatchmakingBehaviour​ - helps to create simple game match on master server and connect to created game room
- MirrorRoomClient - manages connection to room server and is responsible for getting access to room server with the help of​ access token and getting data about room server from master server
- MirrorRoomClientStarter - helps to start ​MirrorRoomClient and connect to MirrorRoomServer
- MirrorRoomServer - ​​​is responsible for managing of  room players and their profiles and accounts. It is also responsible for starting Mirror Server
- MirrorNetworkManager - is a extension of NetworkManager  and​  main system to manage the ​MirrorRoomClient ​and MirrorRoomServer . It has events you can register to listen to NetworkManager callbacks​

Shared:

- This folder has another useful systems to help you to start your prototype demo fast and easily.​

### Msf 3.6.1

 - Delete old version before import

### Msf 3.6.0

- Demo scenes are fully remade
- Deleted deprecated classes
- Bug fixes

**Mirror demos are coming! Stay tuned!!!**

- Spawn rooms and use NetworkManager and NetworkRoomManager
- Matchmaking system

### Msf 3.5.3

- When room is full client is disconnected by server, but stay the in the same scene. Now after disconnection client goes back to offline scene
- Some changes in lobby
- Another Bugs and errors fixed 

### Msf 3.5.2

- Another Bugs and errors fixed

### Msf 3.5.1

- Bugs and errors fixed

### Msf 3.5.0

**Mirror official support is integrated**

- MirrorRoomManager - controls the room server and client
- MirrorRoomServer - controls all the logic of mirror room server side
- MirrorRoomClient - controls all the logic of mirror room client side
- MirrorRoomPlayer - is a holder of mirror room player info

**Added Async methods to IAccountsDatabaseAccessor**

- void GetAccountByUsernameAsync(string username, GetAccountCallback callback)
- void GetAccountByTokenAsync(string token, GetAccountCallback callback)
- void GetAccountByEmailAsync(string email, GetAccountCallback callback)
- void SavePasswordResetCodeAsync(IAccountInfoData account, string code, Action<string> callback)
- void GetPasswordResetDataAsync(string email, GetPasswordResetCallback callback)
- void SaveEmailConfirmationCodeAsync(string email, string code, Action<string> callback)
- void GetEmailConfirmationCodeAsync(string email, GetEmailConfirmationCodeCallback callback)
- void UpdateAccountAsync(IAccountInfoData account, Action<string> callback)
- void InsertNewAccountAsync(IAccountInfoData account, Action<string> callback)
- void InsertTokenAsync(IAccountInfoData account, string token, Action<string> callback)

Fixed token saving issue. When guest client is authenticated an empty token is saving. There was no possibility to sign in again after you stop game.

### Msf 3.4.0 - 3.4.1

- Spawning process is finished
- Created spawn menu
- Created games list menu
- Create RoomServerBehaviour class
- Create RoomClientBehaviour class


Now you can test how to spawn **[Mirror](https://github.com/vis2k/Mirror)** rooms and list tham in games list view.


### Msf 3.3.1

- Many bugs fixed
- Made some improvement

### Msf 3.3.0

- Class **BaseClientModule** is removed
- Fix error with **Msf.Runtime.ProductKey**
- Added method **RemoveHandler** to **MsfBaseClientModule**
- Added new system of getting your **public IP**

### Msf 3.2.0

One time login feature is here!

Now you can sign in just once and get a unique token to use it as authentication id next time you signing in again. This feature is useful  to prevent input of username and password eachtime you are trying to sign in.

To see how this feature is working open scene **BasicAuthorization** client and take a look at **​AccountManager** class. You can also see some changes in **MsfAuthClient** and **AuthModule​** classes.

### Msf 3.1.0 - Attention! Significant changes! This version is not compatible with earlier ones. Before update test it in new project.

- Created new cass **DictionaryOptions**. It is implemented in many classes  derived from **ISerializablePacket**. There are 65 positions this class is used in. This class helps easily you work with custom options data. In this regard, many changes were made in source code.
- Made some documentation for methods right in source code. This approach of documenting the scripts will be used further as well.
- I nearly have finished **SpawnersDemo** scene, but you can see it right now in package.

### Msf 2.3.2 - Attention! Remove previous Barebones folder and than reimport new.

- MsfArgNames
 - SpawnId (-msfSpawnId) is renamed to SpawnTaskId (-msfSpawnTaskId)
 - SpawnCode(-msfSpawnCode) is renamed to SpawnTaskUniqueCode (-msfSpawnTaskUniqueCode)
- ClientsSpawnRequestPacket
 - CustomArgs is renamed to CustomOptions
 - Region is removed
- SpawnRequestPacket
 - CustomArgs is renamed to CustomOptions
 - SpawnId is renamed to SpawnTaskId
 - SpawnCode is renamed to SpawnTaskUniqueCode
 - Properties is renamed to Options
- Fixed bug with Region. Now if you set region as empty all rooms created with this region will be International
- BaseClientModule is renamed to MsfBaseClientModule
 - Now this class can listen to network messages

### Msf 2.3.1 - Attention! Remove previous Barebones folder and than reimport new.

Simple bug fixes.

- Room **region** is now created as parameter in **RoomOptions**
- Fixed **"Error while handling a message from Client. OpCode: 30021, Error: System.InvalidOperationException: The current state of the connection is not Open"**

### Msf 2.3.0 - Attention! Remove previous Barebones folder and than reimport new.

- Added **ISpawnerController** interface
- **SpawnerController** class is rewritten and now is extendable.
- Made some changes to **SpawnerBehaviour** and added two fields
 - **usePublicIp** - will help use use public IP address for your rooms
 - **region** - now you can set region in inspector
- **MsfArgs**
 - Added **StartSpawner** that will help to start spawner automatically
 - Added **RoomRegion** that will help you to set your spawner region from cmd

### Msf 2.2.4 - Attention! Remove previous Barebones folder and than reimport new.

- **RoomServerBehaviour** can be started registered both with spawner and just as standalone application. Use **startRoomAsProcess** field to control this feature. For more info see its script.
- **ConnectionToMaster** is now **ConnectionHelper** that can be extended to your own connection helper class.
- For quick connection to master use **ClientToMasterConnector**
- **SpawnerController** added some changed. Removed some static methods.
- **SpawnerBehaviour**  added some changed.
- **RoomsModule, SpawnersModule**. Made some fixes and added log messages. To see whole process of room spawning.

### Msf 2.2.3
---
- BaseClientModule is now more extandable
- Started to create Spawner demo
- Fixed bug in RegisteredSpawner class
- Fixed bug in SpawnerController class
- Fixed bug in SpawnTask class
- Fixed bug in SpawnerBehaviour class
- Fixed bug in SpawnersModule class
