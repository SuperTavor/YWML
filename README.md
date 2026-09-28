![logo](https://i.imgur.com/8KkfMAj.png)


**YWML** is an easy, fun and efficient mod loader for the Yo-kai Watch series on 3DS, available on **Windows** and **Android**.

Instead of distributing the entire game assets archive, you can simply publish only the files you edited and let your users install them with YWML's intelligent mod layering system.

**AI disclaimer if someone gaf: Most of YWML has been written fully by human hands, but the 1.2.0 update features some AI assisted code. This is not a vibe coded project. The `AGENTS.md` is provided so contributors can have their agents not absolutely fuck my codebase up.**

## Features

- **Windows and Android** — the same mods and extensions, on your PC or your phone.
- **Extensions** — add support for a specific game from the built-in Extension Library.
- **Add mods from a folder or an archive** — loose folders, or `.zip` / `.7z` archives that YWML unpacks for you.
- **Local install** — write straight into an emulator's folder (Citra, Azahar, Azahar+, Mandarine) or onto a 3DS SD card, and optionally specify the install path yourself for more advanced use cases. 
- **Remote install** — upload the patched archive directly to a modded 3DS over FTP (`ftpd`).
- **Smart mod layering** — Choose your mod priority so you can actually load multiple mods without manually merging FAs


## How can I install a mod?

### 1. Pick your target game
Open **Load** and choose your game from the target-game list. This list only shows the extensions you have installed, and YWML remembers your last choice.

Can't find your game or region? Click **"Can't find your game/region? Add it from here"** to jump to the Extension Library and download the appropriate extension for your game and region.

![TargetGame](https://i.imgur.com/qYaT25q.png)

### 2. Add your mods
Click **Add mod**. On Windows you can pick a **folder** or an **archive** (`.zip` / `.7z`); on Android you pick an **archive**. Folder support for Android may come in the future if demand arises. 

![mod](https://i.imgur.com/ca9EuZK.png)


### 3. Choose where to install

**Local install** writes into an emulator's folder or a 3DS SD card.
- *Emulator:* On Desktop, YWML can automatically detect the RomFS folder for your game, depending on your emulator. on Android, you need to select your emulator's `User` folder (e.g. Azahar) before the Load process through the onboarding guide or the Settings. To find it, open the emulator's settings and look for the option that shows/opens the user folder.
- *3DS SD card:* Optionally, if you don't want to use FTP for any reason, you can pop your 3ds microSD into your PC and have YWML automatically detect the correct loading folder for it after selecting the drive.

**Remote install** uploads straight to a modded 3DS running an FTP server (`ftpd`). Enter your 3DS's IP address displayed on the home screen of `ftpd` (port `5000` by default) when prompted.

### 4. Install
Click **INSTALL MODS** and wait for the loading screen.

![mod](https://i.imgur.com/OoTnOET.png)

You're done! Enjoy.



## But wait, what even are YWML extensions? ⭐

In YWML, extensions can dynamically add support for specific games. To install one for your game, open the **Extension Library** and pick it from its category. While it downloads, YWML shows the unpack progress, and installed extensions are listed at the top under **Installed** so you can uninstall them at any time.

![The extension library window](https://i.imgur.com/5UMZXN8.png)



## This sounds awesome! But how can I make my mod compatible with YWML?
Well, it's really simple! ⭐


### If you're making a mod from scratch, and want it to be compatible from YWML from the get go for easy testing and collaboration
Make sure you know exactly which files you edited in the FA, then extract them and sort them in a loose fashion inside an `include` folder. For example, if I edited `data/menu/title_screen.xa`, This is how my folder structure will look:
```
-MyMod
    -include
        -data
            -menu
                -title_screen.xa
```

Now, let's prepare our YWML project configuration!

Right outside of the `include` folder, create a file called `ywml.json` and paste the following into it:
```json
{
    "Name": "Your mod's name",
    "Author": "Your name",
    "Version": "Your mod's version"
}
```

Fill out all of the fields! 

But wait, what if you edited files that are already loose, like files in `mov` or `snd`? Well, it's super simple to integrate! Simply paste your `mov` and `snd` folders, for example, right outside the `include` folder!

### If you have an existing FA mod, and want to be able to play it with other mods at the same time and distribute only the files you edited
You can use the **Migrate mod** feature. Simply click on the *Migrate old mod* button on the home screen (Only available on PC), then dump your game's vanilla RomFS and select it, then select your mod and fill out the metadata fields.

That's it!

**You can now load your mod using YWML with the appropriate game extension!**


## Android

The first time you open YWML on Android, a short setup guide walks you through it. Most importantly, it asks you to select your emulator's **User** folder — YWML needs it to install mods locally. You can change it later, or re-run the guide, from **Settings**.

> Please use an emulator installed from its official website — the Play Store builds of Azahar and others can't access the files YWML needs.


### Happy mod loading!
