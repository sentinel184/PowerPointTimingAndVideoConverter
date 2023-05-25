using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vosk;
using System.IO;
using Microsoft.Office.Core;

namespace TestProj
{
    internal class GetSubsFromAnySlide
    {
        public static void GetSubs()
        {
            Model model = new Model("E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\model");
            var Presentation = Globals.ThisAddIn.Application.ActivePresentation;
            // проходим по всем слайдам презентации
            foreach (Slide slide in Presentation.Slides)
            {
                foreach (Microsoft.Office.Interop.PowerPoint.Shape shape in slide.Shapes)
                {
                    // Проверка, является ли форма аудио-объектом
                    if (shape.Type == MsoShapeType.msoMedia && shape.MediaType == PpMediaType.ppMediaTypeSound)
                    {
                        // Получение пути к файлу аудио
                        //string audioPath = shape.MediaFormat.SourceFullName;
                        string audioPath = shape.MediaFormat.Application.Path;
                        List<string> Ext = SubTitlesExtractor.ExportSubTitlesFromAudioFile(model, audioPath);
                        for (int i = 0; i < Ext.Count; i++)
                        {
                            File.WriteAllText("E:\\Visual_studio_files_and_Visual_trash\\TestProj\\TestProj\\Subs.txt", Ext[i]);
                        }
                        // Копирование аудио-файла в новое место
                        string newAudioPath = Path.Combine("C:\\Audios", Path.GetFileName(audioPath));
                        File.Copy(audioPath, newAudioPath);

                        // Обработка аудио-файла
                        // ...
                    }
                }
            }
                    // File.WriteAllText("E:\\Visual_studio_files_and_Visual_trash\\TestProj\\TestProj\\Subs.txt",)
                    // делаем что-то с файлом аудио записи, например, сохраняем его на диск
                    // ...
              
           
        }
    }
}
        

