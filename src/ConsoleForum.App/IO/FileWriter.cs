using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleForum.App.IO
{
    public class FileWriter : IOutputWriter
    {
        private readonly string pathToFile;

        public FileWriter(string pathToFile)
        {
            this.pathToFile = pathToFile;
        }

        public void Write(object output)
        {
            File.AppendAllText(this.pathToFile, output.ToString());
        }

        public void Write(object output, params object[] parameters)
        {
            File.AppendAllText(this.pathToFile, string.Format(output.ToString(), parameters));
        }

        public void WriteLine(object output)
        {
            File.AppendAllText(this.pathToFile, output.ToString() + Environment.NewLine);
        }

        public void WriteLine(object output, params object[] parameters)
        {
            File.AppendAllText(this.pathToFile, string.Format(output.ToString(), parameters) + Environment.NewLine);
        }

        public void ErrorLine(object output, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void SuccessLine(object output, params object[] parameters)
        {
            throw new NotImplementedException();
        }

        public void ImportantLine(object output, params object[] parameters)
        {
            throw new NotImplementedException();
        }
    }
}
