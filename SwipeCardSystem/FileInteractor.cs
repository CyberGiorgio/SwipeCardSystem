using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SwipeCardSystem
{
    public static class FileInteractor         //This class will interact with files
    {
        public static string ChooseFile(int IDFile)
        {                                    //choose path for reading file
            string filePath = "";
            string fileRooms = "jsonBuilding.json";
            string fileAccess = "access_log";
            string filePerson = "jsonPerson.json";
            string fileCard = "jsonCard.json";
            if (IDFile == 1)        //for building
            {
                filePath = fileRooms;
            }
            if (IDFile == 2)
            {
                filePath = fileAccess;  //for access log
            }
            else if (IDFile == 3)
            {
                filePath = filePerson;      //for persons
            } else if (IDFile == 4)
            {
                filePath = fileCard;        //fot cards
            }
            return filePath;
        }
        public static void JSONPrinter(string filePath)     //print the full structure
        {
            string fn = DateTime.Now.ToString("ddMMyyyy");
            if (filePath == "jsonBuilding.json")        //building structure
            {
                JSONBuilding jsonBuilding = (JSONBuilding)JSONReader(filePath);
                Floor floorCycle;
                Building buildingCycle;
                for (int k = 0; k < jsonBuilding.Building.Count; k++)
                {
                    buildingCycle = jsonBuilding.Building[k];
                    Console.WriteLine("Building: " + buildingCycle.Id);
                    for (int i = 0; i < jsonBuilding.Building[0].Floors.Count; i++)
                    {
                        floorCycle = jsonBuilding.Building[0].Floors[i];
                        Console.WriteLine("     Floor: " + floorCycle.FloorNo);
                        for (int j = 0; j < floorCycle.Rooms.Count; j++)
                        {
                            Console.WriteLine("        Room: " + floorCycle.Rooms[j].IdRoom + 
                                                "\n           Type: " + floorCycle.Rooms[j].Type);
                            if (floorCycle.Rooms[j].Normal == true)
                            {
                                Console.Write("             State Normal: ");
                                Console.WriteLine(floorCycle.Rooms[j].Normal);
                            }
                            else
                            {
                                Console.Write("         State Emergency: ");
                                Console.WriteLine(floorCycle.Rooms[j].Emergency);
                            }
                        }
                        Console.WriteLine("");
                    }
                }
            } else if (filePath == "jsonPerson.json")   //persons sorted by Surname structure
            {
                Lists.SortPersonBySurname(filePath);
                
            } else if (filePath == "jsonCard.json")
            {
                JSONCard cards = (JSONCard)FileInteractor.JSONReader(FileInteractor.ChooseFile(4));
                for (int i = 0; i < cards.Card.Count; i++)
                {
                    Console.WriteLine("ID : " + cards.Card[i].Id);
                    Console.WriteLine("     Surname : " + cards.Card[i].Surname);
                    Console.WriteLine("     Name : " + cards.Card[i].Name);
                    Console.WriteLine("         Category : " + cards.Card[i].Category);
                }
            }  else if (filePath == "access_log")       //read day log
            {
                filePath = filePath + fn + ".txt";
                try
                {
                    List<string> lines = new List<string>();
                    lines = File.ReadAllLines(filePath).ToList();
                    foreach (string line in lines)
                    {
                       
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("Daily log empty,\n press enter");
                }
            }
        }
        public static Object JSONReader(string filePath)      // JSON file reader
        {
            StreamReader stream = new StreamReader(filePath);
            string sr = stream.ReadToEnd().ToString();
            stream.Close();
            Object objG = new Object();
            JSONBuilding obj;
            JSONPerson obj2;
            JSONCard obj3;
            if (filePath == "jsonBuilding.json")    //for building
            {
                obj = System.Text.Json.JsonSerializer.Deserialize<JSONBuilding>(sr);
                objG = obj;
            } else if (filePath == "jsonPerson.json"){       //for persons
                obj2 = System.Text.Json.JsonSerializer.Deserialize<JSONPerson>(sr);
                objG = obj2;
            }
            else  if(filePath == "jsonCard.json")                                       //for access log
            {
                obj3 = System.Text.Json.JsonSerializer.Deserialize<JSONCard>(sr);
                objG = obj3;
            }
            return objG;
        }
        public static void WriteLogEvent(string filePath, Card card, Room room, string access, int floorNo)        //txt log writer
        {
            try
            {
                string fn = DateTime.Now.ToString("ddMMyyyy");
                filePath = filePath + fn + ".txt"; 
                if (!File.Exists(filePath))     //if file does not exists create a new one
                {
                    // Create a file to write to.
                        using (StreamWriter sw = File.CreateText(filePath))
                        {
                                sw.WriteLine(DateTime.Now.ToString("HH:mm:ss dd MMMM yyyy") + " ID: " + card.Id + ", Surname: " + card.Surname + "" +
                                   ", Name : " + card.Name + ", Floor: " + floorNo + ", Room: " + room.IdRoom + ", Type: " + room.Type + "" +
                                   ", State: " + room.RoomState(room) + ", was " + access);
                        }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(filePath))     //else append new text line
                    {
                        sw.WriteLine(DateTime.Now.ToString("HH:mm:ss dd MMMM yyyy") + " ID: " + card.Id + ", Surname: " + card.Surname + "" +
                                ", Name : " + card.Name + ", Floor: " + floorNo + ", Room: " + room.IdRoom + ", Type: " + room.Type + "" +
                                ", State: " + room.RoomState(room) + ", was " + access);
                    }
                }
            } catch(IOException)         //not valid input
            {
                Console.WriteLine("File cannot be written");
            }
        }
        
    }
}
