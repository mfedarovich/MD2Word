using System;

namespace MD2Word
{
    [AttributeUsage(AttributeTargets.Field)]
    sealed class NesstingStyleAttribute : Attribute
    {
        public int MaxLevel { get; set; }
    }
}