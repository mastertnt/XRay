using System;
using System.Linq;
using System.Collections;
using System.Reflection;
using System.Xml.Linq;
using XSystem;
using System.Collections.Generic;

namespace XSerialization.Collections
{
    /// <summary>
    /// This class defines a serialization contract for object list.
    /// </summary>
    public class ListSerializationContract : ATypeSerializationContract<IList>
    {
        /// <summary>
        /// Flag to know if an external creation is necessary.
        /// </summary>
        public override bool NeedCreate
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Determines whether this instance can manage the specified object type.
        /// </summary>
        /// <param name="pObjectType">The object type.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public override SupportPriority CanManage(Type pObjectType, IXSerializationContext pSerializationContext)
        {
            if (this.SupportedType.IsAssignableFrom(pObjectType))
            {
                return new SupportPriority(SupportLevel.Interface, pObjectType.DistanceTo(this.SupportedType));
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// Determines whether this instance can manage the specified object.
        /// </summary>
        /// <param name="pObject">The object.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns></returns>
        public override SupportPriority CanManage(object pObject, IXSerializationContext pSerializationContext)
        {
            if (pObject != null)
            {
                return this.CanManage(pObject.GetType(), pSerializationContext);
            }
            return SupportPriority.CANNOT_SUPPORT;
        }

        /// <summary>
        /// This method reads the specified element.
        /// </summary>
        /// <param name="pObjectToInitialize">The object to initialize</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>
        /// <returns>The initialized object if the input object is valid.</returns>
        public override object Read(object pObjectToInitialize, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            if (pObjectToInitialize != null)
            {
                IList lList = pObjectToInitialize as IList;
                if (lList != null)
                {
                    foreach (XElement lChild in pParentElement.Elements(XConstants.ITEM_TAG))
                    {
                        Type[] lInterfaces = lList.GetType().GetInterfaces();
                        Type lGenericListType = lInterfaces.FirstOrDefault(pType => pType.IsGenericType == true && pType.GetGenericTypeDefinition() == typeof(IList<>));

                        IXSerializationContract lContract = pSerializationContext.SelectContract(lChild, lGenericListType.GetGenericArguments()[0]);
                        if (lContract != null)
                        {
                            object lItem = null;
                            try
                            {
                                if (lGenericListType.GetGenericArguments()[0].IsValueType)
                                {
                                    lItem = Activator.CreateInstance(lGenericListType.GetGenericArguments()[0]);
                                }
                                else
                                {
                                    ConstructorInfo lDefaultConstructor = lGenericListType.GetGenericArguments()[0].GetConstructor(Type.EmptyTypes);
                                    if (lDefaultConstructor != null)
                                    {
                                        lItem = Activator.CreateInstance(lGenericListType.GetGenericArguments()[0]);
                                    }
                                }
                            }
                            catch
                            {
                                
                            }
                            lItem = lContract.Read(lItem, lChild, pSerializationContext);
                            lList.Add(lItem);
                        }

                    } 
                }
            }
            return pObjectToInitialize;
        }

        /// <summary>
        /// This method writes the specified object.
        /// </summary>
        /// <param name="pObject">The object to serialize.</param>
        /// <param name="pParentElement">The parent element.</param>
        /// <param name="pSerializationContext">The serialization context.</param>lList.GetType().GetGenericArguments()[0]
        /// <returns>The modified parent element</returns>
        public override XElement Write(object pObject, XElement pParentElement, IXSerializationContext pSerializationContext)
        {
            // Store the type.
            pParentElement.Add(pSerializationContext.ReferenceType(pObject.GetType()));

            // Store all items.
            IList lList = pObject as IList;
            foreach (object lItem in lList)
            {
                XElement lItemElement = new XElement(XConstants.ITEM_TAG);
                IXSerializationContract lContract = pSerializationContext.SelectContract(lItemElement, lItem);
                if (lContract != null)
                {
                    lContract.Write(lItem, lItemElement, pSerializationContext);
                }
                pParentElement.Add(lItemElement);
            }
            return pParentElement;
        }
    }
}
