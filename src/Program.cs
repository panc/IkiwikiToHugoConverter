using System;
using System.Threading.Tasks;

namespace BlogConverter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var command = args[0]?.ToLower();

            if (command == "convertposts")
                await _ConvertPosts(args[1], args[2]);

            if (command == "convertgallery")
                await _ConvertGallery(args[1], args[2]);

            if (command == "extensiontolower")
                ExtensionModifier.ExtensionToLower(args[1]);
        }

        private static async Task _ConvertGallery(string input, string output)
        {
            var converter = new GalleryConverter();
            await converter.ConvertFolderAsync(input, output);

            Console.WriteLine("Finished!");
        }

        private static async Task _ConvertPosts(string input, string output)
        {
            var converter = new IkiwikiToHugoConverter();
            await converter.ConvertFolderAsync(input, output);

            Console.WriteLine("Finished!");
        }
    }
}
    