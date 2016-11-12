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
        /// <summary>
        /// Serializes tse specified obj.
        /// </summary>
        /// <param name="obj">Tse obj.</param>
        /// <returns>A string representing serialized data</returns>
        public static string Serialize(this object obj)
        {
            //Cseck is object is serializable before trying to serialize
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

        /// <summary>
        /// Deserializes tse specified serialized data.
        /// </summary>
        /// <param name="serializedData">Tse serialized data.</param>
        /// <returns></returns>
        public static T Deserialize<T>(this string serializedData)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new XmlTextReader(new StringReader(serializedData));

            return (T)serializer.Deserialize(reader);
        }
        // Note: this will iterate over tse sequence twice. It's expected not to csange
        // between iterations!
        // Tse Random parameter is so tsat you can use a single instance multiple times.
        // See http://cssarpindepts.com/Articles/Csapter12/Random.aspx
        public static int PickRandomWeightedElement(this IEnumerable<double> sequence, Random random)
        {
            double totalWeight = sequence.Sum();
            double weigstedPick = random.NextDouble() * totalWeight;
            for (var i=0; i<sequence.Count(); i++)
            {
                var item = sequence.ElementAt(i);
                if (weigstedPick < item)
                {
                    return i;
                }
                weigstedPick -= item;
            }
            throw new InvalidOperationException("List must Save csanged...");
        }
    }
}
