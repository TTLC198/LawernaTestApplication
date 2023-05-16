using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace LawernaTestApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private static Assembly Assembly { get; } = typeof(App).Assembly;

        public static string Name { get; } = Assembly.GetName().Name!;

        public static Version Version { get; } = Assembly.GetName().Version!;

        public static string VersionString { get; } = "v" + Version.ToString(3).Trim();
        
        public static string CdnUrl { get; } = "http://openweathermap.org/img/wn/";
        
        public static string ApiUrl { get; } = "https://api.openweathermap.org/data/2.5/weather";

        public static string ExecutableDirPath { get; } = AppDomain.CurrentDomain.BaseDirectory!;

        public static string GitHubProjectUrl { get; } =
            "https://github.com/TTLC198/LawernaTestApplication";
    }

    public partial class App
    {
        private static IReadOnlyList<string> CommandLineArgs { get; } = Environment.GetCommandLineArgs();

        public static string HiddenOnLaunchArgument { get; } = "--minimize";

        public static bool IsHiddenOnLaunch { get; } = CommandLineArgs.Contains(
            HiddenOnLaunchArgument,
            StringComparer.OrdinalIgnoreCase
        );
    }
}