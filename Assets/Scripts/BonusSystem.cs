using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using UnityEngine;
using DataManagement;

namespace BonusSystem
{
    public class Bonus : Entity
    {
        private int bonusLevel;
        private List<int> bonusValue = new List<int>();

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
                if (value != null)
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

        protected Bonus LoadBonus(string xmlPath, string bonusPrefixOrPostfix, string bonusType, int bonusLevel)
        {
            if (bonusPrefixOrPostfix == "Prefix" | bonusPrefixOrPostfix == "Postfix")
            {
                if (bonusType == "Weapon" | bonusType == "Armor" | bonusType == "Accesory")
                {
                    try
                    {

                        int bonusModified = 0;
                        int chosenBonus = 0;
                        List<Bonus> bonuses = new List<Bonus>();
                        string[] separator = { "HP", "MP", "%", ",", " ", "Damage", "Defense", "Physical", "damage" };

                        XmlDocument xmlDatabase = new XmlDocument();
                        xmlDatabase.Load(xmlPath);

                        XmlNodeList xmlNodeList = xmlDatabase.SelectNodes(string.Format(".//{0}{1}Bonus/Bonus", bonusType, bonusPrefixOrPostfix));

                        foreach (XmlNode Node in xmlNodeList)
                        {
                            int index = bonuses.Count;

                            bonuses.Add(new Bonus());
                            bonuses[index].EntityName = Node.Attributes[0].Value;
                            bonuses[index].EntityDescription = Node["BonusDescription"].InnerText;

                            string[] tempStr = bonuses[index].EntityDescription.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                            for (int i = 0; i < tempStr.Length; i++)
                            {
                                bonuses[index].BonusValue.Add(int.Parse(tempStr[i]));
                                bonusModified = bonuses[index].BonusValue[i] * UnityEngine.Random.Range(0, bonusLevel) + 1;
                                bonuses[index].BonusValue[i] = bonusModified;
                                
                                bonuses[index].EntityDescription = bonuses[index].EntityDescription.Replace(tempStr[i], bonusModified.ToString());
                            }
                        }

                        chosenBonus = UnityEngine.Random.Range(-1, bonuses.Count);

                        if (chosenBonus > 0)
                        {
                            return bonuses[chosenBonus];
                        }
                        else
                        {
                            return null;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.Log("Error: " + ex);
                    }
                }
                else
                {
                    Debug.Log("No such bonus type: " + bonusType);
                    return null;
                }
            }
            else
            {
                Debug.Log("No such bonus type: " + bonusType);
                return null;
            }

            return null;
        }

        protected void GenerateBonus(string xmlPath, string bonusPrefixOrPostfix, string bonusType, int bonusLevel)
        {
            Bonus bonus = LoadBonus(xmlPath, bonusPrefixOrPostfix, bonusType, bonusLevel);

            if (bonus != null)
            {
                this.EntityName = bonus.EntityName;
                this.EntityDescription = bonus.EntityDescription;
                this.BonusLevel = bonusLevel;
                this.BonusValue = bonus.BonusValue;
            }
        }
    }

    public class ArmorBonus : Bonus
    {
        public ArmorBonus(int bonusLevel)
        {
            GenerateBonus("Assets//XmlFiles//ItemBonuses.xml", "Prefix", "Armor", bonusLevel);
        }
    }

    public class WeaponBonus : Bonus
    {
        public WeaponBonus(int bonusLevel)
        {
            GenerateBonus("Assets//XmlFiles//ItemBonuses.xml", "Prefix", "Weapon", bonusLevel);
        }
    }

    public class AccesoryBonus : Bonus
    {
        public AccesoryBonus(int bonusLevel)
        {
            GenerateBonus("Assets//XmlFiles//ItemBonuses.xml", "Prefix", "Accesory", bonusLevel);
        }
    }

    public class BonusSystem : MonoBehaviour
    {
        private void Start()
        {
            ArmorBonus ar = new ArmorBonus(26);
            WeaponBonus wp = new WeaponBonus(71);
            AccesoryBonus acc = new AccesoryBonus(612);

            Debug.Log(ar.GetInfo());
            Debug.Log(wp.GetInfo());
            Debug.Log(acc.GetInfo());
        }
    }
}
