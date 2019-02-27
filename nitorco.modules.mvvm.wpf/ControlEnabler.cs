using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using nitorco.data;

namespace nitorco.modules.mvvm.wpf
{
  public class ControlEnabler : iControlEnabler
  {
    #region Data

    public event PropertyChangedEventHandler PropertyChanged;

    protected bool m_allowedit;

    #endregion // Data

    #region Construction

    public ControlEnabler()
    {
      m_allowedit = true;
    }
    public ControlEnabler(bool enabled)
    {
      m_allowedit = enabled;
    }

    #endregion // Construction

    #region Acessors

    public string Name { get; set; }     // optional

    public virtual bool IsEnabled
    {
      get { return m_allowedit; }
    }

    public virtual bool IsReadOnly
    {
      get { return !m_allowedit; }
    }

    public virtual bool AllowEdit
    {
      get { return m_allowedit; }
      set
      {
        if (m_allowedit == value)
          return;

        m_allowedit = value;
        fire_PropertyChanged("AllowEdit");
        fire_PropertyChanged("IsEnabled");
        fire_PropertyChanged("IsReadOnly");
      }
    }

    #endregion // Acessors

    public virtual void Refresh()
    {

    }

    #region Messages

    protected void fire_PropertyChanged(string propname)
    {
      if (PropertyChanged != null)
        PropertyChanged(this, new PropertyChangedEventArgs(propname));
    }

    #endregion // Messages
  }


  public abstract class ControlEnablerWatch : ControlEnabler
  {
    #region Data

    protected ObjectWatcher m_watcher;

    #endregion // Data

    #region Construction

    protected ControlEnablerWatch()
      : base()
    {
      m_watcher = new ObjectWatcher();
      m_watcher.ToWatch.evtItemAdded += (sender, added) => Refresh();
      m_watcher.ToWatch.evtItemRemoved += (sender, added) => Refresh();
      m_watcher.PropertyChanged += onParentChanged;
    }

    protected ControlEnablerWatch(object p)
      : this()
    {
      ToWatch.Add(p);
    }

    protected ControlEnablerWatch(IEnumerable<object> p)
      : this()
    {
      foreach(object o in p)
        ToWatch.Add(o);
    }

    #endregion // Construction

    #region Acessors

    public qList<object> ToWatch
    {
      get { return m_watcher.ToWatch; }
    }

    #endregion // Acessors

    protected abstract void Refresh(object sender, PropertyChangedEventArgs args);
    public override void Refresh()
    {
      Refresh(this, null);
    }

    #region Messages

    private void onParentChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
      Refresh(sender, propertyChangedEventArgs);
    }

    #endregion // Messages  
  }
}
