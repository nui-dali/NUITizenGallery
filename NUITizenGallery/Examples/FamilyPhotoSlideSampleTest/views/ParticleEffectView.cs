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
    internal class ParticleEffectView : ImageView
    {
        private List<String> particleList;
        public ParticleEffectView() : base()
        {
            particleList = new List<String>();

            String FolderName = CommonResource.GetResourcePath() + "/particle/";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(FolderName);
            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(".png") == 0)
                {
                    String FileNameOnly = File.Name.Substring(0, File.Name.Length - 4);
                    String FullFileName = File.FullName;

                    particleList.Add(FullFileName);
                }
            }
            particleList.Sort();

            PropertyMap property = new PropertyMap();
            PropertyArray array = new PropertyArray();
            for (int i = 0; i < particleList.Count; i++)
            {
                array.PushBack(new PropertyValue(particleList[i]));
            }

            property.Add(ImageVisualProperty.FrameDelay, new PropertyValue(30));
            property.Add(ImageVisualProperty.URL, new PropertyValue(array));
            this.Image = property;

        }
    }
}
