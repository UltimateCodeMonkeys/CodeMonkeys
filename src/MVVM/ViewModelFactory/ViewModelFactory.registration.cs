using CodeMonkeys.DependencyInjection;
using CodeMonkeys.Logging;
using CodeMonkeys.Navigation;

using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CodeMonkeys.MVVM
{
    public static partial class ViewModelFactory
    {
        private static ConcurrentDictionary<Type, Type> map;
    }
}