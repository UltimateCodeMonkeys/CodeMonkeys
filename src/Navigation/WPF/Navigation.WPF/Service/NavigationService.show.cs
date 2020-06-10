using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Activator = CodeMonkeys.Activator;

namespace CodeMonkeys.Navigation.WPF
{
    public partial class NavigationService
    {
        public async Task ShowAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            ThrowIfNotRegistered<TViewModel>();


            var viewModel = await InitializeViewModel<TViewModel>();
            var content = CreateContent<TViewModel>(
                viewModel);


            ShowInternal(
                viewModel,
                content);
        }

        public async Task ShowAsync<TViewModel, TData>(
            TData data)

            where TViewModel : class, IViewModel<TData>
        {
            var viewModel = await InitializeViewModel<TViewModel, TData>(
                data);

            var content = CreateContent<TViewModel>(
                viewModel);


            ShowInternal(
                viewModel,
                content);
        }


        public bool TryGoBack()
        {
            if (!CanGoBack)
                return false;


            var destination = BackStack.LastOrDefault();

            if (destination == null)
                return false;

            BackStack.Remove(destination);


            AddToForwardStack(
                CurrentViewModel,
                CurrentContent);


            SetCurrent(
                destination.ViewModel,
                destination.Content);


            return true;
        }

        public bool TryGoForward()
        {
            if (!CanGoForward)
                return false;


            var destination = ForwardStack.LastOrDefault();

            if (destination == null)
                return false;


            if (!destination.ViewModel.TryGetTarget(
                out var viewModel))
            {
                return false;
            }

            if (!destination.Content.TryGetTarget(
                out var content))
            {
                return false;
            }


            ForwardStack.Remove(destination);


            AddToBackStack(
                viewModel,
                content);

            SetCurrent(
                viewModel,
                content);


            return true;
        }



        internal TView CreateContentInternal<TViewModel, TView>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
            where TView : FrameworkElement
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                typeof(TView),
                out var registration))
            {
                throw new InvalidOperationException(
                    $"No registration found for ViewModel of type {typeof(TViewModel).FullName}!");
            }


            FrameworkElement content;

            if (Configuration.CacheContent)
            {
                content = AddOrUpdateContentCache<TView>(
                    registration);
            }
            else
            {
                content = GetContentInstance<TView>(
                    registration);
            }


            content.DataContext = viewModel;


            if (viewModel is IHandleClosing)
            {
                content.Unloaded += OnContentUnloaded;
            }


            Log?.Info(
                $"View of type {content.GetType().Name} has been created!");

            return (TView)content;
        }


        protected async Task<TViewModel> InitializeViewModel<TViewModel>()

            where TViewModel : class, IViewModel
        {
            return await InitializeViewModelInternal<TViewModel>();
        }

        protected async Task<TViewModel> InitializeViewModel<TViewModel, TData>(
            TData model)

            where TViewModel : class, IViewModel<TData>
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModel>();
            await viewModelInstance.InitializeAsync(
                model);

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModel).Name} has been created and initialized with parameters!");

            return viewModelInstance;
        }


        protected FrameworkElement CreateContent<TViewModel>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
        {
            return CreateContentInternal<TViewModel, FrameworkElement>(
                viewModel);
        }

        protected TView CreateContent<TViewModel, TView>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
            where TView : FrameworkElement
        {
            return CreateContentInternal<TViewModel, TView>(
                viewModel);
        }


        private void ShowInternal(
            IViewModel viewModel,
            FrameworkElement content)
        {
            if (Current != null)
            {
                AddToBackStack(
                    Current.ViewModel,
                    Current.Content);
            }


            ClearForwardStack();


            SetCurrent(
                viewModel,
                content);
        }


        private void AddToBackStack(
            IViewModel viewModel,
            FrameworkElement content)
        {
            var origin = new NavigationStackEntry(
                viewModel,
                content);

            BackStack.Add(origin);
        }

        private void AddToForwardStack(
            IViewModel viewModel,
            FrameworkElement content)
        {
            var origin = new WeakNavigationStackEntry(
                viewModel,
                content);

            ForwardStack.Add(origin);
        }

        private void SetCurrent(
            IViewModel viewModel,
            FrameworkElement content)
        {
            Current = new NavigationStackEntry(
                viewModel,
                content);


            RaisePropertyChanged(
                nameof(CanGoBack));

            RaisePropertyChanged(
                nameof(CanGoForward));
        }




        internal static async Task<TViewModelInterface> InitializeViewModelInternal<TViewModelInterface>()

            where TViewModelInterface : class, IViewModel
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModelInterface>();
            await viewModelInstance.InitializeAsync();

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModelInterface).Name} has been created and initialized!");

            return viewModelInstance;
        }


        private static TView GetContentInstance<TView>(
            INavigationRegistration registration)

            where TView : FrameworkElement
        {
            if (registration.ResolveViewUsingDependencyInjection)
            {
                return (TView)dependencyResolver.Resolve(
                    registration.ViewType);
            }
            else
            {
                return (TView)Activator.CreateInstance(
                        registration.ViewType);
            }
        }
    }
}