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
    internal class FocusIndicator : ContentPage
    {
        private View rootContent;
        private Button buttonSetNewIndi, buttonRestoreIndi, buttonSetNull, buttonSetDefaultAlgorithm, buttonUnsetDefaultAlgorithm;
        private FocusManager focusmanager;

        /// Modify this method for adding other examples.
        public FocusIndicator() : base()
        {
            focusmanager = FocusManager.Instance;

            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            // Navigator bar title is added here.
            AppBar = new AppBar()
            {
                Title = "Focus Indicator Test",
            };

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

            buttonSetNewIndi = new Button
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Text = "Set New Focus Indicator",
            };
            rootContent.Add(buttonSetNewIndi);

            buttonSetNewIndi.Clicked += (s, e) =>
            {
                focusmanager.FocusIndicator = new View()
                {
                    PositionUsesPivotPoint = true,
                    PivotPoint = new Position(0, 0, 0),
                    WidthResizePolicy = ResizePolicyType.FillToParent,
                    HeightResizePolicy = ResizePolicyType.FillToParent,
                    BorderlineColor = Color.Orange,
                    BorderlineWidth = 4.0f,
                    BorderlineOffset = -1f,
                    BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.2f),
                };
            };

            buttonSetNull = new Button
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Text = "Set null Focus Indicator",
            };
            rootContent.Add(buttonSetNull);

            buttonSetNull.Clicked += (s, e) =>
            {
                focusmanager.FocusIndicator = null;
            };

            buttonRestoreIndi = new Button
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Text = "Restore default Focus Indicator",
            };
            rootContent.Add(buttonRestoreIndi);

            buttonRestoreIndi.Clicked += (s, e) =>
            {
                focusmanager.FocusIndicator = focusmanager.GetDefaultFocusIndicator();
            };

            buttonSetDefaultAlgorithm = new Button
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Text = "Enable FocusManager Default Algorithm",
            };
            rootContent.Add(buttonSetDefaultAlgorithm);

            buttonSetDefaultAlgorithm.Clicked += (s, e) =>
            {
                focusmanager.EnableDefaultAlgorithm(true);
            };

            buttonUnsetDefaultAlgorithm = new Button
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                Text = "Disable FocusManager Default Algorithm",
            };
            rootContent.Add(buttonUnsetDefaultAlgorithm);

            buttonUnsetDefaultAlgorithm.Clicked += (s, e) =>
            {
                focusmanager.EnableDefaultAlgorithm(false);
            };

            Content = rootContent;
        }
    }

    public class FocusIndicatorTest : IExample
    {
        private Window window;

        public void Activate()
        {
            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new FocusIndicator());
        }

        public void Deactivate()
        {
            window.GetDefaultNavigator().Pop();
        }
    }

}
