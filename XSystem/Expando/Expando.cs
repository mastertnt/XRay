using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;

namespace XSystem.Expando
{
    /// <summary>
    ///     Class that provides extensible properties and methods to an
    ///     existing object when cast to dynamic. This
    ///     dynamic object stores 'extra' properties in a dictionary or
    ///     checks the actual properties of the instance passed via
    ///     constructor.
    ///     This class can be subclassed to extend an existing type or
    ///     you can pass in an instance to extend. Properties (both
    ///     dynamic and strongly typed) can be accessed through an
    ///     indexer.
    ///     This type allows you three ways to access its properties:
    ///     Directly: any explicitly declared properties are accessible
    ///     Dynamic: dynamic cast allows access to dictionary and native properties/methods
    ///     Dictionary: Any of the extended properties are accessible via IDictionary interface
    ///     Source : https://weblog.west-wind.com/posts/2012/feb/08/creating-a-dynamic-extensible-c-expando-object
    /// </summary>
    [Serializable]
    public class Expando : DynamicObject, IDynamicMetaObjectProvider
    {
        private PropertyInfo[] _InstancePropertyInfo;

        /// <summary>
        ///     Instance of object passed in
        /// </summary>
        private object Instance;

        /// <summary>
        ///     Cached type of the instance
        /// </summary>
        private Type InstanceType;


        /// <summary>
        ///     String Dictionary that contains the extra dynamic values
        ///     stored on this object/instance
        /// </summary>
        /// <remarks>Using PropertyBag to support XML Serialization of the dictionary</remarks>
        public Dictionary<string, object> Properties = new Dictionary<string, object>();

        /// <summary>
        ///     This constructor just works off the internal dictionary and any
        ///     public properties of this object.
        ///     Note you can subclass Expando.
        /// </summary>
        public Expando()
        {
            this.Initialize(this);
        }

        /// <summary>
        ///     Allows passing in an existing instance variable to 'extend'.
        /// </summary>
        /// <remarks>
        ///     You can pass in null here if you don't want to
        ///     check native properties and only check the Dictionary!
        /// </remarks>
        /// <param name="instance"></param>
        public Expando(object instance)
        {
            this.Initialize(instance);
        }

        /// <summary>
        ///     Create an Expando from a dictionary
        /// </summary>
        /// <param name="dict"></param>
        public Expando(IDictionary<string, object> dict)
        {
            var expando = this;

            this.Initialize(expando);

            this.Properties = new Dictionary<string, object>();
            foreach (var kvp in dict)
            {
                var kvpValue = kvp.Value;
                if (kvpValue is IDictionary<string, object>)
                {
                    var expandoVal = new Expando(kvpValue);
                    expando[kvp.Key] = expandoVal;
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any string-object dictionaries
                    // along the way into expando objects
                    var objList = new List<object>();
                    foreach (var item in (ICollection) kvp.Value)
                    {
                        var itemVals = item as IDictionary<string, object>;
                        if (itemVals != null)
                        {
                            var expandoItem = new Expando(itemVals);
                            objList.Add(expandoItem);
                        }
                        else
                        {
                            objList.Add(item);
                        }
                    }

                    expando[kvp.Key] = objList;
                }
                else
                {
                    expando[kvp.Key] = kvpValue;
                }
            }
        }

        private PropertyInfo[] InstancePropertyInfo
        {
            get
            {
                if (this._InstancePropertyInfo == null && this.Instance != null)
                {
                    this._InstancePropertyInfo = this.Instance.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly);
                }

                return this._InstancePropertyInfo;
            }
        }


