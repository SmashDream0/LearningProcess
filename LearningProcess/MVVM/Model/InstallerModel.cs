using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LearningProcess.MVVM.Model
{
    public class InstallerModel
    {
        /// <summary>
        /// Сделать единственный исполняемый файл
        /// </summary>
        /// <param name="fileName">Выходное имя файла</param>
        /// <returns></returns>
        public bool MakeSingleExecutable(string fileName)
        {
            MakeChecks(fileName);

            MakeFileList();

            MakeBatSingleExecutable(fileName);

            MVVM.WindowManager.CloseLastWindow();

            return true;
        }

        private void MakeBatSingleExecutable(string fileName)
        {
            var parameters = $"\"{fileName}\" \"{Environment.CurrentDirectory}\\LearningProcess.exe\"";
            var startPath = $"{Environment.CurrentDirectory}\\MakeReadOnly\\Make.bat";

            var Info = new ProcessStartInfo("cmd.exe", "/C \"\"" + startPath + "\" " + parameters + "\"");
            //Info.Verb = "runas";

            Process.Start(Info);
        }

        private void MakeFileList()
        {
            var fileListName = ".\\MakeReadOnly\\всякоразное\\списки обновлений\\LP\\install full.txt";

            if (File.Exists(fileListName))
            { File.Delete(fileListName); }

            var tempPath = Path.GetTempPath() + "\\settings.txt";

            var newSettings = new Program.Settings();
            newSettings.BlockSize = Program.SettingsInstance.BlockSize;
            newSettings.DisciplineKey = Program.SettingsInstance.DisciplineKey;
            newSettings.Password = String.Empty;
            newSettings.SetIsReadOnly(false);
            newSettings.SourceType = Program.SettingsInstance.SourceType;

            Program.SaveSettingsToFile(tempPath, newSettings);

            using (var fs = new FileStream(fileListName, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var fw = new StreamWriter(fs, Encoding.GetEncoding(866)))
                {
                    foreach (var fileName in _fileNames)
                    { fw.WriteLine($"{Environment.CurrentDirectory}\\{fileName}"); }

                    fw.WriteLine(tempPath);
                }
            }
        }

        private void MakeChecks(string fileName)
        {
            if (File.Exists(fileName))
            { File.Delete(fileName); }

            var installPath = ".\\MakeReadOnly\\Готовые установщики";

            if (!Directory.Exists(installPath))
            { Directory.CreateDirectory(installPath); }
        }

        private static IEnumerable<string> _fileNames = new string[]
            {
                "locales",
                "swiftshader",
                "x64",
                "x86",
                "cef.pak",
                "cef_100_percent.pak",
                "cef_200_percent.pak",
                "cef_extensions.pak",
                "CefSharp.BrowserSubprocess.Core.dll",
                "CefSharp.BrowserSubprocess.exe",
                "CefSharp.Core.dll",
                "CefSharp.dll",
                "CefSharp.Wpf.dll",
                "chrome_elf.dll",
                "d3dcompiler_47.dll",
                "Dapper.Contrib.dll",
                "Dapper.dll",
                "DataBase.db",
                "devtools_resources.pak",
                "icudtl.dat",
                "LearningProcess.exe",
                "libcef.dll",
                "libEGL.dll",
                "libGLESv2.dll",
                "natives_blob.bin",
                "snapshot_blob.bin",
                "SQLite.Interop.dll",
                "System.Data.SQLite.dll",
                "v8_context_snapshot.bin",
                "icon.ico"
            };
    }
}