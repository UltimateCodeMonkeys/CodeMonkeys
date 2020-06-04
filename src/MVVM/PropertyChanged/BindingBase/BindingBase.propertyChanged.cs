using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Input;

using CodeMonkeys.Logging;
using CodeMonkeys.MVVM.Attributes;
using CodeMonkeys.MVVM.PropertyChanged.Events;

namespace CodeMonkeys.MVVM.PropertyChanged
{
    public partial class BindingBase :
        INotifyPropertyChanged
    {
        private void TrySetupPropertyDependencies()
        {
            var classType = GetType();

            if (propertyDependencies.Value.Any(
                dependencyRegistration => dependencyRegistration.Class == classType))
            {
                return;
            }


            var properties = GetPropertiesDecoratedWith<DependsOn>(
                classType);

            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes<DependsOn>();

                if (attributes == null ||
                    !attributes.Any())
                {
                    continue;
                }

                var dependencyRegistration = new PropertyDependencyWrapper
                {
                    Class = classType,
                    PropertyName = property.Name
                };

                foreach (var dependency in attributes)
                {
                    dependencyRegistration.TryAdd(
                        dependency.PropertyName);
                }

                propertyDependencies.Value.Add(
                    dependencyRegistration);
            }
        }

        private AdvancedPropertyChangingEventArgs BuildPropertyChangingEventArgs<TProperty>(
            string propertyName,
            TProperty newValue)
        {
            TProperty oldValue = GetValue<TProperty>(propertyName);

            var eventArgs = new AdvancedPropertyChangingEventArgs<TProperty>(
                propertyName,
                newValue,
                oldValue);

            return eventArgs;
        }

        private bool RaisePropertyChanging(
            AdvancedPropertyChangingEventArgs eventArgs,
            PropertyChangingEventHandler onPropertyChanging = null)
        {
            onPropertyChanging?
                .Invoke(
                    this,
                    eventArgs);

            var threadSafeCall = PropertyChanging;

            threadSafeCall?.Invoke(
                this,
                eventArgs);

            if (eventArgs.Cancel)
                return false;

            return true;
        }


        private void RemoveEventualNestedEventListeners<TProperty>(
            TProperty value)
        {
            if (!(value is INotifyPropertyChanged instance))
                return;

            instance.PropertyChanged -= OnNestedPropertyChanged;

            if (_nestedProperties.ContainsKey(
                instance))
            {
                _nestedProperties.TryRemove(
                    instance,
                    out _);
            }
        }

        private void AppendNestedEventListeners<TProperty>(
            TProperty value,
            string propertyName)
        {
            if (!(value is INotifyPropertyChanged instance))
                return;


            instance.PropertyChanged += OnNestedPropertyChanged;

            if (!_nestedProperties.ContainsKey(
                instance))
            {
                _nestedProperties.TryAdd(
                    instance,
                    propertyName);
            }
        }


        private void OnNestedPropertyChanged(
            object sender,
            PropertyChangedEventArgs eventArgs)
        {
            if (!_nestedProperties.ContainsKey(
                sender))
                return;

            RaisePropertyChanged(
                _nestedProperties[sender]);
        }


        /// <summary>
        /// Invoke the PropertyChanged event for the given property name
        /// </summary>
        /// <param name="propertyName"></param>
        protected virtual void RaisePropertyChanged(
            [CallerMemberName]string propertyName = "")
        {
            RaisePropertyChangedForDependentProperties(
                propertyName);


            var commandRelevance = commandRelevantProperties.Value
                .FirstOrDefault(relevance => relevance.PropertyName.Equals(propertyName));

            if (commandRelevance != null)
            {
                UpdateCommandsCanExecute(
                    commandRelevance);
            }


            if (ShallPropertyChangedNotificationBeSuppressed(
                propertyName))
                return;

            var threadSafeCall = PropertyChanged;

            threadSafeCall?.Invoke(
                this,
                new PropertyChangedEventArgs(
                    propertyName));
        }

        private void RaisePropertyChangedForDependentProperties(
            string propertyName)
        {
            var classType = GetType();

            var dependencyRegistration = propertyDependencies.Value
                .FirstOrDefault(dependency => dependency.Class == classType &&
                    dependency.Dependencies.Contains(propertyName));

            if (dependencyRegistration == null)
            {
                return;
            }


            var threadSaveCall = PropertyChanged;

            threadSaveCall?.Invoke(
                this,
                new PropertyChangedEventArgs(
                    dependencyRegistration.PropertyName));
        }

        private IEnumerable<PropertyInfo> GetCommandProperties()
        {
            if (commandProperties.Any())
            {
                return commandProperties;
            }


            commandProperties = GetType().
                GetProperties(
                    BindingFlags.Instance |
                    BindingFlags.Public)
                .Where(
                    propertyInfo =>
                        propertyInfo.PropertyType is ICommand
                        || typeof(ICommand).IsAssignableFrom(propertyInfo.PropertyType));


            return commandProperties;
        }

        private void UpdateCommandsCanExecute(
            CommandRelevantPropertyWrapper commandRelevance)
        {
            if (commandRelevance == null)
            {
                return;
            }


            if (!string.IsNullOrWhiteSpace(
                commandRelevance.CommandName))
            {
                var commandProperty = GetCommandProperties()
                    .FirstOrDefault(
                        property => property.Name == commandRelevance.CommandName);


                if (commandProperty?.GetValue(this) is ICommand command)
                {
                    InvokeCommandCanExecuteChanged(
                        command);

                    return;
                }
            }


            foreach (var property in GetCommandProperties())
            {
                if (!(property.GetValue(this) is ICommand command))
                    continue;


                InvokeCommandCanExecuteChanged(
                    command);
            }
        }

        private void InvokeCommandCanExecuteChanged(
            ICommand command)
        {
            if (command == null)
            {
                return;
            }


            var eventDelegate = (MulticastDelegate)command
                .GetType()
                .GetField(
                    nameof(command.CanExecuteChanged),
                    BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(command);


            if (eventDelegate == null)
            {
                _log?.Error(
                    $"Unable to get {typeof(MulticastDelegate).Name} from {command}, cannot invoke {nameof(ICommand.CanExecuteChanged)}");

                return;
            }


            foreach (var eventHandler in eventDelegate.GetInvocationList())
            {
                eventHandler.Method
                    .Invoke(
                        eventHandler.Target,
                        new object[] { command, EventArgs.Empty });
            }
        }

        private bool ShallPropertyChangedNotificationBeSuppressed(
            string propertyName = "")
        {
            if (string.IsNullOrEmpty(propertyName))
                return true;

            var suppressForProperty = SuppressRaisingPropertyChangedEventForProperty(
                propertyName);

            if (SuppressRaisingPropertyChangedEventForClass())
            {
                if (suppressForProperty.HasValue)
                    return suppressForProperty.Value;

                return true;
            }

            return suppressForProperty.HasValue &&
                suppressForProperty.Value;
        }


        private bool SuppressRaisingPropertyChangedEventForClass()
        {
            var suppressNotifying = GetType()
                .GetCustomAttribute<SuppressNotifyPropertyChanged>();

            return suppressNotifying != null &&
                suppressNotifying.Suppress;
        }


        private bool? SuppressRaisingPropertyChangedEventForProperty(
            string propertyName)
        {
            var suppressNotifying = GetType()
                .GetRuntimeProperty(propertyName)
                ?.GetCustomAttribute<SuppressNotifyPropertyChanged>();

            return suppressNotifying?.Suppress;
        }
    }
}