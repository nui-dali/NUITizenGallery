using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.FH.FamilyBoard
{
    public class PictureChooser : ILifecycleObserver
    {
        private View mRootView;

        // close
        private View mCloseView;
        private ImageView mCloseImageView;
        private TapGestureDetector mCloseTapGestureDetector;

        private View mPictureRootView;

        // title
        private View mTitleView;
        private TextLabel mTitleLabel;

        // next button
        private Button mNextButton;

        // picture grid
        private FlexibleView mPictureGrid;
        private PictureGridBridge mPictureGridBridge;
        private List<FlexibleViewViewHolder> mPictureItemList = new List<FlexibleViewViewHolder>();

        private NUI.Components.Toast mMessageIndicator;

        private readonly int SCREEN_WIDTH = 1080;
        private readonly int SCREEN_HEIGHT = 1920;

        //
        private static string FB_PICTURE_DIR = CommonResource.GetResourcePath() + "picker/";
        private static string FB_PICTURE_CLOSE_IMAGE = FB_PICTURE_DIR + "familyboard_ic_delete.png";
        private static string FB_PICTURE_NEXT_BUTTON_IMAGE = FB_PICTURE_DIR + "familyboard_photo_drawer_btn.png";

        private static string FB_PHOTO_ALBUM_DIR = CommonResource.GetResourcePath() + "photo_album/";
        private static string FB_PHOTO_ALBUM_IMAGE_004 = FB_PHOTO_ALBUM_DIR + "Photo_004.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_005 = FB_PHOTO_ALBUM_DIR + "Photo_005.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_006 = FB_PHOTO_ALBUM_DIR + "Photo_006.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_007 = FB_PHOTO_ALBUM_DIR + "Photo_007.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_008 = FB_PHOTO_ALBUM_DIR + "Photo_008.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_009 = FB_PHOTO_ALBUM_DIR + "Photo_009.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_010 = FB_PHOTO_ALBUM_DIR + "Photo_010.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_012 = FB_PHOTO_ALBUM_DIR + "Photo_012.jpg";

        // thumb
        private static string FB_PHOTO_ALBUM_IMAGE_001_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_001_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_002_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_002_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_003_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_003_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_004_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_004_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_005_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_005_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_006_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_006_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_007_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_007_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_008_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_008_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_009_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_009_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_010_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_010_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_011_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_011_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_012_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_012_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_013_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_013_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_014_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_014_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_015_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_015_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_016_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_016_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_017_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_017_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_018_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_018_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_019_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_019_thumb.jpg";
        private static string FB_PHOTO_ALBUM_IMAGE_020_THUMB = FB_PHOTO_ALBUM_DIR + "Photo_020_thumb.jpg";


        public void Activate()
        {
            mRootView = new View();
            mRootView.BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);
            mRootView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT);

            NUIApplication.GetDefaultWindow().Add(mRootView);

            CreateCloseButton();

            CreatePictureGrid();

            // message
            mMessageIndicator = new NUI.Components.Toast("BasicShortToast");
            mMessageIndicator.Duration = 1000;
            mMessageIndicator.TextArray = new string[] { "You can only select up to 5 pictures." };
            mMessageIndicator.Position2D = new Position2D((SCREEN_WIDTH - 800)/2, (SCREEN_HEIGHT - 200)/2);
            mMessageIndicator.Size2D = new Size2D(800, 200);
            mMessageIndicator.BackgroundColor = Color.Transparent;
            mRootView.Add(mMessageIndicator);
            mMessageIndicator.Hide();
        }

        public void Reactivate()
        {
            // nothing to do.
        }

        public void Deactivate()
        {
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

            if (mNextButton != null)
            {
                mPictureRootView.Remove(mNextButton);
                mNextButton.Dispose();
                mNextButton = null;
            }

            if (mTitleLabel != null)
            {
                mTitleView.Remove(mTitleLabel);
                mTitleLabel.Dispose();
                mTitleLabel = null;
            }

            if (mTitleView != null)
            {
                mPictureRootView.Remove(mTitleView);
                mTitleView.Dispose();
                mTitleView = null;
            }

            if (mPictureGrid != null)
            {
                mPictureRootView.Remove(mPictureGrid);
                mPictureGrid.Dispose();
                mPictureGrid = null;
            }

            if (mPictureRootView != null)
            {
                mRootView.Remove(mPictureRootView);
                mPictureRootView.Dispose();
                mPictureRootView = null;
            }

            if (mMessageIndicator != null)
            {
                mRootView.Remove(mMessageIndicator);
                mMessageIndicator.Dispose();
                mMessageIndicator = null;
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

        public void ShowMessage()
        {
            if (mMessageIndicator != null)
            {
                mMessageIndicator.Show();
            }
        }

        public void ShowNextButton(bool selected)
        {
            if (selected)
            {
                mNextButton.Show();
            }
            else
            {
                mNextButton.Hide();
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

        private void CreatePictureGrid()
        {
            // picture
            mPictureRootView = new View();
            mPictureRootView.Position2D = new Position2D(0, 405);
            mPictureRootView.Size2D = new Size2D(SCREEN_WIDTH, 177 + (340 + 10) * 3 + 340);
            mPictureRootView.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            mRootView.Add(mPictureRootView);

            // title
            mTitleView = new View();
            mTitleView.Position2D = new Position2D();
            mTitleView.Size2D = new Size2D(SCREEN_WIDTH, 177);
            mPictureRootView.Add(mTitleView);

            mTitleLabel = new TextLabel();
            mTitleLabel.Text = "Picture";
            mTitleLabel.FontFamily = "SamsungOneUI 600";
            mTitleLabel.PointSize = 40;
            mTitleLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            mTitleLabel.PositionUsesPivotPoint = true;
            mTitleLabel.PivotPoint = PivotPoint.Center;
            mTitleLabel.ParentOrigin = ParentOrigin.Center;
            mTitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            mTitleLabel.VerticalAlignment = VerticalAlignment.Center;
            mTitleView.Add(mTitleLabel);

            // next button
            mNextButton = new Button();
            mNextButton.BackgroundImage = FB_PICTURE_NEXT_BUTTON_IMAGE;
            mNextButton.Text = "Next";
            mNextButton.TextColor = Color.White;
            mNextButton.PointSize = 30;
            mNextButton.FontFamily = "SamsungOneUI 500";
            mNextButton.Position2D = new Position2D(SCREEN_WIDTH - 151 - 40 - 46, (177 - 70) / 2);
            mNextButton.Size2D = new Size2D(151 + 40, 60 + 10);
            mPictureRootView.Add(mNextButton);

            mNextButton.ClickEvent += OnNextButtonClickEvent;
            mNextButton.Hide();

            // grid
            mPictureGrid = new FlexibleView();
            mPictureGrid.Name = "Picture Grid";
            mPictureGrid.Position2D = new Position2D(0, 177);
            mPictureGrid.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT - 405 - 177);
            mPictureGrid.Padding = new Extents(41, 41, 0, 0);
            mPictureGrid.ItemClicked += OnGridItemClickEvent;
            mPictureRootView.Add(mPictureGrid);

            // check if thumb is generated or not.

            //Multimedia.Size imgSize = new Multimedia.Size(326, 340);
            //ThumbnailExtractor.Extract(FB_PHOTO_ALBUM_004_IMAGE, imgSize, Applications.Application.Current.DirectoryInfo.Cache + "Photo_004_thumb.jpg");
            //ThumbnailExtractor.Extract(FB_PHOTO_ALBUM_007_IMAGE, imgSize, Applications.Application.Current.DirectoryInfo.Cache + "Photo_007_thumb.jpg");
            //ThumbnailExtractor.Extract(FB_PHOTO_ALBUM_010_IMAGE, imgSize, Applications.Application.Current.DirectoryInfo.Cache + "Photo_010_thumb.jpg");
            //ThumbnailExtractor.Extract(FB_PHOTO_ALBUM_012_IMAGE, imgSize, Applications.Application.Current.DirectoryInfo.Cache + "Photo_012_thumb.jpg");
            //ThumbnailExtractor.Extract(FB_PHOTO_ALBUM_005_IMAGE, imgSize, Applications.Application.Current.DirectoryInfo.Cache + "Photo_005_thumb.jpg");
            //ThumbnailExtractor.Extract(FB_PHOTO_ALBUM_006_IMAGE, imgSize, Applications.Application.Current.DirectoryInfo.Cache + "Photo_006_thumb.jpg");
            //ThumbnailExtractor.Extract(FB_PHOTO_ALBUM_008_IMAGE, imgSize, Applications.Application.Current.DirectoryInfo.Cache + "Photo_008_thumb.jpg");
            //ThumbnailExtractor.Extract(FB_PHOTO_ALBUM_009_IMAGE, imgSize, Applications.Application.Current.DirectoryInfo.Cache + "Photo_009_thumb.jpg");

            List<PictureGridItemData> dataList = new List<PictureGridItemData>();
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_001_THUMB, 2000, 1334));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_002_THUMB, 2000, 1331));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_003_THUMB, 1080, 1920));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_004_THUMB, 1080, 1920));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_005_THUMB, 1080, 1920));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_006_THUMB, 1080, 1920));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_007_THUMB, 2000, 1334));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_008_THUMB, 1080, 1920));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_009_THUMB, 2000, 1333));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_010_THUMB, 1080, 1920));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_011_THUMB, 1080, 1920));

            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_012_THUMB, 1080, 1920));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_013_THUMB, 2000, 1375));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_014_THUMB, 2000, 1333));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_015_THUMB, 2000, 1333));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_016_THUMB, 2000, 1333));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_017_THUMB, 2000, 1333));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_018_THUMB, 2000, 1333));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_019_THUMB, 1080, 1920));
            dataList.Add(new PictureGridItemData(FB_PHOTO_ALBUM_IMAGE_020_THUMB, 2000, 1333));


            mPictureGridBridge = new PictureGridBridge(this, dataList);
            mPictureGrid.SetAdapter(mPictureGridBridge);

            GridLayoutManager layoutManager = new GridLayoutManager(3, LinearLayoutManager.VERTICAL);
            mPictureGrid.SetLayoutManager(layoutManager);
        }

        private void OnNextButtonClickEvent(object sender, Button.ClickEventArgs e)
        {
            PictureWizard.Instance.Next();
        }

        private void OnGridItemClickEvent(object sender, FlexibleViewItemClickedEventArgs e)
        {
            if (e.ClickedView != null)
            {
                PictureGridItemView iv = e.ClickedView.ItemView as PictureGridItemView;
                if (iv != null)
                {
                    iv.SelectImage();

                    if (iv.IsSelected())
                    {
                        mPictureItemList.Add(e.ClickedView);
                    }
                    else
                    {
                        mPictureItemList.Remove(e.ClickedView);
                        iv.SetIndicator("");
                    }

                    for (int i = 0; i < mPictureItemList.Count; i++)
                    {
                        PictureGridItemView pgv = mPictureItemList[i].ItemView as PictureGridItemView;
                        int index = ImageManager.Instance.GetImageIndex(pgv.ImageId());
                        if (index != 0)
                        {
                            pgv.SetIndicator("" + index);
                        }
                        else
                        {
                            pgv.SetIndicator("");
                        }
                    }
                }
            }
        }
    }

    internal class PictureGridItemData
    {
        public PictureGridItemData(string url, int width, int height)
        {
            Picture = url;
            Width = width;
            Height = height;
        }

        public string Picture
        {
            set;
            get;
        }

        public int Width
        {
            set;
            get;
        }

        public int Height
        {
            set;
            get;
        }
    }

    internal class PictureGridItemView : View
    {
        private View mRootView;
        private ImageView mPictureView;
        private View mCheckTextView;
        private TextLabel mTextLabel;

        // state
        private bool mIsSelected = false;

        //
        private PictureChooser mPictureChooser;

        private static string FB_PICTURE_DIR = CommonResource.GetResourcePath() + "picker/";
        private static string FB_PICTURE_OVAL_NORMAL_IMAGE = FB_PICTURE_DIR + "familyboard_photo_ic_nor.png";
        private static string FB_PICTURE_OVAL_SELECTED_IMAGE = FB_PICTURE_DIR + "familyboard_photo_ic_sel.png";

        public PictureGridItemView(PictureChooser chooser, string photoThumb)
        {
            mPictureChooser = chooser;

            //
            mRootView = new View();
            mRootView.Size2D = new Size2D(326, 340);

            mPictureView = new ImageView();

            mPictureView.ResourceUrl = photoThumb;
            mPictureView.PositionUsesPivotPoint = true;
            mPictureView.ParentOrigin = Tizen.NUI.ParentOrigin.TopLeft;
            mPictureView.PivotPoint = Tizen.NUI.PivotPoint.TopLeft;
            mPictureView.Position2D = new Position2D(0, 0);
            mPictureView.Size2D = new Size2D(326, 340);

            if (mPictureView.NaturalSize2D.Width < mPictureView.NaturalSize2D.Height)
            {
                mPictureView.FittingMode = FittingModeType.FitWidth;
            }
            else
            {
                mPictureView.FittingMode = FittingModeType.FitHeight;
            }

            mRootView.Add(mPictureView);

            // check and digit
            mCheckTextView = new View();
            mCheckTextView.Position2D = new Position2D(326 - 10 - 50, 10);
            mCheckTextView.Size2D = new Size2D(50, 50);
            mRootView.Add(mCheckTextView);

            mTextLabel = new TextLabel();
            mTextLabel.BackgroundImage = FB_PICTURE_OVAL_NORMAL_IMAGE;
            mTextLabel.FontFamily = "SamsungOneUI 600";
            mTextLabel.PointSize = 16;
            mTextLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            mTextLabel.HorizontalAlignment = HorizontalAlignment.Center;
            mTextLabel.VerticalAlignment = VerticalAlignment.Center;
            mTextLabel.PositionUsesPivotPoint = true;
            mTextLabel.PivotPoint = Tizen.NUI.PivotPoint.Center;
            mTextLabel.ParentOrigin = Tizen.NUI.ParentOrigin.Center;
            mTextLabel.Size2D = new Size2D(50, 50);
            mCheckTextView.Add(mTextLabel);

            Add(mRootView);
        }

        public bool IsSelected()
        {
            return mIsSelected;
        }

        public string ImageId()
        {
            return mPictureView.ResourceUrl.Replace("_thumb.jpg", ".jpg"); ;
        }

        public void SetIndicator(string index)
        {
            mTextLabel.Text = index;
        }

        public void SelectImage()
        {
            mIsSelected = !mIsSelected;
            if (mIsSelected)
            {
                mPictureChooser.ShowNextButton(true);
                if (ImageManager.Instance.ImageCount() >= 5)
                {
                    mPictureChooser.ShowMessage();
                    mIsSelected = !mIsSelected; // restore.
                }
                else
                {
                    mTextLabel.BackgroundImage = FB_PICTURE_OVAL_SELECTED_IMAGE;
                    string newPath = mPictureView.ResourceUrl.Replace("_thumb.jpg", ".jpg");
                    ImageManager.Instance.AddImage(newPath, mPictureView.ResourceUrl, "", ImageManager.ItemType.PHOTO);
                }
            }
            else
            {
                mTextLabel.BackgroundImage = FB_PICTURE_OVAL_NORMAL_IMAGE;
                string newPath = mPictureView.ResourceUrl.Replace("_thumb.jpg", ".jpg");
                ImageManager.Instance.RemoveImage(newPath);
                if (ImageManager.Instance.ImageCount() == 0)
                {
                    mPictureChooser.ShowNextButton(false);
                }
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
                if (mTextLabel != null)
                {
                    mCheckTextView.Remove(mTextLabel);
                    mTextLabel.Dispose();
                    mTextLabel = null;
                }
                if (mCheckTextView != null)
                {
                    mRootView.Remove(mCheckTextView);
                    mCheckTextView.Dispose();
                    mCheckTextView = null;
                }
                if (mPictureView != null)
                {
                    mRootView.Remove(mPictureView);
                    mPictureView.Dispose();
                    mPictureView = null;
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

    internal class PictureGridBridge : FlexibleViewAdapter
    {
        private List<PictureGridItemData> mDatas;
        private PictureChooser mPictureChooser;

        public PictureGridBridge(PictureChooser chooser, List<PictureGridItemData> datas)
        {
            mPictureChooser = chooser;
            mDatas = datas;
        }

        public override int GetItemViewType(int position)
        {
            return position;  // position is used as item view type.
        }

        public override FlexibleViewViewHolder OnCreateViewHolder(int viewType)
        {
            View item_view = null;
            PictureGridItemData listItemData = mDatas[viewType];

            item_view = new PictureGridItemView(mPictureChooser, listItemData.Picture);
            FlexibleViewViewHolder viewHolder = new FlexibleViewViewHolder(item_view);

            return viewHolder;
        }

        public override void OnBindViewHolder(FlexibleViewViewHolder holder, int position)
        {
            PictureGridItemData listItemData = mDatas[position];
            if (position % (3 - 1) == 0)
            {
                PictureGridItemView listItemView = holder.ItemView as PictureGridItemView;
                listItemView.Name = "Item" + position;
                listItemView.Size2D = new Size2D(326, 340);
                listItemView.Margin = new Extents(0, 10, 0, 10);
            }
            else
            {
                PictureGridItemView listItemView = holder.ItemView as PictureGridItemView;
                listItemView.Name = "Item" + position;
                listItemView.Size2D = new Size2D(326, 340);
                listItemView.Margin = new Extents(0, 0, 0, 10);
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
