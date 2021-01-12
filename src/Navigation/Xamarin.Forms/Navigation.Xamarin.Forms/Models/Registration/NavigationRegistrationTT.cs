using CodeMonkeys.MVVM;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public class NavigationRegistration<TViewModel, TView> :
        NavigationRegistration

        where TViewModel : class, IViewModel
        where TView : Page
    {
        public NavigationRegistration()
            : base(typeof(TViewModel), typeof(TView))
        {

        }
    }
}