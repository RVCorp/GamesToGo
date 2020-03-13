using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Database.Models;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project
{
    public class WorkingProject
    {
        public ProjectInfo DatabaseObject { get; }

        public Bindable<string> Title { get; private set; }

        public readonly List<IProjectElement> Cards = new List<IProjectElement>();

        public readonly List<IProjectElement> Tokens = new List<IProjectElement>();

        public readonly List<IProjectElement> Boards = new List<IProjectElement>();

        public readonly List<IProjectElement> Tiles = new List<IProjectElement>();

        public WorkingProject(ProjectInfo project)
        {
            DatabaseObject = project;
            Title = new Bindable<string>(string.IsNullOrEmpty(DatabaseObject.Name) ? "New game" : DatabaseObject.Name);
            Title.ValueChanged += name => DatabaseObject.Name = name.NewValue;
        }

        private void parse()
        {
            //TODO
        }
    }
}
