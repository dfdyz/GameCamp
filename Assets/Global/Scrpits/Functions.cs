using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Assets.Global.Scrpits
{
    public class Functions
    {
        public delegate R Function<A,out R>(A arg);
        public delegate R Function2<A,B,out R>(A arg1,B arg2);
        public delegate float Function3<A, B, C>(A arg1, B arg2, C arg3);
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

    public static class Hit
    {
        private static ContactFilter2D filter;

        public static int Overlap(Collider2D org,int Layer, Collider2D[] results)
        {
            filter.useTriggers = false;
            filter.useLayerMask = true;
            filter.layerMask= Layer;
            return org.OverlapCollider(filter,results);
        }
    }

    public static class Battle
    {
        public static bool Damage(IhasTag tags,float damage)
        {
            if(!tags) return false;
            if (!tags.hasVar("float","_health")) return false;
            float hp = tags.getFloat("_health");
            hp -= damage;
            if(hp < 0) hp = 0;
            tags.putFloat("_health",hp);
            return true;
        }
    }

}
