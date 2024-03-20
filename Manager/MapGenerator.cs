using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public static class MapGenerator
    {
        private static int mActorCountId = 0;

        public static void MakeWallWithDoor(Vec2 DoorPos, int roomSize, Axis doorSide)
        {
            if (doorSide == Axis.HORIZONTAL)
            {
                Vec2 LeftWallPos = new Vec2(DoorPos.X, DoorPos.Y);
                Vec2 RightWallPos = new Vec2(DoorPos.X - roomSize / 2 - 1, DoorPos.Y);
                new Wall($"AutoWall{mActorCountId++}", LeftWallPos, new Vec2(roomSize / 2, 2), false);
                new Wall($"AutoWall{mActorCountId++}", RightWallPos, new Vec2(roomSize / 2, 2), false);
            }
        }

        public static void DrawMapOne()
        {
            for (int i = 0; i < MapOne.mapHeight; i++)
            {
                for (int k = 0; k < MapOne.mapWidth; k++)
                {
                    if (MapOne.mapStr[i * MapOne.mapWidth + k] == 'W')
                    {
                        new Wall($"Map{i}{k}" ,new Vec2(k,i), new Vec2(1,1), false);
                    } 
                }
            }

        }
    }
    public static class MapOne
    {
        public static string mapStr = MapData.Map;
        public static int mapWidth = mapStr.IndexOf("\r\n") + 2;
        public static int mapHeight = mapStr.Length / (mapWidth); 
    }
}
