using System;
using System.Collections.Generic;
using DotNetKillboard;
using TinyIoC;

namespace Tests
{
    public class TestResolver : IResolver
    {
        public TestResolver() {
            Container = new TinyIoCContainer();
        }

        public TinyIoCContainer Container { get; private set; }

        public void Register(Type type, object instance) {
            Container.Register(type, instance);
        }

        public object Resolve(Type type) {
            var result = TryResolve(type);

            if (result != null)
                return result;

            throw new ArgumentException(string.Format("type not registered {0}", type), "type");
        }

        public IEnumerable<object> ResolveAll(Type type) {
            var all = TryResolveAll(type);

            if (all != null)
                return all;

            throw new ArgumentException(string.Format("type not registered {0}", type), "type");
        }

        public object TryResolve(Type type) {
            object result;
            Container.TryResolve(type, ResolveOptions.Default, out result);
            return result;
        }

        public IEnumerable<object> TryResolveAll(Type type) {
            return Container.ResolveAll(type);
        }
    }
}