using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MyBuffer;
using MyData;

namespace ConsoleGameProject
{
    public class Player:Pawn
    {
        public static Random random = new Random();
        private FightComponent fightComponent;
        private PlayerInteractArea InteractArea;
        public bool Fight { get; set; }
        public Player(string name, int hp, Vec2 position, Vec2 size, bool overlap):base(name, hp, position, size, overlap)
        {
            fightComponent = new FightComponent(this, hp, 100);
            //사이즈는 앞뒤 양옆 한줄씩 크게 하기 위해서
            Vec2 interactSize = size + new Vec2(2, 2);
            Vec2 interactCenter = position - interactSize * .5;
            InteractArea = new PlayerInteractArea(this, "InteractArea", interactCenter, interactSize);
            InteractArea.AddIgnoreCollision(this);
            this.AddIgnoreCollision(InteractArea);

            OnOverlap += OverlapWithHuntArae;

            RenderPriority = 1;
        }

        private void OverlapWithHuntArae(Actor actor)
        {
            if (actor is HuntArea)
            {
                if (Player.random.Next(100) < 20)
                {
                    GameManager.StartFight();
                }
            }
        }
        public void StartFight()
        {
            this.Fight = true;
        }

        public void Damaged(int v)
        {
            fightComponent.Damaged(v);
        }

        public override bool Move(EDirection direction)
        {
            bool moved = base.Move(direction);
            InteractArea.UpdateInteractableOverlap(GetPosition());
            Thread.Sleep(20);
            return moved;
        }

        public bool CheckInteract()
        {
            List<IInteractable> interactables = InteractArea.GetInteractList();
            if (interactables.Count > 0)
            {
                interactables[0].Interact();
                return true;
            }
            return false;
        }

        public void Interact()
        {
            CheckInteract();
        }
    }
}
