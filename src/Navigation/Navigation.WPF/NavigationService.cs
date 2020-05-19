using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMonkeys.Navigation.WPF
{
    public class NavigationService :
        INavigationService
    {
        public IReadOnlyCollection<INavigationRegistration> Registrations => throw new NotImplementedException();

        public void ClearCache()
        {
            throw new NotImplementedException();
        }

        public Task CloseAllAsync()
        {
            throw new NotImplementedException();
        }

        public void Register(INavigationRegistration registrationInfo)
        {
            throw new NotImplementedException();
        }

        Task INavigationService.CloseAsync<TViewModelInterface>()
        {
            throw new NotImplementedException();
        }

        Task INavigationService.CloseAsync<TViewModelInterface, TParentViewModelInterface>()
        {
            throw new NotImplementedException();
        }

        Task INavigationService.CloseAsync<TViewModelInterface, TParentViewModelInterface, TResult>(TResult resultData)
        {
            throw new NotImplementedException();
        }

        Task INavigationService.SetRoot<TViewModelInterface>()
        {
            throw new NotImplementedException();
        }

        Task INavigationService.ShowAsync<TViewModelInterface>()
        {
            throw new NotImplementedException();
        }

        Task INavigationService.ShowAsync<TViewModelInterface, TModel>(TModel modelToPass)
        {
            throw new NotImplementedException();
        }

        void INavigationService.Unregister<TViewModelInterface>()
        {
            throw new NotImplementedException();
        }
    }
}