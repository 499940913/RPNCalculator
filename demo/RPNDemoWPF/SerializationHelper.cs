

using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace RPNDemoWPF
{
    public static class SerializationHelper
    {
        public static bool Serialize<T>(this T source, string path) where T : new()
        {
            FileStream fs=null;
            try
            {
                using (fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    fs.SetLength(0);
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(fs,source);
                    fs.Flush();
                    fs.Seek(0, SeekOrigin.Begin);
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return true;
        }

        public static T DeSerialize<T>(string path) where T:class
        {
            FileStream fs = null;
            object res;
            try
            {
                using (fs=new FileStream(path,FileMode.Open))
                {
                    fs.Seek(0, SeekOrigin.Begin);
                    using (var textreader=new XmlTextReader(fs))
                    {
                        var xmlSerializer = new XmlSerializer(typeof(T));
                        res=xmlSerializer.Deserialize(textreader);
                    }
                }
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                }
            }
            return res as T;
        }
    }
}
