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
        private TextLabel[] board = new TextLabel[3];
        private Button[] button = new Button[3];
        private Progress[] progressBar = new Progress[3];
        private View[] layout = new View[5];
        private TextLabel indeterminateImageUrl;

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
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height / 2 - 25),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(50, 50),
                }
            };
            layout[0].Add(layout[1]);

            // Layout for progress layout which is created by properties.
            layout[2] = new View()
            {
                Size = new Size(window.WindowSize.Width / 2 - 25, window.WindowSize.Height / 2 - 25),
                Margin = new Extents(0, 0, 100, 0),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size2D(50, 50)
                }
            };
            CreatePropElements(window);
            layout[1].Add(layout[2]);

            // Layout for progress layout which is created by attributes.
            layout[3] = new View()
            {
                Size = new Size(window.WindowSize.Width / 2 - 25, window.WindowSize.Height / 2 - 25),
                Margin = new Extents(0, 0, 100, 0),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size2D(50, 50)
                }
            };
            CreateAttrElements(window);
            layout[1].Add(layout[3]);
            
            layout[4] = new View()
            {
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height / 2 - 25),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(10, 50),
                }
            };
            CreateBottomView(window);
            layout[0].Add(layout[4]);

            Content = layout[0];
        }

        void CreatePropElements(Window window)
        {
            ///////////////////////////////////////////////Create by Properties//////////////////////////////////////////////////////////
            board[1] = new TextLabel()
            {
                Focusable = true,
                WidthSpecification = window.WindowSize.Width / 2 - 25,
                HeightSpecification = window.WindowSize.Height / 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Magenta,
                Text = "Property construction"
            };
            layout[2].Add(board[1]);
            board[1].FocusGained += Board_FocusGained;
            board[1].FocusLost += Board_FocusLost;

            progressBar[0] = new Progress()
            {
                WidthSpecification = window.WindowSize.Width / 2 - 25,
                HeightSpecification = 20,
                MaxValue = 100,
                MinValue = 0,
                TrackColor = Color.Cyan,
                ProgressState = Progress.ProgressStatusType.Buffering,
                BufferValue = 45,
                BufferColor = Color.Yellow,
                BufferImageURL = ResourcePath + "clock_tabs_ic_stopwatch.png"
            };
            layout[2].Add(progressBar[0]);

            progressBar[1] = new Progress()
            {
                WidthSpecification = window.WindowSize.Width / 2 - 25,
                HeightSpecification = 12,
                MaxValue = 100,
                MinValue = 0,
                CurrentValue = 30,
                ProgressColor = Color.Blue,
                ProgressImageURL = ResourcePath + "clock_tabs_ic_stopwatch.png",
                TrackImageURL = ResourcePath + "bg_1.png",
            };
            layout[2].Add(progressBar[1]);
        }

        private void CreateAttrElements(Window window)
        {
            ///////////////////////////////////////////////Create by attributes//////////////////////////////////////////////////////////
            board[2] = new TextLabel()
            {
                Focusable = true,
                WidthSpecification = window.WindowSize.Width / 2 - 25,
                HeightSpecification = window.WindowSize.Height / 12,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Magenta,
                Text = "Attribute construction"
            };
            layout[3].Add(board[2]);
            board[2].FocusGained += Board_FocusGained;
            board[2].FocusLost += Board_FocusLost;

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
            };
            progressBar[2] = new Progress(attr)
            {
                WidthSpecification = window.WindowSize.Width / 2 - 25,
                HeightSpecification = 12,
                MaxValue = 100,
                MinValue = 0,
                TrackColor = Color.Green,
                CurrentValue = 45,
                ProgressColor = Color.Red,
                IndeterminateImageUrl = ResourcePath + "a.jpg",
            };
            progressBar[2].OnInitialize();
            layout[3].Add(progressBar[2]);

            indeterminateImageUrl = new TextLabel()
            { 
                Ellipsis = false,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
                WidthSpecification = window.WindowSize.Width / 2 - 25,
            };
            indeterminateImageUrl.Text = "IndeterminateImageUrl : " + progressBar[2].IndeterminateImageUrl;
            layout[3].Add(indeterminateImageUrl);
        }

        private void CreateBottomView(Window window)
        {
            View buttonArea = new View()
            {
                Size = new Size(window.WindowSize.Width, 80),
                Layout = new GridLayout()
                {
                    Columns = 3,
                }
            };

            button[0] = new Button()
            {
                WidthSpecification = 140,
                HeightSpecification = 50,
                Text = "+",
                BackgroundColor = Color.Green,
                Focusable = true
            };
            button[0].Clicked += ProgressAdd;

            button[1] = new Button()
            {
                WidthSpecification = 140,
                HeightSpecification = 50,
                Text = "-",
                BackgroundColor = Color.Green,
                Focusable = true
            };
            button[1].Clicked += ProgressMinus;

            button[2] = new Button()
            {
                Text = "ChangeIndeterminateImageUrl",
                BackgroundColor = Color.Green,
                Focusable = true
            };
            button[2].Clicked += OnChangeIndeterminateImageUrlClick;

            buttonArea.Add(button[0]);
            buttonArea.Add(button[1]);
            buttonArea.Add(button[2]);
            layout[4].Add(buttonArea);

            board[0] = new TextLabel()
            {
                Focusable = true,
                WidthSpecification = window.WindowSize.Width * 2 / 3,
                HeightSpecification = 80,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Magenta,
                Text = "log pad",
            };
            board[0].FocusGained += Board_FocusGained;
            board[0].FocusLost += Board_FocusLost;

            layout[4].Add(board[0]);
            FocusManager.Instance.SetCurrentFocusView(button[0]);
        }

        private void OnChangeIndeterminateImageUrlClick(object sender, ClickedEventArgs e)
        {
            progressBar[2].IndeterminateImageUrl = ResourcePath + "b.jpg";
            indeterminateImageUrl.Text = "IndeterminateImageUrl : " + progressBar[2].IndeterminateImageUrl;
        }

        private void Board_FocusLost(object sender, global::System.EventArgs e)
        {
            board[0].BackgroundColor = Color.Magenta;
        }

        private void Board_FocusGained(object sender, global::System.EventArgs e)
        {
            board[0].BackgroundColor = Color.Cyan;
        }

        private void ProgressAdd(object sender, global::System.EventArgs e)
        {
            if (progressBar[2].CurrentValue == 100)
            {
                board[0].Text = "Current value is: 100";
            }
            else
            {
                board[0].Text = "Current value is: " + ++progressBar[2].CurrentValue;
            }
        }
        private void ProgressMinus(object sender, global::System.EventArgs e)
        {
            if (progressBar[2].CurrentValue == 0)
            {
                board[0].Text = "Current value is: 0";
            }
            else
            {
                board[0].Text = "Current value is: " + --progressBar[2].CurrentValue;
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
