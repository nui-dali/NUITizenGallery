
using System.Collections.Generic;
using Tizen.NUI;

namespace Tizen.FH.FamilyBoard
{
    class ImageManager
    {
        private static ImageManager instance = null;

        // photo
        private List<string> mPhotoList = new List<string>();

        // selected picture, frame style
        private List<ImageDataItem> mImageList = new List<ImageDataItem>();

        // text
        private TextDataItem mTextDataItem = new TextDataItem();

        // background images
        private string mBackgroundImage = null;

        public static ImageManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ImageManager();
                }
                return instance;
            }
        }

        public enum ItemType
        {
            PHOTO = 0,
            STICKER,
        }

        public void AddImage(string file, string thumb, string style, ItemType type)
        {
            ImageDataItem item = new ImageDataItem();
            item.Index = mImageList.Count + 1;
            item.FileName = file;
            item.ThumbFileName = thumb;
            item.FrameStyle = style;
            item.DataItemType = type;
            mImageList.Add(item);
        }

        public void RemoveImage(string file)
        {
            List<ImageDataItem> aliveList = new List<ImageDataItem>();

            for (int i = 0; i < mImageList.Count; i++)
            {
                if (mImageList[i].FileName.Equals(file))
                {
                    continue;
                }
                aliveList.Add(mImageList[i]);
            }

            mImageList.Clear();

            for (int i = 0; i < aliveList.Count; i++)
            {
                ImageDataItem item = aliveList[i];
                item.Index = i + 1;
                mImageList.Add(item);
            }
        }

        public void RemoveAll()
        {
            mImageList.Clear();

            mTextDataItem.Content = "";
        }

        public int GetImageIndex(string file)
        {
            for (int i = 0; i < mImageList.Count; i++)
            {
                if (mImageList[i].FileName.Equals(file))
                {
                    return mImageList[i].Index;
                }
            }

            return 0;
        }

        public void SetFrameStyle(int index, string style)
        {
            mImageList[index].FrameStyle = style;
        }

        public void SetBackgroundImage(string image)
        {
            mBackgroundImage = image;
        }


        public string GetBackgroundImage()
        {
            return mBackgroundImage;
        }

        public int ImageCount()
        {
            return mImageList.Count;
        }

        public string GetFrameStyle(int index)
        {
            return mImageList[index].FrameStyle;
        }

        public string GetImageFile(int index)
        {
            return mImageList[index].FileName;
        }
        public string GetThumbFile(int index)
        {
            return mImageList[index].ThumbFileName;
        }

        public ItemType GetDataItemType(int index)
        {
            return mImageList[index].DataItemType;
        }

        public void AddText(string text, Color color, string style, HorizontalAlignment alignment)
        {
            mTextDataItem.Content = text;
            mTextDataItem.Style = style;
            mTextDataItem.TextColor = color;
            mTextDataItem.horizontalAlignment = alignment;
        }

        public string GetContent()
        {
            return mTextDataItem.Content;
        }

        public Color GetTextColor()
        {
            return mTextDataItem.TextColor;
        }
        public HorizontalAlignment GetHorizontalAlignment()
        {
            return mTextDataItem.horizontalAlignment;
        }

        public string GetTextStyle()
        {
            return mTextDataItem.Style;
        }

        private class TextDataItem
        {
            public string Style
            {
                get;
                set;
            }

            public string Content
            {
                get;
                set;
            }

            public Color TextColor
            {
                get;
                set;
            }

            public HorizontalAlignment horizontalAlignment
            {
                get;
                set;
            }
        }

        private class ImageDataItem
        {
            public int Index
            {
                get;
                set;
            }

            public string FileName
            {
                get;
                set;
            }

            public string ThumbFileName
            {
                get;
                set;
            }

            public string FrameStyle
            {
                get;
                set;
            }

            public ItemType DataItemType
            {
                get;
                set;
            }
        }

        private ImageManager()
        {
            
        }
    }
}
