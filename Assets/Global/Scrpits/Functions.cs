﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.PlayerLoop;

namespace Assets.Global.Scrpits
{
    public class Functions
    {
        public delegate float FunctionF<A>(A arg);
    }
    public static class Timer
    {
        public static float Update(float buffer,float dt)
        {
            float t = buffer;
            if(t >= 0) t -= dt;
            return t;
        }
    }

}