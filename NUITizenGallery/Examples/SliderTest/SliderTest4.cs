using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class SliderTestPage4 : ContentPage
    {
        private View root;
        private TextLabel label, label1, label2;
        private Slider slider;
        private Slider slider1;
        private Slider slider2;

        private static string ResourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "/images/";

        internal SliderTestPage4(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "Slider Sample",
            };

            root = new View()
            {
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height),
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    CellPadding = new Size(50, 80),
                }
            };

            label = new TextLabel()
            { 
                WidthSpecification = LayoutParamPolicies.MatchParent,
                TextColor = Color.Red,
            };
            label.Text = "Low(High)IndicatorText";

            slider = new Slider();
            var ss = slider.Style;
            ss.IndicatorType = Slider.IndicatorType.Text;
            ss.LowIndicator = new TextLabelStyle
            {
                Size = new Size(60, 60),
                Color = Color.Yellow,
            };
            ss.HighIndicator = new TextLabelStyle
            {
                Size = new Size(60, 60),
                Color = Color.Yellow,
            };

            ss.ValueIndicatorText = new TextLabelStyle()
            {
                Size = new Size(60, 60),
                TextColor = Color.Yellow
            };
            slider.ApplyStyle(ss);

            slider.Name = "slider41";
            slider.Size = new Size((int)root.SizeWidth * 3 / 4, 20);
            slider.SlidedTrackColor = Color.Red;
            slider.ThumbImageUrl = ResourcePath + "progress_7.png";
            slider.IsValueShown = true;
            slider.LowIndicatorSize = new Size(60, 60);
            slider.LowIndicatorTextContent = "Low";
            slider.HighIndicatorSize = new Size(65, 65);
            slider.HighIndicatorTextContent = "High";
            slider.ValueIndicatorSize = new Size(60, 60);
            slider.ValueChanged += OnValueChanged;

            label1 = new TextLabel()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                TextColor = Color.Yellow,
            };
            label1.Text = "Low(High)IndicatorImage";

            slider1 = new Slider();
            var ss1 = slider.Style;
            ss1.IndicatorType = Slider.IndicatorType.Image;
            ss1.LowIndicator = new TextLabelStyle
            {
                Size = new Size(160, 160),
            };
            ss1.HighIndicator = new TextLabelStyle
            {
                Size = new Size(160, 160),

            };
            ss1.LowIndicatorImage = new ImageViewStyle
            {
                Size = new Size(160, 160),
            };
            ss1.HighIndicatorImage = new ImageViewStyle
            {
                Size = new Size(160, 160),
            };
            ss1.ValueIndicatorImage = new ImageViewStyle
            {
                Size = new Size(160, 160),
            };
            slider1.ApplyStyle(ss1);

            slider1.Name = "slider42";
            slider1.Size = new Size((int)root.SizeWidth * 3 / 4, 20);
            slider1.SlidedTrackColor = Color.Yellow;
            slider1.ThumbImageUrl = ResourcePath + "tizen.png";
            slider1.IsValueShown = true;
            slider1.LowIndicatorSize = new Size(60, 60);
            slider1.LowIndicatorImageURL = ResourcePath + "progress_0.png";
            slider1.HighIndicatorSize = new Size(60, 60);
            slider1.HighIndicatorImageURL = ResourcePath + "progress_7.png";
            slider1.ValueIndicatorSize = new Size(60, 60);
            slider1.ValueIndicatorUrl = ResourcePath + "bg_1.png";
            slider1.ValueChanged += OnValueChanged;

            label2 = new TextLabel()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                TextColor = Color.Blue,
            };
            label2.Text = "ValueIndicatorText";

            slider2 = new Slider();
            var ss2 = slider2.Style;
            ss2.ValueIndicatorText = new TextLabelStyle() 
            { 
                Size = new Size(60, 60), 
                TextColor = Color.Yellow 
            };
            slider2.ApplyStyle(ss2);

            slider2.Name = "slider43";
            slider2.SlidedTrackColor = Color.Blue;
            slider2.ThumbImageUrl = ResourcePath + "Boston.png";
            slider2.Size = new Size((int)root.SizeWidth * 3 / 4, 20);
            slider2.IsValueShown = true;
            slider2.ValueIndicatorSize = new Size(60, 60);

            slider2.ValueChanged += OnValueChanged;

            root.Add(label);
            root.Add(slider);
            root.Add(label1);
            root.Add(slider1);
            root.Add(label2);
            root.Add(slider2);

            Content = root;
        }

        private void OnValueChanged(object sender, SliderValueChangedEventArgs args)
        {
            Slider source = sender as Slider;
            if (source != null)
            {
                source.ValueIndicatorText = ((int)source.CurrentValue).ToString();
            }
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
                root.Dispose();
                root = null;
            }
        }
    }

    class SliderTest4 : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new SliderTestPage4(window));
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
