
using MyData;
using ConsoleExtenderNs;

namespace ConsoleGameProject
{
    public class Goal : Actor
    {
        public Goal(string name, Vec2 pos, Vec2 size):base(name,pos,size,true)
        {
            RenderPriority = 3;
            OnOverlap += DestroyNearWall;
            RenderC = 'G';
        }

        private void DestroyNearWall(Actor other)
        {

            //되긴 하는데 일단 Actor에서 모든 액터와 충돌검사중 충돌 이벤트 발생시 벽을 삭제해서 문제가있었다
            //(반복자 사용중 리스트에 변화가 생김)//아래의 Destry안에 모든액터에서 빼는 연산이있음
            //그래서일단 리스트로 이벤트 발생할것들만 따로 뺴놨었는데
            //이것조차 문제가 발생할 가능성이 있지 않나?
            //참조를 아직 하고있는 애들이 있기때문에 문제가 발생하지 않을것 같다
            //그냥 Destroy를 비동기로 딜레이시키면 충돌검사 이후에 작동해서 문제가 없을수 있을것같았지만 다시한번 생각해보니 문제가 발생하지 않을것 같기두?
            Vec2 rangeSize = new Vec2(3, 3);
            Vec2 rangePos = GetPosition().GetLeftTopCoord(rangeSize);
            List<Actor> col = Actor.CheckAllCollision(rangePos, rangeSize);
            foreach (Actor c in col) 
            {
                if(c is Wall)
                {
                    c.Destroy();
                }
            }
            OnOverlap -= DestroyNearWall;
            ConsoleExtender.ShakeWindow(8, 15, 20);
            this.Destroy();
        }
    }
}
