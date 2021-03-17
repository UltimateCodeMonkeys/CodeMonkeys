using System.Collections.Generic;
using System.Threading.Tasks;

using CodeMonkeys.MVVM;

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
        /// <typeparam name="TViewModel"></typeparam>
        void Unregister<TViewModel>()
            where TViewModel : class, IViewModel;


        /// <summary>
        /// Removes all existing mappings between ViewModel and View types
        /// </summary>
        void ResetRegistrations();


        /// <summary>
        /// Sets the application's root view using the ViewModel interface and the registered View type
        /// </summary>
        /// <typeparam name="TViewModel"></typeparam>
        /// <param name="dataContext">A ViewModel instance of the given interface type</param>
        void SetRoot<TViewModel>()
            where TViewModel : class, IViewModel;


        /// <summary>
        /// Creates a new instance of the ViewModel interface type, looks up the associated view and displays it
        /// </summary>
        Task ShowAsync<TViewModel>()
            where TViewModel : class, IViewModel;

        /// <summary>
        /// Creates a new instance of the ViewModel interface type, initializes it using the parameter, looks up the associated view and displays it
        /// </summary>
        /// <param name="data">The data that should be used to initialize the ViewModel</param>
        Task ShowAsync<Interface, TData>(
            TData data)
            where Interface : class, IViewModel<TData>;



        /// <summary>
        /// Closes the View that is associated with the ViewModel interface type
        /// </summary>
        Task CloseAsync<TViewModel>()
            where TViewModel : class, IViewModel;


        /// <summary>
        /// Clears the view instance cache.
        /// </summary>
        void ClearCache();
    }
}