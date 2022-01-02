using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace SwipeCardSystem
{
    public class JSONCard       //JSON class card
    {
        private List<Card> card;        //attributes

        public JSONCard(List<Card> card)    //constructor
        {
            this.card = card;
        }
        public JSONCard()
        {
        }
        public List<Card> Card { get => card; set => card = value; }        //encapsulation
    }
    public class Card         // card class
    {
        private int idCards;                 //attributes
        private bool normals;
        private bool emergencies;
        private string name;
        private string surname;
        private string category;
        public Card(int idCards, bool normals, bool emergencies, string name, string surname, string category)
        {
            this.idCards = idCards;         //constructor
            this.normals = normals;
            this.emergencies = emergencies;
            this.name = name;
            this.surname = surname;
            this.category = category;
        }
        public Card(int idCards)
        {
            this.idCards = idCards;
        }
        public Card() //constructor
        {
        }
        public int Id { get => idCards; set => idCards = value; }       //encapsulation
        public bool Normals { get => normals; set => normals = value; }
        public bool Emergencies { get => emergencies; set => emergencies = value; }
        public string Name { get => name; set => name = value; }
        public string Surname { get => surname; set => surname = value; }
        public string Category { get => category; set => category = value; }

        public void cardSwitchStatus(Card card) //card switch for future implementation
        {
            if(card.Emergencies == true)
            {
                card.Normals = false;
            } else if (card.Normals == true)
            {
                card.Emergencies = false;
            }
        }
    }
}
