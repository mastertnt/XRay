﻿/*************************************************************************************

   Extended WPF Toolkit

   Copyright (C) 2007-2013 Xceed Software Inc.

   This program is provided to you under the terms of the Microsoft Public
   License (Ms-PL) as published at http://wpftoolkit.codeplex.com/license 

   For more features, controls, and fast professional support,
   pick up the Plus Edition at http://xceed.com/wpf_toolkit

   Stay informed: follow @datagrid on Twitter or Like http://facebook.com/datagrids

  ***********************************************************************************/

using System;
using System.Windows;

namespace XControls
{
  public class ShortUpDown : ANativeNumericUpDown<short>
  {
    #region Constructors

    static ShortUpDown()
    {
      UpdateMetadata( typeof( ShortUpDown ), ( short )1, short.MinValue, short.MaxValue );
    }

    public ShortUpDown()
      : base( Int16.Parse, Decimal.ToInt16, ( v1, v2 ) => v1 < v2, ( v1, v2 ) => v1 > v2 )
    {
    }

    #endregion //Constructors

    #region Base Class Overrides

    protected override short CustomIncrementValue( short value, short increment )
    {
      return ( short )( value + increment );
    }

    protected override short CustomDecrementValue( short value, short increment )
    {
      return ( short )( value - increment );
    }

    #endregion //Base Class Overrides
  }
}
