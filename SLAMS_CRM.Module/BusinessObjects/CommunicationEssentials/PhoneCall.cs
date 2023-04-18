using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects.CommunicationEssentials
{
    //[DefaultClassOptions]
    //[NavigationItem("Inbox")]

    public class PhoneCall : Communication
    { 
        public PhoneCall(Session session)
            : base(session)
        {
            Type = CommunicationType.Phone;

        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private string subject;

        public string GetSubject()
        {
            return subject;
        }

        public void SetSubject(string value)
        {
            subject = value;
        }

        public string Description { get; set; }

        public DateTime StartOn { get; set; }

        public DateTime EndOn { get; set; }

        public Contact Participant { get; set; }
    }
}