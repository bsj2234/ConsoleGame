using MyBuffer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Actor(string name, Vec2 position, Vec2 size, bool overlap) {
            this.Name = name;
            this.SetPosition(position);
            this.Size = size;
            this.Overlap = overlap;
            GameManager.AllActors.Add(this);
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
            foreach (Actor otherActor in GameManager.AllActors)
            {
                if (otherActor == this)
                {
                    continue;
                }
                if(CheckCollision(otherActor))
                {
                    //overlap처리
                    if (otherActor.Overlap == true || this.Overlap == true)
                    {
                        if (this.OnOverlap != null)
                        {
                            this.OnOverlap(otherActor);
                        }
                        if (otherActor.OnOverlap != null)
                        {
                            otherActor.OnOverlap(otherActor);
                        }
                        continue;
                    }
                    //collision처리
                    if (otherActor.OnCollision != null)
                    {
                        otherActor.OnCollision(this);
                    }
                    if (this.OnCollision != null)
                    {
                        this.OnCollision(otherActor);
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
        public virtual char? GetRenderChar(int x, int y)
        {
            return 'd';
        }

        public void AddIgnoreCollision(Actor actor)
        {
            ignoreCollision.Add(actor);
        }
    }
}
