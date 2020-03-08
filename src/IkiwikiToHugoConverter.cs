using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace BlogConverter
{
    internal sealed class IkiwikiToHugoConverter
    {
        public IkiwikiToHugoConverter()
        {
        }

        internal async Task ConvertFolderAsync(string inputFolder, string outputFolder)
        {
            foreach (var file in Directory.GetFiles(inputFolder, "*.mdwn"))
                await _ConvertFileAsync(file, outputFolder + "\\" + Path.GetFileName(file));
        }

        private async Task _ConvertFileAsync(string file, string output)
        {
            using(var reader = File.OpenText(file))
            {
                output = Path.ChangeExtension(output, "md");

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
            // start with parsing meta date of the blog post
            var state = ParseState.MetaData;
            await writer.WriteLineAsync("+++");

            var tags = new List<string>();

            while(!reader.EndOfStream)
            {
                var line = await reader.ReadLineAsync();

                // meta data always comes first
                if (state == ParseState.MetaData)
                {
                    if (line.StartsWith("[[!tag"))
                        tags.Add("\"" + line.Substring(7, line.Length - 2 - 7) + "\"");

                    else if (line.StartsWith("[[!meta date="))
                        await writer.WriteLineAsync("date = " + line.Substring(13, line.Length - 3 - 13).Replace("T", " ").Replace("pm", "").Replace("am", "") + ":00+02:00\"");

                    else if (line.StartsWith("[[!meta title="))
                        await writer.WriteLineAsync("title = " + line.Substring(14, line.Length - 2 - 14));

                    else
                    {
                        await writer.WriteLineAsync("tags = [" + string.Join(",", tags) + "]");
                        await writer.WriteLineAsync("+++");
                        state = ParseState.Content;
                    }
                }

                // skip lines with meta info for images as they are not needed anymore
                if (line.StartsWith("[[!img defaults "))
                    continue;

                // now it can be normal content or a image/gallery
                var isImage = line.StartsWith("[[!img ");

                if (isImage && state == ParseState.Content)
                {
                    // start a image gallery
                    await writer.WriteLineAsync("{{< gallery >}}");
                    state = ParseState.Gallery;
                }

                if (state == ParseState.Gallery)
                {
                    if (!isImage)
                    {
                        // end the image gallery
                        await writer.WriteLineAsync("{{< /gallery >}}");
                        state = ParseState.Content;
                    }
                    else 
                    {
                        var endIndex = line.IndexOf(".jpg ");
                        if (endIndex < 0)
                            endIndex = line.IndexOf(".png");

                        if (endIndex < 0)
                            throw new InvalidOperationException("End of image tag not found!");

                        await writer.WriteLineAsync("{{< figure link=\"/" + line.Substring(7, endIndex + 4 - 7) + "\" >}}");
                    }
                }

                if (state == ParseState.Content)
                {
                    await writer.WriteLineAsync(line);
                }
            }

            if (state == ParseState.Gallery)
            {
                // end the image gallery
                await writer.WriteLineAsync("{{< /gallery >}}");
            }
        }
    }
}