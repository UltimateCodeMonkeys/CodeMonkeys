using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

using CodeMonkeys.MVVM.PropertyChanged;

namespace CodeMonkeys.MVVM.Models
{
    public abstract partial class ModelBase :
        BindingBase
    {
        /// <summary>
        /// Sets the property value and raises the PropertyChanged event
        /// Automatically attached an event listener for nested PropertyChanged events
        /// </summary>
        /// <typeparam name="TProperty">Type of the value</typeparam>
        /// <param name="value">Value to set</param>
        /// <param name="onPropertyChanged">EventHandler to invoke when this property value has changed</param>
        /// <param name="onPropertyChanging">EventHandler to invoke when this property value is changing</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Flag indicating wether the value has successfully been set</returns>
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
        /// Sets the property value and raises the PropertyChanged event
        /// Automatically attached an event listener for nested PropertyChanged events
        /// </summary>
        /// <typeparam name="TProperty">Type of the value</typeparam>
        /// <param name="value">Value to set</param>
        /// <param name="onPropertyChanged">Action to invoke when this property value has changed</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns>Flag indicating wether the value has successfully been set</returns>
        protected new void SetValue<TProperty>(
            TProperty value,
            Action<TProperty> onPropertyChanged,
            [CallerMemberName]string propertyName = "")
        {
            bool successfullySet = base.SetValue(
                value,
                onPropertyChanged,
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
    }
}