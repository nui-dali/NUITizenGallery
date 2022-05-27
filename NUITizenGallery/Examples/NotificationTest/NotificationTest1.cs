using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class NotificationTestPage1 : ContentPage
    {
        private View root;
        private View view;
        private TextLabel info;
        private Notification noti;
        private TextLabel contentView;
        private Button dismiss;
        private Button forceQuit;
        private Button setDismissOnTouch;
        private View bottomView;

        internal NotificationTestPage1(Window window)
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
                Name = "Test for notification.",
                BackgroundColor = Color.Blue,
                WidthSpecification = window.WindowSize.Width /2,
                HeightSpecification = window.WindowSize.Height / 4,
            };
            info = new TextLabel()
            {
                Text = "This is system test infomation, please ignore it!",
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
            noti.SetDismissOnTouch(false);
            uint duration = 0;
            noti.Post(duration);
 
            contentView = new TextLabel()
            {
                Text = "ContentView : " + noti.ContentView.Name,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = window.WindowSize.Height / 10,
            };
            root.Add(contentView);

            bottomView = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = window.WindowSize.Height / 12,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(30, 10),
                }
            };

            dismiss = new Button()
            {
                Text = "Dismiss",
            };
            dismiss.Clicked += OnDismissClicked;

            forceQuit = new Button()
            {
                Text = "ForceQuit",
            };
            forceQuit.Clicked += OnForceQuiltClicked;

            setDismissOnTouch = new Button()
            {
                Text = "SetDismissOnTouch",
            };
            setDismissOnTouch.Clicked += OnSetDismissOnTouch;

            bottomView.Add(dismiss);
            bottomView.Add(forceQuit);
            bottomView.Add(setDismissOnTouch);

            root.Add(bottomView);

            Content = root;
        }

        private void OnSetDismissOnTouch(object sender, ClickedEventArgs e)
        {
            noti.SetDismissOnTouch(true);
        }

        private void OnDismissClicked(object sender, ClickedEventArgs e)
        {
            noti.Dismiss();
        }

        private void OnForceQuiltClicked(object sender, ClickedEventArgs e)
        {
            noti.ForceQuit();
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

    class NotificationTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new NotificationTestPage1(window));
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
