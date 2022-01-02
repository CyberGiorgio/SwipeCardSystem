using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SwipeCardSystem
{
    public class CardReader             //this class implements the cardReaderS
    {
        private string id;
        private TimeSpan[] tss = new TimeSpan []{
            new TimeSpan(05, 30, 0),         //staffMember          allowed time for Lecture Hall, StaffRoom, Teaching Room, no Emergency
            new TimeSpan(23, 59, 59), 
            new TimeSpan(08, 30, 0),        //student             allowed time for Lecture Hall, Teaching Room, no Emergency
            new TimeSpan(22, 0, 0),
            new TimeSpan(08, 30, 0),        //VisitorGuest        allowed time for Lecture Hall, no Emergency
            new TimeSpan(22, 0, 0),
            new TimeSpan(05, 30, 0),         //Cleaner             allowed time for any room except Secure Room ,no Emergency
            new TimeSpan(10, 30, 0),
            new TimeSpan(17, 30, 0),        //Cleaner
            new TimeSpan(22, 30, 0),
            new TimeSpan(0, 0, 0)            //Manager            Any room, any time, no Emergency
                                             //Security           Any room, any time
                                             //Emergency          Emergency state only
    };
        const string cleaner = "Cleaner";           //roles and rooms used within the code for easy update
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
        public CardReader(string id)        //constructors
        {
            this.id = id;
        }
        public CardReader()
        {
        }
        public string Id { get => id; set => id = value; }
        public void PersonRoomCheck(Room room, Card card, int floorNo)         //check if the person is allowed to enter in the room
        {
            string access;
            if (card.Category == staffMember &&
               ((room.Type == lectureHall && room.Emergency == false) || (room.Type == staffRoom && room.Emergency == false) || 
               (room.Type == teachingRoom && room.Emergency == false)))
            {
                TimeCheck(tss[0],tss[1], card, room, floorNo);      //timeChecker
            }
            else if (card.Category == student &&
            ((room.Type == lectureHall && room.Emergency == false) || (room.Type == teachingRoom && room.Emergency == false)))
            {
                TimeCheck(tss[2], tss[3], card, room, floorNo);         //timeChecker
            }
            else if (card.Category == visitorGuest &&
             room.Type == lectureHall && room.Emergency == false)
            {
                TimeCheck(tss[4], tss[5], card, room, floorNo);         //timeChecker
            }
            else if (card.Category == cleaner &&
            ((room.Type == lectureHall && room.Emergency == false) || (room.Type == staffRoom && room.Emergency == false) ||
               (room.Type == teachingRoom && room.Emergency == false)))
            {
                TimeCheck(tss[6], tss[7], card, room, floorNo);         //timeChecker
                TimeCheck(tss[8], tss[9], card, room, floorNo);        //timeChecker
            }
            else if (card.Category == manager && room.Emergency == false)
            {
                TimeCheck(tss[10], tss[10], card, room, floorNo);       //timeChecker
            }
            else if(card.Category == security)
            {
                TimeCheck(tss[10], tss[10], card, room, floorNo);       //timeChecker
            }
            else if (card.Category == emergency && room.Emergency == true)
            {
                TimeCheck(tss[10], tss[10], card, room, floorNo);       //timeChecker
            }
            else                                                  
            {                                               //not allowed
                access = "'NOT ALLOWED'";
                Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                Console.WriteLine("The user is " + access + " to enter in this room");
                FileInteractor.WriteLogEvent(FileInteractor.ChooseFile(2), card, room, access, floorNo);        //save log file
            }
        }
        public void TimeCheck(TimeSpan ts, TimeSpan ts2, Card card, Room room, int floorNo)        //check allowed time
        {
            var dateTime = DateTime.Now.TimeOfDay;
            string access;
            if ((dateTime >= ts && dateTime <= ts2) || ts == ts2) //if current time between allowed timespans OR timespans are equals(allowed always), enter
            {
                access = "'ALLOWED'";
                Console.WriteLine(DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss"));
                Console.WriteLine("The user is " + access + " to enter in this room ");
                FileInteractor.WriteLogEvent(FileInteractor.ChooseFile(2), card, room, access , floorNo);       //save file log
            }
            else
            {
                return;
            }
        }
    }
}
