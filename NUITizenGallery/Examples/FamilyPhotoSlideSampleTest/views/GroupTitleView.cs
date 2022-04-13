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
    internal class GroupTitleView : View
    {
        private ImageView character;
        private TextLabel titleLabel;
        private TextLabel subTitleLabel;
        private Animation textFinishAnimation;
        private Animation textAnimation;

        private TextLabel CreateTextLabel(string text, float point)
        {
            TextLabel textLabel = new TextLabel();
            textLabel.WidthResizePolicy = ResizePolicyType.UseNaturalSize;
            textLabel.HeightResizePolicy = ResizePolicyType.UseNaturalSize;
            textLabel.HorizontalAlignment = HorizontalAlignment.Center;
            textLabel.VerticalAlignment = VerticalAlignment.Center;
            textLabel.BackgroundColor = new Color(0, 0, 0, 0.0f);
            textLabel.TextColor = Color.White;
            textLabel.PositionUsesPivotPoint = true;
            textLabel.ParentOrigin = Tizen.NUI.ParentOrigin.Center;
            textLabel.PivotPoint = Tizen.NUI.PivotPoint.Center;
            textLabel.FontFamily = "Samsung One 600";
            textLabel.MultiLine = true;
            textLabel.PointSize = point;
            textLabel.Text = text;
            return textLabel;
        }

        public GroupTitleView(string mainTitle, string subTitle, int delayTime = 800)
        {
            WidthResizePolicy = ResizePolicyType.FillToParent;
            HeightResizePolicy = ResizePolicyType.FillToParent;

            titleLabel = CreateTextLabel(mainTitle, 55);
            this.Add(titleLabel);

            subTitleLabel = CreateTextLabel(subTitle, 30);
            subTitleLabel.Position2D = new Position2D(0, 70);
            this.Add(subTitleLabel);

            textAnimation = new Animation(1300);
            textAnimation.AnimateTo(this, "Opacity", 1.0f, delayTime, 1300);
            textAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut);
            textAnimation.Finished += TextAni_Finished;

            textFinishAnimation = new Animation(1000);
            textFinishAnimation.AnimateTo(this, "Opacity", 0.0f, 2000, 2400);
            textFinishAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOut);
        }

        public void CreateCharacter()
        {
            character = new ImageView();
            character.PositionUsesPivotPoint = true;
            character.ParentOrigin = Tizen.NUI.ParentOrigin.Center;
            character.PivotPoint = Tizen.NUI.PivotPoint.Center;
            character.ResourceUrl = CommonResource.GetResourcePath() + "/images/" + "character.png";
            character.WidthResizePolicy = ResizePolicyType.UseNaturalSize;
            character.HeightResizePolicy = ResizePolicyType.UseNaturalSize;
            character.Position2D = new Position2D(0, -100);

            this.Add(character);
            
        }
        public void SetTextAndPoint(string mainTitle, string subTitle)
        {
            this.Opacity = 0.0f;

            titleLabel.Text = mainTitle;
            this.Add(titleLabel);
            subTitleLabel.Text = subTitle;
            subTitleLabel.Position2D = new Position2D(0, 100);
            this.Add(subTitleLabel);
            textAnimation.Play();
        }

        private void TextAni_Finished(object sender, EventArgs e)
        {
            textFinishAnimation.Play();
        }

        public void PlayHideAnimation()
        {
            textFinishAnimation.Play();
        }
        public void PlayAnimation()
        {
            textAnimation.Play();
        }

        public void ShowView()
        {
            this.Opacity = 1.0f;
        }
        public void HideView()
        {
            this.Opacity = 0.0f;
        }
    }
}
