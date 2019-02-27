using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace nitorco.modules.mvvm.wpf
{
  public class ControlEnableJoin : ControlEnabler
  {
    #region Data

    protected List<iControlEnabler> m_Enablers;

    #endregion // Data

    #region Construction

    public ControlEnableJoin()
      : base()
    {
      m_Enablers = new List<iControlEnabler>();
    }

    public ControlEnableJoin(iControlEnabler en1, iControlEnabler en2)
      : this()
    {
      Add(en1);
      Add(en2);
    }

    public ControlEnableJoin(IEnumerable<iControlEnabler> l)
      : this()
    {
      foreach (iControlEnabler c in l)
        Add(c);
    }

    #endregion // Construction

    #region List Management

    public void Add(iControlEnabler en)
    {
      if (en == null)
        return;
      en.PropertyChanged += OnPropertyChanged;
      m_Enablers.Add(en);
      Refresh();
    }

    public void Remove(iControlEnabler en)
    {
      if (en == null)
        return;
      if (m_Enablers.Remove(en))
        en.PropertyChanged -= OnPropertyChanged;
      Refresh();
    }
    public void Remove(string name)
    {
      iControlEnabler c = m_Enablers.Find((f) => f.Name == name);
      Remove(c);
    }

    #endregion // List Management

    public override void Refresh()
    {
      bool a = true;
      foreach (iControlEnabler i in m_Enablers)
      {
        if (!i.AllowEdit)
          a = false;
      }
      AllowEdit = a;
    }

    private void OnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
      if (propertyChangedEventArgs.PropertyName == "AllowEdit")
        Refresh();
    }
  }

}
