using CodeMonkeys.Configuration;

namespace CodeMonkeys.MVVM.PropertyChanged
{
    public class BindingBaseOptions :
        Options
    {
        public bool UseCommandRelevanceAttribute
        {
            get => GetValue<bool>();
            set => SetValue(value);
        }


        public BindingBaseOptions()
        {
            UseCommandRelevanceAttribute = true;
        }
    }
}