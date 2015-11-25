using System;
using System.Collections.ObjectModel;

namespace XTreeListView.TestApp.Model
{
    /// <summary>
    /// This class defines a person.
    /// </summary>
    /// <!-- DPE -->
	public class Person
    {
        #region Fields

        /// <summary>
        /// Stores the id used to identify a person.
        /// </summary>
        private static Int32 msId;

        #endregion // Fields.   
 
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
		{
            this.Id = ++Person.msId;
            this.Children = new ObservableCollection<Person>();
        }

        #endregion // Constructor.

        #region Properties

        /// <summary>
        /// Gets or sets the person name.
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Gets or sets the person id.
        /// </summary>
        public Int32 Id { get; set; }

        /// <summary>
        /// Gets or sets the children list of this person.
        /// </summary>
        public ObservableCollection<Person> Children { get; private set; }

        /// <summary>
        /// Gets or sets the number of item to ad when benching the add performance.
        /// </summary>
        public int NbItemToAdd { get; private set; }

        #endregion // Properties.

        #region Methods

        /// <summary>
        /// Returns the current object as string.
        /// </summary>
        /// <returns>The string description of the object.</returns>
        public override String ToString()
		{
			return this.Name;
        }

        /// <summary>
        /// Creates a test model.
        /// </summary>
        /// <param name="pCount1">The number of item to create at the first level.</param>
        /// <param name="pCount2">The number of item to create at the second level.</param>
        /// <param name="pCount3">The number of item to create at the third level.</param>
        /// <returns>The model.</returns>
        public static Person CreateTestModel(Int32 pCount1, Int32 pCount2, Int32 pCount3)
        {
            Person lModel = new Person();
            for
                (Int32 lIter1 = 0; lIter1 < pCount1; lIter1++)
            {
                Person lP1 = new Person() { Name = "Person A" + lIter1.ToString() };
                lModel.Children.Add(lP1);
                for
                    (Int32 lIter2 = 0; lIter2 < pCount2; lIter2++)
                {
                    Person lP2 = new Person() { Name = "Person B" + lIter2.ToString() };
                    lP1.Children.Add(lP2);
                    for
                        (Int32 lIter3 = 0; lIter3 < pCount3; lIter3++)
                    {
                        lP2.Children.Add(new Person() { Name = "Person C" + lIter3.ToString() });
                    }
                }
            }

            return lModel;
        }

        /// <summary>
        /// Creates the model for the expand or remove test.
        /// </summary>
        public static Person CreateFullTestModel()
        {
            Person lRoot = new Person() { Name = "Root" };

            Person lPerson1 = Person.CreateTestModel(100, 0, 0);
            lPerson1.Name = "100 children";
            lPerson1.NbItemToAdd = 100;
            lRoot.Children.Add(lPerson1);

            Person lPerson2 = Person.CreateTestModel(250, 0, 0);
            lPerson2.Name = "250 children";
            lPerson1.NbItemToAdd = 250;
            lRoot.Children.Add(lPerson2);

            Person lPerson3 = Person.CreateTestModel(500, 0, 0);
            lPerson3.Name = "500 children";
            lPerson1.NbItemToAdd = 500;
            lRoot.Children.Add(lPerson3);

            Person lPerson4 = Person.CreateTestModel(1000, 0, 0);
            lPerson4.Name = "1000 children";
            lPerson1.NbItemToAdd = 1000;
            lRoot.Children.Add(lPerson4);

            Person lPerson5 = Person.CreateTestModel(2000, 0, 0);
            lPerson5.Name = "2000 children";
            lPerson1.NbItemToAdd = 2000;
            lRoot.Children.Add(lPerson5);

            Person lPerson6 = Person.CreateTestModel(10000, 0, 0);
            lPerson6.Name = "10000 children";
            lPerson1.NbItemToAdd = 10000;
            lRoot.Children.Add(lPerson6);

            return lRoot;
        }

        /// <summary>
        /// Creates an empty model.
        /// </summary>
        public static Person CreateEmptyTestModel()
        {
            Person lRoot = new Person() { Name = "Root" };
            lRoot.Children.Add(new Person() { Name = "100 children", NbItemToAdd = 100 });
            lRoot.Children.Add(new Person() { Name = "250 children", NbItemToAdd = 250 });
            lRoot.Children.Add(new Person() { Name = "500 children", NbItemToAdd = 500 });
            lRoot.Children.Add(new Person() { Name = "1000 children", NbItemToAdd = 1000 });
            lRoot.Children.Add(new Person() { Name = "2000 children", NbItemToAdd = 2000 });

            return lRoot;
        }

        #endregion // Methods.
    }
}