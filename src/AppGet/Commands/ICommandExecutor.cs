using System;
using System.Threading.Tasks;

namespace AppGet.Commands
{
    public interface ICommandHandler
    {
        Task Execute(AppGetOption commandOptions);
    }
}