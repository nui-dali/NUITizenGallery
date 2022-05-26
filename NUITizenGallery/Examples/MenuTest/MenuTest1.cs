using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class MenuContentPage : ContentPage
    {
        private Menu menuRef;

        public MenuContentPage()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Menu Sample",
            };

            var view = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                {
                    HorizontalAlignment = HorizontalAlignment.Center,
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                }
            };
            Content = view;

            var showButton = new Button()
            {
                Text = "Show Menu",
                CornerRadius = 0,
                WidthSpecification = 200,
                HeightSpecification = 100,
            };
            view.Add(showButton);

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
                Text = "Menu shown here",
                CornerRadius = 0,
                WidthSpecification = 200,
                HeightSpecification = 100,
            };

            showButton.Clicked += (object s1, ClickedEventArgs a1) =>
            {
                var testMenu = new Menu()
                {
                    Anchor = moreButton,
                    HorizontalPositionToAnchor = Menu.RelativePosition.Center,
                    VerticalPositionToAnchor = Menu.RelativePosition.End,
                    Items = new MenuItem[] { menuItem, menuItem2, menuItem3, menuItem4 },
                };
                menuRef = testMenu;
                testMenu.Post();
            };

            var dismissButton = new Button()
            {
                Text = "Dismiss Menu",
                CornerRadius = 0,
                WidthSpecification = 200,
                HeightSpecification = 100,
            };
            view.Add(dismissButton);

            dismissButton.Clicked += (object s1, ClickedEventArgs a1) =>
            {
                if (menuRef != null)
                {
                    menuRef.Dismiss(); //
                    menuRef = null;
                }
            };

            view.Add(moreButton);
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
