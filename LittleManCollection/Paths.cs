// Copyright 2009, 2010, 2011 Matvei Stefarov <me@matvei.org>
// Creative Commons BY-NC-SA
// http://creativecommons.org/licenses/by-nc-sa/3.0/

// Copyright 2011 Gareth Higgins <Gareth.higgins@sympatico.ca>
// Creative Commons BY-NC-SA
// http://creativecommons.org/licenses/by-nc-sa/3.0/


using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;

namespace LittleMan {
    /// <summary> Contains fCraft path settings, and some filesystem-related utilities. </summary>
    public static class Paths {

        static readonly string[] ProtectedFiles;

        internal static readonly string[] DataFilesToBackup;

        /// <summary>
        /// Bytes to buffer for File IO
        /// </summary>
        public const ushort BUFFER_BYTES = 4096;

        static Paths() {
            string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (assemblyDir != null) {
                WorkingPathDefault = Path.GetFullPath(assemblyDir);
            }
            else {
                WorkingPathDefault = Path.GetPathRoot(assemblyDir);
            }

            WorkingPath = WorkingPathDefault;
            ArchivePath = ArchivePathDefault;
            LogPath = LogPathDefault;
            ConfigFileName = ConfigFileNameDefault;

            ProtectedFiles = new[]{
                "LittleManCompiler.exe",
                "LittleManComputer.exe",
                "LittleManIDE.exe"
            };

            DataFilesToBackup = new[]{
                PlaceHolderName
            };
        }


        #region Paths & Properties

        /// <summary>
        /// String Paths of all files
        /// </summary>
        public const string ArchivePathDefault = "archives",
                            LogPathDefault = "logs",
                            ConfigFileNameDefault = "config.xml";

        /// <summary>
        /// Current directory of executeable
        /// </summary>
        public static readonly string WorkingPathDefault;

        /// <summary> Path to save old inputs to (default: .\archives)
        /// Can be overridden at startup via command-line argument "--archivepath=",
        /// or via "ArchivePath" ConfigKey </summary>
        public static string ArchivePath { get; set; }

        /// <summary> Working path (default: whatever directory LittleManCompiler.exe is located in)
        /// Can be overridden at startup via command line argument "--path=" </summary>
        public static string WorkingPath { get; set; }

        /// <summary> Path to save logs to (default: .\logs)
        /// Can be overridden at startup via command-line argument "--logpath=" </summary>
        public static string LogPath { get; set; }

        /// <summary> Path to load/save config to/from (default: .\config.xml)
        /// Can be overridden at startup via command-line argument "--config=" </summary>
        public static string ConfigFileName { get; set; }


        /// <summary>
        /// Placeholder for future fomatting
        /// </summary>
        public const string PlaceHolderName = "placeholdername";

        /// <summary>
        /// Placeholder for future formatting
        /// </summary>
        public const string PlaceHolderDirectory = "placeholder";

        /// <summary>
        /// Placeholder for future formatting
        /// </summary>
        public static string PlaceHolderPath {
            get { return Path.Combine(WorkingPath, PlaceHolderDirectory); }
        }
        
        /// <summary>
        /// Compiled work
        /// </summary>
        public const string CompiledDirectory = "Compiled";

        /// <summary>
        /// Default compiled filename
        /// </summary>
        public const string DefaultCompiledFileName = "default.lmc";

        public static string CompiledPath {
            get { return Path.Combine(WorkingPath, CompiledDirectory); }
        }
        public static string DefaultCompiledPath {
            get { return Path.Combine(CompiledPath, DefaultCompiledFileName); }
        }

        /// <summary>
        /// Compiler filename
        /// </summary>
        public const string CompilerFileName = "LittleManCompiler";

        /// <summary>
        /// Compiler filepath
        /// </summary>
        public static string CompilerPath {
            get { return Path.Combine(WorkingPath, CompilerFileName); }
        }

        /// <summary>
        /// Computer filename
        /// </summary>
        public const string ComputerFileName = "LittleManComputer";


        /// <summary>
        /// Computer filepath
        /// </summary>
        public static string ComputerFilePath {
            get { return Path.Combine(WorkingPath, ComputerFileName); }
        }

