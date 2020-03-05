using System;
using System.Collections.Generic;

namespace GamesToGo.Desktop.Screens
{
    /// <summary>
    /// Pantalla de inicio de sesión y registro de usuarios, carga un usuario y los proyectos relacionados a él. (WIP)
    /// </summary>
    public class SessionStartScreen : EmptyScreen
    {
        //Agregamos el menu principal como pantalla a la cual se deberia poder acceder
        protected override IEnumerable<Type> FollowingScreens => new[]
        {
            typeof(MainMenuScreen),
            typeof(ProjectHome)
        };
    }
}
