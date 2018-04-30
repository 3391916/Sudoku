using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuApp.Core.Tests.Helpers
{
    public class FileNameResolver
    {
        private static string _appPath;

        public static string ResolveFileName(string relativePathToFile)
        {
            if (_appPath == null)
            {
                _appPath = Path.GetDirectoryName(typeof(FileNameResolver).Assembly.Location);
            }

            return Path.Combine(_appPath, relativePathToFile);
        }
    }
}
