using System.Collections.Generic;
using UnityEngine;
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

        public TurnData(Player player, List<Enemy> enemies, MapData mapData, Room roomData)
        {
            this.player = player;
            this.enemies = enemies;
            this.mapData = mapData;
            this.roomData = roomData;
        }
    }

    public class TurnManage : MonoBehaviour
    {
        void Start()
        {
            
        }

        void Update()
        {

        }
    }
}