﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGameProject
{
    public class UIPawnStatus: UiContainerGridContent
    {
        public UIPawnStatus(string name, Func<string> contentFunc, Action? action)
            : base(name, contentFunc, null, 1, 1, false)
        {
        }



    }
}