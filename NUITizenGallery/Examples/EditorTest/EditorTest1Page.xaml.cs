﻿/*
 * Copyright(c) 2021 Samsung Electronics Co., Ltd.
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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
namespace NUITizenGallery
{
    public partial class EditorTest1Page : ContentPage
    {
        public Size2D viewFullSize;

        public EditorTest1Page()
        {
            InitializeComponent();
            viewFullSize = new Size2D(0, 0);

            // Text set to TextEditor
            editor.Text = "This test is for testing Editor with very long text. This software is the confidential and proprietary information of Samsung Electronics, Inc. You shall not disclose such Confidential Information and shall use it only in accordance with the terms of the license agreement you entered into with Samsung.";
            editor.Margin = new Extents(20, 20, 20, 0);

            // Text set to TextField
            field.Text = "Please, input any sentence.";
            field.MaxLength = 999;

            underline.Margin = new Extents(20, 20, 5, 20);
            underline2.Margin = new Extents(0, 0, 0, 10);

            // Set focus highlight to underline 
            editor.FocusGained += (s, e) =>
            {
                underline.BackgroundColor = Color.Cyan;
            };

            editor.FocusLost += (s, e) =>
            {
                underline.BackgroundColor = Color.Gray;
            };

            field.FocusGained += (s, e) =>
            {
                underline2.BackgroundColor = Color.Cyan;
            };

            field.FocusLost += (s, e) =>
            {
                underline2.BackgroundColor = Color.Gray;
            };

            // View size adjustment from ime state
            InputMethodContext imeEditor = editor.GetInputMethodContext();
            imeEditor.StatusChanged += OnImeStatusChanged;
            InputMethodContext imeField = field.GetInputMethodContext();
            imeField.StatusChanged += OnImeStatusChanged;
        }

        private void OnImeStatusChanged(object sender, InputMethodContext.StatusChangedEventArgs e)
        {
            if (e.StatusChanged)
            {
                // When the virtual keyboard (IME) is shown, StatusChanged is true
                var resizedIME = sender as InputMethodContext;
                Rectangle rectangle = resizedIME.GetInputMethodArea();

                if (rectangle.Height > viewFullSize.Height)
                {
                    viewFullSize = (Size2D)mainView.Size2D.Clone();
                }
                
                int width = viewFullSize.Width;
                int height = viewFullSize.Height - rectangle.Height;
                mainView.Size2D = new Size2D(width, height);
            }
            else
            {
                mainView.Size2D = viewFullSize;
            }

            // Set bounding box for text decoration
            // This prevents the cursor handle from leaving the valid area.
            field.DecorationBoundingBox = new Rectangle(mainView.Position2D.X, mainView.Position2D.Y, mainView.Size2D.Width, mainView.Size2D.Height);
            editor.DecorationBoundingBox = new Rectangle(mainView.Position2D.X, mainView.Position2D.Y, mainView.Size2D.Width, mainView.Size2D.Height);
        }
    }
}
