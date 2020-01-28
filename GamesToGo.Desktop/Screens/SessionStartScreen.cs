using System;
using System.Collections.Generic;

namespace GamesToGo.Desktop.Screens
{
    public class SessionStartScreen : EmptyScreen
    {
        protected override IEnumerable<Type> FollowingScreens => new[]
        {
            typeof(MainMenuScreen)
        };
    }
}
