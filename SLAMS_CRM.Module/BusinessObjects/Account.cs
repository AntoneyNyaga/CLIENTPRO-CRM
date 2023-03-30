using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Pdf.Native.BouncyCastle.Utilities.IO.Pem;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraReports.ErrorPanel.Native;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]

    public class Account : BaseObject
    {
        public Account(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).

        }


        bool isNewObject;
        DateTime modifiedOn;
        DateTime createdOn;
        double annualRevenue;
        string industryType;
        string accountType;
        string description;
        string officePhone;
        string emailAddress;
        string website;
        string name;

        [Size(50)]
        public string Name { get => name; set => SetPropertyValue(nameof(Name), ref name, value); }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Website { get => website; set => SetPropertyValue(nameof(Website), ref website, value); }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string EmailAddress
        {
            get => emailAddress;
            set => SetPropertyValue(nameof(EmailAddress), ref emailAddress, value);
        }


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string OfficePhone
        {
            get => officePhone;
            set => SetPropertyValue(nameof(OfficePhone), ref officePhone, value);
        }

        public Address ShippingAddress;


        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Description
        {
            get => description;
            set => SetPropertyValue(nameof(Description), ref description, value);
        }


        [Browsable(false)]
        public int AccountType
        {
            get => accountType == null ? 0 : (int)Enum.Parse(typeof(AccountType), accountType);
            set { SetPropertyValue(nameof(AccountType), ref accountType, Enum.GetName(typeof(AccountType), value)); }
        }

        [NotMapped]
        public AccountType Type { get => (AccountType)AccountType; set => AccountType = (int)value; }


        public double AnnualRevenue
        {
            get => annualRevenue;
            set => SetPropertyValue(nameof(AnnualRevenue), ref annualRevenue, value);
        }


        [Browsable(false)]
        public int IndustryType
        {
            get => industryType == null ? 0 : (int)Enum.Parse(typeof(IndustryType), industryType);
            set { SetPropertyValue(nameof(IndustryType), ref industryType, Enum.GetName(typeof(IndustryType), value)); }
        }

        [NotMapped]
        public IndustryType Industry { get => (IndustryType)IndustryType; set => IndustryType = (int)value; }

        [Editable(false)]
        [ReadOnly(false)]
        public DateTime CreatedOn
        {
            get => createdOn;
            set => SetPropertyValue(nameof(CreatedOn), ref createdOn, value);
        }

        [Editable(false)]
        [ReadOnly(true)]
        public DateTime ModifiedOn
        {
            get => modifiedOn;
            set => SetPropertyValue(nameof(ModifiedOn), ref modifiedOn, value);
        }

        //public bool IsNewObject { get; private set; }
        
        public bool IsNewObject
        {
            get => isNewObject;
            set => SetPropertyValue(nameof(IsNewObject), ref isNewObject, value);
        }

        protected override void OnSaving()
        {
            if(IsNewObject)
            {
                CreatedOn = DateTime.Now;
            }
            ModifiedOn = DateTime.Now;
            base.OnSaving();
        }
    }

    public enum AccountType
    {
        Analyst,
        Competitor,
        Customer,
        Integrator,
        Investor,
        partner,
        Press,
        Prospect,
        Reseller,
        Other
    }

    public enum IndustryType
    {
        Agriculture,
        Automotive,
        BankingAndFinance,
        Biotechnology,
        Chemicals,
        Construction,
        ConsumerGoods,
        Education,
        EnergyAndUtilities,
        EntertainmentAndMedia,
        HealthCare,
        HospitalityAndTourism,
        InformationTechnology,
        Insurance,
        Manufacturing,
        Mining,
        Pharmaceuticals,
        RealEstate,
        Retail,
        Telecommunications,
        TransportationAndLogistics
    }
}