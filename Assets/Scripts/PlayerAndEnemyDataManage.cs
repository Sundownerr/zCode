using System.Collections.Generic;
using System;
using System.Text;
using System.Xml;
using System.IO;
using UnityEngine;

namespace DataManagement
{
    abstract public class Entity
    {
        private string entityName;
        private string entityDescription;
        
        protected bool IsItemExist = false;

        public string EntityName
        {
            get { return entityName; }
            protected set
            {
                if (value == "")
                {
                    entityName = " - ";
                }
                else
                {
                    entityName = value;
                }
            }
        }

        public string EntityDescription
        {
            get { return entityDescription; }
            protected set
            {
                if (value == "")
                {
                    entityDescription = " - ";
                }
                else
                {
                    entityDescription = value;
                }
            }
        }
    }

    public class Material : Entity
    {
        private int maxHP, maxMP, maxArmor, currentHP, currentMP, currentArmor;
        private bool isAlive;

        public int MaxHP
        {
            get { return maxHP; }
            set
            {
                if (value > 0)
                {
                    maxHP = value;
                }
                else
                {
                    maxHP = 0;
                }
            }
        }

        public int MaxMP
        {
            get { return maxMP; }
            set
            {
                if (value > 0)
                {
                    maxMP = value;
                }
                else
                {
                    maxMP = 0;
                }
            }
        }

        public int MaxArmor
        {
            get { return maxArmor; }
            set
            {
                if (value > 0)
                {
                    maxArmor = value;
                }
                else
                {
                    maxArmor = 0;
                }
            }
        }

        public int CurrentHP
        {
            get { return currentHP; }
            set
            {
                if (value > 0)
                {
                    currentHP = value;
                    isAlive = true;
                }
                else
                {
                    currentHP = 0;
                    isAlive = false;
                    this.IsDead();
                }
            }
        }

        public int CurrentMP
        {
            get { return currentMP; }
            set
            {
                if (value > 0)
                {
                    currentMP = value;
                }
                else
                {
                    currentMP = 0;
                }
            }
        }

        public int CurrentArmor
        {
            get { return currentArmor; }
            set
            {
                if (value > 0)
                {
                    currentArmor = value;
                }
                else
                {
                    currentArmor = 0;
                }
            }
        }

        public virtual void IsDead() { }

        public virtual string GetInfo()
        {
            return string.Format(" EntityName: {0}\r\n EntityDescription: {1}\r\n MaxHP: {2}\r\n MaxMP: {3}\r\n MaxArmor: {4}", EntityName, EntityDescription, MaxHP, MaxMP, MaxArmor);
        }
    }

    public class Virtual : Entity
    {
        private int value, duration;

        public int Value
        {
            get { return value; }
            set
            {
                if (value > 0)
                {
                    this.value = value;
                }
                else
                {
                    this.value = 0;
                }
            }
        }

        public int Duration
        {
            get { return duration; }
            set
            {
                if (value > 0)
                {
                    duration = value;
                }
                else
                {
                    duration = 0;
                }
            }
        }

        public virtual string GetInfo()
        {
            return string.Format(" EntityName: {0}\r\n EntityDescription: {1}\r\n Value: {1}\r\n Duration: {2}", EntityName, EntityDescription, Value, Duration);
        }

