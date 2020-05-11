﻿using CodeMonkeys.Logging;
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
        IViewModelNavigationService
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

            var viewModel = await ResolveViewModelInstance<TViewModel>();
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
            var viewModel = await ResolveViewModelInstance<TViewModel, TData>(
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


        protected async Task<TViewModel> ResolveViewModelInstance<TViewModel>()

            where TViewModel : class, IViewModel
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModel>();
            await viewModelInstance.InitializeAsync();

            LogService?.Info(
                $"ViewModel viewModel of type {typeof(TViewModel).Name} has been created and initialized!");

            return viewModelInstance;
        }

        protected async Task<TViewModel> ResolveViewModelInstance<TViewModel, TData>(
            TData model)

            where TViewModel : class, IViewModel<TData>
        {
            var viewModelInstance = dependencyResolver.Resolve<TViewModel>();
            await viewModelInstance.InitializeAsync(
                model);

            LogService?.Info(
                $"ViewModel viewModel of type {typeof(TViewModel).Name} has been created and initialized with parameters!");

            return viewModelInstance;
        }


        protected FrameworkElement CreateContent<TViewModel>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
        {
            return CreateContent<TViewModel, FrameworkElement>(
                viewModel);
        }

        protected TView CreateContent<TViewModel, TView>(
            TViewModel viewModel)

            where TViewModel : class, IViewModel
            where TView : FrameworkElement
        {
            if (!TryGetRegistration(
                typeof(TViewModel),
                typeof(TView),
                out var registrationInfo))
            {
                throw new InvalidOperationException(
                    $"No registration found for ViewModel of type {typeof(TViewModel).FullName}!");
            }

            if (ContentCache.All(cachedPage => cachedPage.Type != registrationInfo.ViewType))
            {
                CreateCachedContent(typeof(TView));
            }

            var reference = ContentCache
                .First(cachedPage => cachedPage.Type == registrationInfo.ViewType)
                .Reference;


            if (!reference.TryGetTarget(out var content))
            {
                var instance = Activator.CreateInstance<TView>();

                content = instance;
                reference.SetTarget(instance);
            }

            content.DataContext = viewModel;


            if (viewModel is IHandleClosing)
            {
                content.Unloaded += OnContentUnloaded;
            }


            LogService?.Info(
                $"View of type {content.GetType().Name} has been created!");

            return (TView)content;
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