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
    internal class GroupLayerView : View
    {
        public int itemCount = 6;
        private List<Item> itemList;
        private VideoItem videoItem;

        private List<GroupItemView> groupItemViewList;
        private List<GroupItemView> jakeViewList;
        private List<GroupItemView> selectedGroup;

        private GroupItemView currentItemView;
        private GroupTitleView groupTitleView;

        private int currentPage = 0;        

        public void SetItemScale(float scale)
        {
            foreach(Item item in itemList)
            {
                item.SetScale(scale);
            }
            videoItem?.SetScale(scale);
        }

        public void SetItemSpeed(float speed)
        {
            foreach (Item item in itemList)
            {
                item.SetAniTime(speed);
            }
            videoItem?.SetAniTime(speed);
        }

        public void SetItemCount(int cnt)
        {
            itemCount = cnt;
        }
        
        public void AllPictures()
        {
            foreach (GroupItemView v in selectedGroup)
            {
                v.StopImageAnimation();
                v.Opacity = 0.0f;
            }
            InitPage();
            PlayNextView();
        }

        private string current_user = "";

        public void ChangeUserPage(string name)
        {
            Tizen.Log.Error("PhotoSlide", "change user");
            current_user = name;
            currentItemView.StopImageAnimation();
            currentItemView.Opacity = 0.0f;
            PlayNextView(true);
        }

        public void ResetThisPage()
        {
            current_user = "";
            currentItemView.StopImageAnimation();
            currentItemView.Opacity = 0.0f;

            //currentPage--;//Change current->re current
            PlayNextView();
        }

        private VideoItem CreateVideoItem(string name)
        {
            VideoItem item = null;
            /*
            VideoItem item = new VideoItem();
            item.ResourceUrl = CommonResource.GetResourcePath() + "/video/" + name;
            item.Size2D = new Size2D(594, 396);
            */
            return item;
        }

        public void SaveVideoItem()
        {
            videoItem = CreateVideoItem("demoVideo.mp4");
        }

        public void InitPage()
        {
            SaveVideoItem();
            SaveImageItemList(itemList);

            List<ImageItemData> dataArray1 = new List<ImageItemData>();
            List<ImageItemData> dataArray2 = new List<ImageItemData>();
            List<ImageItemData> dataArray3 = new List<ImageItemData>();

            for (int i = 0; i < MapXYData.positions1.Length; i++)
            {
                dataArray1.Add(new ImageItemData(MapXYData.positions1[i]));
            }

            for (int i = 0; i < MapXYData.positions2.Length; i++)
            {
                dataArray2.Add(new ImageItemData(MapXYData.positions2[i]));
            }

            for (int i = 0; i < MapXYData.positions3.Length; i++)
            {
                dataArray3.Add(new ImageItemData(MapXYData.positions3[i]));
            }
            
            GroupItemView groupItemView1 = new GroupItemView(this, 1, dataArray1, itemList);
            GroupItemView groupItemView2 = new GroupItemView(this, 2, dataArray2, itemList);
            GroupItemView groupItemView3 = new GroupItemView(this, 3, dataArray2, itemList, false, videoItem);
            GroupItemView groupItemView4 = new GroupItemView(this, 4, dataArray3, itemList, true);

            groupItemView2.Opacity = 0.0f;
            groupItemView3.Opacity = 0.0f;
            groupItemView4.Opacity = 0.0f;


            groupItemViewList.Add(groupItemView1);
            groupItemViewList.Add(groupItemView2);
            groupItemViewList.Add(groupItemView3);
            groupItemViewList.Add(groupItemView4);

            this.Add(groupItemView1);
            this.Add(groupItemView2);
            this.Add(groupItemView3);
            this.Add(groupItemView4);

            selectedGroup = groupItemViewList;
            currentItemView = selectedGroup[0];

            groupTitleView.RaiseToTop();
            groupTitleView.PlayAnimation();
        }

        public void RemoveInitPage()
        {
            foreach(GroupItemView v in groupItemViewList)
            {
                v.Unparent();
            }
            groupItemViewList.Clear();
            itemList.Clear();
        }

        private ImageItem CreateImageItem(string name)
        {
            ImageItem img = new ImageItem();
            img.ResourceUrl = name;
            Tizen.Log.Error("PhotoSlide", "name : " + name);
            if(img.NaturalSize2D.Width > img.NaturalSize2D.Height)
            {
                img.OriginalImageSize = new Vector2(594,396);
            }
            else
            {
                img.OriginalImageSize = new Vector2(333, 594);
            }

            if (name.Contains("GettyImages-506699770"))
            {
                ParticleEffectView effectView = new ParticleEffectView();
                effectView.Position2D = new Position2D(0, 0);
                effectView.Size2D = img.OriginalImageSize;
                img.Add(effectView);

                View textView = new View();
                textView.ParentOrigin = Tizen.NUI.ParentOrigin.BottomLeft;
                textView.PivotPoint = Tizen.NUI.PivotPoint.BottomLeft;
                textView.Size2D = new Size2D((int)img.OriginalImageSize.Width, 200);
                textView.Position2D = new Position2D(-40, 35);
                //textView.BackgroundColor = Color.Red;

                TextLabel label = new TextLabel();
                label.TextColor = new Color(0.9f, 0.9f, 0.9f, 1.0f);
                label.PointSize = 30.0f;
                label.FontFamily = "Samsung One 500";
                label.Text = "My Son's Birthday";

                textView.Add(label);

                TextLabel label2 = new TextLabel();
                label2.TextColor = new Color(0.85f, 0.85f, 0.85f, 1.0f);
                label2.PointSize = 15.0f;
                label2.FontFamily = "Samsung One 400";
                label2.Text = "26, Feburary 2019";
                label2.Position2D = new Position2D(0,50);

                textView.Add(label2);
                img.Add(textView);
            }
            return img;
        }

        public void SaveImageItemList(List<Item> itemList, string resPath = "images")
        {
            List<String> imageFileList = new List<String>();
            String FolderName = CommonResource.GetResourcePath() + resPath + "/";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(FolderName);
            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(".jpg") == 0)
                {
                    String FileNameOnly = File.Name.Substring(0, File.Name.Length - 4);
                    String FullFileName = File.FullName;

                    imageFileList.Add(FullFileName);
                }
            }
            imageFileList.Sort();
            foreach (string str in imageFileList)
            {
                itemList.Add(CreateImageItem(str));
            }
        }

        public GroupLayerView()
        {
            WidthResizePolicy = ResizePolicyType.FillToParent;
            HeightResizePolicy = ResizePolicyType.FillToParent;

            itemList = new List<Item>();

            groupItemViewList = new List<GroupItemView>();
            jakeViewList = new List<GroupItemView>();

            groupTitleView = new GroupTitleView("All Pictures", "35 out of 35");
            this.Add(groupTitleView);

            InitPage();
            PlayNextView();
        }
        
        public void PlayNextView(bool isJakeBtn = false)
        {
            currentItemView = selectedGroup[currentPage];

            currentItemView.Position2D = new Position2D(0, 200);
            currentItemView.InitItems();
            currentItemView.Opacity = 1.0f;

            currentItemView.PlayImageAnimation(itemCount);

            currentPage++;

            if(isJakeBtn)
            {
                groupTitleView.SetTextAndPoint("Shared by "+current_user, "12 out of 35");
                groupTitleView.RaiseToTop();
                groupTitleView.PlayAnimation();
            }
            else
            {
                if (currentPage == 4)
                {
                    groupTitleView.SetTextAndPoint("New Photo", "Shared by Jake");
                    groupTitleView.RaiseToTop();
                    groupTitleView.PlayAnimation();
                    videoItem?.Play();
                }
                else
                {
                    videoItem?.Stop();
                }

            }

            if (currentPage == selectedGroup.Count)
                currentPage = 0;
        }

        public void RegisterImageTouchCallback(EventHandler callback)
        {
            foreach(GroupItemView view in groupItemViewList)
            {
                view.touchStartHandler += callback;
            }
        }

        private void GroupItemView_ImageViewTouched(object sender, EventArgs e)
        {
            groupTitleView.HideView();
        }

        public void ShowTextView()
        {
            groupTitleView.PlayAnimation();
        }

        public void HideTextView()
        {
            groupTitleView.HideView();
        }

        public void PauseImageAnimation()
        {
            currentItemView.PauseImageAnimation();
        }

        public void PlayImageAnimation(bool isText = true)
        {
            currentItemView?.PlayImageAnimation(itemCount);

            if(isText)
                groupTitleView.PlayAnimation();
        }

    }
}
