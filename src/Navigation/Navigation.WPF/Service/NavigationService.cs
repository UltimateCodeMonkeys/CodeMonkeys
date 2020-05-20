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

        INavigationService,
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;



        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);


        private static IDependencyResolver dependencyResolver;
        protected static ILogService Log;

        public static Configuration Configuration { get; set; } =
            new Configuration();


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
                current = value;
                RaisePropertyChanged();

                RaisePropertyChanged(nameof(CurrentViewModel));
                RaisePropertyChanged(nameof(CurrentContent));
            }
        }


        public IViewModel CurrentViewModel => Current?.ViewModel;

        public FrameworkElement CurrentContent => Current?.Content;



        public IReadOnlyCollection<INavigationRegistration> Registrations =>
            new ReadOnlyCollection<INavigationRegistration>(NavigationRegistrations);



        public NavigationService(
            IDependencyResolver resolver)
        {
            BackStack = new List<NavigationStackEntry>();
            ForwardStack = new List<WeakNavigationStackEntry>();

            SetResolverInstance(
                resolver);
        }


        public async Task SetRoot<TViewModel>()

            where TViewModel : class, IViewModel
        {
            await SetRootInternal<TViewModel, FrameworkElement>();
        }


        public bool CanGoBack()
        {
            return BackStack.Any();
        }

        public bool CanGoForward()
        {
            var target = ForwardStack.LastOrDefault();

            if (target == null)
                return false;


            return target.Content?.TryGetTarget(out _) == true &&
                target.ViewModel?.TryGetTarget(out _) == true;
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


        private void SetResolverInstance(
            IDependencyResolver resolver)
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


        private void RaisePropertyChanged(
            [CallerMemberName]string propertyName = "")
        {
            var threadSafeCall = PropertyChanged;

            threadSafeCall?.Invoke(
                this,
                new PropertyChangedEventArgs(
                    propertyName));
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
        #endregion



        public static void SetupLogging(
            ILogService logService)
        {
            Log = logService;
        }
    }
}