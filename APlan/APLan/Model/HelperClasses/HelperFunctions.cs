using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APLan.Model.HelperClasses
{
    public class HelperFunctions
    {

        /// <summary>
        /// given set of files concatenated in a string with separation +~+. copy them to specific path.
        /// </summary>
        /// <param name="filesString"></param>
        /// <param name="folderPath"></param>
        public static void copyFilesInString(string filesString, string folderPath)
        {
            var files = getFileNamesFromString(filesString);
            foreach (var item in files)
            {
                File.Copy(item, $"{folderPath}/{Path.GetFileName(item)}", true);
            }
        }

        /// <summary>
        /// separate file name which are concatenated with +~+ separation into list.
        /// </summary>
        /// <param name="filesString"></param>
        /// <returns></returns>
        public static List<string> getFileNamesFromString(string filesString)
        {
            var files = filesString.Split("+~+").ToList();
            files.RemoveAll(x => string.IsNullOrWhiteSpace(x));
            return files;
        }


    }
}
