# PowerPointTimingAndVideoConverter
For use this extension you should download Vosk small, or Vosk large model from the link https://alphacephei.com/vosk/models (not other model*). 
And you should change this Path:             
//Path to model
     
     Model model = new Model("E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\model");
And also you need to download ffmpeg.exe from the link https://www.videohelp.com/software/ffmpeg

1)unpack the archive, 

2)find a folder bin,

3)extract the file ffmpeg.exe to a convenient location,

4)change this Path:

//Path to ffmpeg.exe 

ffmpegProcess.StartInfo.FileName = "E:\\Visual_studio_files_and_Visual_trash\\SecondVooosk\\SecondVooosk\\ffmpeg.exe";
