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


### Happy mod loading!
