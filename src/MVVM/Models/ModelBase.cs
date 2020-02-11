using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

using CodeMonkeys.MVVM.Attributes;
using CodeMonkeys.MVVM.PropertyChanged;

namespace CodeMonkeys.MVVM.Models
{
    public abstract class ModelBase :
        BindingBase
    {
        private readonly ConcurrentDictionary<string, object> _comittedPropertyValues =
            new ConcurrentDictionary<string, object>();

        private readonly IEnumerable<string> _dontAffectIsDirtyDecoratedProperties;

        /// <summary>
        /// States whether a model contains uncommitted changes
        /// </summary>
        private bool dirtyFlag;
        public bool IsDirty
        {
            get => dirtyFlag;
            private set
            {
                dirtyFlag = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Contains the name of the property that has been modified the latest and set IsDirty to true
        /// </summary>
        private string lastPropertySetName = string.Empty;
        public string LastPropertySet
        {
            get => lastPropertySetName;
            private set
            {
                lastPropertySetName = value;
                RaisePropertyChanged();
            }
        }


        protected ModelBase()
        {
            _dontAffectIsDirtyDecoratedProperties =
                GetPropertiesDecoratedWith<DontAffectIsDirty>()
                .Select(property => property.Name);
        }


        /// <summary>
        /// Sets the property value and raises the PropertyChanged event
        /// Automatically attached an event listener for nested PropertyChanged events
        /// </summary>
        /// <typeparam name="TProperty">Type of the value</typeparam>
        /// <param name="value">Value to set</param>
        /// <param name="onPropertyChanged">EventHandler to invoke when this property value has changed</param>
        /// <param name="onPropertyChanging">EventHandler to invoke when this property value is changing</param>
        /// <param name="propertyName">Name of the property</param>
        protected new void SetValue<TProperty>(
            TProperty value,
            PropertyChangedEventHandler onPropertyChanged = null,
            PropertyChangingEventHandler onPropertyChanging = null,
            [CallerMemberName]string propertyName = "")
        {
            bool successfullySet = base.SetValue(
                value,
                onPropertyChanged,
                onPropertyChanging,
                propertyName);

            if (!successfullySet)
                return;


            if (!_comittedPropertyValues.ContainsKey(
                    propertyName))
            {
                _comittedPropertyValues.TryAdd(
                    propertyName,
                    value);
            }


            if (_dontAffectIsDirtyDecoratedProperties.Contains(
                propertyName))
                return;

            IsDirty = true;
            LastPropertySet = propertyName;
        }


        /// <summary>
        /// Set the current model state as committed and resets the IsDirty flag after
        /// IMPORTANT: This does not persist any data!
        /// </summary>
        /// <returns>bool indicating whether commitment has been successful</returns>
        public virtual bool CommitChanges()
        {
            try
            {
                if (!IsDirty)
                    return true;


                foreach (var propertyName in _comittedPropertyValues.Keys.ToList())
                {
                    _comittedPropertyValues[propertyName] =
                        GetValue<object>(propertyName);
                }

                IsDirty = false;
                LastPropertySet = string.Empty;
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Resets the model to the last committed state.
        /// IsDirty and LastPropertySet will be reset as well.
        /// </summary>
        protected virtual void ResetChanges()
        {
            if (!IsDirty)
                return;

            foreach (var property in _comittedPropertyValues)
            {
                SetValue(
                    property.Value,
                    null,
                    null,
                    property.Key);
            }

            ClearIsDirtyFlag();
        }

        /// <summary>
        /// Resets the IsDirty flag and LastPropertySet to default state.
        /// </summary>
        protected void ClearIsDirtyFlag()
        {
            IsDirty = false;
            LastPropertySet = string.Empty;
        }

        /// <summary>
        /// Looks up all properties in the class that have the specified attribute attached
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute to look for</typeparam>
        /// <returns>List of properties that have the given attribute attached</returns>
        protected IEnumerable<PropertyInfo> GetPropertiesDecoratedWith<TAttribute>()
            where TAttribute : Attribute
        {
            return GetPropertiesDecoratedWith<TAttribute>(
                GetType());
        }
    }
}