using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class ButtonPage8 : ContentPage
    {
        private View root;
        private View parent1;
        private Button textButton;
        private Button utilityBasicButton;

        private View parent2;
        private Button iconButton;
        private Button iconButton2;

        private View parent3;
        private Button iconTextButton;
        private int clickedCount;
        private int clickedCount1;

        internal ButtonPage8(Window window)
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Button Sample",
            };

            root = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    HorizontalAlignment = HorizontalAlignment.Begin,
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    CellPadding = new Size(50, 50),
                }
            };

            Content = root;

            parent1 = new View()
            {
                Size = new Size(200, 600),
                Layout = new LinearLayout()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size(50, 50),
                }
            };
            root.Add(parent1);

            // Only show a text button.
            textButton = new Button()
            {
                BackgroundImage = CommonResource.GetResourcePath() + "components/c_buttonbasic/c_basic_button_white_bg_normal_9patch.png",
                BackgroundImageBorder = new Rectangle(4, 4, 5, 5),
                Size = new Size(200, 80),
                PointSize = 16,
                FontFamily = "Samsung200",
                TextColor = Color.Magenta,
                TextAlignment = HorizontalAlignment.Center,
                IsSelectable = true,
                IsSelected = true,
            };
            textButton.TextLabel.Text = "Button";
            parent1.Add(textButton);

            clickedCount1 = 0;
            textButton.Clicked += (ojb, e) => {
                if (clickedCount1 % 2 == 0)
                {
                    textButton.TextSelector = new StringSelector()
                    {
                        All = "test",
                    };
                    textButton.PointSizeSelector = new FloatSelector()
                    { 
                        All = 14,
                    };
                    textButton.TextColorSelector = new ColorSelector()
                    {
                        All = Color.Blue,
                    };
                }
                else
                {
                    textButton.TextSelector = new StringSelector()
                    {
                        All = "Button",
                    };
                    textButton.PointSizeSelector = new FloatSelector()
                    {
                        All = 16,
                    };
                    textButton.TextColorSelector = new ColorSelector()
                    {
                        All = Color.Magenta,
                    };
                }
                clickedCount1++;
            };

            //Create utility basic style of button.
            var utilityBasicButtonStyle = new ButtonStyle()
            {
                Overlay = new ImageViewStyle()
                {
                    ResourceUrl = new Selector<string>
                    {
                        Pressed = CommonResource.GetResourcePath() + "components/button/rectangle_btn_press_overlay.png",
                        Other = ""
                    },
                    Border = new Rectangle(5, 5, 5, 5)
                },
                Text = new TextLabelStyle()
                {
                    TextColor = new Selector<Color>
                    {
                        Normal = new Color(0, 0, 0, 1),
                        Pressed = new Color(0, 0, 0, 0.7f),
                        Selected = new Color(0.058f, 0.631f, 0.92f, 1),
                        Disabled = new Color(0, 0, 0, 0.4f)
                    },
                    Text = "UtilityBasicButton",
                    PointSize = 16,
                },
                BackgroundImage = CommonResource.GetResourcePath() + "components/button/rectangle_btn_normal.png",
                BackgroundImageBorder = new Rectangle(5, 5, 5, 5),
            };
            utilityBasicButton = new Button();
            utilityBasicButton.ApplyStyle(utilityBasicButtonStyle);
            utilityBasicButton.IsSelectable = true;
            utilityBasicButton.ImageShadow = new ImageShadow(CommonResource.GetResourcePath() + "components/button/rectangle_btn_shadow.png", new Rectangle(5, 5, 5, 5));
            utilityBasicButton.OverlayImage.Border = new Rectangle(5, 5, 5, 5);
            utilityBasicButton.Size = new Size(200, 80);
            utilityBasicButton.IsEnabled = false;
            parent1.Add(utilityBasicButton);

            parent2 = new View()
            {
                Size = new Size(200, 600),
                Layout = new LinearLayout()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size(50, 50),
                }
            };
            root.Add(parent2);

            //Only show an icon button.
            iconButton = new Button()
            {
                Text = "",
                Name = "IconButton",
                BackgroundImage = CommonResource.GetResourcePath() + "components/c_buttonbasic/c_basic_button_white_bg_normal_9patch.png",
                BackgroundImageBorder = new Rectangle(4, 4, 5, 5),
                Size = new Size(80, 80),
            };
            iconButton.Icon.ResourceUrl = CommonResource.GetResourcePath() + "components/c_radiobutton/c_radiobutton_white_check.png";
            parent2.Add(iconButton);

            iconButton2 = new Button()
            {
                IconURL = CommonResource.GetResourcePath() + "components/c_buttonbasic/c_basic_button_white_bg_normal_9patch.png",
                IconSize = new Size(80, 80),
                Name = "IconButton",
                BackgroundImageBorder = new Rectangle(4, 4, 5, 5),
                Size = new Size(80, 80),
            };
            iconButton2.Icon.ResourceUrl = CommonResource.GetResourcePath() + "components/c_radiobutton/c_radiobutton_white_check.png";
            parent2.Add(iconButton2);

            parent3 = new View()
            {
                Size = new Size(300, 600),
                Layout = new LinearLayout()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size(50, 50),
                }
            };
            root.Add(parent3);

            //Show a button with icon and text.
            iconTextButton = new Button()
            {
                Text = "IconTextButton",
                BackgroundImage = CommonResource.GetResourcePath() + "components/c_buttonbasic/c_basic_button_white_bg_normal_9patch.png",
                BackgroundImageBorder = new Rectangle(4, 4, 5, 5),
                IconRelativeOrientation = Button.IconOrientation.Left,
                IconPadding = new Extents(20, 20, 20, 20),
                TextPadding = new Extents(20, 50, 20, 20),
                ItemAlignment = LinearLayout.Alignment.Begin,
                ItemSpacing = new Size2D(50, 50),
                Size = new Size(300, 200),
            };
            iconTextButton.Icon.ResourceUrl = CommonResource.GetResourcePath() + "components/c_radiobutton/c_radiobutton_white_check.png";
            parent3.Add(iconTextButton);

            clickedCount = 0;

            iconTextButton.Clicked += (ojb, e) => {
                clickedCount++;
                if (clickedCount % 4 == 0)
                {
                    iconTextButton.IconRelativeOrientation = Button.IconOrientation.Right;
                }
                else if (clickedCount % 4 == 1)
                {
                    iconTextButton.IconRelativeOrientation = Button.IconOrientation.Top;
                }
                else if (clickedCount % 4 == 2)
                {
                    iconTextButton.IconRelativeOrientation = Button.IconOrientation.Bottom;
                }
                else
                {
                    iconTextButton.IconRelativeOrientation = Button.IconOrientation.Left;
                }
            };
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
                parent1.Remove(textButton);
                textButton.Dispose();
                textButton = null;

                parent1.Remove(utilityBasicButton);
                utilityBasicButton.Dispose();
                utilityBasicButton = null;

                root.Remove(parent1);
                parent1.Dispose();
                parent1 = null;

                root.Remove(parent2);
                parent2.Dispose();
                parent2 = null;

                parent3.Remove(iconTextButton);
                iconTextButton.Dispose();
                iconTextButton = null;

                root.Remove(parent3);
                parent3.Dispose();
                parent3 = null;
            }
        }
    }
    
    class ButtonTest8 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new ButtonPage8(window));
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
