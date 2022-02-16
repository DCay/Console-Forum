using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleForum.App.Core
{
    public interface ICommandManager
    {
        void Reset();

        void HandleInput();
    }
}
