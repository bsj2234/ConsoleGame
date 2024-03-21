using MyBuffer;
using MyData;

namespace ConsoleGameProject
{
    public class Actor : IRenderable
    {
        private Vec2 mPosition;
        protected List<Actor> ignoreCollision = new List<Actor>();
        protected event Action<Actor> OnCollision;
        public event Action<Actor> OnOverlap;
        public int RenderPriority = 0;
        public char RenderC = 'd';
        public ref Vec2 GetPosition()
        {
            return ref mPosition;
        }
        public void SetPosition(Vec2 value)
        {
            mPosition = value;
        }
        public string Name { get; protected set; }
        public bool Overlap { get; protected set; }
        public Vec2 Size { get; protected set; }


        //포지션은 좌상단을 기준//크기가 가로랑 세로가 다르게 보임
        public Actor(string name, Vec2 position, Vec2 size, bool overlap)
        {
            this.Name = name;
            this.SetPosition(position);
            this.Size = size;
            this.Overlap = overlap;
            GameManager.AllActors.Add(this);

            //SortByRenderPriority 누가 나중에 렌더될지 결정
            GameManager.AllActors.Sort((left, right) =>
            {
                if (left != null && right != null)
                {
                    if (left.RenderPriority > right.RenderPriority)
                    { return 1; }
                    else if (left.RenderPriority < right.RenderPriority)
                    { return -1; }
                    else
                    { return 0; }
                }
                else
                { return 0; }
            });
        }
        //
        public virtual bool CheckCollision(Actor other)
        {
            if (ignoreCollision.Contains(other))
            {
                return false;
            }
            int startX = this.GetPosition().X;
            int startY = this.GetPosition().Y;
            int endX = startX + this.Size.X;
            int endY = startY + this.Size.Y;
            int otherStartX = other.GetPosition().X;
            int otherStartY = other.GetPosition().Y;
            int otherEndX = other.GetPosition().X + other.Size.X;
            int otherEndY = other.GetPosition().Y + other.Size.Y;
            //충돌 검사      서로 침범하는지 검사
            if (startX < otherEndX && endX > otherStartX
                && startY < otherEndY && endY > otherStartY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public virtual bool CheckCollision(Vec2 pos, Vec2 size)
        {
            int startX = this.GetPosition().X;
            int startY = this.GetPosition().Y;
            int endX = startX + this.Size.X;
            int endY = startY + this.Size.Y;
            int otherStartX = pos.X;
            int otherStartY = pos.Y;
            int otherEndX = otherStartX + size.X;
            int otherEndY = otherStartY + size.Y;
            //충돌 검사      서로 침범하는지 검사
            if (startX < otherEndX && endX > otherStartX
                && startY < otherEndY && endY > otherStartY)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //public static bool CheckCollision(int aPosX, int aPosY, int aSizeX, int aSizeY, int bPosX, int bPosY, int bSizeX, int bSizeY)
        //{
        //    int startX = aPosX;
        //    int startY = aPosY;
        //    int endX = startX + aSizeX;
        //    int endY = startY + aSizeY;
        //    int otherStartX = bPosX;
        //    int otherStartY = bPosY;
        //    int otherEndX = otherStartX + bSizeX;
        //    int otherEndY = otherStartY + bSizeY;
        //    //충돌 검사      서로 침범하는지 검사
        //    if (startX < otherEndX && endX > otherStartX
        //        && startY < otherEndY && endY > otherStartY)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        //움직이는 녀서득은 모든 충돌을 검사함
        public bool CheckCollisionAllOtherActor()
        {
            bool collion = false;
            List<Tuple<Actor,Actor>> overlapActorList = new List<Tuple<Actor, Actor>>();
            List<Tuple<Actor, Actor>> collisionActorList = new List<Tuple<Actor, Actor>>();

            foreach (Actor otherActor in GameManager.AllActors)
            {
                if (otherActor == this)
                {
                    continue;
                }
                if (CheckCollision(otherActor))
                {
                    //overlap처리
                    if (otherActor.Overlap == true || this.Overlap == true)
                    {
                        if (this.OnOverlap != null)
                        {
                            overlapActorList.Add(new Tuple<Actor, Actor>(this,otherActor));
                        }
                        if (otherActor.OnOverlap != null)
                        {
                            overlapActorList.Add(new Tuple<Actor, Actor>(otherActor, this));
                        }
                        continue;
                    }
                    //collision처리
                    if (this.OnCollision != null)
                    {
                        collisionActorList.Add(new Tuple<Actor, Actor>(this, otherActor));
                    }
                    if (otherActor.OnCollision != null)
                    {
                        collisionActorList.Add(new Tuple<Actor, Actor>(otherActor, this));
                    }
                    collion = true;
                }
            }

            foreach (var tu in overlapActorList)
            {
                tu.Item1.OnOverlap(tu.Item2);
            }
            foreach (var tu in collisionActorList)
            {
                tu.Item1.OnCollision(tu.Item2);
            }

            if (collion)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public List<Actor> GetCollisionAllOtherActor()
        {
            //overlap이던 collide 던 모두 추가
            List<Actor> collideActors = new List<Actor>();

            bool collion = false;
            foreach (Actor otherActor in GameManager.AllActors)
            {
                if (otherActor == this)
                {
                    continue;
                }
                if (CheckCollision(otherActor))
                {
                    //collision처리
                    collideActors.Add(otherActor);
                    collion = true;
                }
            }
            return collideActors;
        }

        public virtual char? GetRenderChar(int x, int y)
        {
            return RenderC;
        }

        //충돌 무시할 액터들을 추가
        public void AddIgnoreCollision(Actor actor)
        {
            ignoreCollision.Add(actor);
        }

        public static List<Actor> CheckAllCollision(Vec2 pos, Vec2 size)
        {
            //overlap이던 collide 던 모두 추가
            List<Actor> collideActors = new List<Actor>();

            bool collion = false;
            foreach (Actor otherActor in GameManager.AllActors)
            {
                if (otherActor.CheckCollision(pos, size))
                {
                    //collision처리
                    collideActors.Add(otherActor);
                    collion = true;
                }
            }
            return collideActors;
        }

        public void Destroy()
        {
            RenderC = ' ';
            Overlap = true;
            GameManager.AllActors.Remove(this);
        }
    }
}
