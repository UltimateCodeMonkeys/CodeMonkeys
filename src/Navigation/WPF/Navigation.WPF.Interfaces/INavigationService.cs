using CodeMonkeys.MVVM;

using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.WPF
{
    public interface INavigationService :
        CodeMonkeys.Navigation.INavigationService
    {
        IViewModel RootViewModel { get; }

        IViewModel CurrentViewModel { get; }


        bool CanGoBack { get; }

        bool CanGoForward { get; }



        Task SetRootWindowAsync<TRootViewModel>()
            where TRootViewModel : class, IViewModel;

        Task SetRootWindowAsync<TRootViewModel, TInitialViewModel>()
            where TRootViewModel : class, IViewModel
            where TInitialViewModel : class, IViewModel;


        bool TryGoBack();

        bool TryGoForward();



        void ClearStacks();

        void ClearBackStack();

        void ClearForwardStack();
    }
}