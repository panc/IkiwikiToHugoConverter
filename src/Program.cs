using System;
using System.Threading.Tasks;

namespace StaticSiteConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var file = @"C:\Users\Christoph\Documents\Projekte\pangerl.ch.blog\posts\2015_05_15_RaspberryPi_1.mdwn";
            var output = @"C:\Users\Christoph\Documents\Projekte\pangerl.ch.blog.azure\content\archive\2015_05_15_RaspberryPi_1.md";

            var converter = new IkiwikiToHugoConverter();
            await converter.ConvertFileAsync(file, output);
            
            Console.WriteLine("Finished!");
        }
    }
}
