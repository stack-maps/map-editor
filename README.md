# Stack Maps - Map Editor
This repository contains code to create and modify library floor plans specified in the companion repository [here](https://github.com/stack-maps/server). It is a [Unity3D](https://unity3d.com) project and can be deployed on multiple platforms. To modify the editor requires a copy of Unity engine, which is free at the time of writing.

To start editing an existing database, simply download the latest editor from the releases tab and run the application.

## Usage Guide
When the editor is launched, it will show a login screen where server information and optional username/password are asked for. Once the server responds with granted access, the editor will load its content. For specific howtos and help, refer to the help section in the editor.

## Security
Since no credential information should be sent as clear-text over the internet, the editor employs RSA encryption on the entered username and password before sending it to the access API. The API will then decrypt the content and forward the information to a third-party authorization site.

Once access has been granted, the editor receives a token, a string that acts as the access card to the protected API calls. This token has an expiry duration (which gets refreshed every time a new call has been made). The token becomes void when the user logs out of the system.
