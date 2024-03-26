using MyBuffer;
using MyData;
using System.Text;

namespace ConsoleGameProject
{
    public struct PosAndPath
    {

        public Vec2 Pos;
        public List<Vec2> Paths;
        public PosAndPath(Vec2 pos, List<Vec2> paths)
        {
            Pos = pos;
            Paths = paths;
        }

        readonly public Vec2 GetPos()
        {
            
            return Pos;
        }

    }
    public class PathFindingComponent
    {
        public List<Actor> CheckedActorList = new List<Actor>();

        public PathFindingComponent()
        {
            PosAndPath pnp = new PosAndPath();
            pnp.Pos = new Vec2(1,1);
        }


        /*
         * 레퍼런스 없이 짠거 그냥 못짬
        public PosAndPath? FindPath(in Vec2 start)
        {
            //결과를 추가하고
            //이동 경로를 추가하고
            //둘다해야할듯
            //이유는 추가한 다음 경로를 
            //도착지에 도달할때까지 모든 경우의 수를 탐색해감 약간 가스가 퍼져너가는 모습과 비슷
            //맵을 따로 받아서 한칸씩 검사하기 vs 모든 액터를 돌면서 충돌 검사하기
            //두번째 방법은 모든 액터를 참조해야한다
            //첫번째 방법은 정리된 뱁을 ㅂ ㅏㄷ아야한다
            //어찌되었든 기준이 되는 데이터가 필요하다
            //한칸짜리 임시 객체를 만들고 옮겨가며 충돌체크하면 되지 않을까
            //현재 칸 검사용 객체
            //
            //근데근데 근데용 깊이를 재려면용 카운트를 해줘야하지 않겠어용 깊이는 움직인 path의카운트지 이 바보야 뒤진다 진짜 그만 멍청해
            //경우의수 ^ 깊이
            Actor temp = new Actor("Temp", start, new Vec2(1, 1), false);

            temp.AddIgnoreCollision(GameManager.player);
            temp.AddIgnoreCollision(GameManager.player.InteractArea);

            Queue<PosAndPath> queue = new Queue<PosAndPath>();
            queue.Enqueue(new PosAndPath(start, new List<Vec2>()));
            while (queue.Count != 0)
            {
                PosAndPath current = queue.Dequeue();
                for (int i = 0; i < Enum.GetValues<EDirection>().Length; i++)
                {
                    //현재 위치와 경로 복사
                    Vec2 pos = current.Pos;
                    List<Vec2> paths = current.Paths.ToList();
                    //이동처리후 큐에 넣을 값 수정
                    pos = MovedPos((EDirection)i, pos);
                    paths.Add(pos);
                    //오버랩 포함
                    temp.SetPosition(pos);
                    List<Actor> collideActors = temp.GetCollisionAllOtherActor();
                    //결과
                    PosAndPath result = new PosAndPath(pos, paths);
                    //아니 못가는지 검사하고 
                    if (temp.CheckCollisionAllOtherActor())
                    {
                    }
                    //갈수 있으면 도착지인지 검사후 아니면 큐에 추가
                    else
                    {

                        foreach (Actor actor in collideActors)
                        {
                            if (actor is Goal)
                            {
                                return result;
                            }
                        }
                        int maxPathCount = 1000;
                        if (paths.Count == maxPathCount)
                        {
                            //넘 길당 나가
                            return null;
                        }
                        queue.Enqueue(result);
                    }
                }
            }
            return null;
        }*/

