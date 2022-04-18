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
using NUITizenGallery;
using Tizen;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace NUIPhotoSlide
{
    internal class CommonResource
    {
        public static string GetResourcePath()
        {
            return Tizen.Applications.Application.Current.DirectoryInfo.Resource + "images/family_photo_slide/";
        }
    }

    internal class PhotoSlidePage : ContentPage
    {
        private BGEffectView bgEffectView;
        private GroupLayerView groupLayerView;
        private OptionPageView optPageView;

        private ImageView settingIcon;
        private PreViewManager previewManager;

        private ImageView topShadow;
        private ImageView bottomShadow;

        public PhotoSlidePage()
        {
            Initialize();
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

        private void Initialize()
        {
            WidthSpecification = LayoutParamPolicies.MatchParent;
            HeightSpecification = LayoutParamPolicies.MatchParent;

            AppBar = new AppBar()
            {
                Title = "Photo Slide Sample",
            };

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

            groupLayerView = new GroupLayerView();
            bgEffectView = new BGEffectView(Color.Black);
            optPageView = new OptionPageView(groupLayerView);

            settingIcon = new ImageView();
            settingIcon.ResourceUrl = CommonResource.GetResourcePath() + "/images/" + "screensaver_ic_slideshow_settings.png";
            settingIcon.ParentOrigin = Tizen.NUI.ParentOrigin.BottomLeft;
            settingIcon.PivotPoint = Tizen.NUI.PivotPoint.BottomLeft;
            settingIcon.PositionUsesPivotPoint = true;
            settingIcon.Size2D = new Size2D(70, 70);
            settingIcon.Position2D = new Position2D(50, -100);
            settingIcon.TouchEvent += SettingIcon_TouchEvent;

            topShadow = new ImageView();
            topShadow.ResourceUrl = CommonResource.GetResourcePath() + "/shadow/" + "shadow_top.png";
            topShadow.ParentOrigin = Tizen.NUI.ParentOrigin.TopLeft;
            topShadow.PivotPoint = Tizen.NUI.PivotPoint.TopLeft;

            bottomShadow = new ImageView();
            bottomShadow.ResourceUrl = CommonResource.GetResourcePath() + "/shadow/" + "shadow_bottom.png";
            bottomShadow.Position2D = new Position2D(0, 1920 - bottomShadow.NaturalSize2D.Height);
            bottomShadow.ParentOrigin = Tizen.NUI.ParentOrigin.TopLeft;
            bottomShadow.PivotPoint = Tizen.NUI.PivotPoint.TopLeft;

            previewManager = new PreViewManager(Window.Instance, groupLayerView);

            rootContent.Add(groupLayerView);

            rootContent.Add(bgEffectView);
            rootContent.Add(topShadow);
            rootContent.Add(bottomShadow);

            rootContent.Add(optPageView);
            rootContent.Add(settingIcon);

            Content = rootContent;

            bgEffectView.Play();
        }

        private bool SettingIcon_TouchEvent(object source, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                if (optPageView.Opacity == 1.0f)
                {
                    //groupLayerView.ShowTextView();
                    optPageView.PlayHideAnimation();
                }
                else
                {
                    //groupLayerView.HideTextView();
                    optPageView.PlayShowAnimation();
                }
            }
            return false;
        }
    }

    class PhotoSlideSampleTest : IExample
    {
        private Window window;

        public void Activate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            window.GetDefaultNavigator().Push(new PhotoSlidePage());
        }

        public void Deactivate()
        {
            Log.Info(this.GetType().Name, $"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            window.GetDefaultNavigator().Pop();
        }
    }
}
