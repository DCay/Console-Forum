using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleForum.App.Commands.Attributes
{
    public class CommandExampleAttribute : Attribute
    {
        public CommandExampleAttribute(string example)
        {
            this.Example = example;
        }

        public string Example { get; set; }
    }
}
