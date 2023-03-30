﻿using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects
{
    //[DefaultClassOptions]
    [DefaultProperty("FullAddress")]
    [Persistent("Address")]

    public class Address : BaseObject
    { 
        public Address(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        //Contact contact;
        private String defaultFullAddressFormat = "{Country}, {StateProvince}, {City}, {Street}, {ZipPostal}";
        string country;
        string zipPostal;
        string stateProvince;
        string city;
        string street;

        [RuleRequiredField("RuleRequiredField for Address.Street", DefaultContexts.Save)]
        public string Street
        {
            get => street;
            set => SetPropertyValue(nameof(Street), ref street, value);
        }


        [RuleRequiredField("RuleRequiredField for Address.City", DefaultContexts.Save)]
        public string City
        {
            get => city;
            set => SetPropertyValue(nameof(City), ref city, value);
        }


        [RuleRequiredField("RuleRequiredField for Address.StateProvince", DefaultContexts.Save)]
        public string StateProvince
        {
            get => stateProvince;
            set => SetPropertyValue(nameof(StateProvince), ref stateProvince, value);
        }


        [RuleRequiredField("RuleRequiredField for Address.ZipPostal", DefaultContexts.Save)]
        public string ZipPostal
        {
            get => zipPostal;
            set => SetPropertyValue(nameof(ZipPostal), ref zipPostal, value);
        }


        [RuleRequiredField("RuleRequiredField for Address.Country", DefaultContexts.Save)]
        public string Country
        {
            get => country;
            set => SetPropertyValue(nameof(Country), ref country, value);
        }

        //[Browsable(false)]
        public String FullAddress
        {
            get
            {
                return ObjectFormatter.Format(defaultFullAddressFormat, this, EmptyEntriesMode.RemoveDelimiterWhenEntryIsEmpty);
            }
        }
        /*[Browsable(false)]
        public IList<Contact> Contact { get; set; } = new ObservableCollection<Contact>();

        [Browsable(false)]
        public IList<Lead> Lead { get; set; } = new ObservableCollection<Lead>();*/


        /*[Association("Contact-Addresses")]
        public Contact Contact
        {
            get => contact;
            set => SetPropertyValue(nameof(Contact), ref contact, value);
        }*/
        [Browsable(false)]
        public IList<Quote> Quote { get; set; } = new ObservableCollection<Quote>();
    }
}