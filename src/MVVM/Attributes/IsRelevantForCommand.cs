using System;

namespace CodeMonkeys.MVVM.Attributes
{
    /// <summary>
    /// States that a property affects an ICommand's CanExecute state and should force an update on PropertyChanged
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Property,
        Inherited = true)]
    public class IsRelevantForCommand :
        Attribute
    {
        public string CommandName { get; set; }


        public IsRelevantForCommand()
        {
        }

        public IsRelevantForCommand(
            string commandName)
        {
            CommandName = commandName;
        }
    }
}