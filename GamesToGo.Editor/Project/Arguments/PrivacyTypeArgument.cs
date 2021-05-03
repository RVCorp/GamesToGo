using System;
using GamesToGo.Common.Game;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class PrivacyTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 2;

        public override ArgumentReturnType Type => ArgumentReturnType.Privacy;

        public override ArgumentReturnType[] ExpectedArguments => Array.Empty<ArgumentReturnType>();

        public override string[] Text { get; } =
        {
            @"Privacidad predeterminada",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is ElementPrivacy privacyValue && (int)privacyValue == Result.Value;
    }
}
