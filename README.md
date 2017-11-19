# Game for Game Off 2017 (title tbd)

[<img src="https://aml-code.visualstudio.com/_apis/public/build/definitions/d0e34213-d2b1-43e2-98a1-89d3dffa820a/3/badge"/>](https://aml-code.visualstudio.com/Github-Integration/_build/index?definitionId={3})

## Introduction
A game built with [F#](http://fsharp.org/) and [MonoGame](http://www.monogame.net/) for the [2017 Game Off](https://itch.io/jam/game-off-2017) game 

## Getting Started
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

## Content Pipeline

New textures and other content to the project will have to be built using the Monogame Content Pipeline, which comes with Monogame when it is installed.

1. Download and Install [Monogame](http://www.monogame.net/downloads/)
1. Open the `contentPipeline/contentBuilder.mgcb` with the Pipeline program
1. Add the new content to the files folder in `contentPipeline/files`
1. Use the build command in the Pipeline program
1. FAKE will take care of moving the generated files to the content folder in GameOff2017, and MSBuild will take it from there

## Notes

While the game is being built with Monogame and could in theory support additional platforms, it currently only supports windows.
