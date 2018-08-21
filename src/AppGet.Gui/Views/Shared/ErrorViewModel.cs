using System;
using AppGet.Gui.Framework;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace AppGet.Gui.Views.Shared
{
    [UsedImplicitly]
    public class ErrorViewModel : Screen
    {
        public ErrorViewModel(Exception ex) : this(ex.GetType().Name, ex.Message)
        {

        }

        public ErrorViewModel(string title, string message)
        {
            Title = title;
            Message = message;
        }

        public string Title { get; }
        public string Message { get; }
    }
}