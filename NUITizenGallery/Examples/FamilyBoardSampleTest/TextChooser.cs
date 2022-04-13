
using System;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;

namespace Tizen.FH.FamilyBoard
{
    class TextChooser : ILifecycleObserver
    {
        private View mRootView;
        private TextEditor mTextEditor;

        // buttons
        private View mButtonsRootView;
        private Button mStyleButton;

        private ImageView mAlignmentImageView;

        private ImageView mColorButtonImageView;

        // done button
        private ImageView mDoneNavigationBgImageView;
        private ImageView mDoneNavigationImageView;
        private TextLabel mDoneNavigationText;
        private ImageView mBackNavigationBgImageView;
        private ImageView mBackNavigationImageView;

        // popup
        private View mPopupRootView;
        private TableView mColorTableView;
        private View[] mColorViews;
        private ImageView[] mColorImageViews;

        private int mCurrentColorIndex;
        private static Color[] COLOR_TABLE = {
            new Color((float)0xff / 0xff, (float)0xff / 0xff, (float)0xff / 0xff, 1.0f),
            new Color((float)0xa8 / 0xff, (float)0xa8 / 0xff, (float)0xa8 / 0xff, 1.0f),
            new Color((float)0x4f / 0xff, (float)0x4f / 0xff, (float)0x4f / 0xff, 1.0f),
            new Color((float)0x00 / 0xff, (float)0x00 / 0xff, (float)0x00 / 0xff, 1.0f),
            null,

            new Color((float)0x7b / 0xff, (float)0x4a / 0xff, (float)0x37 / 0xff, 1.0f),
            new Color((float)0xef / 0xff, (float)0x26 / 0xff, (float)0x26 / 0xff, 1.0f),
            new Color((float)0xf8 / 0xff, (float)0x5e / 0xff, (float)0x3a / 0xff, 1.0f),
            new Color((float)0xff / 0xff, (float)0x9b / 0xff, (float)0x1a / 0xff, 1.0f),
            new Color((float)0xfe / 0xff, (float)0xd5 / 0xff, (float)0x48 / 0xff, 1.0f),

            new Color((float)0xc6 / 0xff, (float)0xe2 / 0xff, (float)0x57 / 0xff, 1.0f),
            new Color((float)0x55 / 0xff, (float)0xce / 0xff, (float)0x59 / 0xff, 1.0f),
            new Color((float)0x5d / 0xff, (float)0xdc / 0xff, (float)0xc9 / 0xff, 1.0f),
            new Color((float)0x53 / 0xff, (float)0xab / 0xff, (float)0xe2 / 0xff, 1.0f),
            new Color((float)0x3a / 0xff, (float)0x4c / 0xff, (float)0xe8 / 0xff, 1.0f),

            new Color((float)0xe8 / 0xff, (float)0xb9 / 0xff, (float)0x81 / 0xff, 1.0f),
            new Color((float)0xb9 / 0xff, (float)0x77 / 0xff, (float)0x66 / 0xff, 1.0f),
            new Color((float)0xe7 / 0xff, (float)0x84 / 0xff, (float)0xb0 / 0xff, 1.0f),
            new Color((float)0xbe / 0xff, (float)0x84 / 0xff, (float)0xdf / 0xff, 1.0f),
            new Color((float)0x7b / 0xff, (float)0x51 / 0xff, (float)0xd8 / 0xff, 1.0f),
        };

        private static string FB_COLOR_PICKER_PRIVATE = CommonResource.GetResourcePath() + "private-images/";

        private static string[] COLOR_BG =
        {
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_0.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_4.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_8.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_12.png",
            FB_COLOR_PICKER_PRIVATE + "fb_color_pickup_popup_color_wheel.png",

            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_1.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_5.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_9.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_13.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_17.png",

            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_2.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_6.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_10.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_14.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_18.png",

            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_3.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_7.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_11.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_15.png",
            FB_COLOR_PICKER_PRIVATE + "fb_private_color_picker_19.png",
        };

        private readonly int SCREEN_WIDTH = 1080;
        private readonly int SCREEN_HEIGHT = 1920;

