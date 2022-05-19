using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    public class DialogPageContentPage : ContentPage
    {
        private Window window = null;
        private int oldPageCount = 0;

        public DialogPageContentPage(Window win)
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Dialog page Sample",
            };

            window = win;
            oldPageCount = window.GetDefaultNavigator().PageCount;

            var button = new Button()
            {
                Text = "Click to show Dialog",
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            };

            button.Clicked += (object sender, ClickedEventArgs e) =>
            {
                var textLabel = new TextLabel("Message")
                {
                    BackgroundColor = Color.White,
                    Size = new Size(180, 180),
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                };

                DialogPage.ShowDialog(textLabel);
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
            var newPageCount = window.GetDefaultNavigator().PageCount;

            for (int i = 0; i < (newPageCount - oldPageCount); i++)
            {
                window.GetDefaultNavigator().Pop();
            }
        }
    }

    class DialogPageTest : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new DialogPageContentPage(window));
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
