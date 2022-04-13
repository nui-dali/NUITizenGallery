using System.Collections.Generic;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.FH.FamilyBoard
{
    class StickerChooser : ILifecycleObserver
    {
        private View mRootView;

        // close
        private View mCloseView;
        private ImageView mCloseImageView;
        private TapGestureDetector mCloseTapGestureDetector;

        private View mStickersRootView;

        // title
        private View mTitleView;
        private TextLabel mTitleLabel;

        // stickers grid
        private FlexibleView mStickersListView;
        private StickersListBridge mStickerListBridge;
        private List<FlexibleViewViewHolder> mStickerItemList = new List<FlexibleViewViewHolder>();

        private readonly int SCREEN_WIDTH = 1080;
        private readonly int SCREEN_HEIGHT = 1920;

        private static string FB_PICTURE_DIR = CommonResource.GetResourcePath() + "picker/";
        private static string FB_PICTURE_CLOSE_IMAGE = FB_PICTURE_DIR + "familyboard_ic_delete.png";

        private static string FB_STICKERS_DIR = CommonResource.GetResourcePath() + "stickers/";
        private static string FB_STICKERS_EMOJI_500X500_DIR = FB_STICKERS_DIR + "emoji_500x500/";
        private static string FB_STICKERS_500X500_DIR = FB_STICKERS_DIR + "stickers_500x500/";

        public void Activate()
        {
            mRootView = new View();
            mRootView.BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);
            mRootView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT);

            NUIApplication.GetDefaultWindow().Add(mRootView);

            CreateCloseButton();

            CreateStickerList();
        }

        public void Reactivate()
        {
            // nothing...
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

            if (mTitleLabel != null)
            {
                mTitleView.Remove(mTitleLabel);
                mTitleLabel.Dispose();
                mTitleLabel = null;
            }

            if (mTitleView != null)
            {
                mStickersRootView.Remove(mTitleView);
                mTitleView.Dispose();
                mTitleView = null;
            }

            if (mStickersListView != null)
            {
                mStickersRootView.Remove(mStickersListView);
                mStickersListView.Dispose();
                mStickersListView = null;
            }

            if (mStickersRootView != null)
            {
                mRootView.Remove(mStickersRootView);
                mStickersRootView.Dispose();
                mStickersRootView = null;
            }

            if (mRootView != null)
            {
                NUIApplication.GetDefaultWindow().Remove(mRootView);
                mRootView.Dispose();
                mRootView = null;
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

        private void CreateStickerList()
        {
            // sticker root view
            mStickersRootView = new View();
            mStickersRootView.Position2D = new Position2D(0, 405);
            mStickersRootView.Size2D = new Size2D(SCREEN_WIDTH, 177 + (340 + 10) * 3 + 340);
            mStickersRootView.BackgroundColor = new Color(0.2f, 0.2f, 0.2f, 1.0f);
            mRootView.Add(mStickersRootView);

            // title
            mTitleView = new View();
            mTitleView.Position2D = new Position2D();
            mTitleView.Size2D = new Size2D(SCREEN_WIDTH, 177);
            mStickersRootView.Add(mTitleView);

            mTitleLabel = new TextLabel();
            mTitleLabel.Text = "Stickers";
            mTitleLabel.FontFamily = "SamsungOneUI 600";
            mTitleLabel.PointSize = 40;
            mTitleLabel.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.8f);
            mTitleLabel.PositionUsesPivotPoint = true;
            mTitleLabel.PivotPoint = PivotPoint.Center;
            mTitleLabel.ParentOrigin = ParentOrigin.Center;
            mTitleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            mTitleLabel.VerticalAlignment = VerticalAlignment.Center;
            mTitleView.Add(mTitleLabel);

            // grid
            mStickersListView = new FlexibleView();
            mStickersListView.Name = "Stickers List";
            mStickersListView.Position2D = new Position2D(0, 177);
            mStickersListView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT - 405 - 177);
            mStickersListView.Padding = new Extents(0, 0, 0, 0);
            mStickersRootView.Add(mStickersListView);

            List<StickersListItemData> dataList = new List<StickersListItemData>();

            string[] cartoon = new string[10];
            for(int i = 0; i < 10; i++)
            {
                if (i < 9)
                {
                    cartoon[i] = $"{FB_STICKERS_EMOJI_500X500_DIR}fb_emoji_500_0{i + 1}.png";
                }
                else
                {
                    cartoon[i] = $"{FB_STICKERS_EMOJI_500X500_DIR}fb_emoji_500_{i + 1}.png";
                }
            }
            dataList.Add(new StickersListItemData(cartoon));

            string[] texts = new string[10];
            for (int i = 0; i < 10; i++)
            {
                if (i < 9)
                {
                    texts[i] = $"{FB_STICKERS_500X500_DIR}fb_stickers_500_0{i + 1}.png";
                }
                else
                {
                    texts[i] = $"{FB_STICKERS_500X500_DIR}fb_stickers_500_{i + 1}.png";
                }
            }
            dataList.Add(new StickersListItemData(texts));

            string[] shapes = new string[5];
            for (int i = 0; i < 5; i++)
            {
                shapes[i] = $"{FB_STICKERS_500X500_DIR}fb_stickers_500_{i + 11}.png";
            }
            dataList.Add(new StickersListItemData(shapes));

            string[] shadows = new string[30];
            for (int i = 0; i < 30; i++)
            {
                shadows[i] = $"{FB_STICKERS_500X500_DIR}fb_stickers_500_{i + 16}.png";
            }
            dataList.Add(new StickersListItemData(shadows));

            mStickerListBridge = new StickersListBridge(dataList);
            mStickersListView.SetAdapter(mStickerListBridge);

            LinearLayoutManager layoutManager = new LinearLayoutManager(LinearLayoutManager.VERTICAL);
            mStickersListView.SetLayoutManager(layoutManager);
        }
    }

    internal class StickersListItemData
    {
        public StickersListItemData(string[] images)
        {
            Images = images;
        }

        public string[] Images
        {
            set;
            get;
        }
    }

    internal class StickerItemView : View
    {
        private View mRootView;
        private TableView mStickerTableView;
        private ImageView[] mStickerImageViews;
        private TapGestureDetector[] mTapGestureDetectors;
        private View mLineView;

        private readonly int SCREEN_WIDTH = 1080;

        public StickerItemView(string[] images, uint cols, ushort top, bool hasLine)
        {
            int horizontal_padding = (int)(SCREEN_WIDTH - ((178 + 26) * cols - 26));
            int imageCount = images.Length;
            uint rows = (uint)imageCount / cols;

            mRootView = new View();
            mRootView.Position2D = new Position2D(horizontal_padding / 2, top);
            mRootView.Size2D = new Size2D(SCREEN_WIDTH - horizontal_padding, (178 + 22) * (int)rows - 22 + 46 + 2);

            mStickerTableView = new TableView(rows, cols);
            mStickerTableView.Position2D = new Position2D(0, 0);
            mStickerTableView.Size2D = new Size2D(SCREEN_WIDTH - horizontal_padding, (178 + 22) * (int)rows - 22);
            mStickerTableView.PositionUsesPivotPoint = true;
            mStickerTableView.ParentOrigin = Tizen.NUI.ParentOrigin.TopLeft;
            mStickerTableView.PivotPoint = Tizen.NUI.PivotPoint.TopLeft;
            mRootView.Add(mStickerTableView);

            mStickerImageViews = new ImageView[imageCount];
            mTapGestureDetectors = new TapGestureDetector[imageCount];
            for (int i = 0; i < imageCount; i++)
            {
                mStickerImageViews[i] = new ImageView();
                mStickerImageViews[i].ResourceUrl = images[i];
                mStickerImageViews[i].Size2D = new Size2D(178, 178);
                mStickerImageViews[i].PositionUsesPivotPoint = true;
                mStickerImageViews[i].ParentOrigin = Tizen.NUI.ParentOrigin.TopLeft;
                mStickerImageViews[i].PivotPoint = Tizen.NUI.PivotPoint.TopLeft;

                mTapGestureDetectors[i] = new TapGestureDetector();
                mTapGestureDetectors[i].Attach(mStickerImageViews[i]);
                mTapGestureDetectors[i].Detected += OnStickerImageTapDetected;

                mStickerTableView.SetCellAlignment(new TableView.CellPosition((uint) i / cols, (uint)i % cols),
                    HorizontalAlignmentType.Center, VerticalAlignmentType.Center);
                mStickerTableView.AddChild(mStickerImageViews[i], new TableView.CellPosition((uint)i / cols, (uint)i % cols));
            }

            if (hasLine)
            {
                mLineView = new View();
                mLineView.Position2D = new Position2D(0, (178 + 22) * (int)rows - 22 + 46);
                mLineView.Size2D = new Size2D(SCREEN_WIDTH - horizontal_padding, 2);
                mLineView.BackgroundColor = new Color(1.0f, 1.0f, 1.0f, 0.1f);
                mRootView.Add(mLineView);
            }

            Add(mRootView);
        }

        private void OnStickerImageTapDetected(object source, TapGestureDetector.DetectedEventArgs e)
        {
            ImageView img = e.View as ImageView;
            if (img != null)
            {
                ImageManager.Instance.AddImage(img.ResourceUrl, "", "", ImageManager.ItemType.STICKER);
                FamilyBoardPage.Instance.RemoveView();
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
                for (int i = 0; i < mStickerImageViews.Length; i++)
                {
                    mStickerTableView.Remove(mStickerImageViews[i]);
                    mStickerImageViews[i].Dispose();
                    mStickerImageViews[i] = null;
                }
                mStickerImageViews = null;

                if (mStickerTableView != null)
                {
                    mRootView.Remove(mStickerTableView);
                    mStickerTableView.Dispose();
                    mStickerTableView = null;
                }

                if (mLineView != null)
                {
                    mRootView.Remove(mLineView);
                    mLineView.Dispose();
                    mLineView = null;
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

    internal class StickersListBridge : FlexibleViewAdapter
    {
        private List<StickersListItemData> mDatas;

        private readonly int SCREEN_WIDTH = 1080;

        public StickersListBridge(List<StickersListItemData> datas)
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
            StickersListItemData listItemData = mDatas[viewType];

            switch (viewType)
            {
                case 0:
                    {
                        item_view = new StickerItemView(listItemData.Images, 5, 0, true);
                        break;
                    }
                case 1:
                case 2:
                    {
                        item_view = new StickerItemView(listItemData.Images, 5, 46, true);
                        break;
                    }
                case 3:
                    {
                        item_view = new StickerItemView(listItemData.Images, 5, 46, false);
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
            StickersListItemData listItemData = mDatas[position];
            int horizontal_padding = 0; //SCREEN_WIDTH - ((178 + 26) * 5 - 26);

            switch (position)
            {
                case 0:
                    {
                        StickerItemView listItemView = holder.ItemView as StickerItemView;
                        listItemView.Name = "Item" + position;
                        listItemView.Size2D = new Size2D(SCREEN_WIDTH - horizontal_padding, 178 * 2 + 22 + 46 + 2);
                        listItemView.Margin = new Extents(0, 0, 0, 0);
                        break;
                    }
                case 1:
                    {
                        StickerItemView listItemView = holder.ItemView as StickerItemView;
                        listItemView.Name = "Item" + position;
                        listItemView.Size2D = new Size2D(SCREEN_WIDTH - horizontal_padding, 178 * 2 + 22 + 46 + 46 +2);
                        listItemView.Margin = new Extents(0, 0, 0, 0);
                        break;
                    }
                case 2:
                    {
                        StickerItemView listItemView = holder.ItemView as StickerItemView;
                        listItemView.Name = "Item" + position;
                        listItemView.Size2D = new Size2D(SCREEN_WIDTH - horizontal_padding, 178 + 46 + 46 + 2);
                        listItemView.Margin = new Extents(0, 0, 0, 0);
                        break;
                    }
                case 3:
                    {
                        StickerItemView listItemView = holder.ItemView as StickerItemView;
                        listItemView.Name = "Item" + position;
                        listItemView.Size2D = new Size2D(SCREEN_WIDTH - horizontal_padding, (178 + 22) * 6 - 22 + 46);
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
