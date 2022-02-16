using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsoleForum.App.IO
{
    public class FileReader : IInputReader
    {
        private readonly string[] fileContent;

        private int currentLineIndex;

        public FileReader(string pathToFile)
        {
            this.fileContent = File.ReadAllLines(pathToFile);
            this.currentLineIndex = 0;
        }

        private string ReadNextLine()
        {
            if (this.currentLineIndex < this.fileContent.Length)
            {
                return this.fileContent[this.currentLineIndex++];
            }

            return null;
        }

        public string ReadLine()
        {
            return this.ReadNextLine();
        }
    }
}
