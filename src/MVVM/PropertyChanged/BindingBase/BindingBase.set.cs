using CodeMonkeys.Logging;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CodeMonkeys.MVVM.PropertyChanged
{
    public partial class BindingBase :
        INotifyPropertyChanged
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
        protected bool SetValue<TProperty>(
            TProperty value,
            PropertyChangedEventHandler onPropertyChanged = null,
            PropertyChangingEventHandler onPropertyChanging = null,
            [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<TProperty>.Default.Equals(
                value,
                GetValue<TProperty>(propertyName)))
                return false;

            var propertyChangingEventArgs = BuildPropertyChangingEventArgs(
                propertyName,
                value);

            bool shouldContinue = RaisePropertyChanging(
                propertyChangingEventArgs,
                onPropertyChanging);

            if (!shouldContinue)
            {
                return false;
            }


            RemoveEventualNestedEventListeners(
                value);

            bool success = _propertyBag.SetValue(
                value,
                propertyName);

            AppendNestedEventListeners(
                value,
                propertyName);


            onPropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));

            RaisePropertyChanged(
                propertyName);

            return success;
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
        protected bool SetValue<TProperty>(
            TProperty value,
            Action<TProperty> onPropertyChanged,
            [CallerMemberName]string propertyName = "")
        {
            if (EqualityComparer<TProperty>.Default.Equals(
                value,
                GetValue<TProperty>(propertyName)))
                return false;

            var propertyChangingEventArgs = BuildPropertyChangingEventArgs(
                propertyName,
                value);

            bool shouldContinue = RaisePropertyChanging(
                propertyChangingEventArgs);

            if (!shouldContinue)
                return false;


            RemoveEventualNestedEventListeners(
                value);

            bool success = _propertyBag.SetValue(
                value,
                propertyName);

            AppendNestedEventListeners(
                value,
                propertyName);



            onPropertyChanged?.Invoke(
                value);

            RaisePropertyChanged(propertyName);

            return success;
        }

        protected void SetValueAndLog<TProperty>(
            TProperty value,
            ILogService logService,
            PropertyChangedEventHandler onPropertyChanged = null,
            PropertyChangingEventHandler onPropertyChanging = null,
            [CallerMemberName] string propertyName = "")
        {
            logService?.Trace(
                $"Setting value for property {propertyName}.");


            SetValue(
                value,
                onPropertyChanged,
                onPropertyChanging,
                propertyName);


            logService?.Debug(
                $"Set value for property {propertyName} to '{value}'");
        }
    }
}