        private static string FB_TEXT_EDIT_STYLE = CommonResource.GetResourcePath() + "fb_text_edit_style.png";
        private static string FB_TEXT_EDIT_ALIGN_LEFT = CommonResource.GetResourcePath() + "fb_text_edit_align_left.png";
        private static string FB_TEXT_EDIT_ALIGN_CENTER = CommonResource.GetResourcePath() + "fb_text_edit_align_center.png";
        private static string FB_TEXT_EDIT_ALIGN_RIGHT = CommonResource.GetResourcePath() + "fb_text_edit_align_rignt.png";
        private static string FB_TEXT_EDIT_COLORPICKER = CommonResource.GetResourcePath() + "fb_text_edit_colorpicker.png";
        private static string FB_TEXT_EDIT_COLORPICKER_ORIGINAL = CommonResource.GetResourcePath() + "fb_text_edit_colorpicker_original_private.png";
        private static string FB_TEXT_EDIT_COLORPICKER_BG = CommonResource.GetResourcePath() + "fb_text_edit_colorpicker_bg_private.png";
        private static string FB_TEXT_EDIT_COLOR_MASK = CommonResource.GetResourcePath() + "fb_color_pickup_popup_color_mask.png";
        private static string FB_TEXT_EDIT_COLOR_WHITE_ORIGINAL = CommonResource.GetResourcePath() + "fb_color_pickup_popup_color_private.png";
        private static string FB_TEXT_EDIT_COLOR_WHITE_BG = CommonResource.GetResourcePath() + "fb_color_pickup_popup_color_bg_private.png";
        //private static string FB_TEXT_EDIT_COLOR_STROKE = CommonResource.GetResourcePath() + "fb_color_pickup_popup_color_stroke.png";
        private static string FB_TEXT_EDIT_COLOR_SEL = CommonResource.GetResourcePath() + "fb_color_pickup_popup_color_sel.png";
        private static string FB_COLOR_PICKUP_POPUP_COLOR_WHEEL = CommonResource.GetResourcePath() + "fb_color_pickup_popup_color_wheel.png";
        private static string FB_WRITE_MODE_COLOR_POPUP = CommonResource.GetResourcePath() + "fb_writemode_color_popup.png";

        private static string FB_NAVI_BUTTON_IMAGE_DIR = CommonResource.GetResourcePath() + "side-navi/";
        private static string FB_NAVI_DONE_BUTTON_BG = FB_NAVI_BUTTON_IMAGE_DIR + "sidenavi_bg_b.png";
        private static string FB_NAVI_DONE_BUTTON_SAVE_B = FB_NAVI_BUTTON_IMAGE_DIR + "sidenavi_btn_save_b.png";
        private static string FB_NAVI_DONE_BUTTON_SAVE_B_DIM = FB_NAVI_BUTTON_IMAGE_DIR + "sidenavi_btn_save_b_dim.png";
        private static string FB_NAVI_DONE_BUTTON_SAVE_B_PRESS = FB_NAVI_BUTTON_IMAGE_DIR + "sidenavi_btn_save_b_press.png";

        private static string FB_NAVI_BACK_BUTTON_BG = FB_NAVI_BUTTON_IMAGE_DIR + "sidenavi_back_bg_b.png";
        private static string FB_NAVI_BUTTON_BACK_B = FB_NAVI_BUTTON_IMAGE_DIR + "sidenavi_btn_back_b.png";
        private static string FB_NAVI_BUTTON_BACK_B_DIM = FB_NAVI_BUTTON_IMAGE_DIR + "sidenavi_btn_back_b_dim.png";
        private static string FB_NAVI_BUTTON_BACK_B_PRESS = FB_NAVI_BUTTON_IMAGE_DIR + "sidenavi_btn_back_b_press.png";

        public void Activate()
        {
            mRootView = new View();
            mRootView.BackgroundColor = new Color(0.0f, 0.0f, 0.0f, 0.7f);
            mRootView.Size2D = new Size2D(SCREEN_WIDTH, SCREEN_HEIGHT);

            NUIApplication.GetDefaultWindow().Add(mRootView);

            mCurrentColorIndex = 0; // white.

            CreateTextEditor();

            CreateButtons();

            CreateNavigationButton();

            // focus on editor.
            FocusManager.Instance.FocusIndicator = new View();
            FocusManager.Instance.SetCurrentFocusView(mTextEditor);
        }

        public void Reactivate()
        {

        }

        public void Deactivate()
        {
            if (mTextEditor != null)
            {
                mRootView.Remove(mTextEditor);
                mTextEditor.Dispose();
                mTextEditor = null;
            }

            if (mRootView != null)
            {
                NUIApplication.GetDefaultWindow().Remove(mRootView);
                mRootView.Dispose();
                mRootView = null;
            }
        }

