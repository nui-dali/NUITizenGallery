using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    //Please expand Window size, When it runs on Ubuntu.
    public class DatePickerContentPage : ContentPage
    {
        private DatePicker datePicker;
        private TextLabel dateLabel;

        private void OnValueChanged(object sender, DateChangedEventArgs e)
        {
            dateLabel.Text = $"Current date is {e.Date.Day} {e.Date.Month} {e.Date.Year}";
        }

        public DatePickerContentPage()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "DatePicker Sample",
            };

            var root = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size(50, 50),
                }
            };
            Content = root;

            datePicker = new DatePicker()
            {
                WidthSpecification = 400,
                HeightSpecification = 200,
            };
            datePicker.DateChanged += OnValueChanged;

            dateLabel = new TextLabel()
            {
                WidthSpecification = 400,
                HeightSpecification = 100,
                PointSize = 20,
                TextColor = Color.Red,
                Text = $"Current date is {datePicker.Date.Day} {datePicker.Date.Month} {datePicker.Date.Year}",
            };
            root.Add(dateLabel);
            root.Add(datePicker);
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
            datePicker.Dispose();
            datePicker = null;
        }
    }

    class DatePickerTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new DatePickerContentPage());
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
