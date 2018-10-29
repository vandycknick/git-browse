using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
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
            var url = GetRemoteUrlFromGitRepository(repository);

            var info = new ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName = url,
            };

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) && RuntimeInformation.OSDescription.Contains("Microsoft"))
            {
                info.UseShellExecute = false;
                info.FileName = "powershell.exe";
                info.Arguments = $"start {url}";
            }

            var proc = Process.Start(info);
            proc.WaitForExit();
        }

        public static string GetRemoteUrlFromGitRepository(IRepository repository)
        {
            var remote = repository.Config.GetValueOrDefault<string>("remote.origin.url");
            var branch = repository.Head.FriendlyName;
            (string protocol, string domain, string urlPath) = ParseGitRemoteUrl(remote);

            protocol = repository.Config.GetValueOrDefault<string>("browse.protocol", protocol);
            domain = repository.Config.GetValueOrDefault<string>("browse.domain", domain);

            var url = $"{protocol}://{domain}/{urlPath}";

            if (branch != "master" && repository.Head.IsTracking)
                url = $"url/tree/{branch}";

            return url;
        }

        public static (string, string, string) ParseGitRemoteUrl(string remote)
        {
            string domain = "";
            string urlpath = "";
            string protocol = PROTOCOL;

            if (IsUrlRegex.IsMatch(remote))
            {
                var matches = IsUrlRegex.Matches(remote);
                var uri = new Uri(remote);
                domain = uri.Host;
                urlpath = uri.AbsolutePath;
            }
            else
            {
                var uri = remote.Split('@', 2)[1];
                var parts = uri.Split(':', 2);
                domain = parts[0];
                urlpath = parts[1];
            }

            urlpath = urlpath.TrimStart('/').TrimEnd('/');
            urlpath = urlpath.TrimEnd('.', 'g', 'i', 't');

            return (protocol, domain, urlpath);
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
