using CodeMonkeys.MVVM;

using System;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms
{
    public class NavigationRegistration<TViewModel, TPhoneView, TTabletView> :
        NavigationRegistration

        where TViewModel : class, IViewModel
        where TPhoneView : Page
        where TTabletView : Page
    {
        public override Type ViewModelType => typeof(TViewModel);

        public override Type ViewType
        {
            get
            {
                if (Device.Idiom == TargetIdiom.Tablet ||
                    Device.Idiom == TargetIdiom.Desktop)
                {
                    return TabletViewType;
                }

                return PhoneViewType;
            }
        }

        public Type PhoneViewType => typeof(TPhoneView);
        public Type TabletViewType => typeof(TTabletView);
        public Type DesktopViewType => typeof(TTabletView);
    }
}