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
        DependencyObject,

        INavigationService,
        INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<IViewModel> CurrentViewModelChanged;


        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);

        private static IDependencyResolver dependencyResolver;
        protected static ILogService Log;

        public static Configuration Configuration { get; set; } =
            new Configuration();


        protected IList<NavigationStackEntry> BackStack { get; set; }
        protected IList<NavigationStackEntry> ForwardStack { get; set; }



        public static readonly DependencyProperty CurrentViewModelProperty =
            DependencyProperty.Register(
                nameof(CurrentViewModel),
                typeof(IViewModel),
                typeof(NavigationService),
                new PropertyMetadata(OnCurrentViewModelChanged));

        public IViewModel CurrentViewModel
        {
            get => (IViewModel)GetValue(CurrentViewModelProperty);
            protected set => SetValue(CurrentViewModelProperty, value);
        }



        public static readonly DependencyProperty CurrentContentProperty =
            DependencyProperty.Register(
                nameof(CurrentContent),
                typeof(FrameworkElement),
                typeof(NavigationService));

        private FrameworkElement currentContent;
        public FrameworkElement CurrentContent
        {
            //get => (FrameworkElement)GetValue(CurrentContentProperty);
            //protected set => SetValue(CurrentContentProperty, value);
            get => currentContent;
            set
            {
                currentContent = value;
                RaisePropertyChanged();
            }
        }



        public IReadOnlyCollection<INavigationRegistration> Registrations =>
            new ReadOnlyCollection<INavigationRegistration>(NavigationRegistrations);


        public NavigationService(
            IDependencyResolver resolver)
        {
            BackStack = new List<NavigationStackEntry>();
            ForwardStack = new List<NavigationStackEntry>();

            SetResolverInstance(
                resolver);
        }


        public async Task SetRoot<TViewModel>()

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


            CurrentViewModel = viewModel;
            CurrentContent = content;


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
                new PropertyChangedEventArgs(propertyName));
        }

        private void RaiseCurrentViewModelChanged()
        {
            var threadSafeCall = CurrentViewModelChanged;

            threadSafeCall?.Invoke(
                this,
                CurrentViewModel);
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


        private static void OnCurrentViewModelChanged(
            DependencyObject @object,
            DependencyPropertyChangedEventArgs eventArgs)
        {
            if (!(@object is NavigationService navigationService))
                return;


            navigationService.RaiseCurrentViewModelChanged();
        }
    }
}