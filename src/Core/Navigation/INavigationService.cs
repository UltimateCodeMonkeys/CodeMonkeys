using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace CodeMonkeys.Navigation
{
    public interface INavigationService
    {
        IReadOnlyCollection<INavigationRegistration> Registrations { get; }

        /// <summary>
        /// Register a ViewModel interface to a View
        /// </summary>
        /// <param name="registrationInfo">The predefined registration</param>
        void Register(INavigationRegistration registrationInfo);


        /// <summary>
        /// Removes an existing mapping between a ViewModel and a View
        /// </summary>
        /// <typeparam name="TViewModelInterface"></typeparam>
        void Unregister<TViewModelInterface>()
            where TViewModelInterface : class, IViewModel;



        /// <summary>
        /// Sets the application's root view using the ViewModel interface and the registered View type
        /// </summary>
        /// <typeparam name="TViewModelInterface"></typeparam>
        /// <param name="dataContext">A ViewModel instance of the given interface type</param>
        Task SetRootAsync<TViewModelInterface>()
            where TViewModelInterface : class, IViewModel;


        /// <summary>
        /// Creates a new instance of the ViewModel interface type, looks up the associated view and displays it
        /// </summary>
        Task ShowAsync<TViewModelInterface>()
            where TViewModelInterface : class, IViewModel;

        /// <summary>
        /// Creates a new instance of the ViewModel interface type, initializes it using the parameter, looks up the associated view and displays it
        /// </summary>
        /// <param name="modelToPass">The data that should be used to initialize the ViewModel</param>
        Task ShowAsync<TViewModelInterface, TModel>(
            TModel modelToPass)
            where TViewModelInterface : class, IViewModel<TModel>;



        /// <summary>
        /// Closes the View that is associated with the ViewModel interface type
        /// </summary>
        Task CloseAsync<TViewModelInterface>()
            where TViewModelInterface : class, IViewModel;

        /// <summary>
        /// Closes the View that is associated with the ViewModel interface type and informs the parent one
        /// </summary>
        Task CloseAsync<TViewModelInterface, TParentViewModelInterface>()
            where TViewModelInterface : class, IViewModel
            where TParentViewModelInterface : class, IViewModel, IListenToChildViewModelClosing;
        
        /// <summary>
        /// Closes the View that is associated with the ViewModel interface type and informs the parent one
        /// </summary>
        /// <param name="resultData">The data that should be passed back to the parent (OnChildViewModelClosed)</param>
        Task CloseAsync<TViewModelInterface, TParentViewModelInterface, TResult>(
            TResult resultData)
            where TViewModelInterface : class, IViewModel
            where TParentViewModelInterface : class, IViewModel, IListenToChildViewModelClosing<TResult>;


        /// <summary>
        /// Clears the view instance cache.
        /// </summary>
        void ClearCache();
    }
}
