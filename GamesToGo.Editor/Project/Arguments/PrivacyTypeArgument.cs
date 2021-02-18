using System;
using GamesToGo.Editor.Project.Elements;
using osu.Framework.Bindables;

namespace GamesToGo.Editor.Project.Arguments
{
    public class PrivacyTypeArgument : Argument, IHasResult
    {
        public override int ArgumentTypeID => 2;

        public override ArgumentType Type => ArgumentType.Privacy;

        public override ArgumentType[] ExpectedArguments => Array.Empty<ArgumentType>();

        public override string[] Text { get; } =
        {
            @"Privacidad predeterminada",
        };

        public Bindable<int?> Result { get; } = new Bindable<int?>();

        public bool ResultMapsTo(object result) => result is ElementPrivacy privacyValue && (int)privacyValue == Result.Value;
    }
}