        protected virtual void LoadXmlDatabase(string xmlPath, string name)
        {
            try
            {
                XmlDocument xmlDatabase = new XmlDocument();
                xmlDatabase.Load(xmlPath);

                foreach (XmlNode Node in xmlDatabase.DocumentElement)
                {
                    if (Node.Attributes[0].Value == name)
                    {
                        this.IsItemExist = true;

                        this.EntityName = Node.Attributes[0].Value;
                        this.Value = int.Parse(Node["Value"].InnerText);
                        this.Duration = int.Parse(Node["Duration"].InnerText);
                        this.EntityDescription = Node["EntityDescription"].InnerText;
                    }
                }
                if (!this.IsItemExist)
                {
                    throw new Exception("Item not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class Potion : Virtual
    {
        public Potion(string name)
        {
            this.LoadXmlDatabase("Assets\\XmlFiles\\Potions.xml", name) ;
        }
    }

    public class BuffDebuff : Virtual
    {
        public BuffDebuff(string name)
        {
            this.LoadXmlDatabase("Assets\\XmlFiles\\BuffDebuff.xml", name);
        }
    }

    public class SkillSet : Virtual
    {
        private PassiveSkill passiveSkill;
        private List<ActiveSkill> activeSkill;

        public PassiveSkill PassiveSkill
        {
            get { return this.passiveSkill; }
            protected set { this.passiveSkill = value; }
        }

        public List<ActiveSkill> ActiveSkill
        {
            get { return this.activeSkill; }
            protected set { this.activeSkill = value; }
        }

        protected override sealed void LoadXmlDatabase(string xmlPath, string name)
        {           
            try
            {
                activeSkill = new List<ActiveSkill>();

                XmlDocument xmlDatabase = new XmlDocument();
                xmlDatabase.Load(xmlPath);

                foreach (XmlNode Node in xmlDatabase.DocumentElement)
                {
                    if (Node.Attributes[0].Value == name)
                    {
                        this.IsItemExist = true;

                        if (Node["ActiveSkills"].ChildNodes.Count > 0)
                        {
                            for (int i = 0; i < Node["ActiveSkills"].ChildNodes.Count; i++)
                            {
                                if (Node["ActiveSkills"].ChildNodes[i].InnerText != "")
                                {
                                    this.ActiveSkill.Add(new ActiveSkill(Node["ActiveSkills"].ChildNodes[i].InnerText));
                                }
                            }
                        }

                        this.PassiveSkill = new PassiveSkill(Node["PassiveSkill"].InnerText);
                    }
                }

                if (!this.IsItemExist)
                {
                    throw new Exception("Item not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public SkillSet(string name, string type)
        {
            this.EntityName = name;

            if (type == "Enemy")
            {
                this.LoadXmlDatabase("Assets\\XmlFiles\\EnemiesStats.xml", name);
            }
            if (type == "Player")
            {
                this.LoadXmlDatabase("Assets\\XmlFiles\\CharacterStats.xml", name);
            }

        }

        public SkillSet() { }
    }

    public class ActiveSkill : Virtual
    {
        private int cost;
       
        protected List<BuffDebuff> BuffDebuff;

        public int Cost
        {
            get { return this.cost; }
            set
            {
                if (value >= 0)
                {
                    cost = value;
                }
                else
                {
                    cost = 0;
                }
            }
        }

        public override string GetInfo()
        {
            StringBuilder info = new StringBuilder(string.Format("{0}\r\n Cost: {1}", base.GetInfo(), Cost));

            if (BuffDebuff.Count > 0)
            {
                info.Append("\r\n\r\n   Buffs and Debuffs: ");

                for (int i = 0; i < BuffDebuff.Count; i++)
                {
                    info.Append(string.Format("\r\n {0}", BuffDebuff[i].GetInfo()));

                    if (i == BuffDebuff.Count - 1)
                    {
                        info.Append("\r\n");
                    }
                }
            }

            return info.ToString();
        }

        protected override sealed void LoadXmlDatabase(string xmlPath, string name)
        {
            BuffDebuff = new List<BuffDebuff>();

            try
            {
                XmlDocument xmlDatabase = new XmlDocument();
                xmlDatabase.Load(xmlPath);

                foreach (XmlNode Node in xmlDatabase.DocumentElement)
                {
                    if (Node.Attributes[0].Value == name)
                    {
                        this.IsItemExist = true;

                        this.EntityName = Node.Attributes[0].Value;
                        this.Cost = int.Parse(Node["Cost"].InnerText);
                        this.Value = int.Parse(Node["Value"].InnerText);
                        this.Duration = int.Parse(Node["Duration"].InnerText);
                        this.EntityDescription = Node["EntityDescription"].InnerText;

                        if (Node["BuffDebuff"].ChildNodes.Count > 0)
                        {
                            for (int i = 0; i < Node["BuffDebuff"].ChildNodes.Count; i++)
                            {
                                this.BuffDebuff.Add(new BuffDebuff(Node["BuffDebuff"].ChildNodes[i].InnerText));
                            }
                        }
                        break;
                    }
                }

                if (!this.IsItemExist)
                {
                    throw new Exception("Item not found");
                }


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public ActiveSkill(string name)
        {
            this.LoadXmlDatabase("Assets\\XmlFiles\\ActiveSkill.xml", name);
        }
    }

    public class PassiveSkill : Virtual
    {
        protected List<BuffDebuff> BuffDebuff;

        protected override sealed void LoadXmlDatabase(string xmlPath, string name)
        {
            BuffDebuff = new List<BuffDebuff>();

            try
            {
                XmlDocument xmlDatabase = new XmlDocument();
                xmlDatabase.Load(xmlPath);

                foreach (XmlNode Node in xmlDatabase.DocumentElement)
                {
                    if (Node.Attributes[0].Value == name)
                    {
                        this.IsItemExist = true;

                        this.EntityName = Node.Attributes[0].Value;
                        this.Value = int.Parse(Node["Value"].InnerText);
                        this.Duration = int.Parse(Node["Duration"].InnerText);
                        this.EntityDescription = Node["EntityDescription"].InnerText;

                        if (Node["BuffDebuff"].ChildNodes.Count > 0)
                        {
                            for (int i = 0; i < Node["BuffDebuff"].ChildNodes.Count; i++)
                            {
                                this.BuffDebuff.Add(new BuffDebuff(Node["BuffDebuff"].ChildNodes[i].InnerText));
                            }
                        }

                        break;
                    }
                }

                if (!this.IsItemExist)
                {
                    throw new Exception("Item not found");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public PassiveSkill(string name)
        {
            this.LoadXmlDatabase("Assets\\XmlFiles\\PassiveSkill.xml", name);
        }
    }

    public class Character : Material
    {          
        private SkillSet skillSet;
        private List<Potion> potions;

        private bool isCurrentTurn;
        private string characterName;
        private double expModifier;
        private int gold, score, level;
        private long expirience, expNeedToLvlUp;
        private int toughness, resolution, imagination, vigor, investigation, agility, luck;
        private int initiative, maxNewActionsPerTurn, physicalDamage, lightningDamage, iceDamage, fireDamage, physicalDefense, lightningDefense, iceDefense, fireDefense, criticalHitChance, criticalHitDamage;

        public bool IsCurrentTurn { get; set; }

        public string CharacterName
        {
            get { return this.characterName; }
            set
            {
                if (value == "")
                {
                    this.characterName = " - ";
                }
                else
                {
                    this.characterName = value;
                }
            }
        }

        public int Toughness
        {
            get { return toughness; }
            set
            {
                if (value > 0)
                {
                    toughness = value;
                }
                else
                {
                    toughness = 0;
                }
            }
        }

        public int Resolution
        {
            get { return resolution; }
            set
            {
                if (value > 0)
                {
                    resolution = value;
                }
                else
                {
                    resolution = 0;
                }
            }
        }

        public int Imagination
        {
            get { return imagination; }
            set
            {
                if (value > 0)
                {
                    imagination = value;
                }
                else
                {
                    imagination = 0;
                }
            }
        }

        public int Vigor
        {
            get { return vigor; }
            set
            {
                if (value > 0)
                {
                    vigor = value;
                }
                else
                {
                    vigor = 0;
                }
            }
        }

        public int Investigation
        {
            get { return investigation; }
            set
            {
                if (value > 0)
                {
                    investigation = value;
                }
                else
                {
                    investigation = 0;
                }
            }
        }

        public int Agility
        {
            get { return agility; }
            set
            {
                if (value > 0)
                {
                    agility = value;
                }
                else
                {
                    agility = 0;
                }
            }
        }

        public int Luck
        {
            get { return luck; }
            set
            {
                if (value > 0)
                {
                    luck = value;
                }
                else
                {
                    luck = 0;
                }
            }
        }

        public int MaxNewActionsPerTurn
        {
            get { return maxNewActionsPerTurn; }
            set
            {
                if (value > 0)
                {
                    if (value > 6)
                    {
                        maxNewActionsPerTurn = 6;
                    }
                    else
                    {
                        maxNewActionsPerTurn = value;
                    }
                }
                else
                {
                    maxNewActionsPerTurn = 0;
                }
            }
        }

        public int Initiative
        {
            get { return initiative; }
            set
            {
                if (value > 0)
                {
                    initiative = value;
                }
                else
                {
                    initiative = 0;
                }
            }
        }

        public int PhysicalDamage
        {
            get { return physicalDamage; }
            set
            {
                if (value > 0)
                {
                    physicalDamage = value;
                }
                else
                {
                    physicalDamage = 0;
                }
            }
        }

        public int LightningDamage
        {
            get { return lightningDamage; }
            set
            {
                if (value > 0)
                {
                    lightningDamage = value;
                }
                else
                {
                    lightningDamage = 0;
                }
            }
        }

        public int IceDamage
        {
            get { return iceDamage; }
            set
            {
                if (value > 0)
                {
                    iceDamage = value;
                }
                else
                {
                    iceDamage = 0;
                }
            }
        }

        public int FireDamage
        {
            get { return fireDamage; }
            set
            {
                if (value > 0)
                {
                    fireDamage = value;
                }
                else
                {
                    fireDamage = 0;
                }
            }
        }

        public int PhysicalDefense
        {
            get { return physicalDefense; }
            set
            {
                if (value > 0)
                {
                    if (value > 90)
                    {
                        physicalDefense = 90;
                    }
                    else
                    {
                        physicalDefense = value;
                    }
                }
                else
                {
                    physicalDefense = 0;
                }
            }
        }

        public int LightningDefense
        {
            get { return lightningDefense; }
            set
            {
                if (value > 0)
                {
                    if (value > 90)
                    {
                        lightningDefense = 90;
                    }
                    else
                    {
                        lightningDefense = value;
                    }
                }
                else
                {
                    lightningDefense = 0;
                }
            }
        }

        public int IceDefense
        {
            get { return iceDefense; }
            set
            {
                if (value > 0)
                {
                    if (value > 90)
                    {
                        iceDefense = 90;
                    }
                    else
                    {
                        iceDefense = value;
                    }
                }
                else
                {
                    iceDefense = 0;
                }
            }
        }

        public int FireDefense
        {
            get { return fireDefense; }
            set
            {
                if (value > 0)
                {
                    if (value > 90)
                    {
                        fireDefense = 90;
                    }
                    else
                    {
                        fireDefense = value;
                    }
                }
                else
                {
                    fireDefense = 0;
                }
            }
        }

        public int CriticalHitChance
        {
            get { return criticalHitChance; }
            set
            {
                if (value > 0)
                {
                    criticalHitChance = value;
                }
                else
                {
                    criticalHitChance = 0;
                }
            }
        }

        public int CriticalHitDamage
        {
            get { return criticalHitDamage; }
            set
            {
                if (value > 0)
                {
                    criticalHitDamage = value;
                }
                else
                {
                    criticalHitDamage = 0;
                }
            }
        }

        public int Gold
        {
            get { return this.gold; }
            set
            {
                if (value > 0)
                {
                    this.gold = value;
                }
                else
                {
                    this.gold = 0;
                }
            }
        }

        public int Score
        {
            get { return this.score; }
            set
            {
                if (value > 0)
                {
                    this.score = value;
                }
                else
                {
                    this.score = 0;
                }
            }
        }

        public SkillSet SkillSet
        {
            get { return this.skillSet; }
            set { this.skillSet = value; }
        }

        public List<Potion> Potions
        {
            get { return this.potions; }
            set { this.potions = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public long Expirience
        {
            get { return expirience; }
            set
            {
                if (expirience >= expNeedToLvlUp)
                {
                    this.LevelUp();
                }
                else
                {
                    if (value > 0)
                    {
                        expirience = value;
                    }
                    else
                    {
                        expirience = 0;
                    }
                }
            }
        }

        public double ExpModifier
        {
            get { return expModifier; }
            set
            {
                if (value > 0)
                {
                    expModifier = value;
                }
                else
                {
                    expModifier = 0;
                }
            }
        }

        public long ExpNeedToLevelUp
        {
            get { return expNeedToLvlUp; }
            private set { expNeedToLvlUp = value; }
        }

        protected void LevelUp()
        {
            this.Level++;
            this.expModifier = this.Level * new System.Random().Next(this.Level, this.Level * 2);
            this.expNeedToLvlUp = Convert.ToInt64(this.Level * this.expModifier);
        }

        public override string GetInfo()
        {
            return string.Format("{0} \r\n \r\n Toughness = {1} \r\n Resolution = {2} \r\n Imagination = {3} \r\n Vigor = {4} \r\n Investigation = {5} \r\n Agility = {6}  \r\n Luck = {7} \r\n",
                                base.GetInfo(), this.Toughness, this.Resolution, this.Imagination, this.Vigor, this.Investigation, this.Agility, this.Luck);
        }

        protected void LoadStats(string xmlPath, string name, string type)
        {
            try
            {
                XmlDocument xmlDatabase = new XmlDocument();
                xmlDatabase.Load(xmlPath);

                string characterName = "";
                bool isntNewCharacter = xmlDatabase.SelectSingleNode("//Class") == null;
                bool nodeCondition = true;

                foreach (XmlNode Node in xmlDatabase.DocumentElement)
                {
                    if (!isntNewCharacter)
                    {
                        characterName = xmlDatabase.SelectSingleNode("//Class").InnerText;
                        nodeCondition = true;
                    }

                    if (isntNewCharacter)
                    {
                        characterName = name;
                        nodeCondition = Node.Attributes[0].Value == name;
                    }

                    if (nodeCondition)
                    {
                        this.IsItemExist = true;

                        this.EntityName = characterName;
                        this.CharacterName = name;
                        this.SkillSet = new SkillSet(characterName, type);
                        this.EntityDescription = Node["EntityDescription"].InnerText;

                        this.MaxHP = int.Parse(Node["MaxHP"].InnerText);
                        this.MaxMP = int.Parse(Node["MaxMP"].InnerText);
                        this.MaxArmor = int.Parse(Node["MaxArmor"].InnerText);

                        this.Toughness = int.Parse(Node["Toughness"].InnerText);
                        this.Resolution = int.Parse(Node["Resolution"].InnerText);
                        this.Imagination = int.Parse(Node["Imagination"].InnerText);
                        this.Vigor = int.Parse(Node["Vigor"].InnerText);
                        this.Investigation = int.Parse(Node["Investigation"].InnerText);
                        this.Agility = int.Parse(Node["Agility"].InnerText);
                        this.Luck = int.Parse(Node["Luck"].InnerText);

                        this.Initiative = int.Parse(Node["Initiative"].InnerText);
                        this.MaxNewActionsPerTurn = int.Parse(Node["MaxNewActionsPerTurn"].InnerText);
                        this.PhysicalDamage = int.Parse(Node["PhysicalDamage"].InnerText);
                        this.LightningDamage = int.Parse(Node["LightningDamage"].InnerText);
                        this.IceDamage = int.Parse(Node["IceDamage"].InnerText);
                        this.FireDamage = int.Parse(Node["FireDamage"].InnerText);
                        this.PhysicalDefense = int.Parse(Node["PhysicalDefense"].InnerText);
                        this.LightningDefense = int.Parse(Node["LightningDefense"].InnerText);
                        this.IceDefense = int.Parse(Node["IceDefense"].InnerText);
                        this.FireDefense = int.Parse(Node["FireDefense"].InnerText);
                        this.CriticalHitChance = int.Parse(Node["CriticalHitChance"].InnerText);
                        this.CriticalHitDamage = int.Parse(Node["CriticalHitDamage"].InnerText);

                        this.Score = int.Parse(Node["Score"].InnerText);
                        this.Gold = int.Parse(Node["Gold"].InnerText);
                        this.Level = int.Parse(Node["Level"].InnerText);
                        this.Expirience = int.Parse(Node["Expirience"].InnerText);

                        this.CurrentHP = int.Parse(Node["CurrentHP"].InnerText);
                        this.CurrentMP = int.Parse(Node["CurrentMP"].InnerText);
                        this.CurrentArmor = int.Parse(Node["CurrentArmor"].InnerText);

                        break;
                    }
                }

                if (!this.IsItemExist)
                {
                    throw new Exception("Item not found");
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }

    public class Player : Character
    {
        private long bestScore;

        public long BestScore
        {
            get { return bestScore; }
            set
            {
                if (value > 0)
                {
                    bestScore = value;
                }
                else
                {
                    bestScore = 0;
                }
            }
        }

        public override string GetInfo()
        {
            StringBuilder info = new StringBuilder(string.Format("{0}\r\n Skills:", base.GetInfo()));

            for (int i = 0; i < this.SkillSet.ActiveSkill.Count; i++)
            {
                info.Append(string.Format("\r\n{0} ", this.SkillSet.ActiveSkill[i].GetInfo()));
            }

            return info.ToString();
        }

        private void CreateSaveFile(string playername, string chosenClassName)
        {           
            using (XmlTextWriter textWritter = new XmlTextWriter(string.Format("Assets\\XmlFiles\\SaveFiles\\{0}CharacterSaveFile.xml", playername), Encoding.UTF8))
            {
                textWritter.WriteStartDocument();
                textWritter.WriteStartElement("head");
                textWritter.WriteEndElement();
                textWritter.Close();

                XmlDocument xmlSaveFile = new XmlDocument();
                xmlSaveFile.Load(string.Format("Assets\\XmlFiles\\SaveFiles\\{0}CharacterSaveFile.xml", playername));
                this.LoadStats("Assets\\XmlFiles\\CharacterStats.xml", chosenClassName, "Player");

                this.Score = 0;
                this.BestScore = 0;
                this.Gold = 100;
                this.Level = 1;
                this.Expirience = 0;

                this.CurrentHP = this.MaxHP;
                this.CurrentMP = this.MaxMP;
                this.CurrentArmor = this.CurrentArmor;

                XmlNode characterData = xmlSaveFile.CreateElement(playername + "CharacterData");
                xmlSaveFile.DocumentElement.AppendChild(characterData);

                XmlAttribute PlyName = xmlSaveFile.CreateAttribute("CharacterName");
                PlyName.Value = playername;
                characterData.Attributes.Append(PlyName);
                
                AddNodeToXml(xmlSaveFile, characterData, "Class", chosenClassName);
                AddNodeToXml(xmlSaveFile, characterData, "EntityDescription", this.EntityDescription);

                AddNodeToXml(xmlSaveFile, characterData, "MaxHP", this.MaxHP.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "MaxMP", this.MaxMP.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "MaxArmor", this.MaxArmor.ToString());

                AddNodeToXml(xmlSaveFile, characterData, "Toughness", this.Toughness.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Resolution", this.Resolution.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Imagination", this.Imagination.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Vigor", this.Vigor.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Investigation", this.Investigation.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Agility", this.Agility.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Luck", this.Luck.ToString());

                AddNodeToXml(xmlSaveFile, characterData, "Initiative", this.Initiative.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "MaxNewActionsPerTurn", this.MaxNewActionsPerTurn.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "PhysicalDamage", this.PhysicalDamage.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "LightningDamage", this.LightningDamage.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "IceDamage", this.IceDamage.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "FireDamage", this.FireDamage.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "PhysicalDefense", this.PhysicalDefense.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "LightningDefense", this.LightningDefense.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "IceDefense", this.IceDefense.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "FireDefense", this.FireDefense.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "CriticalHitChance", this.CriticalHitChance.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "CriticalHitDamage", this.CriticalHitDamage.ToString());

                AddNodeToXml(xmlSaveFile, characterData, "Gold", this.Gold.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Score", this.Score.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "BestScore", this.BestScore.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Level", this.Level.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "Expirience", this.Expirience.ToString());

                AddNodeToXml(xmlSaveFile, characterData, "CurrentHP", this.CurrentHP.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "CurrentMP", this.CurrentMP.ToString());
                AddNodeToXml(xmlSaveFile, characterData, "CurrentArmor", this.CurrentArmor.ToString());

                xmlSaveFile.Save(string.Format("Assets\\XmlFiles\\SaveFiles\\{0}CharacterSaveFile.xml", playername));
            }
        }

        private void AddNodeToXml(XmlDocument xmlDatabase, XmlNode mainNode, string nodeName, string nodeValue)
        {
            XmlNode newNode = xmlDatabase.CreateElement(nodeName);
            newNode.InnerText = nodeValue;
            mainNode.AppendChild(newNode);
        }

        public Player(string name, string className)
        {
            if (!File.Exists(string.Format("Assets\\XmlFiles\\SaveFiles\\{0}CharacterSaveFile.xml", name)))
            {
                CreateSaveFile(name, className);
            }
            else
            {
                this.LoadStats(string.Format("Assets\\XmlFiles\\SaveFiles\\{0}CharacterSaveFile.xml", name), className, "Player");
            }
        }
    }

    public class Enemy : Character
    {
        public override string GetInfo()
        {
            StringBuilder info = new StringBuilder(string.Format("{0}\r\n Skills:", base.GetInfo()));

            for (int i = 0; i < this.SkillSet.ActiveSkill.Count; i++)
            {
                info.Append(string.Format("\r\n {0}", this.SkillSet.ActiveSkill[i].GetInfo()));
            }

            return info.ToString();
        }

        public Enemy(string name)
        {
            this.LoadStats("Assets\\XmlFiles\\EnemiesStats.xml", name, "Enemy");
        }

        public Enemy() { }

    }

    public class PlayerAndEnemyDataManage : MonoBehaviour
    {
        void Start()
        {
            Player pl = new Player("Josh", "Warrior");
           
            Debug.Log(pl.GetInfo());
        }

        void Update()
        {

        }
    }
}