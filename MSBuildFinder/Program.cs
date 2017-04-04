using System;
using System.Runtime.InteropServices.ComTypes;
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

                Console.WriteLine($@"Instance: {instance.GetDisplayName()}
    Version: {version}
    State: {state}
    Path: {instance.GetInstallationPath()}
    Description: {instance.GetDescription()}
    Installation name: {instance.GetInstallationName()}
    Instance Id: {instance.GetInstanceId()}
    Install Date: {GetDateTime(instance.GetInstallDate())}");

            } while (fetched > 0);
        }
        catch
        {
        }
    }

    private static object GetDateTime(FILETIME time)
    {
        long highBits = time.dwHighDateTime;
        highBits = highBits << 32;
        return DateTime.FromFileTimeUtc(highBits + (uint)time.dwLowDateTime);
    }
}

