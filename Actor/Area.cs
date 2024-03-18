using MyBuffer;
using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class Area:Actor
    {
        public Area(string name, Vec2 position, Vec2 size, bool overlap = true):base(name, position, size, overlap)
        {

        }

        public override char? GetRenderChar(int x, int y)
        {
            //return checker
            if ((x + y) % 2 == 0)
                return null;
            else
                return 'P';
        }
    }
}
