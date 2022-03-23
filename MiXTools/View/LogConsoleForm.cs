using MiXTools.Shared;
using Serilog;
using Serilog.Sinks.RichTextBox.Themes;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.Integration;

namespace MiXTools.View
{
    /// <summary>
    /// Form that will be used as a console for displaying log messages.
    /// </summary>
    public partial class LogConsoleForm : Form, IViewLanguageManager, IViewSimpleDarkMode
    {
        private readonly LanguageManager languageManager;
        private readonly System.Windows.Controls.RichTextBox _logRichTextBox;
        public bool DarkMode { get; set; }
        /// <summary>
        /// Text font
        /// </summary>
        public static readonly string FONT = "Segoe UI";
        /// <summary>
        /// Font size
        /// </summary>
        public static readonly int FONT_SIZE = 14;
        /// <summary>
        /// Constructor initializing the form
        /// </summary>
        public LogConsoleForm() : this(LanguageManager.Instance) { }
        public LogConsoleForm(LanguageManager languageManager)
        {
            this.languageManager = languageManager;
            Log.Information("Starting log console...");
            InitializeComponent();

            SettingsManager.AddSettingsPropertyChangeEventHandler(Settings_PropertyChanged);
            UpdateTextLanguage();

            ChangeDarkMode();

            // add reference to visual studio for ystem.Windows.Forms.Integration 
            // ProgramFiles %\Reference Assemblies\Microsoft\Framework\v3.0\WindowsFormsIntegration.dll
            var richTextBoxHost = new ElementHost
            {
                Dock = DockStyle.Fill, // fills the form
            };

            // add host to panel controls
            panel.Controls.Add(richTextBoxHost);

            // uses XAML
            // set richTextBox properties
            var logRichTextBox = new System.Windows.Controls.RichTextBox
            {
                Background = System.Windows.Media.Brushes.Black,
                Foreground = System.Windows.Media.Brushes.LightGray,
                FontFamily = new System.Windows.Media.FontFamily(FONT),
                FontSize = FONT_SIZE,
                IsReadOnly = true,
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
                HorizontalScrollBarVisibility = ScrollBarVisibility.Auto,
                Margin = new Thickness(0)
            };

            // add created RichTextBox to the host as child
            richTextBoxHost.Child = logRichTextBox;
            _logRichTextBox = logRichTextBox;

            logRichTextBox.TextChanged += new TextChangedEventHandler(RichTextBox_TextChanged);
            // Logger configurations
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.RichTextBox(
                    richTextBoxControl: _logRichTextBox,
                    theme: RichTextBoxConsoleTheme.Colored,
                    outputTemplate: Utils.LOG_OUTPUT_TEMPLATE
                )
                .WriteTo.Async(logSink =>
                {
                    logSink.File(
                        path: Utils.GetLogOutput(),
                        outputTemplate: Utils.LOG_OUTPUT_TEMPLATE,
                        shared: true
                    );
                }).CreateLogger();
            Log.Debug("log dir: " + Utils.GetLogOutput());
            Log.Information("Log console is ready");
        }

        private void RichTextBox_TextChanged(object? sender, TextChangedEventArgs args)
        {
            if (sender != null)
            {
                System.Windows.Controls.RichTextBox richTextBox = (System.Windows.Controls.RichTextBox)sender;
                richTextBox.ScrollToEnd();
            }
        }

        public void UpdateTextLanguage()
        {
            this.Text = languageManager.GetTranslation("LOG_CONSOLE");
        }

        public void Settings_PropertyChanged(object? sender, PropertyChangedEventArgs args)
        {
            Log.Debug($"Settings property {args.PropertyName} changed in LoggingForm");
            switch (args.PropertyName)
            {
                case "CurrentLanguage":
                    UpdateTextLanguage();
                    break;
                default:
                    ChangeDarkMode();
                    break;
            }
        }

        public void Unsubscribe()
        {
            SettingsManager.RemoveSettingsPropertyChangeEventHandler(Settings_PropertyChanged);
        }

        public void ChangeDarkMode()
        {
            DarkMode = Properties.Settings.Default.DarkMode;
            Utils.ChangeWindowDarkMode(this.Handle, DarkMode);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                    components.Dispose();

                if (Log.Logger != null)
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .WriteTo.Async(logSink =>
                        {
                            logSink.File(
                                path: Utils.GetLogOutput(),
                                outputTemplate: Utils.LOG_OUTPUT_TEMPLATE,
                                shared: true
                            );
                        }).CreateLogger();
                }
                Unsubscribe();
            }
            base.Dispose(disposing);
        }
    }
}
