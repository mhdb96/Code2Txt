using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Text_Extractor
{    
    public class FileInfo
    {
        public string Extension { get; set; }
        public string Name { get; set; }
        public PathInfo Path { get; set; }

        public override string ToString()
        {
            return Name + Extension;
        }
    }
    public class PathInfo
    {
        public List<string> Folders { get; set; } = new List<string>();
        public PathInfo(string path)
        {
            string[] folders = path.Split('\\');
            foreach (string folder in folders)
            {
                Folders.Add(folder);
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (string folder in Folders)
            {
                sb.Append(folder + "/");
            }
            return sb.ToString();
        }
    }
    public class DirectoryInfo
    {
        public DirectoryInfo(int level)
        {
            Level = level;
        }

        public List<FileInfo> Files { get; set; } = new List<FileInfo>();
        public List<DirectoryInfo> Directories { get; set; } = new List<DirectoryInfo>();
        public PathInfo Path { get; set; }
        public string Name { get => Path.Folders.LastOrDefault(); }
        public int Level { get; set; }

        public override string ToString()
        {
            string t = "";
            for (int i = 0; i < Level; i++)
            {
                t += "--";
            }
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(t+Path.ToString());
            foreach (FileInfo file in Files)
            {
                sb.AppendLine(t+file.ToString());
            }
            foreach (DirectoryInfo directory in Directories)
            {
                sb.Append(directory.ToString());
            }
            return sb.ToString();
        }

    }
    public class IgnoreInfo
    {
        public List<Regex> FilesToIgnore { get; set; } = new List<Regex>();
        public List<Regex> ToIgnore { get; set; } = new List<Regex>();
        public List<Regex> DirectoriesToIgnore { get; set; } = new List<Regex>();
    }
}
