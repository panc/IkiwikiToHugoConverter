using System;
using System.Threading.Tasks;

namespace StaticSiteConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = args[0];
            var output = args[1];

            var converter = new IkiwikiToHugoConverter();
            await converter.ConvertFolderAsync(input, output);
            
            Console.WriteLine("Finished!");
        }
    }
}
    