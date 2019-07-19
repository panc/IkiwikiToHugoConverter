using System;
using System.Threading.Tasks;

namespace StaticSiteConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = @"C:\Users\Christoph\Documents\Projekte\pangerl.ch.privat\posts\2013_Interrail";
            var output = @"C:\Users\Christoph\Documents\Projekte\pangerl.ch.privat.azure\content\2013_Interrail";

            var converter = new IkiwikiToHugoConverter();
            await converter.ConvertFolderAsync(input, output);
            
            Console.WriteLine("Finished!");
        }
    }
}
