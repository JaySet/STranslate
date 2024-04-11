﻿using Microsoft.Xaml.Behaviors;
using STranslate.Model;
using System.Windows;
using System.Windows.Input;

namespace STranslate.Style.Commons
{
    /// <summary>
    /// 热键绑定keydown、keyup到viewmodel
    /// </summary>
    public class KeyArgsToCommandBehavior : Behavior<UIElement>
    {
        public static readonly DependencyProperty KeydownProperty = DependencyProperty.Register(
            "Keydown",
            typeof(ICommand),
            typeof(KeyArgsToCommandBehavior),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty KeyupProperty = DependencyProperty.Register(
            "Keyup",
            typeof(ICommand),
            typeof(KeyArgsToCommandBehavior),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty KeydownParamProperty = DependencyProperty.Register(
            "KeydownParam",
            typeof(object),
            typeof(KeyArgsToCommandBehavior),
            new PropertyMetadata(null)
        );

        public static readonly DependencyProperty KeyupParamProperty = DependencyProperty.Register(
            "KeyupParam",
            typeof(object),
            typeof(KeyArgsToCommandBehavior),
            new PropertyMetadata(null)
        );

        public ICommand Keydown
        {
            get { return (ICommand)GetValue(KeydownProperty); }
            set { SetValue(KeydownProperty, value); }
        }

        public ICommand Keyup
        {
            get { return (ICommand)GetValue(KeyupProperty); }
            set { SetValue(KeyupProperty, value); }
        }

        public object KeydownParam
        {
            get { return GetValue(KeydownParamProperty); }
            set { SetValue(KeydownParamProperty, value); }
        }

        public object KeyupParam
        {
            get { return GetValue(KeyupParamProperty); }
            set { SetValue(KeyupParamProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewKeyDown += OnPreviewKeyDown;
            this.AssociatedObject.KeyUp += OnKeyUp;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PreviewKeyDown -= OnPreviewKeyDown;
            this.AssociatedObject.KeyUp -= OnKeyUp;
        }

        private void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keydown != null)
            {
                var commandParameter = new UserdefineKeyArgsModel { KeyEventArgs = e, Obj = KeydownParam };
                Keydown.Execute(commandParameter);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (Keyup != null)
            {
                var commandParameter = new UserdefineKeyArgsModel { KeyEventArgs = e, Obj = KeyupParam };

                Keyup.Execute(commandParameter);
            }
        }
    }
}
