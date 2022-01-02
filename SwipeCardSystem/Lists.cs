using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace SwipeCardSystem
{
    public static class Lists       // this class add, remove, update lists
    {
        const string cleaner = "Cleaner";           //roles and rooms used within the code for easy updates
        const string emergency = "Emergency Resp";
        const string manager = "Manager";
        const string security = "Security";
        const string staffMember = "Staff Member";
        const string student = "Student";
        const string visitorGuest = "Visitor/ Guest";
        const string lectureHall = "Lecture Hall";
        const string staffRoom = "Staff Room";
        const string teachingRoom = "Teaching Room";
        const string secureRoom = "Secure Room";

        public static int LoopPersonChecker(int idUser, string filePath)    //looking for person index
        {
            JSONPerson jsonPerson = (JSONPerson)FileInteractor.JSONReader(filePath);
            int j = 0;
            for (int i = 0; i < jsonPerson.Person.Count; i++)
            {
                if (idUser == jsonPerson.Person[i].Id)
                {
                   j = i;
                }
            }
            return j;
        }
        public static (int, int, int) LoopBuildingChecker(string idRoom, string filePath)   //looking for room index
        {
            JSONBuilding jsonBuilding = (JSONBuilding)FileInteractor.JSONReader(filePath);
            int roomCounter = 0;
            int floorCounter = 0;
            int buildingCounter = 0;
            for (int k = 0; k < jsonBuilding.Building.Count; k++)
            {
                for (int i = 0; i < jsonBuilding.Building[k].Floors.Count; i++)
                {
                    for (int j = 0; j < jsonBuilding.Building[k].Floors[i].Rooms.Count; j++)
                    {
                        if (jsonBuilding.Building[k].Floors[i].Rooms[j].IdRoom == idRoom)
                        {
                            roomCounter = j;
                            floorCounter = i;
                            buildingCounter = k;
                        }
                    }
                }
            }
            return (roomCounter, floorCounter, buildingCounter);    // these indexes will give floor, room positions
        }
        public static void Update(string filePath)        //update room/person/card
        {
            Console.WriteLine("You chose Update");
            if (filePath == "jsonBuilding.json")       //update building
            {
                JSONBuilding structureOrganization = (JSONBuilding)FileInteractor.JSONReader(filePath);
                Console.WriteLine("Which room ID would you like to update?");
                string idRoom = Console.ReadLine();
                int indexRoom = LoopBuildingChecker(idRoom, filePath).Item1;    //indexes to replace looking for the room to update
                int indexFloor = LoopBuildingChecker(idRoom, filePath).Item2;
                int indexBuilding = LoopBuildingChecker(idRoom, filePath).Item3;
                Console.WriteLine("Which is te new ID?");           //id room
                string newId = Console.ReadLine();
                Console.WriteLine("New Type of Room, Press:\n" +        //type of room
                "  [1]" + lectureHall + "\n" +
                "  [2]" + secureRoom + "\n" +
                "  [3]" + staffRoom + "\n" +
                "  [4]" + teachingRoom + "\n");
                int typeOfRoom = Convert.ToInt32(Console.ReadLine());
                string type = "";
                if (typeOfRoom == 1)
                {
                    type = lectureHall;
                }
                else if (typeOfRoom == 2)
                {
                    type = secureRoom;
                }
                else if (typeOfRoom == 3)
                {
                    type = staffRoom;
                }
                else if (typeOfRoom == 4)
                {
                    type = teachingRoom;
                }
                else
                {
                    Console.WriteLine("Invalid Input");          //not valid input
                }
                string oldId = structureOrganization.Building[indexBuilding].Floors[indexFloor].Rooms[indexRoom].IdRoom; //update fields room
                structureOrganization.Building[indexBuilding].Floors[indexFloor].Rooms[indexRoom].IdRoom = newId;
                structureOrganization.Building[indexBuilding].Floors[indexFloor].Rooms[indexRoom].Type = type;
                Console.WriteLine(oldId + " has been replaced with " + newId + ", new type of room "+ type + "\npress 'ENTER' to go back to the main menu");
                Console.ReadLine();
                string json = JsonConvert.SerializeObject(structureOrganization, Formatting.Indented); //write file building
                File.WriteAllText(FileInteractor.ChooseFile(1), json);
            }
            else if(filePath == "jsonPerson.json")
            {
                Console.Clear();
                FileInteractor.JSONPrinter(filePath);       //print list of persons
                JSONPerson groupPerson = (JSONPerson)FileInteractor.JSONReader(filePath);
                Console.WriteLine("Which ID would you like to update?");
                int idUser = Convert.ToInt32(Console.ReadLine());       //choose id person to replace
                Person person = groupPerson.Person[LoopPersonChecker(idUser,filePath)];
                Console.WriteLine("Type the new values for '" + person.Surname + " " + person.Name + "'");
                Console.WriteLine("Which is te new Name?");
                string newName = Console.ReadLine();        //type new name person
                person.Name = newName;
                Console.WriteLine("Which is te new Surname?");
                string newSurname = Console.ReadLine();     //type new surname person
                person.Surname = newSurname;
                Console.WriteLine("Which is the new Category type?\n" +         //choose new type person
                     "  [1]" + cleaner + "\n" +
                     "  [2]" + emergency + "\n" +
                     "  [3]" + manager + "\n" +
                     "  [4]" + security + "\n" +
                     "  [5]" + staffMember + "\n" +
                     "  [6]" + student + "\n" +
                     "  [7]" + visitorGuest + "\n"); 
                int newCategory = Convert.ToInt32(Console.ReadLine());
                switch (newCategory)
                {
                    case 1:
                        person.Category = cleaner;
                        break;
                    case 2:
                        person.Category = emergency;
                        break;
                    case 3:
                        person.Category = manager;
                        break;
                    case 4:
                        person.Category = security;
                        break;
                    case 6:
                        person.Category = staffMember;
                        break;
                    case 7:
                        person.Category = student;
                        break;
                    case 8:
                        person.Category = visitorGuest;
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
                JSONCard cards = (JSONCard)FileInteractor.JSONReader(FileInteractor.ChooseFile(4));
                Card card = cards.Card[LoopPersonChecker(idUser, filePath)];        //update card details
                card.Name = newName;        
                card.Surname = newSurname;
                card.Category = person.Category;
                Console.WriteLine("The new Name is: '"+ newName + "' the new Surname is: '" + newSurname + "' the new Category is: '" + person.Category + " '");
                Console.ReadLine();
                string json = JsonConvert.SerializeObject(groupPerson, Formatting.Indented);        //write file person
                File.WriteAllText(FileInteractor.ChooseFile(3), json);
                json = JsonConvert.SerializeObject(cards, Formatting.Indented);        //write file cards
                File.WriteAllText(FileInteractor.ChooseFile(4), json);
            }
        }
        public static string RoomDetails()       //print for choose ID Room
        {
            Console.WriteLine("What is the ID of the room?");
            string idRoom = Console.ReadLine();
            return idRoom;
        }
        public static void Remove(string filePath)      //remove room/person/card
        {
            if (filePath == "jsonBuilding.json")
            {
                JSONBuilding jsonBuilding = (JSONBuilding)FileInteractor.JSONReader(filePath); 
                Console.WriteLine("What room do you wish to Remove?");  // input index to remove
                string idRoom = Console.ReadLine();
                Floor floor = jsonBuilding.Building[0].Floors[LoopBuildingChecker(idRoom, filePath).Item2];    //get index floor and index room
                Room room = jsonBuilding.Building[0].Floors[LoopBuildingChecker(idRoom, filePath).Item2].Rooms[LoopBuildingChecker(idRoom, filePath).Item1];
                floor.Rooms.Remove(room);       //remove room
                Console.WriteLine(room.IdRoom + " was removed,\npress 'ENTER' to go back to the main menu");    
                Console.ReadLine();
                string json = JsonConvert.SerializeObject(jsonBuilding, Formatting.Indented);      //write file room
                File.WriteAllText(FileInteractor.ChooseFile(1), json);
            }
            else if (filePath == "jsonPerson.json")
            {
                Console.Clear();
                FileInteractor.JSONPrinter(FileInteractor.ChooseFile(3));       //print all persons
                JSONPerson jsonPerson = (JSONPerson)FileInteractor.JSONReader(filePath);
                Console.WriteLine("Type the ID of the user you wish to Remove");
                int idUser = Convert.ToInt32(Console.ReadLine());       //index person to remove
                int index = LoopPersonChecker(idUser, filePath);
                Person person = jsonPerson.Person[index];   
                jsonPerson.Person.Remove(person);               //remove person
                JSONCard cards = (JSONCard)FileInteractor.JSONReader(FileInteractor.ChooseFile(4));
                Card card = cards.Card[index];
                cards.Card.Remove(card);        //remove card
                Console.WriteLine(person.Surname + " " + person.Name + " was removed,\npress 'ENTER' to go back to the main menu");
                Console.ReadLine();
                string json = JsonConvert.SerializeObject(jsonPerson, Formatting.Indented);        //write file persons
                File.WriteAllText(FileInteractor.ChooseFile(3), json);
                json = JsonConvert.SerializeObject(cards, Formatting.Indented);        //write file cards
                File.WriteAllText(FileInteractor.ChooseFile(4), json);
            }
        }
        public static void Add(string filePath)       //add room/person/card
        {
            if (filePath == "jsonBuilding.json")        //add room
            {
                Console.WriteLine("Type of Room, Press:\n" +        //type of room choice
                 "  [1]" + lectureHall + "\n" +
                 "  [2]" + secureRoom + "\n" +
                 "  [3]" + staffRoom + "\n" +
                 "  [4]" + teachingRoom + "\n");
                int typeOfRoom = Convert.ToInt32(Console.ReadLine());
                JSONBuilding structureOrganisation = (JSONBuilding)FileInteractor.JSONReader(FileInteractor.ChooseFile(1));
                Console.WriteLine("What floor?");
                int idFloor = Convert.ToInt32(Console.ReadLine());      //floor choice
                int maxFloors = structureOrganisation.Building[0].Floors.Count;
                if (idFloor > structureOrganisation.Building[0].Floors.Count)
                {
                    Room room = new Room();         //new room details, default normal value true, emergency false
                    if (typeOfRoom == 1)
                    {
                        room = new Room(RoomDetails(), true, false, lectureHall);
                    }
                    else if (typeOfRoom == 2)
                    {
                        room = new Room(RoomDetails(), true, false, secureRoom);
                    }
                    else if (typeOfRoom == 3)
                    {
                        room = new Room(RoomDetails(), true, false, staffRoom);
                    }
                    else if (typeOfRoom == 4)
                    {
                        room = new Room(RoomDetails(), true, false, teachingRoom);
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input");          //not valid input
                    }
                    structureOrganisation.Building[0].Floors[idFloor].Rooms.Add(room);          //add room
                    string json = JsonConvert.SerializeObject(structureOrganisation, Formatting.Indented);      //save file rooms
                    File.WriteAllText(FileInteractor.ChooseFile(1), json);
                }
                else
                {
                    Console.WriteLine("Invalid input, the max number of floors is : " + maxFloors);
                }
            } else if (filePath == "jsonPerson.json") {             //add person
                
                Console.WriteLine("Type of Person, Press:\n" +      //type of person choice
                     "  [1]" + cleaner + "\n" +
                     "  [2]" + emergency + "\n" +
                     "  [3]" + manager + "\n" +
                     "  [4]" + security + "\n" +
                     "  [5]" + staffMember + "\n" +
                     "  [6]" + student + "\n" +
                     "  [7]" + visitorGuest + "\n");
                int typeOfPerson = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Type the 'Surname', please");
                string surname = Console.ReadLine();                //type new Surname
                Console.WriteLine("Type the 'Name', please");   
                string name = Console.ReadLine();                   //type new Name    
                int id = 1;
                JSONPerson jsonPerson = (JSONPerson)FileInteractor.JSONReader(FileInteractor.ChooseFile(3));
                List<Person> newPerson = new List<Person>();        //new list of persons needed to sort
                for (int i = 0; i < jsonPerson.Person.Count; i++)
                {
                    newPerson.Add(jsonPerson.Person[i]);
                }
                newPerson = newPerson.OrderBy(newPerson => newPerson.Id).ToList();
                for (int i = 0; i < newPerson.Count; i++)           //after sorting IDs, assign the first space free
                {
                    if (id == newPerson[i].Id)
                    {
                        id++;
                    }
                }
                JSONCard cards = (JSONCard)FileInteractor.JSONReader(FileInteractor.ChooseFile(4));
                Card card = new Card();             //new card created
                Person person = new Person();       //new person created
                if (typeOfPerson == 1)
                {
                    person = new Person(id, name, surname, cleaner);
                    card = new Card(id, true, false, name, surname, cleaner);
                }
                else if (typeOfPerson == 2)
                {
                    person = new Person(id, name, surname, emergency);
                    card = new Card(id, true, false, name, surname, emergency);
                }
                else if (typeOfPerson == 3)
                {
                    person = new Person(id, name, surname, manager);
                    card = new Card(id, true, false, name, surname, manager);
                }
                else if (typeOfPerson == 4)
                {
                    person = new Person(id, name, surname, security);
                    card = new Card(id, true, true, name, surname, security);
                }
                else if (typeOfPerson == 5)
                {
                    person = new Person(id, name, surname, staffMember);
                    card = new Card(id, true, false, name, surname, staffMember);
                }
                else if (typeOfPerson == 6)
                {
                    person = new Person(id, name, surname, security);
                    card = new Card(id, true, false, name, surname, security);
                }
                else if (typeOfPerson == 7)
                {
                    person = new Person(id, name, surname, student);
                    card = new Card(id, true, false, name, surname, student);
                }
                else if (typeOfPerson == 8)
                {
                    person = new Person(id, name, surname, visitorGuest);
                    card = new Card(id, true, false, name, surname, visitorGuest);
                }
                else                                 //not valid input
                {
                    Console.WriteLine("Invalid Input");
                }
                jsonPerson.Person.Add(person);         //new person added to list
                cards.Card.Add(card);                   //new card added to list
                CardReleaser(person);                   //assign id to card
                Console.WriteLine("New User " + person.Surname + " " + person.Surname + " added,\n" +
                    "Press 'Enter' to get back to the main menu");
                string json = JsonConvert.SerializeObject(jsonPerson, Formatting.Indented);        //write file persons
                File.WriteAllText(FileInteractor.ChooseFile(3), json);
                json = JsonConvert.SerializeObject(cards, Formatting.Indented);        //write file card
                File.WriteAllText(FileInteractor.ChooseFile(4), json);
            }
        }
        private static void CardReleaser(Person person)     // assign id card
        {
            Card card = new Card(person.Id);
            Console.WriteLine("New Card added, ID: " + card.Id);
        }

        public static void SortPersonBySurname(string filePath)     //sort persons by surname
        {
            JSONPerson jsonPerson = (JSONPerson)FileInteractor.JSONReader(filePath);
            List<Person> newPerson = new List<Person>();
            for (int i = 0; i < jsonPerson.Person.Count; i++)      //create a new list to sort by surname
            {
                newPerson.Add(jsonPerson.Person[i]);
            }
            newPerson = newPerson.OrderBy(newPerson => newPerson.Surname).ToList();     //sort
            for (int i = 0; i < newPerson.Count; i++)
            {
                Console.WriteLine("  ID: " + newPerson[i].Id + "\n" +       //print list
                     "      Surname: " + newPerson[i].Surname + "\n" +
                     "      Name: " + newPerson[i].Name + "\n" +
                     "          Category: " + newPerson[i].Category + "\n");
            }
        }
        public static (Object, string) CardReaderAssign(string roomId)      //will print card reader ID
        {
            CardReader cardReader = new CardReader();
            JSONBuilding structureOrganization = (JSONBuilding)FileInteractor.JSONReader(FileInteractor.ChooseFile(1));
            string id = "";
            for (int i = 0; i < structureOrganization.Building[0].Floors.Count; i++)
            {
                for (int j = 0; j < structureOrganization.Building[0].Floors[i].Rooms.Count; j++)
                {
                    id = structureOrganization.Building[0].Floors[i].Rooms[j].IdRoom;
                    cardReader = new CardReader(id);
                    if(cardReader.Id == roomId)
                    {
                        cardReader.Id = "CardReader ID: " + roomId; //print card reader ID
                        Console.WriteLine(cardReader.Id);
                        return (cardReader, roomId);
                    }
                }
            }
             return (null,null);        //if not found return null
        }
    }
}
