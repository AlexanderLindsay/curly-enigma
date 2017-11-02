# Introduction
A game built with [F#](http://fsharp.org/) and [MonoGame](http://www.monogame.net/) for the [2017 Game Off](https://itch.io/jam/game-off-2017) game 

# Getting Started
This project was built using [Visual Studio Code](https://code.visualstudio.com/) as a development environment and that is the setup that is described here. Visual Studio should work as well, but is not described.

1. Download [Visual Studio Code](https://code.visualstudio.com/Download) and install
1. Install [dotnet core](https://www.microsoft.com/net/core)
1. Install [F#](http://fsharp.org/)
1. Install the three [Ionide visual studio code extensions](https://marketplace.visualstudio.com/search?term=publisher%3A%22Ionide%22&target=VSCode&category=All%20categories&sortBy=Relevance)
1. Install the [C# visual studio code extensions](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp)
1. Clone the project
1. Open up the Visual Studio Code Command Prompt (`Ctrl+Shift+P`) and run `Debug: Download .NET Core Debugger`
1. Run .paket/paket.exe install
1. Press `F5` to launch the game in debug mode

While the game is being built with Monogame and could in theory support additional platforms, it currently only supports windows.