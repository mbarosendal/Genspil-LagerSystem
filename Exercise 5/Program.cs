using ProjektGenspil;

Menu newMenu = new Menu();
newMenu.LoadGamesFromFile();

Console.Write("Welcome to Genspil menusystem! Please press enter to proceed to the menu. ");
Console.ReadLine();

// Keeps user in the loop.
while (true)
{
    Console.Clear();
    Console.WriteLine("Please select an option below: \n");

    Console.Write("1) Show games \n2) Search games \n3) Add, edit or remove games Exit \n4) Show requests \n5) Exit \n\nPlease select an option: ");

    int menuChoice = int.Parse(Console.ReadLine());

    // Top menu.
    switch (menuChoice)
    {
        // Show the menu.
        case 1:
            Console.Clear();
            newMenu.ShowGames();    // ++ sort, print
            continue;
        case 2:
            Console.Clear();
            //newMenu.SearchGames();
            continue;
        case 3:
            Console.Clear();
            newMenu.AddEditRemoveMenu();
            continue;
        case 4:
            Console.Clear();
            //newMenu.ShowRequests();
            continue;
        case 5:
            Console.Clear();
            Console.WriteLine("Thank you for using the service. You may close the program now.");
            return;
        default:
            Console.Clear();
            Console.WriteLine("Please select a valid option. Press <enter> to try again.");
            Console.ReadLine();
            continue;
    }
}
