using System;

namespace DotNetKillboard.Data
{
    public static class CollectionNamesFactory
    {
        public static string GetCollectionNameFromType<T>() {
            return GetCollectionNameFromType(typeof(T));
        }

        public static string GetCollectionNameFromType(Type type) {
            return type.Name;
        }
    }
}