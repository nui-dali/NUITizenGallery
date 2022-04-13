using System;
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.FH.FamilyBoard
{
    public class FrameStyleChooser : ILifecycleObserver
    {
        private View mRootView;

        // close
        private View mCloseView;
        private ImageView mCloseImageView;
        private TapGestureDetector mCloseTapGestureDetector;

        // back, title, done
        private View mTitleRootView;
        private View mTitleView;
        private TextLabel mTitleLabel;
        private Button mBackButton;
        private Button mDoneButton;

        // preview
        private View mPreviewRootView;

        private struct ImagePreview
        {
            public View view;
            public ImageView image;
            public Size2D size;
            public ImageView framestyle;
        }
        ImagePreview[] mPreviewedImages;

        private int mCurrentPreviewIndex;
        private PanGestureDetector mPreviewPanGestureDetector;
        private int mLastPreviewPositionX;
        private Animation mMoveAnimation;

        // switch
        private View mSliderShowView;
        private TextLabel mSliderShowTextLabel;
        private Switch mSliderShowSwitch;

        // frame style grid
        private View mFrameStyleRootView;
        private FlexibleView mFrameStyleGrid;
        private FrameStyleGridBridge mFrameStyleBridge;
        private FlexibleViewViewHolder mLastCheckedFrameStyle;

        //
        private Rectangle[] mFrameBorders = {
                                        new Rectangle(25, 25, 25, 25),
                                        new Rectangle(25, 25, 25, 25),
                                        new Rectangle(25, 25, 25, 25),
                                        new Rectangle(25, 25, 25, 25),
                                        new Rectangle(25, 25, 25, 25),
                                        new Rectangle(25, 25, 25, 25),
                                        new Rectangle(25, 25, 25, 25),
                                        new Rectangle(25, 25, 25, 25)};

        private readonly int SCREEN_WIDTH = 1080;
        private readonly int SCREEN_HEIGHT = 1920;

        //
        private static string FB_PICTURE_DIR = CommonResource.GetResourcePath() + "picker/";
        private static string FB_PICTURE_CLOSE_IMAGE = FB_PICTURE_DIR + "familyboard_ic_delete.png";
        private static string FB_PICTURE_BACK_BUTTON_IMAGE = FB_PICTURE_DIR + "familyboard_gallery_ic_back.png";
        private static string FB_PICTURE_DONE_BUTTON_IMAGE = FB_PICTURE_DIR + "familyboard_photo_drawer_btn.png";
        private static string FB_PICTURE_FRAME_PICTURE_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_picture.png";
        private static string FB_PICTURE_FRAME_FRAME1_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_01.png";
        private static string FB_PICTURE_FRAME_FRAME2_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_02.png";
        private static string FB_PICTURE_FRAME_FRAME3_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_03.png";
        private static string FB_PICTURE_FRAME_FRAME4_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_04.png";
        private static string FB_PICTURE_FRAME_FRAME5_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_05.png";
        private static string FB_PICTURE_FRAME_FRAME6_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_06.png";
        private static string FB_PICTURE_FRAME_FRAME7_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_07.png";
        private static string FB_PICTURE_FRAME_FRAME8_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_08.png";

        public void Activate()
        {
            mRootView = new View();
            mRootView.BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);
            mRootView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT);
            mRootView.ClippingMode = ClippingModeType.ClipToBoundingBox;
            NUIApplication.GetDefaultWindow().Add(mRootView);

            CreateCloseButton();

            // picture
            CreateBackTitleNext();

            CreatePreview();

            CreateFrameStyleGrid();

            ChooseFrameStyle(0, FB_PICTURE_FRAME_FRAME1_IMAGE);
        }

        public void Reactivate()
        {
            DestroyPreview();
            CreatePreview();
        }

        public void Deactivate()
        {
            if (mFrameStyleGrid != null)
            {
                mFrameStyleRootView.Remove(mFrameStyleGrid);
                mFrameStyleGrid.Dispose();
                mFrameStyleGrid = null;
            }

            if (mFrameStyleRootView != null)
            {
                mRootView.Remove(mFrameStyleRootView);
                mFrameStyleRootView.Dispose();
                mFrameStyleRootView = null;
            }

            if (mTitleLabel != null)
            {
                mTitleView.Remove(mTitleLabel);
                mTitleLabel.Dispose();
                mTitleLabel = null;
            }

            if (mTitleView != null)
            {
                mTitleRootView.Remove(mTitleView);
                mTitleView.Dispose();
                mTitleView = null;
            }

            if (mDoneButton != null)
            {
                mTitleRootView.Remove(mDoneButton);
                mDoneButton.Dispose();
                mDoneButton = null;
            }

            if (mBackButton != null)
            {
                mTitleRootView.Remove(mBackButton);
                mBackButton.Dispose();
                mBackButton = null;
            }

            if (mTitleRootView != null)
            {
                mRootView.Remove(mTitleRootView);
                mTitleRootView.Dispose();
                mTitleRootView = null;
            }

            if (mCloseTapGestureDetector != null)
            {
                mCloseTapGestureDetector.Detected -= OnCloseTapGestureDetected;
                mCloseTapGestureDetector.Detach(mCloseView);
                mCloseTapGestureDetector.Dispose();
                mCloseTapGestureDetector = null;
            }

            if (mCloseImageView != null)
            {
                mCloseView.Remove(mCloseImageView);
                mCloseImageView.Dispose();
                mCloseImageView = null;
            }

            if (mCloseView != null)
            {
                mRootView.Remove(mCloseView);
                mCloseView.Dispose();
                mCloseView = null;
            }

            if (mRootView != null)
            {
                NUIApplication.GetDefaultWindow().Remove(mRootView);
                mRootView.Dispose();
                mRootView = null;
            }
        }

        public View GetRootView()
        {
            return mRootView;
        }

        public void ChooseFrameStyle(int position, string style)
        {
            int imageCount = ImageManager.Instance.ImageCount();
            for (int i = 0; i < imageCount; i++)
            {
                NPatchVisual patchVisual = new NPatchVisual()
                {
                    URL = style,
                    Border = mFrameBorders[position],
                };
                mPreviewedImages[i].framestyle.Background = patchVisual.OutputVisualMap;

                int width = mPreviewedImages[i].size.Width + mFrameBorders[position].X + mFrameBorders[position].Width;
                int height = mPreviewedImages[i].size.Height + mFrameBorders[position].Y + mFrameBorders[position].Height;

                if(width < height)
                {
                    mPreviewedImages[i].framestyle.Size2D = new Size2D(width + 15, height + 65);
                    mPreviewedImages[i].framestyle.Position2D = new Position2D(2, 40);
                }
                else
                {
                    mPreviewedImages[i].framestyle.Size2D = new Size2D(width + 20, height + 45);
                    mPreviewedImages[i].framestyle.Position2D = new Position2D(0, 27);
                }

                mPreviewedImages[i].framestyle.Show();
                ImageManager.Instance.SetFrameStyle(i, style);
            }
        }

        private void CreateCloseButton()
        {
            // close
            mCloseView = new View();
            mCloseView.Position2D = new Position2D(0, 0);
            mCloseView.Size2D = new Size2D(SCREEN_WIDTH, 405);
            mRootView.Add(mCloseView);

            mCloseImageView = new ImageView();
            mCloseImageView.ResourceUrl = FB_PICTURE_CLOSE_IMAGE;
            mCloseImageView.Position2D = new Position2D(SCREEN_WIDTH - 40 - 41, 405 - 22 - 40);
            mCloseImageView.Size2D = new Size2D(40, 40);
            mCloseView.Add(mCloseImageView);

            mCloseTapGestureDetector = new TapGestureDetector();
            mCloseTapGestureDetector.Attach(mCloseView);
            mCloseTapGestureDetector.Detected += OnCloseTapGestureDetected;
        }

        private void OnCloseTapGestureDetected(object source, TapGestureDetector.DetectedEventArgs e)
        {
            ImageManager.Instance.RemoveAll();
            FamilyBoardPage.Instance.RemoveView();
        }

        private void CreateBackTitleNext()
        {
            mTitleRootView = new View();
            mTitleRootView.Position2D = new Position2D(0, 405);
            mTitleRootView.Size2D = new Size2D(SCREEN_WIDTH, 164);
            mTitleRootView.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            mRootView.Add(mTitleRootView);

            // back button
            mBackButton = new Button();
            mBackButton.BackgroundImage = FB_PICTURE_BACK_BUTTON_IMAGE;
            mBackButton.Position2D = new Position2D(24, 77);
            mBackButton.Size2D = new Size2D(46, 46);
            mTitleRootView.Add(mBackButton);

            mBackButton.ClickEvent += OnBackButtonClickEvent;

            // title
            mTitleView = new View();
            mTitleView.Position2D = new Position2D();
            mTitleView.Size2D = new Size2D(SCREEN_WIDTH, 177);
            mTitleRootView.Add(mTitleView);

            mTitleLabel = new TextLabel();
            mTitleLabel.Text = "Choose Frame Style";
            mTitleLabel.FontFamily = "SamsungOneUI 600";
            mTitleLabel.PointSize = 30;
            mTitleLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            mTitleLabel.PositionUsesPivotPoint = true;
            mTitleLabel.PivotPoint = PivotPoint.Center;
            mTitleLabel.ParentOrigin = ParentOrigin.Center;
            mTitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            mTitleLabel.VerticalAlignment = VerticalAlignment.Center;
            mTitleView.Add(mTitleLabel);

            // done button
            mDoneButton = new Button();
            mDoneButton.BackgroundImage = FB_PICTURE_DONE_BUTTON_IMAGE;
            mDoneButton.Text = "Done";
            mDoneButton.TextColor = Color.White;
            mDoneButton.PointSize = 30;
            mDoneButton.FontFamily = "SamsungOneUI 500";
            mDoneButton.Position2D = new Position2D(SCREEN_WIDTH - 151 - 40 - 46, (177 - 70) / 2);
            mDoneButton.Size2D = new Size2D(151 + 40, 60 + 10);
            mTitleRootView.Add(mDoneButton);

            mDoneButton.ClickEvent += OnDoneButtonClickEvent;
        }

        private void OnBackButtonClickEvent(object sender, Button.ClickEventArgs e)
        {
            PictureWizard.Instance.Back();
        }

        private void OnDoneButtonClickEvent(object sender, Button.ClickEventArgs e)
        {
            FamilyBoardPage.Instance.RemoveView();
        }

        private void CreatePreview()
        {
            int imageCount = ImageManager.Instance.ImageCount();

            mPreviewRootView = new View();
            mPreviewRootView.BackgroundColor = new Color(0.1f, 0.1f, 0.1f, 1.0f);
            mPreviewRootView.Position2D = new Position2D(0, 405 + 164);
            mPreviewRootView.Size2D = new Size2D((SCREEN_WIDTH - 540) + (540 + 60) * imageCount - 60, 610);
            mRootView.Add(mPreviewRootView);

            mPreviewPanGestureDetector = new PanGestureDetector();
            mPreviewPanGestureDetector.Attach(mPreviewRootView);
            mPreviewPanGestureDetector.Detected += OnPreviewTapGestureDetected;

            mCurrentPreviewIndex = 0;

            mPreviewedImages = new ImagePreview[imageCount];
            for (int i = 0; i < imageCount; i++)
            {
                // view
                mPreviewedImages[i].view = new View();
                mPreviewedImages[i].view.Position2D = new Position2D((SCREEN_WIDTH - 540) / 2 + i * (540 + 60), (610 - 540) / 2);
                mPreviewedImages[i].view.Size2D = new Size2D(540, 540);
                mPreviewRootView.Add(mPreviewedImages[i].view);

                float opacity = 0.5f;
                if (i == mCurrentPreviewIndex)
                {
                    opacity = 1.0f;
                }

                // image
                mPreviewedImages[i].image = new ImageView();
                mPreviewedImages[i].image.Color = new Color(opacity, opacity, opacity, 1.0f); ;
                mPreviewedImages[i].image.ResourceUrl = ImageManager.Instance.GetThumbFile(i);

                Position pivot = PivotPoint.Center;
                Position origin = ParentOrigin.Center;
                int width = mPreviewedImages[i].image.NaturalSize2D.Width;
                int height = mPreviewedImages[i].image.NaturalSize2D.Height;
                if (width < height)
                {
                    width = 540 * width / height;
                    height = 540;
                    //pivot = PivotPoint.TopCenter;
                    //origin = ParentOrigin.TopCenter;
                }
                else
                {
                    width = 540;
                    height = 540 * height / width;
                    //pivot = PivotPoint.CenterLeft;
                    //origin = ParentOrigin.CenterLeft;
                }

                mPreviewedImages[i].size = new Size2D(width, height);

                mPreviewedImages[i].image.Size2D = mPreviewedImages[i].size;
                mPreviewedImages[i].image.PositionUsesPivotPoint = true;
                mPreviewedImages[i].image.PivotPoint = pivot;
                mPreviewedImages[i].image.ParentOrigin = origin;

                // frame style
                mPreviewedImages[i].framestyle = new ImageView();
                mPreviewedImages[i].framestyle.Color = new Color(opacity, opacity, opacity, 1.0f);
                mPreviewedImages[i].framestyle.Size2D = mPreviewedImages[i].size;
                mPreviewedImages[i].framestyle.PositionUsesPivotPoint = true;
                mPreviewedImages[i].framestyle.PivotPoint = pivot;
                mPreviewedImages[i].framestyle.ParentOrigin = origin;
                mPreviewedImages[i].view.Add(mPreviewedImages[i].framestyle);
                mPreviewedImages[i].framestyle.Hide();

                //
                mPreviewedImages[i].view.Add(mPreviewedImages[i].image);
            }
        }

        private void OnPreviewTapGestureDetected(object source, PanGestureDetector.DetectedEventArgs e)
        {
            int imageCount = ImageManager.Instance.ImageCount();

            switch (e.PanGesture.State)
            {
                case Gesture.StateType.Started:
                    {
                        mLastPreviewPositionX = (int)mPreviewRootView.PositionX;
                        if (mMoveAnimation != null && mMoveAnimation.State == Animation.States.Playing)
                        {
                            mMoveAnimation.Stop(Animation.EndActions.StopFinal);
                        }
                        break;
                    }
                case Gesture.StateType.Continuing:
                    {
                        int deltaX = (int)e.PanGesture.Displacement.X;
                        if (deltaX > 0) // move from left to right
                        {
                            if (mCurrentPreviewIndex <= 0)
                            {
                                return;
                            }
                        }
                        else
                        {
                            if (mCurrentPreviewIndex >= imageCount - 1)
                            {
                                return;
                            }
                        }

                        mPreviewRootView.PositionX += deltaX;
                        break;
                    }
                case Gesture.StateType.Finished:
                    {
                        mPreviewRootView.PositionX += (int)e.PanGesture.Displacement.X;
                        float deltaX = mPreviewRootView.PositionX - mLastPreviewPositionX;
                        if (Math.Abs(deltaX) < 300 / 2)
                        {
                            if (mMoveAnimation == null)
                            {
                                mMoveAnimation = new Animation();
                                mMoveAnimation.Duration = 300;
                                mMoveAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOutSquare);
                                mMoveAnimation.Finished += OnMoveAnimationFinished;
                            }

                            mMoveAnimation.Clear();
                            mMoveAnimation.AnimateTo(mPreviewRootView, "PositionX", mLastPreviewPositionX);
                            mMoveAnimation.Play();
                        }
                        else
                        {
                            if (deltaX < 0) // move from right to left
                            {
                                mCurrentPreviewIndex++;
                                if (mCurrentPreviewIndex < imageCount)
                                {
                                    if (mMoveAnimation == null)
                                    {
                                        mMoveAnimation = new Animation();
                                        mMoveAnimation.Duration = 300;
                                        mMoveAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOutSquare);
                                        mMoveAnimation.Finished += OnMoveAnimationFinished;
                                    }

                                    mMoveAnimation.Clear();
                                    mMoveAnimation.AnimateTo(mPreviewRootView, "PositionX", mLastPreviewPositionX - (540 + 60));
                                    mMoveAnimation.Play();
                                }
                            }
                            else
                            {
                                mCurrentPreviewIndex--;
                                if (mCurrentPreviewIndex >= 0)
                                {
                                    if (mMoveAnimation == null)
                                    {
                                        mMoveAnimation = new Animation();
                                        mMoveAnimation.Duration = 300;
                                        mMoveAnimation.DefaultAlphaFunction = new AlphaFunction(AlphaFunction.BuiltinFunctions.EaseOutSquare);
                                        mMoveAnimation.Finished += OnMoveAnimationFinished;
                                    }

                                    mMoveAnimation.Clear();
                                    mMoveAnimation.AnimateTo(mPreviewRootView, "PositionX", mLastPreviewPositionX + (540 + 60));
                                    mMoveAnimation.Play();
                                }
                            }
                        }
                        break;
                    }
            }
        }

        private void OnMoveAnimationFinished(object sender, EventArgs e)
        {
            // update opacity.
            int imageCount = ImageManager.Instance.ImageCount();
            for (int i = 0; i < imageCount; i++)
            {
                if (i == mCurrentPreviewIndex)
                {
                    mPreviewedImages[i].framestyle.Color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    mPreviewedImages[i].image.Color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                }
                else
                {
                    mPreviewedImages[i].framestyle.Color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                    mPreviewedImages[i].image.Color = new Color(0.5f, 0.5f, 0.5f, 1.0f);
                }
            }
        }

        private void DestroyPreview()
        {
            mCurrentPreviewIndex = 0;

            for (int i = 0; i < mPreviewedImages.Length; i++)
            {
                //

                //
                mPreviewRootView.Remove(mPreviewedImages[i].view);
                mPreviewedImages[i].view.Dispose();
                mPreviewedImages[i].view = null;
            }
            mPreviewedImages = null;

            if (mPreviewRootView != null)
            {
                mRootView.Remove(mPreviewRootView);
                mPreviewRootView.Dispose();
                mPreviewRootView = null;
            }
        }

        private void CreateFrameStyleGrid()
        {
            mFrameStyleRootView = new View();
            mFrameStyleRootView.Position2D = new Position2D(0, 405 + 610 + 164);
            mFrameStyleRootView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT - 405 - 610 - 164);
            mFrameStyleRootView.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            mRootView.Add(mFrameStyleRootView);

            // switch
            mSliderShowView = new View();
            mSliderShowView.Position2D = new Position2D(0, 0);
            mSliderShowView.Size2D = new Size2D(SCREEN_WIDTH, 88);
            //mFrameStyleRootView.Add(mSliderShowView);

            mSliderShowTextLabel = new TextLabel();
            mSliderShowTextLabel.Text = "Create Slidershow";
            mSliderShowTextLabel.FontFamily = "SamsungOneUI 500";
            mSliderShowTextLabel.PointSize = 30;
            mSliderShowTextLabel.TextColor = new Color((float)0xff / 0xff, (float)0xff / 0xff, (float)0xff / 0xff, 1.0f);
            mSliderShowTextLabel.Size2D = new Size2D(SCREEN_WIDTH - 44 - 80 - 20 - 40, 88);
            mSliderShowTextLabel.PositionUsesPivotPoint = true;
            mSliderShowTextLabel.PivotPoint = PivotPoint.CenterLeft;
            mSliderShowTextLabel.ParentOrigin = ParentOrigin.CenterLeft;
            mSliderShowTextLabel.HorizontalAlignment = HorizontalAlignment.End;
            mSliderShowTextLabel.VerticalAlignment = VerticalAlignment.Center;
            mSliderShowView.Add(mSliderShowTextLabel);

            mSliderShowSwitch = new Switch("Switch");
            mSliderShowSwitch.Position2D = new Position2D(SCREEN_WIDTH - 44 - 80 - 40, (88 - 50) / 2 - 4);
            mSliderShowSwitch.Size2D = new Size2D(80 + 20, 50);
            mSliderShowView.Add(mSliderShowSwitch);

            mSliderShowSwitch.SelectedEvent += OnSliderShowSwitchSelectedEvent;

            // grid
            mFrameStyleGrid = new FlexibleView();
            mFrameStyleGrid.Name = "Frame Style Grid";
            mFrameStyleGrid.Position2D = new Position2D(0, 88 + 25);
            mFrameStyleGrid.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT - 405 - 610 - 164 - 88 - 25);
            mFrameStyleGrid.Padding = new Extents(54, 54, 0, 0);
            mFrameStyleGrid.ItemClicked += OnGridItemClickEvent;
            mFrameStyleRootView.Add(mFrameStyleGrid);

            List<FrameStyleGridItemData> dataList = new List<FrameStyleGridItemData>();
            dataList.Add(new FrameStyleGridItemData(FB_PICTURE_FRAME_PICTURE_IMAGE, FB_PICTURE_FRAME_FRAME1_IMAGE, "Frameless"));
            dataList.Add(new FrameStyleGridItemData(FB_PICTURE_FRAME_PICTURE_IMAGE, FB_PICTURE_FRAME_FRAME2_IMAGE, "No shadow"));
            dataList.Add(new FrameStyleGridItemData(FB_PICTURE_FRAME_PICTURE_IMAGE, FB_PICTURE_FRAME_FRAME3_IMAGE, "Shelf shadow"));
            dataList.Add(new FrameStyleGridItemData(FB_PICTURE_FRAME_PICTURE_IMAGE, FB_PICTURE_FRAME_FRAME4_IMAGE, "Thin frame"));
            dataList.Add(new FrameStyleGridItemData(FB_PICTURE_FRAME_PICTURE_IMAGE, FB_PICTURE_FRAME_FRAME5_IMAGE, "Bold frame"));
            dataList.Add(new FrameStyleGridItemData(FB_PICTURE_FRAME_PICTURE_IMAGE, FB_PICTURE_FRAME_FRAME6_IMAGE, "Polaroid frame"));
            dataList.Add(new FrameStyleGridItemData(FB_PICTURE_FRAME_PICTURE_IMAGE, FB_PICTURE_FRAME_FRAME7_IMAGE, "Drawing frame"));
            dataList.Add(new FrameStyleGridItemData(FB_PICTURE_FRAME_PICTURE_IMAGE, FB_PICTURE_FRAME_FRAME8_IMAGE, "Brush frame"));

            mFrameStyleBridge = new FrameStyleGridBridge(dataList);
            mFrameStyleGrid.SetAdapter(mFrameStyleBridge);

            GridLayoutManager layoutManager = new GridLayoutManager(4, LinearLayoutManager.VERTICAL);
            mFrameStyleGrid.SetLayoutManager(layoutManager);
        }

        private void OnSliderShowSwitchSelectedEvent(object source, Switch.SelectEventArgs e)
        {
            Switch sliderSwitch = source as Switch;
            if (sliderSwitch != null)
            {
                if (sliderSwitch.IsSelected)
                {
                    mPreviewRootView.Hide();
                }
                else
                {
                    mPreviewRootView.Show();
                }
            }
        }

        private void OnGridItemClickEvent(object sender, FlexibleViewItemClickedEventArgs e)
        {
            if (e.ClickedView != null)
            {
                FrameStyleGridItemView iv = e.ClickedView.ItemView as FrameStyleGridItemView;
                if (iv != null)
                {
                    if (iv.IsChecked())
                    {
                        return;
                    }

                    iv.CheckFrameStyle(true);
                    if (mLastCheckedFrameStyle != null && mLastCheckedFrameStyle != e.ClickedView)
                    {
                        FrameStyleGridItemView lastItem = mLastCheckedFrameStyle.ItemView as FrameStyleGridItemView;
                        lastItem.CheckFrameStyle(false);
                    }
                    mLastCheckedFrameStyle = e.ClickedView;

                    ChooseFrameStyle(e.ClickedView.AdapterPosition, iv.GetFrameStyle());
                }
            }
        }
    }

    internal class FrameStyleGridItemData
    {
        public FrameStyleGridItemData(string picture, string frame, string style)
        {
            Picture = picture;
            Frame = frame;
            Style = style;
        }

        public string Picture
        {
            set;
            get;
        }

        public string Frame
        {
            set;
            get;
        }

        public string Style
        {
            set;
            get;
        }
    }

    internal class FrameStyleGridItemView : View
    {
        private View mRootView;
        private View mTextView;
        private TextLabel mTextLabel;
        private ImageView mPictureView;
        private ImageView mFrameView;
        private ImageView mCheckView;

        // state
        private bool mIsChecked = false;

        private readonly int FRAME_WIDTH = 228;
        private readonly int FRAME_HEIGHT = 241;

        private static string FB_PICTURE_DIR = CommonResource.GetResourcePath() + "picker/";
        private static string FB_PICTURE_CHECK_IMAGE = FB_PICTURE_DIR + "familyboard_photo_frame_check.png";

        public FrameStyleGridItemView(string picture, string frame, string text, int offset)
        {
            mRootView = new View();
            mRootView.Size2D = new Size2D(228, FRAME_HEIGHT + 34 + offset);

            // title
            mTextView = new View();
            mTextView.Position2D = new Position2D(29, 0);
            mTextView.Size2D = new Size2D(FRAME_WIDTH - 29, 34);
            mRootView.Add(mTextView);

            mTextLabel = new TextLabel();
            mTextLabel.Text = text;
            mTextLabel.FontFamily = "SamsungOneUI 500";
            mTextLabel.PointSize = 24;
            mTextLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            mTextLabel.HorizontalAlignment = HorizontalAlignment.Begin;
            mTextLabel.VerticalAlignment = VerticalAlignment.Bottom;
            mTextLabel.PositionUsesPivotPoint = true;
            mTextLabel.ParentOrigin = Tizen.NUI.ParentOrigin.CenterLeft;
            mTextLabel.PivotPoint = Tizen.NUI.PivotPoint.CenterLeft;
            mTextView.Add(mTextLabel);

            mFrameView = new ImageView();
            mFrameView.ResourceUrl = frame;
            mFrameView.Position2D = new Position2D(0, 34 + offset);
            mFrameView.Size2D = new Size2D(FRAME_WIDTH, FRAME_HEIGHT);
            mRootView.Add(mFrameView);

            mPictureView = new ImageView();
            mPictureView.ResourceUrl = picture;
            mPictureView.Position2D = new Position2D(0, 34 + offset);
            mPictureView.Size2D = new Size2D(FRAME_WIDTH, FRAME_HEIGHT);
            mRootView.Add(mPictureView);

            // check
            mCheckView = new ImageView();
            mCheckView.ResourceUrl = FB_PICTURE_CHECK_IMAGE;
            mCheckView.Position2D = new Position2D(0, 34 + offset);
            mCheckView.Size2D = new Size2D(FRAME_WIDTH, FRAME_HEIGHT);
            mRootView.Add(mCheckView);
            mCheckView.Hide();

            Add(mRootView);
        }

        public bool IsChecked()
        {
            return mIsChecked;
        }

        public string GetFrameStyle()
        {
            return mFrameView.ResourceUrl;
        }

        public void CheckFrameStyle(bool isChecked)
        {
            if (isChecked)
            {
                if (!mIsChecked)
                {
                    mCheckView.Show();
                    mIsChecked = true;
                }
            }
            else
            {
                mCheckView.Hide();
                mIsChecked = false;
            }
        }

        protected override void Dispose(DisposeTypes type)
        {
            if (disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                if (mCheckView != null)
                {
                    mRootView.Remove(mCheckView);
                    mCheckView.Dispose();
                    mCheckView = null;
                }
                if (mPictureView != null)
                {
                    mRootView.Remove(mPictureView);
                    mPictureView.Dispose();
                    mPictureView = null;
                }
                if (mTextLabel != null)
                {
                    mTextView.Remove(mTextLabel);
                    mTextLabel.Dispose();
                    mTextLabel = null;
                }
                if (mTextView != null)
                {
                    mRootView.Remove(mTextView);
                    mTextView.Dispose();
                    mTextView = null;
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

    internal class FrameStyleGridBridge : FlexibleViewAdapter
    {
        private List<FrameStyleGridItemData> mDatas;

        public FrameStyleGridBridge(List<FrameStyleGridItemData> datas)
        {
            mDatas = datas;
        }

        public override int GetItemViewType(int position)
        {
            return position;  // position is used as item view type.
        }

        public override FlexibleViewViewHolder OnCreateViewHolder(int viewType)
        {
            View item_view = null;
            FrameStyleGridItemData listItemData = mDatas[viewType];
            int offset = -2;
            if (viewType > 3)
            {
                offset = 11;
            }
            item_view = new FrameStyleGridItemView(listItemData.Picture, listItemData.Frame, listItemData.Style, offset);
            FlexibleViewViewHolder viewHolder = new FlexibleViewViewHolder(item_view);
            return viewHolder;
        }

        public override void OnBindViewHolder(FlexibleViewViewHolder holder, int position)
        {
            FrameStyleGridItemData listItemData = mDatas[position];
            if (position < 4)
            {
                FrameStyleGridItemView listItemView = holder.ItemView as FrameStyleGridItemView;
                listItemView.Name = "Item" + position;
                listItemView.Size2D = new Size2D(228, 273);
                if (position < 3)
                {
                    listItemView.Margin = new Extents(0, 20, 0, 0);
                }
                else
                {
                    listItemView.Margin = new Extents(0, 0, 0, 0);
                }
            }
            else
            {
                FrameStyleGridItemView listItemView = holder.ItemView as FrameStyleGridItemView;
                listItemView.Name = "Item" + position;
                listItemView.Size2D = new Size2D(228, 286);
                if (position < 7)
                {
                    listItemView.Margin = new Extents(0, 20, 0, 0);
                }
                else
                {
                    listItemView.Margin = new Extents(0, 0, 0, 0);
                }
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
