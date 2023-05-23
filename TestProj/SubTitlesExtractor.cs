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
            var mp3out = "";
            var ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.RedirectStandardInput = true;
            ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            Console.WriteLine("Convert started11");
            ffmpegProcess.StartInfo.CreateNoWindow = true;
            ffmpegProcess.StartInfo.FileName = "E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\ffmpeg.exe";
            Console.WriteLine("Convert started2");
            ffmpegProcess.StartInfo.Arguments = " -i " + inputStereoFile + " -ar 16000 " + outputMonoFile;

            ffmpegProcess.Start();
            ffmpegProcess.StandardOutput.ReadToEnd();
            mp3out = ffmpegProcess.StandardError.ReadToEnd();
            Console.WriteLine("Convert started222");
            ffmpegProcess.WaitForExit();
            if (!ffmpegProcess.HasExited)
            {
                Console.WriteLine("Convert in the if");
                ffmpegProcess.Kill();
            }
            Console.WriteLine(mp3out);
            Console.WriteLine("Convert ended");
        }
        public static void ConverStereoToMono(string inputStereoFile, string outputMonoFile)
        {
            var mp3out = "";
            var ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.RedirectStandardInput = true;
            ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            Console.WriteLine("Convert started11");
            ffmpegProcess.StartInfo.CreateNoWindow = true;
            ffmpegProcess.StartInfo.FileName = "E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\ffmpeg.exe";
            Console.WriteLine("Convert started2");
            ffmpegProcess.StartInfo.Arguments = " -i " + inputStereoFile + " -ac 1 " + outputMonoFile;
            // ffmpegProcess.StartInfo.Arguments = " -i " + inputStereoFile + " -r 16000 " + outputMonoFile;

            ffmpegProcess.Start();
            ffmpegProcess.StandardOutput.ReadToEnd();
            mp3out = ffmpegProcess.StandardError.ReadToEnd();
            Console.WriteLine("Convert started222");
            ffmpegProcess.WaitForExit();
            if (!ffmpegProcess.HasExited)
            {
                Console.WriteLine("Convert in the if");
                ffmpegProcess.Kill();
            }
            Console.WriteLine(mp3out);
            Console.WriteLine("Convert ended");
        }

        public static void ConverMp4toMp3(string inputFile, string outputFile)
        {
            var mp3out = "";
            var ffmpegProcess = new Process();
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.RedirectStandardInput = true;
            ffmpegProcess.StartInfo.RedirectStandardOutput = true;
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            Console.WriteLine("Convert started11");
            ffmpegProcess.StartInfo.CreateNoWindow = true;
            ffmpegProcess.StartInfo.FileName = "E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\ffmpeg.exe";
            Console.WriteLine("Convert started2");
            ffmpegProcess.StartInfo.Arguments = " -i " + inputFile + " -vn -f mp3 -ab 320k " + outputFile;

            Console.WriteLine("Convert started");
            ffmpegProcess.Start();
            ffmpegProcess.StandardOutput.ReadToEnd();
            mp3out = ffmpegProcess.StandardError.ReadToEnd();
            Console.WriteLine("Convert started222");
            ffmpegProcess.WaitForExit();
            if (!ffmpegProcess.HasExited)
            {
                Console.WriteLine("Convert in the if");
                ffmpegProcess.Kill();
            }
            Console.WriteLine(mp3out);
            Console.WriteLine("Convert ended");
        }
        public static void ConvertToWav(string inputMp3, string outputWav)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(inputMp3))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(outputWav, pcm);
                }
            }
        }
        
        public static List<string> ExportSubTitlesFromAudioFile(Model model, string audioPath)
        {
            //Model model2 = new Model("E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\model");
            VoskRecognizer rec = new VoskRecognizer(model, 16000);
            rec.SetMaxAlternatives(0);
            rec.SetWords(true);
            var FullText = new List<string>();

            using (Stream source = File.OpenRead(audioPath))
            {
                byte[] buffer = new byte[4096];
                int bytesRead;
                while ((bytesRead = source.Read(buffer, 0, buffer.Length)) > 0)
                {
                    if (rec.AcceptWaveform(buffer, bytesRead))
                    {
                        string text = rec.Result();//Sentense
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
            // Console.WriteLine(rec.FinalResult());
            // запись в файл
            //string path = "SubTitles.txt";
            /*string text = rec.FinalResult();
            int index = text.IndexOf("text");
            string cutingText = "";
            for (int i = index; i < text.Length; i++)
            {
                cutingText += string.Join("", text[i]);
            }
            using (FileStream fstream = new FileStream(txtFilePath, FileMode.OpenOrCreate))
            {
                byte[] buffer = Encoding.Default.GetBytes(cutingText);
                await fstream.WriteAsync(buffer, 0, buffer.Length);
                Console.WriteLine("Текст записан в файл");
            }*/
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
