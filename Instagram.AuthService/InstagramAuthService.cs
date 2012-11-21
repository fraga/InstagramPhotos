using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;

namespace Instagram.Photos.AuthService
{

    [ServiceContract(Namespace = "instagramService",
      Name = "instagramContract")]
    public interface IInstagramAuth
    {
        [OperationContract]
        [WebGet(UriTemplate = "?code={tag}")]
        string AuthGet(string codeGet);
    }

    public class InstagramAuthServiceProgram
    {
        public ServiceHost InstagramAuthService { get; set; }

        public InstagramAuthServiceProgram(string address)
        {
            Uri baseAddress = new Uri(address);
            InstagramAuthService = new ServiceHost(typeof(InstagramAuthService), baseAddress);
            ServiceEndpoint ep = InstagramAuthService.AddServiceEndpoint(typeof(IInstagramAuth), new WebHttpBinding(), "");
            ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
            smb.HttpGetEnabled = true;
            smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            smb.HttpGetBinding = ep.Binding;
            InstagramAuthService.Description.Behaviors.Add(smb);

        }

        public void Open()
        {
            if (InstagramAuthService != null)
                InstagramAuthService.Open();

        }

        public void Close()
        {
            if (InstagramAuthService != null)
                InstagramAuthService.Close();
        }
    }

    [ServiceBehavior(AddressFilterMode = AddressFilterMode.Prefix)]
    public class InstagramAuthService : IInstagramAuth
    {
        public string AuthGet(string codeGet)
        {
            return "is this working?";
        }
    }
}
