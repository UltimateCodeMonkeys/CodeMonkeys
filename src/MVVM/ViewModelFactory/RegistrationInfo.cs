using System;

namespace CodeMonkeys.MVVM
{
    public class RegistrationInfo
    {
        public Type ViewModel { get; internal set; }
        public Type Interface { get; internal set; }

        public Type Model { get; internal set; }


        public bool Initialize { get; internal set; } = true;


        public RegistrationInfo(
            Type viewModelType)
        {
            ViewModel = viewModelType;
        }

        public RegistrationInfo(
            Type interfaceType,
            Type viewModelType)
            : this(viewModelType)
        {
            Interface = interfaceType;
        }

        public RegistrationInfo(
            Type interfaceType,
            Type viewModelType,
            Type modelType)
            : this(interfaceType, viewModelType)
        {
            Model = modelType;
        }


        public override bool Equals(
            object @object)
        {
            if (!(@object is RegistrationInfo other))
            {
                return false;
            }


            if (ViewModel != other.ViewModel)
            {
                return false;
            }

            if (Interface != other.Interface)
            {
                return false;
            }

            if (Model != other.Model)
            {
                return false;
            }

            if (Initialize != other.Initialize)
            {
                return false;
            }


            return true;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(
                ViewModel,
                Interface,
                Model,
                Initialize);
        }
    }
}