using CodeMonkeys.MVVM;

using System.Threading.Tasks;

namespace CodeMonkeys.Samples.ViewModels
{
    public class ItemDetailsViewModel :
        BaseViewModel,

        IViewModel<string>
    {
        public ItemDetailsViewModel()
        {
        }


        public async Task InitializeAsync(
            string item)
        {
            Title = item;

            await base.InitializeAsync();
        }
    }
}