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
        private ContentPage firstPage = null, bottomPage = null;
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
                CreateTopPage();
            };

            navigator = window.GetDefaultNavigator();
            //window.Add(navigator);

            navigator.Popped += Popped; // it works?
        }

        private void CreateTopPage()
        {
            firstButton = new Button()
            {
                Text = $"Page count is {navigator.PageCount}. Click to insert a page below.",
                WidthSpecification = 500,
                HeightSpecification = 100,
                ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                PivotPoint = Tizen.NUI.PivotPoint.Center,
                PositionUsesPivotPoint = true,
            };
            firstButton.Clicked += (object sender, ClickedEventArgs e) =>
            {
                CreateBottomPage();
            };

            firstPage = new ContentPage()
            {
                AppBar = new AppBar()
                {
                    AutoNavigationContent = false,
                    Title = "Top Page",
                },
                Content = firstButton,
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
            };
            firstPage.Appearing += (object sender, PageAppearingEventArgs e) =>
            {
                Log.Info(this.GetType().Name, "First Page is appearing!");
            };
            firstPage.Disappearing += (object sender, PageDisappearingEventArgs e) =>
            {
                Log.Info(this.GetType().Name, "First Page is disappearing!");
            };

            Page topPage = navigator.Peek();
            int topIndex = navigator.IndexOf(topPage);
            navigator.Insert(topIndex + 1, firstPage);
        }

        private void CreateBottomPage()
        {
            if (bottomPage != null)
            {
                return; // avoid creating many times.
            }

            // change count of first button.
            firstButton.Text = $"Page count is {navigator.PageCount}. Do not click again.";
            firstButton.IsEnabled = false;
            firstButton.TextColor = new Color(0.5f, 1.0f, 1.0f, 1.0f);

            secondButton = new Button()
            {
                Text = $"Page count is {navigator.PageCount}. Click to pop page.",
                WidthSpecification = 500,
                HeightSpecification = 100,
                ParentOrigin = Tizen.NUI.ParentOrigin.Center,
                PivotPoint = Tizen.NUI.PivotPoint.Center,
                PositionUsesPivotPoint = true,
            };
            secondButton.Clicked += (object sender, ClickedEventArgs e) =>
            {
                Page tp = navigator.Peek();

                int ti = navigator.IndexOf(tp);
                int secondIndex = navigator.IndexOf(bottomPage);

                Log.Info(this.GetType().Name, $"Is it the second page? {ti == secondIndex}");

                Page page2 = navigator.GetPage(secondIndex);
                int pIndex2 = navigator.IndexOf(page2);

                Log.Info(this.GetType().Name, $"Is it the second page? {pIndex2 == navigator.IndexOf(bottomPage)}");

                navigator.Pop();
            };

            bottomPage = new ContentPage()
            {
                AppBar = new AppBar()
                {
                    Title = "Bottom Page",
                },
                Content = secondButton,
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
            };
            bottomPage.Appearing += (object sender, PageAppearingEventArgs e) =>
            {
                Log.Info(this.GetType().Name, "Second Page is appearing!");
            };
            bottomPage.Disappearing += (object sender, PageDisappearingEventArgs e) =>
            {
                Log.Info(this.GetType().Name, "Second Page is disappearing!");
            };

            navigator.InsertBefore(firstPage, bottomPage);
        }

        private void Popped(object sender, PoppedEventArgs args)
        {
            Log.Info(this.GetType().Name, "Page is popped!");
            
            if (args.Page == firstPage)
            {
                firstPage = null;
                if (bottomPage != null)
                {
                    bottomPage.AppBar.Title = "Bottom Page. Top page has been removed.";
                }
                if (secondButton != null)
                {
                    secondButton.Text = $"Page count is {navigator.PageCount - 1}. Click to pop page.";
                }
                Log.Info(this.GetType().Name, $"NavigatorContentPage1 page count is {navigator.PageCount}");
            }
            else
            {
                bottomPage = null;
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
                int index = navigator.IndexOf(bottomPage);
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
                bottomPage = null;
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
