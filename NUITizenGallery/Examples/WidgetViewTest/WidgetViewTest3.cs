/*
 * Copyright (c) 2020 Samsung Electronics Co., Ltd.
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
using NUITizenGallery;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Tizen;
using Tizen.Applications;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    public class WidgetViewPageContentPage : ContentPage
    {
        private Window window;
        private View rootView;
        private int widgetWidth;
        private int widgetHeight;
        private WidgetView mWidgetView;
        private WidgetView mWidgetView2;

        public WidgetViewPageContentPage(Window win)
        {
            window = win;

            window.KeyEvent += OnKeyEvent;

            rootView = new View();
            rootView.BackgroundColor = Color.White;
            rootView.Size = Window.Instance.Size;
            rootView.PivotPoint = Tizen.NUI.PivotPoint.Center;
            window.GetDefaultLayer().Add(rootView);

            TextLabel sampleLabel = new TextLabel("Widget Viewer ");
            sampleLabel.FontFamily = "SamsungOneUI 500";
            sampleLabel.PointSize = 8;
            sampleLabel.TextColor = Color.Black;
            sampleLabel.SizeWidth = 300;
            sampleLabel.PivotPoint = Tizen.NUI.PivotPoint.Center;
            rootView.Add(sampleLabel);

            Bundle bundle = new Bundle();
            bundle.AddItem("COUNT", "1");
            String encodedBundle = bundle.Encode();

            Tizen.Log.Info("NUI", "*********************************************************");
            Tizen.Log.Info("NUI", "Tizen.NUI.WidgetTest must be installed at first!!!!!!!!!!");
            Tizen.Log.Info("NUI", "*********************************************************");

            widgetWidth = 500;
            widgetHeight = 500;
            mWidgetView = WidgetViewManager.Instance.AddWidget("class1@Tizen.NUI.WidgetTest", encodedBundle, widgetWidth, widgetHeight, 0.0f);
            mWidgetView.Position = new Position(100, 100);
            window.GetDefaultLayer().Add(mWidgetView);

            mWidgetView2 = WidgetViewManager.Instance.AddWidget("class2@Tizen.NUI.WidgetTest", encodedBundle, widgetWidth, widgetHeight, 0.0f);
            mWidgetView2.Position = new Position(100, widgetHeight + 110);
            window.GetDefaultLayer().Add(mWidgetView2);
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

        protected void Deactivate()
        {
            if (rootView != null)
            {
                window.GetDefaultLayer().Remove(rootView);
                rootView.Dispose();
                rootView = null;
            }
            if (mWidgetView != null)
            {
                window.GetDefaultLayer().Remove(mWidgetView);
                mWidgetView.Dispose();
                mWidgetView = null;
            }
            if (mWidgetView2 != null)
            {
                window.GetDefaultLayer().Remove(mWidgetView2);
                mWidgetView2.Dispose();
                mWidgetView2 = null;
            }
        }

        public void OnKeyEvent(object sender, Window.KeyEventArgs e)
        {
            if (e.Key.State == Key.StateType.Down)
            {
                Tizen.Log.Info("NUI", "OnKeyEvent(View-Window) : " + e.Key.KeyPressedName + "\n");
                if (e.Key.KeyPressedName == "1")
                {
                    widgetWidth += 200;
                    widgetHeight += 200;
                    if (widgetWidth > 1000 || widgetHeight > 1000)
                    {
                        widgetWidth = 200;
                        widgetHeight = 200;
                    }
                    mWidgetView.Size2D = new Size2D(widgetWidth, widgetHeight);
                }
            }
        }
    }

    class WidgetViewTest1 : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new WidgetViewPageContentPage(window));
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}