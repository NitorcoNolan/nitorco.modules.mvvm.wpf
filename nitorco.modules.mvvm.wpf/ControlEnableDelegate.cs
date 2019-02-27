using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using nitorco.data;

namespace nitorco.modules.mvvm.wpf
{
  public class ControlEnableDelegate : ControlEnablerWatch
  {
    #region Data

    public delegate bool dlgTestEdit(object h, object s, PropertyChangedEventArgs a);

    private dlgTestEdit m_handler;

    #endregion // Data

    #region Construction

    public ControlEnableDelegate()
      : base()
    {
    }

    public ControlEnableDelegate(object p)
      : base(p)
    {
    }
    public ControlEnableDelegate(object p, dlgTestEdit handler)
      : base(p)
    {
      Handler = handler;
    }

    public ControlEnableDelegate(IEnumerable<object> p)
      : base(p)
    {
    }
    public ControlEnableDelegate(IEnumerable<object> p, dlgTestEdit handler)
      : base(p)
    {
      Handler = handler;
    }

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
      AllowEdit = m_handler == null || m_handler(this, sender, args);
    }
  }

}
