using YWML.Src.Utils.Tinifan.ArchiveL5.Level5.Archive.ARC0;
using YWML.Src.Utils.Tinifan.ArchiveL5.Tools;

namespace YWML.Src.Utils.Arc0Ex
{
    public class CARC0Ex : ARC0
    {
        public CARC0Ex(FileStream fs)
        {
            this.BaseStream = fs;
            this.Directory = this.Open();
        }
        public void AddOrReplace(string filePath, byte[] data)
        {
            //To ensure consistent formatting
            filePath = filePath.Replace("\\", "/");

            var parts = filePath.Split("/");
            VirtualDirectory currentDirectory = this.Directory;
            for(int i = 0; i < parts.Length; i++)
            {
                //If last part, look for files.
                if(i == parts.Length - 1)
                {
                    //if this is still false by the end of our search, we'll create one.
                    bool isFoundFile = false;
                    for(int j = 0; j < currentDirectory.Files.Count; j++) 
                    {
                        var fileName = currentDirectory.Files.Keys.ToList()[j];
                        //Meaning we found our file
                        if(fileName == parts[i])
                        {
                            //To be quite honest - I'm not sure what this class is lol. Seemingly Tini didn't wanna use a regular MemoryStream?
                            var newMs = new SubMemoryStream(data);
                            //Replace data
                            currentDirectory.Files[fileName] = newMs;
                            isFoundFile = true;
                            break;
                        }
                    }

                    if(!isFoundFile)
                    {
                        currentDirectory.AddFile(parts[i],new SubMemoryStream(data));
                    }
                }
                //else, just look for folders
                else
                {
                    var targetFolder = parts[i];
                    bool isFoundFolder = false;
                    foreach(var folder in currentDirectory.Folders)
                    {
                        if(folder.Name == targetFolder)
                        {
                            currentDirectory = folder;
                            isFoundFolder = true;
                            break;
                        }
                    }
                    //if this is reached the folder wasn't found so we should create it and traverse into it
                    if(!isFoundFolder)
                    {
                        currentDirectory.AddFolder(targetFolder);
                        currentDirectory = currentDirectory.SearchDirectories(targetFolder)[0];
                    }
                
                }
            }
        }

    }
}
