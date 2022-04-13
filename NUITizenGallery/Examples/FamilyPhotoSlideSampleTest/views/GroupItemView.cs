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
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace NUIPhotoSlide
{
    internal class GroupItemView : View
    {
        public event EventHandler touchStartHandler;

        private GroupLayerView parentGroup;
        private List<Item> imageItemList;
        
        private int item_event_checker = 0;
        
        private Item selected;

        public void InitItems()
        {
            Tizen.Log.Error("PhotoSlide", "Init Items");
            float color = 1.0f;
            float scale = 1.0f;
            float ratio = 0.12f;

            if (imageItemList.Count <= 3)
                ratio = 0.3f;

            for(int i = 0; i < imageItemList.Count; i++)
            {
                imageItemList[i].ResetValue(scale, color);

                Tizen.Log.Error("PhotoSlide", "scale :" + scale);
                scale -= ratio;
                color -= 0.15f;
            }
        }

        public GroupItemView(GroupLayerView parentGroup, int page, List<ImageItemData> data, 
            List<Item> itemList, bool isScaleAnimation = false, VideoItem vitem = null)
        {
            this.parentGroup = parentGroup;
            this.imageItemList = new List<Item>();

            Item prev = null;
            View mainImage = null;
            Tizen.Log.Error("PhotoSlide", " data length : " + data.Count);
            for (int idx = 0; idx < data.Count; idx++)
            {
                
                if(vitem != null && idx ==0)
                {
                    imageItemList.Add(vitem);
                    vitem.SlideAnimationFinished += AnimationFinished;
                    vitem.MainPosition = data[0].Position;
                    vitem.FinishPosition = data[idx].Position;
                    vitem.isScaleAnimation = isScaleAnimation;
                    vitem.RegisterNewAnimation(true);
                    this.Add(vitem);
                    continue;
                }

                ImageItem item = itemList[(page - 1) * 6 + idx] as ImageItem;
                item.SlideAnimationFinished += AnimationFinished;

                item.DesiredWidth = (int)(item.OriginalImageSize.Width);
                item.DesiredHeight = (int)(item.OriginalImageSize.Height);
                imageItemList.Add(item);

                item.MainPosition = data[0].Position;
                item.FinishPosition = data[idx].Position;
                item.isScaleAnimation = isScaleAnimation;

                this.Add(item);

                if (prev != null)
                {
                    prev.NextItem = item;
                    item.PrevItem = prev;
                }
                prev = item;

                if (mainImage == null)
                {
                    mainImage = item;
                }

                if (isScaleAnimation)
                {
                    item.StartPosition = item.FinishPosition;
                }
                else
                {
                    item.StartPosition = new Position((int)(mainImage.PositionX + (mainImage.SizeWidth / 2)) - (int)(item.SizeWidth / 2),
                        (int)(mainImage.PositionY + (mainImage.SizeHeight / 2)), 0.0f);
                }
                
                item.RegisterNewAnimation((idx == 0) ? true : false);
                item.TouchEvent += View_TouchEvent;
                item.LowerToBottom();
            }

            imageItemList[0].PrevItem = imageItemList[data.Count - 1];
            imageItemList[data.Count - 1].NextItem = imageItemList[0];

            imageItemList[0].RaiseToTop();
            
            selected = imageItemList[0];
        }

        private void AnimationFinished(object source, EventArgs e)
        {
            Item item = source as Item;
            
            int length = parentGroup.itemCount;
            if (item.isScaleAnimation && parentGroup.itemCount>3)
                length = 3;

            item_event_checker++;
            Tizen.Log.Error("PhotoSlide", "play next - " + item_event_checker);
            Tizen.Log.Error("PhotoSlide", "play len - " + length);
            if (item_event_checker >= length)
            {
                parentGroup.PlayNextView();
                item_event_checker = 0;
            }
        }

        public void PlayImageAnimation(int itemCount = 6)
        {
            int idx = 0;
            foreach (Item item in imageItemList)
            {
                item.PlayAnimation();
                idx++;
                if (idx == itemCount)
                    break;
            }
        }

        public void ResetImageAnimation()
        {
            foreach (Item item in imageItemList)
            {
                item.ResetAnimation();
            }
        }
        public void PauseImageAnimation()
        {
            foreach (Item item in imageItemList)
            {
                item.PauseAnimation();
            }
        }
        public void StopImageAnimation()
        {
            foreach (Item item in imageItemList)
            {
                item.StopAnimation();
            }
        }

        private bool View_TouchEvent(object source, View.TouchEventArgs e)
        {
            Item item = source as Item;
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                if(item == selected)
                {
                    selected = source as Item;

                    //PauseImageAnimation();

                    if (touchStartHandler != null)
                    {
                        touchStartHandler(selected, null);
                    }
                }
                else
                {
                    item.ChangeMainItem(selected);
                    selected = item;
                }
            }
            return false;
        }
    }
}
