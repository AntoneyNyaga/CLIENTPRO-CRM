using CLIENTPRO_CRM.Module.BusinessObjects;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace CLIENTPRO_CRM.Module.BusinessObjects.CustomerService
{
    [DefaultClassOptions]
    [ImageName("FunctionsInformation")]
    [NavigationItem("Customer Service & Settings")]

    public class KnowledgeBase : BaseObject
    {
        public KnowledgeBase(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        public string Name { get; set; }
        public Topic Topic { get; set; }
        [Size(4096)]
        public string Summary { get; set; }

    }

    public class Topic : BaseObject
    {
        public Topic(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }
        public string Name { get; set; }
        public string ParentTopic { get; set; }
        public string Description { get; set; }

        ApplicationUser assignedTo;

        [Association("ApplicationUser-Topics")]
        public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }


    }
}