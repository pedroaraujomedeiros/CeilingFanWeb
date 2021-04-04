# CeilingFan API + Web App
It is an app that contains a ceiling fan with 2 pull cords:   
  * One increases the speed each time it is pulled. There are 3 speed settings, and an “off” (speed 0) setting. If the cord is pulled on speed 3, the fan returns to the “off” setting.
  * One reverses the direction of the fan at the current speed setting. Once the direction has been reversed, it remains reversed as we cycle through the speed settings, until reversed again.

### A live web version is available at [Link](https://ceilingfan.azurewebsites.net) 

## Software Development Concepts Applied
  * C# / WPF
  * .NET 5 (lastest .NET version)
  * Dependency Injection
  * N-Tier layer
    * UI (Presentation) Layer
    * Service Layer
    * Domain Layer

## How to compile and run
  * To compile and run, you need to have Visual Studio and .NET 5 sdk insalled on your machine.
  * Follow the steps below:
    1. Clone this repository on your machine;
    2. Open the solution (.sln file) in Visual Studio;
    3. Define the "CeilingFanDesktopApp" project as the startup project;
    4. Run (Green arrow on top center of Visual Studio).

## UI - User Interface
![alt text](https://github.com/pedroaraujomedeiros/CeilingFanDesktop/blob/main/Snapshots/Desktop-MainWindow.PNG "Main Window")
