using CodeMonkeys.MVVM;

using System.Threading.Tasks;

namespace CodeMonkeys.UnitTests.Navigation.ViewModels
{
    public class ParameterViewModel :

        IViewModel<string>
    {
        public string Data { get; set; }

        public bool IsInitialized { get; private set; }


        public Task InitializeAsync()
        {
            IsInitialized = true;

            return Task.CompletedTask;
        }


        public async Task InitializeAsync(
            string data)
        {
            Data = data;


            await InitializeAsync()
                .ConfigureAwait(false);
        }
    }
}