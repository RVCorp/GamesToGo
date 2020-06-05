using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Database.Models;
using GamesToGo.Desktop.Project.Elements;
using System.Linq;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Project
{
    public class WorkingProject
    {
        public ProjectInfo DatabaseObject { get; }

        public Bindable<string> Title { get; private set; }

        private int latestElementID = 0;

        private readonly BindableList<IProjectElement> projectElements = new BindableList<IProjectElement>();

        public IEnumerable<Card> ProjectCards => projectElements.Where(e => e is Card).Select(e => (Card)e);

        public IEnumerable<Token> ProjectTokens => projectElements.Where(e => e is Token).Select(e => (Token)e);

        public IEnumerable<Board> ProjectBoards => projectElements.Where(e => e is Board).Select(e => (Board)e);

        public IBindableList<IProjectElement> ProjectElements => projectElements;

        public WorkingProject(ProjectInfo project)
        {
            DatabaseObject = project;
            Title = new Bindable<string>(string.IsNullOrEmpty(DatabaseObject.Name) ? "New game" : DatabaseObject.Name);
            Title.ValueChanged += name => DatabaseObject.Name = name.NewValue;
        }

        public void AddElement(IProjectElement element)
        {
            element.ID = latestElementID++;
            projectElements.Add(element);
        }

        private void parse()
        {
            //TODO
        }
    }
}
