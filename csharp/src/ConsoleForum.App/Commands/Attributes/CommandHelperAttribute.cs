namespace ConsoleForum.App.Commands.Attributes
{
    using System;

    public class CommandHelperAttribute : Attribute
    {
        public CommandHelperAttribute(string help)
        {
            this.Help = help;
        }

        public string Help { get; set; }
    }
}
