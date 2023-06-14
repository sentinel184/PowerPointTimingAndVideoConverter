using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using Vosk;
using TestProj;
using Microsoft.Office.Tools.Ribbon;


namespace TestProj
{
    internal class SubTitlesExtractor
    {
        public static void ConvertInternal(string inputStereoFile, string outputMonoFile, string args)
        {

            using (var ffmpegProcess = new Process())
            {
                ffmpegProcess.StartInfo.UseShellExecute = false;
                ffmpegProcess.StartInfo.RedirectStandardInput = true;
                ffmpegProcess.StartInfo.RedirectStandardOutput = true;
                ffmpegProcess.StartInfo.RedirectStandardError = true;
                ffmpegProcess.StartInfo.CreateNoWindow = true;
                ffmpegProcess.StartInfo.FileName = "E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\ffmpeg.exe";
                ffmpegProcess.StartInfo.Arguments = $" -i {inputStereoFile} {args} {outputMonoFile}";
                ffmpegProcess.Start();
                ffmpegProcess.StandardOutput.ReadToEnd();
                ffmpegProcess.WaitForExit();
                if (!ffmpegProcess.HasExited)
                {
                    ffmpegProcess.Kill();
                }
            }
        }
        public static void ExtractAudio(string inputFilePath, string outputFilePath)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\ffmpeg.exe";
            startInfo.Arguments = $"-i \"{inputFilePath}\" -vn -acodec copy \"{outputFilePath}\"";
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardOutput = true;
            startInfo.CreateNoWindow = true;

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();
                process.WaitForExit();
            }
        }
        public static void ConverDiscridisationTo16K(string inputStereoFile, string outputMonoFile)
        {
            ConvertInternal(inputStereoFile, outputMonoFile, "-ar 16000");

        }
        public static void ConverStereoToMono(string inputStereoFile, string outputMonoFile)
        {
            ConvertInternal(inputStereoFile, outputMonoFile, "-ac 1");
        }

        public static void ConverMp4toMp3(string inputFile, string outputFile)
        {
            ConvertInternal(inputFile, outputFile, "-vn -f mp3 -ab 320k");
        }
        public static void ConvertToWav(string inputMp3, string outputWav)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(inputMp3))
            using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
            {
                WaveFileWriter.CreateWaveFile(outputWav, pcm);
            }

        }

        public static List<string> ExportSubTitlesFromAudioFile(Model model, string audioPath)
        {
            VoskRecognizer rec = new VoskRecognizer(model, 16000);
            rec.SetMaxAlternatives(0);
            rec.SetWords(true);
            var FullText = new List<string>();
            FullText.Add("Start");

            using (Stream source = File.OpenRead(audioPath))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (rec.AcceptWaveform(buffer, bytesRead))
                    {
                        string text = rec.Result();
                        int index = text.IndexOf("text") + 7;
                        string cutingText = "";
                        for (int i = index; i < text.Length - 2; i++)
                        {
                            cutingText += string.Join("", text[i]);
                        }
                        FullText.Add(cutingText);
                        Console.WriteLine(cutingText);
                    }
                    else
                    {
                        // Console.WriteLine(rec.PartialResult());
                    }
                }

            }
            DeleteUselessFile(audioPath);
            return FullText;
        }
        public static void DeleteUselessFile(string filePathName)
        {
            FileInfo fileInf = new FileInfo(filePathName);
            if (fileInf.Exists)
            {
                fileInf.Delete();
            }
        }
        public static void FullConvertForExportSubTitles(string inputVideoMp4, string PathOutputMonoWavWithDesritisation16K)
        {
            ExtractAudio(inputVideoMp4, "AudioExtracted.mp4");
            ConverMp4toMp3("AudioExtracted.mp4", "OnlyAudio.mp3");
            DeleteUselessFile("AudioExtracted.mp4");
            ConverStereoToMono("OnlyAudio.mp3", "OnlyMonoAudio.mp3");
            DeleteUselessFile("OnlyAudio.mp3");
            ConverDiscridisationTo16K("OnlyMonoAudio.mp3", "MonoAudioWithDicridisation16k.mp3");
            DeleteUselessFile("OnlyMonoAudio.mp3");
            ConvertToWav("MonoAudioWithDicridisation16k.mp3", "MonoAudioWithDicridisation16kInWavFormat.wav");
            DeleteUselessFile("MonoAudioWithDicridisation16k.mp3");

        }






    }
}
