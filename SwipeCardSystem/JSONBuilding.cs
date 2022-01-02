using System;
using System.Collections.Generic;
using System.Text;

namespace SwipeCardSystem
{                                   //composition class
	public class JSONBuilding           //JSON class for building
	{
        private List<Building> building;            //attributes
        public JSONBuilding(List<Building> building)        //constructor
        {
            this.building = building;
        }
        public JSONBuilding()       //constructor
        {
        }
        public List<Building> Building { get => building; set => building = value; }    //encapsulation
    }
	public class Building       //class building
	{
        private string id;              //attributes
        private List<Floor> floor;
        public Building(string id, List<Floor> floor)       //constructors
        {
            this.id = id;
            this.floor = floor;
        }
        public Building()
        {
        }
        public string Id { get => id; set => id = value; }      //encapsulation
        public List<Floor> Floors { get => floor; set => floor = value; }
    }
	public class Floor          //class floor
	{
        private int floorNo;        //attributes
        private List<Room> room;
        public Floor(int floorNo, List<Room> room)      //constructor
        {
            this.floorNo = floorNo;
            this.room = room;
        }
        public Floor()
        {
        }
        public int FloorNo { get => floorNo; set => floorNo = value; }      //encapsulation
        public List<Room> Rooms { get => room; set => room = value; }
    }
	public class Room       //room class
	{
        private string idRoom;      //attributes
        private bool normal;
        private bool emergency;
        private string type;

        public Room(string idRoom, bool normal, bool emergency, string type)    //constructor
        {
            this.idRoom = idRoom;
            this.normal = normal;
            this.emergency = emergency;
            this.type = type;
        }
        public Room(string idRoom)
        {
            this.idRoom = idRoom;
        }
        public Room()
        {
        }
        public string IdRoom { get => idRoom; set => idRoom = value; }      //encapsulation
        public bool Normal { get => normal; set => normal = value; }
        public bool Emergency { get => emergency; set => emergency = value; }
        public string Type { get => type; set => type = value; }
        public string RoomState(Room room)      //room state check
        {
            string state = "";
            if (room.Emergency == true)
            {
                state = "Emergency";
            }
            else
            {
                state = "Normal";
            }
            return state;
        }
    }
}
