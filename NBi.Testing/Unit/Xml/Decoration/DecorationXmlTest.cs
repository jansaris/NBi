﻿using System.IO;
using System.Reflection;
using NBi.Xml;
using NBi.Xml.Decoration;
using NBi.Xml.Decoration.Command;
using NUnit.Framework;
using NBi.Core.FileManipulation;
using NBi.Core.Batch;
using NBi.Core.Process;

namespace NBi.Testing.Unit.Xml.Decoration
{
    [TestFixture]
    public class DecorationXmlTest
    {

        protected TestSuiteXml DeserializeSample()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.DecorationXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }
            manager.ApplyDefaultSettings();
            return manager.TestSuite;
        }
        
        [Test]
        public void Deserialize_SampleFile_Setup()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup, Is.TypeOf<SetupXml>());
        }


        [Test]
        public void Deserialize_SampleFile_SetupCountCommands()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup.Commands, Has.Count.EqualTo(2));
        }

        [Test]
        public void Deserialize_SampleFile_Cleanup()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Cleanup, Is.TypeOf<CleanupXml>());
        }

        [Test]
        public void Deserialize_SampleFile_CleanupCountCommands()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Cleanup.Commands, Has.Count.EqualTo(1));
        }

        [Test]
        public void Deserialize_SampleFile_LoadCommand()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup.Commands[1], Is.TypeOf<TableLoadXml>());
            var cmd = ts.Tests[testNr].Setup.Commands[1] as TableLoadXml;
            Assert.That(cmd.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2012;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
            Assert.That(cmd.TableName, Is.EqualTo("Users"));
            Assert.That(cmd.InternalFileName, Is.EqualTo("Users.csv"));
        }


        [Test]
        public void Deserialize_SampleFile_ResetCommand()
        {
            int testNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<TableResetXml>());
            var cmd = ts.Tests[testNr].Setup.Commands[0] as TableResetXml;
            Assert.That(cmd.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2012;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
            Assert.That(cmd.TableName, Is.EqualTo("Users"));
        }

        [Test]
        public void Deserialize_SampleFile_ConnectionStringFromDefaults()
        {
            int testNr = 1;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<TableResetXml>());
            var cmd = ts.Tests[testNr].Setup.Commands[0] as TableResetXml;
            Assert.That(cmd.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2012;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
        }

        [Test]
        public void Deserialize_SampleFile_StartAndStopService()
        {
            int testNr = 2;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<ServiceStartXml>());
            var cmd = ts.Tests[testNr].Setup.Commands[0] as ServiceStartXml;
            Assert.That(cmd.TimeOut, Is.EqualTo(5000)); //Default value
            Assert.That(cmd.ServiceName, Is.EqualTo("MyService")); 

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Cleanup.Commands[0], Is.TypeOf<ServiceStopXml>());
            var cmdCleanup = ts.Tests[testNr].Cleanup.Commands[0] as ServiceStopXml;
            Assert.That(cmdCleanup.TimeOut, Is.EqualTo(15000)); //Value Specified
            Assert.That(cmdCleanup.ServiceName, Is.EqualTo("MyService")); 
        }

        [Test]
        public void Deserialize_ThreeCommandsInOneTask_TaskContainsThreeCommands()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup.Commands, Has.Count.EqualTo(1));
            Assert.That(ts.Tests[testNr].Setup.Commands[0], Is.TypeOf<CommandGroupXml>());
            var commandGroup = ts.Tests[testNr].Setup.Commands[0] as CommandGroupXml;
            Assert.That(commandGroup.Commands, Has.Count.EqualTo(3));
        }

        [Test]
        public void Deserialize_OneTaskWithoutParallelAttributeExplicitelySet_TasksAreParallel()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var commandGroup = ts.Tests[testNr].Setup.Commands[0] as CommandGroupXml;
            Assert.That(commandGroup.Parallel, Is.True);
        }

        [Test]
        public void Deserialize_OneTaskWithoutRunOnceAttributeExplicitelySet_TasksAreNotRunOnce()
        {
            int testNr = 3;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var commandGroup = ts.Tests[testNr].Setup.Commands[0] as CommandGroupXml;
            Assert.That(commandGroup.RunOnce, Is.False);
        }

        [Test]
        public void Deserialize_TasksAndCommandsPermutted_AllTasksAndCommandsAreLoaded()
        {
            int testNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Tests[testNr].Setup.Commands, Has.Count.EqualTo(5));
        }

        [Test]
        public void Deserialize_SecondGroupWithRunOnceSetTrue_SecondGroupWithRunOnce()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var group = ts.Groups[1].Setup.Commands[0] as CommandGroupXml;
            Assert.That(group.RunOnce, Is.True);
        }

        [Test]
        public void Deserialize_SecondGroupWithExeRun_SecondGroupWithExeRun()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var group = ts.Groups[1].Setup.Commands[0] as CommandGroupXml;
            Assert.That(group.Commands[2], Is.TypeOf<ExeRunXml>());
        }

        [Test]
        public void Deserialize_SampleFile_ParentSetup()
        {
            int groupNr = 0;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            Assert.That(ts.Groups[groupNr].Tests, Has.Count.EqualTo(2));
            foreach (var test in ts.Groups[groupNr].Tests)
            {
                Assert.That(test.Setup.Commands, Has.Count.EqualTo(2));
                Assert.That(test.Setup.Commands[0], Is.InstanceOf<ServiceStartXml>());
                Assert.That(test.Setup.Commands[1], Is.InstanceOf<TableLoadXml>());
            }
        }

        [Test]
        public void Deserialize_SampleFile_ProcessRun()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            int groupNr = 2;

            // Check the properties of the object.
            var command = ts.Groups[groupNr].Tests[0].Setup.Commands[0];

            Assert.That(command, Is.TypeOf<ExeRunXml>());
            var move = command as IRunCommand;
            Assert.That(move.FullPath, Is.EqualTo(@"Batches\clean.exe"));
            Assert.That(move.Argument, Is.EqualTo("-all"));
            Assert.That(move.TimeOut, Is.EqualTo(1000));
        }

        [Test]
        public void Deserialize_SampleFile_ProcessRunWithoutOptionalArguments()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            int groupNr = 2;

            // Check the properties of the object.
            var command = ts.Groups[groupNr].Tests[0].Setup.Commands[1];

            Assert.That(command, Is.TypeOf<ExeRunXml>());
            var move = command as IRunCommand;
            Assert.That(move.FullPath, Is.EqualTo(@"load.exe"));
            Assert.That(move.TimeOut, Is.EqualTo(0));
        }


        [Test]
        public void Deserialize_SampleFile_BatchRun()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            int groupNr = 3;

            // Check the properties of the object.
            var command = ts.Groups[groupNr].Tests[0].Setup.Commands[0];

            Assert.That(command, Is.TypeOf<SqlRunXml>());
            var batchRun = command as IBatchRunCommand;
            Assert.That(batchRun.FullPath, Is.EqualTo(@"Batches\build.sql"));
            Assert.That(batchRun.ConnectionString, Is.EqualTo("Data source=(local);Initial Catalog=MyDB"));
        }

        [Test]
        public void Deserialize_SampleFile_BatchRunWithoutOptional()
        {
            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();
            int groupNr = 3;

            // Check the properties of the object.
            var command = ts.Groups[groupNr].Tests[0].Setup.Commands[1];

            Assert.That(command, Is.TypeOf<SqlRunXml>());
            var batchRun = command as IBatchRunCommand;
            Assert.That(batchRun.FullPath, Is.EqualTo(@"import.sql"));
            Assert.That(batchRun.ConnectionString, Is.EqualTo(@"Data Source=(local)\SQL2012;Initial Catalog=AdventureWorksDW2012;Integrated Security=true"));
        }

        [Test]
        public void Deserialize_SampleFile_FileDelete()
        {
            int groupNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var command = ts.Groups[groupNr].Tests[0].Setup.Commands[0];

            Assert.That(command, Is.TypeOf<FileDeleteXml>());
            var delete = command as IDeleteCommand;
            Assert.That(delete.FullPath, Is.EqualTo(@"Temp\toto.xls"));
        }

        
        [Test]
        public void Deserialize_SampleFile_FileMove()
        {
            int groupNr = 4;

            // Create an instance of the XmlSerializer specifying type and namespace.
            TestSuiteXml ts = DeserializeSample();

            // Check the properties of the object.
            var command = ts.Groups[groupNr].Tests[1].Setup.Commands[0];

            Assert.That(command, Is.TypeOf<FileCopyXml>());
            var move = command as ICopyCommand;
            Assert.That(move.FullPath, Is.EqualTo(@"Temp\toto.xls"));
            Assert.That(move.SourceFullPath, Is.EqualTo(@"Backup\toto.xls"));
        }



    }
}
