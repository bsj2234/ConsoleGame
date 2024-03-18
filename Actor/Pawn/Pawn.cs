using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData;
using System.Diagnostics;

namespace ConsoleGameProject
{
    public class Pawn:Actor
    {
        protected FightComponent fightComponent;
        protected event Action OnDead;
        protected InventoryComponent inventoryComponent;
        public Pawn(string name, int hp, Vec2 position, Vec2 size, bool overlap):base(name, position, size, overlap)
        {
            fightComponent = new FightComponent(this, hp, 100, 100);
            inventoryComponent = new InventoryComponent();
        }
        public virtual bool Move(EDirection direction)
        {
            Vec2 tempPos = base.GetPosition();
            switch (direction)
            {
                case EDirection.UP:
                    base.GetPosition().Y--;
                    break;
                case EDirection.DOWN:
                    base.GetPosition().Y++;
                    break;
                case EDirection.LEFT:
                    base.GetPosition().X--;
                    break;
                case EDirection.RIGHT:
                    base.GetPosition().X++;
                    break;
                default: 
                    Debug.Assert(false, "unhandled moving dir");
                    return false;
            }
            if(CheckCollisionAllOtherActor())
            {
                base.GetPosition() = tempPos;
                return false;
            }
            return true;
        }
        public int GetHp()
        {
            return fightComponent.GetHp();
        }
        public void Attack(Pawn other)
        {
            int damage = 10;
            fightComponent.Attack(other.fightComponent, damage);
            UIFightLogManager.Append($"{Name} Attacked {other.Name}!!! Damaged {damage}\n");
        }
        public bool IsDead()
        {
            return this.fightComponent.IsDead();
        }

        public void AddItem(Item item)
        {
            inventoryComponent.Add(item);
        }

        public void SetOpponent(Player player)
        {
            fightComponent.SetOpponent(player);
        }

        public InventoryComponent GetInventory()
        {
            return inventoryComponent;
        }
    }
}
