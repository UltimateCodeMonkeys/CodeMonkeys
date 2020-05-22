using CodeMonkeys.MVVM;

using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.Navigation.ViewModels
{
    public class SecondViewModel :

        IViewModel
    {
        public bool IsInitialized { get; private set; }

        public Task InitializeAsync()
        {
            IsInitialized = true;

            return Task.CompletedTask;
        }
    }
}