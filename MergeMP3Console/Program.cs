using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

namespace MergeMP3Console
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 1)
            {
                WriteLine("usage: MergeMP3Console path");
            }

            var inputDir = @"D:\_Nora\02 Hoerbuecher\Das kleine Känguru und der Angsthase";

            var inputFiles = Directory.GetFiles(inputDir, "*.mp3");

            if (inputFiles.Length == 0)
            {
                WriteLine("no *.mp3 files found in specified path");
            }

            var outputFile = inputDir + ".mp3";

            Array.Sort(inputFiles);

            try
            {
                using (var outputStream = new FileStream(outputFile, FileMode.Create))
                {
                    Combine(inputFiles, outputStream);

                }
            }
            catch(Exception e)
            {
                WriteLine(e.Message);
            }

        }

        public static void Combine(string[] inputFiles, Stream output)
        {
            foreach (string file in inputFiles)
            {
                WriteLine($"merging file {file}...");
                Mp3FileReader reader = new Mp3FileReader(file);
                if ((output.Position == 0) && (reader.Id3v2Tag != null))
                {
                    output.Write(reader.Id3v2Tag.RawData, 0, reader.Id3v2Tag.RawData.Length);
                }
                Mp3Frame frame;
                while ((frame = reader.ReadNextFrame()) != null)
                {
                    output.Write(frame.RawData, 0, frame.RawData.Length);
                }
            }
        }
    }
}
