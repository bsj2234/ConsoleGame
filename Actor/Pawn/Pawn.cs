using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyData;
using System.Diagnostics;
using System.Transactions;

namespace ConsoleGameProject
{
    public class Pawn:Actor
    {
        protected FightComponent fightComponent;
        protected event Action OnDead;
        protected InventoryComponent inventoryComponent;

        protected ECharacterType chracterType;

        public Pawn(string name, int hp, Vec2 position, Vec2 size, bool overlap, ECharacterType characterType):base(name, position, size, overlap)
        {
            fightComponent = new FightComponent(this, hp, 0);
            inventoryComponent = new InventoryComponent(this);
            this.chracterType = characterType;
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
            UiFightLogManager.Append($"{Name} Attacked {other.Name}!!! Damaged {damage}\n");
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

        public void Heal(int healAmount)
        {
            fightComponent.Heal(healAmount);
        }

        public void TakeItem(Pawn other, Item item)
        {
            inventoryComponent.Add(other.RemoveItem(item));
        }

        private Item RemoveItem(Item item)
        {
            return inventoryComponent.Remove(item);
        }

        public FightComponent GetFightComponent()
        {
            return fightComponent;
        }

        public virtual string GetFightCharacterArt()
        {
            return AsciiArts.All[(int)chracterType][0];
        }
    }
}
