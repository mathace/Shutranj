﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Logic
{
    public static class ExtMet
    {
        /// <summary>
        /// Serializes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>A string representing serialized data</returns>
        public static string Serialize(this object obj)
        {
            //Check is object is serializable before trying to serialize
            if (obj.GetType().IsSerializable)
            {
                using (var stream = new MemoryStream())
                {
                    var serializer = new XmlSerializer(obj.GetType());
                    serializer.Serialize(stream, obj);
                    var bytes = new byte[stream.Length];
                    stream.Position = 0;
                    stream.Read(bytes, 0, bytes.Length);

                    return Encoding.UTF8.GetString(bytes);
                }
            }
            throw new NotSupportedException(string.Format("{0} is not serializable.", obj.GetType()));
        }

        /// <summary>
        /// Deserializes the specified serialized data.
        /// </summary>
        /// <param name="serializedData">The serialized data.</param>
        /// <returns></returns>
        public static T Deserialize<T>(this string serializedData)
        {
            var serializer = new XmlSerializer(typeof(T));
            var reader = new XmlTextReader(new StringReader(serializedData));

            return (T)serializer.Deserialize(reader);
        }
        public static string Obfuscate(this string data)
        {
            var bytes = Encoding.UTF8.GetBytes(data);
            for (int i = 0; i < bytes.Length; i++) bytes[i] ^= 0x5a;
            return Convert.ToBase64String(bytes);
        }
        public static string Deobfuscate(this string data)
        {
            var bytes = Convert.FromBase64String(data);
            for (int i = 0; i < bytes.Length; i++) bytes[i] ^= 0x5a;
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
