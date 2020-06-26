using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Text_Extractor
{
    public class PatternAnalyzer
    {

        DirectoryInfo DirectoryInfo;
        IgnoreInfo IgnoreInfo = new IgnoreInfo();
        Processors P = new Processors();
        List<FileInfo> Files = new List<FileInfo>();
        List<string> Extinsions;
        string DirPath;
        string IgnorePath;

        public PatternAnalyzer(string dirPath, string ignorePath = "", List<string> exts = null)
        {
            DirPath = dirPath;
            IgnorePath = ignorePath;
            Extinsions = exts;
        }

        public void Run()
        {
            GetDirectoryInfo();
            GetIgnoreInfo();
            FindFiles();            
            ExportFiles();
        }

        private void GetDirectoryInfo()
        {
            if (Directory.Exists(DirPath))
            {
                DirectoryInfo = P.ProcessDirectory(DirPath, 0);                
            }
            else
            {
                Console.WriteLine("{0} is not a valid file or directory.", DirPath);
            }
        }

        private void GetIgnoreInfo()
        {
            if (File.Exists(IgnorePath))
            {
                IgnoreInfo = P.ProcessIgnoreFile(IgnorePath);

            }
            else
            {
                Console.WriteLine("{0} is not a valid .gitignore file path.", IgnorePath);
            }
        }

        private void FindFiles()
        {
            AddFiles(DirectoryInfo);
        }

        private void AddFiles(DirectoryInfo dir)
        {
            bool toIgnore= false;
            foreach (DirectoryInfo di in dir.Directories)
            {
                toIgnore = false;
                foreach (Regex re in IgnoreInfo.DirectoriesToIgnore)
                {
                    if (re.IsMatch(di.Name))
                    {
                        toIgnore = true;
                        break;
                    }                        
                }
                if(!toIgnore)
                {
                    toIgnore = false;
                    foreach (Regex re in IgnoreInfo.ToIgnore)
                    {
                        if (re.IsMatch(di.Path.ToString()))
                        {
                            toIgnore = true;
                            break;
                        }
                    }
                    if(!toIgnore)
                        AddFiles(di);
                }
            }
            if (Extinsions != null)
            {
                foreach (FileInfo fi in dir.Files)
                {
                    if (Extinsions.Contains(fi.Extension))
                    {
                        Files.Add(fi);
                        Console.WriteLine(fi.Name + fi.Extension);
                    }
                }
            }
            else
            {
                foreach (FileInfo fi in dir.Files)
                {
                    toIgnore = false;
                    foreach (Regex re in IgnoreInfo.FilesToIgnore)
                    {
                        if (re.IsMatch(fi.Name + "." + fi.Extension))
                        {
                            toIgnore = true;
                            break;
                        }
                    }
                    if (!toIgnore)
                    {
                        toIgnore = false;
                        foreach (Regex re in IgnoreInfo.FilesToIgnore)
                        {
                            if (re.IsMatch(fi.Path.ToString() + fi.Name + "." + fi.Extension))
                            {
                                toIgnore = true;
                                break;
                            }
                        }
                        if (!toIgnore)
                        {
                            Files.Add(fi);
                            Console.WriteLine(fi.Name + fi.Extension);
                        }
                    }
                }
            }
        }

        private void ExportFiles()
        {
            if (Extinsions != null)
            {
                foreach (var file in Files)
                {
                    if (Extinsions.Contains(file.Extension))
                    {
                        string t = file.Path.ToString();
                        t = t.Remove(t.Length - 1, 1);
                        WriteToFile(t);
                    }
                }
            }
            else
            {
                foreach (var file in Files)
                {
                    string t = file.Path.ToString();
                    t = t.Remove(t.Length - 1, 1);
                    WriteToFile(t);
                }
            }
        }
        private void WriteToFile(string path)
        {
            string[] lines = File.ReadAllLines(path);
            File.AppendAllLines("allcode.txt", lines);
        }

    }
}
