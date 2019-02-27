using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace nitorco.modules.mvvm
{
  public interface iControlEnabler : INotifyPropertyChanged
  {
    string Name { get; set; }     // optional
    bool IsEnabled { get; }
    bool IsReadOnly { get; }
    bool AllowEdit { get; set; }
    void Refresh();
  }

}
