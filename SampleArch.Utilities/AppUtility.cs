using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Management;
using AllTheSame.Common.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SampleArch.Logging;
using System.Data.Entity;

namespace SampleArch.Utilities
{
    /// <summary>
    ///     AppHelper
    /// </summary>
    public static class AppUtility
    {
        /// <summary>
        ///     Validates the context.
        /// </summary>
        /// <param name="context"></param>
        public static void ValidateContext(DbContext context)
        {
            if (context != null) return;

            var msg = string.Format(AppConstant.ErrorMessages.ArgumentNullExceptionMessage,
                "_context");
            var anex = new NullReferenceException(msg);

            Audit.Log.Error(msg, anex);

            throw anex;
        }

        /// <summary>
        ///     Validates the database set.
        /// </summary>
        /// <param name="dbSet">The database set.</param>
        public static void ValidateDbSet<TEntity>(IDbSet<TEntity> dbSet) where TEntity : class
        {
            if (!dbSet.IsNull()) return;

            var msg = string.Format(AppConstant.ErrorMessages.NullReferenceExceptionMessage,
                "_dbset");
            var nex = new NullReferenceException(msg);

            Audit.Log.Error(msg, nex);

            throw nex;
        }

        /// <summary>
        ///     Validates the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public static void ValidateEntity<TEntity>(TEntity entity)
        {
            if (!entity.IsNull()) return;

            var msg = string.Format(AppConstant.ErrorMessages.ArgumentNullExceptionMessage,
                "entity");
            var anex = new NullReferenceException(msg);

            Audit.Log.Error(msg, anex);

            throw anex;
        }

        /// <summary>
        ///     Determines whether [is higher version] [the specified sample version].
        /// </summary>
        /// <param name="sampleVersion">The sample version.</param>
        /// <param name="baseVersion">The base version.</param>
        /// <returns></returns>
        public static bool IsHigherVersion(IReadOnlyList<byte> sampleVersion, IReadOnlyList<byte> baseVersion)
        {
            for (var i = 0; i < baseVersion.Count; i++)
            {
                if (sampleVersion[i] > baseVersion[i])
                {
                    return true;
                }
                if (sampleVersion[i] < baseVersion[i])
                {
                    return false;
                }
            }
            return false;
        }



