using CodeMonkeys.Core.Interfaces.MVVM;

using System;
using Xamarin.Forms;

namespace CodeMonkeys.Navigation.Xamarin.Forms.Models
{
    public class NavigationRegistration<TViewModelInterface, TPhoneView, TTabletView> :
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