        private void CreateTextEditor()
        {
            mTextEditor = new TextEditor();
            mTextEditor.Position2D = new Position2D(34, 210);
            mTextEditor.Size2D = new Size2D(SCREEN_WIDTH - 34 * 2, 570);
            mTextEditor.FontFamily = "SamsungOneUI 500";
            mTextEditor.PointSize = 100;
            mTextEditor.TextColor = COLOR_TABLE[mCurrentColorIndex];
            mTextEditor.CellHorizontalAlignment = HorizontalAlignmentType.Center;
            mTextEditor.CellVerticalAlignment = VerticalAlignmentType.Center;
            mTextEditor.EnableScrollBar = true;
            mTextEditor.HorizontalAlignment = HorizontalAlignment.Center;
            mTextEditor.Focusable = true;

            mTextEditor.FocusGained += OnTextEditorFocusGained;

            mRootView.Add(mTextEditor);
        }

        private void OnTextEditorFocusGained(object sender, EventArgs e)
        {
            mTextEditor.GetInputMethodContext().Activate();
            mTextEditor.GetInputMethodContext().ShowInputPanel();
        }

        private void CreateButtons()
        {
            mButtonsRootView = new View();
            mButtonsRootView.Position2D = new Position2D(0, 616 + 430);
            mButtonsRootView.Size2D = new Size2D(SCREEN_WIDTH, 64 + 34);
            mRootView.Add(mButtonsRootView);

            mStyleButton = new Button();
            mStyleButton.Text = "Style";
            mStyleButton.Style.BackgroundImage = FB_TEXT_EDIT_STYLE;
            mStyleButton.TextAlignment = HorizontalAlignment.Center;
            mStyleButton.TextColor = Color.White;
            mStyleButton.FontFamily = "SamsungOneUI 500";
            mStyleButton.PointSize = 30;
            mStyleButton.Position2D = new Position2D(329, 0);
            mStyleButton.Size2D = new Size2D(155, 64);
            mStyleButton.TouchEvent += MStyleButton_TouchEvent;
            mButtonsRootView.Add(mStyleButton);

            mAlignmentImageView = new ImageView();
            mAlignmentImageView.ResourceUrl = FB_TEXT_EDIT_ALIGN_CENTER;
            mAlignmentImageView.Position2D = new Position2D(329 + 155 + 55, 0);
            mAlignmentImageView.Size2D = new Size2D(64, 64);
            mButtonsRootView.Add(mAlignmentImageView);

            mAlignmentImageView.TouchEvent += OnAlignmentTouchEvent;

            ImageVisual buttonImageVisual = new ImageVisual()
            {
                URL = FB_TEXT_EDIT_COLORPICKER_ORIGINAL,
                MixColor = COLOR_TABLE[mCurrentColorIndex],
                AlphaMaskURL = FB_TEXT_EDIT_COLORPICKER,
            };
            mColorButtonImageView = new ImageView();
            mColorButtonImageView.BackgroundImage = FB_TEXT_EDIT_COLORPICKER_BG;
            mColorButtonImageView.Image = buttonImageVisual.OutputVisualMap;
            mColorButtonImageView.Position2D = new Position2D(329 + 155 + 55 + 64 + 59, 0);
            mColorButtonImageView.Size2D = new Size2D(64, 64);
            mButtonsRootView.Add(mColorButtonImageView);

            mColorButtonImageView.TouchEvent += OnColorTouchEvent;
        }

        private bool MStyleButton_TouchEvent(object source, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                if(mTextEditor.FontFamily == "SamsungOneUI 500")
                {
                    mStyleButton.FontFamily = "SamsungOneUI 300";
                    mTextEditor.FontFamily = "SamsungOneUI 300";
                }
                else
                {
                    mStyleButton.FontFamily = "SamsungOneUI 500";
                    mTextEditor.FontFamily = "SamsungOneUI 500";
                }

            }
            return false;
        }

