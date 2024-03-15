using MyBuffer;
using MyData;

namespace ConsoleGameProject
{
    
    internal class Wall : Actor
    {

        public Wall(string name, Vec2 position, Vec2 size, bool overlap):base(name,position,size,overlap)
        {

        }

        public override char GetRenderChar(int x, int y)
        {
            return 'ㅇ';
        }
    }
}
