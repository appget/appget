using System.Threading.Tasks;

namespace AppGet.Commands
{

    public interface ICommandHandler
    {

    }

    public interface ICommandHandler<T> : ICommandHandler where T : AppGetOption
    {
        Task Execute(T commandOptions);
    }
}