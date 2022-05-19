using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class MenuContentPage : ContentPage
    {
        public MenuContentPage()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Menu Sample",
            };

            var pageContent = new Button()
            {
                Text = "Page Content",
                CornerRadius = 0,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };

            Content = pageContent;

            pageContent.Clicked += (object s1, ClickedEventArgs a1) =>
            {
                var menuItem = new MenuItem() { Text = "Menu" };
                menuItem.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    Log.Info(this.GetType().Name, $"1st MenuItem's IsSelected is changed to {args.IsSelected}.");
                };

                var menuItem2 = new MenuItem() { Text = "Menu2" };
                menuItem2.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    Log.Info(this.GetType().Name, $"2nd MenuItem's IsSelected is changed to {args.IsSelected}.");
                };

                var menuItem3 = new MenuItem() { Text = "Menu3" };
                menuItem3.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    Log.Info(this.GetType().Name, $"3rd MenuItem's IsSelected is changed to {args.IsSelected}.");
                };

                var menuItem4 = new MenuItem() { Text = "Menu4" };
                menuItem4.SelectedChanged += (object sender, SelectedChangedEventArgs args) =>
                {
                    Log.Info(this.GetType().Name, $"4th MenuItem's IsSelected is changed to {args.IsSelected}.");
                };

                var moreButton = new Button()
                {
                    Text = "More",
                };

                var menu = new Menu()
                {
                    Anchor = moreButton,
                    HorizontalPositionToAnchor = Menu.RelativePosition.Center,
                    VerticalPositionToAnchor = Menu.RelativePosition.End,
                    Items = new MenuItem[] { menuItem, menuItem2, menuItem3, menuItem4 },
                };
                menu.Post();
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

        public void Deactivate()
        {
        }
    }

    class MenuTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new MenuContentPage());
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
