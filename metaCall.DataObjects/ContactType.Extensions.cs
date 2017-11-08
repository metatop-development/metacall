using System;
using System.Collections.Generic;
using System.Text;

namespace metatop.Applications.metaCall.DataObjects
{
    public partial class ContactType
    {
        public static readonly Guid NichtErreichbarId = new Guid("{93626933-6E3B-4B6E-8E6F-08389D9B4484}");
        public static readonly Guid AdresseDoppeltId = new Guid("{4D966B34-9C91-424B-A051-186A521332A8}");
        public static readonly Guid GespraechMoeglichId = new Guid("{AECB8B6E-FFDC-4862-9C9D-592F77A4178A}");
        public static readonly Guid BesetztId = new Guid("{386C6E5B-2505-44C3-9E85-94AD24FB8FDA}");
        public static readonly Guid AdresseNichtGeeignetId = new Guid("{F2426419-76B0-4759-AE83-B63A23AD2AA0}");
        public static readonly Guid NummerFalschId = new Guid("{D0829FF3-F5AF-42A6-B2F5-CC24CA8E55E6}");
        public static readonly Guid AnrufbeantworterId = new Guid("{5F430989-617F-46BE-B0FA-E4059657F501}");

        public static readonly Guid DurringAnrufbeantworterId = new Guid("{4C85E252-D8B5-40FC-87A2-9F96F2496644}");
        public static readonly Guid DurringGespraechMoeglichId = new Guid("{1CA3376D-AD49-401C-AD5C-E4910F4BCA64}");
        public static readonly Guid DurringBesetztId = new Guid("{3A7DA119-E248-48CC-9FC1-1B01A2492B44}");
        public static readonly Guid DurringNichtErreichbarId = new Guid("{8D66CDEF-0988-4BFB-8D76-C9CF47E82622}");
        public static readonly Guid DurringKeineZahlungId = new Guid("{7C2D5277-2C5F-4B29-A4A4-1A9CAD260BB6}");

    }
}
