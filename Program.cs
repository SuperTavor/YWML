using ywml_console;

class Program
{
    static string mode = "";
    static void Main(string[] args)
    {
        if(args.Length < 2)
        {
            Console.WriteLine("args: ywml [create/load] [path to modded romfs/path to patch file]");
            Environment.Exit(1);
        }
        ValidateArguments(args);
        if (mode == "create")
        {
            if (args.Length < 5)
            {
                Console.WriteLine("When packing a mod, this is the arg syntax: ywml create [path to modded romfs] [mod name] [mod version] [mod author]");
                Environment.Exit(1);
            }
            pack pack = new pack();
            pack.Pack(args);
        }
        else if (mode == "load")
        {
            load load = new load();
            load.Load(args);
        }

    }
    static void ValidateArguments(string[] args)
    {
        if (args[0] != ("create") && args[0] != ("load"))
        {
            Console.WriteLine("args: ywml [create/load] [path to modded romfs/path to patch file]");
            Environment.Exit(1);
        }
        else
        {
            mode = args[0];
        }
    }
}