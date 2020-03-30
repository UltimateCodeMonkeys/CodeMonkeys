using System;
using System.Collections.Generic;

namespace CodeMonkeys.MVVM.PropertyChanged
{
    internal class PropertyDependencyWrapper
    {
        internal Type Class { get; set; }
        internal string PropertyName { get; set; }

        internal IList<string> Dependencies { get; }


        internal PropertyDependencyWrapper()
        {
            Dependencies = new List<string>();
        }

        internal PropertyDependencyWrapper(
            Type classType,
            string propertyName,
            string dependencyName)
            : this()
        {
            Class = classType;
            PropertyName = propertyName;

            Dependencies.Add(dependencyName);
        }


        internal bool TryAdd(
            string propertyName)
        {
            if (Dependencies.Contains(
                propertyName))
            {
                return false;
            }

            Dependencies.Add(propertyName);

            return true;
        }
    }
}