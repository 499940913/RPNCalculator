using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using RPNCore;

namespace RPNDemoWPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow
    {
        Calculator _calculator=new Calculator();

        public MainWindow()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
                var buttons = SerializationHelper.DeSerialize<List<ButtonModel>>("buttons.xml");
                if (buttons != null)
                {
                    foreach (var model in buttons)
                    {
                        var btncont=new BaseButton
                        {
                            IsOperator = model.IsOperator,
                            Content = model.Display,
                            Tag = model.Symbol
                        };
                        Buttons.Children.Add(btncont);
                        btncont.Click += OnButtonClick;
                    }
                   
                }
            };
        }

        private void OnButtonClick(object sender,RoutedEventArgs e)
        {
            var btn = sender as BaseButton;
            if(btn==null) return;
            ExpressionTxt.Text += $"{btn.Tag}";
        }

        private void Apply_OnClick(object sender, RoutedEventArgs e)
        {
            string msg;
            bool isok;
            var express= ExpressionTxt.Text;
            var res= _calculator.Compute(ExpressionTxt.Text,out isok,out msg);
            ExpressionTxt.Text=!isok?msg:res.ToString(CultureInfo.InvariantCulture);
            if (!isok) return;
            var builder=new ExpressBuilder();
            builder.Parse(express,out msg);
            PreviewTxt.Text = $"rpn:{builder.RPNExpression}\r\nnormal:{express}=";
        }
    }
}
