using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;


namespace ThreadDiziOlusturma
{
    class Program
    {

        static void RunThread(ThreadStart threadStart, out long elapsedTime)
        {
            
            Stopwatch stopwatch = new Stopwatch();            
            Thread thread = new Thread(threadStart);
            
            stopwatch.Start();
            thread.Start();
            
            thread.Join();
            stopwatch.Stop();

            elapsedTime = stopwatch.ElapsedMilliseconds;
        }

        static void Main(string[] args)
        {
            // HDD üzerinden dosya yazma ve okuma
            ThreadStart[] hddThreadStarts = new ThreadStart[]
            {
                DiskYonetimi.WriteToHDD,
                DiskYonetimi.WriteToHDD,
                DiskYonetimi.ReadFromHDD,
                DiskYonetimi.ReadFromHDD
            };

            // HDD üzerinden dosya yazma ve okuma işlemlerinin sürelerini tutan bir dizi 
            long[] hddElapsedTimes = new long[hddThreadStarts.Length];
           
            for (int i = 0; i < hddThreadStarts.Length; i++)
            {
               
                RunThread(hddThreadStarts[i], out hddElapsedTimes[i]);
            }

            // HDD üzerinden dosya yazma ve okuma işlemlerinin sürelerini ekrana yazdırma
            for (int i = 0; i < hddElapsedTimes.Length; i++)
            {
                Console.WriteLine("HDD üzerinden dosya {0} işlemi: {1} ms", i % 2 == 0 ? "yazma" : "okuma", hddElapsedTimes[i]);
            }

            // HDD üzerinden okunan 4 diziyi bir diziye toplama işlemleri
            byte[] ReadArrayFromHDD(string filePath)
            {
                // Dosyadan okunacak bayt sayısı (1 MB * 100(daha fazlasında RAMDosyaİslemlerinde kapasite hatası aldım)
                int byteCount = 1024 * 1024 *100;

                int bytesRead;

                byte[] array = new byte[byteCount];

                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    bytesRead = fileStream.Read(array, 0, byteCount);
                }

                if (bytesRead < byteCount)
                {
                    Array.Resize(ref array, bytesRead);
                }

                return array;
            }

            // HDD üzerinden okunan 4 diziyi bir diziye toplamak için dosya yolu oluşturduk
            string hddfilePath = @"C:\Users\Abdullah\Desktop\İşletim Sistemleri Dersi\ThreadDiziOlusturma\ThreadDiziOlusturma\bin\Debug\data.txt";

            
            Stopwatch stopwatchTotal = new Stopwatch();

            stopwatchTotal.Start();

            byte[] totalArray = new byte[0];

            for (int i = 0; i < 4; i++)
            {
                byte[] array = ReadArrayFromHDD(hddfilePath);

                Array.Resize(ref totalArray, totalArray.Length + array.Length);

                Array.Copy(array, 0, totalArray, totalArray.Length - array.Length, array.Length);
            }

            stopwatchTotal.Stop();
            Console.WriteLine("HDD üzerinden 4 dizinin toplanma süresi: {0} ms", stopwatchTotal.ElapsedMilliseconds);
            Console.WriteLine("------------------\n HDD üzerindeki yazılan veriler birleştirilerek RAM de oluşturulan diziye aktarıldı\n----------------");
            Console.WriteLine("RAM de oluşturulan toplam dizinin yazma ve okuma süreleri aşağıda yer almaktadır");

            RAMDosyaİslemleri ramDosyaIslemleri = new RAMDosyaİslemleri();

            Stopwatch stopwatchRAMWrite = new Stopwatch();

            stopwatchRAMWrite.Start();
            ramDosyaIslemleri.WriteToRAM(totalArray);

            stopwatchRAMWrite.Stop();
            Console.WriteLine(">RAM üzerine dosya yazma işlemi: {0} ms", stopwatchRAMWrite.ElapsedMilliseconds);

            Stopwatch stopwatchRAMRead = new Stopwatch();

            stopwatchRAMRead.Start();
            ramDosyaIslemleri.ReadFromRAM();
            stopwatchRAMRead.Stop();
            Console.WriteLine(">RAM'den dosya okuma işlemi: {0} ms", stopwatchRAMRead.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }

}