        /// <summary>
        ///     Convenience method that provides a string Indexer
        ///     to the Properties collection AND the strongly typed
        ///     properties of the object by name.
        ///     // dynamic
        ///     exp["Address"] = "112 nowhere lane";
        ///     // strong
        ///     var name = exp["StronglyTypedProperty"] as string;
        /// </summary>
        /// <remarks>
        ///     The getter checks the Properties dictionary first
        ///     then looks in PropertyInfo for properties.
        ///     The setter checks the instance properties before
        ///     checking the Properties dictionary.
        /// </remarks>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                try
                {
                    // try to get from properties collection first
                    return this.Properties[key];
                }
                catch (KeyNotFoundException)
                {
                    // try reflection on instanceType
                    object result = null;
                    if (this.GetProperty(this.Instance, key, out result))
                    {
                        return result;
                    }

                    // nope doesn't exist
                    throw;
                }
            }
            set
            {
                if (this.Properties.ContainsKey(key))
                {
                    this.Properties[key] = value;
                    return;
                }

                // check instance for existance of type first
                var miArray = this.InstanceType.GetMember(key, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
                if (miArray != null && miArray.Length > 0)
                {
                    this.SetProperty(this.Instance, key, value);
                }
                else
                {
                    this.Properties[key] = value;
                }
            }
        }

        /// <summary>
        ///     Initializes the instance
        /// </summary>
        /// <param name="instance"></param>
        protected void Initialize(object instance)
        {
            this.Instance = instance;
            if (instance != null)
            {
                this.InstanceType = instance.GetType();
            }
        }


        /// <summary>
        ///     Return both instance and dynamic names.
        ///     Important to return both so JSON serialization with
        ///     Json.NET works.
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            foreach (var prop in this.GetProperties(true))
            {
                yield return prop.Key;
            }
        }


        /// <summary>
        ///     Try to retrieve a member by name first from instance properties
        ///     followed by the collection entries.
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = null;

            // first check the Properties collection for member
            if (this.Properties.Keys.Contains(binder.Name))
            {
                result = this.Properties[binder.Name];
                return true;
            }


            // Next check for Public properties via Reflection
            if (this.Instance != null)
            {
                try
                {
                    return this.GetProperty(this.Instance, binder.Name, out result);
                }
                catch
                {
                }
            }

            // failed to retrieve a property
            result = null;
            return false;
        }


        /// <summary>
        ///     Property setter implementation tries to retrieve value from instance
        ///     first then into this object
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            // first check to see if there's a native property to set
            if (this.Instance != null)
            {
                try
                {
                    var result = this.SetProperty(this.Instance, binder.Name, value);
                    if (result)
                    {
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            // no match - set or add to dictionary
            this.Properties[binder.Name] = value;
            return true;
        }

        /// <summary>
        ///     Dynamic invocation method. Currently allows only for Reflection based
        ///     operation (no ability to add methods dynamically).
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (this.Instance != null)
            {
                try
                {
                    // check instance passed in for methods to invoke
                    if (this.InvokeMethod(this.Instance, binder.Name, args, out result))
                    {
                        return true;
                    }
                }
                catch
                {
                }
            }

            result = null;
            return false;
        }


        /// <summary>
        ///     Reflection Helper method to retrieve a property
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool GetProperty(object instance, string name, out object result)
        {
            if (instance == null)
            {
                instance = this;
            }

            var miArray = this.InstanceType.GetMember(name, BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance);
            if (miArray != null && miArray.Length > 0)
            {
                var mi = miArray[0];
                if (mi.MemberType == MemberTypes.Property)
                {
                    result = ((PropertyInfo) mi).GetValue(instance, null);
                    return true;
                }
            }

            result = null;
            return false;
        }

        /// <summary>
        ///     Reflection helper method to set a property value
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected bool SetProperty(object instance, string name, object value)
        {
            if (instance == null)
            {
                instance = this;
            }

            var miArray = this.InstanceType.GetMember(name, BindingFlags.Public | BindingFlags.SetProperty | BindingFlags.Instance);
            if (miArray != null && miArray.Length > 0)
            {
                var mi = miArray[0];
                if (mi.MemberType == MemberTypes.Property)
                {
                    ((PropertyInfo) mi).SetValue(this.Instance, value, null);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///     Reflection helper method to invoke a method
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="name"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        protected bool InvokeMethod(object instance, string name, object[] args, out object result)
        {
            if (instance == null)
            {
                instance = this;
            }

            // Look at the instanceType
            var miArray = this.InstanceType.GetMember(name, BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.Instance);

            if (miArray != null && miArray.Length > 0)
            {
                var mi = miArray[0] as MethodInfo;
                result = mi.Invoke(this.Instance, args);
                return true;
            }

            result = null;
            return false;
        }


        /// <summary>
        ///     Returns and the properties of
        /// </summary>
        /// <param name="includeInstanceProperties"></param>
        /// <returns></returns>
        public IEnumerable<KeyValuePair<string, object>> GetProperties(bool includeInstanceProperties = false)
        {
            if (includeInstanceProperties && this.Instance != null)
            {
                foreach (var prop in this.InstancePropertyInfo)
                {
                    yield return new KeyValuePair<string, object>(prop.Name, prop.GetValue(this.Instance, null));
                }
            }

            foreach (var key in this.Properties.Keys)
            {
                yield return new KeyValuePair<string, object>(key, this.Properties[key]);
            }
        }


        /// <summary>
        ///     Checks whether a property exists in the Property collection
        ///     or as a property on the instance
        /// </summary>
        /// <param name="item"></param>
        /// <param name="includeInstanceProperties"></param>
        /// <returns></returns>
        public bool Contains(KeyValuePair<string, object> item, bool includeInstanceProperties = false)
        {
            var res = this.Properties.ContainsKey(item.Key);
            if (res)
            {
                return true;
            }

            if (includeInstanceProperties && this.Instance != null)
            {
                foreach (var prop in this.InstancePropertyInfo)
                {
                    if (prop.Name == item.Key)
                    {
                        return true;
                    }
                }
            }

            return false;
        }


        /// <summary>
        ///     Converts an <see cref="System.Collections.IDictionary" /> into an <see cref="Expando" />
        /// </summary>
        /// <returns>
        ///     <see cref="Expando" />
        /// </returns>
        public static Expando ToIndexableExpando(IDictionary<string, object> dict)
        {
            var expando = new Expando();


            foreach (var kvp in dict)
            {
                var kvpValue = kvp.Value as IDictionary<string, object>;
                if (kvpValue != null)
                {
                    var expandoVal = ToIndexableExpando(kvpValue);
                    expando[kvp.Key] = expandoVal;
                }
                else if (kvp.Value is ICollection)
                {
                    // iterate through the collection and convert any string-object dictionaries
                    // along the way into expando objects
                    var objList = new List<object>();
                    foreach (var item in (ICollection) kvp.Value)
                    {
                        var itemVals = item as IDictionary<string, object>;
                        if (itemVals != null)
                        {
                            var expandoItem = ToIndexableExpando(itemVals);
                            objList.Add(expandoItem);
                        }
                        else
                        {
                            objList.Add(item);
                        }
                    }

                    expando[kvp.Key] = objList;
                }
                else
                {
                    expando[kvp.Key] = kvp.Value;
                }
            }

            return expando;
        }
    }
}