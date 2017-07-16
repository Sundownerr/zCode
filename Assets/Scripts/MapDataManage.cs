using System.Collections.Generic;
using System;
using System.Xml;
using UnityEngine;
using DataManagement;

namespace MapMechanics
{

    //public static class StaticRandom
    //{
    //    private static int seed;

    //    private static ThreadLocal<new System.Random()> threadLocal = new ThreadLocal<new System.Random()>
    //        (() => new Random(Interlocked.Increment(ref seed)));

    //    static StaticRandom()
    //    {
    //        seed = Environment.TickCount;
    //    }

    //    public static Random Instance { get { return threadLocal.Value; } }
    //}

    public class MapData : Entity
    {       
        private int levelCount, roomsCount, bossCount;
        private List<string> roots = new List<string>();
        private List<string> prefixes = new List<string>();
        private List<string> postfixes = new List<string>();
        private List<int> roomsPerLevel = new List<int>();
        private List<List<Room>> roomsInLevel = new List<List<Room>>();

        int counter = 0;

        private void GenerateMapName(string xmlPath)
        {
            try
            {
                XmlDocument xmlDatabase = new XmlDocument();
                xmlDatabase.Load(xmlPath);

                foreach (XmlNode Node in xmlDatabase.DocumentElement)
                {
                    for (int i = 0; i < Node["Prefix"].ChildNodes.Count; i++)
                    {
                        prefixes.Add(Node["Prefix"].ChildNodes[i].InnerText);
                    }

                    for (int i = 0; i < Node["Root"].ChildNodes.Count; i++)
                    {
                        roots.Add(Node["Root"].ChildNodes[i].InnerText);
                    }

                    for (int i = 0; i < Node["Postfix"].ChildNodes.Count; i++)
                    {
                        postfixes.Add(Node["Postfix"].ChildNodes[i].InnerText);
                    }
                }

                string prefix = prefixes[new System.Random().Next(0, prefixes.Count)] + " ";
                string root = roots[new System.Random().Next(0, roots.Count)];
                string postfix = " " + postfixes[new System.Random().Next(0, postfixes.Count)];

                this.EntityName = prefix + root + postfix;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private void GenerateLevelsAndRooms(int playerLevel)
        {
            List<bool> playerLevelConditions = new List<bool>
            {
             playerLevel >= 1 & playerLevel <= 15,
             playerLevel >= 16 & playerLevel <= 42,
             playerLevel >= 42 & playerLevel <= 100,
             playerLevel > 100
            };

            if (playerLevelConditions[0])
            {
                levelCount = 1;
            }

            else if (playerLevelConditions[1])
            {
                levelCount = new System.Random().Next(1, 2);
            }

            else if (playerLevelConditions[2])
            {
                levelCount = new System.Random().Next(1, 3);
            }

            else if (playerLevelConditions[3])
            {
                levelCount = new System.Random().Next(1, 4);
            }

            roomsCount = (new System.Random().Next(2, levelCount * 10) * levelCount) / new System.Random().Next(1, levelCount);

            if (levelCount >= 2)
            {
                roomsPerLevel = DivideRoomsPerLevel(levelCount, roomsCount);
            }

            else
            {
                roomsPerLevel.Add(roomsCount);
            }

            try
            {
                for (var i = 0; i < levelCount; i++)
                {
                    roomsInLevel.Add(new List<Room>());

                    if (roomsPerLevel[i] > 1)
                    {
                        roomsInLevel[i].Add(new Room("Start", i, 0));
                    }
                    else if (levelCount > 1)
                    {
                        if ((i + 1) == levelCount)
                            roomsInLevel[i].Add(new Room("End", i, roomsPerLevel[i]));
                        else
                            roomsInLevel[i].Add(new Room("ToNextLevel", i, roomsPerLevel[i]));
                    }

                    counter++;

                    for (var j = 1; j < roomsPerLevel[i] - 1; j++)
                    {
                        int rand = new System.Random().Next(1, 5);
                        counter++;

                        if (rand == 2 && bossCount < 1)
                        {
                            roomsInLevel[i].Add(new Room("Boss", i, j));
                            bossCount++;
                        }
                        else
                        {
                            roomsInLevel[i].Add(new Room("Normal", i, j));
                        }
                    }

                    bossCount = 0;
                    counter++;

                    if (levelCount > 1)
                    {
                        if ((i + 1) == levelCount)
                            roomsInLevel[i].Add(new Room("End", i, roomsPerLevel[i] - 1));
                        else
                            roomsInLevel[i].Add(new Room("ToNextLevel", i, roomsPerLevel[i] - 1));
                    }
                    else
                    {
                        roomsInLevel[i].Add(new Room("End", i, roomsPerLevel[i] - 1));
                    }

                    Debug.Log("" + string.Format("\r\nRooms in Level: {0}\r\n", counter));
                    counter = 0;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private List<int> DivideRoomsPerLevel(int levelCount, int roomsCount)
        {
            int i = 0;
            int tempSum = 0;
            double tempNum = 0;
            int calculatedRooms = 0;
            List<int> roomsPerLevel = new List<int>();

            while (i < levelCount - 1)
            {
                tempNum = Convert.ToDouble(roomsCount) / 100 * new System.Random().Next(20, 60);

                if (tempNum > 0)
                    tempSum += Convert.ToInt32(tempNum);
                else
                    return DivideRoomsPerLevel(levelCount, roomsCount);

                roomsPerLevel.Add(Convert.ToInt32(tempNum));
                i++;
            }

            roomsPerLevel.Add(roomsCount - tempSum);

            foreach (var k in roomsPerLevel)
            {
                if (k > 0)
                    calculatedRooms += k;
                else
                    return DivideRoomsPerLevel(levelCount, roomsCount);
            }

            if (calculatedRooms == roomsCount)
                return roomsPerLevel;
            else
                return DivideRoomsPerLevel(levelCount, roomsCount);
        }

        public string GetInfo()
        {
            int sumRoomsCount = 0;

            for (var i = 0; i < roomsPerLevel.Count; i++)
                sumRoomsCount += roomsPerLevel[i];

            return string.Format("Map name: {0}, Levels = {1}, Rooms = {2} ", this.EntityName, levelCount, sumRoomsCount);
        }

        public MapData(int playerLevel)
        {
            this.GenerateMapName("Assets\\XmlFiles\\this.EntityNames.xml");
            this.GenerateLevelsAndRooms(playerLevel);
            Debug.Log("\r\n" + this.GetInfo());
        }
    }

    public class Room
    {
        private string roomType;
        private int RoomLevel, RoomNumber;
        private List<string> EndRoomTypes = new List<string>();
        private List<string> BossRoomTypes = new List<string>();
        private List<string> StartRoomTypes = new List<string>();
        private List<string> NormalRoomTypes = new List<string>();
        private List<string> ToNextLevelRoomTypes = new List<string>();

        public string RoomType
        {
            get { return roomType; }
            private set { roomType = value; }
        }

        private void LoadRoomTypes(string xmlPath)
        {
            try
            {
                XmlDocument xmlDatabase = new XmlDocument();
                xmlDatabase.Load(xmlPath);

                foreach (XmlNode Node in xmlDatabase.DocumentElement)
                {
                    for (int i = 0; i < Node["StartRoom"].ChildNodes.Count; i++)
                    {
                        StartRoomTypes.Add(Node["StartRoom"].ChildNodes[i].InnerText);
                    }

                    for (int i = 0; i < Node["NormalRoom"].ChildNodes.Count; i++)
                    {
                        NormalRoomTypes.Add(Node["NormalRoom"].ChildNodes[i].InnerText);
                    }

                    for (int i = 0; i < Node["BossRoom"].ChildNodes.Count; i++)
                    {
                        BossRoomTypes.Add(Node["BossRoom"].ChildNodes[i].InnerText);
                    }

                    for (int i = 0; i < Node["ToNextLevelRoom"].ChildNodes.Count; i++)
                    {
                        ToNextLevelRoomTypes.Add(Node["ToNextLevelRoom"].ChildNodes[i].InnerText);
                    }

                    for (int i = 0; i < Node["EndRoom"].ChildNodes.Count; i++)
                    {
                        EndRoomTypes.Add(Node["EndRoom"].ChildNodes[i].InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public string GetRoomInfo()
        {
            return string.Format("Room {2} on level {1}, type: {0}", RoomType, RoomLevel, RoomNumber);
        }

        public Room(string roomType, int roomLevel, int roomNumber)
        {
            LoadRoomTypes("Assets\\XmlFiles\\RoomTypes.xml");
            RoomLevel = roomLevel;
            RoomNumber = roomNumber;

            switch (roomType)
            {
                case "Start": RoomType = StartRoomTypes[new System.Random().Next(0, StartRoomTypes.Count)]; break;
                case "Normal": RoomType = NormalRoomTypes[new System.Random().Next(0, NormalRoomTypes.Count)]; break;
                case "Boss": RoomType = BossRoomTypes[new System.Random().Next(0, BossRoomTypes.Count)]; break;
                case "ToNextLevel": RoomType = ToNextLevelRoomTypes[new System.Random().Next(0, ToNextLevelRoomTypes.Count)]; break;
                case "End": RoomType = EndRoomTypes[new System.Random().Next(0, EndRoomTypes.Count)]; break;
            }

            Debug.Log("\r\n" + this.GetRoomInfo());
        }
    }

    public class MapDataManage : MonoBehaviour
    {

        void Start()
        {
            MapData m = new MapData(1200);
        }

        void Update()
        {

        }
    }
}