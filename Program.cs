using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
//using DocumentFormat.OpenXml.Spreadsheet;
using Aspose.Cells;
using Aspose.Cells.Utility;
using DocumentFormat.OpenXml.Drawing.Charts;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            List<FileDetails> filesList = new List<FileDetails>();

        DirectoryInfo directoryInfo = new DirectoryInfo(@"C:\Users\AlKhalilAlMaamari\Documents\Data Managment\Backup");
        FileInfo[] fileInfo = directoryInfo.GetFiles();

        FileDetails fi = new FileDetails();

        foreach (FileInfo f in fileInfo)
            {
                FileInfo file = new FileInfo(f.Name);
                string filePath = file.FullName;
                string fileHashCode = fi.GetHashCode(@"C:\Users\AlKhalilAlMaamari\Documents\Data Managment\Backup\"+  f.Name, new SHA512Cng());

                filesList.Add(new FileDetails { FileName = f.Name, FilePath = filePath, FileSize = f.Length/1024, FileHash = fileHashCode});

            }

            var json = System.Text.Json.JsonSerializer.Serialize(filesList);
            
            
            File.WriteAllText(@"C:\Users\AlKhalilAlMaamari\Documents\Data Managment\Json\Files Info.txt", json);
            string jsonString = File.ReadAllText(@"C:\Users\AlKhalilAlMaamari\Documents\Data Managment\Json\Files Info.txt");
            var filesList2 = JsonConvert.DeserializeObject<List<FileDetails>>(jsonString);
            foreach (var a in filesList2)
            {
                Console.WriteLine(a.FileName+" "+a.FilePath+" "+a.FileSize+" "+a.FileHash);
            }

            // Create a Workbook object
            Workbook workbook = new Workbook();
            Worksheet worksheet = workbook.Worksheets[0];

            // Read JSON File
            string jsonInput = File.ReadAllText(@"C:\Users\AlKhalilAlMaamari\Documents\Data Managment\Json\Files Info.txt");

            // Set JsonLayoutOptions
            JsonLayoutOptions options = new JsonLayoutOptions();
            options.ArrayAsTable = true;

            // Import JSON Data
            JsonUtility.ImportData(jsonInput, worksheet.Cells, 0, 0, options);

            // Save Excel file
            workbook.Save("Import-Data-JSON-To-Excel.xlsx");
            workbook.Save("Import-Data-JSON-To-Excel.csv");

            



        }
        


    }

    public class FileDetails
    {
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public long FileSize { get; set; }
        public string FileHash { get; set; }

        

        public string GetHashCode(string filePath, HashAlgorithm cryptoService)
        {
            
            using (cryptoService)
            {
                using (var fileStream = new FileStream(filePath,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.ReadWrite))
                {
                    var hash = cryptoService.ComputeHash(fileStream);
                    var hashString = Convert.ToBase64String(hash);
                    return hashString.TrimEnd('=');
                }
            }
        }



    }
}




   








