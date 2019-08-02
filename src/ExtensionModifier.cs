using System.IO;

namespace BlogConverter
{
    internal static class ExtensionModifier
    {
        public static void ExtensionToLower(string directory)
        {
            if (!Directory.Exists(directory))
                return;

            foreach(var file in Directory.GetFiles(directory))
            {
                var extension = Path.GetExtension(file);
                var lowerExtension = extension.ToLower();

                if (extension != lowerExtension)
                    File.Move(file, Path.ChangeExtension(file, lowerExtension));
            }
        }
    }
}
