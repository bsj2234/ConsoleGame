using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    internal class Enemy : Pawn
    {
        public Enemy(string name, int hp, Vec2 position, Vec2 size, bool overlap):base(name, hp, position, size, overlap) 
        {
            
        }
    }
}
