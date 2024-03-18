using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class AreaPlayerInteract : Area
    {
        public List<IInteractable> Interactables = new List<IInteractable>();
        public AreaPlayerInteract(Player player, string name, Vec2 position, Vec2 size, bool overlap = true) : base(name, position, size, overlap)
        {
            
        }

        public override bool CheckCollision(Actor other)
        {
            bool isOverlap = base.CheckCollision(other);
            if (isOverlap)
            {
                if(other is IInteractable)
                {
                    Interactables.Add(other as IInteractable);
                }
            }
            return isOverlap;

        }

        internal List<IInteractable> GetInteractList()
        {
            return Interactables;
        }

        public override char? GetRenderChar(int x, int y)
        {
            return null;
        }
    }
}