        private void CreateNavigationButton()
        {
            mBackNavigationBgImageView = new ImageView();
            mBackNavigationBgImageView.ResourceUrl = FB_NAVI_BACK_BUTTON_BG;
            mBackNavigationBgImageView.Position2D = new Position2D(0, SCREEN_HEIGHT / 2 - 128 - 100);
            mBackNavigationBgImageView.TouchEvent += NavigationMoveTouchEvent;
            mRootView.Add(mBackNavigationBgImageView);

            mBackNavigationImageView = new ImageView();
            mBackNavigationImageView.ParentOrigin = ParentOrigin.Center;
            mBackNavigationImageView.Position2D = new Position2D(-40, -30);
            mBackNavigationImageView.ResourceUrl = FB_NAVI_BUTTON_BACK_B;
            mBackNavigationImageView.TouchEvent += BackNavigationTouchEvent;
            mBackNavigationBgImageView.Add(mBackNavigationImageView);

            mDoneNavigationBgImageView = new ImageView();
            mDoneNavigationBgImageView.ResourceUrl = FB_NAVI_DONE_BUTTON_BG;
            mDoneNavigationBgImageView.Position2D = new Position2D(SCREEN_WIDTH - 116 - 10, SCREEN_HEIGHT / 2 - 128 - 100);
            mDoneNavigationBgImageView.TouchEvent += NavigationMoveTouchEvent;
            mRootView.Add(mDoneNavigationBgImageView);

            mDoneNavigationImageView = new ImageView();
            mDoneNavigationImageView.ParentOrigin = ParentOrigin.TopLeft;
            mDoneNavigationImageView.Position2D = new Position2D(40, 30);
            mDoneNavigationImageView.ResourceUrl = FB_NAVI_DONE_BUTTON_SAVE_B;
            mDoneNavigationImageView.TouchEvent += DoneNavigationTouchEvent;
            mDoneNavigationBgImageView.Add(mDoneNavigationImageView);

            mDoneNavigationText = new TextLabel();
            mDoneNavigationText.Text = "Done";
            mDoneNavigationText.FontFamily = "SamsungOneUI 500C";
            mDoneNavigationText.PointSize = 18;
            mDoneNavigationText.TextColor = new Color(1.0f, 1.0f, 1.0f, 0.85f);
            mDoneNavigationText.ParentOrigin = ParentOrigin.TopLeft;
            mDoneNavigationText.Position2D = new Position2D(40, 30 + 56);
            mDoneNavigationText.Size2D = new Size2D(108-40, 40);
            mDoneNavigationBgImageView.Add(mDoneNavigationText);
        }

        private float lastY;

        private bool NavigationMoveTouchEvent(object source, View.TouchEventArgs e)
        {
            float current = 0.0f;
            if (e.Touch.GetState(0) == PointStateType.Down)
            {
                lastY = e.Touch.GetScreenPosition(0).Y;
            }

            if (e.Touch.GetState(0) == PointStateType.Motion)
            {
                current = e.Touch.GetScreenPosition(0).Y;
                int distance = (int)(current - lastY);
                mDoneNavigationBgImageView.Position2D.Y += distance;
                mBackNavigationBgImageView.Position2D.Y += distance;
                lastY = current;
            }

            return false;
        }

