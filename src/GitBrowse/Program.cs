using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using LibGit2Sharp;

namespace GitBrowse
{
    public class Program
    {
        private const string PROTOCOL = "https";
        private static readonly Regex IsUrlRegex = new Regex(@"^([a-z\+]+):\/\/.*");

        public static void Main(string[] args)
        {
            var option = args.Length > 0 ? args[0] : "";

            switch (option)
            {
                case "--help":
                    ShowHelp();
                    break;
                case "--version":
                    ShowVersion();
                    break;
                default:
                    GitBrowse();
                    break;
            }
        }

        public static void GitBrowse()
        {
            var path = Repository.Discover(Directory.GetCurrentDirectory());

            if (String.IsNullOrEmpty(path))
            {
                Console.WriteLine("Not inside a git repository");
                return;
            }

            var repository = new Repository(path);
            var origin = repository.Config.GetValueOrDefault<string>("remote.origin.url");
            var branch = repository.Head.FriendlyName;
            var url = GitRemoteToUrl(origin);

            var info = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = url,
            };

            Process.Start(info);
        }

        public static string GitRemoteToUrl(string origin)
        {
            string domain = "";
            string urlpath = "";

            if (IsUrlRegex.IsMatch(origin))
            {
                var matches = IsUrlRegex.Matches(origin);
                var uri = new Uri(origin);
                var protocol = uri.Scheme;
                domain = uri.Host;
                urlpath = uri.AbsolutePath;
            }
            else
            {
                var uri = origin.Split('@', 2)[1];
                var parts = uri.Split(':', 2);
                domain = parts[0];
                urlpath = parts[1];
            }

            urlpath = urlpath.TrimStart('/').TrimEnd('/');
            urlpath = urlpath.TrimEnd('.', 'g', 'i', 't');

            return $"{PROTOCOL}://{domain}/{urlpath}";
        }

        public static void ShowHelp()
        {
            Console.WriteLine($"git-browse {GetVersion()}");
            Console.WriteLine();
            Console.WriteLine("Usage: git-browse [options]");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --version      Show version information");
            Console.WriteLine();
        }

        public static void ShowVersion() => Console.WriteLine(GetVersion());

        private static string GetVersion() =>
            typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion;
    }
}
