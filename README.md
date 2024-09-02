# ⭐YWML⭐

![YWML's main window.](https://i.imgur.com/YA4f0rP.png)

**YWML** is an easy, fun and efficient mod loader for the Yo-kai Watch series on 3DS!

Instead of distributing the entire game assets archive, you can simply publish only the files you edited and let your users install them with YWML's intelligent mod layering system.


## This sounds awesome! But how can I make my mod compatible with YWML?
Well, it's really simple! ⭐

First, make sure you know exactly which files you edited in the FA, then extract them and sort them in a loose fashion inside an `include` folder. For example, if I edited `data/menu/title_screen.xa`, This is how my folder structure will look:
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

**You can now load your mod using YWML with the appropriate game extension!**

But wait, what even are YWML extensions? ⭐

In YWML, extensions can dynamically add support for specific games! To download an appropriate extension for your game, simply visit the Extension Library.

![The extension library window](https://i.imgur.com/5UMZXN8.png)

## How can I install a mod?
First, download the appropriate extension from the extension library

![The extension library window](https://i.imgur.com/5UMZXN8.png)

After that, in the main menu, click "Load" and choose the appropriate target game (should be from an extension you downloaded)

![TargetGame](https://i.imgur.com/qYaT25q.png)

Then, select your mod folder. on Citra, you can get it by right clicking your game -> open mods directory, then creating a `romfs` folder inside and selecting that folder. On 3DS, put the SD card into your computer, and navigate to `luma/titles/[your_title_id]/romfs`. You can get the appropriate title ID for your game [here](https://3dsdb.com/). Create any folders that don't exist to get to that path, and make sure you have game patching enabled.

Now, in your mod loading window, click "Add mod" and select your mod like so:

![mod](https://i.imgur.com/ca9EuZK.png)

Now, click "Install selected mods"! This will install all of the mods in the list.

![mod](https://i.imgur.com/OoTnOET.png)

You're done! Enjoy.


### Happy mod loading!
