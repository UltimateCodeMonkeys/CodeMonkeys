using CodeMonkeys.Logging;
using CodeMonkeys.MVVM.PropertyChanged.Events;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace CodeMonkeys.MVVM.PropertyChanged
{
    public partial class BindingBase :
        INotifyPropertyChanged
    {
        private static ILogService _logService;

        private static readonly Lazy<IList<PropertyDependencyWrapper>> propertyDependencies =
            new Lazy<IList<PropertyDependencyWrapper>>(
                () => new List<PropertyDependencyWrapper>(),
                isThreadSafe: true);


        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<AdvancedPropertyChangingEventArgs> PropertyChanging;



        private readonly PropertyBag _propertyBag =
            new PropertyBag();

        private readonly ConcurrentDictionary<object, string> _nestedProperties =
            new ConcurrentDictionary<object, string>();


        private IEnumerable<PropertyInfo> commandProperties =
            new List<PropertyInfo>();



        protected BindingBase()
        {
            TrySetupPropertyDependencies();
        }


        private void GetLogServiceInstance()
        {
            var logServiceFields = GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic)
                .Where(field => field.FieldType == typeof(ILogService));

            if(!logServiceFields.Any())
                throw new InvalidEnumArgumentException();

            var logService = (ILogService)logServiceFields
                .First()
                .GetValue(
                    this);

            if(logService == null)
                throw new InvalidEnumArgumentException();

            _logService = logService;
        }


        /// <summary>
        /// Returns all properties that have a given attribute attached
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute to look for</typeparam>
        /// <param name="classType">The class to look for properties in</param>
        /// <returns>Collection of properties that got the given attribute</returns>
        protected static IEnumerable<PropertyInfo> GetPropertiesDecoratedWith<TAttribute>(
            Type classType)
            where TAttribute : Attribute
        {
            return classType
                .GetProperties()
                .Where(
                    p => p.IsDefined(typeof(TAttribute), true));
        }
    }
}