using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    public class AppBarContentPage1 : ContentPage
    {
        private ContentPage firstPage, secondPage;
        private AppBar firstAppBar, secondAppBar;
        private Button firstActionButton, secondActionButton;
        private Button firstButton, secondButton;
        private Window window;

        public AppBarContentPage1(Window win)
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "AppBar Sample",
            };

            window = win;

            var button = new Button()
            {
                Text = "Click to show AppBar",
                WidthSpecification = 400,
                HeightSpecification = 100,
                ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                PivotPoint = Tizen.NUI.PivotPoint.Center,
                PositionUsesPivotPoint = true,
            };
            Content = button;

            button.Clicked += (object sender, ClickedEventArgs e) =>
            {
                CreateFirstPage();
            };
        }

        private void CreateFirstPage()
        {
            firstActionButton = new Button()
            {
                Text = "Page 2",
            };
            firstActionButton.Clicked += (object sender, ClickedEventArgs e) =>
            {
                CreateSecondPage();
            };       

            firstAppBar = new AppBar()
            {
                AutoNavigationContent = false,
                Title = "First Page",
                ActionContent = firstActionButton,
            };

            firstButton = new Button()
            {
                Text = "Click to next",
                WidthSpecification = 400,
                HeightSpecification = 100,
                ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                PivotPoint = Tizen.NUI.PivotPoint.Center,
                PositionUsesPivotPoint = true,
            };
            firstButton.Clicked += (object sender, ClickedEventArgs e) =>
            {
                CreateSecondPage();
            };

            firstPage = new ContentPage()
            {
                AppBar = firstAppBar,
                Content = firstButton,
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
            };

            window.GetDefaultNavigator().Push(firstPage);
        }

        private void CreateSecondPage()
        {
            secondActionButton = new Button()
            {
                Text = "Page 1",
            };
            secondActionButton.Clicked += (object sender, ClickedEventArgs e) =>
            {
                window.GetDefaultNavigator().Pop();
            };

            var popButton = new Button()
            {
                Text = "Pop page",
            };
            popButton.Clicked += (object sender, ClickedEventArgs e) =>
            {
                window.GetDefaultNavigator().Pop();
            };

            secondAppBar = new AppBar()
            {
                AutoNavigationContent = true,
                TitleContent = new TextLabel()
                {
                    Text = "Second Page",
                },
                NavigationContent = popButton,
                Actions = new View[] { secondActionButton },
            };

            secondButton = new Button()
            {
                Text = "Click to prev",
                WidthSpecification = 400,
                HeightSpecification = 100,
                ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                PivotPoint = Tizen.NUI.PivotPoint.Center,
                PositionUsesPivotPoint = true,
            };
            secondButton.Clicked += (object sender, ClickedEventArgs e) =>
            {
                Page topPage = window.GetDefaultNavigator().Peek();

                int topIndex = window.GetDefaultNavigator().IndexOf(topPage);
                int secondIndex = window.GetDefaultNavigator().IndexOf(secondPage);

                Log.Info(this.GetType().Name, $"Is it the second page? {topIndex == secondIndex}");

                window.GetDefaultNavigator().Pop();
            };

            secondPage = new ContentPage()
            {
                AppBar = secondAppBar,
                Content = secondButton,
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
            };

            window.GetDefaultNavigator().Push(secondPage);
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
            int index = window.GetDefaultNavigator().IndexOf(secondPage);
            window.GetDefaultNavigator().RemoveAt(index);
            secondPage = null;
            secondAppBar = null;
            secondActionButton = null;
            secondButton = null;

            window.GetDefaultNavigator().Remove(firstPage);
            firstPage = null;
            firstAppBar = null;
            firstActionButton = null;
            firstButton = null;
        }
    }

    class AppBarTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new AppBarContentPage1(window));
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
