using System;
using System.Reflection;
using GamesToGo.Common.Game;

namespace GamesToGo.Game.LocalGame.Arguments
{
    public enum ArgumentType
    {
        [ReturnType(ArgumentReturnType.Default)]
        DefaultArgument = 0,

        [ReturnType(ArgumentReturnType.Comparison)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SingleCard,
            ArgumentReturnType.CardType,
        })]
        CompareCardTypes = 1,

        [ReturnType(ArgumentReturnType.Privacy)]
        PrivacyType = 2,

        [ReturnType(ArgumentReturnType.SingleNumber)]
        NumberArgument = 3,

        [ReturnType(ArgumentReturnType.TokenType)]
        TokenType = 4,

        [ReturnType(ArgumentReturnType.SinglePlayer)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
        })]
        PlayerRightOfPlayerWithToken = 5,

        [ReturnType(ArgumentReturnType.SinglePlayer)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.TokenType,
        })]
        PlayerWithToken = 6,

        [ReturnType(ArgumentReturnType.Comparison)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.TokenType,
        })]
        ComparePlayerWithTokenHasXTokens = 7,

        [ReturnType(ArgumentReturnType.Comparison)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.TokenType,
        })]
        ComparePlayerWithTokenHasMoreThanXTokens = 8,

        [ReturnType(ArgumentReturnType.CardType)]
        CardType = 10,

        [ReturnType(ArgumentReturnType.Comparison)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.CardType,
        })]
        ComparePlayerHasNoCardType = 11,

        [ReturnType(ArgumentReturnType.TileType)]
        TileType = 12,

        [ReturnType(ArgumentReturnType.Comparison)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.CardType,
        })]
        ComparePlayerHasCardType = 13,

        [ReturnType(ArgumentReturnType.Comparison)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.MultiplePlayer,
            ArgumentReturnType.TokenType,
        })]
        ComparePlayerHasTokenType = 14,

        [ReturnType(ArgumentReturnType.MultipleCard)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.TileType,
        })]
        FirstXCardsFromTile = 16,

        [ReturnType(ArgumentReturnType.MultipleCard)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
            ArgumentReturnType.TokenType,
        })]
        PlayerCardsWithToken = 17,

        [ReturnType(ArgumentReturnType.MultipleCard)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.TokenType,
        })]
        CardsWithToken = 18,

        [ReturnType(ArgumentReturnType.SinglePlayer)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
        })]
        PlayerChosenByPlayer = 19,

        [ReturnType(ArgumentReturnType.Comparison)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SingleNumber,
            ArgumentReturnType.Direction,
            ArgumentReturnType.TileType,
            ArgumentReturnType.CardType,
        })]
        CompareDirectionHasXTilesWithCards = 20,

        [ReturnType(ArgumentReturnType.Direction)]
        DirectionType = 21,

        [ReturnType(ArgumentReturnType.SingleTile)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer
        })]
        TileWithNoCardsChosenByPlayer = 22,

        [ReturnType(ArgumentReturnType.SinglePlayer)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SingleNumber,
        })]
        PlayerAtXPosition = 23,

        [ReturnType(ArgumentReturnType.SingleTile)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
        })]
        TileSelectedByPlayer = 24,

        [ReturnType(ArgumentReturnType.CardType)]
        [InnerReturnTypes(new[]
        {
            ArgumentReturnType.SinglePlayer,
        })]
        CardSelectedByPlayer = 25,
    }

    public static class ArgumentTypeExtensions
    {
        public static bool ShouldHaveResult(this ArgumentType type)
        {
            return type.GetType().GetField(type.ToString())?.GetCustomAttribute<InnerReturnTypesAttribute>() == null;
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class InnerReturnTypesAttribute : Attribute
    {
        public ArgumentReturnType[] InnerReturnTypes { get; }
        public InnerReturnTypesAttribute(ArgumentReturnType[] returnTypes)
        {
            InnerReturnTypes = returnTypes;
        }
    }
}
