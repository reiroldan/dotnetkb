using System;
using System.Collections.Generic;
using DotNetKillboard;
using TinyIoC;

namespace Tests
{
    public class TestResolver : IResolver
    {

        private readonly TinyIoCContainer _container;

        public TestResolver() {
            _container = new TinyIoCContainer();
        }

        public void Register(Type type, object instance) {
            _container.Register(type, instance);
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
            _container.TryResolve(type, ResolveOptions.Default, out result);
            return result;
        }

        public IEnumerable<object> TryResolveAll(Type type) {
            return _container.ResolveAll(type);            
        }
    }
}