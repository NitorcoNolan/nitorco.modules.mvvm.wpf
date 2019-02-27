using System.Collections.ObjectModel;
using System.Windows.Data;
using nitorco.data;

namespace nitorco.modules.mvvm.wpf
{
  public class ncViewableCollection<T> : qObservableCollection<T> where T : class
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="ncViewableCollection{T}"/> class.
    /// Uses an empty qList<T> for the base list
    /// </summary>
    public ncViewableCollection() : this(new qList<T>()) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="ncViewableCollection{T}"/> class.
    /// </summary>
    /// <param name="own">Object that implements qListBase, most commonly appobject_List types.</param>
    public ncViewableCollection(qListBase<T> own) : base(own) { }

    private CollectionView _view;
    /// <summary>
    /// Gets the view.
    /// Subscribe your WPF XAML properties to this.View property.
    /// </summary>
    /// <value>The view.</value>
    public CollectionView View
    {
      get
      {
        if (_view == null)
        {
          _view = new ListCollectionView(Owner.ObservableCollection());
        }

        return _view;
      }
    }

    /// <summary>
    /// Adds an object to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1" />.
    /// This adds directly to the ObservableCollection instead of the qList for user experience in the View
    /// </summary>
    /// <param name="item">The object to be added to the end of the <see cref="T:System.Collections.ObjectModel.Collection`1" />. The value can be null for reference types.</param>
    public new void Add(T item)
    {
      Owner.ObservableCollection().Add(item);
    } 

  }
}
