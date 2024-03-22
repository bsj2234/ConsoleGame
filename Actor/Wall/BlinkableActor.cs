using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class BlinkableActor : Actor
    {
        public BlinkableActor(string name, Vec2 position, Vec2 size, bool overlap = true) : base(name, position, size, overlap)
        {
        }

        public override char? GetRenderChar(int x, int y)
        {
            if (GameManager.UiFocusedBlink)
            {
                return ' ';
            }
            return base.GetRenderChar(x, y);
        }
    }
}
