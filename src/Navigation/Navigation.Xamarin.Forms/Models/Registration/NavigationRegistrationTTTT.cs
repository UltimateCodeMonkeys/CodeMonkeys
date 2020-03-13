using CodeMonkeys.Core.MVVM;

using System;

using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms.Models
{
    public class NavigationRegistration<TViewModelInterface, TPhoneView, TTabletView, TDesktopView> :
        NavigationRegistration

        where TViewModelInterface : class, IViewModel
        where TPhoneView : Page
        where TTabletView : Page
    {
        public override Type ViewModelInterfaceType => typeof(TViewModelInterface);

        public override Type ViewType
        {
            get
            {
                if (Device.Idiom == TargetIdiom.Tablet)
                {
                    return TabletViewType;
                }
                else if (Device.Idiom == TargetIdiom.Desktop)
                {
                    return DesktopViewType;
                }

                return PhoneViewType;
            }
        }

        public Type PhoneViewType => typeof(TPhoneView);
        public Type TabletViewType => typeof(TTabletView);
        public Type DesktopViewType => typeof(TDesktopView);
    }
}