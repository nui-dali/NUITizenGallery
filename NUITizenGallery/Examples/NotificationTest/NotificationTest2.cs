using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class NotificationTestPage2 : ContentPage
    {
        private View root;
        private View view;
        private TextLabel info;
        private Notification noti;
        private Button makeToast;

        internal NotificationTestPage2(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "Notification Sample",
            };

            root = new View()
            {
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height),
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(10, 50),
                }
            };

            view = new View()
            {
                BackgroundColor = Color.Blue,
                WidthSpecification = window.WindowSize.Width / 2,
                HeightSpecification = window.WindowSize.Height / 4,
            };
            info = new TextLabel()
            {
                Text = "This notice will be dismissed after a moment while automatically !",
                Ellipsis = false,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };
            view.Add(info);

            noti = new Notification(view);
            NotificationLevel level = NotificationLevel.Medium;
            noti.SetLevel(level);
            Rectangle rec = new Rectangle(window.WindowSize.Width / 4, 0, window.WindowSize.Width / 2, window.WindowSize.Height / 4);
            noti.SetPositionSize(rec);
            noti.SetAnimationOnDismiss(new Animation()
            {
                Duration = 2000,
                DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.Linear)
            });
            noti.SetAnimationOnPost(new Animation()
            {
                Duration = 2000,
                DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.Reverse)
            });
            uint duration = 3000;
            noti.Post(duration);

            makeToast = new Button()
            {
                Text = "MakeToast",
                WidthSpecification = window.WindowSize.Width / 2,
                HeightSpecification = window.WindowSize.Height / 12,
            };
            makeToast.Clicked += OnMakeToastClicked;

            root.Add(makeToast);

            Content = root;
        }

        private void OnMakeToastClicked(object sender, ClickedEventArgs e)
        {
            Notification.MakeToast("Hello World!", Notification.ToastBottom).Post(Notification.ToastLong);
        }

        protected override void Dispose(DisposeTypes type)
        {
            if (Disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                Deactivate();
            }

            base.Dispose(type);
        }

        private void Deactivate()
        {
            if (root != null)
            {

            }
        }
    }

    class NotificationTest2 : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new NotificationTestPage2(window));
        }

        public void Deactivate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
#pragma warning restore Reflection // The code contains reflection
            window.GetDefaultNavigator().Pop();
        }
    }
}
