/*
 * Copyright(c) 2022 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */
using System;
using System.ComponentModel;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class InternalFocusTest1 : ContentPage
    {
        private View rootContent;
        private FocusManager focusmanager;
        private TextField textField;
        private Button focusableView;
        private Button focusChangingBtn;
        private bool toggle = false;
        private TextLabel textLabel;

        private class MyAppBar : AppBar
        {
            public Button DefaultNavigationContent => base.DefaultNavigationContent as Button;
        }

        /// Modify this method for adding other examples.
        public InternalFocusTest1() : base()
        {
            focusmanager = FocusManager.Instance;

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            var myappbar = new MyAppBar()
            {
                Title = "FocusTest1",
            };
            myappbar.DefaultNavigationContent.Clicked += (s, e) =>
            {
                Console.WriteLine($"myappbar.DefaultNavigationContent.Clicked!");
                UnsubscribeFocusManagerEvents();
            };

            // Navigator bar title is added here.
            AppBar = myappbar;

            // Example root content view.
            // you can decorate, add children on this view.
            rootContent = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,

                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    CellPadding = new Size2D(10, 20),
                },
            };
            Content = rootContent;

            textField = new TextField
            {
                Text = "text field",
                BackgroundColor = Color.Gray,
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Focusable = true,
            };
            textField.FocusGained += (s, e) => textField.Text = $"focused";
            textField.FocusLost += (s, e) => textField.Text = $"Unfocused";

            rootContent.Add(textField);

            focusableView = new Button
            {
                Text = "Focusable Button",
                Focusable = true,
                FocusableInTouch = true,
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            focusableView.FocusGained += (s, e) => focusableView.Text = "Focused";
            focusableView.FocusLost += (s, e) => focusableView.Text = "Unfocused";
            rootContent.Add(focusableView);

            var btn1 = new Button
            {
                Text = "Focus on TextField",
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };

            var btn2 = new Button
            {
                Text = "Focus clear",
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };

            rootContent.Add(btn1);
            rootContent.Add(btn2);

            focusChangingBtn = new Button
            {
                Text = "FocusChanging Unsubscribed",
                WidthSpecification = LayoutParamPolicies.MatchParent,
            };
            focusChangingBtn.Clicked += (s, e) =>
            {
                toggle = !toggle;
                if(s is Button btn)
                {
                    if(toggle)
                    {
                        btn.Text = "FocusChanging Subscribed";
                        focusmanager.FocusChanging += OnFocusChanging;
                    }
                    else
                    {
                        btn.Text = "FocusChanging Unsubscribed";
                        focusmanager.FocusChanging -= OnFocusChanging;
                    }
                }
            };
            rootContent.Add(focusChangingBtn);

            btn1.Clicked += (s, e) => focusmanager.SetCurrentFocusView(textField);
            btn2.Clicked += (s, e) => focusmanager.ClearFocus();

            textLabel = new TextLabel
            {
                MultiLine = true,
                LineWrapMode = LineWrapMode.Character,
                Text = "Current Focused : ",
                WidthSpecification = LayoutParamPolicies.MatchParent,
                SizeHeight = 200,
            };
            rootContent.Add(textLabel);

            focusmanager.FocusChanged += OnFocusChanged;

            focusmanager.FocusedViewActivated += OnFocusViewActivated;
        }

        internal void OnFocusViewActivated(object s, FocusManager.FocusedViewActivatedEventArgs e)
        {
            Console.WriteLine($"FocusedViewActivated : CurrentFocusView - {focusmanager.GetCurrentFocusView()}");
        }

        internal void OnFocusChanged(object s, FocusManager.FocusChangedEventArgs e)
        {
            textLabel.Text = $"current : {e.CurrentView}, Next : {e.NextView}";
            Console.WriteLine($"FocusManager.FocusChanged: CurrentView(deprecated) : {e.CurrentView}, NextView(deprecated) : {e.NextView}");
            Console.WriteLine($"FocusManager.FocusChanged: Previous: {e.Previous}, Current : {e.Current}");
        }

        internal void OnFocusChanging(object s, FocusChangingEventArgs e)
        {
            Console.WriteLine($"FocusManager.FocusChanging: Current: {e.Current}, Proposed: {e.Proposed}");
            if(e.Current == textField)
            {
                e.Proposed = focusableView;
            }
            else if(e.Current == focusableView)
            {
                e.Proposed = focusChangingBtn;
            }
            else if(e.Current == focusChangingBtn)
            {
                e.Proposed = textField;
            }
            else
            {
                e.Proposed = null;
            }
        }

        internal void UnsubscribeFocusManagerEvents()
        {
            Console.WriteLine($"UnsubscribeFocusManagerEvents() unsubscribe here!");
            focusmanager.FocusedViewActivated -= OnFocusViewActivated;
            focusmanager.FocusChanged -= OnFocusChanged;
            focusmanager.FocusChanging -= OnFocusChanging;
        }
    }

    public class FocusTest1 : IExample
    {
        private Window window;
        InternalFocusTest1 test;
        public void Activate()
        {
            window = NUIApplication.GetDefaultWindow();
            test = new InternalFocusTest1();
            window.GetDefaultNavigator().Push(test);
        }

        public void Deactivate()
        {
            test.UnsubscribeFocusManagerEvents();
            window.GetDefaultNavigator().Pop();
        }
    }

}
