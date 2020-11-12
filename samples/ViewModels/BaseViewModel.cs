using System.Threading.Tasks;

using CodeMonkeys.MVVM.ViewModels;
using CodeMonkeys.Navigation.ViewModels;

namespace CodeMonkeys.Samples.ViewModels
{
    public class BaseViewModel :
        ViewModelBase,
        IHandleClosing
    {
        public bool IsBusy
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }

        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }


        public virtual Task OnClosing()
        {
            return Task.CompletedTask;
        }
    }
}