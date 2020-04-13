## Updates Log

### Msf 3.2.0

One time login feature is here!

Now you can sign in just once and get a unique token to use it as authentication id next time you signing in again. This feature is useful  to prevent input of username and password eachtime you are trying to sign in.

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
