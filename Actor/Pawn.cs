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
    internal class Pawn:Actor
    {
        private FightComponent fightComponent;
        public Pawn(string name, int hp, Vec2 position, Vec2 size, bool overlap):base(name, position, size, overlap)
        {
            fightComponent = new FightComponent(this, hp, 100, 100);
        }
        public bool Move(Direction direction)
        {
            Vec2 tempPos = base.GetPosition();
            switch (direction)
            {
                case Direction.UP:
                    base.GetPosition().Y--;
                    break;
                case Direction.DOWN:
                    base.GetPosition().Y++;
                    break;
                case Direction.LEFT:
                    base.GetPosition().X--;
                    break;
                case Direction.RIGHT:
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
    }
}
