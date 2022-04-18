

using Tizen.NUI.BaseComponents;

namespace Tizen.FH.FamilyBoard
{
    public class PictureWizard : ILifecycleObserver
    {
        private static PictureWizard instance = null;

        private PictureChooser mPictureChooser;
        private FrameStyleChooser mFrameStyleChooser;

        public static PictureWizard Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new PictureWizard();
                }
                return instance;
            }
        }

        private PictureWizard()
        {

        }

        public void Activate()
        {
            mPictureChooser = new PictureChooser();
            mPictureChooser.Activate();
        }

        public void Reactivate()
        {

        }

        public void Deactivate()
        {
            if (mPictureChooser != null)
            {
                mPictureChooser.Deactivate();
                mPictureChooser = null;
            }

            if (mFrameStyleChooser != null)
            {
                mFrameStyleChooser.Deactivate();
                mFrameStyleChooser = null;
            }
        }

        public void ShowFrame()
        {
            if (mFrameStyleChooser == null)
            {
                mFrameStyleChooser = new FrameStyleChooser();
                mFrameStyleChooser.Activate();

                // fade in animation
                mFrameStyleChooser.GetRootView().Show();
            }
        }

        public void Next()
        {
            if (mPictureChooser != null)
            {
                // fade out animation
                mPictureChooser.GetRootView().Hide();
            }

            if (mFrameStyleChooser == null)
            {
                mFrameStyleChooser = new FrameStyleChooser();
                mFrameStyleChooser.Activate();
            }
            else
            {
                mFrameStyleChooser.Reactivate();
                mFrameStyleChooser.GetRootView().Show();
            }
        }

        public void Back()
        {
            if (mFrameStyleChooser != null)
            {
                // fade out animation
                mFrameStyleChooser.GetRootView().Hide();
            }

            if (mPictureChooser != null)
            {
                // fade in animation
                mPictureChooser.Reactivate();
                mPictureChooser.GetRootView().Show();
            }
        }
    }
}
