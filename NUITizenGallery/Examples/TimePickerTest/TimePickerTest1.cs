using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class TimePickerPage1 : ContentPage
    {
        private View root;
        private TimePicker timePicker;
        private DateTime time;
        private TextLabel text;
        private Button set;
        private Button reset;
        private bool flag = false;
            
        internal TimePickerPage1(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "TimePicker Sample",
            };

            root = new View()
            {
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height, 0),
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                    CellPadding = new Size2D(20, 20),
                    Padding = new Extents(20, 20, 20, 20),
                }
            };

            flag = false;
            time = DateTime.Now;
            text = new TextLabel();
            text.Text = time.Hour + ":" + time.Minute + ", TimeChanged : " + flag;
            root.Add(text);

            set = new Button()
            { 
                Text = "Set",
                BackgroundColor = Color.Blue
            };
            set.Clicked += OnSetClicked;

            reset = new Button()
            { 
                Text = "Reset",
                BackgroundColor = Color.Blue
            };
            reset.Clicked += OnResetClicked;

            root.Add(set);
            root.Add(reset);

            Content = root;
        }

        private void OnSetClicked(object sender, ClickedEventArgs e)
        {
            timePicker = new TimePicker()
            {
                //Should give a size to picker for content of AlertDialog
                Size = new Size(600, 339),
                Time = time,
            };
            timePicker.Is24HourView = true;
            timePicker.TimeChanged += OnTimeChanged;

            var btn1 = new Button() { Text = "Set", };
            btn1.Clicked += (object s, ClickedEventArgs a) =>
            {
                time = timePicker.Time;
                text.Text = timePicker.Time.Hour + ":" + timePicker.Time.Minute + ", TimeChanged : " + flag;
                Navigator?.Pop();
            };

            var btn2 = new Button() { Text = "Cancel", };
            btn2.Clicked += (object s, ClickedEventArgs a) =>
            {
                Navigator?.Pop();
            };

            View[] actions = { btn1, btn2 };
            var dialogPage = new DialogPage()
            {
                Content = new AlertDialog()
                {
                    Title = "Set Date",
                    Content = timePicker,
                    Actions = actions,
                },
            };

            NUIApplication.GetDefaultWindow().GetDefaultNavigator().Push(dialogPage);
        }

        private void OnResetClicked(object sender, ClickedEventArgs e)
        {
            time = DateTime.Now;
            flag = false;
            text.Text = time.Hour + ":" + time.Minute + ", TimeChanged : " + flag;
        }

        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            flag = true;
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
                text.Dispose();
                text = null;

                set.Dispose();
                set = null;

                reset.Dispose();
                reset = null;

                timePicker.Dispose();
                timePicker = null;
            }
        }
    }

    internal class TimePickerTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new TimePickerPage1(window));
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
