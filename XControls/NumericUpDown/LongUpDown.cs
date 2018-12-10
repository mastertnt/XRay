using System;

namespace XControls.NumericUpDown
{
  public class LongUpDown : ANativeNumericUpDown<long>
  {
    #region Constructors

    static LongUpDown()
    {
      UpdateMetadata( typeof( LongUpDown ), 1L, long.MinValue, long.MaxValue );
    }

    public LongUpDown()
      : base( Int64.Parse, Decimal.ToInt64, ( v1, v2 ) => v1 < v2, ( v1, v2 ) => v1 > v2 )
    {
    }

    #endregion //Constructors

    #region Base Class Overrides

    protected override long CustomIncrementValue( long value, long increment )
    {
      return value + increment;
    }

    protected override long CustomDecrementValue( long value, long increment )
    {
      return value - increment;
    }

    #endregion //Base Class Overrides
  }
}
