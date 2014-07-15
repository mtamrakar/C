using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using System.Linq;

namespace Resources
{
    class Program
    {
        private const String NAMESPACE = "Resources.";
        private const String JSON_EXT = ".json";


        static void Main(string[] args)
        {


            var data = GetObjectList<TestObject>();
            var data2 = GetObjectList<TestObject2>();

            var data3 = GetObject<TestObject3>();

            var data4 = GetObject<TestObject>();




            var json = GetJson<TestObject>();
            var json2 = GetJson<TestObject2>();
            var json3 = GetJson<TestObject3>();


        }


        private static String LoadJson(String objectName)
        {
            try
            {
                var jsonFile = String.Format("{0}{1}", objectName, JSON_EXT);
                using (var r = new StreamReader(jsonFile))
                {
                    return (r.ReadToEnd());
                }
            }
            catch
            {
                //Add error logging
                throw;
            }
        }

        /// <summary>
        /// Gets the json.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static String GetJson<T>()
        {
            var type = typeof(T);
            return LoadJson(type.Name);
        }


        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetObject<T>()
        {
            var type = typeof(T);

            try
            {               
                var serializer = new JavaScriptSerializer();
                var jsonString = GetJson<T>();

                return IsJsonArray(jsonString) ? GetObjectList<T>().First() : (T)serializer.Deserialize(jsonString, type);
            }
            catch
            {
                return (T)Convert.ChangeType(null, type);
                //Add Error Logging
            }
        }

        /// <summary>
        /// Gets the object list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<T> GetObjectList<T>()
        {
            try
            {
                var type = typeof(T);
                var serializer = new JavaScriptSerializer();
                Type list = typeof(List<>).MakeGenericType(type);
                return (List<T>)serializer.Deserialize(GetJson<T>(), list);
            }
            catch
            {
                return null;
                //Add Error Logging
            }
        }

        /// <summary>
        /// Gets the object list.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static List<T> GetObjectList<T>(List<T> defaultValue)
        {
            return GetObjectList<T>() ?? defaultValue;
        }

        /// <summary>
        /// Determines whether [is json array] [the specified json string].
        /// </summary>
        /// <param name="jsonString">The json string.</param>
        /// <returns></returns>
        public static Boolean IsJsonArray(String jsonString)
        {
            return jsonString.StartsWith("[");
        }

       
    }
}
