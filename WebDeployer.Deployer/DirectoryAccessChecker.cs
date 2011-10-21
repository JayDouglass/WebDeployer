using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;

namespace WebDeployer.Deployer
{
    public class DirectoryAccessChecker
    {
        /// <summary>
        /// Checks the ability to create and write to a file in the supplied directory.
        /// </summary>
        /// <param name="directory">String representing the directory path to check.</param>
        /// <returns>True if successful; otherwise false.</returns>
        public static bool CheckDirectoryAccess(string directory)
        {
            bool success = false;
            string fullPath = directory + "access-checker-temp-file";

            if (Directory.Exists(directory))
            {
                try
                {
                    // Attempt to create a temporary file.
                    using (FileStream fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write))
                    {
                        fs.WriteByte(0xff);
                    }

                    // Delete temporary file if it was successfully created.
                    if (File.Exists(fullPath))
                    {
                        File.Delete(fullPath);
                        success = true;
                    }
                }
                catch (SecurityException)
                {
                    success = false;
                }
            }

            return success;
        }
    }
}




