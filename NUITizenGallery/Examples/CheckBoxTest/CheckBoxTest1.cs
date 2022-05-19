using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    public class CheckBoxContentPage : ContentPage
    {
        private View root;
        private View left;
        private View leftbody;

        private TextLabel createText = null;

        private CheckBoxGroup group = null;

        private TextLabel[] modeText = new TextLabel[4];
        private CheckBox[] utilityCheckBox = new CheckBox[4];

        private static string[] mode = new string[]
        {
            "Utility0",
            "Utility1",
            "Utility2",
            "Utility3"
        };

        public CheckBoxContentPage()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "CheckBox Sample",
            };

            root = new View()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Padding = new Extents(40, 40, 40, 40),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    CellPadding = new Size(40, 40),
                    HorizontalAlignment = HorizontalAlignment.Center,
                }
            };
            Content = root;

            ///////////////////////////////////////////////Create by Property//////////////////////////////////////////////////////////
            left = new View()
            {
                Size = new Size(920, 800),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                },
            };

            //Create description text.
            createText = new TextLabel()
            {
                Text = "Create CheckBox just by properties",
                TextColor = Color.White,
                Size = new Size(800, 100),
            };
            left.Add(createText);

            leftbody = new View()
            {
                Layout = new GridLayout()
                {
                    Columns = 4,
                },
            };

            group = new CheckBoxGroup();

            for (int i = 0; i < 4; i++)
            {
                modeText[i] = new TextLabel()
                {
                    Text = mode[i],
                    Size = new Size(200, 48),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                leftbody.Add(modeText[i]);
                
                //Create utility radio button.
                utilityCheckBox[i] = new CheckBox();
                utilityCheckBox[i].SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    Log.Info(this.GetType().Name, $"Left {i + 1}th Utility CheckBox's IsSelected is changed to {args.IsSelected}.");
                };
                var utilityStyle = utilityCheckBox[i].Style;
                utilityStyle.Icon.Opacity = new Selector<float?>
                {
                    Normal = 1.0f,
                    Selected = 1.0f,
                    Disabled = 0.4f,
                    DisabledSelected = 0.4f
                };
                utilityStyle.Icon.BackgroundImage = new Selector<string>
                {
                    Normal = CommonResource.GetResourcePath() + "components/checkbox/controller_btn_check_off.png",
                    Selected = CommonResource.GetResourcePath() + "components/checkbox/controller_btn_check_on.png",
                    Disabled = CommonResource.GetResourcePath() + "components/checkbox/controller_btn_check_off.png",
                    DisabledSelected = CommonResource.GetResourcePath() + "components/checkbox/controller_btn_check_on.png",
                };
                utilityStyle.Icon.ResourceUrl = new Selector<string>
                {
                    Normal = "",
                    Selected = CommonResource.GetResourcePath() + "components/checkbox/controller_btn_check.png",
                    Disabled = "",
                    DisabledSelected = CommonResource.GetResourcePath() + "components/checkbox/controller_btn_check.png",
                };
                utilityStyle.Icon.ImageShadow = new Selector<ImageShadow>
                {
                    Normal = "",
                    Selected = CommonResource.GetResourcePath() + "components/checkbox/controller_btn_check_shadow.png",
                    Disabled = "",
                    DisabledSelected = CommonResource.GetResourcePath() + "components/checkbox/controller_btn_check_shadow.png",
                };
                utilityCheckBox[i].ApplyStyle(utilityStyle);

                utilityCheckBox[i].Size = new Size(48, 48);
                utilityCheckBox[i].Margin = new Extents(76, 76, 25, 25);
                utilityCheckBox[i].Icon.Size = new Size(48, 48);

                group.Add(utilityCheckBox[i]);
                leftbody.Add(utilityCheckBox[i]);
            }

            root.Add(left);
            left.Add(leftbody);

            utilityCheckBox[2].IsEnabled = false;
            utilityCheckBox[3].IsEnabled = false;
            utilityCheckBox[3].IsSelected = true;
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
                    leftbody.Remove(utilityCheckBox[i]);
                    utilityCheckBox[i].Dispose();
                    utilityCheckBox[i] = null;

                    leftbody.Remove(modeText[i]);
                    modeText[i].Dispose();
                    modeText[i] = null;
                }

                left.Remove(createText);
                createText.Dispose();
                createText = null;
                left.Remove(leftbody);
                leftbody.Dispose();
                leftbody = null;

                root.Remove(left);
                left.Dispose();
                left = null;
            }
        }
    }

    class CheckBoxTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new CheckBoxContentPage());
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
