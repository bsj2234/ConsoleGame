using MyData;

namespace ConsoleGameProject
{
    public class PlayerInteractArea : Area
    {
        public List<IInteractable> Interactables = new List<IInteractable>();
        public PlayerInteractArea(Player player, string name, Vec2 position, Vec2 size, bool overlap = true) : base(name, position, size, overlap)
        {

        }

        public override bool CheckCollision(Actor other)
        {
            bool isOverlap = base.CheckCollision(other);
            if (isOverlap)
            {
                if (other is IInteractable)
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

        public void UpdateInteractableOverlap(Vec2 playerPos)
        {
            Interactables.Clear();
            Vec2 interactCenter = playerPos - (Size-Vec2.Unit) * .5;
            SetPosition(interactCenter);
            CheckCollisionAllOtherActor();
        }
    }
}
