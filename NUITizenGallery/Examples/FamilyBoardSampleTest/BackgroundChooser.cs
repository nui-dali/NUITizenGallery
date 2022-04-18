
using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.FH.FamilyBoard
{
    public class BackgroundImageChooser : ILifecycleObserver
    {
        private View mRootView;

        // close
        private View mCloseView;
        private ImageView mCloseImageView;
        private TapGestureDetector mCloseTapGestureDetector;

        // title
        private View mTitleRootView;

        private View mTitleView;
        private TextLabel mTitleTextLabel;

        private View[] mDividers;

        private View mDateView;
        private TextLabel mDateTextLabel;

        private View mWallPaperView;
        private TextLabel mWallPaperTextLabel;

        // background image grid
        private View mBackgroundImageRootView;
        private FlexibleView mBackgourndImageGrid;
        private BackgroundImageGridBridge mBackgroundImageGridBridge;
        private FlexibleViewViewHolder mLastCheckedBackgroundImage;

        //
        private readonly int SCREEN_WIDTH = 1080;
        private readonly int SCREEN_HEIGHT = 1920;

        //
        private static string FB_BACKGROUND_IMAGE_DIR = CommonResource.GetResourcePath() + "picker/";
        private static string FB_BACKGROUND_IMAGE_CLOSE_IMAGE = FB_BACKGROUND_IMAGE_DIR + "familyboard_ic_delete.png";

        public void Activate()
        {
            mRootView = new View();
            mRootView.BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);
            mRootView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT);
            mRootView.ClippingMode = ClippingModeType.ClipToBoundingBox;

            CreateCloseButton();

            // title
            CreateTitleDateWallpaper();

            // background image
            CreateBackgroundImageGrid();

            //
            NUIApplication.GetDefaultWindow().Add(mRootView);
        }

        public void Reactivate()
        {
        }

        public void Deactivate()
        {
            if (mBackgourndImageGrid != null)
            {
                mBackgroundImageRootView.Remove(mBackgourndImageGrid);
                mBackgourndImageGrid.Dispose();
                mBackgourndImageGrid = null;
            }

            if (mBackgroundImageRootView != null)
            {
                mRootView.Remove(mBackgroundImageRootView);
                mBackgroundImageRootView.Dispose();
                mBackgroundImageRootView = null;
            }

            if (mTitleTextLabel != null)
            {
                mTitleView.Remove(mTitleTextLabel);
                mTitleTextLabel.Dispose();
                mTitleTextLabel = null;
            }

            if (mTitleView != null)
            {
                mTitleRootView.Remove(mTitleView);
                mTitleView.Dispose();
                mTitleView = null;
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

        public void ChooseBackgroundImage(int position, string style)
        {
            ImageManager.Instance.SetBackgroundImage(style);
            FamilyBoardPage.Instance.ChangeMainBackground();
        }

        public void SetLastCheckedBackgroundImage(FlexibleViewViewHolder vh)
        {
            mLastCheckedBackgroundImage = vh;
        }

        private void CreateCloseButton()
        {
            // close
            mCloseView = new View();
            mCloseView.Position2D = new Position2D(0, 0);
            mCloseView.Size2D = new Size2D(SCREEN_WIDTH, 405);
            mRootView.Add(mCloseView);

            mCloseImageView = new ImageView();
            mCloseImageView.ResourceUrl = FB_BACKGROUND_IMAGE_CLOSE_IMAGE;
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

        private void CreateTitleDateWallpaper()
        {
            mTitleRootView = new View();
            mTitleRootView.Position2D = new Position2D(0, 605 - 1 - 164);
            mTitleRootView.Size2D = new Size2D(SCREEN_WIDTH, 164 + 1 + 164 + 1 + 164);
            mTitleRootView.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            mRootView.Add(mTitleRootView);

            // title
            mTitleView = new View();
            mTitleView.Position2D = new Position2D(0, 0);
            mTitleView.Size2D = new Size2D(SCREEN_WIDTH, 164 + 1);
            mTitleRootView.Add(mTitleView);

            mTitleTextLabel = new TextLabel();
            mTitleTextLabel.Text = "Family Board Settings";
            mTitleTextLabel.FontFamily = "SamsungOneUI 600";
            mTitleTextLabel.PointSize = 30;
            mTitleTextLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            mTitleTextLabel.Size2D = new Size2D(SCREEN_WIDTH, 164);
            mTitleTextLabel.PositionUsesPivotPoint = true;
            mTitleTextLabel.PivotPoint = PivotPoint.Center;
            mTitleTextLabel.ParentOrigin = ParentOrigin.Center;
            mTitleTextLabel.HorizontalAlignment = HorizontalAlignment.Center;
            mTitleTextLabel.VerticalAlignment = VerticalAlignment.Center;
            mTitleView.Add(mTitleTextLabel);

            // divider
            mDividers = new View[2];

            mDividers[0] = new View();
            mDividers[0].BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.08f);
            mDividers[0].Size2D = new Size2D(968, 1);
            mDividers[0].PositionUsesPivotPoint = true;
            mDividers[0].PivotPoint = PivotPoint.BottomCenter;
            mDividers[0].ParentOrigin = ParentOrigin.BottomCenter;
            //mTitleView.Add(mDividers[0]);

            // date
            mDateView = new View();
            mDateView.Position2D = new Position2D(64, 164 + 1);
            mDateView.Size2D = new Size2D(SCREEN_WIDTH - 64 - 48, 164 + 1);
            mTitleRootView.Add(mDateView);

            mDateTextLabel = new TextLabel();
            mDateTextLabel.Text = "Show Date, Time and Weather";
            mDateTextLabel.FontFamily = "SamsungOneUI 400";
            mDateTextLabel.PointSize = 35;
            mDateTextLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            mDateTextLabel.Size2D = new Size2D(SCREEN_WIDTH - 64 - 48 - 120 - 48, 164);
            mDateTextLabel.PositionUsesPivotPoint = true;
            mDateTextLabel.PivotPoint = PivotPoint.CenterLeft;
            mDateTextLabel.ParentOrigin = ParentOrigin.CenterLeft;
            mDateTextLabel.HorizontalAlignment = HorizontalAlignment.Begin;
            mDateTextLabel.VerticalAlignment = VerticalAlignment.Center;
            //mDateView.Add(mDateTextLabel);

            // divider
            mDividers[1] = new View();
            mDividers[1].BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.08f);
            mDividers[1].Position2D = new Position2D((SCREEN_WIDTH - 968) / 2, 164 + 1 + 164 - 180);
            mDividers[1].Size2D = new Size2D(968, 1);
            mTitleRootView.Add(mDividers[1]);

            // wallpaper
            mWallPaperView = new View();
            mWallPaperView.Position2D = new Position2D(64, 164 + 1 + 164 + 1 - 180);
            mWallPaperView.Size2D = new Size2D(SCREEN_WIDTH - 64 - 48, 164 + 1);
            mTitleRootView.Add(mWallPaperView);

            mWallPaperTextLabel = new TextLabel();
            mWallPaperTextLabel.Text = "Background Wallpaper";
            mWallPaperTextLabel.FontFamily = "SamsungOneUI 400";
            mWallPaperTextLabel.PointSize = 35;
            mWallPaperTextLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            mWallPaperTextLabel.Size2D = new Size2D(SCREEN_WIDTH - 64 - 48 - 120 - 48, 164);
            mWallPaperTextLabel.PositionUsesPivotPoint = true;
            mWallPaperTextLabel.PivotPoint = PivotPoint.CenterLeft;
            mWallPaperTextLabel.ParentOrigin = ParentOrigin.CenterLeft;
            mWallPaperTextLabel.HorizontalAlignment = HorizontalAlignment.Begin;
            mWallPaperTextLabel.VerticalAlignment = VerticalAlignment.Center;
            mWallPaperView.Add(mWallPaperTextLabel);
        }

        private void CreateBackgroundImageGrid()
        {
            mBackgroundImageRootView = new View();
            mBackgroundImageRootView.Position2D = new Position2D(0, 405 + 164 + 1 + 164);
            mBackgroundImageRootView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT - (405 + 164 + 1 + 164));
            mBackgroundImageRootView.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            mRootView.Add(mBackgroundImageRootView);

            // grid
            mBackgourndImageGrid = new FlexibleView();
            mBackgourndImageGrid.Name = "Background Image Grid";
            mBackgourndImageGrid.Position2D = new Position2D(0, 0);
            mBackgourndImageGrid.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT - (405 + 164 + 1 + 164));
            mBackgourndImageGrid.Padding = new Extents(40, 40, 0, 0);
            mBackgourndImageGrid.ItemClicked += OnGridItemClickEvent;
            mBackgroundImageRootView.Add(mBackgourndImageGrid);

            List<BackgroundImageGridItemData> dataList = new List<BackgroundImageGridItemData>();

            // image 9, 10, 14 does not exist.
            for (int i = 1; i <= 17; i++)
            {
                string backImage = "";
                if (i == 9 || i == 10 || i == 14)
                {
                    continue; 
                }
                backImage = $"{CommonResource.GetResourcePath()}familyboard_setting_bg{i}.png";
                dataList.Add(new BackgroundImageGridItemData(backImage));
            }

            mBackgroundImageGridBridge = new BackgroundImageGridBridge(this, dataList);
            mBackgourndImageGrid.SetAdapter(mBackgroundImageGridBridge);

            GridLayoutManager layoutManager = new GridLayoutManager(4, LinearLayoutManager.VERTICAL);
            mBackgourndImageGrid.SetLayoutManager(layoutManager);
        }

        private void OnGridItemClickEvent(object sender, FlexibleViewItemClickedEventArgs e)
        {
            if (e.ClickedView != null)
            {
                BackgroundImageGridItemView iv = e.ClickedView.ItemView as BackgroundImageGridItemView;
                if (iv != null)
                {
                    if (iv.IsChecked())
                    {
                        return;
                    }

                    iv.CheckBackgroundImage(true);
                    if (mLastCheckedBackgroundImage != null && mLastCheckedBackgroundImage != e.ClickedView)
                    {
                        BackgroundImageGridItemView lastItem = mLastCheckedBackgroundImage.ItemView as BackgroundImageGridItemView;
                        lastItem.CheckBackgroundImage(false);
                    }
                    mLastCheckedBackgroundImage = e.ClickedView;

                    ChooseBackgroundImage(e.ClickedView.AdapterPosition, iv.GetBackgroundImage());
                }
            }
        }
    }

    internal class BackgroundImageGridItemData
    {
        public BackgroundImageGridItemData(string picture)
        {
            BackgroundImage = picture;
        }

        public string BackgroundImage
        {
            set;
            get;
        }
    }

    internal class BackgroundImageGridItemView : View
    {
        private ImageView mBackgroundImageView;
        private ImageView mCheckImageView;

        // state
        private bool mIsChecked = false;

        private readonly int IMAGE_WIDTH = 236;
        private readonly int IMAGE_HEIGHT = 420;

        private static string FB_PICTURE_DIR = CommonResource.GetResourcePath();
        private static string FB_PICTURE_CHECK_IMAGE = FB_PICTURE_DIR + "familyboard_settings_check.png";

        public BackgroundImageGridItemView(string picture)
        {
            mBackgroundImageView = new ImageView();
            mBackgroundImageView.ResourceUrl = picture;
            mBackgroundImageView.Position2D = new Position2D(0, 0);
            mBackgroundImageView.Size2D = new Size2D(IMAGE_WIDTH, IMAGE_HEIGHT);
            Add(mBackgroundImageView);

            // check
            mCheckImageView = new ImageView();
            mCheckImageView.ResourceUrl = FB_PICTURE_CHECK_IMAGE;
            mCheckImageView.Position2D = new Position2D(0, 0);
            mCheckImageView.Size2D = new Size2D(IMAGE_WIDTH, IMAGE_HEIGHT);
            Add(mCheckImageView);

            mIsChecked = false;
            mCheckImageView.Hide();
        }

        public bool IsChecked()
        {
            return mIsChecked;
        }

        public void Checked()
        {
            mIsChecked = true;
            mCheckImageView.Show();
        }

        public string GetBackgroundImage()
        {
            return mBackgroundImageView.ResourceUrl;
        }

        public void CheckBackgroundImage(bool isChecked)
        {
            if (isChecked)
            {
                if (!mIsChecked)
                {
                    mCheckImageView.Show();
                    mIsChecked = true;
                }
            }
            else
            {
                mCheckImageView.Hide();
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
                if (mCheckImageView != null)
                {
                    Remove(mCheckImageView);
                    mCheckImageView.Dispose();
                    mCheckImageView = null;
                }

                if (mBackgroundImageView != null)
                {
                    Remove(mBackgroundImageView);
                    mBackgroundImageView.Dispose();
                    mBackgroundImageView = null;
                }
            }

            base.Dispose(type);
        }
    }

    internal class BackgroundImageGridBridge : FlexibleViewAdapter
    {
        private List<BackgroundImageGridItemData> mDatas;
        private BackgroundImageChooser mBackgroundImageChooser;

        public BackgroundImageGridBridge(BackgroundImageChooser chooser, List<BackgroundImageGridItemData> datas)
        {
            mBackgroundImageChooser = chooser;
            mDatas = datas;
        }

        public override int GetItemViewType(int position)
        {
            return position;  // position is used as item view type.
        }

        public override FlexibleViewViewHolder OnCreateViewHolder(int viewType)
        {
            BackgroundImageGridItemData listItemData = mDatas[viewType];

            BackgroundImageGridItemView item_view = new BackgroundImageGridItemView(listItemData.BackgroundImage);
            FlexibleViewViewHolder viewHolder = new FlexibleViewViewHolder(item_view);
            if (ImageManager.Instance.GetBackgroundImage().Equals(listItemData.BackgroundImage))
            {
                item_view.Checked();
                mBackgroundImageChooser.SetLastCheckedBackgroundImage(viewHolder);
            }

            return viewHolder;
        }

        public override void OnBindViewHolder(FlexibleViewViewHolder holder, int position)
        {
            BackgroundImageGridItemData listItemData = mDatas[position];
            BackgroundImageGridItemView listItemView = holder.ItemView as BackgroundImageGridItemView;

            listItemView.Name = "Item" + position;
            listItemView.Size2D = new Size2D(236, 420);

            if (position % (4 - 1) == 0)
            {
                listItemView.Margin = new Extents(0, 10, 0, 20);
            }
            else
            {
                listItemView.Margin = new Extents(0, 0, 0, 20);
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
