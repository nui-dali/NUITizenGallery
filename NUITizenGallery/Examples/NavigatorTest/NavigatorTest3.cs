using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    public class NavigatorContentPage3 : ContentPage
    {
        private Window window;
        private Navigator navigator;
        private ContentPage firstPage, secondPage;
        private Button firstButton, secondButton;

        public NavigatorContentPage3(Window win)
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Navigator Sample",
            };

            window = win;

            var button = new Button()
            {
                Text = "Click to show Navigator",
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

            navigator = window.GetDefaultNavigator();
            //window.Add(navigator);

            navigator.Popped += Popped; // it works?
        }

        private void CreateFirstPage()
        {
            firstButton = new Button()
            {
                Text = "First Page",
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
                AppBar = new AppBar()
                {
                    AutoNavigationContent = false,
                    Title = "FirstPage",
                },
                Content = firstButton,
            };
            firstPage.Appearing += (object sender, PageAppearingEventArgs e) =>
            {
                Log.Info(this.GetType().Name, "First Page is appearing!");
            };
            firstPage.Disappearing += (object sender, PageDisappearingEventArgs e) =>
            {
                Log.Info(this.GetType().Name, "First Page is disappearing!");
            };

            navigator.Push(firstPage);
        }

        private void CreateSecondPage()
        {
            secondButton = new Button()
            {
                Text = "Second Page",
                WidthSpecification = 400,
                HeightSpecification = 100,
                ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                PivotPoint = Tizen.NUI.PivotPoint.Center,
                PositionUsesPivotPoint = true,
            };
            secondButton.Clicked += (object sender, ClickedEventArgs e) =>
            {
                navigator.Pop();
            };

            secondPage = new ContentPage()
            {
                AppBar = new AppBar()
                {
                    Title = "SecondPage",
                },
                Content = secondButton,
            };
            secondPage.Appearing += (object sender, PageAppearingEventArgs e) =>
            {
                Log.Info(this.GetType().Name, "Second Page is appearing!");
            };
            secondPage.Disappearing += (object sender, PageDisappearingEventArgs e) =>
            {
                Log.Info(this.GetType().Name, "Second Page is disappearing!");
            };

            navigator.Push(secondPage);
        }

        private void Popped(object sender, PoppedEventArgs args)
        {
            Log.Info(this.GetType().Name, "Page is popped!");
            
            if (args.Page == firstPage)
            {
                firstPage = null;
                navigator.Popped -= Popped;
                Log.Info(this.GetType().Name, $"NavigatorContentPage1 page count is {navigator.PageCount}");
            }
            else
            {
                secondPage = null;
                Log.Info(this.GetType().Name, $"NavigatorContentPage1 page count is {navigator.PageCount}");
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
            Log.Info(this.GetType().Name, $"NavigatorContentPage1 page count is {navigator.PageCount}, Deactivated0");
            if (navigator != null)
            {
                Log.Info(this.GetType().Name, $"NavigatorContentPage1 page count is {navigator.PageCount}, Deactivated1");
                // remove second page.
                int index = navigator.IndexOf(secondPage);
                navigator.RemoveAt(index);

                Log.Info(this.GetType().Name, $"NavigatorContentPage1 page count is {navigator.PageCount}, Deactivated2");
                // remove first page.
                navigator.Remove(firstPage);

                Log.Info(this.GetType().Name, $"NavigatorContentPage1 page count is {navigator.PageCount}, Deactivated3");
                // remove navigator.
                //window.Remove(navigator);
                //navigator.Dispose();
                navigator = null;

                Log.Info(this.GetType().Name, $"NavigatorContentPage1 page count is {navigator.PageCount}, Deactivated4");
                firstButton = null;
                firstPage = null;
                secondButton = null;
                secondPage = null;
            }
        }
    }

    class NavigatorTest3 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new NavigatorContentPage3(window));
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
