using System;
using AppGet.Gui.Framework;
using Caliburn.Micro;
using JetBrains.Annotations;

namespace AppGet.Gui.Views.Shared
{
    [UsedImplicitly]
    public class RestartRequiredViewModel : Screen
    {

        public RestartRequiredViewModel()
        {
            Message = $"${AppSession.CurrentManifest.Name} has requested that restart your machine.";
        }

        public string Message { get; }
    }
}