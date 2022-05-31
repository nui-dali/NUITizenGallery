using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    public class LoadingContentPage1 : ContentPage
    {
        private TextLabel[] textLabel = new TextLabel[3];
        private Button[] button = new Button[3];
        private Loading[] loading = new Loading[2];
        private View root;
        private View gridLayout;
        private View[] layout = new View[4];
        private string[] imageArray;

        private int clickedCount = 0;

        public LoadingContentPage1()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Loading Sample",
            };

            root = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f)
            };
            Content = root;

            gridLayout = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Layout = new GridLayout()
                {
                    Rows = 4,
                    GridOrientation = GridLayout.Orientation.Horizontal,
                }
            };
            root.Add(gridLayout);

            imageArray = new string[36];
            for (int i = 0; i < 36; i++)
            {
                if (i < 10)
                {
                    imageArray[i] = CommonResource.GetResourcePath() + "components/loading/Loading_Sequence_Native/loading_0" + i + ".png";
                }
                else
                {
                    imageArray[i] = CommonResource.GetResourcePath() + "components/loading/Loading_Sequence_Native/loading_" + i + ".png";
                }
            }

            CreatePropLayout();
            CreateAttrLayout();
        }

        void CreatePropLayout()
        {
            //To set spacing between grid cells.
            textLabel[0] = new TextLabel()
            {
                WidthSpecification = 500,
                HeightSpecification = 100,
                Margin = new Extents(0, 20, 0, 100),
                PointSize = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Magenta,
                Text = "Property construction",
            };
            gridLayout.Add(textLabel[0]);

            // layout for loading which is created by properties.
            // It'll update the visual when framerate is changed, so put loading into a layout.
            layout[1] = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                }
            };
            loading[0] = new Loading()
            {
                Size = new Size(100, 100),
                ImageArray = imageArray,
            };
            layout[1].Add(loading[0]);
            gridLayout.Add(layout[1]);

            CreatePropBtnLayout();

            textLabel[1] = new TextLabel()
            {
                PointSize = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Magenta,
                Text = "log pad",
            };
            gridLayout.Add(textLabel[1]);
        }

        void CreatePropBtnLayout()
        {
            // layout for button.
            // To avoid button size same as the grid cell.
            layout[0] = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(10, 50)
                }
            };

            button[0] = new Button()
            {
                Size = new Size(200, 50),
                Text = "FPS++",
                PointSize = 10,
                Focusable = true,
                BackgroundColor = Color.Green,
            };
            layout[0].Add(button[0]);
            button[0].Clicked += propFpsAdd;
            //FocusManager.Instance.SetCurrentFocusView(button[0]);

            button[1] = new Button()
            {
                Size = new Size(200, 50),
                Text = "FPS--",
                PointSize = 10,
                Focusable = true,
                BackgroundColor = Color.Green,
            };
            layout[0].Add(button[1]);
            button[1].Clicked += propFpsMinus;
            //FocusManager.Instance.SetCurrentFocusView(button[1]);

            gridLayout.Add(layout[0]);

            button[0].RightFocusableView = button[1];
            button[1].LeftFocusableView = button[0];
        }

        private void CreateAttrLayout()
        {
            textLabel[2] = new TextLabel()
            {
                PointSize = 10,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Magenta,
                Text = "Attribute construction",
            };
            gridLayout.Add(textLabel[2]);

            // layout for loading which is created by attributes.
            // It'll update the visual when framerate is changed, so put loading into a layout.
            layout[2] = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                }
            };
            LoadingStyle style = new LoadingStyle
            {
                Images = imageArray
            };
            loading[1] = new Loading(style)
            {
                Size = new Size(100, 100),
            };
            layout[2].Add(loading[1]);
            gridLayout.Add(layout[2]);

            // layout for button.
            // To avoid button size same as the grid cell.
            layout[3] = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                }
            };
            button[2] = new Button()
            {
                Size = new Size(400, 50),
                Text = "Pause",
                PointSize = 10,
                Focusable = true,
                BackgroundColor = Color.Green,
            };
            layout[3].Add(button[2]);
            gridLayout.Add(layout[3]);
            //FocusManager.Instance.SetCurrentFocusView(button[2]);

            clickedCount = 0;
            button[2].Clicked += loadingPlayPauseStop;
        }

        private void loadingPlayPauseStop(object sender, global::System.EventArgs e)
        {
            if (clickedCount % 2 == 0) // There is no way to check the state of loading.
            {
                loading[1].Pause();
                button[2].Text = "Play";
            }
            else if (clickedCount % 2 == 1)
            {
                loading[1].Play();
                button[2].Text = "Pause";
            }

            clickedCount++;
        }

        private void propFpsAdd(object sender, global::System.EventArgs e)
        {
            loading[0].FrameRate += 1;
            textLabel[1].Text = "loading1_1 FPS: " + loading[0].FrameRate.ToString();
        }

        private void propFpsMinus(object sender, global::System.EventArgs e)
        {
            loading[0].FrameRate -= 1;
            textLabel[1].Text = "loading1_1 FPS: " + loading[0].FrameRate.ToString();
        }

        public void Deactivate()
        {
            if (root != null)
            {
                layout[0].Remove(button[0]);
                button[0].Dispose();
                button[0] = null;

                layout[0].Remove(button[1]);
                button[1].Dispose();
                button[1] = null;

                gridLayout.Remove(layout[0]);
                layout[0].Dispose();
                layout[0] = null;

                gridLayout.Remove(textLabel[0]);
                textLabel[0].Dispose();
                textLabel[0] = null;

                // stop firstly
                loading[0].Stop();

                layout[1].Remove(loading[0]);
                loading[0].Dispose();
                loading[0] = null;

                gridLayout.Remove(layout[1]);
                layout[1].Dispose();
                layout[1] = null;

                gridLayout.Remove(textLabel[1]);
                textLabel[1].Dispose();
                textLabel[1] = null;

                gridLayout.Remove(textLabel[2]);
                textLabel[2].Dispose();
                textLabel[2] = null;

                // stop firstly
                loading[1].Stop();

                layout[2].Remove(loading[1]);
                loading[1].Dispose();
                loading[1] = null;

                gridLayout.Remove(layout[2]);
                layout[2].Dispose();
                layout[2] = null;

                layout[3].Remove(button[2]);
                button[2].Dispose();
                button[2] = null;

                gridLayout.Remove(layout[3]);
                layout[3].Dispose();
                layout[3] = null;

                root.Remove(gridLayout);
                gridLayout.Dispose();
                gridLayout = null;
            }
        }
    }

    class LoadingTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new LoadingContentPage1());
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
