using System.Linq;
using System.Reflection;

using CodeMonkeys.Core.Interfaces.MVVM;
using CodeMonkeys.Core.Interfaces.Navigation;

namespace CodeMonkeys.MVVM.ViewModels.Navigation
{
    internal static class ViewModelBaseHelper
    {
        internal static MethodInfo GetCloseViewModelAsyncMethodInfo(
            this ViewModelBase viewModelBase,
            int parametersCount = 1,
            int genericArgumentsCount = 2)
        {
            var closeAsyncTask = viewModelBase.NavigationService.Value
                .GetType()
                .GetMethods()
                .Where(m => m.Name == nameof(IViewModelNavigationService.CloseAsync))
                .Select(aiMethodInfo => new
                {
                    Method = aiMethodInfo,
                    Parameters = aiMethodInfo.GetParameters(),
                    GenericArguments = aiMethodInfo.GetGenericArguments()
                })
                .Where(aiInfoGroup =>
                    aiInfoGroup.Parameters.Length == parametersCount &&
                    aiInfoGroup.GenericArguments.Length == genericArgumentsCount)
                .Select(aiInfoGroup => aiInfoGroup.Method)
                .First();

            return closeAsyncTask;
        }

        internal static MethodInfo GetCloseViewModelAsyncMethodInfo<TInterface>(
            this ViewModelBase<TInterface> viewModelBase,
            int parametersCount = 1,
            int genericArgumentsCount = 2)
            where TInterface : IViewModel
        {
            var closeAsyncTask = viewModelBase.NavigationService.Value
                .GetType()
                .GetMethods()
                .Where(m => m.Name == nameof(IViewModelNavigationService.CloseAsync))
                .Select(aiMethodInfo => new
                {
                    Method = aiMethodInfo,
                    Parameters = aiMethodInfo.GetParameters(),
                    GenericArguments = aiMethodInfo.GetGenericArguments()
                })
                .Where(aiInfoGroup =>
                    aiInfoGroup.Parameters.Length == parametersCount &&
                    aiInfoGroup.GenericArguments.Length == genericArgumentsCount)
                .Select(aiInfoGroup => aiInfoGroup.Method)
                .First();

            return closeAsyncTask;
        }

        internal static MethodInfo GetCloseViewModelAsyncMethodInfo<TInterface, TModel>(
            this ViewModelBase<TInterface, TModel> viewModelBase,
            int parametersCount = 1,
            int genericArgumentsCount = 2)
            where TInterface : IViewModel<TModel>
        {
            var closeAsyncTask = viewModelBase.NavigationService.Value
                .GetType()
                .GetMethods()
                .Where(m => m.Name == nameof(IViewModelNavigationService.CloseAsync))
                .Select(aiMethodInfo => new
                {
                    Method = aiMethodInfo,
                    Parameters = aiMethodInfo.GetParameters(),
                    GenericArguments = aiMethodInfo.GetGenericArguments()
                })
                .Where(aiInfoGroup =>
                    aiInfoGroup.Parameters.Length == parametersCount &&
                    aiInfoGroup.GenericArguments.Length == genericArgumentsCount)
                .Select(aiInfoGroup => aiInfoGroup.Method)
                .First();

            return closeAsyncTask;
        }
    }
}