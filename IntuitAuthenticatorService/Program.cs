using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace IntuitAuthenticatorService
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceAddress = new Uri("http://localhost:8080/authenticator");

            // Create the ServiceHost.
            using(var host = new WebServiceHost(typeof(AuthenticatorService), serviceAddress))
            {
                ServiceEndpoint ep = host.AddServiceEndpoint(typeof(IAuthenticatorService), new WebHttpBinding(), "");
                //var smb = new ServiceMetadataBehavior();
                //smb.HttpGetEnabled = true;
                //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
                //host.Description.Behaviors.Add(smb);

                host.Open();

                Log.WriteLine($"The service is ready at {serviceAddress}", outputToConsole: true);
                Log.WriteLine("Press return to stop the service...", outputToConsole: true);
                Console.ReadLine();

                host.Close();

                Log.WriteLine($"The service is closed.\n\n", outputToConsole: true);
            }
        }
    }
}
