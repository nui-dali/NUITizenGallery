using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    public class NavigatorContentPage1 : ContentPage
    {
        private Window window;
        private Navigator navigator;
        private ContentPage firstPage, secondPage;
        private Button firstButton, secondButton;

        public NavigatorContentPage1(Window win)
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
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            };
            Content = button;

            button.Clicked += (object sender, ClickedEventArgs e) =>
            {
                CreateFirstPage();
            };
        }

        private void CreateFirstPage()
        {
            navigator = new Navigator()
            {
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent
            };
            window.Add(navigator);

            navigator.Popped += Popped; // it works?

            firstButton = new Button()
            {
                Text = "First Page",
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
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
                WidthResizePolicy = ResizePolicyType.FillToParent,
                HeightResizePolicy = ResizePolicyType.FillToParent,
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
            args.Page.Dispose();

            if (args.Page == firstPage)
            {
                firstPage = null;
                navigator.Popped -= Popped;
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
            if (navigator != null)
            {
                window.Remove(navigator);

                navigator.Dispose();
                navigator = null;
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
            window.GetDefaultNavigator().Push(new NavigatorContentPage1(window));
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
