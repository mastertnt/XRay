using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using XSystem.Collections;

namespace XSystem.TestApp
{
    class Program
    {
        /// <summary>
        /// Main entry point of the application.
        /// </summary>
        /// <param name="pArguments">The arguments.</param>
        static void Main(string[] pArguments)
        {
            ObservableCollection<int> lSourceCollection = new ObservableCollection<int>();
            ObservableCollection<string> lTargetCollection = new ObservableCollection<string>();
            ObservableCollectionSynchronizer<int, string> lSynchronizer = new ObservableCollectionSynchronizer<int, string>(lSourceCollection, lTargetCollection);

            Console.WriteLine("Add a single value in source");
            lSourceCollection.Add(12);
            Console.WriteLine(lSynchronizer.ToString());
            Debug.Assert(lTargetCollection.Count == lSourceCollection.Count);

            Console.WriteLine("Add multiple values in source");
            lSourceCollection.Add(17);
            lSourceCollection.Add(18);
            lSourceCollection.Add(12);
            lSourceCollection.Add(14);
            lSourceCollection.Add(14);
            Console.WriteLine(lSynchronizer.ToString());
            Debug.Assert(lTargetCollection.Count == lSourceCollection.Count);

            Console.WriteLine("Remove a value duplicated in target");
            lTargetCollection.Remove("12");
            Console.WriteLine(lSynchronizer.ToString());
            Debug.Assert(lTargetCollection.Count == lSourceCollection.Count);

            Console.WriteLine("Insert a single value in target");
            lTargetCollection.Insert(1, "34");
            Console.WriteLine(lSynchronizer.ToString());
            Debug.Assert(lTargetCollection.Count == lSourceCollection.Count);

            Console.WriteLine("Remove a value duplicated in source");
            lSourceCollection.Remove(12);
            Console.WriteLine(lSynchronizer.ToString());
            Debug.Assert(lTargetCollection.Count == lSourceCollection.Count);

            Console.WriteLine("Clear source");
            lSourceCollection.Clear();
            Console.WriteLine(lSynchronizer.ToString());
            Debug.Assert(lTargetCollection.Count == lSourceCollection.Count);

        }
    }
}
