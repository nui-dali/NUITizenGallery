/*
 * Copyright (c) 2017 Samsung Electronics Co., Ltd.
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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.FH.FamilyBoard
{
    internal class CommonResource
    {
        public static string GetResourcePath()
        {
            return Applications.Application.Current.DirectoryInfo.Resource + "images/family_board/";
        }
    }

    internal class FamilyBoardPage : ContentPage
    {
        private static FamilyBoardPage instance = null;

        private ILifecycleObserver main_view = null;
        private Stack<ILifecycleObserver> view_stack = new Stack<ILifecycleObserver>();

        public static FamilyBoardPage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new FamilyBoardPage();
                }
                return instance;
            }
        }

        private FamilyBoardPage()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Photo Slide Sample",
            };

            main_view = CreateView("Main");

            var rootContent = new View()
            {
                WidthSpecification = LayoutParamPolicies.MatchParent,
                HeightSpecification = LayoutParamPolicies.MatchParent,

                Layout = new LinearLayout()
                {
                    LinearOrientation = LinearLayout.Orientation.Vertical,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                },
            };
            FamilyBoardMain main = (FamilyBoardMain)main_view;
            rootContent.Add(main.GetRootView());

            Content = rootContent;
        }

        protected override void Dispose(DisposeTypes type)
        {
            if (Disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                RemoveAllChildren(true);
            }

            base.Dispose(type);
        }

        private void RemoveAllChildren(bool dispose = false)
        {
            RecursiveRemoveChildren(this, dispose);
        }

        private void RecursiveRemoveChildren(View parent, bool dispose)
        {
            if (parent == null)
            {
                return;
            }

            int maxChild = (int)parent.ChildCount;
            for (int i = maxChild - 1; i >= 0; --i)
            {
                View child = parent.GetChildAt((uint)i);
                if (child == null)
                {
                    continue;
                }

                RecursiveRemoveChildren(child, dispose);
                parent.Remove(child);
                if (dispose)
                {
                    child.Dispose();
                }
            }
        }

        public void ChangeMainBackground()
        {
            FamilyBoardMain fb = main_view as FamilyBoardMain;
            if (fb != null)
            {
                fb.ChangeBackground();
            }
        }

        public ILifecycleObserver CreateView(string view_name)
        {
            ILifecycleObserver view = null;

            if (view_name.Equals("Main"))
            {
                view = new FamilyBoardMain();
            }
            else if (view_name.Equals("Picture Wizard"))
            {
                view = PictureWizard.Instance;
            }
            else if (view_name.Equals("Stickers"))
            {
                view = new StickerChooser();
            }
            else if (view_name.Equals("Text"))
            {
                view = new TextChooser();
            }
            else if (view_name.Equals("Background Image"))
            {
                view = new BackgroundImageChooser();
            }
            else
            {
                return view;
            }

            view.Activate();
            view_stack.Push(view);

            return view;
        }

        public void RemoveView()
        {
            ILifecycleObserver lastView = view_stack.Pop();
            lastView.Deactivate();

            ILifecycleObserver currentView = view_stack.Peek();
            currentView.Reactivate();
        }
    }

    class FamilyBoardSampleTest : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(FamilyBoardPage.Instance);
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}

