using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using nitorco.data;
using nitorco.modules.mvvm.factory;
using nitorco.ui.views.wpf;

namespace nitorco.modules.mvvm.wpf
{
  public class DialogManager
  {
    private static Dictionary<string, MvvmFactory> m_windows = new Dictionary<string, MvvmFactory>();

    //public static qList<MvvmFactory> Windows { get { return m_windows; } }
    public static ncViewModelBase GetViewModel(Type viewModel_Type)
    {
      return m_windows[viewModel_Type.ToString()].ViewModel as ncViewModelBase;
    }

    #region Events

    public static void AddDialog(MvvmFactory factory, bool showAsModal = true, string title = "", Visibility cancelButtonVisibility = Visibility.Visible,
      Visibility okButtonVisibility = Visibility.Visible, HorizontalAlignment okcancelButtonsAlignment = HorizontalAlignment.Center, double form_height = 500, double form_width = 500)
    {
      iViewForViewModel vvm = factory.View as iViewForViewModel;
      if (vvm == null)
        throw new Exception("DialogWindowManager must work with a view which subscribes to iViewForViewModel");

      ncViewModelBase vm = factory.ViewModel as ncViewModelBase;
      if (vm == null)
        throw new Exception("DialogWindowManager must work with a ViewModel which inherits from ncViewModelBase");

      string key = vm.GetType().ToString();
      if (m_windows.ContainsKey(key))
        throw new Exception("DialogWindowManager cannot contain two windows with the same view model type.");

      m_windows.Add(key, factory);
      vm.evtAfterClosed += vm_evtClose;

#if !TESTING
      wndGeneric_UserControl wnd = new wndGeneric_UserControl(factory.View as UserControl, title, cancelButtonVisibility, okButtonVisibility, okcancelButtonsAlignment, form_height, form_width);
      if (showAsModal)
      {
        wnd.ShowDialog();
      }
      else
      {
        wnd.Show();
      }
#endif
    }

    /// <summary>
    /// Creates a generic dialog with a combobox and gets the result back
    /// </summary>
    /// <param name="title">Title of window</param>
    /// <param name="label">Message to display in label</param>
    /// <param name="displayMemberPath">Column in itemSource to display in combobox</param>
    /// <param name="valuePath">Column in itemSource to use as value (int)</param>
    /// <param name="itemSource">Item Source to fill out as list in combobox</param>
    /// <param name="combo_style">Style to apply to combobox for multi-column</param>
    /// <param name="testcase_ReturnResult">result to return in place of a user picking a value in the window (TEST CASES ONLY)</param>
    /// <returns>User selected value or test case value passed in</returns>
    public static int? GetComboResult(string title, string label, string displayMemberPath, string valuePath, DataView itemSource, Style combo_style, int? testcase_ReturnResult)
    {
#if !TESTING
      wndGeneric_Combo wnd = new wndGeneric_Combo(title, label, displayMemberPath, valuePath, itemSource, combo_style);
      if (wnd.ShowDialog() == true)
        return wnd.ResultAsInt;
      else
        return null;
#else
      return testcase_ReturnResult;
#endif
    }

    /// <summary>
    /// Creates a generic dialog with a datetime control and gets the result back
    /// </summary>
    /// <param name="title">Title of window</param>
    /// <param name="label">Message to display in label</param>
    /// <param name="testcase_ReturnResult">result to return in place of a user picking a value in the window (TEST CASES ONLY)</param>
    /// <returns>User selected value or test case value passed in</returns>
    public static DateTime? GetDateResult(string title, string label, DateTime? testcase_ReturnResult)
    {

#if !TESTING
      wndGeneric_Date wnd = new wndGeneric_Date(title, label);
      if (wnd.ShowDialog() == true)
        return wnd.Result;
      else
        return null;
#else
      return testcase_ReturnResult;
#endif

    }

    /// <summary>
    /// Creates a generic dialog with a textbox control and gets the result back
    /// </summary>
    /// <param name="title">Title of window</param>
    /// <param name="label">Message to display in label</param>
    /// <param name="testcase_ReturnResult">result to return in place of a user picking a value in the window (TEST CASES ONLY)</param>
    /// <param name="height">height of window</param>
    /// <param name="width">width of window</param>
    /// <returns>User selected value or test case value passed in</returns>
    public static string GetTextResult(string title, string label, string testcase_ReturnResult, int height = 250, int width = 300)
    {

#if !TESTING
      wndGeneric_Text wnd = new wndGeneric_Text(title, label, height, width);
      if (wnd.ShowDialog() == true)
        return wnd.Result;
      else
        return null;
#else
      return testcase_ReturnResult;
#endif

    }

    private static void vm_evtClose(object sender, EventArgs e)
    {
      string key = sender.GetType().ToString();
      m_windows.Remove(key);
    }

    #endregion
  }
}
