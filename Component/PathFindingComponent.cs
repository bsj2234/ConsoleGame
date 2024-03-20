using MyData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class PathFindingComponent
    {
        public PathFindingComponent() 
        { }

        public List<EDirection> FindPath()
        {
            Queue<List<EDirection>> bfsQueue = new Queue<List<EDirection>>();
            for(int i = 0; i < Enum.GetValues<EDirection>().Length; i++)
            {
                bfsQueue.Enqueue(new List<EDirection>());
            }

            return;
        }
    }
}
