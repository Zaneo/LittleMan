// The MIT License (MIT)
//
// Copyright (c) 2013 Gareth Higgins
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;
using System.Threading;

namespace LittleMan.IO {

    /// <summary>
    /// Interface for which data is expected to be handled
    /// </summary>
     public interface IHumanInterface {
        /// <summary>
        /// Writes the specified value, to the defined ouput.
        /// </summary>
        /// <typeparam name="T"> Type of value to output </typeparam>
        /// <param name="value"></param>
        void Output<T>(T value);
        void SetManualResetEvent(ref ManualResetEvent mre);
        void PreFetch();
        //void NotifyInputRequest();
        short Input();
        void Advance();
        void Cleanup();
    } 

    /// <summary>
    /// CLI implementation of HumanInterface
    /// </summary>
    public class ConsoleInterface : IHumanInterface {
        
        /// <summary>
        /// Outputs value to console
        /// </summary>
        /// <param name="value">value being output</param>
        public void Output<T>(T value) {
            Console.WriteLine(value);
        }

        public void SetManualResetEvent(ref ManualResetEvent mre) {
            return;
        }
        /// <summary>
        /// Dummy method for notifying device of an upcoming fetch
        /// </summary>
        /// <param name="reset"> ThreadSleepSignal </param>
        public void PreFetch() {
            return;
        }

        /// <summary>
        /// Gets value from console
        /// </summary>
        /// <returns>String input</returns>
        public short Input() {
            short val;
            if (!short.TryParse(Console.ReadLine(), out val)){
                throw new FormatException("Expected parsable short");
            }
            return val;
        }

        public void Advance() {
            Console.ReadKey(true);
            return;
        }

        /// <summary>
        /// Isn't needed on console
        /// </summary>
        public void Cleanup() {
        }

    }

    public class FileInterface : IHumanInterface {

        FileStream _inputStream;
        FileStream _outputStream;

        StreamReader _inputReader;
        StreamWriter _outputWriter;

        /// <summary>
        /// Creates a file interface, and the needed files for use
        /// </summary>
        public FileInterface() {
            
            if (!Paths.TestDirectory(Paths.InputOutputDirectory, Paths.InputOuputPath, false)) {
                Directory.CreateDirectory(Paths.InputOuputPath);
            }
            if (!Paths.FileExists(Paths.InputPath, true)) {
                _inputStream = File.Create(Paths.InputPath, Paths.BUFFER_BYTES, FileOptions.SequentialScan);
            }
            else {
                _inputStream = new FileStream(Paths.InputPath, FileMode.Open, FileAccess.Read, FileShare.Read, Paths.BUFFER_BYTES, FileOptions.SequentialScan);
            }
            if (!Paths.FileExists(Paths.OutputPath, true)) {
                _outputStream = File.Create(Paths.OutputPath, Paths.BUFFER_BYTES, FileOptions.WriteThrough);
            }
            else {
                _outputStream = new FileStream(Paths.OutputPath, FileMode.Open, FileAccess.Write, FileShare.Read, Paths.BUFFER_BYTES, FileOptions.WriteThrough);
            }

            _inputReader = new StreamReader(_inputStream);
            _outputWriter = new StreamWriter(_outputStream);
        }

        /// <summary>
        /// Dummy method for notifying device of an upcoming fetch
        /// </summary>
        /// <param name="reset"> ThreadSleepSignal </param>
        public void PreFetch() {
            return;
        }

        public void SetManualResetEvent(ref ManualResetEvent mre) {
            return;
        }
        /// <summary>
        /// Reads the input from file
        /// </summary>
        /// <returns>Input string</returns>
        public short Input() {
            short val;
            if (!short.TryParse(_inputReader.ReadLine(), out val)) {
                throw new FormatException("Expected parsable short");
            }
            return val;
        }

        public void Advance() {
            throw new NotImplementedException("I don't know how you would advance through fileinput, or not advance...");
        }

        /// <summary>
        /// Writes the outpus to file 
        /// </summary>
        /// <param name="value">Output value</param>
        public void Output<T>(T value) {
            _outputWriter.WriteLine(value);
        }

        /// <summary>
        /// Flushes, closes, and releases all resources
        /// </summary>
        public void Cleanup() {
            _inputReader.Close();
            _outputWriter.Close();

            _inputStream.Flush();
            _inputStream.Close();

            _outputStream.Flush();
            _outputStream.Close();
        }
    }
}