        /// <summary>
        /// Input filename
        /// </summary>
        public const string InputFileName = "input.mas";

        /// <summary>
        /// Output filename
        /// </summary>
        public const string OutputFileName = "output.mas";

        /// <summary>
        /// InputOutput directory name
        /// </summary>
        public const string InputOutputDirectory = "IO";

        /// <summary>
        /// InputOutput directory path
        /// </summary>
        public static string InputOuputPath {
            get { return Path.Combine(WorkingPath, InputOutputDirectory); }
        }

        /// <summary>
        /// Input path
        /// </summary>
        public static string InputPath {
            get { return Path.Combine(InputOuputPath, InputFileName); }
        }

        /// <summary>
        /// Output path
        /// </summary>
        public static string OutputPath {
            get { return Path.Combine(InputOuputPath, OutputFileName); }
        }

        #endregion


        #region Utility Methods

        /// <summary>
        /// Moves the specified file or replaces it if it already exist at the new location
        /// </summary>
        /// <param name="source">File to move</param>
        /// <param name="destination">Location to move to</param>
        public static void MoveOrReplace( string source,  string destination) {
            if (source == null) throw new ArgumentNullException("source");
            if (destination == null) throw new ArgumentNullException("destination");
            if (File.Exists(destination)) {
                File.Replace(source, destination, null, true);
            }
            else {
                File.Move(source, destination);
            }
        }


        /// <summary> Makes sure that the path format is valid, that it exists, that it is accessible and writeable. </summary>
        /// <param name="pathLabel"> Name of the path that's being tested (e.g. "map path"). Used for logging. </param>
        /// <param name="path"> Full or partial path. </param>
        /// <param name="checkForWriteAccess"> If set, tries to write to the given directory. </param>
        /// <returns> Full path of the directory (on success) or null (on failure). </returns>
        public static bool TestDirectory( string pathLabel,  string path, bool checkForWriteAccess) {
            if (pathLabel == null) throw new ArgumentNullException("pathLabel");
            if (path == null) throw new ArgumentNullException("path");
            try {
                if (!Directory.Exists(path)) {
                    Directory.CreateDirectory(path);
                }
                DirectoryInfo info = new DirectoryInfo(path);
                if (checkForWriteAccess) {
                    string randomFileName = Path.Combine(info.FullName, "fCraft_write_test_" + Guid.NewGuid());
                    using (File.Create(randomFileName)) { }
                    File.Delete(randomFileName);
                }
                return true;

            }
            catch (Exception ex) {
                if (ex is ArgumentException || ex is NotSupportedException || ex is PathTooLongException) {
                    //Logger.Log("Paths.TestDirectory: Specified path for {0} is invalid or incorrectly formatted ({1}: {2}).", LogType.Error,
                                //pathLabel, ex.GetType().Name, ex.Message);
                }
                else if (ex is SecurityException || ex is UnauthorizedAccessException) {
                    //Logger.Log("Paths.TestDirectory: Cannot create or write to file/path for {0}, please check permissions ({1}: {2}).", LogType.Error,
                                //pathLabel, ex.GetType().Name, ex.Message);
                }
                else if (ex is DirectoryNotFoundException) {
                    //Logger.Log("Paths.TestDirectory: Drive/volume for {0} does not exist or is not mounted ({1}: {2}).", LogType.Error,
                                //pathLabel, ex.GetType().Name, ex.Message);
                }
                else if (ex is IOException) {
                    //Logger.Log("Paths.TestDirectory: Specified directory for {0} is not readable/writable ({1}: {2}).", LogType.Error,
                                //pathLabel, ex.GetType().Name, ex.Message);
                }
                else {
                    throw;
                }
            }
            return false;
        }

