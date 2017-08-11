using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DataManagement;
using MapMechanics;

namespace TurnMechanics
{
    public class TurnData
    {
       
        private Player player;
        private List<Enemy> enemies;
        private MapData mapData;
        private Room roomData;
        private int turnCount;
        private string nextTurn;
        
        public int TurnCount
        {
            get { return turnCount; }
            set
            {
                if(value >= 0)
                {
                    turnCount = value;
                } else
                {
                    turnCount = 0;
                }
            }
        }

        public string NextTurn
        {
            get { return nextTurn; } 
            set
            {
                if(value == "Player" | value == "Enemy")
                {
                    nextTurn = value;
                } else
                {
                    nextTurn = "";
                }
            }
        }

        public TurnData(Player player, List<Enemy> enemies, MapData mapData, Room roomData)
        {
            this.player = player;
            this.enemies = enemies;
            this.mapData = mapData;
            this.roomData = roomData;
        }

        public void TurnStart()
        {
            Enemy mostInitiativeEnemy = new Enemy();

            if (enemies.Count > 1)
            {               
                for (int i = 1; i < enemies.Count; i++)
                {
                    if(enemies[i].Initiative > enemies[i - 1].Initiative)
                    {
                        mostInitiativeEnemy = enemies[i];
                    } else
                    {
                        mostInitiativeEnemy = enemies[i - 1];
                    }
                }
            }

            if(player.Initiative > mostInitiativeEnemy.Initiative)
            {
                NextTurn = "Player";
                player.IsCurrentTurn = true;
                mostInitiativeEnemy.IsCurrentTurn = false;
            } else
            {
                NextTurn = "Enemy";
                mostInitiativeEnemy.IsCurrentTurn = true;
                player.IsCurrentTurn = false;
            }
        }

        public void TurnEnd()
        {
            if(NextTurn == "Player")
            {
                NextTurn = "Enemy";
                enemies[0].IsCurrentTurn = true;
                player.IsCurrentTurn = false;
            } else
            {
                NextTurn = "Player";
                player.IsCurrentTurn = true;
                enemies[0].IsCurrentTurn = false;
            }
        }
    }

    public class TurnManage : MonoBehaviour
    {
        private List<GameObject> cards;
        private Button createCardButton;
        private TurnData turnData;

        private void CreateCard()
        {
            int cardRand = Random.Range(0, 3);
            string cardName;

            switch (cardRand)
            {
                case 0:
                    cardName = "Prefabs/testCardGray";
                    break;

                case 1:
                    cardName = "Prefabs/testCardRed";
                    break;

                case 2:
                    cardName = "Prefabs/testCardYellow";
                    break;

                default:
                    Debug.Log("Error when loading card");
                    cardName = "error";
                    break;
            }
          
            cards.Add((GameObject)Instantiate(Resources.Load(cardName),  transform));
          
            PlaceCard();
        }

        private void PlaceCard()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                float newPosX = -Screen.width / 3.7f + (i * (Screen.width / (10f + cards.Count)));              
                float newPosY = -Screen.height / 2f + Random.Range(0, 7);

                newPosX = Mathf.Clamp(newPosX, -Screen.width / 3.7f, Screen.width / 3.7f);            

                cards[i].GetComponent<CardsHover>().startPos.Set(newPosX, newPosY, -i);               
            }
        }

        void Start()
        {
            Player ply = new Player("Josh", "Warrior");
            List<Enemy> enm = new List<Enemy>() { new Enemy("Rat") };
            MapData md = new MapData(ply.Level);

            turnData = new TurnData(ply, enm, md, new Room("Empty", 0, 1));
            turnData.TurnStart();

            Screen.SetResolution(1440, 900, false);

            cards = new List<GameObject>();
            cards.AddRange(GameObject.FindGameObjectsWithTag("Card"));

            createCardButton = GetComponentInChildren<Button>();
            createCardButton.onClick.AddListener(CreateCard);

            if(cards.Count > 0)
            {
                PlaceCard();
            }          
        }      

        void Update()
        {
            
        }

       
    }
}