<!-- Badges -->
[![GitHub issues](https://img.shields.io/github/issues/Kuborros/FreedomManager?style=flat)](https://github.com/Kuborros/FreedomManager/issues)
[![GitHub forks](https://img.shields.io/github/forks/Kuborros/FreedomManager?style=flat)](https://github.com/Kuborros/FreedomManager/network)
[![GitHub stars](https://img.shields.io/github/stars/Kuborros/FreedomManager?style=flat)](https://github.com/Kuborros/FreedomManager/stargazers)
[![GitHub license](https://img.shields.io/github/license/Kuborros/FreedomManager?style=flat)](https://github.com/Kuborros/FreedomManager/blob/master/LICENSE)
# FreedomManager

A simple mod manager capable of setting up BepInEx, as well as installing all of my current mods.

## Installation:

To install the manager, copy the .exe from the downloaded zip to the main game directory.  
You can find it from Steam, by right clicking on the game on the list, and then pressing "Manage > Browse local files".  
![xqB3yB7](https://user-images.githubusercontent.com/33236735/195651639-d54b74e7-ce74-486c-b094-a0fde05dbc81.png)

.NET framework 4.8 is required for the loader to function. It is included by default in Windows 10 and above. 
It has been tested to work on Windows 7 as well.
Vista/XP lack the required .NET Runtime to support it - however the game is not completable on these systems due to DX9 shader bug in Lunar Cannon, so lack of support for these systems is acceptable.

## Usage:

On initial startup, you will be prompted to install BepInEx - simply click on the `Install BepInEx` button to download, and install, recommended version of the loader. The file is securely downloaded from the official GitHub repository over https://.  

From there you can use `Install Mod` button to select a zipfile to install.  
The files are checked for basic validity (containing a BepInEx folder in its root, A MelonLoader Mods folder or a mod manifest json).  
Mods using mod_overrides are also properly extracted.  
If you wish to install an old MelonLoader compatible mod instead, you must install the MelonLoader compatibility layer with the `Install MelonLoader Compat` button before running any MelonLoader mods. You do **NOT** need it for normal use. 

Loader also offers a list of currently installed mods, enabling or disabling of them, support for enabling a debug console and 1-click integration on GameBanana!    
![image](https://github.com/Kuborros/FreedomManager/assets/22860063/9793599a-8221-4a55-8161-5119f36f53cd)  

Mod manager supports automatic updates for itself, installed mods that support this feature, as well as its support library [FP2Lib](https://github.com/Kuborros/FP2Lib).

## Configuration options:

Configuration tab allows you to edit most of BepInEx settings, as well as controll features of FP2Lib and Automatic Updates.
You can also set custom launch parameters here!

![image](https://github.com/Kuborros/FreedomManager/assets/22860063/38f7ef0f-056c-463f-bf65-8143a2cce1d9)

## Notes:

If you don't trust random .exe from the internet, thats good!  
You can check the code (including solution files, you can sneak _spicy_ things in there), and compile it yourself from the github repo.
Visual Studio 2022 will happily load and set it all up for you. 

Additionally, here is the VirusTotal scan result: [Here](https://www.virustotal.com/gui/file/0910a97edddff134e20a00a1ad43b973428c57b45db60d5e194becdb5112ef3b). I know MaxSecure shows it as "sus" (literaly), but if you check online its a common issue, like in [this example](https://www.reddit.com/r/antivirus/comments/qo9vus/is_this_safe_and_false_positive_from_virustotal/). And their 'report false positive' webpage 404s me.  

As for Theta, let their official site speak for itself:
"Bitdefender Theta: All detections are based on **machine learning** which do not include detection created by malware researchers. *High chances of false positive.*"

## For modders:

This mod manager installs any BepInEx mod as long as it has a proper directory structure, as recommended by BepInEx devs and GameBanana (so main directory of the zip maps to main game directory)  

MelonLoader mods can be supported, if they implement a valid directory structure. Using these mods is however not recommended.

Please read our Wiki documentation on making mods [here](https://github.com/Kuborros/FreedomManager/wiki)!  

## Building:
Clone and open the solution in Visual Studio 2022, then build the project - all the needed dependencies should be automatically pulled from nuGet repository.
