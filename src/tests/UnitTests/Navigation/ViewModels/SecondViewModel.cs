using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.Navigation.ViewModels
{
    public class SecondViewModel :

        IViewModel,
        IHandleClosing
    {
        public bool IsInitialized { get; private set; }
        public bool Closing { get; private set; }


        public Task InitializeAsync()
        {
            Closing = false;
            IsInitialized = true;

            return Task.CompletedTask;
        }

        public Task OnClosing()
        {
            Closing = true;

            return Task.CompletedTask;
        }
    }
}