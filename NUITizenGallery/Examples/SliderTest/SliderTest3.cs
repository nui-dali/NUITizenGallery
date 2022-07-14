using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class SliderTestPage3 : ContentPage
    {
        private View root;
        private const float MIN_VALUE = 0;
        private const float MAX_VALUE = 100;
        private View slider_view, ver_slider_parent, hori_slider_parent, bottom_view;
        private TextLabel createText = new TextLabel();
        private TextLabel inforText = new TextLabel();
        private Slider[] slider_style = new Slider[4];
        private Slider bottom_slider1;
        private Slider bottom_slider2;

        private static string ResourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "/images/";

        internal SliderTestPage3(Window window)
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
                    CellPadding = new Size(50, 80),
                }
            };

            // Textlabel of Null style construction/Style construction and infoText
            CreateInfoText();

            // Various kinds of Slider
            CreateSliderView();

            CreateBottomView();

            Content = root;
        }

        private void InitSliders()
        {
            SliderStyle st = new SliderStyle
            {
                TrackThickness = 4,
                Track = new ImageViewStyle
                {
                    BackgroundColor = new Selector<Color>
                    {
                        //All = new Color(0, 0, 0, 0.1f),
                        All = Color.Red
                    }
                },

                Progress = new ImageViewStyle
                {
                    BackgroundColor = new Selector<Color>
                    {
                        //All = new Color(0.05f, 0.63f, 0.9f, 1),
                        All = Color.Yellow
                    }
                },

                Thumb = new ImageViewStyle
                {
                    Size = new Size(60, 60),
                    Color = Color.Blue,
                    ResourceUrl = new Selector<string>
                    {
                        Normal = ResourcePath + "controller/controller_btn_slide_handler_normal.png",
                        Pressed = ResourcePath + "controller/controller_btn_slide_handler_press.png",
                    },
                    BackgroundImage = new Selector<string>
                    {
                        Normal = "",
                        Pressed = ResourcePath + "controller/controller_btn_slide_handler_effect.png",
                    }
                }
            };
            slider_style[0] = CreateByStyle(st, (int)root.SizeWidth *3 / 4, 50, 20, Slider.DirectionType.Horizontal);
            slider_style[0].Indicator = Slider.IndicatorType.None;
            slider_style[0].IsDiscrete = true;
            slider_style[0].DiscreteValue = 20;

            slider_style[1] = CreateByStyle(st, (int)root.SizeWidth * 3 / 4, 50, 30, Slider.DirectionType.Horizontal);
            slider_style[1].Indicator = Slider.IndicatorType.Text;
            slider_style[1].LowIndicatorTextContent = "Low";
            slider_style[1].LowIndicatorSize = new Size(100, 40);
            slider_style[1].HighIndicatorTextContent = "High";
            slider_style[1].HighIndicatorSize = new Size(100, 40);
            slider_style[1].SpaceBetweenTrackAndIndicator = 20;
            slider_style[1].ValueIndicatorText = slider_style[1].CurrentValue.ToString();
            slider_style[1].ValueIndicatorSize = new Size(100, 40);

            slider_style[2] = CreateByStyle(st, 50, (int)root.SizeHeight / 3, 20, Slider.DirectionType.Vertical);
            slider_style[2].Indicator = Slider.IndicatorType.Image;
            slider_style[2].LowIndicatorImageURL = ResourcePath + "bg_0.png";
            slider_style[2].LowIndicatorSize = new Size(60, 60);
            slider_style[2].HighIndicatorImageURL = ResourcePath + "bg_1.png";
            slider_style[2].HighIndicatorSize = new Size(60, 60);

            slider_style[3] = CreateByStyle(st, 50, (int)root.SizeHeight / 3, 30, Slider.DirectionType.Vertical);
        }

        private void CreateInfoText()
        {
            inforText = new TextLabel();
            inforText.Margin = new Extents(0, 0, 40, 0);
            inforText.TextColor = Color.Blue;
            inforText.Text = "currentValue = ";
            inforText.BackgroundColor = new Color(0, 0, 0, 0.1f);
            inforText.HorizontalAlignment = HorizontalAlignment.Center;
            inforText.VerticalAlignment = VerticalAlignment.Center;
            root.Add(inforText);
        }

        private void CreateSliderView()
        {
            slider_view = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(50, 50),
                }
            };
            root.Add(slider_view);

            // Init Sliders
            InitSliders();

            // Add Horizontal Slider
            hori_slider_parent = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(50, 50)
                }
            };
            slider_view.Add(hori_slider_parent);
            slider_style[0].Margin = new Extents(0, 0, 25, 0);
            slider_style[0].Name = "slider1";
            slider_style[1].Margin = new Extents(0, 0, 25, 0);
            slider_style[1].Name = "slider2";
            hori_slider_parent.Add(slider_style[0]);
            hori_slider_parent.Add(slider_style[1]);

            // Add vertical Slider
            ver_slider_parent = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(150, 150)
                }
            };
            slider_view.Add(ver_slider_parent);
            slider_style[2].Margin = new Extents(0, 0, 30, 0);
            slider_style[2].Name = "slider3";
            slider_style[3].Margin = new Extents(0, 0, 30, 0);
            slider_style[3].Name = "slider4";
            ver_slider_parent.Add(slider_style[2]);
            ver_slider_parent.Add(slider_style[3]);
        }

        private void CreateBottomView()
        {
            bottom_view = new View()
            { 
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                { 
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size2D(100, 100)
                },
            };

            bottom_slider1 = new Slider()
            {
                Name = "slider5",
                Size = new Size((int)root.SizeWidth * 3 / 4, 12),
                ThumbSize = new Size(60, 60),
                ThumbImageUrl = ResourcePath + "tizen.png",
                SlidedTrackColor = Color.Cyan,
                BgTrackColor = Color.Blue,
                TrackThickness = 12,
                MinValue = 0,
                MaxValue = 100,
                ValueIndicatorSize = new Size(30, 30),
                WarningStartValue = 75,
                WarningTrackColor = Color.Red,
                WarningSlidedTrackColor = Color.Black,
                WarningThumbColor = Color.DarkRed,
                WarningThumbImageUrl = ResourcePath + "picture.png",
            };
            bottom_view.Add(bottom_slider1);

            bottom_slider2 = new Slider()
            {
                Name = "slider6",
                Size = new Size((int)root.SizeWidth * 3 / 4, 12),
                ThumbSize = new Size(50, 50),
                ThumbImageURLSelector = new StringSelector
                {
                    All = ResourcePath + "Boston.png",
                },
                IsDiscrete = true,
                DiscreteValue = 20,
                IsValueShown = true,
                Indicator = Slider.IndicatorType.Image,
                ValueIndicatorUrl = ResourcePath + "bg_0.png",
                ValueIndicatorSize = new Size(30, 30),
            };
            bottom_view.Add(bottom_slider2);

            root.Add(bottom_view);
        }

        private Slider CreateByStyle(SliderStyle st, int w, int h, int curValue, Slider.DirectionType dir)
        {
            // input style in construction
            Slider source = new Slider(st);
            source.Direction = dir;
            root.Add(source);
            source.Focusable = true;
            source.MinValue = MIN_VALUE;
            source.MaxValue = MAX_VALUE;
            source.IsValueShown = true;
            source.ControlStateChangedEvent += OnStateChanged;
            source.ValueChanged += OnValueChanged;
            source.SlidingStarted += OnSlidingStarted;
            source.SlidingFinished += OnSlidingFinished;
            source.Size = new Size(w, h);
            source.CurrentValue = curValue;
            return source;
        }

        private void OnValueChanged(object sender, SliderValueChangedEventArgs args)
        {
            Slider source = sender as Slider;
            if (source != null)
            {
                inforText.Text = "currentValue = " + args.CurrentValue;
            }
        }

        private void OnSlidingStarted(object sender, SliderSlidingStartedEventArgs args)
        {
            Slider source = sender as Slider;
            if (source != null)
            {
                inforText.Text = "Started currentValue = " + args.CurrentValue;
            }
        }

        private void OnSlidingFinished(object sender, SliderSlidingFinishedEventArgs args)
        {
            Slider source = sender as Slider;
            if (source != null)
            {
                inforText.Text = "Finished currentValue = " + args.CurrentValue;
            }
        }

        private void OnStateChanged(object sender, ControlStateChangedEventArgs args)
        {
            if (sender is Tizen.NUI.Components.Slider)
            {
                Tizen.NUI.Components.Slider slider = sender as Tizen.NUI.Components.Slider;
                if (slider != null)
                {
                    // Do something
                }
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
                if (inforText != null)
                {
                    inforText.Dispose();
                    inforText = null;
                }

                for (int j = 0; j < 4; j++)
                {
                    if (slider_style[j] != null)
                    {
                        slider_style[j].Dispose();
                        slider_style[j] = null;
                    }
                }

                if (slider_view != null)
                {
                    slider_view.Dispose();
                    slider_view = null;
                }

                if (ver_slider_parent != null)
                {
                    ver_slider_parent.Dispose();
                    ver_slider_parent = null;
                }

                if (hori_slider_parent != null)
                {
                    hori_slider_parent.Dispose();
                    hori_slider_parent = null;
                }
                root.Dispose();
                root = null;
            }
        }
    }

    class SliderTest3 : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new SliderTestPage3(window));
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
