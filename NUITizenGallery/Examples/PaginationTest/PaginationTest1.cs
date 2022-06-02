using System;
using System.Collections.Generic;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class PaginationTestPage1 : ContentPage
    {
        private TextLabel[] board = new TextLabel[2];
        private Pagination[] pagination = new Pagination[2];
        private View[] layout = new View[3];
        private TextLabel selectedIndex;
        private TextLabel positionInfo;

        private View buttonView;
        private Button btn1;
        private Button btn2;

        private readonly int PAGE_COUNT = 5;
        private static string ResourcePath = Tizen.Applications.Application.Current.DirectoryInfo.Resource + "/images/";

        internal PaginationTestPage1(Window window)
        {
            AppBar = new AppBar()
            {
                Title = "Pagination Sample",
            };

            layout[0] = new View()
            {
                Size = new Size(window.WindowSize.Width, window.WindowSize.Height),
                BackgroundColor = new Color(0.7f, 0.9f, 0.8f, 1.0f),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    VerticalAlignment = VerticalAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(10, 50),
                }
            };

            window.KeyEvent += Window_KeyEvent;

            // A pagination sample created by properties will be added to this layout.
            layout[1] = new View()
            {
                Size = new Size(700, 70),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(20, 50)
                }
            };
            layout[0].Add(layout[1]);

            // A pagination sample created by attributes will be added to this layout.
            layout[2] = new View()
            {
                Size = new Size(700, 70),
                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size(20, 50)
                }
            };
            layout[0].Add(layout[2]);

            createBorads(window);

            ///////////////////////////////////////////////Create by Properties//////////////////////////////////////////////////////////
            pagination[0] = new Pagination();
            pagination[0].Name = "Pagination1";
            pagination[0].Size = new Size(window.WindowSize.Width / 2, window.WindowSize.Height / 12);
            pagination[0].BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);
            pagination[0].IndicatorSize = new Size(26, 26);
            pagination[0].IndicatorSpacing = 8;
            pagination[0].IndicatorColor = Color.Yellow;
            pagination[0].SelectedIndicatorColor = Color.Blue;
            pagination[0].IndicatorImageUrl = new Selector<string>
            {
                Normal = ResourcePath + "controller/pagination_ic_next.png",
                Selected = ResourcePath + "controller/pagination_ic_return.png"
            };
            pagination[0].IndicatorCount = PAGE_COUNT;
            pagination[0].SelectedIndex = 0;
            layout[1].Add(pagination[0]);

            ///////////////////////////////////////////////Create by Attributes//////////////////////////////////////////////////////////
            PaginationStyle style = new PaginationStyle()
            {
                IndicatorSize = new Size(15, 15),
                IndicatorSpacing = 20,
                IndicatorImageUrl = new Selector<string>
                {
                    Normal = ResourcePath + "controller/pagination_ic_nor.png",
                    Selected = ResourcePath + "controller/pagination_ic_sel.png"
                }
            };
            pagination[1] = new Pagination(style);
            pagination[1].Name = "Pagination2";
            pagination[1].Size = new Size(window.WindowSize.Width / 2, window.WindowSize.Height / 12);
            pagination[1].BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.6f);
            pagination[1].IndicatorCount = PAGE_COUNT;
            pagination[1].SelectedIndex = 0;
            layout[2].Add(pagination[1]);

            selectedIndex = new TextLabel();
            selectedIndex.Text = "SelectedIndex : " + pagination[0].SelectedIndex;
            layout[0].Add(selectedIndex);

            positionInfo = new TextLabel();
            positionInfo.Text = "GetIndicatorPosition , X : " + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).X + ", Y" + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).Y;
            layout[0].Add(positionInfo);

            buttonView = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = 60,
                Layout = new LinearLayout()
                { 
                    LinearOrientation = LinearLayout.Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    CellPadding = new Size2D(20, 20),
                }
            };
            layout[0].Add(buttonView);

            btn1 = new Button()
            {
                WidthSpecification = window.WindowSize.Width / 3,
                HeightSpecification = 50,
                Text = "Right",
            };
            btn1.Clicked += OnRightClicked;

            btn2 = new Button()
            {
                WidthSpecification = window.WindowSize.Width / 3,
                HeightSpecification = 50,
                Text = "Left",
            };
            btn2.Clicked += OnLeftClicked;

            buttonView.Add(btn1);
            buttonView.Add(btn2);

            Content = layout[0];
        }

        private void OnLeftClicked(object sender, ClickedEventArgs e)
        {
            if (pagination[0].SelectedIndex > 0)
            {
                pagination[0].SelectedIndex = pagination[0].SelectedIndex - 1;
                if (pagination[0].SelectedIndex != pagination[0].IndicatorCount - 1)
                {
                    pagination[0].LastIndicatorImageUrl = new Selector<string>
                    {
                        Normal = ResourcePath + "controller/pagination_ic_next.png",
                        Selected = ResourcePath + "controller/pagination_ic_return.png"
                    };
                }
                selectedIndex.Text = "SelectedIndex : " + pagination[0].SelectedIndex;
            }
            if (pagination[1].SelectedIndex > 0)
            {
                pagination[1].SelectedIndex = pagination[1].SelectedIndex - 1;
                positionInfo.Text = "GetIndicatorPosition , X : " + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).X + ", Y" + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).Y;
            }
        }

        private void OnRightClicked(object sender, ClickedEventArgs e)
        {
            if (pagination[0].SelectedIndex < pagination[0].IndicatorCount - 1)
            {
                pagination[0].SelectedIndex = pagination[0].SelectedIndex + 1;
                if (pagination[0].SelectedIndex == pagination[0].IndicatorCount - 1)
                {
                    pagination[0].LastIndicatorImageUrl = new Selector<string>
                    {
                        Normal = ResourcePath + "controller/pagination_ic_nor.png",
                        Selected = ResourcePath + "controller/pagination_ic_sel.png"
                    };
                }
                selectedIndex.Text = "SelectedIndex : " + pagination[0].SelectedIndex;
            }
            if (pagination[1].SelectedIndex < pagination[1].IndicatorCount - 1)
            {
                pagination[1].SelectedIndex = pagination[1].SelectedIndex + 1;
                positionInfo.Text = "GetIndicatorPosition , X : " + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).X + ", Y" + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).Y;
            }
        }

        void createBorads(Window window)
        {
            board[0] = new TextLabel();
            board[0].Size = new Size(window.WindowSize.Width / 2, window.WindowSize.Height / 12);
            board[0].HorizontalAlignment = HorizontalAlignment.Center;
            board[0].VerticalAlignment = VerticalAlignment.Center;
            board[0].BackgroundColor = Color.Magenta;
            board[0].Text = "Property construction";
            layout[1].Add(board[0]);

            board[1] = new TextLabel();
            board[1].Size = new Size(window.WindowSize.Width / 2, window.WindowSize.Height / 12);
            board[1].HorizontalAlignment = HorizontalAlignment.Center;
            board[1].VerticalAlignment = VerticalAlignment.Center;
            board[1].BackgroundColor = Color.Magenta;
            board[1].Text = "Attribute construction";
            layout[2].Add(board[1]);
        }

        private void Window_KeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                if (e.Key.KeyPressedName == "Left")
                {
                    if (pagination[0].SelectedIndex > 0)
                    {
                        pagination[0].SelectedIndex = pagination[0].SelectedIndex - 1;
                        if (pagination[0].SelectedIndex != pagination[0].IndicatorCount - 1)
                        {
                            pagination[0].LastIndicatorImageUrl = new Selector<string>
                            {
                                Normal = ResourcePath + "controller/pagination_ic_next.png",
                                Selected = ResourcePath + "controller/pagination_ic_return.png"
                            };
                        }
                        selectedIndex.Text = "SelectedIndex : " + pagination[0].SelectedIndex;
                    }
                    if (pagination[1].SelectedIndex > 0)
                    {
                        pagination[1].SelectedIndex = pagination[1].SelectedIndex - 1;
                        positionInfo.Text = "GetIndicatorPosition , X : " + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).X + ", Y" + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).Y;
                    }
                }
                else if (e.Key.KeyPressedName == "Right")
                {
                    if (pagination[0].SelectedIndex < pagination[0].IndicatorCount - 1)
                    {
                        pagination[0].SelectedIndex = pagination[0].SelectedIndex + 1;
                        if (pagination[0].SelectedIndex == pagination[0].IndicatorCount - 1)
                        {
                            pagination[0].LastIndicatorImageUrl = new Selector<string>
                            {
                                Normal = ResourcePath + "controller/pagination_ic_nor.png",
                                Selected = ResourcePath + "controller/pagination_ic_sel.png"
                            };
                        }
                        selectedIndex.Text = "SelectedIndex : " + pagination[0].SelectedIndex;
                    }
                    if (pagination[1].SelectedIndex < pagination[1].IndicatorCount - 1)
                    {
                        pagination[1].SelectedIndex = pagination[1].SelectedIndex + 1;
                        positionInfo.Text = "GetIndicatorPosition , X : " + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).X + ", Y" + pagination[1].GetIndicatorPosition(pagination[1].SelectedIndex).Y;
                    }
                }
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
            Window window = NUIApplication.GetDefaultWindow();
            window.KeyEvent -= Window_KeyEvent;

            if (layout[0] != null)
            {
                layout[1].Remove(board[0]);
                board[0].Dispose();
                board[0] = null;

                layout[1].Remove(pagination[0]);
                pagination[0].Dispose();
                pagination[0] = null;

                layout[0].Remove(layout[1]);
                layout[1].Dispose();
                layout[1] = null;

                layout[2].Remove(board[1]);
                board[1].Dispose();
                board[1] = null;

                layout[2].Remove(pagination[1]);
                pagination[1].Dispose();
                pagination[1] = null;

                layout[0].Remove(layout[2]);
                layout[2].Dispose();
                layout[2] = null;

                window.Remove(layout[0]);
                layout[0].Dispose();
                layout[0] = null;
            }
        }
    }

    class PaginationTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
#pragma warning disable Reflection // The code contains reflection
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");
#pragma warning restore Reflection // The code contains reflection

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new PaginationTestPage1(window));
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
