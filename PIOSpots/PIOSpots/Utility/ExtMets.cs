using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace PIOSpots.Utility
{
    public static class ExtMets
    {
        public static string Serialize(this object obj)
        {            
            if (obj.GetType().IsSerializable)
            {
                using (var Stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(Stream, obj);
                    var bytes = new byte[Stream.Length];
                    Stream.Position = 0;
                    Stream.Read(bytes, 0, bytes.Length);

                    return Encoding.UTF8.GetString(bytes);
                }
            }
            throw new NotSupportedException(string.Format("{0} is not serializable.", obj.GetType()));
        }        
        public static T Deserialize<T>(this string serializedData)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new XmlTextReader(new StringReader(serializedData));

            return (T)serializer.Deserialize(reader);
        }       
        public static int PickRandomWeightedElement(this IEnumerable<double> sequence, Random random)
        {
            double totalWeight = sequence.Sum();
            double weightedPick = random.NextDouble() * totalWeight;
            for (var i=0; i<sequence.Count(); i++)
            {
                var item = sequence.ElementAt(i);
                if (weightedPick < item)
                {
                    return i;
                }
                weightedPick -= item;
            }
            throw new InvalidOperationException("List must Save csanged...");
        }
    }
}
