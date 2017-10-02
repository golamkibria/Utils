using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileSys;

namespace Utils
{
    class Program
    {
        static void Main(string[] args)
        {
            //var srcPath = new DirectoryInfo(@"D:\Media\MyMedia\AsParents\2017\March 12 - Copy");
            //srcPath.Organizer().GroupImagesByDateTaken();

            MyGCCollectClass.Run();

            Console.ReadLine();
        }
    }
}
