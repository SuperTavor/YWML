![logo](https://i.imgur.com/8KkfMAj.png)

# YWML

**YWML** is an easy, fun, and efficient mod loader for the **Yo-kai Watch** series on 3DS!  

Instead of distributing the entire game assets archive, you can simply publish only the files you edited and let your users install them with YWML's intelligent mod layering system.  


<a href="https://github.com/supertavor/ywml/releases/latest" target="_blank">
  <img alt="Download YWML" src="https://img.shields.io/badge/Download-YWML-blue?style=for-the-badge&logo=github" width="300">
</a>



------------------

## 🎮 How to Load a Mod with YWML

### **Step 1: Download the appropriate extension**
- Open the extension library and select the correct region for your game.  
- This ensures autoinstallation works correctly.  

![The extension library window](https://i.imgur.com/5UMZXN8.png)

### **Step 2: Select your target game**
- In the main menu, click **Load** and choose the appropriate game extension you downloaded.  

![TargetGame](https://i.imgur.com/qYaT25q.png)

### **Step 3: Generate your installation directory**
**For 3DS with microSD inserted:**
1. Select **Modded3DS** in the Platform/Emulator dropbox and click **Generate installation dir**.  
2. Select your microSD drive (e.g., `D:/`) and click **Select**.  

**For emulators:**
1. Select your emulator from the Platform/Emulator dropbox.  
2. Click **Generate installation dir**.  

✅ Your mod installation directory field should now be filled automatically.

### **Step 4: Add and install your mod**
1. Click **Add mod** and select your mod folder.  

![mod](https://i.imgur.com/ca9EuZK.png)

2. Click **Install selected mods** to install all mods in the list.  

![mod](https://i.imgur.com/OoTnOET.png)

🎉 **You're done! Enjoy!**  

---

## 🛠 How to Make Your Mod Compatible with YWML

### **Option 1: Migrate an existing FA mod**
1. Go to the YWML main menu and click **Migrate old mod**.  
2. Fill out mod information (Name, Author, Version).  

![migrate info](https://i.imgur.com/WqveTpz.png)

3. Select your mod folder.  

![select folder](https://i.imgur.com/fZNmVnk.png)  
*Ensure your folder matches the structure in the screenshot.*

4. Select your **unmodified RomFS folder** (usually the original dumped RomFS from your game).  
   - To dump RomFS: open your 3DS emulator, right-click your game, and select **Dump RomFS**.  
   - After dumping, select the folder that matches your mod folder's structure.  

![dump romfs](https://i.imgur.com/9OPGoov.png)

5. Click **Migrate** and select the folder where you want the YWML mod to be saved.

---

### **Option 2: Create a YWML mod from scratch**

1. Sort your mod files in a loose fashion inside an include folder. For example, if I edited data/menu/title_screen.xa, This is how my folder structure will look:
```
-MyMod
    -include
        -data
            -menu
                -title_screen.xa
```
3. Right outside of the include folder, create a file called ywml.json and paste the following into it:
```json
{
    "Name": "Your mod's name",
    "Author": "Your name",
    "Version": "Your mod's version"
}
```

Fill out all of the fields! *But wait, what if you edited files that are already loose, like files in mov or snd? Well, it's super simple to integrate! Simply paste your mov and snd folders, for example, right outside the include folder!*

✅ **Your mod is now ready to load with YWML!**

---

### ⭐ Happy Mod Loading!
