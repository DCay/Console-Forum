using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleForum.App.IO
{
    public interface IOutputWriter
    {
        void Write(object output);

        void Write(object output, params object[] parameters);

        void WriteLine(object output);

        void WriteLine(object output, params object[] parameters);

        void ErrorLine(object output, params object[] parameters);

        void SuccessLine(object output, params object[] parameters);

        void ImportantLine(object output, params object[] parameters);
    }
}
