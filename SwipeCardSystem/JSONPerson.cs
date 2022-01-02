using System;
using System.Collections.Generic;
using System.Text;

namespace SwipeCardSystem
{
    public class JSONPerson            //JSON class for person class
    {
        private string id;      //attributes
        private List<Person> person;
        public JSONPerson()     //constructor
        {
        }
        public JSONPerson(string id, List<Person> person)
        {
            this.id = id;
            this.person = person;
        }
        public List<Person> Person { get => person; set => person = value; }        //encapsulation
        public string Id { get => id; set => id = value; }
    }
    public class Person     //person class
    {
        private int id;                  //attributes
        private string name;
        private string surname;
        private string category;
        public Person(int id, string name, string surname, string category)      //constructor
        {
            this.id = id;
            this.name = name;
            this.surname = surname;
            this.category = category;
        }
        public Person()      //constructor
        {
        }
        public int Id { get => id; set => id = value; } //encapsulation
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Category { get => category; set => category = value; }
        public void SwipeCard(Person person)        //swipe card function
        {
            Console.WriteLine(person.Surname + " " + person.Name + " is swiping its card");
        }
    }
}
