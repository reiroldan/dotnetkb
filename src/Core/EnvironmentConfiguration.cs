using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace DotNetKillboard
{
    /// <summary>
    /// A configuration that can resolve requested instances.
    /// </summary>
    public interface IEnvironmentConfiguration
    {
        /// <summary>
        /// Tries to get the specified instance.
        /// </summary>
        /// <typeparam name="T">The type of the instance to get.</typeparam>
        /// <param name="result">The result.</param>
        /// <returns>A indication whether the instance could be get or not.</returns>
        Boolean TryGet<T>(out T result) where T : class;

        Boolean TryGet(Type type, out object result);
    }

    public class EnvironmentConfiguration
    {
        /// <summary>
        /// Holds the defaults for requested types that are not configured.
        /// </summary>
        /// <remarks>
        /// Use the <see cref="SetDefault{T}"/> method to set a default.
        /// </remarks>
        private static readonly IDictionary<Type, Object> Defaults = new Dictionary<Type, object>(0);

        /// <summary>
        /// Returns the current environment configuration
        /// </summary>
        /// <remarks>
        /// Returns the current environment configuration, or null if not configured
        /// </remarks>
        public static IEnvironmentConfiguration CurrentConfiguration { get; private set; }

        /// <summary>
        /// Configures the initial SisoEnvironment
        /// </summary>
        static EnvironmentConfiguration() {
            InitializeDefaults();
        }

        /// <summary>
        /// Initialize defaults with default components.
        /// </summary>
        private static void InitializeDefaults() {
            // Initialize sane defaults.
        }

        /// <summary>
        /// Gets or create the requested instance specified by the parameter <typeparamref name="T"/>.
        /// </summary>
        /// <remarks>This</remarks>
        /// <typeparam name="T">The type of the instance that is requested.
        /// </typeparam>
        /// <returns>If the type specified by <typeparamref name="T"/> is
        /// registered, it returns an instance that is, is a super class of, or
        /// implements the type specified by <typeparamref name="T"/>. Otherwise
        /// a <see cref="SisoDbException"/>
        /// occurs.
        /// </returns>
        public static T Get<T>() where T : class {
            T result = null;

            if (CurrentConfiguration == null || !CurrentConfiguration.TryGet(out result)) {
                object defaultResult;

                if (Defaults.TryGetValue(typeof(T), out defaultResult)) {
                    result = (T)defaultResult;
                }
            }

            // Try to construct the type            
            if (result == null) {
                result = (T)ConstructType(typeof(T));
            }

            if (result == null) {
                var msg = String.Format("Could not find requested type {0} in the environment configuration. Make sure that " +
                                 "the Environment is configured correctly or that defaults are correctly set.", typeof(T).FullName);

                throw new ConfigurationException(msg);
            }

            return result;
        }

        public static object TryGet(Type type) {
            object result = null;

            if (CurrentConfiguration != null && CurrentConfiguration.TryGet(type, out result)) {
                return result;
            }

            object defaultResult;

            if (Defaults.TryGetValue(type, out defaultResult)) {
                result = defaultResult;
            }

            return result;
        }

        /// <summary>
        /// Sets the default for an type. This default instance is returned when
        /// the configured <see cref="IEnvironmentConfiguration"/> did not
        /// returned an instance for this type.
        /// </summary>
        /// <remarks>When the type already contains a default, it is overridden.
        /// </remarks>
        /// <typeparam name="T">The type of the instance to set a default.
        /// </typeparam>
        /// <param name="instance">The instance to set as default.</param>
        public static void SetDefault<T>(T instance) where T : class {
            Defaults[typeof(T)] = instance;
        }

        /// <summary>
        /// Removes the default for specified type.
        /// </summary>
        /// <remarks>When there is no default set, this action is ignored.</remarks>
        /// <typeparam name="T">The registered default type.</typeparam>
        public static void RemoveDefault<T>() where T : class {
            Defaults.Remove(typeof(T));
        }

        /// <summary>
        /// Configures the environment.
        /// </summary>
        /// <param name="source">The source that contains the configuration for the current environment.</param>
        public static void Configure(IEnvironmentConfiguration source) {
            CurrentConfiguration = source;
        }

        /// <summary>
        /// When the environment is configured it removes the configuration. Defaults are reconfigured.
        /// </summary>
        public static void Deconfigure() {
            CurrentConfiguration = null;
            Defaults.Clear();
            InitializeDefaults();
        }

        /// <summary>
        /// Gets a value indicating whether this environment is configured.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this environment is configured; otherwise, <c>false</c>.
        /// </value>
        public static Boolean IsConfigured {
            get {
                return CurrentConfiguration != null;
            }
        }

        private static object ConstructType(Type type) {
            var constructor = GetBestConstructor(type);

            if (constructor == null)
                throw new ConfigurationException(string.Format("Could not construct type {0}", type));

            var ctorParams = constructor.GetParameters();
            var args = new object[ctorParams.Count()];

            for (var i = 0; i < ctorParams.Count(); i++) {
                var currentParam = ctorParams[i];

                args[i] = TryGet(currentParam.ParameterType);
            }

            try {
                return constructor.Invoke(args);
            } catch (Exception ex) {
                throw new ConfigurationException(ex.Message);
            }
        }

        private static ConstructorInfo GetBestConstructor(Type type) {
            if (type.IsValueType)
                return null;

            // Get constructors in reverse order based on the number of parameters
            // i.e. be as "greedy" as possible so we satify the most amount of dependencies possible
            var ctors = type.GetConstructors().OrderByDescending(ctor => ctor.GetParameters().Count());

            foreach (var ctor in ctors) {
                if (CanConstruct(ctor))
                    return ctor;
            }

            return null;
        }

        private static bool CanConstruct(ConstructorInfo ctor) {
            foreach (var parameter in ctor.GetParameters()) {
                if (parameter.ParameterType.IsPrimitive)
                    return false;

                var instance = TryGet(parameter.ParameterType);

                if (instance == null)
                    return false;
            }

            return true;
        }
    }

    public class ConfigurationException : Exception
    {
        public ConfigurationException(string message)
            : base(message) {
        }
    }
}