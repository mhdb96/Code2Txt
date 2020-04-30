using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Text_Extractor
{
    public class Processors
    {

        public DirectoryInfo ProcessDirectory(string targetDirectory, int level)
        {
            DirectoryInfo di = new DirectoryInfo(level);
            di.Path = new PathInfo(targetDirectory);
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                FileInfo fi = ProcessFile(fileName);
                di.Files.Add(fi);
            }
               
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                DirectoryInfo subDi = ProcessDirectory(subdirectory, level+1);
                di.Directories.Add(subDi);
            }
                
            return di;
        }

        public FileInfo ProcessFile(string path)
        {
            FileInfo fi = new FileInfo();
            fi.Path = new PathInfo(path);
            fi.Extension = "." + fi.Path.Folders[fi.Path.Folders.Count - 1].Split('.').LastOrDefault();
            string name = fi.Path.Folders[fi.Path.Folders.Count - 1].Replace(fi.Extension, "");
            fi.Name = name == "." ? "" : name; 
            return fi;
        }

        public IgnoreInfo ProcessIgnoreFile(string path)
        {
            IgnoreInfo ii = new IgnoreInfo();
            string[] allLines = File.ReadAllLines(path);            
            Regex reg = new Regex(@"\.git");
            ii.DirectoriesToIgnore.Add(reg);
            foreach (string line in allLines)
            {
                if (line == "" || line[0] == '#')
                    continue;
                else if(line[line.Length-1] == '/')
                {
                    string l = line.Replace("/", "");
                    l = l.Replace(".", "\\.");
                    l = l.Replace("*", ".*");
                    Regex re = new Regex(l);                    
                    ii.DirectoriesToIgnore.Add(re);
                }
                else if(!line.Contains('/'))
                {
                    string l = line;
                    l = l.Replace(".", "\\.");
                    l = l.Replace("*", ".*");
                    Regex re = new Regex(l);                    
                    ii.FilesToIgnore.Add(re);
                }
                else
                {
                    string l = line;
                    l = l.Replace(".", "\\.");
                    l = l.Replace("*", ".*");
                    Regex re = new Regex(l);
                    ii.ToIgnore.Add(re);
                }
            }
            return ii;
        }
    }
}
