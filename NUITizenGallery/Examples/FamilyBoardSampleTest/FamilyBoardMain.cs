
using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.FH.FamilyBoard
{
    public class FamilyBoardMain : ILifecycleObserver
    {
        private View mRootView;

        // background
        private TapGestureDetector mRootViewTapGestureDetector;

        // date time
        private View mTimeView;
        private TextLabel mTimeLabel;
        private View mAmPmView;
        private TextLabel mAmPmLabel;
        private View mDateView;
        private TextLabel mDateLabel;

        // menu
        private View mMenuRootView;

        private View mMenuBackgroundView;
        private NPatchVisual mNPatchVisual;
        private Animation mMenuBackgroundSizeAnimation;
        private Animation mMenuBackgroundOpacityAnimation;

        private FlexibleView mMenuList;
        private MenuListBridge mMenuListBridge;
        private Animation mMenuListOpacityAnimation;
        private Timer mKeepMenuShowTimer;

        // view
        private View mTopView;
        private View mLongPressedView;

        // gesture
        private struct ImageGesture
        {
            public View root;
            public View background;
            public BorderVisual border;
            public VisualView borderBackground;
            public View shadowView;
            public ImageView image;
            public TapGestureDetector tapDetector;
            public PanGestureDetector panDetector;
            public LongPressGestureDetector longPressDetector;
            public bool isShadow;
        }
        private Dictionary<string, ImageGesture> mImageGestureTable;
        private int mCurrentImagesCount = 0; // id generator

        // text
        private struct TextGesture
        {
            public View root;
            public View background;
            public BorderVisual border;
            public VisualView borderBackground;
            public TextLabel text;
            public TapGestureDetector tapDetector;
            public PanGestureDetector panDetector;
            public LongPressGestureDetector longPressDetector;
        }
        private Dictionary<string, TextGesture> mTextGestureTable;
        private int mCurrentTextsCount = 0; // id generator

        private float mLastRadian;
        private float mLastDistance;
        private float mLastPositionX;
        private float mLastPositionY;

        //
        private View mDeleteRootView;
        private View mDeleteButtonView;
        private View mDeleteArrayDownView;
        private TextLabel mDeleteTextLabel;
        private TapGestureDetector mDeleteTapGestureDetector;
        private LongPressGestureDetector mDeleteLongPressGestureDetector;
        private Timer mKeepButtonShowTimer;

        private readonly int MAX_IMAGE_COUNT = 20;
        private readonly int MAX_TEXT_COUNT = 20;

        private readonly int SCREEN_WIDTH = 1080;
        private readonly int SCREEN_HEIGHT = 1920;

        private readonly int MENU_WIDTH = 270 + 40;
        private readonly int MENU_HEIGHT = 369 + 50;

        // images
        private static string FB_BACKGROUND_IMAGE = CommonResource.GetResourcePath() + "familyboard_setting_bg2.png";

        private static string FB_MENU_IMAGE = CommonResource.GetResourcePath() + "add_menu/";
        private static string FB_MENU_BACKGROUND_IMAGE = FB_MENU_IMAGE + "fb_menu_bg.png";
        private static string FB_MENU_PICTURE_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_picture.png";
        private static string FB_MENU_MEMO_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_memo.png";
        private static string FB_MENU_STICKERS_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_stickers.png";
        private static string FB_MENU_DRAWING_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_drawing.png";
        private static string FB_MENU_TEXT_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_text.png";
        private static string FB_MENU_MUSIC_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_music.png";
        private static string FB_MENU_NEW_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_new.png";
        private static string FB_MENU_OPEN_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_open.png";
        private static string FB_MENU_MORE_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_more.png";

        private static string FB_EDIT_DELETE_BG = CommonResource.GetResourcePath() + "edit_bubble_bg_down.png";
        private static string FB_EDIT_DELETE_ARROW_DOWN = CommonResource.GetResourcePath() + "edit_bubble_arrow_down.png";
        private static string FB_EDIT_DELETE_NORMAL = CommonResource.GetResourcePath() + "fb_menu_btn_delete_nor.png";
        private static string FB_EDIT_DELETE_PRESSED = CommonResource.GetResourcePath() + "fb_menu_btn_delete_press.png";
        private static string FB_EDIT_DELETE_DIM = CommonResource.GetResourcePath() + "fb_menu_btn_delete_dim.png";

        public void Activate()
        {
            mRootView = new View();
            mRootView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT);
            mRootView.BackgroundImage = FB_BACKGROUND_IMAGE;

            mRootViewTapGestureDetector = new TapGestureDetector();
            mRootViewTapGestureDetector.Attach(mRootView);
            mRootViewTapGestureDetector.Detected += OnRootViewTapGestureDetected;

            ImageManager.Instance.SetBackgroundImage(FB_BACKGROUND_IMAGE);

            CreateDateTime();

            // for test
            mImageGestureTable = new Dictionary<string, ImageGesture>(MAX_IMAGE_COUNT);

            mTextGestureTable = new Dictionary<string, TextGesture>(MAX_TEXT_COUNT);
        }

        public void Reactivate()
        {
            // show images
            ShowSelectedImage();

            // show texts
            ShowEditedText();

            // clear db
            ImageManager.Instance.RemoveAll();

            //
            mRootViewTapGestureDetector.Attach(mRootView);
        }

        public void Deactivate()
        {
            //
            StopMenuAnimation();

            //
            DestroyDateTime();

            //
            DestroyMenu();

            if (mRootViewTapGestureDetector != null)
            {
                mRootViewTapGestureDetector.Detected -= OnRootViewTapGestureDetected;
                mRootViewTapGestureDetector.Detach(mRootView);
                mRootViewTapGestureDetector.Dispose();
                mRootViewTapGestureDetector = null;
            }

            if (mRootView != null)
            {
                //NUIApplication.GetDefaultWindow().Remove(mRootView);
                //mRootView.Dispose();
                //mRootView = null;
            }
        }

        public View GetRootView()
        {
            return mRootView;
        }

        public void HideMenu()
        {
            //
            mMenuRootView.Hide();

            //
            mMenuBackgroundView.Opacity = 0.0f;
            mMenuBackgroundView.Size = new Size(MENU_WIDTH * 1.0f, MENU_HEIGHT * 0.8f, 1.0f);

            //
            mMenuList.Opacity = 0.0f;
        }

        public void DetachTapGesture()
        {
            mRootViewTapGestureDetector.Detach(mRootView);
        }

        public void ChangeBackground()
        {
            if (ImageManager.Instance.GetBackgroundImage() != null)
            {
                mRootView.BackgroundImage = ImageManager.Instance.GetBackgroundImage();
            }
        }

        private void OnRootViewTapGestureDetected(object source, TapGestureDetector.DetectedEventArgs e)
        {
            if (e.TapGesture.State == Gesture.StateType.Started)
            {
                HideDeleteButton();

                if (e.View != mRootView)
                    return;

                Vector2 pos = e.TapGesture.ScreenPoint;
                int newX = (int)pos.X;
                if (newX + MENU_WIDTH > SCREEN_WIDTH)
                {
                    newX = SCREEN_WIDTH - MENU_WIDTH;
                }

                Position menuPivotPoint = PivotPoint.TopLeft;
                Position menuParentOrigin = ParentOrigin.TopLeft;
                int newY = (int)pos.Y;
                if (newY + MENU_HEIGHT > SCREEN_HEIGHT || newY - MENU_HEIGHT >= 0)
                {
                    newY = newY - MENU_HEIGHT;
                    menuPivotPoint = PivotPoint.BottomLeft;
                    menuParentOrigin = ParentOrigin.BottomLeft;
                }

                if (mMenuRootView == null)
                {
                    CreateMenu(newX, newY);
                    HideMenu();
                }

                if (mMenuRootView.Visibility)
                {
                    StopMenuAnimation();
                    HideMenu();
                }
                else
                {
                    //
                    mMenuRootView.Position2D = new Position2D(newX, newY);
                    mMenuRootView.Show();
                    mMenuRootView.RaiseToTop();

                    //
                    mMenuBackgroundView.PivotPoint = menuPivotPoint;
                    mMenuBackgroundView.ParentOrigin = menuParentOrigin;

                    //
                    StartMenuAnimation();
                }
            }
        }

        private void StartMenuAnimation()
        {
            // background size animation in menu list.
            if (mMenuBackgroundSizeAnimation == null)
            {
                mMenuBackgroundSizeAnimation = new Animation();
                mMenuBackgroundSizeAnimation.Duration = 250;
                mMenuBackgroundSizeAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOutSquare);
            }
            mMenuBackgroundSizeAnimation.Clear();
            mMenuBackgroundSizeAnimation.Finished += OnMenuAlphaAnimationFinished;
            mMenuBackgroundSizeAnimation.AnimateTo(mMenuBackgroundView, "Size", new Size(MENU_WIDTH * 1.0f, MENU_HEIGHT * 1.0f, 1.0f));
            mMenuBackgroundSizeAnimation.Play();

            // background opacity animation in menu list.
            if (mMenuBackgroundOpacityAnimation == null)
            {
                mMenuBackgroundOpacityAnimation = new Animation();
                mMenuBackgroundOpacityAnimation.Duration = 100;
                mMenuBackgroundOpacityAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.Linear);
            }
            mMenuBackgroundOpacityAnimation.Clear();
            mMenuBackgroundOpacityAnimation.AnimateTo(mMenuBackgroundView, "Opacity", 0.95f);
            mMenuBackgroundOpacityAnimation.Play();

            // list opacity animation in menu list.
            if (mMenuListOpacityAnimation == null)
            {
                mMenuListOpacityAnimation = new Animation();
                mMenuListOpacityAnimation.Duration = 50;
                mMenuListOpacityAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.Linear);
            }
            mMenuListOpacityAnimation.Clear();
            mMenuListOpacityAnimation.AnimateTo(mMenuList, "Opacity", 1.0f);
            mMenuListOpacityAnimation.PlayAfter(150);

            // timer to hide menu list
            if (mKeepMenuShowTimer == null)
            {
                mKeepMenuShowTimer = new Timer(2000);
            }
            mKeepMenuShowTimer.Tick += OnHideMenuTimerTick;
        }

        private void StopMenuAnimation()
        {
            if (mMenuBackgroundSizeAnimation != null && mMenuBackgroundSizeAnimation.State == Animation.States.Playing)
            {
                mMenuBackgroundSizeAnimation.Finished -= OnMenuAlphaAnimationFinished;
                mMenuBackgroundSizeAnimation.Stop(Animation.EndActions.StopFinal);
            }

            if (mMenuBackgroundOpacityAnimation != null && mMenuBackgroundOpacityAnimation.State == Animation.States.Playing)
            {
                mMenuBackgroundOpacityAnimation.Stop(Animation.EndActions.StopFinal);
            }

            if (mMenuListOpacityAnimation != null && mMenuListOpacityAnimation.State == Animation.States.Playing)
            {
                mMenuListOpacityAnimation.Stop(Animation.EndActions.StopFinal);
            }

            if (mKeepMenuShowTimer != null && mKeepMenuShowTimer.IsRunning())
            {
                mKeepMenuShowTimer.Tick -= OnHideMenuTimerTick;
                mKeepMenuShowTimer.Stop();
            }
        }

        private void OnMenuAlphaAnimationFinished(object sender, EventArgs e)
        {
            mKeepMenuShowTimer.Start();
        }

        private void ResetHideMenuTimer()
        {
            mKeepMenuShowTimer.Stop();
            mKeepMenuShowTimer.Start();
        }

        private bool OnHideMenuTimerTick(object source, Timer.TickEventArgs e)
        {
            HideMenu();
            return false;
        }

        private void ShowSelectedImage()
        {
            int imageCount = ImageManager.Instance.ImageCount();
            if (imageCount == 0 || mImageGestureTable.Count + imageCount >= MAX_IMAGE_COUNT)
                return;

            for (int i = 0; i < imageCount; i++)
            {
                ImageGesture ig = new ImageGesture();
                ig.root = new View();
                if (ImageManager.Instance.GetDataItemType(i) == ImageManager.ItemType.PHOTO)
                {
                    ig.root.Name = $"image_photo_{mCurrentImagesCount++}";
                }
                else
                {
                    ig.root.Name = $"image_sticker_{mCurrentImagesCount++}";
                }
                ig.root.Position2D = new Position2D((SCREEN_WIDTH - 400) / 2 + i * 14, (SCREEN_HEIGHT - 400) / 2 + i * 14);

                ig.background = new View();
                ig.background.BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.15f);
                ig.background.PositionUsesPivotPoint = true;
                ig.background.PivotPoint = PivotPoint.Center;
                ig.background.ParentOrigin = ParentOrigin.Center;

                ig.borderBackground = new VisualView();
                ig.border = new BorderVisual()
                {
                    BorderSize = 1.0f,
                    Color = Color.White,
                    AntiAliasing = true,
                };
                ig.borderBackground.AddVisual("border", ig.border);
                ig.borderBackground.BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.2f);
                ig.borderBackground.PositionUsesPivotPoint = true;
                ig.borderBackground.PivotPoint = PivotPoint.Center;
                ig.borderBackground.ParentOrigin = ParentOrigin.Center;
                ig.image = new ImageView();
                ig.image.ResourceUrl = ImageManager.Instance.GetImageFile(i);
                ig.image.PositionUsesPivotPoint = true;
                ig.image.PivotPoint = PivotPoint.Center;
                ig.image.ParentOrigin = ParentOrigin.Center;
                                             
                int width = 400;
                int height = 400;
                if (ImageManager.Instance.GetDataItemType(i) == ImageManager.ItemType.STICKER)
                {
                    width = 350;
                    height = 350;
                }

                int realWidth = ig.image.NaturalSize2D.Width;
                int realHeight = ig.image.NaturalSize2D.Height;
                if (realWidth <= realHeight)
                {
                    width = height * realWidth / realHeight;
                }
                else
                {
                    height = width * realHeight / realWidth;
                }
                
                ig.isShadow = false;
                if (ImageManager.Instance.GetDataItemType(i) == ImageManager.ItemType.PHOTO)
                {
                    string style = ImageManager.Instance.GetFrameStyle(i);
                    Tizen.Log.Error("PhotoSlide", "select style : " + style);
                    if (style.Contains("01"))//Frameless
                    {
                        ig.isShadow = true;
                        ig.shadowView = new View();
                        ig.shadowView.BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.3f);
                    }
                    else if (style.Contains("02"))//No Shadow
                    {
                        ig.shadowView = new View();
                        ig.shadowView.Hide();
                    }
                    else if (style.Contains("04"))//Thin frame
                    {
                        ig.shadowView = CreateWhiteFrame((int)(width * 1.03f), (int)(height * 1.03f));
                    }
                    else if (style.Contains("05"))//Bold
                    {
                        ig.shadowView = CreateWhiteFrame((int)(width * 1.1f), (int)(height * 1.1f));
                    }
                    else if (style.Contains("06"))//Polaroid
                    {
                        ig.shadowView = CreateWhiteFrame((int)(width * 1.03f), height + 40, 15);
                    }
                    else if (style.Contains("03"))//Shelf shadow
                    {
                        ig.shadowView = CreateImageFrameStyle(style, width, height);
                    }
                    else if (style.Contains("07"))//Drawing
                    {
                        ig.shadowView = CreateImageFrameStyle(style, width, height);
                    }
                    else if (style.Contains("08"))//Brush
                    {
                        ig.shadowView = CreateImageFrameStyle(style, width, height);
                    }

                }
                else
                {
                    ig.isShadow = true;
                    ig.shadowView = new ImageView();
                    String path = ig.image.ResourceUrl;
                    if (path.Contains("stickers_500x500"))
                    {
                        path = path.Replace("stickers_500x500", "stickers_500x500_shadow").Replace(".png", "_shadow.png");
                    }
                    else if (path.Contains("emoji_500x500"))
                    {
                        path = "";//
                    }
                    (ig.shadowView as ImageView).ResourceUrl = path;
                }

                ig.shadowView.PositionUsesPivotPoint = true;
                ig.shadowView.PivotPoint = PivotPoint.Center;
                ig.shadowView.ParentOrigin = ParentOrigin.Center;

                if (ig.isShadow)
                {
                    ig.shadowView.Position2D = new Position2D(5, 5);
                    ig.shadowView.Size2D = new Size2D(width, height);
                }
                ig.image.Size2D = new Size2D(width, height);

                int size = 32;
                ig.borderBackground.Size2D = new Size2D(width + size + size, height + size + size);
                ig.background.Size2D = new Size2D(width + size + size + 1 + 1 + 2 + 2, height + size + size + 1 + 1 + 2 + 2);
                ig.root.Size2D = new Size2D(width + size + size + 1 + 1 + 2 + 2, height + size + size + 1 + 1 + 2 + 2);

                ig.root.Add(ig.background);
                ig.root.Add(ig.borderBackground);
                //if (ImageManager.Instance.GetDataItemType(i) == ImageManager.ItemType.PHOTO)
                {
                    ig.root.Add(ig.shadowView);
                }
                ig.root.Add(ig.image);
                mRootView.Add(ig.root);

                // hidden
                ig.background.Hide();
                ig.borderBackground.Hide();

                ig.tapDetector = new TapGestureDetector();
                ig.tapDetector.Attach(ig.root);
                ig.tapDetector.Detected += OnViewTapGestureDetected;

                ig.longPressDetector = new LongPressGestureDetector();
                ig.longPressDetector.Attach(ig.root);
                ig.longPressDetector.Detected += OnViewLongPressGestureDetected;

                ig.panDetector = new PanGestureDetector();
                ig.panDetector.Attach(ig.root);
                ig.panDetector.Detected += OnViewPanGestureDetected;

                ig.root.TouchEvent += OnViewTouchEvent;

                mImageGestureTable.Add(ig.root.Name, ig);
            }
        }

        private View CreateWhiteFrame(int width, int height, int pos = 0)
        {
            View frameView = new View();
            frameView.BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            frameView.Position2D = new Position2D(0, pos);
            frameView.Size2D = new Size2D(width, height);
            return frameView;
        }

        private ImageView CreateImageFrameStyle(string style, int width, int height)
        {
            ImageView frameView = new ImageView();
            //(ig.shadowView as ImageView).ResourceUrl = style;
            NPatchVisual patchVisual = new NPatchVisual()
            {
                URL = style,
                Border = new Rectangle(25, 25, 25, 25),
            };

            frameView.Background = patchVisual.OutputVisualMap;
            frameView.Position2D = new Position2D(0, 23);
            frameView.Size2D = new Size2D((int)(width + 50), (int)(height + 88));
            return frameView;
        }

        private void OnViewTapGestureDetected(object source, TapGestureDetector.DetectedEventArgs e)
        {
            if (mMenuRootView.Visibility)
            {
                HideMenu();
            }

            HideDeleteButton();

            if (e.TapGesture.State == Gesture.StateType.Started)
            {
                View v = e.View as View;
                if (v != null)
                {
                    if (v != mTopView)
                    {
                        v.RaiseToTop();
                        mTopView = v;
                    }
                    else
                    {
                        // open another program to view image...
                    }
                }
            }
        }

        private void OnViewPanGestureDetected(object source, PanGestureDetector.DetectedEventArgs e)
        {
            View v = e.View as View;
            if (v == null)
            {
                Log.Fatal("fb", "pan gesture detected, img is null........");
                return;
            }

            switch (e.PanGesture.State)
            {
                case Gesture.StateType.Started:
                    {
                        mLastPositionX = (int)e.PanGesture.ScreenPosition.X;
                        mLastPositionY = (int)e.PanGesture.ScreenPosition.Y;

                        //
                        HideDeleteButton();
                    }
                    break;
                case Gesture.StateType.Continuing:
                case Gesture.StateType.Finished:
                    {
                        // image must be on the top and long-pressed.
                        if (v == mTopView && v == mLongPressedView)
                        {
                            v.Position2D.X += (int)e.PanGesture.ScreenPosition.X - (int)mLastPositionX;
                            v.Position2D.Y += (int)e.PanGesture.ScreenPosition.Y - (int)mLastPositionY;
                        }

                        mLastPositionX = (int)e.PanGesture.ScreenPosition.X;
                        mLastPositionY = (int)e.PanGesture.ScreenPosition.Y;
                    }
                    break;
                default:
                    break;
            }
        }

        private void OnViewLongPressGestureDetected(object source, LongPressGestureDetector.DetectedEventArgs e)
        {
            if (mMenuRootView.Visibility)
            {
                HideMenu();
            }

            View v = e.View as View;
            if (v == null)
            {
                return;
            }

            switch (e.LongPressGesture.State)
            {
                case Gesture.StateType.Started:
                {
                    if (v != mTopView)
                    {
                        v.RaiseToTop();
                        mTopView = v;
                    }
                    mLongPressedView = v;

                    ShowSelectImageBG(mLongPressedView.Name);

                    ShowDeleteButton((int)(v.Position2D.X + v.SizeWidth / 2), (int)(v.Position2D.Y + v.SizeHeight / 2));
                }
                break;
                case Gesture.StateType.Finished:
                {
                    HideSelectImageBG(mLongPressedView.Name);
                    mLongPressedView = null;
                }
                break;
            }
        }

        private void ShowDeleteButton(int x, int y)
        {
            if (mDeleteRootView != null)
            {
                mDeleteRootView.Position2D = new Position2D(x - 164 / 2, y - 110 / 2);
                mDeleteRootView.RaiseToTop();
                mDeleteRootView.Show();
                ResetHideButtonTimer();
                return;
            }

            mDeleteRootView = new View();
            mDeleteRootView.Position2D = new Position2D(x - 164 / 2, y - 110 / 2);
            mDeleteRootView.Size2D = new Size2D(164, 110);
            NPatchVisual pv = new NPatchVisual()
            {
                URL = FB_EDIT_DELETE_BG,
                Border = new Rectangle(25, 50, 25, 58),
            };
            mDeleteRootView.Background = pv.OutputVisualMap;
            mRootView.Add(mDeleteRootView);

            mDeleteTapGestureDetector = new TapGestureDetector();
            mDeleteTapGestureDetector.Attach(mDeleteRootView);
            mDeleteTapGestureDetector.Detected += OnDeleteButtonTapGestureDetected;

            mDeleteLongPressGestureDetector = new LongPressGestureDetector();
            mDeleteLongPressGestureDetector.Attach(mDeleteRootView);
            mDeleteLongPressGestureDetector.Detected += OnDeleteButtonLongPressGestureDetected;

            mDeleteButtonView = new View();
            mDeleteButtonView.BackgroundImage = FB_EDIT_DELETE_NORMAL;
            mDeleteButtonView.Position2D = new Position2D(164 / 2 - 30 / 2, 20);
            mDeleteButtonView.Size2D = new Size2D(30, 30);
            mDeleteRootView.Add(mDeleteButtonView);

            mDeleteArrayDownView = new View();
            mDeleteArrayDownView.BackgroundImage = FB_EDIT_DELETE_ARROW_DOWN;
            mDeleteArrayDownView.Position2D = new Position2D(164 / 2 - 16 / 2, 110 - 17);
            mDeleteArrayDownView.Size2D = new Size2D(16, 17);
            mDeleteRootView.Add(mDeleteArrayDownView);

            mDeleteTextLabel = new TextLabel();
            mDeleteTextLabel.Text = "Delete";
            mDeleteTextLabel.FontFamily = "SamsungOneUI 600";
            mDeleteTextLabel.PointSize = 16;
            mDeleteTextLabel.TextColor = new Color(0.0f, 0.0f, 0.0f, 0.8f);
            mDeleteTextLabel.CellHorizontalAlignment = HorizontalAlignmentType.Center;
            mDeleteTextLabel.CellVerticalAlignment = VerticalAlignmentType.Center;
            mDeleteTextLabel.HorizontalAlignment = HorizontalAlignment.Center;
            mDeleteTextLabel.Position2D = new Position2D(14, 20 + 30 + 2);
            mDeleteTextLabel.Size2D = new Size2D(164 - 14 - 14, 23 + 7);
            mDeleteRootView.Add(mDeleteTextLabel);

            mDeleteRootView.RaiseToTop();

            mKeepButtonShowTimer = new Timer(2000);
            mKeepButtonShowTimer.Tick += OnHideButtonTimerTick;
            mKeepButtonShowTimer.Start();
        }

        private void OnDeleteButtonTapGestureDetected(object source, TapGestureDetector.DetectedEventArgs e)
        {
            HideDeleteButton();

            if (mTopView != null)
            {
                if (mTopView.Name.StartsWith("image"))
                {
                    string vname = mTopView.Name;
                    DestroyImageGesture(mImageGestureTable[vname]);
                    mImageGestureTable.Remove(vname);
                    mTopView = null;
                }
                else if (mTopView.Name.StartsWith("text"))
                {
                    string vname = mTopView.Name;
                    DestroyTextGesture(mTextGestureTable[vname]);
                    mTextGestureTable.Remove(vname);
                    mTopView = null;
                }
            }
        }

        private void DestroyImageGesture(ImageGesture ig)
        {
            if (ig.background != null)
            {
                ig.root.Remove(ig.background);
                ig.background.Dispose();
                ig.background = null;
            }

            if (ig.borderBackground != null)
            {
                ig.root.Remove(ig.borderBackground);
                ig.borderBackground.Dispose();
                ig.borderBackground = null;
            }

            if (ig.shadowView != null)
            {
                ig.root.Remove(ig.shadowView);
                ig.shadowView.Dispose();
                ig.shadowView = null;
            }

            if (ig.image != null)
            {
                ig.root.Remove(ig.image);
                ig.image.Dispose();
                ig.image = null;
            }

            if (ig.tapDetector != null)
            {
                ig.tapDetector.Detach(ig.root);
                ig.tapDetector.Dispose();
                ig.tapDetector = null;
            }

            if (ig.longPressDetector != null)
            {
                ig.longPressDetector.Detach(ig.root);
                ig.longPressDetector.Dispose();
                ig.longPressDetector = null;
            }

            if (ig.panDetector != null)
            {
                ig.panDetector.Detach(ig.root);
                ig.panDetector.Dispose();
                ig.panDetector = null;
            }

            if (ig.root != null)
            {
                mRootView.Remove(ig.root);
                ig.root.Dispose();
                ig.root = null;
            }
        }

        private void DestroyTextGesture(TextGesture tg)
        {
            if (tg.background != null)
            {
                tg.root.Remove(tg.background);
                tg.background.Dispose();
                tg.background = null;
            }

            if (tg.borderBackground != null)
            {
                tg.root.Remove(tg.borderBackground);
                tg.borderBackground.Dispose();
                tg.borderBackground = null;
            }

            if (tg.text != null)
            {
                tg.root.Remove(tg.text);
                tg.text.Dispose();
                tg.text = null;
            }

            if (tg.tapDetector != null)
            {
                tg.tapDetector.Detach(tg.root);
                tg.tapDetector.Dispose();
                tg.tapDetector = null;
            }

            if (tg.longPressDetector != null)
            {
                tg.longPressDetector.Detach(tg.root);
                tg.longPressDetector.Dispose();
                tg.longPressDetector = null;
            }

            if (tg.panDetector != null)
            {
                tg.panDetector.Detach(tg.root);
                tg.panDetector.Dispose();
                tg.panDetector = null;
            }

            if (tg.root != null)
            {
                mRootView.Remove(tg.root);
                tg.root.Dispose();
                tg.root = null;
            }
        }

        private void OnDeleteButtonLongPressGestureDetected(object source, LongPressGestureDetector.DetectedEventArgs e)
        {
            if (e.LongPressGesture.State == Gesture.StateType.Started)
            {
                mDeleteButtonView.BackgroundImage = FB_EDIT_DELETE_PRESSED;
                mKeepButtonShowTimer.Stop();
            }
            else if (e.LongPressGesture.State == Gesture.StateType.Finished)
            {
                mDeleteButtonView.BackgroundImage = FB_EDIT_DELETE_NORMAL;
                mKeepButtonShowTimer.Start();
            }
        }

        private void ResetHideButtonTimer()
        {
            mKeepButtonShowTimer.Stop();
            mKeepButtonShowTimer.Start();
        }

        private bool OnHideButtonTimerTick(object source, Timer.TickEventArgs e)
        {
            HideDeleteButton();
            return false;
        }

        private void HideDeleteButton()
        {
            if (mDeleteRootView != null)
            {
                mDeleteRootView.Hide();
            }
        }

        private void ShowSelectImageBG(string name)
        {
            if (name.StartsWith("image"))
            {
                mImageGestureTable[name].background.Show();
                mImageGestureTable[name].borderBackground.Show();
            }
            else
            {
                int cnt = mTextGestureTable[name].text.LineCount;
                int width = (int)mTextGestureTable[name].text.NaturalSize2D.Width;
                int height = cnt * 200;

                mTextGestureTable[name].borderBackground.Size2D = new Size2D(width + 8 + 8, height + 8 + 8);
                mTextGestureTable[name].background.Size2D = new Size2D(width + 8 + 8 + 1 + 1 + 2 + 2, height + 8 + 8 + 1 + 1 + 2 + 2);

                mTextGestureTable[name].background.Show();
                mTextGestureTable[name].borderBackground.Show();
            }
        }

        private void HideSelectImageBG(string name)
        {
            if (name.StartsWith("image"))
            {
                mImageGestureTable[name].background.Hide();
                mImageGestureTable[name].borderBackground.Hide();
            }
            else
            {
                mTextGestureTable[name].background.Hide();
                mTextGestureTable[name].borderBackground.Hide();
            }
        }

        private bool OnViewTouchEvent(object source, View.TouchEventArgs e)
        {
            View v = source as View;
            if (v == null)
            {
                Log.Fatal("fb", "view touched, but view is null........");
                return false;
            }

            Touch touch = e.Touch;
            if (touch.GetState(0) == PointStateType.Up)
            {
                HideSelectImageBG(v.Name);
            }

            if (touch.GetHitView(0) != touch.GetHitView(1))
            {
                Log.Fatal("fb", "view touched, only one view could be pinched........");
                return false;
            }

            if (touch.GetPointCount() == 2)
            {
                if (touch.GetState(1) == PointStateType.Down)
                {
                    if (v != mTopView)
                    {
                        v.RaiseToTop();
                        mTopView = v;
                    }
                    ShowSelectImageBG(v.Name);
                }

                Vector2 p1 = touch.GetScreenPosition(0);
                Vector2 p2 = touch.GetScreenPosition(1);

                float radian = (float)Math.Atan2(p1.Y - p2.Y, p1.X - p2.X);
                float distance = (p1 - p2).Length();

                if (!(touch.GetState(0) == PointStateType.Started || touch.GetState(1) == PointStateType.Started))
                {
                    Radian r = new Radian(radian - mLastRadian);
                    v.RotateBy(r, new Vector3(0, 0, 1));

                    float s = distance / mLastDistance;
                    v.ScaleBy(new Vector3(s, s, 1));

                    RotateShadowBG(v.Name);
                }

                mLastRadian = radian;
                mLastDistance = distance;
            }

            return true;
        }

        private void RotateShadowBG(string name)
        {
            if ((name.StartsWith("image") || name.StartsWith("sticker")) && mImageGestureTable[name].shadowView != null && mImageGestureTable[name].isShadow)
            {
                Tizen.Log.Error("PhotoSlide", "rotate");
                Rotation rotation = mImageGestureTable[name].root.Orientation;
                Radian angle = new Radian();
                rotation.GetAxisAngle(new Vector3(), angle);
                Position2D rotatePoint = RotatePosition(new Position2D(5, 5), angle.Value);
                mImageGestureTable[name].shadowView.Position2D = rotatePoint;
            }
        }

        private Position2D RotatePosition(Position2D sourcePoint, double radian)
        {
            Position2D centerPoint = new Position2D(0, 0);
            Position2D targetPoint = new Position2D();
            float x = (int)(Math.Cos(radian) * (sourcePoint.X - centerPoint.X) - Math.Sin(radian) * (sourcePoint.Y - centerPoint.Y) + centerPoint.X);
            float y = (int)(Math.Sin(radian) * (sourcePoint.X - centerPoint.X) + Math.Cos(radian) * (sourcePoint.Y - centerPoint.Y) + centerPoint.Y);

            targetPoint.X = (int)x;
            targetPoint.Y = (int)y;

            return targetPoint;
        }

        private void ShowEditedText()
        {
            if (mTextGestureTable.Count + 1 >= MAX_TEXT_COUNT)
                return;

            if (ImageManager.Instance.GetContent() != null && ImageManager.Instance.GetContent().Length > 0)
            {
                TextGesture tg = new TextGesture();

                tg.root = new View();
                tg.root.Name = $"text{mCurrentTextsCount++}";
                tg.root.Position2D = new Position2D((SCREEN_WIDTH - 400) / 2, (SCREEN_HEIGHT - 200) / 2);

                tg.background = new View();
                tg.background.BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.15f);
                tg.background.PositionUsesPivotPoint = true;
                tg.background.PivotPoint = PivotPoint.Center;
                tg.background.ParentOrigin = ParentOrigin.Center;

                tg.borderBackground = new VisualView();
                tg.border = new BorderVisual()
                {
                    BorderSize = 1.0f,
                    Color = Color.White,
                    AntiAliasing = true,
                };
                tg.borderBackground.AddVisual("border", tg.border);
                tg.borderBackground.BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.45f);
                tg.borderBackground.PositionUsesPivotPoint = true;
                tg.borderBackground.PivotPoint = PivotPoint.Center;
                tg.borderBackground.ParentOrigin = ParentOrigin.Center;

                tg.text = new TextLabel();
                tg.text.Text = ImageManager.Instance.GetContent();
                tg.text.TextColor = ImageManager.Instance.GetTextColor();
                tg.text.FontFamily = ImageManager.Instance.GetTextStyle();
                tg.text.PointSize = 100;
                tg.text.VerticalAlignment = VerticalAlignment.Top;
                tg.text.HorizontalAlignment = ImageManager.Instance.GetHorizontalAlignment();
                tg.text.PositionUsesPivotPoint = true;
                tg.text.PivotPoint = PivotPoint.Center;
                tg.text.ParentOrigin = ParentOrigin.Center;


                tg.text.MultiLine = true;
                tg.text.Ellipsis = false;
                //tg.text.SizeWidth = 1000.0f;
                tg.text.MaximumSize = new Size2D(1000, 2000);
                tg.text.WidthResizePolicy = ResizePolicyType.UseNaturalSize;
                tg.text.HeightResizePolicy = ResizePolicyType.DimensionDependency;
                
                int width = (int)tg.text.NaturalSize2D.Width;
                int height = (int)tg.text.NaturalSize2D.Height;
                
                tg.borderBackground.Size2D = new Size2D(width + 8 + 8, height + 8 + 8);
                tg.background.Size2D = new Size2D(width + 8 + 8 + 1 + 1 + 2 + 2, height + 8 + 8 + 1 + 1 + 2 + 2);
                tg.root.Size2D = new Size2D(width + 8 + 8 + 1 + 1 + 2 + 2, height + 8 + 8 + 1 + 1 + 2 + 2);

                tg.root.Add(tg.background);
                tg.root.Add(tg.borderBackground);
                tg.root.Add(tg.text);
                mRootView.Add(tg.root);

                // hidden
                tg.background.Hide();
                tg.borderBackground.Hide();

                tg.tapDetector = new TapGestureDetector();
                tg.tapDetector.Attach(tg.root);
                tg.tapDetector.Detected += OnViewTapGestureDetected;

                tg.longPressDetector = new LongPressGestureDetector();
                tg.longPressDetector.Attach(tg.root);
                tg.longPressDetector.Detected += OnViewLongPressGestureDetected;

                tg.panDetector = new PanGestureDetector();
                tg.panDetector.Attach(tg.root);
                tg.panDetector.Detected += OnViewPanGestureDetected;

                tg.root.TouchEvent += OnViewTouchEvent;

                mTextGestureTable.Add(tg.root.Name, tg);
            }
        }

        private void CreateDateTime()
        {
            // time
            mTimeView = new View();
            mTimeView.Position2D = new Position2D(48, 33 + 30);
            mTimeView.SizeHeight = 28 + 58 + 22;
            mTimeView.SizeWidth = 226 + 300;
            //mRootView.Add(mTimeView);

            mTimeLabel = new TextLabel();
            mTimeLabel.Text = "09:30";
            mTimeLabel.FontFamily = "SamsungOneUI 500";
            mTimeLabel.PointSize = 71;
            mTimeLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.7f);
            mTimeLabel.Position2D = new Position2D(0, 0);
            mTimeLabel.SizeHeight = 58 + 100;
            mTimeLabel.SizeWidth = mTimeLabel.GetWidthForHeight(mTimeLabel.SizeHeight);
            mTimeLabel.PositionUsesPivotPoint = true;
            mTimeLabel.PivotPoint = PivotPoint.CenterLeft;
            mTimeLabel.ParentOrigin = ParentOrigin.CenterLeft;
            mTimeView.Add(mTimeLabel);

            // am, pm
            mAmPmView = new View();
            mAmPmView.Position2D = new Position2D(48 + (int)mTimeLabel.SizeWidth + 11 - 60, 95 - 20);
            mAmPmView.SizeHeight = 9 + 18 + 7 + 40;
            mAmPmView.SizeWidth = 226;
            //mRootView.Add(mAmPmView);

            mAmPmLabel = new TextLabel();
            mAmPmLabel.Text = "AM";
            mAmPmLabel.FontFamily = "SamsungOneUI 500";
            mAmPmLabel.PointSize = 22;
            mAmPmLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.7f);
            //mAmPmLabel.Position2D = new Position2D(0, 0);
            //mAmPmLabel.SizeHeight = 18;
            //mAmPmLabel.SizeWidth = mAmPmLabel.GetWidthForHeight(mAmPmLabel.SizeHeight);
            mAmPmLabel.PositionUsesPivotPoint = true;
            mAmPmLabel.PivotPoint = PivotPoint.CenterRight;
            mAmPmLabel.ParentOrigin = ParentOrigin.CenterRight;
            mAmPmView.Add(mAmPmLabel);

            // date
            mDateView = new View();
            mDateView.Position2D = new Position2D(48, 134 + 22);
            mDateView.SizeHeight = 9 + 18 + 7;
            mDateView.SizeWidth = 490;
            //mRootView.Add(mDateView);

            mDateLabel = new TextLabel();
            mDateLabel.Text = "Wednesday, June 21";
            mDateLabel.FontFamily = "SamsungOneUI 500";
            mDateLabel.PointSize = 22;
            mDateLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.7f);
            //mDateLabel.Position2D = new Position2D(0, 9);
            //mDateLabel.SizeHeight = 18;
            //mDateLabel.SizeWidth = mDateLabel.GetWidthForHeight(mDateLabel.SizeHeight);
            mDateLabel.PositionUsesPivotPoint = true;
            mDateLabel.PivotPoint = PivotPoint.CenterRight;
            mDateLabel.ParentOrigin = ParentOrigin.CenterRight;

            mDateView.Add(mDateLabel);
        }

        private void DestroyDateTime()
        {
            if (mTimeLabel != null)
            {
                mTimeView.Remove(mTimeLabel);
                mTimeLabel.Dispose();
                mTimeLabel = null;
            }

            if (mTimeView != null)
            {
                mRootView.Remove(mTimeView);
                mTimeView.Dispose();
                mTimeView = null;
            }

            if (mAmPmLabel != null)
            {
                mAmPmView.Remove(mAmPmLabel);
                mAmPmLabel.Dispose();
                mAmPmLabel = null;
            }

            if (mAmPmView != null)
            {
                mRootView.Remove(mAmPmView);
                mAmPmView.Dispose();
                mAmPmView = null;
            }

            if (mDateLabel != null)
            {
                mDateView.Remove(mDateLabel);
                mDateLabel.Dispose();
                mDateLabel = null;
            }

            if (mDateView != null)
            {
                mRootView.Remove(mDateView);
                mDateView.Dispose();
                mDateView = null;
            }
        }

        private void CreateMenu(int x, int y)
        {
            mMenuRootView = new View();
            mMenuRootView.Position2D = new Position2D(x, y);
            mMenuRootView.Size2D = new Size2D(MENU_WIDTH, MENU_HEIGHT);
            mRootView.Add(mMenuRootView);

            mNPatchVisual = new NPatchVisual()
            {
                URL = FB_MENU_BACKGROUND_IMAGE,
                Border = new Rectangle(80, 80, 120, 50),
            };
            mMenuBackgroundView = new View();
            mMenuBackgroundView.Background = mNPatchVisual.OutputVisualMap;
            mMenuBackgroundView.PositionUsesPivotPoint = true;
            mMenuBackgroundView.Position2D = new Position2D(0, 0);
            mMenuBackgroundView.Size2D = new Size2D(MENU_WIDTH, MENU_HEIGHT);
            mMenuRootView.Add(mMenuBackgroundView);

            mMenuList = new FlexibleView();
            mMenuList.Name = "Menu List";
            mMenuList.Position2D = new Position2D(30, 5);
            mMenuList.Size2D = new Size2D(210, 6 * 50 + 8 + 56);
            mMenuList.Padding = new Extents(0, 0, 0, 0);
            mMenuList.ItemClicked += OnListItemClickEvent;
            mMenuRootView.Add(mMenuList);

            List<MenuListItemData> dataList = new List<MenuListItemData>();
            dataList.Add(new MenuListItemData(FB_MENU_PICTURE_IMAGE, "picture"));
            dataList.Add(new MenuListItemData(FB_MENU_MEMO_IMAGE, "memo"));
            dataList.Add(new MenuListItemData(FB_MENU_STICKERS_IMAGE, "stickers"));
            dataList.Add(new MenuListItemData(FB_MENU_DRAWING_IMAGE, "drawing"));
            dataList.Add(new MenuListItemData(FB_MENU_TEXT_IMAGE, "text"));
            dataList.Add(new MenuListItemData(FB_MENU_MUSIC_IMAGE, "music"));
            dataList.Add(new MenuListItemData("", ""));

            mMenuListBridge = new MenuListBridge(this, dataList);
            mMenuList.SetAdapter(mMenuListBridge);

            LinearLayoutManager layoutManager = new LinearLayoutManager(LinearLayoutManager.VERTICAL);
            mMenuList.SetLayoutManager(layoutManager);
        }

        private void DestroyMenu()
        {
            if (mMenuList != null)
            {
                mMenuRootView.Remove(mMenuList);
                mMenuList.Dispose();
                mMenuList = null;
            }

            if (mMenuBackgroundView != null)
            {
                mMenuRootView.Remove(mMenuBackgroundView);
                mMenuBackgroundView.Dispose();
                mMenuBackgroundView = null;
            }

            if (mMenuRootView != null)
            {
                mRootView.Remove(mMenuRootView);
                mMenuRootView.Dispose();
                mMenuRootView = null;
            }
        }

        private void OnListItemClickEvent(object sender, FlexibleViewItemClickedEventArgs e)
        {
            if (e.ClickedView != null)
            {
                //
                ResetHideMenuTimer();

                int index = e.ClickedView.AdapterPosition;
                switch (index)
                {
                    case 0:
                        {
                            HideMenu();
                            mRootViewTapGestureDetector.Detach(mRootView);
                            FamilyBoardPage.Instance.CreateView("Picture Wizard");
                            break;
                        }
                    case 2:
                        {
                            HideMenu();
                            mRootViewTapGestureDetector.Detach(mRootView);
                            FamilyBoardPage.Instance.CreateView("Stickers");
                            break;
                        }
                    case 4:
                        {
                            HideMenu();
                            mRootViewTapGestureDetector.Detach(mRootView);
                            FamilyBoardPage.Instance.CreateView("Text");
                            break;
                        }
                    default:
                        break;
                }
            }
        }
    }

    internal class MenuListItemData
    {
        public MenuListItemData(string icon, string text)
        {
            Icon = icon;
            Text = text;
        }

        public string Icon
        {
            set;
            get;
        }

        public string Text
        {
            set;
            get;
        }
    }

    internal class MenuListItemView : View
    {
        private View mRootView;
        private ImageView mIconView;
        private View mTextView;
        private TextLabel mTextLabel;

        public MenuListItemView(string icon, string text)
        {
            mRootView = new View();
            mRootView.Size2D = new Size2D(210, 50);

            mIconView = new ImageView();
            mIconView.Position2D = new Position2D(21, 0);
            mIconView.Size2D = new Size2D(30, 30);
            mIconView.ResourceUrl = icon;
            mIconView.PositionUsesPivotPoint = true;
            mIconView.ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft;
            mIconView.PivotPoint = Tizen.NUI.PivotPoint.CenterLeft;
            mRootView.Add(mIconView);

            mTextView = new View();
            mTextView.Position2D = new Position2D(21 + 30 + 15, 0);
            mTextView.Size2D = new Size2D(210 - 30 - 15, 50);
            mTextView.PositionUsesPivotPoint = true;
            mTextView.ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft;
            mTextView.PivotPoint = Tizen.NUI.PivotPoint.CenterLeft;
            mRootView.Add(mTextView);

            mTextLabel = new TextLabel();
            mTextLabel.Text = text;
            mTextLabel.FontFamily = "SamsungOneUI 600";
            mTextLabel.PointSize = 16;
            mTextLabel.TextColor = new Color(0.0f, 0.0f, 0.0f, 0.8f);
            mTextLabel.PositionUsesPivotPoint = true;
            mTextLabel.ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft;
            mTextLabel.PivotPoint = Tizen.NUI.PivotPoint.CenterLeft;
            mTextView.Add(mTextLabel);

            if(text=="memo" || text == "drawing" || text == "music")
            {
                mIconView.Color = new Color(0.6f, 0.6f, 0.6f, 0.7f);
                mTextLabel.TextColor = new Color(0.6f, 0.6f, 0.6f, 0.7f);
            }

            Add(mRootView);
        }
    }

    internal class MenuButtonsItemView : View
    {
        private View mRootView;
        private TableView mIconsView;
        private ImageView mAddImageView;
        private ImageView mOpenImageView;
        private ImageView mMoreImageView;

        //
        private FamilyBoardMain mFamilyBoardMain;

        private static string FB_MENU_IMAGE = CommonResource.GetResourcePath() + "add_menu/";
        private static string FB_MENU_NEW_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_new.png";
        private static string FB_MENU_OPEN_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_open.png";
        private static string FB_MENU_MORE_IMAGE = FB_MENU_IMAGE + "fb_menu_ic_more.png";

        public MenuButtonsItemView(FamilyBoardMain main)
        {
            mFamilyBoardMain = main;

            mRootView = new View();
            mRootView.Size2D = new Size2D(210, 8 + 56);

            mIconsView = new TableView(1, 3);
            mIconsView.Position2D = new Position2D(31, 8);
            mIconsView.Size2D = new Size2D(210, 56);
            mIconsView.PositionUsesPivotPoint = true;
            mIconsView.ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft;
            mIconsView.PivotPoint = Tizen.NUI.PivotPoint.CenterLeft;
            mRootView.Add(mIconsView);

            mAddImageView = new ImageView();
            mAddImageView.ResourceUrl = FB_MENU_NEW_IMAGE;
            mIconsView.SetCellAlignment(new TableView.CellPosition(0, 0), HorizontalAlignmentType.Center, VerticalAlignmentType.Center);
            mIconsView.AddChild(mAddImageView, new TableView.CellPosition(0, 0));

            mOpenImageView = new ImageView();
            mOpenImageView.ResourceUrl = FB_MENU_OPEN_IMAGE;
            mIconsView.SetCellAlignment(new TableView.CellPosition(0, 1), HorizontalAlignmentType.Center, VerticalAlignmentType.Center);
            mIconsView.AddChild(mOpenImageView, new TableView.CellPosition(0, 1));

            mMoreImageView = new ImageView();
            mMoreImageView.ResourceUrl = FB_MENU_MORE_IMAGE;
            mIconsView.SetCellAlignment(new TableView.CellPosition(0, 2), HorizontalAlignmentType.Center, VerticalAlignmentType.Center);
            mIconsView.AddChild(mMoreImageView, new TableView.CellPosition(0, 2));

            mMoreImageView.TouchEvent += OnMoreItemTouchEvent;

            Add(mRootView);
        }

        private bool OnMoreItemTouchEvent(object source, View.TouchEventArgs e)
        {
            mFamilyBoardMain.HideMenu();
            mFamilyBoardMain.DetachTapGesture();
            FamilyBoardPage.Instance.CreateView("Background Image");

            return false;
        }

        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                if (mAddImageView != null)
                {
                    mIconsView.Remove(mAddImageView);
                    mAddImageView.Dispose();
                    mAddImageView = null;
                }

                if (mOpenImageView != null)
                {
                    mIconsView.Remove(mOpenImageView);
                    mOpenImageView.Dispose();
                    mOpenImageView = null;
                }

                if (mMoreImageView != null)
                {
                    mIconsView.Remove(mMoreImageView);
                    mMoreImageView.Dispose();
                    mMoreImageView = null;
                }

                if (mIconsView != null)
                {
                    mRootView.Remove(mIconsView);
                    mIconsView.Dispose();
                    mIconsView = null;
                }

                if (mRootView != null)
                {
                    Remove(mRootView);
                    mRootView = null;
                }
            }
            base.Dispose(type);
        }
    }

    internal class MenuListBridge : FlexibleViewAdapter
    {
        private List<MenuListItemData> mDatas;

        private FamilyBoardMain mFamilyBoardMain;

        public MenuListBridge(FamilyBoardMain main, List<MenuListItemData> datas)
        {
            mFamilyBoardMain = main;
            mDatas = datas;
        }

        public override int GetItemViewType(int position)
        {
            return position;  // position is used as item view type.
        }

        public override FlexibleViewViewHolder OnCreateViewHolder(int viewType)
        {
            View item_view = null;
            MenuListItemData listItemData = mDatas[viewType];

            switch (viewType)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    {
                        item_view = new MenuListItemView(listItemData.Icon, listItemData.Text);
                        break;
                    }
                case 6:
                    {
                        item_view = new MenuButtonsItemView(mFamilyBoardMain);
                        break;
                    }
                default:
                    break;
            }

            FlexibleViewViewHolder viewHolder = new FlexibleViewViewHolder(item_view);

            return viewHolder;
        }

        public override void OnBindViewHolder(FlexibleViewViewHolder holder, int position)
        {
            MenuListItemData listItemData = mDatas[position];
            switch (position)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                    {
                        MenuListItemView listItemView = holder.ItemView as MenuListItemView;
                        listItemView.Name = "Item" + position;
                        listItemView.Size2D = new Size2D(210, 50);
                        listItemView.Margin = new Extents(0, 0, 0, 0);
                        break;
                    }
                case 6:
                    {
                        MenuButtonsItemView listItemView = holder.ItemView as MenuButtonsItemView;
                        listItemView.Name = "Item" + position;
                        listItemView.Size2D = new Size2D(210, 8 + 56);
                        listItemView.Margin = new Extents(0, 0, 0, 0);
                        break;
                    }
                default:
                    break;
            }
        }

        public override void OnDestroyViewHolder(FlexibleViewViewHolder holder)
        {
            if (holder.ItemView != null)
            {
                holder.ItemView.Dispose();
            }
        }

        public override int GetItemCount()
        {
            return mDatas.Count;
        }
    }
}

