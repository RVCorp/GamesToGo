using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using GamesToGo.Common.Game;

namespace GamesToGo.Game.LocalGame.Arguments
{
    public class ArgumentParameter
    {
        public ArgumentType Type { get; set; }

        public List<ArgumentParameter> Arguments { get; set; } = new List<ArgumentParameter>();

        public List<int> Result { get; set; }

    }

    public static class ArgumentReturnTypeExtensions
    {
        public static ArgumentReturnType[] InnerReturnTypes(this ArgumentType type)
        {
            return type.GetType().GetField(type.ToString())?.GetCustomAttribute<InnerReturnTypesAttribute>()
                ?.InnerReturnTypes;
        }
        public static ArgumentReturnType ReturnType(this ArgumentType type)
        {
            return type.GetType().GetField(type.ToString())?.GetCustomAttribute<ReturnTypeAttribute>()
                ?.ReturnType ?? ArgumentReturnType.Default;
        }
    }

    public class ReturnTypeAttribute : Attribute
    {
        public ArgumentReturnType ReturnType { get; }

        public ReturnTypeAttribute(ArgumentReturnType returnType)
        {
            ReturnType = returnType;
        }
    }
}
