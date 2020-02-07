using LearningProcess.ORM.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LearningProcess
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            SettingsInstance = GetSettings(args);

            new Application();

            {//Устанавливаю размер шрифтов
                var newStyle = new Style(typeof(Control));
                newStyle.Setters.Add(new Setter(Control.FontSizeProperty, (double)TextElement.FontSizeProperty.DefaultMetadata.DefaultValue * 2));

                FrameworkElement.StyleProperty.OverrideMetadata(typeof(Control), new FrameworkPropertyMetadata
                { DefaultValue = newStyle, });
            }

            MVVM.Binds.MakeBinds(SettingsInstance.Context);

            var startWindow = MVVM.WindowManager.GetEditWindow();
            startWindow.ShowDialog();

            SaveSettings();
        }

        public static Settings SettingsInstance { get; private set; }

        public class Settings
        {
            public Settings()
            {
                _isReadOnly = false;
                BlockSize = new Dictionary<string, double>();
                SelectedThemeIndex = 4;
            }

            private IContext _context;
            private string _sourceType;

            public IContext Context => _context;

            private void InitContext()
            {
                switch (SourceType.ToLower())
                {
                    case "local":
                        _context = new LocalContext();
                        break;
                    case "file":
                        _context = new FileContext(GetLocalPath() + "DataBase.db");
                        break;
                    default:
                        throw new Exception($"Unknown sourceType type value = {SourceType}");
                }
            }

            public string SourceType
            {
                get => _sourceType;
                set
                {
                    _sourceType = value;
                    InitContext();
                }
            }

            private bool _isReadOnly;

            public void SetIsReadOnly(bool value)
            { _isReadOnly = value; }

            public bool IsReadOnly => _isReadOnly;
            public string Password { get; set; }
            public int SelectedThemeIndex { get; set; }
            public int DisciplineKey { get; set; }
            public Dictionary<string, double> BlockSize { get; set; }
        }

        public static void SaveSettings()
        {
            var documentsPath = GetDocumentsPath();
            var localPath = GetLocalPath();

            SaveSettingsToFile(localPath + "settings.txt", SettingsInstance);
        }

        private static Settings GetSettings(string[] args)
        {
            var documentsPath = GetDocumentsPath();
            var localPath = GetLocalPath();

            Settings settings = ReadSettingsFromFile(localPath + "settings.txt");

            string sourceType = "file";

            if (args != null && args.Any())
            {
                foreach (var arg in args)
                {
                    var splited = arg.Split('=');

                    if (splited.Length == 2)    //если строки две, тогда одна часть считается именем, другая значением
                    {
                        var settingName = splited[0].Trim().ToLower();
                        var value = splited[1].Trim().ToLower();

                        switch (settingName)
                        {
                            case "sourcetype":  //Тип источника данных
                                sourceType = value;
                                break;
                        }
                    }
                }
            }

            //sourceType заношу тут, т.к. такой настройки в аргументах может не быть
            if (String.IsNullOrEmpty(settings.SourceType))
            { settings.SourceType = sourceType; }

            settings.SetIsReadOnly(String.IsNullOrEmpty(settings.Password));

            return settings;
        }

        private static Settings ReadSettingsFromFile(string fileName)
        {
            var propDictionary = typeof(Settings).GetProperties().Where(x => x.GetMethod != null && x.SetMethod != null).ToDictionary(x => x.Name.ToLower(), x => x);
            var settings = new Settings();

            if (File.Exists(fileName))
            {
                using (var sr = new StreamReader(fileName))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();

                        if (!String.IsNullOrEmpty(line))
                        {
                            var split = line.Split('=');

                            if (split.Length > 1)
                            {
                                var name = split.First().ToLower();

                                if (propDictionary.ContainsKey(name))
                                {
                                    var prop = propDictionary[name];
                                    var strValue = split[1];
                                    object value;

                                    if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(int))
                                    { value = Convert.ChangeType(strValue, prop.PropertyType); }
                                    else if (prop.PropertyType == typeof(Dictionary<string, double>))
                                    { value = Serialize<string, double>(strValue); }
                                    else
                                    { throw new Exception($"Unknown setting type: {prop.PropertyType.Name}"); }

                                    prop.SetValue(settings, value);
                                }
                            }
                        }
                    }
                }
            }

            return settings;
        }

        public static void SaveSettingsToFile(string fileName, Settings settings)
        {
            var propDictionary = typeof(Settings).GetProperties().Where(x => x.GetMethod != null && x.SetMethod != null).ToDictionary(x => x.Name.ToLower(), x => x);

            var sb = new StringBuilder();

            foreach (var keyValuePair in propDictionary)
            {
                sb.Append(keyValuePair.Key);
                sb.Append('=');

                var prop = keyValuePair.Value;

                object value = prop.GetValue(settings);
                string strValue;

                if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(bool) || prop.PropertyType == typeof(int))
                { strValue = Convert.ChangeType(value, typeof(string)).ToString(); }
                else if (prop.PropertyType == typeof(Dictionary<string, double>))
                { strValue = Deserialize(value as Dictionary<string, double>); }
                else
                { throw new Exception($"Unknown setting type: {prop.PropertyType.Name}"); }

                sb.Append(strValue);
                sb.Append(Environment.NewLine);
            }

            if (sb.Length > 0)
            { sb.Length -= Environment.NewLine.Length; }

            using (var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.SetLength(0);
                var txt = sb.ToString();

                var bytes = Encoding.GetEncoding(1251).GetBytes(txt);
                fs.Write(bytes, 0, bytes.Length);
            }
        }

        private static Dictionary<T1, T2> Serialize<T1,T2>(string txt)
        {
            var dictionary = new Dictionary<T1, T2>();

            if (!String.IsNullOrEmpty(txt))
            {
                var pairs = txt.Split(',');

                foreach (var strKeyValuePair in pairs)
                {
                    var splitStrKeyValuePair = strKeyValuePair.Split(':');

                    if (splitStrKeyValuePair.Length == 2)
                    {
                        var strKey = splitStrKeyValuePair.First();
                        var strValue = splitStrKeyValuePair.Last();

                        var key = (T1)Convert.ChangeType(strKey, typeof(T1));

                        if (typeof(T2) == typeof(double))
                        { strValue = strValue.Replace('.', ','); }

                        var value = (T2)Convert.ChangeType(strValue, typeof(T2));

                        try
                        { dictionary.Add(key, value); }
                        catch (Exception ex)
                        {
                            throw new Exception("While dictionary set ", ex);
                        }
                    }
                    else
                    {
                        throw new Exception(
                            "While dictionary set. Key value pair must include two values, but founded: "
                            + String.Join(",", splitStrKeyValuePair));
                    }
                }
            }

            return dictionary;
        }

        private static string Deserialize<T1, T2>(Dictionary<T1, T2> dictionary)
        {
            var sb = new StringBuilder();

            foreach (var keyValuePair in dictionary)
            {
                sb.Append(keyValuePair.Key);
                sb.Append(':');

                var strValue = keyValuePair.Value.ToString();

                if (typeof(T2) == typeof(double))
                { strValue = strValue.Replace(',', '.'); }

                sb.Append(strValue);

                sb.Append(',');
            }

            if (sb.Length > 0)
            { sb.Length--; }

            return sb.ToString();
        }

        public static string GetDocumentsPath()
        {
            var documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\LearningProcess\\";

            if (!Directory.Exists(documentsFolder))
            { Directory.CreateDirectory(documentsFolder); }

            return documentsFolder;
        }

        public static string GetLocalPath()
        { return ".\\"; }
    }
}