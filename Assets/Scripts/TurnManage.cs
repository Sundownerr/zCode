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

        private void TurnStart()
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

        private void TurnEnd()
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
        private List<GameObject> cards = new List<GameObject>();
        private Button btn;
        private Vector3 newPos;
        private bool cardSpawned;
        

        public GameObject Card;

        private void PlaceCards()
        {
            for (int i = 0; i < cards.Count; i++)
            {
                float newPosX = -Screen.width / 3.7f + ((Screen.width / 10 - cards.Count * 5f) * i);
                float newPosY = -Screen.height / 2f + Random.Range(0, 7);

                newPos = new Vector3(newPosX, newPosY);

                cards[i].GetComponent<CardsHover>().startPos = newPos;
            }
        }

        void Start()
        {
            cards.AddRange(GameObject.FindGameObjectsWithTag("Card"));
            btn = GetComponentInChildren<Button>();
            btn.onClick.AddListener(CreateCard);

            if(cards.Count > 0)
            {
                PlaceCards();
            }

            Screen.SetResolution(1440, 900, false);
        }

        private void CreateCard()
        {
            GameObject newCard = Instantiate(Card, new Vector3(Screen.width, Screen.height / 2, 0), new Quaternion());
            newCard.transform.SetParent(transform, false);
            cards.Add(newCard);
            
            PlaceCards();
        }

        void Update()
        {
            
        }

       
    }
}