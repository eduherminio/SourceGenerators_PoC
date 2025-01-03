﻿using System;

namespace Library.Generator
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    internal sealed class GeneratedPackAttribute : Attribute
    {
        public short MG { get; }
        public short EG { get; }

        public GeneratedPackAttribute(short mg, short eg)
        {
            MG = mg;
            EG = eg;
        }
    }
}