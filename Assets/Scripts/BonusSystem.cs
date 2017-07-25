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
                        string plusOrMinus = "";
                        int bonusModified = 0;
                        int chosenBonus = 0;
                        Bonus bonus = new Bonus();
                        StringBuilder newDescription, tempReplace;

                        string[] separator = { "HP", "MP", "%", ",", " ", "Damage", "Defense", "Physical", "damage" };
                        string[] separator2 = { "," };
                        string[] separator3 = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0", "+", "-", ", " };

                        XmlDocument xmlDatabase = new XmlDocument();
                        xmlDatabase.Load(xmlPath);

                        XmlNodeList xmlNodeList = xmlDatabase.SelectNodes(string.Format(".//{0}{1}Bonus/Bonus", bonusType, bonusPrefixOrPostfix));

                        chosenBonus = UnityEngine.Random.Range(0, xmlNodeList.Count);

                        XmlNode node = xmlNodeList[chosenBonus];

                        bonus.EntityName = node.Attributes[0].Value;
                        bonus.EntityDescription = node["BonusDescription"].InnerText;
                        newDescription = new StringBuilder(bonus.EntityDescription, 50);

                        string[] numbersToParse = bonus.EntityDescription.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                        string[] initialValues = bonus.EntityDescription.Split(separator2, StringSplitOptions.RemoveEmptyEntries);
                        string[] stat = bonus.EntityDescription.Split(separator3, StringSplitOptions.RemoveEmptyEntries);

                        for (int i = 0; i < numbersToParse.Length; i++)
                        {
                            bonus.BonusValue.Add(int.Parse(numbersToParse[i]));
                            bonusModified = bonus.BonusValue[i] * UnityEngine.Random.Range(0, bonusLevel) + 1;
                            bonus.BonusValue[i] = bonusModified;

                            if (bonusModified >= 0)
                            {
                                plusOrMinus = "+";
                            }
                            else
                            {
                                plusOrMinus = "";
                            }

                            tempReplace = new StringBuilder(" " + plusOrMinus + bonusModified.ToString() + stat[i], 50);
                            newDescription.Replace(initialValues[i], tempReplace.ToString());

                            bonus.EntityDescription = newDescription.ToString();
                        }

                        int createBonus = 1;

                        if (createBonus == 1)
                        {
                            return bonus;
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
            ArmorBonus ar = new ArmorBonus(UnityEngine.Random.Range(1, 1000));
            WeaponBonus wp = new WeaponBonus(UnityEngine.Random.Range(1, 1000));
            AccesoryBonus acc = new AccesoryBonus(UnityEngine.Random.Range(1, 1000));

            Debug.Log(ar.GetInfo());
            Debug.Log(wp.GetInfo());
            Debug.Log(acc.GetInfo());
        }
    }
}
