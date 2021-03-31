using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

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




   








