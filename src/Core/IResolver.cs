using System;
using System.Collections.Generic;

namespace DotNetKillboard
{
    public interface IResolver
    {
        object Resolve(Type type);

        IEnumerable<object> ResolveAll(Type type);

        object TryResolve(Type type);

        IEnumerable<object> TryResolveAll(Type type);

    }

    public static class ResolverExtensions
    {
        public static T Resolve<T>(this IResolver resolver) {
            var type = typeof(T);
            return (T)resolver.Resolve(type);
        }

        public static T TryResolve<T>(this IResolver resolver) {
            var type = typeof(T);
            return (T)resolver.TryResolve(type);
        }
    }
}