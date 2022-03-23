using Serilog;

namespace MiXTools.Shared
{
    public sealed class LanguageManager
    {
        private static readonly Lazy<LanguageManager> lazy = new(() => { return new LanguageManager(); });
        public static LanguageManager Instance => lazy.Value;
        public string CurrentLanguage { get; set; }
        public string[] Languages { get; }
        public const string MISSING_LANGUAGE_SETTINGS = "Lanugages are missing from property settings!";
        public const string LANGUAGE_MANAGER_ERROR = "Language manager error!";
        public const string ERROR_TITLE = "Error";
        public const int MISSING_SETTINGS_ERROR_CODE = 11;
        private LanguageManager()
        {
            string[]? langs = Properties.Settings.Default.SupportedLanugages.Split('_');
            string? lang = Properties.Settings.Default.CurrentLanguage;

            if (string.IsNullOrEmpty(lang))
            {
                Utils.ErrorHandler(MISSING_LANGUAGE_SETTINGS, LANGUAGE_MANAGER_ERROR, ERROR_TITLE, MISSING_SETTINGS_ERROR_CODE, false);
            }

            Log.Debug("Initializing LanguageManager...");
            Languages = langs;
            Array.Sort(Languages);
            CurrentLanguage = lang;
        }

        public int GetIndexOfCurrentLanugage()
        {
            for (int i = 0; i < Languages.Length; i++)
            {
                if (Languages[i] == CurrentLanguage)
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lang"></param>
        /// <exception cref="InvalidLanguageException"></exception>
        public void ChangeLanguage(string lang)
        {
            if (lang != CurrentLanguage)
            {
                if (Languages.ToList().Contains(lang))
                {
                    Log.Information($"Changing language from {CurrentLanguage} to {lang}");
                    CurrentLanguage = lang;
                    Properties.Settings.Default.CurrentLanguage = lang;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    throw new InvalidLanguageException($"{lang} is not supported");
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="InvalidLanguageException"></exception>
        public string GetTranslation(string key)
        {
            string? translation = CurrentLanguage switch
            {
                "EN" => Properties.Languages.EN_Resources.ResourceManager.GetString(key),
                "HU" => Properties.Languages.HU_Resources.ResourceManager.GetString(key),
                "RU" => Properties.Languages.RU_Resources.ResourceManager.GetString(key),
                _ => throw new InvalidLanguageException($"{CurrentLanguage} is not supported"),
            };
            //Log.Debug($"Translation result {translation} in {CurrentLanguage}");
            return translation ?? key;
        }

    }

    class InvalidLanguageException : Exception
    {
        public InvalidLanguageException(string msg) : base(msg) { }
    }
}
