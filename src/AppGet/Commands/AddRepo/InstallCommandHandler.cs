using System.Threading.Tasks;
using AppGet.Infrastructure.Composition;

namespace AppGet.Commands.AddRepo
{
    [Handles(typeof(AddRepoOptions))]
    public class AddRepoCommandHandler : ICommandHandler
    {


        public async Task Execute(AppGetOption commandOptions)
        {
            var installOptions = (AddRepoOptions)commandOptions;

            return;
        }
    }
}