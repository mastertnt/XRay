using System;
using System.Xml.Linq;
using XSerialization;
using XSerialization.Attributes;

namespace TestModels.TestContracts
{
    class FieldSerializationContract1 : FieldSerializationContract
    {
        public override XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            Console.WriteLine("FieldSerializationContract1 " + this.TypedAttribute.FieldName);
            return base.Write(pObject, pParentElement, pSerializationContext);
        }
    }
}
