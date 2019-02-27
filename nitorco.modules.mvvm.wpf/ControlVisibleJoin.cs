using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace nitorco.modules.mvvm.wpf
{
  public class ControlVisibleJoin : ControlVisibility
  {
    #region Data

    protected List<iControlVisibility> m_vis;

    #endregion // Data

    #region Construction

    public ControlVisibleJoin()
      : base()
    {
      m_vis = new List<iControlVisibility>();
    }

    public ControlVisibleJoin(iControlVisibility en1, iControlVisibility en2)
      : this()
    {
      Add(en1);
      Add(en2);
    }

    public ControlVisibleJoin(IEnumerable<iControlVisibility> l)
      : this()
    {
      foreach (iControlVisibility c in l)
        Add(c);
    }

    #endregion // Construction

    #region List Management

    public void Add(iControlVisibility en)
    {
      if (en == null)
        return;
      en.PropertyChanged += OnPropertyChanged;
      m_vis.Add(en);
      Refresh();
    }

    public void Remove(iControlVisibility en)
    {
      if (en == null)
        return;
      if (m_vis.Remove(en))
        en.PropertyChanged -= OnPropertyChanged;
      Refresh();
    }
    public void Remove(string name)
    {
      iControlVisibility c = m_vis.Find((f) => f.Name == name);
      Remove(c);
    }

    #endregion // List Management

    public override void Refresh()
    {
      bool a = true;
      foreach (iControlVisibility i in m_vis)
      {
        if (!i.IsVisible)
          a = false;
      }
      IsVisible = a;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
      if (propertyChangedEventArgs.PropertyName == "AllowEdit")
        Refresh();
    }
  }

}
