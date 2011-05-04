using System;
using Newtonsoft.Json;

namespace Tests
{   
    public static class ObjectDumperExtension
    {

        public static void Dump(this object obj, int depth = 0) {
            var dump = JsonConvert.SerializeObject(obj, Formatting.Indented);
            Console.WriteLine(dump);
        }
    }
}