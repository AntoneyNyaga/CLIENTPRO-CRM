using System;
using System.ComponentModel;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.Basics
{
    [MapInheritance(MapInheritanceType.OwnTable)]
    [DefaultProperty("DisplayName")]
    public abstract class BasicParty : BaseObject
    {
        /*int id;
        [Key(true)]

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public int Id
        {
            get { return id; }
            set { SetPropertyValue(nameof(Id), ref id, value); }
        }*/
        private BasicAddress address;

        private BasicAddress address2;

        [Size(-1)]
        [Delayed(true)]
        [ImageEditor]
        public byte[] Photo
        {
            get
            {
                return GetDelayedPropertyValue<byte[]>("Photo");
            }
            set
            {
                SetDelayedPropertyValue("Photo", value);
            }
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public BasicAddress Address
        {
            get
            {
                return address;
            }
            set
            {
                SetPropertyValue("Address", ref address, value);
            }
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        public BasicAddress Address2
        {
            get
            {
                return address2;
            }
            set
            {
                SetPropertyValue("Address2", ref address2, value);
            }
        }

        [ObjectValidatorIgnoreIssue(new Type[]
        {
            typeof(ObjectValidatorDefaultPropertyIsVirtual),
            typeof(ObjectValidatorDefaultPropertyIsNonPersistentNorAliased)
        })]
        public string DisplayName => GetDisplayName();

        [Aggregated]
        [Association("BasicParty-BasicPhoneNumbers")]
        public XPCollection<BasicPhoneNumber> PhoneNumbers => GetCollection<BasicPhoneNumber>("PhoneNumbers");

        protected BasicParty(Session session)
            : base(session)
        {
        }

        public override string ToString()
        {
            return DisplayName;
        }

        protected abstract string GetDisplayName();
    }
}