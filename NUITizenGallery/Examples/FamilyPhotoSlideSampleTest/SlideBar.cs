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
    internal class SlideBar : View
    {
        public event EventHandler ItemSelectEvent;

        private int SLIDE_SIZE = 650;//Fixed in this sample

        private View selectBtn;
        private View touchArea;
        private Vector2 preMouse;
        private Vector2 current;
        private int stepSpan;
        private int selectIdx;

        private bool isSelecting = false;

        public SlideBar(int y, float gray, int stepCnt, string[] textList, int defaultValue = 1)
        {
            selectIdx = defaultValue;
            stepSpan = SLIDE_SIZE / stepCnt;
            
            this.ParentOrigin = Tizen.NUI.ParentOrigin.TopCenter;
            this.PivotPoint = Tizen.NUI.PivotPoint.Center;
            this.PositionUsesPivotPoint = true;
            this.Position2D = new Position2D(0, y);
            this.Size2D = new Size2D(SLIDE_SIZE, 20);

            View slideBar = new View();
            slideBar.ParentOrigin = Tizen.NUI.ParentOrigin.Center;
            slideBar.PivotPoint = Tizen.NUI.PivotPoint.Center;
            slideBar.PositionUsesPivotPoint = true;
            slideBar.BackgroundColor = new Color(gray, gray, gray, 0.8f);
            slideBar.Size2D = new Size2D(SLIDE_SIZE, 3);

            this.Add(slideBar);

            int x = 0;
            for (int i = 0; i <= stepCnt; i++)
            {
                View step = new View();
                step.BackgroundColor = new Color(gray, gray, gray, 0.8f);
                step.Size2D = new Size2D(2, 18);
                step.ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft;
                step.PivotPoint = Tizen.NUI.PivotPoint.CenterLeft;
                step.PositionUsesPivotPoint = true;
                step.Position2D = new Position2D(x, 0);
                this.Add(step);

                TextLabel text = new TextLabel();
                text.Text = textList[i];
                text.PointSize = 12;
                text.TextColor = new Color(gray, gray, gray, 0.8f);
                text.ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft;
                text.PivotPoint = Tizen.NUI.PivotPoint.CenterLeft;
                text.PositionUsesPivotPoint = true;
                text.Position2D = new Position2D(x - (text.NaturalSize2D.Width / 2), 25);
                this.Add(text);
                x += stepSpan;
            }

            touchArea = new View();
            touchArea.ParentOrigin = Tizen.NUI.ParentOrigin.Center;
            touchArea.PivotPoint = Tizen.NUI.PivotPoint.Center;
            touchArea.PositionUsesPivotPoint = true;
            //touchArea.BackgroundColor = new Color(1.0f, 0.0f, 0.0f, 0.3f);
            touchArea.Size2D = new Size2D(SLIDE_SIZE, 150);
            touchArea.Position2D = new Position2D(0, 0);
            touchArea.TouchEvent += TouchArea_TouchEvent;
            this.Add(touchArea);
            
            selectBtn = new View();;
            selectBtn.Size2D = new Size2D(50, 50);
            selectBtn.BackgroundColor = new Color(0.7f, 0.7f, 0.7f, 0.8f);
            //selectBtn.ResourceUrl = CommonResource.GetResourcePath() + "/images2/" + "option_btn.png";
            selectBtn.Scale = new Vector3(0.25f, 0.25f, 1.0f);

            selectBtn.ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft;
            selectBtn.PivotPoint = Tizen.NUI.PivotPoint.Center;
            selectBtn.PositionUsesPivotPoint = true;
            selectBtn.Position2D = new Position2D(((defaultValue - 1) * stepSpan)+1, 0);

            this.Add(selectBtn);
        }

        public int SelectIdx
        {
            get
            {
                return selectIdx;
            }
        }
        
        private bool TouchArea_TouchEvent(object source, TouchEventArgs e)
        {            
            current = e.Touch.GetLocalPosition(0);
            if (!isSelecting && (e.Touch.GetState(0) == PointStateType.Down))
            {
                isSelecting = true;
                preMouse = current;

                selectBtn.PositionX = ((float)Math.Round(current.X / stepSpan) * stepSpan) + 1;
                selectIdx = ((int)selectBtn.PositionX / stepSpan) + 1;
                isSelecting = false;
                Tizen.Log.Error("PhotoSlide", "SElect IDx: " + selectIdx);

                if (ItemSelectEvent != null)
                {
                    ItemSelectEvent(source, e);
                }


            }
            /*
            if (isSelecting && (e.Touch.GetState(0) == PointStateType.Motion))
            {
                selectBtn.PositionX = current.X;
            }*/

            if (isSelecting && (e.Touch.GetState(0) == PointStateType.Up ) )
            {
            }


            return false;
        }

        public void FinishMove()
        {
            if(isSelecting)
            {
                selectBtn.PositionX = (float)Math.Round(current.X / stepSpan) * stepSpan;
                isSelecting = false;
            }
        }

    }
}