        /// <summary>
        /// Tests to see if specified file exists, can be read from, and written to.
        /// </summary>
        /// <param name="fileLabel">File label for logging purposes</param>
        /// <param name="filename">File to check</param>
        /// <param name="createIfDoesNotExist">Check if exists</param>
        /// <param name="checkForReadAccess">Check for read access</param>
        /// <param name="checkForWriteAccess">Check for write access</param>
        /// <returns></returns>
        public static bool TestFile( string fileLabel,  string filename,
                                    bool createIfDoesNotExist, bool checkForReadAccess, bool checkForWriteAccess) {
            if (fileLabel == null) throw new ArgumentNullException("fileLabel");
            if (filename == null) throw new ArgumentNullException("filename");
            try {
                new FileInfo(filename);
                if (File.Exists(filename)) {
                    if (checkForReadAccess) {
                        using (File.OpenRead(filename)) { }
                    }
                    if (checkForWriteAccess) {
                        using (File.OpenWrite(filename)) { }
                    }
                }
                else if (createIfDoesNotExist) {
                    using (File.Create(filename)) { }
                }
                return true;

            }
            catch (Exception ex) {
                if (ex is ArgumentException || ex is NotSupportedException || ex is PathTooLongException) {
                    //Logger.Log("Paths.TestFile: Specified path for {0} is invalid or incorrectly formatted ({1}: {2}).", LogType.Error,
                                //fileLabel, ex.GetType().Name, ex.Message);
                }
                else if (ex is SecurityException || ex is UnauthorizedAccessException) {
                    //Logger.Log("Paths.TestFile: Cannot create or write to {0}, please check permissions ({1}: {2}).", LogType.Error,
                                //fileLabel, ex.GetType().Name, ex.Message);
                }
                else if (ex is DirectoryNotFoundException) {
                    //Logger.Log("Paths.TestFile: Drive/volume for {0} does not exist or is not mounted ({1}: {2}).", LogType.Error,
                                //fileLabel, ex.GetType().Name, ex.Message);
                }
                else if (ex is IOException) {
                    //Logger.Log("Paths.TestFile: Specified file for {0} is not readable/writable ({1}: {2}).", LogType.Error,
                                //fileLabel, ex.GetType().Name, ex.Message);
                }
                else {
                    throw;
                }
            }
            return false;
        }


        /// <summary> Path where map backups are stored </summary>
        public static string BackupPath {
            get {
                return Path.Combine(ArchivePath, "backups");
            }
        }

        /// <summary>
        /// Compares specified path, with default archive path
        /// </summary>
        /// <param name="path">Path to compare</param>
        /// <returns>Is the same path</returns>
        public static bool IsDefaultArhivePath(string path) {
            return String.IsNullOrEmpty(path) || Compare(ArchivePathDefault, path);
        }


        /// <summary>Returns true if paths or filenames reference the same location (accounts for all the filesystem quirks).</summary>
        public static bool Compare( string p1,  string p2) {
            if (p1 == null) throw new ArgumentNullException("p1");
            if (p2 == null) throw new ArgumentNullException("p2");
            return Compare(p1, p2, MonoCompat.IsCaseSensitive);
        }


        /// <summary>Returns true if paths or filenames reference the same location (accounts for all the filesystem quirks).</summary>
        public static bool Compare( string p1,  string p2, bool caseSensitive) {
            if (p1 == null) throw new ArgumentNullException("p1");
            if (p2 == null) throw new ArgumentNullException("p2");
            StringComparison sc = (caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
            return String.Equals(Path.GetFullPath(p1).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                                  Path.GetFullPath(p2).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar),
                                  sc);
        }

        /// <summary>
        /// Checks to see if path is valid
        /// </summary>
        /// <param name="path">Path to check</param>
        /// <returns>Is valid</returns>
        public static bool IsValidPath(string path) {
            try {
                new FileInfo(path);
                return true;
            }
            catch (ArgumentException) {
            }
            catch (PathTooLongException) {
            }
            catch (NotSupportedException) {
            }
            return false;
        }


        /// <summary> Checks whether childPath is inside parentPath </summary>
        /// <param name="parentPath">Path that is supposed to contain childPath</param>
        /// <param name="childPath">Path that is supposed to be contained within parentPath</param>
        /// <returns>true if childPath is contained within parentPath</returns>
        public static bool Contains( string parentPath,  string childPath) {
            if (parentPath == null) throw new ArgumentNullException("parentPath");
            if (childPath == null) throw new ArgumentNullException("childPath");
            return Contains(parentPath, childPath, MonoCompat.IsCaseSensitive);
        }


