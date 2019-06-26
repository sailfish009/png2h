using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Drawing;

namespace png2h
{
    public class convert 
    {
        private byte[] img2arr(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms,imageIn.RawFormat);
                return  ms.ToArray();
            }
        }

        public bool run(string array_name="", string in_path="", string out_path="")
        {
            if (array_name == "" || in_path == "" || out_path == "")
                return false;

            using (var stream = File.OpenRead(in_path))
            using (var image = Image.FromStream(stream))
            using (System.IO.StreamWriter file = 
                new System.IO.StreamWriter(out_path))
            {
                file.WriteLine("static uint8_t " + array_name + "[] {");
                byte[] arr = img2arr(image);
                int array_length = arr.Length;
                for(int i=0; i< array_length; ++i)
                {
                    var hex = $"0x{arr[i]:X}";
                    if(i == (array_length-1) )
                    {
                        file.Write(hex);
                    }
                    else
                    {
                        file.Write(hex + ", ");
                    }
                }
                file.WriteLine("};");
                string endline = "static size_t " + array_name + "_size = " + array_length.ToString() + ";";
                file.Write(endline);
            }
            return true;
        }
    }
}
