using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace nitorco.modules.mvvm.wpf
{
  public interface iControlVisibility : INotifyPropertyChanged
  {
    string Name { get; set; }     // optional
    Visibility Visibility { get; }

    Visibility DesignModeVisibility { get; set; }

    bool IsInDesignMode { get; set; }

    bool IsVisible { get; set; }
    void Refresh();
  }
}
