using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class SwitchTestPage1 : ContentPage
    {
        private View root;
        private Switch[] utilitySwitch = new Switch[4];
        private View switchLayout;
        private SwitchStyle utilitySt;
        private SwitchImpl switch4;
        private ImageView imageView;

        private Button btn1;
        private Button btn2;
        private Button btn3;

        private TextLabel label1;

        private static string ResourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "/images/";
        private string defaultTrack = ResourcePath + "controller/controller_switch_bg_off.png";
        private string defaultThumb = ResourcePath + "controller/controller_switch_handler.png";

        internal class SwitchImpl : Switch
        {
            public SwitchImpl() : base()
            { 
                
            }

            public ImageView CreateIconImpl()
            {
                return base.CreateIcon();
            }
        }

        internal SwitchTestPage1(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "Switch Sample",
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

            switchLayout = new View()
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size2D(20, 20),
                }
            };

            for (int i = 0; i < 4; i++)
            {
                utilitySwitch[i] = new Switch();
                utilitySt = new SwitchStyle
                {
                    Size = new Size(window.WindowSize.Width / 5, 150),
                    IsSelectable = true,
                    Track = new ImageViewStyle
                    {
                        Size = new Size(window.WindowSize.Width / 5, 50),
                        ResourceUrl = new Selector<string>
                        {
                            All = defaultTrack,
                        },
                        Border = new Rectangle(30, 30, 30, 30),
                    },
                    Thumb = new ImageViewStyle
                    {
                        Size = new Size(50, 50),
                        ResourceUrl = new Selector<string>
                        {
                            All = defaultThumb,
                        },
                    },
                };
                utilitySwitch[i].ApplyStyle(utilitySt);
                switchLayout.Add(utilitySwitch[i]);
            }

            root.Add(switchLayout);

            btn1 = new Button()
            {
                Text = "SwitchBackgroundImageURLSelector",
                BackgroundColor = Color.Blue,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };

            btn2 = new Button()
            { 
                Text = "SwitchHandlerImageURL & Size",
                BackgroundColor = Color.Blue,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };

            btn3 = new Button()
            { 
                Text = "SwitchHandlerImageURLSelector",
                BackgroundColor = Color.Blue,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };

            btn1.Clicked += OnClicked;
            btn2.Clicked += OnClicked;
            btn3.Clicked += OnClicked;

            root.Add(btn1);
            root.Add(btn2);
            root.Add(btn3);

            switch4 = new SwitchImpl()
            {
                Text = "Switch4",
                Size = new Size(window.WindowSize.Width / 5, 100),
            };
            switch4.OnInitialize();
            switch4.Track.Size2D = new Size2D(window.WindowSize.Width / 5, 100);
            switch4.SelectedChanged += OnSelectedChanged;
            root.Add(switch4);

            label1 = new TextLabel()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Ellipsis = false,
                MultiLine = true,
                LineWrapMode = LineWrapMode.Word,
            };
            if (!imageView)
            {
                label1.Text = "imageView is null! Click switch4 to Create imageView";
            }
            root.Add(label1);

            Content = root;
        }

        private void OnSelectedChanged(object sender, SelectedChangedEventArgs e)
        {
            if (imageView)
            {
                label1.Text = "imageView already been created!";
            }
            else
            {
                imageView = switch4.CreateIconImpl();
                label1.Text = "CreateIcon : imageView's ChildCount,  " + imageView.ChildCount;
            }
        }

        private void OnClicked(object sender, ClickedEventArgs e)
        {
            var control = sender as Button;
            var text = control.Text;
            switch (text)
            {
                case "SwitchBackgroundImageURLSelector":
                    utilitySwitch[0].SwitchBackgroundImageURLSelector = new StringSelector()
                    {
                        All = ResourcePath + "controller/controller_switch_bg_on.png",
                    };
                    btn1.IsEnabled = false;
                    break;
                case "SwitchHandlerImageURL & Size":
                    utilitySwitch[1].SwitchHandlerImageURL = ResourcePath + "Boston.png";
                    utilitySwitch[1].SwitchHandlerImageSize = new Size(60, 60);
                    btn2.IsEnabled = false;
                    break;
                case "SwitchHandlerImageURLSelector":
                    utilitySwitch[2].SwitchHandlerImageURLSelector = new StringSelector()
                    {
                        All = ResourcePath + "bg_1.png",
                    };
                    utilitySwitch[2].Thumb.CornerRadius = new Vector4(50, 50, 50, 50);
                    btn3.IsEnabled = false;
                    break;
                default:
                    break;
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
                for (int i = 0; i < 4; i++)
                {
                    switchLayout.Remove(utilitySwitch[i]);
                    utilitySwitch[i].Dispose();
                    utilitySwitch[i] = null;
                }

                root.Remove(switchLayout);
                switchLayout.Dispose();
                switchLayout = null;

                root.Remove(switch4);
                switch4.Dispose();
                switch4 = null;

                root.Remove(btn1);
                btn1.Dispose();
                btn1 = null;

                root.Remove(btn2);
                btn2.Dispose();
                btn2 = null;

                root.Remove(btn3);
                btn3.Dispose();
                btn3 = null;
            }
        }
    }

    class SwitchTest : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new SwitchTestPage1(window));
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
