using MyBuffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData;

namespace ConsoleGameProject
{
    public class Actor : GameObject,IRenderable
    {
        private Vec2 mPosition;
        public ref Vec2 GetPosition()
        {
            return ref mPosition;
        }
        private void SetPosition(Vec2 value)
        {
            mPosition = value;
        }
        protected event Action<Actor> OnCollision;
        protected event Action<Actor> OnOverlap;

        public string Name { get; protected set; }
        public bool Overlap { get; protected set; }
        //ref형식으로 주고싶은ㄷ데 더 좋은방법없나 .으로 접근하고 싶다
        public Vec2 Size { get; protected set; }

        public Actor(string name, Vec2 position, Vec2 size, bool overlap) {
            this.Name = name;
            this.SetPosition(position);
            this.Size = size;
            this.Overlap = overlap;
            Program.AllActors.Add(this);
        }
        public bool CheckCollision(Actor other)
        {
            if (this.Overlap)
                return false;
            int startX = this.GetPosition().X;
            int startY = this.GetPosition().Y;
            int endX = startX + this.Size.X;
            int endY = startY + this.Size.Y;
            int otherStartX = other.GetPosition().X;
            int otherStartY = other.GetPosition().Y;
            int otherEndX = other.GetPosition().X + other.Size.X;
            int otherEndY = other.GetPosition().Y + other.Size.Y;
            //if collision
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
        public bool CheckOverlap(Actor other)
        {
            int startX = this.GetPosition().X;
            int startY = this.GetPosition().Y;
            int endX = startX + this.Size.X;
            int endY = startY + this.Size.Y;
            int otherStartX = other.GetPosition().X;
            int otherStartY = other.GetPosition().Y;
            int otherEndX = other.GetPosition().X + other.Size.X;
            int otherEndY = other.GetPosition().Y + other.Size.Y;
            //if collision
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
        public bool CheckCollisionAllOtherActor()
        {
            bool collion = false;
            foreach (Actor actor in Program.AllActors)
            {
                if (actor == this)
                {
                    continue;
                }
                if(CheckCollision(actor))
                {
                    //overlap처리
                    if (actor.Overlap == true || this.Overlap == true)
                    {
                        if (this.OnOverlap != null)
                        {
                            this.OnOverlap(actor);
                        }
                        if (actor.OnOverlap != null)
                        {
                            actor.OnOverlap(actor);
                        }
                        continue;
                    }
                    //collision처리
                    if (actor.OnCollision != null)
                    {
                        actor.OnCollision(this);
                    }
                    if (this.OnCollision != null)
                    {
                        this.OnCollision(actor);
                    }
                    collion = true;
                }
            }
            if(collion)
            {
                return true;
            }
            else
            {
            return false;
            }
        }


        public virtual char GetRenderChar(int x, int y)
        {
            return 'd';
        }

        //public bool CheckCollision(Vec2 otherPos)
        //{

        //    int startX = this.GetPosition().X;
        //    int startY = this.GetPosition().Y;
        //    int endX = startX + this.Size.X;
        //    int endY = startY + this.Size.Y;



        //    //if collision
        //    if (startX < otherPos.X && endX > otherPos.X
        //        && startY < otherPos.Y && endY > otherPos.Y)
        //    {
        //        return false;
        //    }
        //    else
        //    {
        //        return true;
        //    }
        //}


    }
}
