using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Collections.Generic;
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
            // todo: does this make sense? maybe the users WANTS to create a new instance?
            // he can still use GoBack and GoForward methods if not
            /*
            if (BackStack.Last().ViewModel.GetType() == typeof(TViewModel))
            {
                GoBack();
                return;
            }
            else if (ForwardStack.Last().ViewModel.GetType() == typeof(TViewModel))
            {
                GoForward();
                return;
            }
            */

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
            if (!CanGoBack())
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
            if (!CanGoForward())
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


        internal async Task<TViewModelInterface> InitializeViewModelInternal<TViewModelInterface>()

            where TViewModelInterface : class, IViewModel
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModelInterface>();
            await viewModelInstance.InitializeAsync();

            Log?.Info(
                $"ViewModel viewModel of type {typeof(TViewModelInterface).Name} has been created and initialized!");

            return viewModelInstance;
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
                if (ContentCache.All(cachedPage => cachedPage.Type != registration.ViewType))
                {
                    CreateCachedContent(typeof(TView));
                }

                var reference = ContentCache
                    .First(cachedPage => cachedPage.Type == registration.ViewType)
                    .Reference;


                if (!reference.TryGetTarget(out content))
                {
                    content = GetContentInstance<TView>(
                        registration);

                    reference.SetTarget(content);
                }
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


        private TView GetContentInstance<TView>(
            INavigationRegistration registrationInfo)

            where TView : FrameworkElement
        {
            if (registrationInfo.ResolveViewUsingDependencyInjection)
            {
                return (TView)dependencyResolver.Resolve(
                    registrationInfo.ViewType);
            }
            else
            {
                return (TView)Activator.CreateInstance(
                        registrationInfo.ViewType);
            }
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
        }
    }
}