        /// <summary> Checks whether childPath is inside parentPath </summary>
        /// <param name="parentPath"> Path that is supposed to contain childPath </param>
        /// <param name="childPath"> Path that is supposed to be contained within parentPath </param>
        /// <param name="caseSensitive"> Whether check should be case-sensitive or case-insensitive. </param>
        /// <returns> true if childPath is contained within parentPath </returns>
        public static bool Contains( string parentPath,  string childPath, bool caseSensitive) {
            if (parentPath == null) throw new ArgumentNullException("parentPath");
            if (childPath == null) throw new ArgumentNullException("childPath");
            string fullParentPath = Path.GetFullPath(parentPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            string fullChildPath = Path.GetFullPath(childPath).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            StringComparison sc = (caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
            return fullChildPath.StartsWith(fullParentPath, sc);
        }


        /// <summary> Checks whether the file exists in a specified way (case-sensitive or case-insensitive) </summary>
        /// <param name="fileName"> filename in question </param>
        /// <param name="caseSensitive"> Whether check should be case-sensitive or case-insensitive. </param>
        /// <returns> true if file exists, otherwise false </returns>
        public static bool FileExists( string fileName, bool caseSensitive) {
            if (fileName == null) throw new ArgumentNullException("fileName");
            if (caseSensitive == MonoCompat.IsCaseSensitive) {
                return File.Exists(fileName);
            }
            else {
                return new FileInfo(fileName).Exists(caseSensitive);
            }
        }


        /// <summary>Checks whether the file exists in a specified way (case-sensitive or case-insensitive)</summary>
        /// <param name="fileInfo">FileInfo object in question</param>
        /// <param name="caseSensitive">Whether check should be case-sensitive or case-insensitive.</param>
        /// <returns>true if file exists, otherwise false</returns>
        public static bool Exists( this FileInfo fileInfo, bool caseSensitive) {
            if (fileInfo == null) throw new ArgumentNullException("fileInfo");
            if (caseSensitive == MonoCompat.IsCaseSensitive) {
                return fileInfo.Exists;
            }
            else {
                DirectoryInfo parentDir = fileInfo.Directory;
                StringComparison sc = (caseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
                return parentDir.GetFiles("*", SearchOption.TopDirectoryOnly)
                                .Any(file => file.Name.Equals(fileInfo.Name, sc));
            }
        }


        /// <summary> Allows making changes to filename capitalization on case-insensitive filesystems. </summary>
        /// <param name="originalFullFileName"> Full path to the original filename </param>
        /// <param name="newFileName"> New file name (do not include the full path) </param>
        public static void ForceRename( string originalFullFileName,  string newFileName) {
            if (originalFullFileName == null) throw new ArgumentNullException("originalFullFileName");
            if (newFileName == null) throw new ArgumentNullException("newFileName");
            FileInfo originalFile = new FileInfo(originalFullFileName);
            if (originalFile.Name == newFileName) return;
            FileInfo newFile = new FileInfo(Path.Combine(originalFile.DirectoryName, newFileName));
            string tempFileName = originalFile.FullName + Guid.NewGuid();
            MoveOrReplace(originalFile.FullName, tempFileName);
            MoveOrReplace(tempFileName, newFile.FullName);
        }


        /// <summary> Find files that match the name in a case-insensitive way. </summary>
        /// <param name="fullFileName"> Case-insensitive filename to look for. </param>
        /// <returns> Array of matches. Empty array if no files matches. </returns>
        public static FileInfo[] FindFiles( string fullFileName) {
            if (fullFileName == null) throw new ArgumentNullException("fullFileName");
            FileInfo fi = new FileInfo(fullFileName);
            DirectoryInfo parentDir = fi.Directory;
            return parentDir.GetFiles("*", SearchOption.TopDirectoryOnly)
                            .Where(file => file.Name.Equals(fi.Name, StringComparison.OrdinalIgnoreCase))
                            .ToArray();
        }

        /// <summary>
        /// Checks to see if specified file is a protected filename
        /// </summary>
        /// <param name="fileName">File to check</param>
        /// <returns>Is protected</returns>
        public static bool IsProtectedFileName( string fileName) {
            if (fileName == null) throw new ArgumentNullException("fileName");
            return ProtectedFiles.Any(t => Compare(t, fileName));
        }

        #endregion
    }
}