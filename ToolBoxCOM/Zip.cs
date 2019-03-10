using System;
using System.IO;
using System.Runtime.InteropServices;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Tar;
using ExcelDna.Integration;

namespace ToolBoxCOM
{
    [ComVisible(true)]
    [ClassInterface(ClassInterfaceType.AutoDispatch)]
    public class Zip
    {
        [ExcelFunction(Description = "Extract .gzip")]
        public void ExtractGZipSample(string gzipFileName, string targetDir)
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

    }
}
