using CodeMonkeys.MVVM.ViewModels;

namespace CodeMonkeys.Samples.ViewModels
{
    public class NestedViewModel :
        ViewModelBase
    {
        public string Title
        {
            get => GetValue<string>();
            set => SetValue(value);
        }
    }
}