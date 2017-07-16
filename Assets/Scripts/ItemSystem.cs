using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DataManagement;
using BonusSystem;

namespace ItemSystem {

    public class Item : Entity
    {
        private bool isBroken;
        private int itemWeight, itemLevel, itemPrice, itemMaxDurability, itemCurrentDurability;
        
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

        protected virtual string GetItemInfo()
        {
            return string.Format("Item name: {0}, Item description: {1} \r\n", EntityName, EntityDescription);
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

    public class ItemSystem : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }
    }
}
