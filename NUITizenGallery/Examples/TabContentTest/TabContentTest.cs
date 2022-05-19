using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class TabContentImpl : TabContent 
    {
        public TabContentImpl()
        {

        }

        public void OnAddView(View view)
        {
            base.AddView(view);
        }

        public void OnSelect(int index)
        {
            base.Select(index);
        }
    }

    internal class TabContentTestPage : ContentPage
    {
        private View root;
        private View view;
        private Button btn;
        private TabContentImpl tabContent;
        private TextLabel textLabel1;
        private TextLabel textLabel2;
        private TextLabel textLabel3;
        private TextLabel textLabel4;
        private int cnt = 0;

        internal TabContentTestPage(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "TabContent Sample",
            };

            root = new View()
            {
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height),
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(50, 50),
                }
            };

            tabContent = new TabContentImpl()
            {
                Size = new Size(window.WindowSize.Width * 4 / 10, window.WindowSize.Height),
                BackgroundColor = Color.Yellow,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                }
            };


            tabContent.OnAddView(new View()
            {
                Size = new Size(window.WindowSize.Width * 4 / 10, window.WindowSize.Height),
                BackgroundColor = Color.Red,
                Opacity = 0.8f,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            });
            tabContent.OnAddView(new View()
            {
                BackgroundColor = Color.Green,
                Opacity = 0.5f,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
            });

            view = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                }
            };

            btn = new Button()
            {
                BackgroundColor = Color.AliceBlue,
            };
            btn.Clicked += OnClicked;

            textLabel1 = new TextLabel();
            textLabel1.Text = "ViewCount : ";
            textLabel2 = new TextLabel();
            textLabel2.Text = tabContent.ViewCount.ToString();
            textLabel3 = new TextLabel();
            textLabel3.Text = "CurrentView's BackgroundColor : ";
            textLabel4 = new TextLabel();
            textLabel4.Text = "R : " + tabContent.GetView(0).BackgroundColor.R + " G : " + tabContent.GetView(0).BackgroundColor.G + " B: " + tabContent.GetView(0).BackgroundColor.B + " A : " + tabContent.GetView(0).BackgroundColor.A;

            view.Add(textLabel1);
            view.Add(textLabel2);
            view.Add(textLabel3);
            view.Add(textLabel4);
            
            view.Add(btn);

            root.Add(tabContent);
            root.Add(view);
            Content = root;
        }

        private void OnClicked(object sender, ClickedEventArgs e)
        {
            ++cnt;
            if (cnt % 2 == 1)
            {
                tabContent.OnSelect(1);
                textLabel4.Text = "R : " + tabContent.GetView(1).BackgroundColor.R + " G : " + tabContent.GetView(1).BackgroundColor.G + " B: " + tabContent.GetView(1).BackgroundColor.B + " A : " + tabContent.GetView(1).BackgroundColor.A;

            }
            else
            {
                tabContent.OnSelect(0);
                textLabel4.Text = "R : " + tabContent.GetView(0).BackgroundColor.R + " G : " + tabContent.GetView(0).BackgroundColor.G + " B: " + tabContent.GetView(0).BackgroundColor.B + " A : " + tabContent.GetView(0).BackgroundColor.A;
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
            if (root != null)
            {
                view.Remove(btn);
                btn.Dispose();
                btn = null;

                view.Remove(textLabel1);
                textLabel1.Dispose();
                textLabel1 = null;

                view.Remove(textLabel2);
                textLabel2.Dispose();
                textLabel2 = null;

                view.Remove(textLabel3);
                textLabel3.Dispose();
                textLabel3 = null;

                view.Remove(textLabel4);
                textLabel4.Dispose();
                textLabel4 = null;

                root.Remove(view);
                view.Dispose();
                view = null;

                root.Remove(tabContent);
                tabContent.Dispose();
                tabContent = null;
            }
        }
    }

    internal class TabContentTest : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new TabContentTestPage(window));
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
