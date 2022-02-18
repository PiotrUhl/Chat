# Chat
Projekt na IX semestrze studiów z przedmiotów Platforma .NET, Technologie Mobilne oraz Organizacja i Rozwój Projektów Open-Source.
## Table of Contents
- [General info](#general-info)
- [Installation](#installation)
  - [Server installation](#server-installation)
  - [Client installation](#client-installation)
- [Developer setup](#developer-setup)
- [User guide](#user-guide)
- [Communication protocol](#communication-protocol)
## General info
This is simple slient-server chat application written as project for *.NET Platform*, *Mobile technologies* and *Organization and development of open-source projects* in second semester of graduate studies.
Application is divided into server part, working as Windows Service, and client desktop application.
Application allows for simple text communication between logged users.
## Installation

### Server installation
Unpack downloaded server archive and run following command as administrator:
```
$ Server.exe Install
```
Server service will be installed in your system.
After (or before, it doesn't matter) installing service edit *Service.dll.config* file and set listening IP and port.
To start service use Windows Service Manager, or run following command:
```
$ Server.exe Start
```
To stop service use Windows Service Manager, or run following command:
```
$ Server.exe Stop
```
To uninstall service run following command:
```
$ Server.exe Uninstall
```
### Client installation
Unpack downloaded client archive.
Edit *Client.dll.config* file and set server IP and port.
Run *Client.exe* to run application.
### Developer setup
Open downloaded source with Visual Studio 2019.
## User guide
After launching client application, enter your credentials and confirm with button or Enter key.
After successful login, you will see main application window. In left section there will be all contacts you messaged with. Contacts with new messages will be bolded. On right section, there will be box with messages of selected contact. If no contact is selected, box will be empty. To select a contact just left click it on contact list. Messeges on left are recived messages, on right are sent. To send message type it in box at bottom and confirm with button at botton-right corner.
## Communication protocol
Application is using its own communication protocol based on raw TCP protokol. Messages are divided into two groups: request from clients to server and answers from server to client. Every message is stared with control byte, determining its type.
#### Syntax:
- Message name (size_of_segment name of segment) *(size_of_segment name of segment sent one or more times)*\* - description of message
  - Answer name (size_of_segment name of segment) (size_of_segment name of segment) - description of answer
- Check (1B type)(4B SenderId) (8B MessageId) - checks if there are any messages newer than *MessageId* for client *SenderId*
  - CheckNoNew (1B type) - no new messages
  - CheckNew (1B type) (n\*4B ClientsIds) - new messages from *ClientsIds* available
- GetNew (1B type) (4B Client1Id ) (4B Client2Id) (8B MessageId) (1B limit) - gets *limit* newest messages, newer than *MessageId* between *Client1Id* and *Client2Id*
  - Get (1B type) *((1B ControlByte1) (8B MessageId) (nB Message) (1B '\0'))*\* - *ControlByte1* 1 means message is sent, 3 means recieved; message text is terminated with null character, next byte after null determines if next message (1 or 3) or end; *ControlByte2* 0 means no more messages, 4 means more messages pending
- New (1B type) (4B Client1Id) (4B Client2Id) (nB Message) - sends new message from *Client1Id* to *Client2Id* 
  - New (1B type) (4B Client1Id) (4B Client2Id) (8B MessageId) - new message *MessageId* sent from *Client1Id* to *Client2Id* 
- Read (1B type) (8B MessageId) - marks *MessageId* as read
  - Read (1B type) - response, does nothing
- GetClient (1B type) (4B ClientId) - gets display name of *ClientId*
  - GetClient (1B type) (nB ClientName) - requested *ClientName*
- LogIn (1B type) (32B PassHash) (nB Login) - tries to login as user with *Login* and *PassHash*
  - (LogIn 1B type) (4B ClientId) - id of logged client if succes, -1 if not
