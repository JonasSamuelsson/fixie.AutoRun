﻿using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Fixie.AutoPilot
{
    public class ContentViewModel
    {
        private const string msbuild = @"C:\Windows\Microsoft.NET\Framework64\v4.0.30319\msbuild.exe";
        private const string msbuildArgs = " /p:Configuration=Debug /p:Platform=\"Any CPU\" /v:minimal /nologo /t:rebuild /tv:4.0";

        private readonly EventBus _eventBus;
        private CancellationTokenSource _cancellationTokenSource;

        public ContentViewModel(EventBus eventBus)
        {
            _eventBus = eventBus;

            _eventBus.Handle<StartEvent>(HandleStartEvent);

            Visible = new Observable<bool>();
            Text = new Observable<string>();
        }

        public Observable<bool> Visible { get; private set; }
        public Observable<string> Text { get; private set; }

        private void HandleStartEvent(StartEvent @event)
        {
            Text.Value = string.Empty;
            Visible.Value = true;
            _cancellationTokenSource = new CancellationTokenSource();

            Task.Factory.StartNew(async () =>
            {
                var msbuildProcess = Process.Start(new ProcessStartInfo(msbuild)
                {
                    Arguments = @event.Solution + msbuildArgs,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                });
                Text.Value = msbuildProcess.StandardOutput.ReadToEnd();
                msbuildProcess.WaitForExit();
                if (msbuildProcess.ExitCode == 0)
                {
                    var originalWriter = Console.Out;
                    try
                    {
                        using (var writer = new StringWriter())
                        {
                            Console.SetOut(writer);
                            var appDomain = AppDomain.CreateDomain(Guid.NewGuid().ToString());
                            var launcher =
                                (Launcher)
                                    appDomain.CreateInstanceAndUnwrap(typeof(Launcher).Assembly.FullName,
                                        typeof(Launcher).FullName);

                            var dir = Path.GetDirectoryName(GetType().Assembly.Location);
                            var fixie = Path.Combine(dir, "Fixie.Console.exe");
                            launcher.Execute(fixie, GetPaths(@event.Solution));
                            Text.Value += writer.ToString();
                        }
                    }
                    catch (Exception exception)
                    {
                        Text.Value += exception.ToString();
                    }
                    finally
                    {
                        Console.SetOut(originalWriter);
                    }

                    //var dir = Path.GetDirectoryName(GetType().Assembly.Location);
                    //var fixie = Path.Combine(dir, "Fixie.Console.exe");

                    //AppDomain.CreateDomain("", null, new AppDomainSetup{})
                    //var fixieProcess = Process.Start(new ProcessStartInfo(fixie)
                    //{
                    //    Arguments = GetPaths(@event.Solution),
                    //    CreateNoWindow = true,
                    //    RedirectStandardOutput = true,
                    //    UseShellExecute = false,
                    //    WorkingDirectory = dir
                    //});
                    //Text.Value += fixieProcess.StandardOutput.ReadToEnd();
                    //fixieProcess.WaitForExit();
                }

                await Task.Delay(TimeSpan.FromSeconds(10));
            },
                _cancellationTokenSource.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }

        private static string[] GetPaths(string solution)
        {
            var dir = Path.GetDirectoryName(solution);
            var separators = new[] { " = ", ", " };
            return File.ReadAllLines(solution)
                .Where(x => x.StartsWith("Project("))
                .Select(x => x.Split(separators, StringSplitOptions.None)[1].Trim('"'))
                .Select(x => Path.Combine(dir, x, @"bin\debug", x + ".dll"))
                .ToArray();
        }

        private class Launcher : MarshalByRefObject
        {
            public void Execute(string exe, string[] args)
            {
                Assembly.LoadFile(exe).EntryPoint.Invoke(null, new object[] { args });
            }
        }
    }
}