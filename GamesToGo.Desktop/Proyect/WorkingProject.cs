using System;
using System.Collections.Generic;
using System.Text;
using GamesToGo.Desktop.Database.Models;
using osu.Framework.Bindables;

namespace GamesToGo.Desktop.Proyect
{
    public class WorkingProject
    {
        public ProyectInfo DatabaseObject { get; }

        public Bindable<string> Title { get; private set; }

        public WorkingProject(ProyectInfo proyect)
        {
            DatabaseObject = proyect;
            Title = new Bindable<string>(string.IsNullOrEmpty(DatabaseObject.Name) ? "New game" : DatabaseObject.Name);;
            Title.ValueChanged += name => DatabaseObject.Name = name.NewValue;
        }
    }
}
