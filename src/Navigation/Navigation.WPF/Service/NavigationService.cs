using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.MVVM;
using CodeMonkeys.Navigation.ViewModels;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CodeMonkeys.Navigation.WPF
{
    public partial class NavigationService :
        DependencyObject,

        IViewModelNavigationService
    {
        public event EventHandler<IViewModel> CurrentViewModelChanged;


        private static readonly SemaphoreSlim _semaphore =
            new SemaphoreSlim(1, 1);

        private static IDependencyResolver dependencyResolver;
        protected static ILogService LogService;

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

        public FrameworkElement CurrentContent
        {
            get => (FrameworkElement)GetValue(CurrentContentProperty);
            protected set => SetValue(CurrentContentProperty, value);
        }



        public IReadOnlyCollection<INavigationRegistration> Registrations =>
            new ReadOnlyCollection<INavigationRegistration>(NavigationRegistrations);


        public NavigationService(
            IDependencyResolver resolver)
        {
            SetResolverInstance(
                resolver);
        }


        public async Task SetRoot<TViewModel>()

            where TViewModel : class, IViewModel
        {
            var viewModel = await ResolveViewModelInstance<TViewModel>();


            var content = CreateContent<TViewModel, Window>(
                viewModel);


            Application.Current.MainWindow = content;
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
            LogService = logService;
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