using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class ProgressbarTestPage2 : ContentPage
    {
        private Button[] button = new Button[4];
        private Progress[] progressBar = new Progress[3];
        private View[] layout = new View[3];
        private TextLabel board;
        private TextLabel indeterminateImageUrl;
        private Slider slider;
        Timer AnimationTimer = new Timer(50);

        private static string ResourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "/images/";

        internal ProgressbarTestPage2(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "Progress Sample",
            };

            layout[0] = new View()
            {
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height),
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(50, 50),
                }
            };

            layout[1] = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = window.WindowSize.Height / 2,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(50, 50),
                }
            };
            layout[0].Add(layout[1]);

            CreateElementsView(window);

            Content = layout[0];
        }

        void CreateElementsView(Window window)
        {
            progressBar[0] = new Progress()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 30,
                MaxValue = 100,
                MinValue = 0,
                TrackColor = Color.Cyan,
                ProgressState = Progress.ProgressStatusType.Buffering,
                BufferValue = 30,
                BufferColor = Color.Yellow,
                BufferImageURL = ResourcePath + "cartman.svg",
            };
            layout[1].Add(progressBar[0]);

            button[0] = new Button()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 50,
                Text = "Animate",
                BackgroundColor = Color.Green,
                Focusable = true
            };
            button[0].Clicked += OnAnimateClicked;
            layout[1].Add(button[0]);

            progressBar[1] = new Progress()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 30,
                MaxValue = 100,
                MinValue = 0,
                CurrentValue = 30,
                ProgressColor = Color.Green,
                ProgressImageURL = ResourcePath + "cartman.svg",
                TrackImageURL = ResourcePath + "bg_1.png",
            };
            layout[1].Add(progressBar[1]);

            slider = new Slider()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 20,
                CurrentValue = 30,
                TrackThickness = 4,
                SlidedTrackColor = Color.Cyan,
                ThumbSize = new Size(30, 30),
                ThumbImageURL = ResourcePath + "Boston.png",
            };
            layout[1].Add(slider);
            slider.ValueChanged += OnValueChanged;

            ProgressStyle attr = new ProgressStyle
            {
                Track = new ImageViewStyle
                {
                    BackgroundColor = new Selector<Color>
                    {
                        All = Color.Cyan
                    }
                },
                Progress = new ImageViewStyle
                {
                    BackgroundColor = new Selector<Color>
                    {
                        All = Color.Red
                    }
                },
                IndeterminateImageUrl = ResourcePath + "a.jpg",
            };
            progressBar[2] = new Progress(attr)
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 12,
                MaxValue = 100,
                MinValue = 0,
                CurrentValue = 45,
            };
            progressBar[2].ApplyStyle(attr);
            layout[1].Add(progressBar[2]);

            board = new TextLabel()
            {
                Focusable = true,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 50,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Magenta,
                Text = "log pad",
            };
            layout[1].Add(board);

            layout[2] = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 50,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(50, 50),
                }
            };

            button[1] = new Button()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 50,
                Text = "+",
                BackgroundColor = Color.Green,
                Focusable = true
            };
            button[1].Clicked += ProgressAdd;

            button[2] = new Button()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 50,
                Text = "-",
                BackgroundColor = Color.Green,
                Focusable = true
            };
            button[2].Clicked += ProgressMinus;

            layout[2].Add(button[1]);
            layout[2].Add(button[2]);
            layout[1].Add(layout[2]);

            indeterminateImageUrl = new TextLabel()
            {
                Ellipsis = false,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            indeterminateImageUrl.Text = "IndeterminateImageUrl : " + progressBar[2].IndeterminateImageUrl;
            layout[1].Add(indeterminateImageUrl);

            button[3] = new Button()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 50,
                Text = "ChangeIndeterminateImageUrl",
                BackgroundColor = Color.Green,
                Focusable = true
            };
            button[3].Clicked += OnChangeIndeterminateImageUrlClick;
            layout[1].Add(button[3]);
        }

        private void OnValueChanged(object sender, SliderValueChangedEventArgs e)
        {
            progressBar[1].CurrentValue = slider.CurrentValue;
        }

        private void OnAnimateClicked(object sender, ClickedEventArgs e)
        {
            AnimationTimer.Tick += OnTimerTick;
            AnimationTimer.Start();
        }

        private bool OnTimerTick(object source, Timer.TickEventArgs e)
        {
            progressBar[0].BufferValue += 2.0f;

            if (progressBar[0].BufferValue >= 100.0f)
            {
                AnimationTimer.Stop();
                return false;
            }

            return true;
        }

        private void OnChangeIndeterminateImageUrlClick(object sender, ClickedEventArgs e)
        {
            progressBar[2].IndeterminateImageUrl = ResourcePath + "b.jpg";
            indeterminateImageUrl.Text = "IndeterminateImageUrl : " + progressBar[2].IndeterminateImageUrl;
        }

        private void Board_FocusLost(object sender, global::System.EventArgs e)
        {
            board.BackgroundColor = Color.Magenta;
        }

        private void Board_FocusGained(object sender, global::System.EventArgs e)
        {
            board.BackgroundColor = Color.Cyan;
        }

        private void ProgressAdd(object sender, global::System.EventArgs e)
        {
            if (progressBar[2].CurrentValue == 100)
            {
                board.Text = "Current value is: 100";
            }
            else
            {
                progressBar[2].CurrentValue += 5;
                board.Text = "Current value is: " + progressBar[2].CurrentValue;
            }
        }
        private void ProgressMinus(object sender, global::System.EventArgs e)
        {
            if (progressBar[2].CurrentValue == 0)
            {
                board.Text = "Current value is: 0";
            }
            else
            {
                progressBar[2].CurrentValue -= 5;
                board.Text = "Current value is: " + progressBar[2].CurrentValue;
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
            if (layout[0] != null)
            {
                

                NUIApplication.GetDefaultWindow().Remove(layout[0]);
                layout[0].Dispose();
                layout[0] = null;
            }
        }
    }

    class ProgressbarTest2 : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new ProgressbarTestPage2(window));
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
