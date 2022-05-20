using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class TabViewPage1 : ContentPage
    {
        private View root;
        private TabView tabView;
        private TextLabel textLabel1;
        private TextLabel textLabel2;
        private TabBar bar;
        private TabContent content;
        private Button btn;

        private static string ResourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "/images/";

        internal TabViewPage1(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "TabView Sample",
            };

            root = new View()
            {
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height),
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                    CellPadding = new Size(50, 50),
                }
            };

            tabView = new TabView()
            {
                BackgroundColor = Color.Cyan,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                SizeHeight = window.WindowSize.Height * 5 / 10,
            };
            root.Add(tabView);

            tabView.AddTab(new TabButton() 
                { 
                    Text = "Tab1",
                    IconURL = ResourcePath + "bg_0.png",  
                }, new View() 
                { 
                    BackgroundColor = Color.Blue,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                });
            tabView.AddTab(new TabButton() 
                { 
                    Text = "Tab2",
                    IconURL = ResourcePath + "bg_1.png",
                }, new View() 
                { 
                    BackgroundColor = Color.Green,
                    WidthSpecification = LayoutParamPolicies.MatchParent,
                    HeightSpecification = LayoutParamPolicies.MatchParent,
                });

            CreateImageTab();

            bar = tabView.TabBar;
            bar.TabButtonSelected += OnTabButtonSelected;

            textLabel1 = new TextLabel();
            textLabel1.Text = " TabButtonCount : " + bar.TabButtonCount;

            textLabel2 = new TextLabel();
            content = tabView.Content;
            textLabel2.Text = " ViewCount : " + content.ViewCount;

            root.Add(textLabel1);
            root.Add(textLabel2);

            btn = new Button()
            {
                Text = "Remove Tab3",
            };
            btn.Clicked += OnClicked;
            root.Add(btn);

            Content = root;
        }

        private void OnTabButtonSelected(object sender, TabButtonSelectedEventArgs e)
        {
            bar.Opacity = 0.5f;
            bar.GetTabButton(1).Clicked += OnTab1Cliked;
        }

        private void OnTab1Cliked(object sender, ClickedEventArgs e)
        {
            bar.GetTabButton(1).BackgroundColor = Color.Red;
        }

        private void CreateImageTab()
        {
            TabButton button = new TabButton() { Text = "Image", };
            View view = new View
            {
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    CellPadding = new Size2D(30, 30),
                },
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            };

            ImageView image = new ImageView
            {
                ResourceUrl = ResourcePath + "picture.png",
                BorderlineWidth = 5f,
                BorderlineColor = Color.Red,
            };

            TextLabel label = new TextLabel
            {
                Text = "tap the photo!",
                PointSize = 8,
            };

            view.Add(image);
            view.Add(label);

            tabView.AddTab(button, view);
        }

        private void OnClicked(object sender, ClickedEventArgs e)
        {
            tabView.RemoveTab(2);
            textLabel1.Text = " TabButtonCount : " + bar.TabButtonCount;
            textLabel2.Text = " ViewCount : " + content.ViewCount;

            btn.IsEnabled = false;
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
            btn.Clicked -= OnClicked;

            if (root != null)
            {
                root.Remove(textLabel1);
                textLabel1.Dispose();
                textLabel1 = null;

                root.Remove(textLabel2);
                textLabel2.Dispose();
                textLabel2 = null;

                root.Remove(btn);
                btn.Dispose();
                btn = null;

                root.Remove(tabView);
                tabView.Dispose();
                tabView = null;
            }
        }
    }

    class TabViewTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new TabViewPage1(window));
        }

        public void Deactivate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
#pragma warning restore Reflection // The code contains reflection
            window.GetDefaultNavigator().Pop();
        }
    }
}
