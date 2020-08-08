using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Project;
using GamesToGo.Desktop.Project.Elements;
using osu.Framework.Allocation;

namespace GamesToGo.Desktop.Graphics
{
    public class BoardObjectManagerContainer : ObjectManagerContainer<Tile, TileButton>
    {
        public BoardObjectManagerContainer() : base("Casillas de la escena")
        {
        }

        [BackgroundDependencyLoader]
        private void load(WorkingProject project)
        {
            BindToList(project.ProjectElements);
        }
    }
}
