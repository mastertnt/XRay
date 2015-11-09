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
            this.DisplayString = pType.Name;
            this.Description = "A class sample node.";
            foreach (PropertyInfo lPropertyInfo in pType.GetProperties())
            {
                if (lPropertyInfo.CanWrite)
                {
                    PortViewModel lPort = null;
                    lPort = new PortViewModel {Direction = PortDirection.Input, DisplayString = lPropertyInfo.Name, PortType = "Property"};
                    this.Ports.Add(lPort);
                }
                
                if (lPropertyInfo.CanRead)
                {
                    PortViewModel lPort = null;
                    lPort = new PortViewModel { Direction = PortDirection.Output, DisplayString = lPropertyInfo.Name, PortType = "Property" };
                    this.Ports.Add(lPort);
                }
            }

            foreach (EventInfo lEventInfo in pType.GetEvents())
            {
                PortViewModel lPort = new PortViewModel {Direction = PortDirection.Output, PortType = "Event", DisplayString = lEventInfo.Name};
                this.Ports.Add(lPort);
            }
        }
        
    }
}
