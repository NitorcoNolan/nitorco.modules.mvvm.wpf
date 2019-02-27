using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using GalaSoft.MvvmLight;
using nitorco.data;

namespace nitorco.modules.mvvm.wpf
{
  public class ControlVisibility : ObservableObject, iControlVisibility, INotifyPropertyChanging
  {
    #region Data

    //public event PropertyChangedEventHandler PropertyChanged;

    protected bool m_visible;
    protected Visibility m_onhide;
    private bool m_IsInDesignMode = false;
    private Visibility m_DesignModeVisibility;

    public event PropertyChangingEventHandler PropertyChanging;

    protected PropertyChangingEventHandler PropertyChangingHandler
    {
      get
      {
        return PropertyChanging;
      }
    }

    public virtual void RaisePropertyChanging(string propertyName = null)
    {
      VerifyPropertyName(propertyName);

      PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
    }

    public virtual void RaisePropertyChanging<T>(Expression<Func<T>> propertyExpression)
    {
      var handler = PropertyChanging;
      if (handler != null)
      {
        var propertyName = GetPropertyName(propertyExpression);
        handler(this, new PropertyChangingEventArgs(propertyName));
      }
    }

    #endregion // Data

    #region Construction

    public ControlVisibility(bool visible, Visibility onHide, bool isInDesignMode)
    {
      m_visible = visible;
      m_onhide = onHide;
      m_IsInDesignMode = isInDesignMode;
      m_DesignModeVisibility = Visibility.Collapsed;
    }

    public ControlVisibility() : this(true, Visibility.Collapsed, false) {}

    public ControlVisibility(bool vis) : this(vis, Visibility.Collapsed, false) {}

    #endregion // Construction

    #region Acessors

    public bool IsInDesignMode
    {
      get { return this.m_IsInDesignMode; }
      set
      {
        RaisePropertyChanging(() => IsInDesignMode);
        this.m_IsInDesignMode = value;
        RaisePropertyChanged(() => IsInDesignMode);
        RaisePropertyChanged(() => DesignModeVisibility);
        RaisePropertyChanged(() => Visibility);
      }
    }

    public Visibility DesignModeVisibility
    {
      get { return this.m_DesignModeVisibility; }
      set
      {
        RaisePropertyChanging(() => DesignModeVisibility);
        this.m_DesignModeVisibility = value;
        RaisePropertyChanged(() => DesignModeVisibility);
        RaisePropertyChanged(() => Visibility);
      }
    }

    public string Name { get; set; }     // optional

    public virtual Visibility Visibility
    {
      get
      {
        if (IsInDesignMode)
        {
          return DesignModeVisibility;
        }
        return m_visible ? Visibility.Visible : m_onhide;
      }
    }

    public virtual bool IsVisible
    {
      get { return m_visible; }
      set
      {
        if (m_visible == value)
          return;

        m_visible = value;
        RaisePropertyChanged(()=>IsVisible);
        RaisePropertyChanged(()=>Visibility);
      }
    }

    #endregion // Acessors

    public virtual void Refresh()
    {

    }

    #region Messages

    //protected void fire_PropertyChanged(string propname)
    //{
    //  if (PropertyChanged != null)
    //    PropertyChanged(this, new PropertyChangedEventArgs(propname));
    //}

    #endregion // Messages
  }


  public abstract class ControlVisibilityWatch : ControlVisibility
  {
    #region Data

    protected ObjectWatcher m_watcher;

    #endregion // Data

    #region Construction

    protected ControlVisibilityWatch()
      : base()
    {
      m_watcher = new ObjectWatcher();
      m_watcher.ToWatch.evtItemAdded += (sender, added) => Refresh();
      m_watcher.ToWatch.evtItemRemoved += (sender, added) => Refresh();
      m_watcher.PropertyChanged += onParentChanged;
    }

    protected ControlVisibilityWatch(object p)
      : this()
    {
      ToWatch.Add(p);
    }

    protected ControlVisibilityWatch(IEnumerable<object> p)
      : this()
    {
      foreach (object o in p)
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
