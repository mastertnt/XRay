
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This interface defines a generic item in a hierarchical view model.
    /// </summary>
    /// <typeparam name="T">The type of the owned object.</typeparam>
    public interface IHierarchicalItemViewModel<T> : IHierarchicalItemViewModel, Generic.IViewModel<T>
    {
    }
}
