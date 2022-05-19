using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class TestDialogPage : DialogPage
    {
        public TestDialogPage() : base()
        {
            Scrim = new View()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
                BackgroundColor = Color.Green,
            };
        }
    }

    public class DialogPageContentPage2 : ContentPage
    {
        private Window window = null;

        public DialogPageContentPage2(Window win)
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Dialog page Sample",
            };

            window = win;

            var button = new Button()
            {
                Text = "Click to show Dialog",
                WidthSpecification = 400,
                HeightSpecification = 100,
                ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                PivotPoint = Tizen.NUI.PivotPoint.Center,
                PositionUsesPivotPoint = true,
            };

            button.Clicked += (object sender, ClickedEventArgs e) =>
            {
                var dialog = new TestDialogPage()
                {
                    WidthSpecification = 600,
                    HeightSpecification = 600,
                    EnableScrim = true,
                    EnableDismissOnScrim = true,
                    ScrimColor = Color.Green,
                    ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                    PivotPoint = Tizen.NUI.PivotPoint.Center,
                    PositionUsesPivotPoint = true,
                };

                var textLabel = new TextLabel("Message")
                {
                    BackgroundColor = Color.White,
                    Size = new Size(180, 180),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                var positiveButton = new Button()
                {
                    Text = "Yes",
                };
                positiveButton.Clicked += (object s1, ClickedEventArgs e1) => { window.GetDefaultNavigator().Pop(); };

                var page = new View()
                {
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                    Layout = new LinearLayout()
                    {
                        LinearOrientation = LinearLayout.Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                    },
                };

                page.Add(textLabel);
                page.Add(positiveButton);

                dialog.Content = page;

                window.GetDefaultNavigator().Push(dialog);
            };

            Content = button;
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
        }
    }

    class DialogPageTest2 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new DialogPageContentPage2(window));
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
