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
        public int Hp {  get; set; }
        public Pawn(string name, int hp, Vec2 position, Vec2 size, bool overlap):base(name, position, size, overlap)
        {
            this.Hp = hp;
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
    }
}
