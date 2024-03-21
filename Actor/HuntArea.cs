﻿using MyData;

namespace ConsoleGameProject
{
    internal class HuntArea : Area
    {
        public HuntArea(string name, Vec2 position, Vec2 size, bool overlap = true) : base(name, position, size, overlap)
        {

        }

        public override char? GetRenderChar(int x, int y)
        {
            //return checker
            if ((x + y) % 2 == 0)
                return null;
            else
                return 'a';
        }
    }
}
