using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Text_Extractor
{
    class Program
    {        
        public static void Main(string[] args)
        {            
            string path = @"C:\Users\mhdb9\Documents\Github\Eventer";
            List<string> t = new List<string> { ".scss", ".html", ".ts"};
            PatternAnalyzer pa = new PatternAnalyzer(path,".gitignore",t);
            pa.Run();
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
