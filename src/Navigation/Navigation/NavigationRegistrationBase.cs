using System;

namespace CodeMonkeys.Navigation
{
    public abstract class NavigationRegistrationBase :
        INavigationRegistration
    {
        public virtual Type ViewModelType { get; protected set; }
        public virtual Type ViewType { get; protected set; }


        /// <inheritdoc cref="INavigationRegistration.ResolveViewUsingDependencyInjection" />
        public virtual bool ResolveViewUsingDependencyInjection { get; set; }
        public virtual bool PreCreateInstance { get; set; }



        private Func<bool> condition = () => true;
        public virtual Func<bool> Condition
        {
            get => condition;
            set
            {
                condition = value ?? (() => true);
            }
        }
    }
}