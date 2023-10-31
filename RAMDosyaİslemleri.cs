using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreadDiziOlusturma
{
    internal class RAMDosyaİslemleri
    {
        MemoryStream memoryStream;
        public void WriteToRAM(byte[] array)
        {
            memoryStream = new MemoryStream(array);

            memoryStream.Write(array, 0, array.Length);
        }

        public void ReadFromRAM()
        {
            byte[] array = memoryStream.ToArray();

        }
       

    }
}
