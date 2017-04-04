using System;
using Microsoft.VisualStudio.Setup.Configuration;

class Program
{
    static void Main(string[] args)
    {
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
                if (fetched <= 0)
                {
                    break;
                }

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

                Console.WriteLine($"{instance.GetDisplayName()} {version} {state} {instance.GetInstallationPath()} ");

                // If the install was complete and a valid version, consider it.
                if (state == InstanceState.Complete)
                {
                }
            } while (fetched > 0);
        }
        catch
        {
        }
    }
}

