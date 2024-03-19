using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public static class UIFightLogManager
    {
        private static string content = "";

        public static void Append(string str)
        {
            content = String.Concat(content, str);
        }

        public static void Clear()
        {
            content = "";
        }

        public static string GetContent() { return content; }
    }
}
