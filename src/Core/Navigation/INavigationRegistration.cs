using System;

namespace CodeMonkeys.Navigation
{
    public interface INavigationRegistration
    {
        Type ViewModelType { get; }
        Type ViewType { get; }


        /// <summary>
        /// <para>
        /// <see cref="CodeMonkeys.Navigation.INavigationService"/> uses the <see cref="CodeMonkeys.DependencyInjection.IDependencyContainer"/> instance to create the view if set to <value>true</value>.
        /// You need to register your view type in bootstrap using <see cref="CodeMonkeys.DependencyInjection.IDependencyRegister.RegisterType(Type)"/> or a similar method.
        /// </para>
        /// <para>
        /// <see cref="CodeMonkeys.Navigation.INavigationService"/> uses the <see cref="CodeMonkeys.Activator"/> to create the view if set to <value>false</value>.
        /// Eventual dependencies in your view won't get resolved.
        /// </para>
        /// <para>
        /// Defaults to <value>false</value>
        /// </para>
        /// </summary>
        bool ResolveViewUsingDependencyInjection { get; set; }

        bool PreCreateInstance { get; set; }


        Func<bool> Condition { get; set; }
    }
}