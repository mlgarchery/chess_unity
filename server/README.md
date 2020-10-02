This is the chess server, made in Dart. Once two clients connected to it, the game starts.

## Requirements:
- dart 
- (optional) dart2native if you want to create an executable

## Launch with :
```
dart src/server.dart <YourIPAddress> <YourPort>
```
default ip address is 198.168.1.25 (you probably have to change it), default port is 5000.

You can also build and executable of it.
```
dart2native src/server.dart -o /bin/server.exe
```



