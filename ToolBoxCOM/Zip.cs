﻿using System;
using System.IO;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Tar;
using ExcelDna.Integration;

namespace ToolBoxCOM
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class Zip
    {
        [ExcelFunction(Description = "Extract .gzip")]
        public void ExtractGZip(string gzipFileName, string targetDir)
        {
            // Use a 4K buffer. Any larger is a waste.    
            byte[] dataBuffer = new byte[4096];

            using (System.IO.Stream fs = new FileStream(gzipFileName, FileMode.Open, FileAccess.Read))
            {
                using (GZipInputStream gzipStream = new GZipInputStream(fs))
                {
                    // Change this to your needs
                    string fnOut = Path.Combine(targetDir, Path.GetFileNameWithoutExtension(gzipFileName));

                    using (FileStream fsOut = File.Create(fnOut))
                    {
                        StreamUtils.Copy(gzipStream, fsOut, dataBuffer);
                    }
                }
            }
        }

        [ExcelFunction(Description = "Extract .tar.gzip")]
        // example: ExtractTGZ(@"c:\temp\test.tar.gz", @"C:\DestinationFolder")
        public void ExtractTGZ(String gzArchiveName, String destFolder)
        {
            Stream inStream = File.OpenRead(gzArchiveName);
            Stream gzipStream = new GZipInputStream(inStream);

            TarArchive tarArchive = TarArchive.CreateInputTarArchive(gzipStream);
            tarArchive.ExtractContents(destFolder);
            tarArchive.Close();

            gzipStream.Close();
            inStream.Close();
        }

        [ExcelFunction(Description = "Extract zip")]
        // example: ExtractTGZ(@"c:\temp\test.tar.gz", @"C:\DestinationFolder")
        public void ExtractZip(String zipFileName, String targetDir)
        {
            FastZip fastZip = new FastZip();
            string fileFilter = null;

            // Will always overwrite if target filenames already exist
            fastZip.ExtractZip(zipFileName, targetDir, fileFilter);

        }

        [ExcelFunction(Description = "Create zip")]
        
        public void CreateZip(string directory, string outputFile, string password ="")
        {
            ZipOutputStream OutputStream = new ZipOutputStream(File.Create(outputFile));
                    OutputStream.Finish();

            OutputStream.Close();
            var fullFileListing = Directory.EnumerateFiles(directory, "*.*", SearchOption.AllDirectories);
            var directories = Directory.EnumerateDirectories(directory, "*", SearchOption.AllDirectories);

            using (var zip = new ZipFile(outputFile))
            {
                zip.UseZip64 = UseZip64.On;

                foreach (var childDirectory in directories)
                {
                    zip.BeginUpdate();
                    zip.AddDirectory(childDirectory.Replace(directory, string.Empty));
                    zip.CommitUpdate();
                }

                foreach (var file in fullFileListing)
                {
                    if (!(file == outputFile))
                    {
                        zip.BeginUpdate();
                        zip.Add(file, file.Replace(directory, string.Empty));
                        zip.CommitUpdate();
                    }
                }

                if (password != "")
                {
                    zip.BeginUpdate();
                    zip.Password = password;
                    zip.CommitUpdate();
                }
            }
        }
    }
}
