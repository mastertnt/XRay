
namespace XTreeListView.ViewModel.Generic
{
    /// <summary>
    /// This interface defines a generic item in a hierarchical view model.
    /// </summary>
    /// <typeparam name="TModel">The type of the owned object.</typeparam>
    public interface IHierarchicalItemViewModel<TModel> : IHierarchicalItemViewModel, Generic.IViewModel<TModel>
    {
    }
}
