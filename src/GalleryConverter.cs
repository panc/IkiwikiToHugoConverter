using System;
using System.IO;
using System.Threading.Tasks;

namespace BlogConverter
{
    internal sealed class GalleryConverter
    {
        public GalleryConverter()
        {
        }

        internal async Task ConvertFolderAsync(string inputFolder, string outputFolder)
        {
            foreach (var file in Directory.GetFiles(inputFolder, "*.md"))
                await _ConvertFileAsync(file, outputFolder + "\\" + Path.GetFileName(file));
        }

        private async Task _ConvertFileAsync(string file, string output)
        {
            using(var reader = File.OpenText(file))
            {
                using(var writer = new StreamWriter(File.Open(output, FileMode.Create, FileAccess.Write)))
                {
                    Console.WriteLine("Processing file: " + file);
                    await _ConvertAsync(reader, writer);         
                    Console.WriteLine("Created file " + output);       
                }
            }
        }

        private async Task _ConvertAsync(StreamReader reader, StreamWriter writer)
        {
            /*
                {{< gallery >}}
                {{< figure link="/images/Hausbau/2019-11-22_1.jpg" >}}
                {{< /gallery >}}

                :::gallery
                ![](/images/Hausbau/2019-11-22_1.jpg)
                :::
            */

            while (!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                if (line.Contains("{{< gallery >}}"))
                    line = ":::gallery";

                if (line.Contains("{{< figure link=\""))
                {
                    line = line.Replace("{{< figure link=\"", "![](");
                    line = line.Replace("\" >}}", ")");
                }

                if (line.Contains("{{< /gallery >}}"))
                    line = ":::";

                await writer.WriteLineAsync(line);
            }
        }
    }
}