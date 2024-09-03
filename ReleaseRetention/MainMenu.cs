public static class MainMenu
{
    public static void RunMainMenuLoop()
    {
        // int key;
        ConsoleKey key;
        do
        {
            Console.Write($"\nDevOps Deploy\n-------------\n");
            Console.Write($"\n\tP. Projects");
            Console.Write($"\n\tR. Releases");
            Console.Write($"\n\tD. Deployments");
            Console.Write($"\n\tE. Environments");
            Console.Write($"\n\n> ");

            // key = char.ToUpper(Convert.ToChar(Console.Read()));
            key = Console.ReadKey().Key;
            switch (key)
            {
                // Projects
                case ConsoleKey.P:
                    Console.WriteLine(DataContext.Projects.ToString(nameof(DataContext.Projects)));
                    break;

                // Releases
                case ConsoleKey.R:
                    Console.WriteLine(DataContext.Releases.ToString(nameof(DataContext.Releases)));
                    break;

                // Deployments
                case ConsoleKey.D:
                    Console.WriteLine(DataContext.Deployments.ToString(nameof(DataContext.Deployments)));
                    break;

                // Environments
                case ConsoleKey.E:
                    Console.WriteLine(DataContext.Environments.ToString(nameof(DataContext.Environments)));
                    break;

                case ConsoleKey.Escape:
                case ConsoleKey.Q:
                    Console.WriteLine($"Exiting...");
                    return;

                default:
                    Console.Error.WriteLine($"Unknown key: {key}");
                    break;
            }

        } while (!ConsoleKey.Escape.Equals(key) && !ConsoleKey.Q.Equals(key));
    }
}