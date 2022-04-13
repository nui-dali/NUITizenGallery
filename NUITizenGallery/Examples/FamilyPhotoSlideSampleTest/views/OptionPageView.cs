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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace NUIPhotoSlide
{
    internal class OptionPageView : View
    {
        private SlideBar[] slideBarList;
        private GroupLayerView groupLayerView;

        public OptionPageView(GroupLayerView groupLayerView)
        {
            this.groupLayerView = groupLayerView;

            slideBarList = new SlideBar[3];

            this.Opacity = 0.0f;
            this.TouchEvent += SettingView_TouchEvent;
            this.HeightResizePolicy = ResizePolicyType.FillToParent;
            this.WidthResizePolicy = ResizePolicyType.FillToParent;

            View colorBG = new View();
            colorBG.HeightResizePolicy = ResizePolicyType.FillToParent;
            colorBG.WidthResizePolicy = ResizePolicyType.FillToParent;
            colorBG.BackgroundColor = Color.Black;
            colorBG.Opacity = 0.3f;
            this.Add(colorBG);
            

            float gray = 0.75f;

            this.Add(CreateTextLabel(250, "Sorting", 20, gray));

            TextLabel all = CreateTextLabel(350, "All", 35, 0.4f);
            TextLabel newest = CreateTextLabel(420, "Newest", 35, 0.4f);
            TextLabel oldest = CreateTextLabel(490, "Oldest", 35, 0.4f);

            all.Focusable = false;
            newest.Focusable = false;
            oldest.Focusable = false;

            this.Add(all);
            this.Add(newest);
            this.Add(oldest);

            this.Add(CreateTextLabel(560, "Shared by Jake", 35));
            this.Add(CreateTextLabel(630, "Shared by Michelle", 35));
            this.Add(CreateTextLabel(700, "Shared by Laura", 35));


            this.Add(CreateTextLabel(950, "Scale", 20, gray));
            this.Add(CreateTextLabel(1200, "Speed", 20, gray));
            this.Add(CreateTextLabel(1450, "Number of Pictures", 20, gray));

            slideBarList[0] = new SlideBar(1050, gray, 1, new string[2] { "Small", "Large" }, 2);
            slideBarList[1] = new SlideBar(1300, gray, 4, new string[5] { "-3x", "-2x", "1x", "2x", "3x" }, 3);
            slideBarList[2] = new SlideBar(1550, gray, 5, new string[6] { "1", "2", "3", "4", "5", "6" }, 6);

            slideBarList[0].ItemSelectEvent += OptionPageView_ChangeScale;
            slideBarList[1].ItemSelectEvent += OptionPageView_ChangeSpeed;
            slideBarList[2].ItemSelectEvent += OptionPageView_ChangeNOP;

            for (int i = 0; i < 3; i++)
            {
                this.Add(slideBarList[i]);
            }
        }

        private bool TextLabel_TouchEvent(object source, TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                TextLabel label = source as TextLabel;
                if (label.Text.Contains("All"))
                {
                    //groupLayerView.AllPictures();
                    //PlayHideAnimation();
                }
                if (label.Text.Contains("Jake"))
                {
                    groupLayerView.ChangeUserPage("Jake");
                    PlayHideAnimation();
                }
                if (label.Text.Contains("Michelle"))
                {
                    groupLayerView.ChangeUserPage("Michelle");
                    PlayHideAnimation();
                }
                if (label.Text.Contains("Laura"))
                {
                    groupLayerView.ChangeUserPage("Laura");
                    PlayHideAnimation();
                }
                SetFocus(label);
            }
            return false;
        }

        private void SetFocus(View view)
        {
            FocusManager.Instance.SetCurrentFocusView(view);
        }

        private void OptionPageView_ChangeScale(object sender, EventArgs e)
        {
            switch (slideBarList[0].SelectIdx)
            {
                case 1:
                    groupLayerView.SetItemScale(0.8f);
                    break;
                case 2:
                    groupLayerView.SetItemScale(1.0f);
                    break;
            }

        }

        private void OptionPageView_ChangeSpeed(object sender, EventArgs e)
        {
            switch (slideBarList[1].SelectIdx)
            {
                case 1:
                    groupLayerView.SetItemSpeed(0.05f);
                    break;
                case 2:
                    groupLayerView.SetItemSpeed(0.1f);
                    break;
                case 3:
                    groupLayerView.SetItemSpeed(0.2f);
                    break;
                case 4:
                    groupLayerView.SetItemSpeed(0.5f);
                    break;
                case 5:
                    groupLayerView.SetItemSpeed(1.0f);
                    break;
            }
        }

        private void OptionPageView_ChangeNOP(object sender, EventArgs e)
        {
            groupLayerView.SetItemCount(slideBarList[2].SelectIdx);
            groupLayerView.ResetThisPage();
        }

        private TextLabel CreateTextLabel(int y, string text, int pointSize, float wColor = 1.0f)
        {
            TextLabel textLabel = new TextLabel();
            textLabel.TouchEvent += TextLabel_TouchEvent;
            textLabel.Position2D = new Position2D(0, y);
            textLabel.Text = text;
            textLabel.PointSize = pointSize;

            textLabel.WidthResizePolicy = ResizePolicyType.UseNaturalSize;
            textLabel.HeightResizePolicy = ResizePolicyType.UseNaturalSize;

            textLabel.HorizontalAlignment = HorizontalAlignment.Center;
            textLabel.VerticalAlignment = VerticalAlignment.Center;

            textLabel.BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.0f);
            textLabel.TextColor = new Color(wColor, wColor, wColor, 1.0f);
            textLabel.PositionUsesPivotPoint = true;
            textLabel.ParentOrigin = Tizen.NUI.ParentOrigin.TopCenter;
            textLabel.PivotPoint = Tizen.NUI.PivotPoint.Center;
            textLabel.FontFamily = "Samsung One 600";
            textLabel.Focusable = true;
            textLabel.FocusGained += TextLabel_FocusGained;
            textLabel.FocusLost += TextLabel_FocusLost;

            return textLabel;
        }

        private void TextLabel_FocusLost(object sender, EventArgs e)
        {
            TextLabel label = sender as TextLabel;
            label.TextColor = Color.White;
        }

        private void TextLabel_FocusGained(object sender, EventArgs e)
        {
            TextLabel label = sender as TextLabel;
            label.TextColor = new Color(0.0f, 0.4f, 0.9f, 1.0f);
        }


        public void PlayShowAnimation()
        {
            Animation showAnimation = new Animation(200);
            showAnimation.AnimateTo(this, "Opacity", 1.0f, 0, 200);
            showAnimation.Play();
        }
        public void PlayHideAnimation()
        {
            Animation showAnimation = new Animation(200);
            showAnimation.AnimateTo(this, "Opacity", 0.0f, 0, 200);
            showAnimation.Play();
        }

        private bool SettingView_TouchEvent(object source, TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up || e.Touch.GetState(0) == PointStateType.Motion)
            {
                foreach(SlideBar slide in slideBarList)
                {
                    slide.FinishMove();
                }
            }
            return false;
        }


    }
}
