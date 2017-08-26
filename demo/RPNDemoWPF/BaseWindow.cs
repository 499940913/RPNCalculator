using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;

// ReSharper disable InconsistentNaming

namespace RPNDemoWPF
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:RPNDemoWPF"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:RPNDemoWPF;assembly=RPNDemoWPF"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[浏览查找并选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:BaseWindow/>
    ///
    /// </summary>
    [TemplatePart(Name = "HeaderWorkArea", Type = typeof(Border))]
    [TemplatePart(Name = "MinimizeButton", Type = typeof(Button))]
    [TemplatePart(Name = "NormalMaximizeButtton", Type = typeof(Button))]
    [TemplatePart(Name = "CloseButton", Type = typeof(Button))]
    public class BaseWindow:Window
    {
        #region win32api
        #region 结构体
        [StructLayout(LayoutKind.Sequential)]
        internal struct WindowCompositionAttributeData
        {
            public WindowCompositionAttribute Attribute;
            public IntPtr Data;
            public int SizeOfData;
        }
        internal enum WindowCompositionAttribute
        {
            // ...
            WCA_ACCENT_POLICY = 19
            // ...
        }
        internal enum AccentState
        {
            ACCENT_DISABLED = 1,
            ACCENT_ENABLE_GRADIENT = 0,
            ACCENT_ENABLE_TRANSPARENTGRADIENT = 2,
            ACCENT_ENABLE_BLURBEHIND = 3,
            ACCENT_INVALID_STATE = 4
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct AccentPolicy
        {
            public AccentState AccentState;
            public int AccentFlags;
            public int GradientColor;
            public int AnimationId;
        }
        #endregion
        #region api
        [DllImport("user32.dll")]
        internal static extern int SetWindowCompositionAttribute(IntPtr hwnd, ref WindowCompositionAttributeData data);
        #endregion
        #endregion

        private Border HeaderWorkArea;
        private Button MinimizeButton;
        private Button NormalMaximizeButtton;
        private Button CloseButton;

        static BaseWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseWindow), new FrameworkPropertyMetadata(typeof(BaseWindow)));
        }

        private void EnableBlur()
        {
            var windowHelper = new WindowInteropHelper(this);
            var accent = new AccentPolicy {AccentState = AccentState.ACCENT_ENABLE_BLURBEHIND};
            var accentStructSize = Marshal.SizeOf(accent);
            var accentPtr = Marshal.AllocHGlobal(accentStructSize);
            Marshal.StructureToPtr(accent, accentPtr, false);
            var data = new WindowCompositionAttributeData
            {
                Attribute = WindowCompositionAttribute.WCA_ACCENT_POLICY,
                SizeOfData = accentStructSize,
                Data = accentPtr
            };
            SetWindowCompositionAttribute(windowHelper.Handle, ref data);

            Marshal.FreeHGlobal(accentPtr);
        }

        private void HeaderWorkAreaMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void MinimizeButtonClick(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

       

        private bool _isMaximized;
        private void NormalMaximizeButttonClick(object sender, RoutedEventArgs e)
        {
            _isMaximized = !_isMaximized;
            WindowState = _isMaximized ? WindowState.Maximized : WindowState.Normal;
        }

        private void CloseClick(object sender, RoutedEventArgs e)
        {
            if (HeaderWorkArea != null)
                HeaderWorkArea.MouseDown -= HeaderWorkAreaMouseDown;
            if (MinimizeButton != null)
                MinimizeButton.Click -= MinimizeButtonClick;
            if (NormalMaximizeButtton != null)
                NormalMaximizeButtton.Click -= NormalMaximizeButttonClick;
            if (CloseButton != null)
                CloseButton.Click -= CloseClick;
            Close();
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            HeaderWorkArea = Template.FindName("HeaderWorkArea", this) as Border;
            if (HeaderWorkArea != null)
                HeaderWorkArea.MouseDown += HeaderWorkAreaMouseDown;
          MinimizeButton = Template.FindName("MinimizeButton", this) as Button;
            if (MinimizeButton != null)
                MinimizeButton.Click += MinimizeButtonClick;
         NormalMaximizeButtton = Template.FindName("NormalMaximizeButtton", this) as Button;
            if (NormalMaximizeButtton != null)
                NormalMaximizeButtton.Click += NormalMaximizeButttonClick;   
           CloseButton = Template.FindName("CloseButton", this) as Button;
            if (CloseButton != null)
                CloseButton.Click += CloseClick;
            EnableBlur();
        }
    }

  
}
