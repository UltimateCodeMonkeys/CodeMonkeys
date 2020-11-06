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
                typeof(TImplementation),
                typeof(TInterface));

            _registrations.Add(registrationInfo);


            container.RegisterType<TInterface, TImplementation>();


            return registrationInfo;
        }

        public static RegistrationInfo Register<TImplementation>()

            where TImplementation : class, IViewModel
        {
            var registrationInfo = new RegistrationInfo(
                typeof(TImplementation));

            _registrations.Add(registrationInfo);


            container.RegisterType<TImplementation>();


            return registrationInfo;
        }


        public static RegistrationInfo WithModel<TModel>(
            this RegistrationInfo registrationInfo)
        {
            Argument.NotNull(
                registrationInfo,
                nameof(registrationInfo));


            if (registrationInfo.Interface?.IsAssignableFrom(
                    typeof(IViewModel<TModel>)) != true &&
                registrationInfo.ViewModel?.IsAssignableFrom(
                    typeof(IViewModel<TModel>)) != true)
            {
                throw new InvalidOperationException();
            }

            if (!_registrations.Contains(registrationInfo))
            {
                throw new InvalidOperationException();
            }


            registrationInfo.Model = typeof(TModel);


            return registrationInfo;
        }


        public static RegistrationInfo DontInitialize(
            this RegistrationInfo registrationInfo)
        {
            if (!_registrations.Contains(registrationInfo))
            {
                throw new InvalidOperationException();
            }


            registrationInfo.Initialize = false;


            return registrationInfo;
        }
    }
}