using CodeMonkeys.Core.Logging;

using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CodeMonkeys.MVVM.PropertyChanged
{
    public partial class BindingBase :
        INotifyPropertyChanged
    {
        /// <summary>
        /// Retrieves the property value
        /// </summary>
        /// <typeparam name="TProperty">Type of the property value</typeparam>
        /// <param name="propertyName">Name of the property (use nameof())</param>
        /// <returns>Current property value / default if no value has been set yet</returns>
        protected TProperty GetValue<TProperty>(
            [CallerMemberName]string propertyName = "")
        {
            return (TProperty)_properties.GetOrAdd(
                propertyName,
                default(TProperty));
        }

        /// <summary>
        /// Retrieves the property value
        /// </summary>
        /// <typeparam name="TProperty">Type of the property value</typeparam>
        /// <param name="defaultValue">The default value for the property</param>
        /// <param name="propertyName">Name of the property (use nameof())</param>
        /// <returns>Current property value / default if no value has been set yet</returns>
        protected TProperty GetValue<TProperty>(
            TProperty defaultValue,
            [CallerMemberName]string propertyName = "")
        {
            return (TProperty)_properties.GetOrAdd(
                propertyName,
                defaultValue);
        }

        /// <summary>
        /// Retrieves the property value
        /// Call is logged on Info-Level
        /// Property value is logged on Debug-Level
        /// </summary>
        /// <typeparam name="TProperty">Value type of the property</typeparam>
        /// <param name="propertyName">Name of the property to retrieves the value for</param>
        /// <exception cref="InvalidOperationException">If no field (static or instance) of type <see cref="ILogService"/> is defined</exception>
        protected TProperty GetValueAndLog<TProperty>(
            [CallerMemberName]string propertyName = "")
        {
            if (_logService == null)
                GetLogServiceInstance();

            _logService?.Trace(
                $"Getting value for property '{propertyName}'");

            var propertyValue = GetValue<TProperty>(
                propertyName);

            _logService?.Debug(
                $"Value for property {propertyName}: '{propertyValue}'");

            return propertyValue;
        }
    }
}