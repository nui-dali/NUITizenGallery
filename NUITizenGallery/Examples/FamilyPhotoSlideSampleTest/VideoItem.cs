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
using Tizen.NUI.BaseComponents;

namespace NUIPhotoSlide
{
    internal class VideoItem : Item
    {
        public VideoItem()
        {
            this.Type = "Video";
            myView = new VideoView();
            myView.BackgroundColor = Tizen.NUI.Color.White;
            myView.Opacity = 0.0f;

            myView.HeightResizePolicy = Tizen.NUI.ResizePolicyType.FillToParent;
            myView.WidthResizePolicy = Tizen.NUI.ResizePolicyType.FillToParent;

            this.HeightResizePolicy = Tizen.NUI.ResizePolicyType.FitToChildren;
            this.WidthResizePolicy = Tizen.NUI.ResizePolicyType.FitToChildren;
            this.Size2D = new Tizen.NUI.Size2D(480, 272);
            this.Add(myView);
        }

        public void Play()
        {
            (myView as VideoView).Muted = true;
            (myView as VideoView).Underlay = false;
            (myView as VideoView).Looping = true;
            (myView as VideoView).Play();
        }

        public void Stop()
        {
            (myView as VideoView).Stop();
        }

        public override string ResourceUrl
        {
            get
            {
                return (myView as VideoView).ResourceUrl;
            }
            set
            {
                (myView as VideoView).ResourceUrl = value;
            }
        }

    }
}
