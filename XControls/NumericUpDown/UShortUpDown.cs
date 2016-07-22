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
    public class UShortUpDown : ANativeNumericUpDown<ushort>
  {
    #region Constructors

    static UShortUpDown()
    {
      UpdateMetadata( typeof( UShortUpDown ), ( ushort )1, ushort.MinValue, ushort.MaxValue );
    }

    public UShortUpDown()
      : base( ushort.Parse, Decimal.ToUInt16, ( v1, v2 ) => v1 < v2, ( v1, v2 ) => v1 > v2 )
    {
    }

    #endregion //Constructors

    #region Base Class Overrides

    protected override ushort CustomIncrementValue( ushort value, ushort increment )
    {
      return ( ushort )( value + increment );
    }

    protected override ushort CustomDecrementValue( ushort value, ushort increment )
    {
      return ( ushort )( value - increment );
    }

    #endregion //Base Class Overrides
  }
}
