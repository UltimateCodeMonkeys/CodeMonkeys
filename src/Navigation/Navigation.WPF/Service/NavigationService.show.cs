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
    public partial class NavigationService :
        DependencyObject,

        INavigationService
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


            SetCurrent(
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


            SetCurrent(
                viewModel,
                content);
        }


        public bool TryGoBack()
        {
            if (!BackStack.Any())
                return false;

            
            var destination = BackStack.Last();
            BackStack.Remove(destination);

            SetCurrent(
                destination.ViewModel,
                destination.Content,
                ForwardStack);


            return true;
        }

        public bool TryGoForward()
        {
            if (!ForwardStack.Any())
                return false;


            var destination = ForwardStack.Last();
            ForwardStack.Remove(destination);

            SetCurrent(
                destination.ViewModel,
                destination.Content);


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
                    content = (TView)Activator.CreateInstance(
                        registration.ViewType);

                    reference.SetTarget(content);
                }
            }
            else content = (TView)Activator.CreateInstance(registration.ViewType);


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


        private void SetCurrent(
            IViewModel viewModel,
            FrameworkElement content)
        {
            SetCurrent(
                viewModel,
                content,
                BackStack);
        }

        private void SetCurrent(
            IViewModel viewModel,
            FrameworkElement content,
            
            IList<NavigationStackEntry> stack)
        {
            var origin = new NavigationStackEntry
            {
                ViewModel = CurrentViewModel,
                Content = CurrentContent
            };

            stack.Add(origin);


            CurrentViewModel = viewModel;
            CurrentContent = content;
        }
    }
}