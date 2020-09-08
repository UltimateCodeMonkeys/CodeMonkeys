using CodeMonkeys.MVVM.Attributes;
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
        private static readonly IList<PropertyDependencyWrapper> propertyDependencies =
            new List<PropertyDependencyWrapper>();

        private static readonly IList<CommandRelevantPropertyWrapper> commandRelevantProperties =
           new List<CommandRelevantPropertyWrapper>();


        public static BindingBaseOptions Options { get; set; } =
            new BindingBaseOptions();



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
            TrySetupCommandRelevantProperties();
        }


        private void TrySetupCommandRelevantProperties()
        {
            var classType = GetType();

            if (commandRelevantProperties.Any(
                relevance => relevance.Class == classType))
            {
                return;
            }


            var properties = GetPropertiesDecoratedWith<IsRelevantForCommand>(
                classType);

            foreach (var property in properties)
            {
                var attribute = property.GetCustomAttribute<IsRelevantForCommand>();

                if (attribute == null)
                {
                    continue;
                }

                var commandRelevance = new CommandRelevantPropertyWrapper
                {
                    Class = classType,
                    PropertyName = property.Name,
                    CommandName = attribute.CommandName
                };


                commandRelevantProperties.Add(
                    commandRelevance);
            }
        }


        /// <summary>
        /// Returns all properties that have a given attribute attached
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute to look for</typeparam>
        /// <param name="class">The class to look for properties in</param>
        /// <returns>Collection of properties that got the given attribute</returns>
        protected static IEnumerable<PropertyInfo> GetPropertiesDecoratedWith<TAttribute>(
            Type @class)
            where TAttribute : Attribute
        {
            return @class
                .GetProperties()
                .Where(property => property.IsDefined(typeof(TAttribute), true));
        }
    }
}