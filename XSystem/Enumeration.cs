// ***********************************************************************
// Assembly         : XApp.Core
// Author           : nbaudrey
// Created          : 12-13-2018
//
// Last Modified By : nbaudrey
// Last Modified On : 12-13-2018
// ***********************************************************************
// <copyright file="Enumeration.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XSystem
{
    /// <summary>
    ///     Base class for all enumeration.
    ///     Implements the <see cref="System.IComparable" />
    /// </summary>
    /// <seealso cref="System.IComparable" />
    public abstract class Enumeration : IComparable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Enumeration" /> class.
        /// </summary>
        protected Enumeration()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:XSystem.Enumeration" /> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        protected Enumeration(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get;
        }

        /// <summary>
        ///     Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public int Id
        {
            get;
        }

        /// <summary>
        ///     Compares to.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns>System.Int32.</returns>
        public int CompareTo(object other)
        {
            return this.Id.CompareTo(((Enumeration) other).Id);
        }

        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        ///     Get all enumeration values.
        /// </summary>
        /// <typeparam name="T">Type of the enumerated object</typeparam>
        /// <returns>All values.</returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        /// <summary>Determines whether the specified <see cref="System.Object"/> is equal to this instance.</summary>
        /// <param name="obj">Objet à comparer à l'objet actuel.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            var otherValue = obj as Enumeration;

            if (otherValue == null)
            {
                return false;
            }

            var typeMatches = this.GetType() == obj.GetType();
            var valueMatches = this.Equals(otherValue);

            return typeMatches && valueMatches;
        }

        /// <summary>
        /// Equalses the specified other.
        /// </summary>
        /// <param name="other">The other.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        protected bool Equals(Enumeration other)
        {
            return string.Equals(this.Name, other.Name) && this.Id == other.Id;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^ this.Id;
            }
        }
    }
}