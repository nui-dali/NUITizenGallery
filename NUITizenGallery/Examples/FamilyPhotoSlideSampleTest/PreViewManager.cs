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
    internal class PreViewManager
    {
        private View rootView;
        private View bg;
        private ImageView[] preView;

        private Item selctedImage;
        private Vector2 preMouse;
        private GroupLayerView groupLayerView;

        private int selectIdx;
        private int prevIdx;
        private int nextIdx;

        public PreViewManager(Window window, GroupLayerView groupLayerView)
        {
            this.groupLayerView = groupLayerView;

            groupLayerView.RegisterImageTouchCallback(ShowPreview);

            rootView = new View();
            rootView.Size2D = new Size2D(1080, 1920);
            rootView.Position2D = new Position2D(0, 0);

            bg = new View();
            bg.Size2D = new Size2D(1080, 1920);
            bg.Position2D = new Position2D(0, 0);
            bg.BackgroundColor = Color.Black;
            bg.Opacity = 0.0f;
            bg.TouchEvent += Bg_TouchEvent;
            rootView.Add(bg);

            window.Add(rootView);

            preView = new ImageView[3];
            for(int i=0;i<3; i++)
            {
                preView[i] = new ImageView();
                preView[i].Opacity = 0.0f;
                preView[i].ParentOrigin = ParentOrigin.TopLeft;
                preView[i].PivotPoint = PivotPoint.TopLeft;
                preView[i].PositionUsesPivotPoint = true;
                preView[i].TouchEvent += PreView_TouchEvent;
                rootView.Add(preView[i]);
            }
        }

        private bool Bg_TouchEvent(object source, View.TouchEventArgs e)
        {
            //blocking touch event
            return false;
        }

        private void ShowPreview(object sender, EventArgs e)
        {
            groupLayerView.PauseImageAnimation();

            rootView.RaiseToTop();

            selctedImage = sender as ImageItem;
            preView[0].ResourceUrl = selctedImage.PrevItem.ResourceUrl;
            preView[0].Opacity = 1.0f;

            preView[2].ResourceUrl = selctedImage.NextItem.ResourceUrl;
            preView[2].Opacity = 1.0f;
            

            preView[1].ResourceUrl = selctedImage.ResourceUrl;
            preView[1].Size2D = selctedImage.Size2D;
            preView[1].Position2D = selctedImage.ScreenPosition;

            Animation ani = new Animation(600);
            AlphaFunction alpha = new AlphaFunction(new Vector2(0.175f, 0.885f), new Vector2(0.32f, 1.275f));
            Position2D pos = new Position2D();
            Size2D size = new Size2D();
            if (preView[1].SizeWidth > preView[1].SizeHeight)
            {
                size.Width = 1080;
                size.Height = 700;
                pos.X = 0;
                pos.Y = 960-350;
            }
            else
            {
                size.Width = 1080;
                size.Height = 1920;
                pos.X = 0;
                pos.Y = 0;
            }
            ani.AnimateTo(preView[1], "Position", new Position(pos), alpha);
            ani.AnimateTo(preView[1], "Size", new Size(size), alpha);
            ani.AnimateTo(preView[1], "opacity", 1.0f,0, 400);
            ani.AnimateTo(bg, "opacity", 0.6f);
            ani.Play();

            preView[0].BackgroundColor = Color.White;
            SetNextPositionSize(preView[0], selctedImage.PrevItem.Size2D, -1080);
            SetNextPositionSize(preView[2], selctedImage.NextItem.Size2D, 1080);
            prevIdx = 0;
            selectIdx = 1;
            nextIdx = 2;
        }

        bool isAnimating = false;
        private bool PreView_TouchEvent(object source, View.TouchEventArgs e)
        {
            ImageView selected = source as ImageView;
            if (e.Touch.GetState(0) == PointStateType.Down)
            {
                preMouse = e.Touch.GetScreenPosition(0);
            }

            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                Vector2 currentPos = e.Touch.GetScreenPosition(0);
                int move = (int)(currentPos.X - preMouse.X);
                
                if (move > 20)
                {
                    if (!isAnimating)
                    {
                        isAnimating = true;
                        Animation ani = new Animation(100);
                        ani.AnimateBy(preView[selectIdx], "position", new Position(1080, 0, 0));
                        ani.AnimateBy(preView[prevIdx], "position", new Position(1080, 0, 0));
                        ani.Play();
                        ani.Finished += PrevMoveFinish;
                    }
                }
                else if (move < -20)
                {
                    if(!isAnimating)
                    {
                        isAnimating = true;
                        Animation ani = new Animation(100);
                        ani.AnimateBy(preView[selectIdx], "position", new Position(-1080, 0, 0));
                        ani.AnimateBy(preView[nextIdx], "position", new Position(-1080, 0, 0));
                        ani.Play();
                        ani.Finished += NextMoveFinish;
                    }
                }
                else
                {
                    Item item = selctedImage;

                    Tizen.Log.Error("PhotoSlide", "Click");
                    groupLayerView.PlayImageAnimation(false);

                    Animation ani = new Animation(600);
                    AlphaFunction alpha = new AlphaFunction(new Vector2(0.175f, 0.885f), new Vector2(0.32f, 1.275f));
                    ani.AnimateTo(preView[selectIdx], "Size", item.Size, alpha);
                    ani.AnimateTo(preView[selectIdx], "Position", new Position(item.ScreenPosition), alpha);
                    ani.AnimateTo(preView[selectIdx], "opacity", 0.0f, alpha);
                    ani.AnimateTo(bg, "opacity", 0.0f);
                    ani.Play();
                    ani.Finished += DisapperFinish;
                }
            }
            return false;
        }

        public void SetNextPositionSize(ImageView view, Size2D chkSize, int x)
        {
            Position2D pos = new Position2D();
            Size2D size = new Size2D();
            if (chkSize.Width > chkSize.Height)
            {
                size.Width = 1080;
                size.Height = 700;
                pos.X = x;
                pos.Y = 960 - 350;
            }
            else
            {
                Tizen.Log.Error("PhotoSlide", "height");
                size.Width = 1080;
                size.Height = 1920;
                pos.X = x;
                pos.Y = 0;
            }
            view.Position2D = new Position2D(x, pos.Y); ;
            view.Size2D = size;

        }
        private void PrevMoveFinish(object sender, EventArgs e)
        {
            int temp = nextIdx;
            nextIdx = selectIdx;
            selectIdx = prevIdx;
            prevIdx = temp;

            selctedImage = selctedImage.PrevItem;
            preView[prevIdx].ResourceUrl = selctedImage.PrevItem.ResourceUrl;
            SetNextPositionSize(preView[prevIdx], selctedImage.PrevItem.Size2D, -1080);

            isAnimating = false;
        }

        private void NextMoveFinish(object sender, EventArgs e)
        {
            int temp = prevIdx;
            prevIdx = selectIdx;
            selectIdx = nextIdx;
            nextIdx = temp;

            selctedImage = selctedImage.NextItem;
            preView[nextIdx].ResourceUrl = selctedImage.NextItem.ResourceUrl;
            SetNextPositionSize(preView[nextIdx], selctedImage.NextItem.Size2D, 1080);

            isAnimating = false;
        }

        private void DisapperFinish(object sender, EventArgs e)
        {
            preView[selectIdx].ResourceUrl = "";
            rootView.LowerToBottom();
            preView[prevIdx].Opacity = 0.0f;
            preView[nextIdx].Opacity = 0.0f;
        }
        
    }
}
