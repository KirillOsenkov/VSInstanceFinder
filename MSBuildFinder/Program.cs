using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Setup.Configuration;

namespace MSBuildFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var validInstances = new List<VisualStudioInstance>();

            try
            {
                var query = new SetupConfiguration();
                var e = query.EnumAllInstances();

                int fetched;
                var instances = new ISetupInstance[1];
                do
                {
                    // Call e.Next to query for the next instance (single item or nothing returned).
                    e.Next(1, instances, out fetched);
                    if (fetched <= 0) break;

                    var instance = instances[0];
                    var state = ((ISetupInstance2)instance).GetState();
                    Version version;

                    try
                    {
                        version = new Version(instance.GetInstallationVersion());
                    }
                    catch (FormatException)
                    {
                        continue;
                    }

                    // If the install was complete and a valid version, consider it.
                    if (state == InstanceState.Complete)
                    {
                        validInstances.Add(new VisualStudioInstance(
                            instance.GetDisplayName(),
                            instance.GetInstallationPath(),
                            version));
                    }
                } while (fetched > 0);
            }
            catch
            {
            }

            foreach (var instance in validInstances)
            {
                Console.WriteLine($"{instance.Name} {instance.Path} {instance.Version}");
            }
        }
    }

    /// <summary>
    /// Wrapper class to represent an installed instance of Visual Studio.
    /// </summary>
    internal class VisualStudioInstance
    {
        /// <summary>
        /// Version of the Visual Studio Instance
        /// </summary>
        internal Version Version { get; }

        /// <summary>
        /// Path to the Visual Studio installation
        /// </summary>
        internal string Path { get; }

        /// <summary>
        /// Full name of the Visual Studio instance with SKU name
        /// </summary>
        internal string Name { get; }

        internal VisualStudioInstance(string name, string path, Version version)
        {
            Name = name;
            Path = path;
            Version = version;
        }
    }
}
