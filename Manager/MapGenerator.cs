using MyData;
using System.Text;

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
                        new Wall($"Map{i}{k}", new Vec2(k, i), new Vec2(1, 1), false);
                    }
                    if (MapOne.mapStr[i * MapOne.mapWidth + k] == 'G')
                    {
                        new Goal($"Map{i}{k}", new Vec2(k, i), new Vec2(1, 1)).RenderC = 'G';
                    }
                }
            }

        }

        public static string DrawCurrentColliderMap(Vec2 center, int size)
        {
            int a = 10;
            StringBuilder map = new StringBuilder(a);
            //맵크기
            Vec2 mapSize = new Vec2(size * 2, size);

            //크기할당 가로는 두배로
            for (int i = 0; i < mapSize.X * mapSize.Y; i++)
            {
                map.Append(' ');
            }

            //모든 액터 돌며 충돌체면 W, 골이면 G 할당
            foreach(Actor actor in GameManager.AllActors)
            {
                int actorHeight = (int)actor.Size.Y;
                int actorWidth = (int)actor.Size.X;

                if (actorWidth > 2)
                {
                    Thread.Sleep    (1);
                }

                // 플레이어는 패스
                if (actor is Player)
                    continue;
                //Todo CenterCalc Function으로 만들기
                //범위 모두 벗어나면 패스
                //액터와 맵의 충돌 검사
                if (!(actor is Goal) && !actor.CheckCollision(center.GetLeftTopCoord(mapSize), mapSize))
                {
                    continue;
                }
                //오버랩 패스
                if (!(actor is Goal) && actor.Overlap == true)
                {
                    continue;
                }
                for (int y = 0; y < actorHeight; y++)
                {
                    for (int x = 0; x < actorWidth; x++)
                    {
                        Vec2 curSize = new Vec2(x, y);

                        // 액터를 시작위치에 상대적으로
                        Vec2 pos = actor.GetPosition() - center.GetLeftTopCoord(mapSize) + curSize;
                        int index = pos.ToOneDimentional(mapSize.X);

                        //인덱스 초과나 미만시 패스
                        if (index < 0 || index >= mapSize.X * mapSize.Y)
                            continue;

                        

                        if (map[index] != 'G')
                            map[index] = 'W';
                        //골 설정
                        if(actor is Goal)
                            map[index] = 'G';
                    }
                }
            }

            return map.ToString();
        }
    }
    public static class MapOne
    {
        public static string mapStr = MapData.Map;
        public static int mapWidth = mapStr.IndexOf("\r\n") + 2;
        public static int mapHeight = mapStr.Length / (mapWidth);
    }
}

/*
 * 
 * 대각선 움직임은 어떻게 따로 처리할까
 * 좌상의 경우 좌상 상좌 둘개의 조합으로 가능
 * 그럼 이걸 어떻게 쉽게 만들까
 * 만약 대각 입력바드염ㄴ
 * 대각에따라 조상 상좌 둘다 실행후 하나라도 가능하면 이동
 */