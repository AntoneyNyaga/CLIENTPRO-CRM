using System;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    public interface BasicIPerson
    {
        string FirstName { get; set; }

        string LastName { get; set; }

        string MiddleName { get; set; }

        DateTime Birthday { get; set; }

        string FullName { get; }

        string Email { get; set; }

        void SetFullName(string fullName);
    }
}