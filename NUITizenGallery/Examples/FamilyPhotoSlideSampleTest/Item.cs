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
    internal class Item : View
    {
        protected View myView;

        private Animation startAnimation;
        private Animation slideAnimation;

        private Animation slideFinishAnimation1;
        private Timer slideTimer;

        public event EventHandler SlideAnimationFinished;

        public bool isScaleAnimation = false;
        public Vector2 OriginalImageSize;

        private float settingScale;
        private int slidingAniTime;
        private float animationSpeed;

        bool isMainImage = false;
        int slidingAniPos = 0;
        Timer timer;

        bool isPlaying;

        public string Type
        {
            get;
            set;
        }

        public Item() : base()
        {
            animationSpeed = 0.2f;
            settingScale = 1.0f;
            slidingAniTime = 10000;
            startAnimation = new Animation(1200);
            slideAnimation = new Animation(slidingAniTime);
            slideAnimation.Finished += SlideAnimation_Finished;
            slideFinishAnimation1 = new Animation(2300);

            slideTimer = new Timer(0);
            slideTimer.Tick += SlideTimer_Tick;
            timer = new Timer(1000);
            timer.Tick += Timer_Tick;

            isPlaying = false;
        }

        public void ChangeMainItem(Item item)
        {
            this.RaiseToTop();
            
            Color color = item.Color;

            item.Color = this.Color;
            this.Color = color;

            Animation changeAni = new Animation(300);
            changeAni.AnimateTo(this, "scale", item.Scale);
            changeAni.AnimateTo(item, "scale", this.Scale);
            changeAni.Play();
        }

        public Item PrevItem
        {
            get;
            set;
        }
        public Item NextItem
        {
            get;
            set;
        }


        public float Ratio
        {
            get;
            set;
        }

        public Position MainPosition
        {
            get;
            set;
        }

        public Position StartPosition
        {
            get;
            set;
        }
        
        public Position FinishPosition
        {
            get;
            set;
        }
        
        public new Color Color
        {
            get
            {
                return myView.Color;
            }
            set
            {
                myView.Color = value;
            }
        }
        public new Size2D NaturalSize2D
        {
            get
            {
                return myView.NaturalSize2D;
            }
        }
        public virtual string ResourceUrl
        {
            get;
            set;
        }

        public void SetScale(float scale)
        {
            settingScale = scale;
            Animation ani = new Animation(400);
            
            if(scale == 1.0f)
            {
                ani.AnimateTo(this, "Scale", new Size(this.Scale.X * 1.25f, this.Scale.X * 1.25f, 1.0f), new AlphaFunction(new Vector2(0.68f, -0.55f), new Vector2(0.265f, 1.55f)));
            }
            else
            {
                ani.AnimateTo(this, "Scale", new Size(this.Scale.X * settingScale, this.Scale.X * settingScale, 1.0f), new AlphaFunction(new Vector2(0.68f, -0.55f), new Vector2(0.265f, 1.55f)));
            }
            ani.Play();
        }
        
        public void SetAniTime(float speed)
        {
            animationSpeed = speed;
            startAnimation.SpeedFactor = 0.5f;
            slideAnimation.SpeedFactor = animationSpeed;
            
        }

        public void ResetValue(float scale = 1.0f, float color = 1.0f)
        {

            this.Opacity = 0.0f;
            
            if (isScaleAnimation)
            {
                slidingAniTime = 5000;
                this.Position2D = new Position2D(this.FinishPosition);
            }
            else
            {
                slidingAniTime = 10000;
                this.Position2D = new Position2D((int)this.MainPosition.X, (int)this.MainPosition.Y + 100);
            }

            this.myView.Scale = new Vector3(1.0f, 1.0f, 1.0f);
            this.Scale = new Vector3(settingScale * scale, settingScale * scale, 1.0f);
            this.Color = new Color(color, color, color, 1.0f);
            Tizen.Log.Error("PhotoSlide", "set :" + settingScale);

            if(isMainImage)
            {
                this.RaiseToTop();
            }
        }

        public void RegisterNewAnimation(bool isMainImage)
        {
            this.isMainImage = isMainImage;

            slidingAniPos = -600;
            if (isMainImage)
            {
                slidingAniPos = -1000;
            }

            startAnimation.Reset();
            startAnimation.SpeedFactor = 0.5f;
            startAnimation.AnimateTo(this, "Opacity", 1.0f, 0, 100);
            if (isScaleAnimation || isMainImage)
            {
                this.Position = MainPosition;
                startAnimation.AnimateTo(this.myView, "Scale", new Size(1.2f, 1.2f, 1.2f), 0, 1200, new AlphaFunction(new Vector2(0.68f, -0.55f), new Vector2(0.265f, 1.55f)));
                
            }
            else
            {
                this.Position = StartPosition;
                startAnimation.AnimateTo(this, "Position", FinishPosition, new AlphaFunction(new Vector2(0.68f, -0.55f), new Vector2(0.265f, 1.55f)));
            }

            slideAnimation.Reset();
            slideAnimation.SpeedFactor = animationSpeed;
            //if (!isScaleAnimation)
            {
                slideAnimation.AnimateBy(this, "Position", new Position(0, slidingAniPos, 0), 0, slidingAniTime);
            }

        }

        private void SlideAnimation_Finished(object sender, EventArgs e)
        {
            if(isPlaying)
            {
                //nextAnimationTimer.Start();
                slideFinishAnimation1.Clear();
                slideFinishAnimation1.SpeedFactor = 0.5f;
                slideFinishAnimation1.DefaultAlphaFunction = new AlphaFunction(new Vector2(0.175f, 0.885f), new Vector2(0.32f, 1.275f));
                slideFinishAnimation1.AnimateBy(this, "Position", new Position(0, -500, 0), 0, 2300);
                slideFinishAnimation1.AnimateBy(this, "Position", new Position((MainPosition.X - FinishPosition.X), (MainPosition.Y - FinishPosition.Y - 200), 0), 200, 1800);
                slideFinishAnimation1.AnimateTo(this, "Opacity", 0.0f, 1000, 1700);
                
                slideFinishAnimation1.Play();

                if (isScaleAnimation)
                {
                    slideTimer.Interval = 2000;
                    slideFinishAnimation1.Duration = 2300;
                }
                else
                {
                    slideTimer.Interval = 0;
                    slideFinishAnimation1.Duration = 2300;
                }
                slideTimer.Start();
                slideFinishAnimation1.Finished += SlideFinishAnimation1_Finished;
            }
        }

        private void SlideFinishAnimation1_Finished(object sender, EventArgs e)
        {

            isPlaying = false;
        }

        private bool SlideTimer_Tick(object source, Timer.TickEventArgs e)
        {
            Tizen.Log.Error("PhotoSlide", "SlideTimer_Tick");
            if (SlideAnimationFinished != null)
            {
                SlideAnimationFinished(this, null);
            }
            return false;
        }
        

        public void ResetAnimation()
        {
            slideFinishAnimation1?.Clear();
        }

        public void PlayAnimation()
        {
            isPlaying = true;
            if (isScaleAnimation)
            {
                timer.Start();
            }
            else
            {
                startAnimation?.Play();
                slideAnimation?.Play();
            }
        }

        private bool Timer_Tick(object source, Timer.TickEventArgs e)
        {
            startAnimation?.Play();
            slideAnimation?.Play();
            return false;
        }

        public void StopAnimation()
        {
            isPlaying = false;
            timer?.Reset();
            timer?.Stop();

            slideTimer?.Stop();
            slideTimer?.Reset();

            startAnimation?.Stop();

            slideAnimation?.Reset();
            //slideAnimation?.Clear();
            slideAnimation?.Stop();

            slideFinishAnimation1.Reset();
            //slideFinishAnimation1.Clear();
            slideFinishAnimation1?.Stop();
        }

        public void PauseAnimation()
        {
            startAnimation?.Pause();
            slideAnimation?.Pause();
            slideFinishAnimation1?.Pause();
        }
        
    }
}
