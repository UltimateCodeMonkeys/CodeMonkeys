using CodeMonkeys.MVVM.ViewModels.Navigation;

namespace CodeMonkeys.Samples.ViewModels
{
    public class BaseViewModel :
        ViewModelBase
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
    }
}