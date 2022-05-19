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
        private View parent2;
        private View parent3;
        Button textButton;
        Button iconButton;
        Button iconTextButton;
        Button utilityBasicButton;

        internal ButtonPage8(Window window)
        {
            window.KeyEvent += Window_KeyEvent;

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
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    CellPadding = new Size(50, 50),
                }
            };

            Content = root;

            parent1 = new View()
            {
                Size = new Size(300, 900),
                Layout = new LinearLayout()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size(50, 50),
                }
            };

            // Only show a text button.
            textButton = new Button()
            {
                BackgroundImage = CommonResource.GetResourcePath() + "components/c_buttonbasic/c_basic_button_white_bg_normal_9patch.png",
                BackgroundImageBorder = new Rectangle(4, 4, 5, 5),
                Size = new Size(300, 80),
            };
            textButton.TextLabel.Text = "Button";
            parent1.Add(textButton);

            parent2 = new View()
            {
                Size = new Size(300, 900),
                Layout = new LinearLayout()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size(50, 50),
                }
            };

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
            iconButton.Clicked += (ojb, e) => {
                var btn = iconButton.Icon.GetParent() as Button;
                string name = btn.Name;
            };

            parent3 = new View()
            {
                Size = new Size(600, 400),
                Layout = new LinearLayout()
                {
                    VerticalAlignment = VerticalAlignment.Top,
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    CellPadding = new Size(50, 50),
                }
            };

            //Show a button with icon and text.
            iconTextButton = new Button()
            {
                Text = "IconTextButton",
                BackgroundImage = CommonResource.GetResourcePath() + "components/c_buttonbasic/c_basic_button_white_bg_normal_9patch.png",
                BackgroundImageBorder = new Rectangle(4, 4, 5, 5),
                IconRelativeOrientation = Button.IconOrientation.Left,
                IconPadding = new Extents(20, 20, 20, 20),
                TextPadding = new Extents(20, 50, 20, 20),
                Size = new Size(500, 300),
            };
            iconTextButton.Icon.ResourceUrl = CommonResource.GetResourcePath() + "components/c_radiobutton/c_radiobutton_white_check.png";
            parent3.Add(iconTextButton);

            ///////////////////////////////////////////////Create by Property//////////////////////////////////////////////////////////
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
                    PointSize = 20,
                },
                BackgroundImage = CommonResource.GetResourcePath() + "components/button/rectangle_btn_normal.png",
                BackgroundImageBorder = new Rectangle(5, 5, 5, 5),
            };
            utilityBasicButton = new Button();
            utilityBasicButton.ApplyStyle(utilityBasicButtonStyle);
            utilityBasicButton.IsSelectable = true;
            utilityBasicButton.ImageShadow = new ImageShadow(CommonResource.GetResourcePath() + "components/button/rectangle_btn_shadow.png", new Rectangle(5, 5, 5, 5));
            utilityBasicButton.OverlayImage.Border = new Rectangle(5, 5, 5, 5);
            utilityBasicButton.Size = new Size(300, 80);
            utilityBasicButton.IsEnabled = false;
            parent1.Add(utilityBasicButton);

            // Add three layout into root
            root.Add(parent1);
            root.Add(parent2);
            root.Add(parent3);
        }

        private void Window_KeyEvent(object sender, Window.KeyEventArgs e)
        {
            if(e.Key.State == Key.StateType.Up)
            {
                switch(e.Key.KeyPressedName)
                {
                    case "1":
                        iconTextButton.IconRelativeOrientation = Button.IconOrientation.Right;
                        break;
                    case "2":
                        iconTextButton.IconRelativeOrientation = Button.IconOrientation.Top;
                        break;
                    case "3":
                        iconTextButton.IconRelativeOrientation = Button.IconOrientation.Bottom;
                        break;
                    case "4":
                        iconTextButton.IconRelativeOrientation = Button.IconOrientation.Left;
                        break;
                    case "5":
                        iconTextButton.Icon.Padding.Start = 50;
                        break;
                    case "6":
                        iconTextButton.Icon.Padding.End = 50;
                        break;
                    case "7":
                        iconTextButton.LayoutDirection = ViewLayoutDirectionType.RTL;
                        break;
                    case "8":
                        iconTextButton.LayoutDirection = ViewLayoutDirectionType.LTR;
                        break;
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
                parent1.Remove(textButton);
                textButton.Dispose();
                textButton = null;
                parent1.Remove(utilityBasicButton);
                utilityBasicButton.Dispose();
                utilityBasicButton = null;

                parent3.Remove(iconTextButton);
                iconTextButton.Dispose();
                iconTextButton = null;

                root.Remove(parent1);
                parent1.Dispose();
                parent1 = null;

                root.Remove(parent2);
                parent2.Dispose();
                parent2 = null;

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
