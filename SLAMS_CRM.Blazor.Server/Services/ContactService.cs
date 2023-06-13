using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects.CustomerManagement;

namespace SLAMS_CRM.Blazor.Server.Services
{
    public interface IContactService
    {
        Task<List<Contact>> GetContacts();
    }

    public class ContactService : IContactService
    {
        private readonly IObjectSpaceProvider objectSpaceProvider;

        public ContactService(IObjectSpaceProvider objectSpaceProvider)
        {
            this.objectSpaceProvider = objectSpaceProvider;
        }

        public async Task<List<Contact>> GetContacts()
        {
            return await Task.Run(() =>
            {
                using (var objectSpace = objectSpaceProvider.CreateObjectSpace())
                {
                    var xpObjectSpace = (XPObjectSpace)objectSpace;
                    var unitOfWork = xpObjectSpace.Session as UnitOfWork;

                    var contacts = new XPQuery<Contact>(unitOfWork).OrderBy(c => c.Account.Name).ToList();
                    // Perform additional operations or filtering as needed

                    return contacts;
                }
            });
        }

    }
}
