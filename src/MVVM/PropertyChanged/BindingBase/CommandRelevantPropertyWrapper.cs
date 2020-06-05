using System;

namespace CodeMonkeys.MVVM.PropertyChanged
{
    public class CommandRelevantPropertyWrapper
    {
        public Type Class { get; set; }
        public string PropertyName { get; set; }

        public string CommandName { get; set; }
    }
}