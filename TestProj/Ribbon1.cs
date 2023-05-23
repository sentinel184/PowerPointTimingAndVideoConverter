using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Core;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using TestProj;
using Vosk;


// TODO:  Выполните эти шаги, чтобы активировать элемент XML ленты:

// 1: Скопируйте следующий блок кода в класс ThisAddin, ThisWorkbook или ThisDocument.

//  protected override Microsoft.Office.Core.IRibbonExtensibility CreateRibbonExtensibilityObject()
//  {
//      return new Ribbon1();
//  }

// 2. Создайте методы обратного вызова в области "Обратные вызовы ленты" этого класса, чтобы обрабатывать действия
//    пользователя, например нажатие кнопки. Примечание: если эта лента экспортирована из конструктора ленты,
//    переместите свой код из обработчиков событий в методы обратного вызова и модифицируйте этот код, чтобы работать с
//    моделью программирования расширения ленты (RibbonX).

// 3. Назначьте атрибуты тегам элементов управления в XML-файле ленты, чтобы идентифицировать соответствующие методы обратного вызова в своем коде.  

// Дополнительные сведения можно найти в XML-документации для ленты в справке набора средств Visual Studio для Office.


namespace TestProj
{
    [ComVisible(true)]
    public class Ribbon1 : Office.IRibbonExtensibility
    {
        private Office.IRibbonUI ribbon;

        public Ribbon1()
        {
        }

        #region Элементы IRibbonExtensibility

        public string GetCustomUI(string ribbonID)
        {
            return GetResourceText("TestProj.Ribbon1.xml");
        }

        #endregion

        #region Обратные вызовы ленты
        //Информацию о методах создания обратного вызова см. здесь. Дополнительные сведения о методах добавления обратного вызова см. по ссылке https://go.microsoft.com/fwlink/?LinkID=271226

        public void Ribbon_Load(Office.IRibbonUI ribbonUI)
        {

            this.ribbon = ribbonUI;
        }

        public void OnButtonClickExportSubTitlesFromVideoFile(Office.IRibbonControl control)
        {
            //TODO
            SubTitlesExtractor extractor = new SubTitlesExtractor();
            FormsForMainMenu newForm= new FormsForMainMenu();
            var Subtitles = new List<string>();
            //Path to model
            Model model = new Model("E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\model");

            var TwoPaths = new List<string>();
            TwoPaths.AddRange(newForm.GetVideoPath());
            SubTitlesExtractor.FullConvertForExportSubTitles(TwoPaths[0], "Output16K.wav");
            // Get a reference to the active presentation
            Presentation presentation = Globals.ThisAddIn.Application.ActivePresentation;
            Subtitles = SubTitlesExtractor.ExportSubTitlesFromAudioFile(model, "MonoAudioWithDicridisation16kInWavFormat.wav");
            // Loop through each slide in the presentation
            for (int i = 1; i <= presentation.Slides.Count; i++)
            {
                // Get a reference to the current slide
                Slide slide = presentation.Slides[i];
                slide.Comments.Add(10, 10, "System", "", Subtitles[i-1]);

            }

            MessageBox.Show("Видео успешно обработано");
        }
        public void OnCustomButtonClick(Office.IRibbonControl control)
        {
            //TODO
            try
            {
                // Get the active presentation.
                var presentation = Globals.ThisAddIn.Application.ActivePresentation;

                // Check if the presentation is in slide show view.
                if (presentation.SlideShowWindow == null)
                {
                    MessageBox.Show("The presentation is not currently in slide show view.");
                    return;
                }

                // Get the total duration of the presentation from the application settings.
                var totalDuration = Properties.Settings.Default.SlideShowDuration;

                // Create a new text file to store the slide times.
                var filePath = Path.Combine("E:\\Visual_studio_files_and_Visual_trash\\TestProj\\TestProj", "slide_times.txt");
                var file = File.CreateText(filePath);

                // Loop through all the slides in the presentation and record their start and end times.
                for (int i = 1; i <= presentation.Slides.Count; i++)
                {
                    var slide = presentation.Slides[i];
                    var slideDuration = slide.SlideShowTransition.AdvanceTime;
                   

                    file.WriteLine($"Slide {i}: {slideDuration} seconds");


                    // If this is not the last slide, subtract the slide duration from the total duration.

                        totalDuration += slideDuration;
                    
                }

                // Add the total duration to the end of the file.
                file.WriteLine($"Total duration: {totalDuration} seconds");

                // Close the file.
                file.Close();

                // Show a message box to indicate that the slide times have been recorded.
                MessageBox.Show("Slide times have been recorded and saved to " + filePath);
            }
            catch (Exception ex)
            {
                // Show an error message box if there was an error.
                MessageBox.Show("Error recording slide times: " + ex.Message);
            }
        }

    


        #endregion

        #region Вспомогательные методы

        private static string GetResourceText(string resourceName)
        {
            Assembly asm = Assembly.GetExecutingAssembly(); 
            string[] resourceNames = asm.GetManifestResourceNames();
            for (int i = 0; i < resourceNames.Length; ++i)
            {
                if (string.Compare(resourceName, resourceNames[i], StringComparison.OrdinalIgnoreCase) == 0)
                {
                    using (StreamReader resourceReader = new StreamReader(asm.GetManifestResourceStream(resourceNames[i])))
                    {
                        if (resourceReader != null)
                        {
                            return resourceReader.ReadToEnd();
                        }
                    }
                }
            }
            return null;
        }

        #endregion
    }
}