        /// <summary>
        /// Any files in directory?
        /// </summary>
        /// <param name="directoryPath">The directory path.</param>
        /// <returns></returns>
        public static bool AnyFilesInDirectory(string directoryPath)
        {
            if (string.IsNullOrWhiteSpace(directoryPath)) return false;
            var result = default (bool);

            try
            {
                var dir = new DirectoryInfo(directoryPath);
                var dvsFiles = dir.GetFiles();

                result = !dvsFiles.Any();
            }
            catch (SecurityException ex)
            {
                Audit.Log.Error("Utils::AnyFilesInDirectory - SecurityException: {0}", ex);
            }
            catch (ArgumentNullException ex)
            {
                Audit.Log.Error("Utils::AnyFilesInDirectory - ArgumentNullException: {0}", ex);
            }
            catch (ArgumentException ex)
            {
                Audit.Log.Error("Utils::AnyFilesInDirectory - ArgumentException: {0}", ex);
            }
            catch (PathTooLongException ex)
            {
                Audit.Log.Error("Utils::AnyFilesInDirectory - PathTooLongException: {0}", ex);
            }
            catch (DirectoryNotFoundException ex)
            {
                Audit.Log.Error("Utils::AnyFilesInDirectory - DirectoryNotFoundException: {0}", ex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error("Utils::AnyFilesInDirectory - Exception: {0}", ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the object from json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">The json.</param>
        /// <returns></returns>
        public static T GetObjectFromJson<T>(string json) where T : class, new()
        {
            if (string.IsNullOrWhiteSpace(json)) return null;
            var result = default(T);

            try
            {
                result = JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonSerializationException ex)
            {
                Audit.Log.Error("Utils::GetObjectFromJson - JsonSerializationException: {0}", ex);
            }
            catch (JsonException ex)
            {
                Audit.Log.Error("Utils::GetObjectFromJson - JsonException: {0}", ex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error("Utils::GetObjectFromJson - Exception: {0}", ex);
            }

            return result;
        }

        /// <summary>
        /// Gets the json from object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj">The object.</param>
        /// <param name="addIndent">if set to <c>true</c> [add indent].</param>
        /// <returns></returns>
        public static string GetJsonFromObject<T>(T obj, bool addIndent = false) where T : class, new()
        {
            if (obj == default(T)) return null;
            var result = default (string);

            try
            {
                result = JsonConvert.SerializeObject(obj, addIndent ? Formatting.Indented : Formatting.None);
            }
            catch (JsonSerializationException ex)
            {
                Audit.Log.Error("Utils::GetJsonFromObject - JsonSerializationException: {0}", ex);
            }
            catch (JsonException ex)
            {
                Audit.Log.Error("Utils::GetJsonFromObject - JsonException: {0}", ex);
            }
            catch (Exception ex)
            {
                Audit.Log.Error("Utils::GetJsonFromObject - Exception: {0}", ex);
            }

            return result;
        }

        /// <summary>
        ///     Gets the default ip gateway.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string GetDefaultIPGateway()
        {
            Audit.Log.DebugFormat("GetDefaultIPGateway called.");

            var result = string.Empty;
            const string query = "SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'";

            using (var moSearch = new ManagementObjectSearcher(query))
            {
                var moCollection = moSearch.Get();

                if (moCollection.Count == 0) return result;

                foreach (var mo in moCollection)
                {
                    Audit.Log.DebugFormat("GetDefaultIPGateway :: HostName = {0}", mo["DNSHostName"]);
                    Audit.Log.DebugFormat("GetDefaultIPGateway :: Description = {0}", mo["Description"]);


                    var addresses = mo["IPAddress"] as string[];
                    if (addresses != null)
                    {
                        try
                        {
                            Parallel.ForEach(addresses,
                                ipaddress =>
                                {
                                    Audit.Log.DebugFormat("GetDefaultIPGateway :: IPAddress = {0}", ipaddress);
                                });
                        }
                        catch (ArgumentNullException argumentNullException)
                        {
                            Audit.Log.Error("GetDefaultIPGateway :: Getting IPAddress - ArgumentNullException: {0}",
                                argumentNullException);
                        }
                        catch (AggregateException aggregateException)
                        {
                            Audit.Log.Error("GetDefaultIPGateway :: Getting IPAddress - AggregateException: {0}",
                                aggregateException);
                        }
                    }

                    var subnets = mo["IPSubnet"] as string[];
                    if (subnets != null)
                    {
                        try
                        {
                            Parallel.ForEach(subnets,
                                ipsubnet =>
                                {
                                    Audit.Log.DebugFormat("GetDefaultIPGateway :: Subnet - IPAddress = {0}", ipsubnet);
                                });
                        }
                        catch (ArgumentNullException argumentNullException)
                        {
                            Audit.Log.Error("GetDefaultIPGateway ::Getting Subnet - ArgumentNullException: {0}",
                                argumentNullException);
                        }
                        catch (AggregateException aggregateException)
                        {
                            Audit.Log.Error("GetDefaultIPGateway :: Getting Subnet - AggregateException: {0}",
                                aggregateException);
                        }
                    }


                    var defaultgateways = mo["DefaultIPGateway"] as string[];
                    if (defaultgateways == null) continue;
                    try
                    {
                        Parallel.ForEach(defaultgateways, gateway =>
                        {
                            Audit.Log.DebugFormat("GetDefaultIPGateway :: DefaultIPGateway = {0}", gateway);

                            if (string.IsNullOrWhiteSpace(gateway) || gateway.Equals("::")) return;
                            result = gateway;
                        });
                    }
                    catch (ArgumentNullException argumentNullException)
                    {
                        Audit.Log.Error("GetDefaultIPGateway :: Getting Gateway - ArgumentNullException: {0}",
                            argumentNullException);
                    }
                    catch (AggregateException aggregateException)
                    {
                        Audit.Log.Error("GetDefaultIPGateway :: Getting Gateway - AggregateException: {0}",
                            aggregateException);
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///     Writes the data to file.
        /// </summary>
        /// <param name="filePath">The final data path.</param>
        /// <param name="originalJson">The original json.</param>
        /// <returns></returns>
        public static bool WriteDataToFile(string filePath, string originalJson)
        {
            Audit.Log.DebugFormat("WriteDataToFile called, file path: {0}.", filePath);

            if (string.IsNullOrWhiteSpace(filePath))
            {
                Audit.Log.ErrorFormat("WriteDataToFile - filePath parameter null or empty.");
                return false;
            }

            var result = default(bool);
            try
            {
                File.WriteAllText(filePath, originalJson);

                Audit.Log.DebugFormat("WriteDataToFile - Data saved to file: {0}", filePath);

                result = File.Exists(filePath);

                Audit.Log.DebugFormat("WriteDataToFile - Data saved successfully: {0}", result);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Audit.Log.Error("WriteDataToFile::DirectoryNotFoundException - {0}", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                Audit.Log.Error("WriteDataToFile::IOException - {0}", ioException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Audit.Log.Error("WriteDataToFile::UnauthorizedAccessException - {0}", unauthorizedAccessException);
            }
            catch (SecurityException securityException)
            {
                Audit.Log.Error("WriteDataToFile::SecurityException - {0}", securityException);
            }
            catch (ArgumentException argumentException)
            {
                Audit.Log.Error("WriteDataToFile::ArgumentException - {0}", argumentException);
            }
            catch (NotSupportedException notSupportedException)
            {
                Audit.Log.Error("WriteDataToFile::NotSupportedException - {0}", notSupportedException);
            }

            return result;
        }

        /// <summary>
        ///     Directories the exists.
        /// </summary>
        /// <param name="folderPath">Name of the folder.</param>
        /// <param name="createIfMissing">if set to <c>true</c> [add].</param>
        /// <returns></returns>
        public static bool DirectoryExists(string folderPath, bool createIfMissing = false)
        {
            Audit.Log.DebugFormat("DirectoryExists called, allowed to create, if it does not exist: {0}.",
                createIfMissing);

            if (string.IsNullOrWhiteSpace(folderPath) && !createIfMissing)
            {
                Audit.Log.ErrorFormat("FilePatternExists - folderPath parameter null or empty.");
                return false;
            }

            var result = default(bool);
            try
            {
                if (!string.IsNullOrWhiteSpace(folderPath))
                {
                    var alreadyExists = Directory.Exists(folderPath);

                    if (createIfMissing && !alreadyExists)
                    {
                        Directory.CreateDirectory(folderPath);
                        Audit.Log.DebugFormat("DirectoryExists -  directory missing, created: {0}", folderPath);
                    }

                    result = Directory.Exists(folderPath);
                }
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Audit.Log.Error("DirectoryExists::DirectoryNotFoundException - {0}", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                Audit.Log.Error("DirectoryExists::IOException - {0}", ioException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Audit.Log.Error("DirectoryExists::UnauthorizedAccessException - {0}", unauthorizedAccessException);
            }
            catch (ArgumentException argumentException)
            {
                Audit.Log.Error("DirectoryExists::ArgumentException - {0}", argumentException);
            }
            catch (NotSupportedException notSupportedException)
            {
                Audit.Log.Error("DirectoryExists::NotSupportedException - {0}", notSupportedException);
            }

            return result;
        }


        /// <summary>
        ///     Files the pattern exists.
        /// </summary>
        /// <param name="folderPath">The file path.</param>
        /// <param name="fileNamePattern">The file name pattern.</param>
        /// <returns></returns>
        public static bool FilePatternExists(string folderPath, string fileNamePattern = "*.*")
        {
            Audit.Log.DebugFormat("FilePatternExists called.");

            if (string.IsNullOrWhiteSpace(folderPath))
            {
                Audit.Log.ErrorFormat("FilePatternExists - folderPath parameter null or empty.");
                return false;
            }

            var result = default(bool);
            try
            {
                result = Directory.GetFiles(folderPath, fileNamePattern).Any();
                Audit.Log.DebugFormat("FilePatternExists - Complete - found: {0}", result);
            }
            catch (IOException ioException)
            {
                Audit.Log.Error("DirectoryExists::IOException - {0}", ioException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Audit.Log.Error("DirectoryExists::UnauthorizedAccessException - {0}", unauthorizedAccessException);
            }
            catch (ArgumentException argumentException)
            {
                Audit.Log.Error("DirectoryExists::ArgumentException - {0}", argumentException);
            }

            return result;
        }

        /// <summary>
        /// Reads the data from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static string ReadDataFromFile(string filePath)
        {
            Audit.Log.DebugFormat("ReadDataFromFile called - filePath: {0}.", filePath);

            if (string.IsNullOrWhiteSpace(filePath))
            {
                Audit.Log.DebugFormat(
                    "ReadDataFromFile called - filePath parameter null or empty, or data size is zero.");
                return null;
            }

            var result = default(string);

            try
            {
                //check directory
                if (!DirectoryExists(Path.GetDirectoryName(filePath)))
                {
                    Audit.Log.Error("ReadDataFromFile - ERROR: filePath directory does not exist.");
                    return null;
                }

                //be sure source location exists but not where it will be copied or moved
                if (!File.Exists(filePath))
                {
                    Audit.Log.Error("ReadDataFromFile - ERROR: filePath file does not exist.");
                    return null;
                }

                result = File.ReadAllText(filePath);
                Audit.Log.DebugFormat("ReadDataFromFile - Read completed - result size: {0}", result.Length);
            }
            catch (DirectoryNotFoundException directoryNotFoundException)
            {
                Audit.Log.Error("ReadDataFromFile::DirectoryNotFoundException - {0}", directoryNotFoundException);
            }
            catch (IOException ioException)
            {
                Audit.Log.Error("ReadDataFromFile::IOException - {0}", ioException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Audit.Log.Error("ReadDataFromFile::UnauthorizedAccessException - {0}", unauthorizedAccessException);
            }
            catch (SecurityException securityException)
            {
                Audit.Log.Error("ReadDataFromFile::SecurityException - {0}", securityException);
            }
            catch (ArgumentException argumentException)
            {
                Audit.Log.Error("ReadDataFromFile::ArgumentException - {0}", argumentException);
            }
            catch (NotSupportedException notSupportedException)
            {
                Audit.Log.Error("ReadDataFromFile::NotSupportedException - {0}", notSupportedException);
            }

            return result;
        }


        /// <summary>
        ///     Reads the CVS file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <returns></returns>
        public static string ReadCvsFileIntoJsonFile(string sourcePath, string destinationPath)
        {
            Audit.Log.DebugFormat("ReadCvsFileIntoJsonFile - Called for filePath: {0}", sourcePath);

            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(destinationPath))
            {
                Audit.Log.Error(
                    "ReadCvsFileIntoJsonFile - ERROR: sourcePath parameter null or empty, or destinationPath parameter null or empty.");
                return null;
            }

            var result = default(string);
            try
            {
                //check directory
                var destDirExists = DirectoryExists(Path.GetDirectoryName(destinationPath));
                if (!destDirExists)
                {
                    destDirExists = DirectoryExists(destinationPath, true);
                }

                var srcDirExists = DirectoryExists(Path.GetDirectoryName(sourcePath));
                if (!srcDirExists || !destDirExists)
                {
                    Audit.Log.Error(
                        "ReadCvsFileIntoJsonFile - ERROR: sourcePath or destinationPath directory does not exist.");
                    return null;
                }

                //be sure source location exists but not where it will be copied or moved
                if (!File.Exists(sourcePath))
                {
                    Audit.Log.Error("ReadCvsFileIntoJsonFile - ERROR: sourcePath file does not exist.");
                    return null;
                }

                var json = default(string);

                #region Old

                //SOURCE FILE
                using (var sr = new StreamReader(@sourcePath))
                {
                    // DESTINATION FILE
                    using (var sw = new StreamWriter(@destinationPath))
                    {
                        //begin read of file
                        var line = sr.ReadLine();

                        if (line != null)
                        {
                            //split into columns, temp-save as table
                            line = line.Replace("\"", "").Trim();
                            var cols = Regex.Split(line, @",");

                            var table = new DataTable();
                            var temp = 0;

                            string[] columnNames = table.Columns.Cast<DataColumn>().
                                Select(column => column.ColumnName).
                                ToArray();
                            foreach (var col in cols)
                            {
                                var newCol = col;
                                if (string.IsNullOrWhiteSpace(col))
                                    newCol = "temp" + temp++;
                                table.Columns.Add(newCol, typeof (string));
                            }


                            while ((line = sr.ReadLine()) != null)
                            {
                                table.Rows.Clear();

                                int i;
                                line = line.Replace("\"", "").Trim();
                                var rows = Regex.Split(line, @",");
                                var dr = table.NewRow();

                                for (i = 0; i < rows.Length; i++)
                                {
                                    if (string.IsNullOrWhiteSpace(rows[i])) continue;

                                    dr[i] = rows[i];
                                }
                                if (dr.Table != null)
                                    table.Rows.Add(dr);

                                if (table.Rows.Count == 0) continue;

                                //all data read, convert table into json
                                json = JsonConvert.SerializeObject(table, Formatting.None);
#if DEBUG
                                Trace.TraceInformation("Converting cvs file to json: {0}", json);
#endif
                                sw.Write(json);
                            }
                        }
                        sw.Close();
                    }
                    sr.Close();
                }

                #endregion old

                //complete
                var success = File.Exists(destinationPath);

                if (!success)
                {
                    Audit.Log.Error(
                        "ReadCvsFileIntoJsonFile - ERROR: destinationPath file does not exist, read and save of file failed.");
                    return null;
                }
                Audit.Log.DebugFormat("ReadCvsFileIntoJsonFile - Completed read of cvs file into json file: {0}",
                    destinationPath);

                result = json;
            }
            catch (IOException ioException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::IOException - {0}", ioException);
            }
            catch (ArgumentException argumentException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::ArgumentException - {0}", argumentException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::UnauthorizedAccessException - {0}",
                    unauthorizedAccessException);
            }
            catch (SecurityException securityException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::SecurityException - {0}", securityException);
            }
            catch (OutOfMemoryException outOfMemoryException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::OutOfMemoryException - {0}", outOfMemoryException);
            }
            catch (RegexMatchTimeoutException regexMatchTimeoutException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::RegexMatchTimeoutException - {0}", regexMatchTimeoutException);
            }
            catch (DuplicateNameException duplicateNameException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::DuplicateNameException - {0}", duplicateNameException);
            }
            catch (InvalidExpressionException invalidExpressionException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::InvalidExpressionException - {0}", invalidExpressionException);
            }
            catch (InvalidConstraintException invalidConstraintException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::InvalidConstraintException - {0}", invalidConstraintException);
            }
            catch (InvalidCastException invalidCastException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::InvalidCastException - {0}", invalidCastException);
            }
            catch (IndexOutOfRangeException indexOutOfRangeException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::IndexOutOfRangeException - {0}", indexOutOfRangeException);
            }
            catch (DeletedRowInaccessibleException deletedRowInaccessibleException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::DeletedRowInaccessibleException - {0}",
                    deletedRowInaccessibleException);
            }
            catch (NoNullAllowedException noNullAllowedException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::NoNullAllowedException - {0}", noNullAllowedException);
            }
            catch (ConstraintException constraintException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::ConstraintException - {0}", constraintException);
            }
            catch (ObjectDisposedException objectDisposedException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::ObjectDisposedException - {0}", objectDisposedException);
            }
            catch (NotSupportedException notSupportedException)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::NotSupportedException - {0}", notSupportedException);
            }
            catch (Exception ex)
            {
                Audit.Log.Error("ReadCvsFileIntoJsonFile::Exception (undetermined) - {0}", ex);
            }

            return result;
        }


        /// <summary>
        ///     Merges the json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json1">The json1.</param>
        /// <param name="json2">The json2.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static T MergeJson<T>(string json1, string json2)
        {
            Audit.Log.DebugFormat("DVSProvider::MergeJson - Called");

            if (string.IsNullOrWhiteSpace(json1) || string.IsNullOrWhiteSpace(json2))
            {
                Audit.Log.DebugFormat("DVSProvider::MergeJson - one or both json data string null; nothing to merge");
                return default(T);
            }

            if (json1 == null) throw new ArgumentNullException();

            var o1 = JObject.Parse(json1);
            var o2 = JObject.Parse(json2);

            if (o1 == null) return default(T);

            o1.Merge(o2, new JsonMergeSettings
            {
                // union array values together to avoid duplicates
                MergeArrayHandling = MergeArrayHandling.Union
            });

            var parsed = JObject.FromObject(o1);

            var result = JsonConvert.DeserializeObject<T>(parsed.ToString());

            return result;
        }

        /// <summary>
        ///     Copies the or move file.
        /// </summary>
        /// <param name="sourcePath">The source path.</param>
        /// <param name="destinationPath">The destination path.</param>
        /// <param name="createCopy">if set to <c>true</c> [create copy].</param>
        /// <returns></returns>
        public static bool CopyOrMoveFile(string sourcePath, string destinationPath, bool createCopy = false)
        {
            Audit.Log.DebugFormat("CopyOrMoveFile::{0} called - sourcePath: {1}\r\ndestinationPath: {2}",
                (createCopy ? "Copy" : "Move"), sourcePath, destinationPath);

            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(destinationPath))
            {
                Audit.Log.Error(
                    "CopyOrMoveFile - ERROR: sourcePath parameter null or empty, or destinationPath parameter null or empty.");
                return false;
            }

            var result = default(bool);
            try
            {
                //check directory
                if (!DirectoryExists(sourcePath) || !DirectoryExists(destinationPath))
                {
                    Audit.Log.Error(
                        "CopyOrMoveFile - ERROR: sourcePath directory does not exist, or destinationPath directory already exists.");
                    return false;
                }

                //be sure source location exists but not where it will be copied or moved
                if (!File.Exists(sourcePath) || File.Exists(destinationPath))
                {
                    Audit.Log.Error(
                        "CopyOrMoveFile - ERROR: sourcePath file does not exist, or destinationPath file already exists.");
                    return false;
                }

                if (createCopy)
                {
                    File.Copy(sourcePath, destinationPath);
                    result = File.Exists(sourcePath) && File.Exists(destinationPath);
                    Audit.Log.ErrorFormat("CopyOrMoveFile - Copy completed successfully: {0}", result);
                }
                else
                {
                    File.Move(sourcePath, destinationPath);
                    result = !File.Exists(sourcePath) && File.Exists(destinationPath);
                    Audit.Log.ErrorFormat("CopyOrMoveFile - Move completed successfully: {0}", result);
                }
            }
            catch (IOException ioException)
            {
                Audit.Log.Error("CopyOrMoveFile::IOException - {0}", ioException);
            }
            catch (UnauthorizedAccessException unauthorizedAccessException)
            {
                Audit.Log.Error("CopyOrMoveFile::UnauthorizedAccessException - {0}", unauthorizedAccessException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                Audit.Log.Error("CopyOrMoveFile::ArgumentNullException - {0}", argumentNullException);
            }
            catch (ArgumentException argumentException)
            {
                Audit.Log.Error("CopyOrMoveFile::ArgumentException - {0}", argumentException);
            }
            catch (NotSupportedException notSupportedException)
            {
                Audit.Log.Error("CopyOrMoveFile::NotSupportedException - {0}", notSupportedException);
            }

            return result;
        }
    }
}