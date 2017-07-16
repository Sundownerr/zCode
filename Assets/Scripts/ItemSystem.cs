using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;
using DataManagement;

namespace ItemSystem
{
    public class Bonus : Entity
    {
        private int bonusLevel;
        
        public List<int> bonusValue = new List<int>();                       
        
        public int BonusLevel
        {
            get
            {
                return bonusLevel;
            }
            set
            {
                if (value > 0)
                {
                    bonusLevel = value;
                }
                else
                {
                    bonusLevel = 0;
                }
            }
        }
        
        public List<int> BonusValue
        {
            get { return bonusValue; }
            set
            {
                if(value != null)
                {
                    bonusValue = value;
                }
            }
        }

        
        public virtual string GetInfo()
        {
            StringBuilder tempString = new StringBuilder();

            for (int i = 0; i < BonusValue.Count; i++)
            {
                tempString.Append(string.Format("\r\nValue {0} = {1}\r\n", i, BonusValue[i]));
            }

            return string.Format("Bonus name: {0}, Description: {1}, Level = {2}, Values: {3} \r\n", this.EntityName, this.EntityDescription, bonusLevel, tempString.ToString());
        }

        protected void LoadBonuses(string xmlPath, string bonusType, int bonusLevel, List<Bonus> bonusPrefix, List<Bonus> bonusPostfix)
        {
            if (bonusType == "Weapon" | bonusType == "Armor" | bonusType == "Accesory")
            {
                string[] separator = { "HP", "MP", ",", " ", "+", "%", "Damage", "Defense", "Physical", "damage" };
                int bonusModified = 0;

                try
                {
                    XmlDocument xmlDatabase = new XmlDocument();
                    xmlDatabase.Load(xmlPath);

                    XmlNodeList xmlNodeList = xmlDatabase.SelectNodes(string.Format(".//{0}PrefixBonus/Bonus", bonusType));
                    System.Random rand = new System.Random();

                    foreach (XmlNode Node in xmlNodeList)
                    {
                        int index = bonusPrefix.Count;                       

                        bonusPrefix.Add(new Bonus());
                        bonusPrefix[index].EntityName = Node.Attributes[0].Value;
                        bonusPrefix[index].EntityDescription = Node["this.EntityDescription"].InnerText;

                        string[] tempStr = bonusPrefix[index].EntityDescription.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < tempStr.Length; i++)
                        {
                            bonusPrefix[index].BonusValue.Add(int.Parse(tempStr[i]));
                            bonusModified = bonusPrefix[index].BonusValue[i] * rand.Next(0, bonusLevel);
                            bonusPrefix[index].BonusValue[i] = bonusModified;
                            bonusPrefix[index].EntityDescription = bonusPrefix[index].EntityDescription.Replace(tempStr[i], bonusModified.ToString());

                            Console.WriteLine(bonusModified);
                        }

                        Console.WriteLine(bonusPrefix[index].EntityName + " " + bonusPrefix[index].EntityDescription + ", Values: " + bonusPrefix[index].BonusValue.Count);
                    }

                    xmlNodeList = xmlDatabase.SelectNodes(string.Format(".//{0}PostfixBonus/Bonus", bonusType));

                    foreach (XmlNode Node in xmlNodeList)
                    {
                        int index = bonusPostfix.Count;
                        
                        bonusPostfix.Add(new Bonus());
                        bonusPostfix[index].EntityName = Node.Attributes[0].Value;
                        bonusPostfix[index].EntityDescription = Node["this.EntityDescription"].InnerText;

                        string[] tempStr = bonusPostfix[index].EntityDescription.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < tempStr.Length; i++)
                        {
                            bonusPostfix[index].BonusValue.Add(int.Parse(tempStr[i]));
                            bonusModified = bonusPostfix[index].BonusValue[i] * rand.Next(0, bonusLevel);
                            bonusPostfix[index].BonusValue[i] = bonusModified;
                            bonusPostfix[index].EntityDescription = bonusPostfix[index].EntityDescription.Replace(tempStr[i], bonusModified.ToString());

                            Console.WriteLine(bonusModified);
                        }

                        Console.WriteLine(bonusPostfix[index].EntityName + " " + bonusPostfix[index].EntityDescription + ", Values: " + bonusPostfix[index].BonusValue.Count);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error" + ex);
                    // throw new Exception(ex.Message);
                }
            }
            else
            {
                throw new Exception("No such bonus type: " + bonusType);
            }
        }
    }

    public class ArmorBonus : Bonus
    {
        public List<Bonus> bonusArmorPrefix = new List<Bonus>();
        private List<Bonus> bonusArmorPostfix = new List<Bonus>();

        public ArmorBonus() 
        {
            LoadBonuses("ItemBonuses.xml", "Armor", 500, bonusArmorPrefix, bonusArmorPostfix);           
        }
    }

    public class WeaponBonus : Bonus
    {
        private List<Bonus> bonusWeaponPrefix = new List<Bonus>();
        private List<Bonus> bonusWeaponPostfix = new List<Bonus>();

        public WeaponBonus()
        {
            LoadBonuses("ItemBonuses.xml", "Weapon",500, bonusWeaponPrefix, bonusWeaponPostfix);
        }
    }

    public class AccesoryBonus : Bonus
    {
        private List<Bonus> bonusAccesoryPrefix = new List<Bonus>();
        private List<Bonus> bonusAccesoryPostfix = new List<Bonus>();

        public AccesoryBonus()
        {
            LoadBonuses("ItemBonuses.xml", "Accesory", 500, bonusAccesoryPrefix, bonusAccesoryPostfix);
        }
    }

    public class Item
    {
        private string itemName, itemDescription;
        private int itemWeight, itemLevel, itemPrice, itemMaxDurability, itemCurrentDurability;
        private bool isBroken;
        protected bool isItemExist;
        protected XmlDocument xmlDatabase;

        public int ItemWeight
        {
            get
            {
                return itemWeight;
            }

            set
            {
                if (value <= 0)
                {
                    itemWeight = 0;
                }
                else
                {
                    itemWeight = value;
                }
            }
        }

        public int ItemLevel
        {
            get
            {
                return itemLevel;
            }

            set
            {
                if (value <= 0)
                {
                    itemLevel = 0;
                }
                else
                {
                    itemLevel = value;
                }
            }
        }

        public int ItemPrice
        {
            get
            {
                return itemPrice = ItemLevel * ItemCurrentDurability - ItemWeight;
            }           
        }

        public int ItemMaxDurability
        {
            get
            {
                return itemMaxDurability = ItemLevel + ItemWeight;
            }

            set
            {
                if (value <= 0)
                {
                    itemMaxDurability = 0;
                }
                else
                {
                    itemMaxDurability = value;
                }
            }
        }

        public int ItemCurrentDurability
        {
            get
            {
                return itemCurrentDurability;
            }

            set
            {
                if (value <= 0)
                {
                    isBroken = true;
                    itemCurrentDurability = 0;
                }
                else
                {
                    isBroken = false;
                    itemCurrentDurability = value;
                }
            }
        }

        public string ItemName
        {
            get { return itemName; }
            protected set
            {
                if (value == "")
                {
                    itemName = " - ";
                }
                else
                {
                    itemName = value;
                }
            }
        }

        public string ItemDescription
        {
            get { return itemDescription; }
            protected set
            {
                if (value == "")
                {
                    itemDescription = " - ";
                }
                else
                {
                    itemDescription = value;
                }
            }
        }

        protected virtual string GetItemInfo()
        {
            return string.Format("Item name: {0}, Item description: {1} \r\n", itemName, itemDescription);
        }

    }

    public class Armor : Item
    {           
        public Armor()
        {

        }
    }

    public class Weapon : Item
    {
        public Weapon()
        {

        }
    }

    public class Accessory : Item
    {
        public Accessory()
        {

        }
    }

    class ItemSystem : MonoBehaviour
    {
        static void Main(string[] args)
        {
            ArmorBonus ar = new ArmorBonus();
            WeaponBonus wp = new WeaponBonus();
            AccesoryBonus acc = new AccesoryBonus();                   

            Console.Read();
        }
    }
}
