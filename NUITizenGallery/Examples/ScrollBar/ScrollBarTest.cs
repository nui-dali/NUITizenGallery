using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class ScrollBarTestPage : ContentPage
    {
        private View root;
        private TextLabel text_nullstyle, text_style, scroll_position;
        private Button[] button = new Button[4];
        private Scrollbar[] scrollbar = new Scrollbar[3];
        private View top_parent, bottom_parent, null_style_parent, style_parent;
        private float contentLength = 100.0f;
        private int pos = 0;

        private static string ResourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "/images/";

        internal ScrollBarTestPage(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "Scollbar Sample",
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
                    CellPadding = new Size(10, 20),
                }
            };

            // TextLabel of Null style construction and Style construction
            CreateTopView();

            // Buttons for moving thumbnail
            CreateBottomView();

            Content = root;
        }

        private void CreateTopView()
        {
            top_parent = new View()
            {
                Size = new Size(root.SizeWidth, root.SizeHeight / 2),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    CellPadding = new Size2D(20, 20)
                }
            };
            root.Add(top_parent);
            CreateNullStylePart();
            CreateStylePart();
        }

        private void CreateNullStylePart()
        {
            null_style_parent = new View()
            {
                Size = new Size(root.SizeWidth / 2, root.SizeHeight  / 2)
            };
            null_style_parent.Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                CellPadding = new Size2D(0, 100)
            };
            top_parent.Add(null_style_parent);

            // Add Textlabel of "Null style construction"
            text_nullstyle = new TextLabel();
            text_nullstyle.Margin = new Extents(0, 0, 100, 0);
            text_nullstyle.Size = new Size(root.SizeWidth /2, 70);
            text_nullstyle.HorizontalAlignment = HorizontalAlignment.Center;
            text_nullstyle.VerticalAlignment = VerticalAlignment.Center;
            text_nullstyle.BackgroundColor = Color.Magenta;
            text_nullstyle.Text = "Null style construction";
            text_nullstyle.Focusable = true;
            null_style_parent.Add(text_nullstyle);

            // Add ScrollBar of Null style construction
            scrollbar[0] = new Scrollbar(contentLength, 2, 20, true);
            scrollbar[0].Size = new Size(root.SizeWidth / 2, 2);
            scrollbar[0].TrackColor = Color.Cyan;
            scrollbar[0].TrackThickness = 5.0f;
            scrollbar[0].TrackPadding = new Extents(20, 20, 20, 0); // start, end, top, bottom
            scrollbar[0].ThumbColor = Color.Red;
            scrollbar[0].ThumbThickness = 25.0f;
            null_style_parent.Add(scrollbar[0]);

            scrollbar[1] = new Scrollbar(contentLength, 2, 40, true);
            scrollbar[1].Size = new Size(root.SizeWidth / 2, 2);
            scrollbar[1].TrackColor = Color.Cyan;
            scrollbar[1].TrackThickness = 5.0f;
            scrollbar[1].ThumbColor = Color.Green;
            scrollbar[1].ThumbHorizontalImageUrl = ResourcePath + "tizen.png";
            scrollbar[1].ThumbThickness = 25.0f;
            null_style_parent.Add(scrollbar[1]);
        }

        private void CreateStylePart()
        {
            style_parent = new View()
            {
                Size = new Size(root.SizeWidth / 2, root.SizeHeight / 2)
            };
            style_parent.Layout = new LinearLayout()
            {
                LinearOrientation = LinearLayout.Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                CellPadding = new Size2D(0, 20)
            };
            top_parent.Add(style_parent);

            // Add Textlabel of "Style construction"
            text_style = new TextLabel();
            text_style.Margin = new Extents(0, 0, 100, 0);
            text_style.Size = new Size(root.SizeWidth / 2, 70);
            text_style.HorizontalAlignment = HorizontalAlignment.Center;
            text_style.VerticalAlignment = VerticalAlignment.Center;
            text_style.BackgroundColor = Color.Magenta;
            text_style.Text = "Style construction";
            text_style.Focusable = true;
            style_parent.Add(text_style);

            // Add ScrollBar of Style construction
            scrollbar[2] = new Scrollbar(contentLength, 2, 30, false);
            ScrollbarStyle st = new ScrollbarStyle
            {
                TrackColor = Color.Cyan,
                TrackThickness = 5.0f,
                ThumbColor = Color.Blue,
                ThumbThickness = 25.0f
            };
            scrollbar[2].ApplyStyle(st);
            scrollbar[2].Size = new Size(2, root.SizeHeight * 2 / 5);
            scrollbar[2].ThumbVerticalImageUrl = ResourcePath + "Boston.png";
            style_parent.Add(scrollbar[2]);
        }

        private void CreateBottomView()
        {
            bottom_parent = new View()
            {
                Size = new Size(root.SizeWidth, root.SizeHeight / 2),
                Layout = new FlexLayout()
                {
                    Alignment = FlexLayout.AlignmentType.Center,
                    ItemsAlignment = FlexLayout.AlignmentType.Center,
                    Direction = FlexLayout.FlexDirection.Row,
                    Justification = FlexLayout.FlexJustification.Center,
                    WrapType = FlexLayout.FlexWrapType.Wrap
                },
            };
            root.Add(bottom_parent);

            // Create buttons
            CreateButtons();

            scroll_position = new TextLabel();
            scroll_position.Text = "scrollbar[0] : ScrollPosition , " + scrollbar[0].ScrollPosition + ", ScrollCurrentPosition : " + scrollbar[0].ScrollCurrentPosition;
            bottom_parent.Add(scroll_position);
        }

        void CreateButtons()
        {
            for (int i = 0; i < 4; i++)
            {
                button[i] = new Button();
                button[i].BackgroundColor = Color.Green;
                button[i].Size = new Size(root.SizeWidth / 4, 50);
                button[i].Focusable = true;
                bottom_parent.Add(button[i]);
            }

            button[0].Text = "+";
            button[0].Clicked += Scroll1Add;

            button[1].Text = "-";
            button[1].Clicked += Scroll1Minus;

            button[2].Text = "ScrollMove";
            button[2].Clicked += Scroll1_2move;

            button[3].Text = "ScrollUpdate";
            button[3].Clicked += Scroll1_2Changed;

            // Set focus to Add Button
            FocusManager.Instance.SetCurrentFocusView(button[0]);
        }

        private void Scroll1Add(object sender, global::System.EventArgs e)
        {
            scrollbar[0].ScrollTo(scrollbar[0].ScrollPosition + 3, 1, null);
            scrollbar[2].ScrollTo(scrollbar[2].ScrollPosition + 3, 1, null);

            scroll_position.Text = "scrollbar[0] : ScrollPosition , " + scrollbar[0].ScrollPosition + ", ScrollCurrentPosition : " + scrollbar[0].ScrollCurrentPosition;
        }
        private void Scroll1Minus(object sender, global::System.EventArgs e)
        {
            scrollbar[0].ScrollTo(scrollbar[0].ScrollPosition - 3, 1, null);
            scrollbar[2].ScrollTo(scrollbar[2].ScrollPosition - 3, 1, null);

            scroll_position.Text = "scrollbar[0] : ScrollPosition , " + scrollbar[0].ScrollPosition + ", ScrollCurrentPosition : " + scrollbar[0].ScrollCurrentPosition;
        }

        private void Scroll1_2Changed(object sender, global::System.EventArgs e)
        {
            pos = (int)scrollbar[2].ScrollPosition + 10;
            if (pos >= 100) pos = 0;
            scrollbar[2].Update(100, 4, pos, 0, new AlphaFunction(AlphaFunction.BuiltinFunctions.Bounce));
        }

        private void Scroll1_2move(object sender, global::System.EventArgs e)
        {
            if (scrollbar[1].ScrollPosition < contentLength / 2)
            {
                scrollbar[1].ScrollTo((contentLength - 2), 1);
            }
            else
            {
                scrollbar[1].ScrollTo(2.0f, 1);
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
                NUIApplication.GetDefaultWindow().Remove(root);
                if (text_nullstyle != null)
                {
                    text_nullstyle.Dispose();
                    text_nullstyle = null;
                }

                if (text_style != null)
                {
                    text_style.Dispose();
                    text_style = null;
                }

                for (int i = 0; i < 4; i++)
                {
                    if (button[i] != null)
                    {
                        button[i].Dispose();
                        button[i] = null;
                    }
                }

                for (int i = 0; i < 3; i++)
                {
                    if (scrollbar[i] != null)
                    {
                        scrollbar[i].Dispose();
                        scrollbar[i] = null;
                    }
                }

                if (top_parent != null)
                {
                    top_parent.Dispose();
                    top_parent = null;
                }

                if (bottom_parent != null)
                {
                    bottom_parent.Dispose();
                    bottom_parent = null;
                }

                if (null_style_parent != null)
                {
                    null_style_parent.Dispose();
                    null_style_parent = null;
                }

                if (style_parent != null)
                {
                    style_parent.Dispose();
                    style_parent = null;
                }

                root.Dispose();
                root = null;
            }
        }
    }

    class ScrollBarTest : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new ScrollBarTestPage(window));
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
