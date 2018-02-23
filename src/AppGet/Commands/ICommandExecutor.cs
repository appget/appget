using System.Threading.Tasks;

namespace AppGet.Commands
{
    public interface ICommandHandler
    {
        bool CanExecute(AppGetOption commandOptions);

        Task Execute(AppGetOption commandOptions);
    }
}