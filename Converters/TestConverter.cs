﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TestPatternConverter.Models;

namespace TestPatternConverter.Converters
{
    public class TestConverter
    {
        /// <summary>
        /// Base path to the test code
        /// (in the Zephyr repository)
        /// </summary>
        public string TestCodeBasePath { get; }

        /// <summary>
        /// Base path to the CMSIS-DSP test patterns
        /// (in the CMSIS-DSP repository)
        /// </summary>
        public string PatternBasePath { get; }

        public TestConverter(string testCodeBasePath, string patternBasePath)
        {
            TestCodeBasePath = testCodeBasePath;
            PatternBasePath = patternBasePath;
        }

        public void Convert(TestModel test, TextWriter writer)
        {
            //
            // Enumerate all patterns.
            //

            foreach (PatternModel pattern in test.Patterns)
            {
                //
                // Create pattern converter.
                //

                PatternConverter pc = new PatternConverter(test, PatternBasePath);

                //
                // Convert pattern.
                //

                pc.Convert(writer, pattern);

                //
                // Add a separation line.
                //

                writer.WriteLine();
                writer.WriteLine();
            }
        }

        public void Convert(TestModel test)
        {
            //
            // Resolve pattern path.
            //

            string testCodeFilePath = Path.GetDirectoryName(test.TestCodePath);
            string testCodeFileName = Path.GetFileNameWithoutExtension(test.TestCodePath);
            string testCodeFileExtension = Path.GetExtension(test.TestCodePath);

            string testCodePatternFileName = $"{testCodeFileName}_patterns{testCodeFileExtension}";
            string testCodePatternFilePath = Path.Combine(testCodeFilePath, testCodePatternFileName);
            string testCodePatternFileFullPath = Path.Combine(TestCodeBasePath, testCodePatternFilePath);

            //
            // Open test code pattern file.
            //

            FileStream testCodePatternFile = File.OpenWrite(testCodePatternFileFullPath);
            StreamWriter testCodePatternWriter = new StreamWriter(testCodePatternFile);

            //
            // Convert test patterns.
            //

            Convert(test, testCodePatternWriter);

            //
            // Flush test code pattern file stream.
            //

            testCodePatternWriter.Flush();
        }
    }
}
