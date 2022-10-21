# FreedomManager

A simple mod manager capable of setting up BepInEx, as well as installing all of my current mods.

## Installation:

To install the manager, copy the .exe from the downloaded zip to the main game directory.  
You can find it from Steam, by right clicking on the game on the list, and then pressing "Manage > Browse local files".  
![xqB3yB7](https://user-images.githubusercontent.com/33236735/195651639-d54b74e7-ce74-486c-b094-a0fde05dbc81.png)

.NET framework 4.8 is required for the loader to function. It is included by default in Windows 10 and above.  

## Usage:

On initial startup, you will be prompted to install BepInEx - simply click on the "Install BepInEx" button to download, and install, recommended version of the loader. The file is securely downloaded from the official GitHub repository over https://.  

From there you can use "Install Mod" button to select a zipfile to install.  
The files are checked for basic validity (containing a BepInEx folder in its root).  
Mods using mod_overrides are also properly extracted.  

Loader also offers a basic list of currently installed mods. With the ability to view information about the mod and allows for mod removal via right-click context menu.  
![image](https://user-images.githubusercontent.com/33236735/197233273-1cc28b7e-05d7-423c-b721-283697d2c72b.png)  

To install MelonLoader mods you must first install the compatibility layer via the `Install MelonLoader Compat` button.
You may also choose to register the URL Handler, which will allow you to install mods via GameBanana's 1-click integration.  

## Notes:

If you don't trust random .exe from the internet, thats good!  
You can check the code, and compile it yourself from the github repo.  
Visual Studio 2022 will happily load and set it all up for you.  

Additionally, here is the VirusTotal scan result: [Here](https://www.virustotal.com/gui/file/0910a97edddff134e20a00a1ad43b973428c57b45db60d5e194becdb5112ef3b). I know MaxSecure shows it as "sus" (literaly), but if you check online its a common issue, like in [this example](https://www.reddit.com/r/antivirus/comments/qo9vus/is_this_safe_and_false_positive_from_virustotal/). And their 'report false positive' webpage 404s me.  

## For modders:

This mod manager installs any BepInEx or MelonLoader mod as long as it has a proper directory structure, as recommended by BepInEx devs and gamebanana (so main directory of the zip maps to main game directory)  

## Building:
Open the solution in Visual Studio 2022 and build the project :)
