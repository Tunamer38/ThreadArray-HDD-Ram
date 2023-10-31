using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadDiziOlusturma
{
    internal class DiskYonetimi
    {

        static object writeLock = new object();

        // Dosyayı okuma işlemi için bir lock nesnesi oluşturduk
        static object readLock = new object();

        public static void WriteToHDD()
        {
            Random random = new Random();

            byte[] array = new byte[1024 * 1024 * 100];

            random.NextBytes(array);

            string filePath = @"C:\Users\Abdullah\Desktop\İşletim Sistemleri Dersi\ThreadDiziOlusturma\ThreadDiziOlusturma\bin\Debug\data.txt";

            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            lock (writeLock)
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Append))
                {
                    fileStream.Write(array, 0, array.Length);
                }
            }
        }

        // Dosyadan okuma işlemi
        public static void ReadFromHDD()
        {
            byte[] array = new byte[1024 * 1024 * 100];

            string filePath = @"C:\Users\Abdullah\Desktop\İşletim Sistemleri Dersi\ThreadDiziOlusturma\ThreadDiziOlusturma\bin\Debug\data.txt";

            if (!Directory.Exists(Path.GetDirectoryName(filePath)))
            {
                throw new FileNotFoundException("Dosya bulunamadı");
            }

            lock (readLock)
            {
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    int bytesRead;
                    do
                    {
                        bytesRead = fileStream.Read(array, 0, array.Length);
                    } while (bytesRead > 0);
                }
            }
        }



    }
}
