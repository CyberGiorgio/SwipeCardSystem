using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SwipeCardSystem
{
    static class Menu       //this class implements the main menu
    {
        public static void MenuList()           //main menu displayed at opening
        {
            Console.WriteLine("Welcome to 'The Swipe Card System'\n\n Choices: \n " +
         "  [+] 1 - Add/update/remove rooms\n " +
         "  [+] 2 - View change room state\n " +
         "  [+] 3 - Alphabetical list users\n " +
         "  [+] 4 - Add/update/remove users from the list\n " +
         "  [+] 5 - Simulate user swiping into room\n " +
         "  [+] 6 - View users log file\n " +
         "  [+] 7 - Exit the program"
            );
        }
        public static void Case1MainMenu()          //case 1 of menu (add/remove/update) rooms
        {
            bool menuBreaker = false;   //rooms
            do
                {
                Console.Clear();
                Console.WriteLine("This is the List of the rooms inside the builiding");
                FileInteractor.JSONPrinter((FileInteractor.ChooseFile(1)));
                Console.WriteLine("Choose an option:\n [1]Add Room\n [2]Update Room\n [3]Remove Room\n [4]Back to menu\n");
                try
                {
                    int keyPressed = Convert.ToInt32(Console.ReadLine());
                    switch (keyPressed)
                    {
                        case 1:
                            Lists.Add(FileInteractor.ChooseFile(1));     //New Room
                            menuBreaker = true;
                            break;
                        case 2:
                            Lists.Update(FileInteractor.ChooseFile(1));      //  Update room;
                            menuBreaker = true;
                            break;
                        case 3:                                     
                            Lists.Remove(FileInteractor.ChooseFile(1));      // Remove room
                            menuBreaker = true;
                            break;
                        case 4:                                             //back to menu
                            menuBreaker = true;
                            break;
                        default:
                            Console.WriteLine("Invalid Input\n" +  //wrong input
                     "press any key to try again");
                            break;
                    }
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid Input\n" +  //wrong input
                     "press any key to try again");
                };
            } while (menuBreaker == false);
        }
        public static void Case2MainMenu(string filePath)       //change state of building rooms(Normal, Emergency)
        {
            Console.WriteLine("View change room state");
            JSONBuilding jsonBuilding = (JSONBuilding)FileInteractor.JSONReader(filePath);
            bool state = jsonBuilding.Building[0].Floors[0].Rooms[0].Normal;
            string normalEmergency;
            if(state == true)
            {
                normalEmergency = "Normal";
            }
            else
            {
                normalEmergency = "Emergency";
            }
            Console.WriteLine("The actual state of the building is: '" + normalEmergency + "'.\nDo you wish to change it?\n" +
                "press 'Y' to change it, or any other button to get back to the main menu\n");
            if (Console.ReadKey().Key == ConsoleKey.Y)      //press  Y to change state of all rooms
            {
                Floor floor;
                for (int k = 0; k < jsonBuilding.Building.Count; k++)
                {
                    for (int i = 0; i < jsonBuilding.Building[k].Floors.Count; i++)
                    {
                        floor = jsonBuilding.Building[k].Floors[i];
                        for (int j = 0; j < floor.Rooms.Count; j++)
                        {
                            if (jsonBuilding.Building[k].Floors[i].Rooms[j].Normal == true)
                            {
                                jsonBuilding.Building[k].Floors[i].Rooms[j].Normal = false;
                                jsonBuilding.Building[k].Floors[i].Rooms[j].Emergency = true;
                            }
                            else if (jsonBuilding.Building[k].Floors[i].Rooms[j].Normal == false)
                            {
                                jsonBuilding.Building[k].Floors[i].Rooms[j].Normal = true;
                                jsonBuilding.Building[k].Floors[i].Rooms[j].Emergency = false;
                            }
                        }
                    }
                }
                Console.WriteLine("\nState of the building changed, Press any button to get back to the main Menu");
                Console.ReadKey();
                string json = JsonConvert.SerializeObject(jsonBuilding, Formatting.Indented); //write jsonFile
                File.WriteAllText(FileInteractor.ChooseFile(1), json);
            }
        }
        public static void Case3MainMenu()  //print list of all users
        {
            Console.Clear();
            Console.WriteLine("Alphabetical list users");
            FileInteractor.JSONPrinter(FileInteractor.ChooseFile(3));
            Console.WriteLine("Press any button to get back to the main menu");
            Console.ReadKey();
        }
        public static void Case4MainMenu()          //case 4 of menu (add/remove/update) users
        {
            bool menuBreaker = false;   
            do
            {
                Console.Clear();
                Console.WriteLine("Choose an option:\n" +
                    "  [1]Add Person\n" +
                    "  [2]Update Person\n" +
                    "  [3]Remove Person\n" +
                    "  [4]Back to menu\n");
                try
                {
                    int keyPressed = Convert.ToInt32(Console.ReadLine());
                    switch (keyPressed)
                    {
                        case 1:
                          
                            Lists.Add(FileInteractor.ChooseFile(3));    //add person
                            Console.ReadKey();
                            menuBreaker = true;
                            break;
                        case 2:
                           Lists.Update(FileInteractor.ChooseFile(3));      //   Update person;
                            menuBreaker = true;
                            break;
                        case 3:
                            Lists.Remove(FileInteractor.ChooseFile(3));      // Remove person;
                            menuBreaker = true;
                            break;
                        case 4:
                            menuBreaker = true;             //back to menu
                            break;
                        default:
                            Console.WriteLine("Invalid Input\n" +  //wrong input
                        "press any key to try again");
                            break;
                    }
                }
                catch (FormatException)         //wrong input
                {
                    Console.WriteLine("Invalid Input\n" +   
                        "press any key to try again");
                };
                Console.ReadKey();
            } while (menuBreaker == false);
        }
        public static void Case5MainMenu()           //simulation
        {
            Console.Clear();
            JSONBuilding jsonBuilding = (JSONBuilding)FileInteractor.JSONReader(FileInteractor.ChooseFile(1));
            Console.WriteLine("Welcome to the " + jsonBuilding.Building[0].Id +
                "  \nThis is the List of the rooms inside the builiding");
                FileInteractor.JSONPrinter((FileInteractor.ChooseFile(1)));     //print full list of floors and rooms
            Console.WriteLine("Type the number of the floor you wish to visit to filter the list of Rooms");
            try
            {
                int floorNo = Convert.ToInt32(Console.ReadLine());
                int maxNoFloor = jsonBuilding.Building[0].Floors.Count;     //input floors and check if exists
                if (floorNo > maxNoFloor)
                {
                    Console.WriteLine("Invalid input, maximum number of floors = " + maxNoFloor);
                }
                else
                {
                    Console.Clear();
                    Floor floor = jsonBuilding.Building[0].Floors[floorNo];
                    Console.WriteLine("welcome to the floor " + floorNo);       //filter room in the floor
                    for (int i = 0; i < floor.Rooms.Count; i++)
                    {
                        Console.WriteLine("        Room: " + floor.Rooms[i].IdRoom +
                                            "\n           Type: " + floor.Rooms[i].Type);
                        if (floor.Rooms[i].Normal == true)
                        {
                            Console.Write("             State Normal: ");
                            Console.WriteLine(floor.Rooms[i].Normal);
                        }
                        else
                        {
                            Console.Write("         State Emergency: ");
                            Console.WriteLine(floor.Rooms[i].Emergency);
                        }
                    }
                    Console.WriteLine("Type the ID of the Room you wish to get");
                    string idRoom = Console.ReadLine();                                     //if room not found return back
                    if (Lists.LoopBuildingChecker(idRoom, FileInteractor.ChooseFile(1)).Item2 != floorNo)
                    {
                        Console.WriteLine("Input not valid, press any button to get back and try again");
                        Console.ReadKey();
                        return;
                    }
                    Room room = floor.Rooms[Lists.LoopBuildingChecker(idRoom, FileInteractor.ChooseFile(1)).Item1];
                    Console.Clear();                                                    //card reader identified
                    CardReader cardReader = (CardReader)Lists.CardReaderAssign(idRoom).Item1;
                    if (cardReader == null)
                    {
                        Console.WriteLine("Invalid ID Room, press any button and try again");
                        Console.ReadKey();
                        return;
                    }
                    FileInteractor.JSONPrinter(FileInteractor.ChooseFile(3));       //print list of persons for access o the room
                    Console.WriteLine("This is a " + room.Type + ".\nType the ID of the user trying to access " + idRoom + "?");
                    JSONPerson jsonPerson = (JSONPerson)FileInteractor.JSONReader(FileInteractor.ChooseFile(3));
                    JSONCard cards = (JSONCard)FileInteractor.JSONReader(FileInteractor.ChooseFile(4));
                    int idUser = Convert.ToInt32(Console.ReadLine());
                    int indexPerson = Lists.LoopPersonChecker(idUser, FileInteractor.ChooseFile(3));
                    Person person = jsonPerson.Person[indexPerson];
                    Card card = cards.Card[indexPerson];
                    Console.WriteLine("Your card ID is : " + card.Id);
                    person.SwipeCard(person);               //card swiped
                    cardReader.PersonRoomCheck(room, card, floorNo);        //check if access allowed
                }
            }
            catch (FormatException)          //not valid input
            {
                Console.WriteLine("Invalid Input, press any button and try again");
            }
            Console.WriteLine("Press any button to get back to the main menu");
            Console.ReadLine();
        }
        public static void Case6MainMenu()      //print log file of the day
        {
            FileInteractor.JSONPrinter(FileInteractor.ChooseFile(2));
            Console.ReadKey();
        }
    }
}