        private bool BackNavigationTouchEvent(object source, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                FamilyBoardPage.Instance.RemoveView();
            }
            return false;
        }

        private bool DoneNavigationTouchEvent(object source, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Up)
            {
                ImageManager.Instance.AddText(mTextEditor.Text, mTextEditor.TextColor,
                mTextEditor.FontFamily, mTextEditor.HorizontalAlignment);
                FamilyBoardPage.Instance.RemoveView();
            }
            return false;
        }

        private bool OnAlignmentTouchEvent(object source, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Down)
            {
                ImageView img = source as ImageView;
                if (mTextEditor.HorizontalAlignment == HorizontalAlignment.Center)
                {
                    img.ResourceUrl = FB_TEXT_EDIT_ALIGN_RIGHT;
                    mTextEditor.HorizontalAlignment = HorizontalAlignment.End;
                }
                else if (mTextEditor.HorizontalAlignment == HorizontalAlignment.End)
                {
                    img.ResourceUrl = FB_TEXT_EDIT_ALIGN_LEFT;
                    mTextEditor.HorizontalAlignment = HorizontalAlignment.Begin;
                }
                else
                {
                    img.ResourceUrl = FB_TEXT_EDIT_ALIGN_CENTER;
                    mTextEditor.HorizontalAlignment = HorizontalAlignment.Center;
                }
            }

            return false;
        }

        private bool OnColorTouchEvent(object source, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Down)
            {
                ImageView img = source as ImageView;
                CreateColorPopupIfNecessary();
                ChangeColorPopupVisibility();
            }

            return false;
        }

        private void CreateColorPopupIfNecessary()
        {
            if (mPopupRootView != null)
                return;

            mPopupRootView = new View();
            mPopupRootView.BackgroundImage = FB_WRITE_MODE_COLOR_POPUP;
            mPopupRootView.Position2D = new Position2D(329 + 155 + 55 + 64 + 59 + 64 / 2 - 390 / 2, 616 + 228 - 14 - 370 + 280);
            mPopupRootView.Size2D = new Size2D(389, 320);
            mRootView.Add(mPopupRootView);

            CreateColorTableView();

            mPopupRootView.Hide();
        }

        private void ChangeColorPopupVisibility()
        {
            if (mPopupRootView.Visibility)
            {
                mPopupRootView.Hide();
            }
            else
            {
                for (int i = 0; i < 20; i++)
                {
                    if (i == mCurrentColorIndex) // focus
                    {
                        mColorImageViews[i].BackgroundImage = FB_TEXT_EDIT_COLOR_SEL;
                        mColorImageViews[i].ResourceUrl = COLOR_BG[i];
                        mColorImageViews[i].AlphaMaskURL = FB_TEXT_EDIT_COLORPICKER;
                        mColorImageViews[i].Size2D = new Size2D(64, 64);

                        break;
                    }
                }

                mPopupRootView.Show();
            }
        }

        private void CreateColorTableView()
        {
            mColorTableView = new TableView(4, 5);
            mColorTableView.Position2D = new Position2D(41 - 5-25, 34-20);
            mColorTableView.Size2D = new Size2D(456 - 41 * 2 + 10, 370 - 34 - 54 + 10);
            mPopupRootView.Add(mColorTableView);

            mColorViews = new View[20];
            mColorImageViews = new ImageView[20];
            for (int i = 0; i < 20; i++)
            {
                mColorViews[i] = new View();
                mColorViews[i].Size2D = new Size2D(64, 64);

                mColorTableView.SetCellAlignment(new TableView.CellPosition((uint)i / 5, (uint)i % 5),
                    HorizontalAlignmentType.Center, VerticalAlignmentType.Center);
                mColorTableView.AddChild(mColorViews[i], new TableView.CellPosition((uint)i / 5, (uint)i % 5));

                mColorImageViews[i] = new ImageView();
                mColorImageViews[i].Size2D = new Size2D(54, 54);
                mColorImageViews[i].PositionUsesPivotPoint = true;
                mColorImageViews[i].ParentOrigin = ParentOrigin.Center;
                mColorImageViews[i].PivotPoint = PivotPoint.Center;
                mColorViews[i].Add(mColorImageViews[i]);

                mColorImageViews[i].ResourceUrl = COLOR_BG[i];
                mColorImageViews[i].AlphaMaskURL = FB_TEXT_EDIT_COLOR_MASK;
                mColorImageViews[i].TouchEvent += OnColorSelectionTouchEvent;
            }
        }

        private void DestroyColorTableView()
        {
            for (int i = 0; i < 20; i++)
            {
                mColorViews[i].Remove(mColorImageViews[i]);
                mColorImageViews[i].Dispose();
                mColorImageViews[i] = null;

                mColorTableView.Remove(mColorViews[i]);
                mColorViews[i].Dispose();
                mColorViews[i] = null;
            }

            mPopupRootView.Remove(mColorTableView);
            mColorTableView.Dispose();
            mColorTableView = null;
        }

        private bool OnColorSelectionTouchEvent(object source, View.TouchEventArgs e)
        {
            if (e.Touch.GetState(0) == PointStateType.Down)
            {
                ImageView img = source as ImageView;

                int i = 0;
                for (i = 0; i < 20; i++)
                {
                    if (mColorImageViews[i] == img)
                    {
                        break;
                    }
                }

                if (i < 20 && COLOR_TABLE[i] == null)
                {
                    Log.Fatal("FamilyBoard", "Currently it is not supported.");
                    return false;
                }

                //
                mPopupRootView.Hide();

                // lose focus of old color
                mColorImageViews[mCurrentColorIndex].ResourceUrl = COLOR_BG[mCurrentColorIndex];
                mColorImageViews[mCurrentColorIndex].AlphaMaskURL = FB_TEXT_EDIT_COLOR_MASK;
                mColorImageViews[mCurrentColorIndex].Size2D = new Size2D(54, 54);

                // save
                mCurrentColorIndex = i;

                // change button color.
                ImageVisual buttonImgVisual = new ImageVisual()
                {
                    URL = FB_TEXT_EDIT_COLORPICKER_ORIGINAL,
                    MixColor = COLOR_TABLE[mCurrentColorIndex],
                    AlphaMaskURL = FB_TEXT_EDIT_COLORPICKER,
                };
                mColorButtonImageView.Image = buttonImgVisual.OutputVisualMap;

                // change text color.
                mTextEditor.TextColor = COLOR_TABLE[mCurrentColorIndex];
            }

            return false;
        }
    }
}
