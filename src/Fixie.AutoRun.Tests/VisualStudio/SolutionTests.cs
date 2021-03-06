﻿using System.Linq;
using Fixie.AutoRun.FileSystem;
using Fixie.AutoRun.VisualStudio;
using Microsoft.Reactive.Testing;
using Shouldly;
using System;

namespace Fixie.AutoRun.Tests.VisualStudio
{
   public class SolutionTests
   {
      public void ShouldContainConfigurationsFromSolutionFile()
      {
	      var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, new FileSystem(), new TestScheduler());

         solution.Configurations.Count.ShouldBe(2);
         solution.Configurations.ElementAt(0).ShouldBe("Debug");
         solution.Configurations.ElementAt(1).ShouldBe("Release");
      }

      public void ShouldContainPlatormsFromSolutionFile()
      {
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, new FileSystem(), new TestScheduler());

         solution.Platforms.Count.ShouldBe(1);
         solution.Platforms.ElementAt(0).ShouldBe("Any CPU");
      }

      public void ContentFileChangedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Change(@"x:\Test\Foo.Tests\FooTypeTests.cs");

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooTestsProjectPath);
      }

      public void ContentFileCreatedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Create(@"x:\Test\Foo.Tests\FooTypeTests.cs");

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooTestsProjectPath);
      }

      public void ContentFileRenamedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Rename(string.Empty, @"x:\Test\Foo.Tests\FooTypeTests.cs");

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooTestsProjectPath);
      }

      public void ReferenceFileChangedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Change(@"x:\Test\packages\Fixie.dll");

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooTestsProjectPath);
      }

      public void ReferenceFileCreatedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Create(@"x:\Test\packages\Fixie.dll");

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooTestsProjectPath);
      }

      public void ReferenceFileRenamedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Rename(string.Empty, @"x:\Test\packages\Fixie.dll");

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooTestsProjectPath);
      }

      public void ProjectFileChangedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Change(TestDataGenerator.FooProjectPath);

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooProjectPath);
      }

      public void ProjectFileCreatedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Create(TestDataGenerator.FooProjectPath);

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooProjectPath);
      }

      public void ProjectFileRenamedShouldTriggerEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Rename(string.Empty, TestDataGenerator.FooTestsProjectPath);

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooTestsProjectPath);
      }

      public void ShouldCombineMultipleChangesIntoOneEvent()
      {
         var fileSystem = new FileSystem();
         var scheduler = new TestScheduler();
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, fileSystem, scheduler);
         var solutionChangedEventArgs = SolutionChangedEventArgs.Empty;
         solution.Changed += (sender, args) => solutionChangedEventArgs = args;

         fileSystem.Change(@"x:\Test\Foo\FooType.cs");
         fileSystem.Change(TestDataGenerator.FoobarProjectPath);

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(2);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooProjectPath);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FoobarProjectPath);

         fileSystem.Change(@"x:\Test\packages\Fixie.dll");

         scheduler.AdvanceBy(1.Seconds());

         solutionChangedEventArgs.ChangedProjects.Count.ShouldBe(1);
         solutionChangedEventArgs.ChangedProjects.ShouldContain(TestDataGenerator.FooTestsProjectPath);
      }

      public void ShouldBeAbleToEnumerateProjects()
      {
         var solution = Solution.Load(TestDataGenerator.SolutionPath, TestDataGenerator.Get, new FileSystem(), new TestScheduler());
         var projects = solution.ToList();
         projects.Count.ShouldBe(3);
         projects.ElementAt(0).Path.ShouldBe(TestDataGenerator.FooProjectPath);
         projects.ElementAt(1).Path.ShouldBe(TestDataGenerator.FooTestsProjectPath);
         projects.ElementAt(2).Path.ShouldBe(TestDataGenerator.FoobarProjectPath);
      }

      public class FileSystem : IFileSystemWatcher
      {
         public void Dispose() { }

         public event EventHandler<CreatedEventArgs> Created = delegate { };
         public event EventHandler<ChangedEventArgs> Changed = delegate { };
         public event EventHandler<DeletedEventArgs> Deleted = delegate { };
         public event EventHandler<RenamedEventArgs> Renamed = delegate { };

         public string Directory { get; set; }

         public void Change(string path)
         {
            Changed(this, new ChangedEventArgs(path));
         }

         public void Create(string path)
         {
            Created(this, new CreatedEventArgs(path));
         }

         public void Delete(string path)
         {
            Deleted(this, new DeletedEventArgs(path));
         }

         public void Rename(string oldPath, string newPath)
         {
            Renamed(this, new RenamedEventArgs(oldPath, newPath));
         }
      }
   }
}