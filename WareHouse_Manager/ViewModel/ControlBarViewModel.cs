using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WareHouse_Manager.ViewModel
{
    public class ControlBarViewModel:BaseViewModel
    {
        #region Commands
        public ICommand CloseWindowCmd { get; set; }
        public ICommand MaximizeWindowCmd { get; set; }
        public ICommand MinimizeWindowCmd { get; set; }
        public ICommand MouseMoveWindowCmd { get; set; }
        public ControlBarViewModel()
        {
            CloseWindowCmd = new RelayCommand<UserControl>(
                x => { return x == null ? false : true; }, 
                x => {FrameworkElement e=GetWindowParrent(x);
                Window window =e as Window;
                    if (window != null)
                            window.Close();
                });
            MaximizeWindowCmd = new RelayCommand<UserControl>(
                x => { return x == null ? false : true; },
                x => {
                    FrameworkElement e = GetWindowParrent(x);
                    Window window = e as Window;
                    if (window != null)
                        if (window.WindowState == WindowState.Normal)
                            window.WindowState = WindowState.Maximized;
                        else
                            window.WindowState = WindowState.Normal;
                });
            MinimizeWindowCmd = new RelayCommand<UserControl>(
                x => { return x == null ? false : true; },
                x => {
                    FrameworkElement e = GetWindowParrent(x);
                    Window window = e as Window;
                    if (window != null)
                        window.WindowState = WindowState.Minimized;
                });
            MouseMoveWindowCmd = new RelayCommand<UserControl>(
                x => { return x == null ? false : true; },
                x => {
                    FrameworkElement e = GetWindowParrent(x);
                    Window window = e as Window;
                    if (window != null)
                        window.DragMove();
                });
        }
        FrameworkElement GetWindowParrent(UserControl uc)
        {
            FrameworkElement e = uc.Parent as FrameworkElement;
            while (e.Parent!=null)
            {
                e = e.Parent as FrameworkElement;
            }
            return e;
        }
        #endregion
    }
}
