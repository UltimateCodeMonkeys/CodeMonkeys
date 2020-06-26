using CodeMonkeys.Navigation;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CodeMonkeys.MVVM.ViewModels.Navigation
{
    internal static class ViewModelBaseHelper
    {
        internal static IEnumerable<MethodInfo> closeAsyncMethodInfos;


        internal static MethodInfo GetCloseViewModelAsyncMethodInfo(
            this ViewModelBase viewModelBase,
            int parametersCount = 1,
            int genericArgumentsCount = 2)
        {
            var closeAsyncTask = GetCloseAsyncMethodInfos(viewModelBase)
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
            var closeAsyncTask = GetCloseAsyncMethodInfos(viewModelBase)
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
            var closeAsyncTask = GetCloseAsyncMethodInfos(viewModelBase)
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

        private static IEnumerable<MethodInfo> GetCloseAsyncMethodInfos(
            ViewModelBase viewModel)
        {
            if (closeAsyncMethodInfos == null || !closeAsyncMethodInfos.Any())
            {
                closeAsyncMethodInfos = viewModel.NavigationService.Value
                    .GetType()
                    .GetMethods()
                    .Where(m => m.Name == nameof(INavigationService.CloseAsync));
            }

            return closeAsyncMethodInfos;                
        }
    }
}