        //
        public PosAndPath? FindPathBfsWithMap(in Vec2 start, int range)
        {
            //충돌검사를 매번 액터전부를 도니까 너무 느림
            //그냥 충돌 맵 만들어서 돌자
            //저장해놓아야지

            Vec2 mapSize = new Vec2(range * 2, range);
            string map = MapGenerator.DrawCurrentColliderMap(start, range);
            bool[] visited = new bool[map.Length];

            Vec2 center = mapSize.GetCenter();
            //중앙에서 시작
            //초기 큐
            Queue<PosAndPath> queue = new Queue<PosAndPath>();
            queue.Enqueue(new PosAndPath(center, new List<Vec2>()));
            //BFS
            while (queue.Count != 0)
            {
                if(InputManager.IsKeyPressed(EInput.Y))
                {
                    DestroyPath();
                    return null;
                }

                PosAndPath current = queue.Dequeue();
                for (int i = 0; i < Enum.GetValues<EDirection>().Length; i++)
                {
                    //현재 위치와 경로 복사
                    Vec2 pos = current.Pos;
                    List<Vec2> pathList = current.Paths.ToList();
                    //이동처리후 큐에 넣을 값 수정
                    pos = MovedPos((EDirection)i, pos);
                    pathList.Add(pos);
                    //결과
                    PosAndPath result = new PosAndPath(pos, pathList);
                    //못가거나 
                    int curindex = pos.ToOneDimentional(mapSize.X);
                    //넘 길당 나가
                    if (curindex >= map.Length)
                    {
                        foreach (var actor in CheckedActorList)
                        {
                            actor.Destroy();
                        }
                        return null;
                    }
                    if (map[curindex] == 'W' || visited[curindex] == true)
                    {
                        continue;
                    }
                    //이동 후의 현재 위치가 전의 위치랑 같다면//r개
                    //틀린부분이 너무 많았었다 일단 전의 움직였던 부분도 아니었을 뿐더러 
                    //나의 위치는 막혀있지 않았기 때문에 처음부터 2개의 경우의수가 더 많았다
                    //가능할 수는 있겠지만 모든 경우의수를 검사하는 방법이다보니 좋지 못한 방법인것같다
                    //갔던곳을 체크해놓는 방법이 맞다 이건 머리에서 지우자
                    //if (pathList.Count > 3)
                    //{
                    //    Vec2 prevPath = pathList[pathList.Count - 3];
                    //    if (prevPath == pos)
                    //    {
                    //        continue;
                    //    }
                    //}

                    //갔던곳 체크
                    visited[curindex] = true;

                    //갈수 있으면 도착지인지 검사후 아니면 큐에 추가
                    if (map[curindex] == 'G')
                        return result;

                    int maxPathCount = 100;
                    queue.Enqueue(result);
                    Vec2 newPos = pos - mapSize.GetCenter() + start;
                    Actor checkedActor = new Actor("temp", newPos, Vec2.Unit, true);
                    checkedActor.RenderC = '*';
                    CheckedActorList.Add(checkedActor);
                    RenderManager.CustomRanderActor();
                    //Thread.Sleep(10);
                }
            }
            return null;
        }

        public void DestroyPath()
        {
            foreach (var actor in CheckedActorList)
            {
                actor.Destroy();
            }
            CheckedActorList = new List<Actor>();
        }

        private static Vec2 MovedPos(EDirection dir, Vec2 pos)
        {
            switch (dir)
            {
                case EDirection.UP:
                    pos.Y -= 1;
                    break;
                case EDirection.DOWN:
                    pos.Y += 1;
                    break;
                case EDirection.LEFT:
                    pos.X -= 1;
                    break;
                case EDirection.RIGHT:
                    pos.X += 1;
                    break;
            }

            return pos;
        }


        /*public PosAndPath? FindPathBfsWithMap(in Vec2 start)
        {
            //결과를 추가하고
            //이동 경로를 추가하고
            //둘다해야할듯
            //이유는 추가한 다음 경로를 
            //도착지에 도달할때까지 모든 경우의 수를 탐색해감 약간 가스가 퍼져너가는 모습과 비슷

            //맵을 따로 받아서 한칸씩 검사하기 vs 모든 액터를 돌면서 충돌 검사하기
            //두번째 방법은 모든 액터를 참조해야한다
            //첫번째 방법은 정리된 뱁을 ㅂ ㅏㄷ아야한다
            //어찌되었든 기준이 되는 데이터가 필요하다
            //한칸짜리 임시 객체를 만들고 옮겨가며 충돌체크하면 되지 않을까

            //현재 칸 검사용 객체
            //

            //근데근데 근데용 깊이를 재려면용 카운트를 해줘야하지 않겠어용 깊이는 움직인 path의카운트지 이 바보야 뒤진다 진짜 그만 멍청해
            Actor temp = new Actor("Temp", start, new Vec2(1, 1), false);

            temp.AddIgnoreCollision(GameManager.player);
            temp.AddIgnoreCollision(GameManager.player.InteractArea);

            Queue<PosAndPath> queue = new Queue<PosAndPath>();
            queue.Enqueue(new PosAndPath(start, new List<Vec2>()));
            while (queue.Count != 0)
            {
                PosAndPath current = queue.Dequeue();
                for (int i = 0; i < Enum.GetValues<EDirection>().Length; i++)
                {

                    Vec2 pos = current.Pos;
                    List<Vec2> paths = current.Paths.ToList();

                    switch ((EDirection)i)
                    {
                        case EDirection.UP:
                            pos.Y -= 1;
                            break;
                        case EDirection.DOWN:
                            pos.Y += 1;
                            break;
                        case EDirection.LEFT:
                            pos.X -= 1;
                            break;
                        case EDirection.RIGHT:
                            pos.X += 1;
                            break;
                    }
                    paths.Add(pos);

                    PosAndPath result = new PosAndPath(pos, paths);
                    temp.SetPosition(pos);

                    List<Actor> collideActors = temp.GetCollisionAllOtherActor();
                    //아니 못가는지 검사하고 
                    if (temp.CheckCollisionAllOtherActor())
                    {
                    }
                    //갈수 있으면 도착지인지 검사
                    else
                    {

                        foreach (Actor actor in collideActors)
                        {
                            if (actor is Goal)
                            {
                                return result;
                            }
                        }
                        int maxPathCount = 1000;
                        if (paths.Count == maxPathCount)
                        {
                            //넘 길당 나가
                            return null;
                        }
                        queue.Enqueue(result);
                    }
                }
            }
            return null;
        }*/
    }
}
