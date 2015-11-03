using System;
using System.Reflection;
using XGraph.ViewModels;

namespace XGraphTestApp
{
    /// <summary>
    /// This view model represents a type.
    /// </summary>
    class TypeNodeViewModel : NodeViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeNodeViewModel"/> class.
        /// </summary>
        /// <param name="pType">Type of the port.</param>
        public TypeNodeViewModel(Type pType)
        {
            foreach (PropertyInfo lPropertyInfo in pType.GetProperties())
            {
                PortViewModel lPort = null;
                
                if (lPropertyInfo.CanRead && lPropertyInfo.CanWrite)
                {
                    lPort = new PortViewModel {Direction = PortDirection.Both};
                }
                else if (lPropertyInfo.CanRead)
                {
                    lPort = new PortViewModel {Direction = PortDirection.Output};
                }
                else
                {
                    lPort = new PortViewModel {Direction = PortDirection.Input, DisplayString = lPropertyInfo.Name};
                }
                lPort.DisplayString = lPropertyInfo.Name;
                lPort.PortType = "Property";
                this.Ports.Add(lPort);
            }

            foreach (EventInfo lEventInfo in pType.GetEvents())
            {
                PortViewModel lPort = new PortViewModel {Direction = PortDirection.Output, PortType = "Event", DisplayString = lEventInfo.Name};
                this.Ports.Add(lPort);
            }
        }
        
    }
}
