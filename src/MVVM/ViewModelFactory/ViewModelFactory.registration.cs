using System;
using System.Collections.Concurrent;
using System.Linq;

namespace CodeMonkeys.MVVM
{
    public static partial class ViewModelFactory
    {
        private static readonly ConcurrentBag<RegistrationInfo> _registrations =
            new ConcurrentBag<RegistrationInfo>();


        public static RegistrationInfo Register<TInterface, TImplementation>()

            where TInterface : class, IViewModel
            where TImplementation : class, TInterface, IViewModel
        {
            var registrationInfo = new RegistrationInfo(
                typeof(TInterface),
                typeof(TImplementation));

            RegisterInternal(
                registrationInfo);


            return registrationInfo;
        }

        public static RegistrationInfo Register<TImplementation>()

            where TImplementation : class, IViewModel
        {
            var registrationInfo = new RegistrationInfo(
                typeof(TImplementation));

            RegisterInternal(
                registrationInfo);


            return registrationInfo;
        }


        public static RegistrationInfo WithModel<TModel>(
            this RegistrationInfo registration)
        {
            Argument.NotNull(
                registration,
                nameof(registration));


            if (registration.Interface?.IsAssignableFrom(
                    typeof(IViewModel<TModel>)) != true &&
                registration.ViewModel?.IsAssignableFrom(
                    typeof(IViewModel<TModel>)) != true)
            {
                throw new InvalidOperationException(
                    $"Neither '{registration.Interface.Name}' nor '{registration.ViewModel.Name}' implement '{typeof(IViewModel<TModel>).Name}'!");
            }

            if (!_registrations.Contains(
                registration))
            {
                RegisterInternal(
                    registration);
            }


            registration.Model = typeof(TModel);


            return registration;
        }


        public static RegistrationInfo DontInitialize(
            this RegistrationInfo registration)
        {
            if (!_registrations.Contains(
                registration))
            {
                RegisterInternal(
                    registration);
            }


            registration.Initialize = false;


            return registration;
        }


        /// <summary>
        /// Try to get a registration with either Interface, ViewModel or Model type matching <c>T</c>
        /// </summary>
        /// <typeparam name="T">Interface, ViewModel or Model type of the registration</typeparam>
        /// <param name="registrationInfo"></param>
        /// <returns><c>true</c> if there was a matching registration, otherwise <c>false</c></returns>
        public static bool TryGetRegistration<T>(
            out RegistrationInfo registrationInfo)
        {
            registrationInfo = _registrations?.FirstOrDefault(registration =>
                registration.Interface == typeof(T) ||
                registration.ViewModel == typeof(T) ||
                registration.Model == typeof(T));


            return registrationInfo != null;
        }


        private static void RegisterInternal(
            RegistrationInfo registration)
        {
            _registrations.Add(registration);


            // dont know what happens in different dependency container implementations when both types are the same
            if (registration.Interface == registration.ViewModel)
            {
                container.RegisterType(
                    registration.Interface);
            }
            else
            {
                container.RegisterType(
                    registration.Interface,
                    registration.ViewModel);
            }
        }
    }
}