using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CodeMonkeys.Navigation.WPF
{
    public partial class NavigationService :

        CodeMonkeys.Navigation.WPF.INavigationService,
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;



        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);


        private static IDependencyContainer dependencyResolver;
        protected static ILogService Log;

        public static NavigationServiceOptions Configuration { get; set; } =
            new NavigationServiceOptions();


        protected IList<NavigationStackEntry> BackStack { get; set; }

        protected IList<WeakNavigationStackEntry> ForwardStack { get; set; }



        private NavigationStackEntry root;

        protected NavigationStackEntry Root
        {
            get => root;
            set
            {
                root = value;
                RaisePropertyChanged();

                RaisePropertyChanged(nameof(CurrentViewModel));
                RaisePropertyChanged(nameof(CurrentContent));
            }
        }

        private NavigationStackEntry current;

        protected NavigationStackEntry Current
        {
            get => current;
            set
            {
                DetachDisappearingEventListener(
                    Current?.Content);


                current = value;
                RaisePropertyChanged();

                RaisePropertyChanged(nameof(CurrentViewModel));
                RaisePropertyChanged(nameof(CurrentContent));


                if (Current.ViewModel is IHandleClosing)
                {
                    Current.Content.Unloaded += OnContentUnloaded;
                }
            }
        }


        public IViewModel RootViewModel => Root?.ViewModel;

        public IViewModel CurrentViewModel => Current?.ViewModel;


        public bool CanGoBack => BackStack?.Any() == true;

        public bool CanGoForward => IsForwardNavigationPossible();


        public FrameworkElement CurrentContent => Current?.Content;



        public IReadOnlyCollection<INavigationRegistration> Registrations =>
            new ReadOnlyCollection<INavigationRegistration>(NavigationRegistrations);



        public NavigationService(
            IDependencyContainer resolver)
        {
            BackStack = new List<NavigationStackEntry>();
            ForwardStack = new List<WeakNavigationStackEntry>();

            SetResolverInstance(
                resolver);
        }


        public async Task SetRootWindowAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            var viewModel = await InitializeViewModelInternal<TViewModel>();

            var window = CreateContentInternal<TViewModel, Window>(
                viewModel);


            Application.Current.MainWindow = window;
            Application.Current.MainWindow.Show();
        }

        public async Task SetRootWindowAsync<TRootViewModel, TInitialViewModel>()

            where TRootViewModel : class, IViewModel
            where TInitialViewModel : class, IViewModel
        {
            await SetRootWindowAsync<TRootViewModel>();

            await SetRootAsync<TInitialViewModel>();

            ClearStacks();
        }


        public async Task SetRootAsync<TViewModel>()

            where TViewModel : class, IViewModel
        {
            await SetRootInternal<TViewModel, FrameworkElement>();
        }


        public void ClearStacks()
        {
            ClearBackStack();
            ClearForwardStack();
        }

        public void ClearBackStack()
        {
            BackStack.Clear();
        }

        public void ClearForwardStack()
        {
            ForwardStack.Clear();
        }


        internal async Task<TContent> SetRootInternal<TViewModel, TContent>()

            where TViewModel : class, IViewModel
            where TContent : FrameworkElement
        {
            ClearStacks();


            var viewModel = await InitializeViewModel<TViewModel>();
            var content = CreateContent<TViewModel, TContent>(
                viewModel);



            Root = new NavigationStackEntry(
                viewModel,
                content);



            SetCurrent(
                viewModel,
                content);


            return content;
        }

        protected void RaisePropertyChanged(
            [CallerMemberName] string propertyName = "")
        {
            var threadSafeCall = PropertyChanged;

            threadSafeCall?.Invoke(
                this,
                new PropertyChangedEventArgs(
                    propertyName));
        }



        private bool IsForwardNavigationPossible()
        {
            var target = ForwardStack.LastOrDefault();

            if (target == null)
                return false;


            return target.Content?.TryGetTarget(out _) == true &&
                target.ViewModel?.TryGetTarget(out _) == true;
        }


        private void SetResolverInstance(
            IDependencyContainer resolver)
        {
            if (dependencyResolver != null)
            {
                return;
            }


            _semaphore.Wait();

            if (dependencyResolver == null)
            {
                dependencyResolver = resolver;
            }

            _semaphore.Release();
        }


        #region View Disappearing event

        private async void OnContentUnloaded(
            object sender,
            EventArgs eventArgs)
        {
            if (!(sender is FrameworkElement content))
            {
                return;
            }

            if (Registrations == null ||
                !Registrations.Any(
                    registration => registration.ViewType == content.GetType()))
            {
                DetachDisappearingEventListener(content);
                return;
            }


            if (!(content.DataContext is IHandleClosing viewModel))
            {
                DetachDisappearingEventListener(content);
                return;
            }

            await viewModel.OnClosing();

            DetachDisappearingEventListener(content);
        }


        private void DetachDisappearingEventListener(
            FrameworkElement content)
        {
            if (content == null)
            {
                return;
            }

            content.Unloaded -= OnContentUnloaded;
        }

        #endregion View Disappearing event



        public static void SetupLogging(
            ILogService logService)
        {
            Log = logService;
        }
    }
}