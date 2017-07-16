using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;
using System.IO;
using UnityEngine;

namespace TurnMechanics
{
    public class TurnData
    {
        private DataManagement.Player player;
        private List<DataManagement.Enemy> enemies;
        private MapMechanics.MapData mapData;
        private MapMechanics.Room roomData;

        public TurnData(DataManagement.Player player, List<DataManagement.Enemy> enemies, MapMechanics.MapData mapData, MapMechanics.Room roomData)
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