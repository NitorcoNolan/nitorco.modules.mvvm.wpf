using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace nitorco.modules.mvvm.wpf
{
  public class ControlVisibleDelegate : ControlVisibilityWatch
  {
    #region Data

    public delegate bool dlgTestEdit(object h, object s, PropertyChangedEventArgs a);

    private dlgTestEdit m_handler;

    #endregion // Data

    #region Construction

    public ControlVisibleDelegate() {}

    public ControlVisibleDelegate(object p, dlgTestEdit handler)
      : base(p)
    {
      Handler = handler;
    }

    public ControlVisibleDelegate(object p) : this(p, null) { }

    public ControlVisibleDelegate(IEnumerable<object> p, dlgTestEdit handler)
      : base(p)
    {
      Handler = handler;
    }

    public ControlVisibleDelegate(IEnumerable<object> p) : this(p, null) { }

    #endregion // Construction

    #region Acessors

    public dlgTestEdit Handler
    {
      set
      {
        m_handler = value;
        Refresh();
      }
    }

    #endregion // Acessors

    protected override void Refresh(object sender, PropertyChangedEventArgs args) 
    {
      IsVisible = m_handler == null || m_handler(this, sender, args);
    }
  }
}
