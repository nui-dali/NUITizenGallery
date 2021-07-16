using Tizen.NUI;
using Tizen.NUI.Components;
using Tizen.NUI.BaseComponents;

namespace NUITizenGallery
{
    public partial class SwitchTest1Page : ContentPage
    {
        public SwitchTest1Page()
        {
            InitializeComponent();
            
            SwitchComponent.SelectedChanged += (o, e) =>
            {
                SwitchState.Text = string.Format("Is Toggled: {0}", e.IsSelected.ToString());
            };

            ButtonStart.Clicked += (o, e) =>
            {
                SwitchViewLayout.LinearAlignment = LinearLayout.Alignment.Begin;
                SwitchViewLayout.RequestLayout();
            };

            ButtonCenter.Clicked += (o, e) =>
            {
                SwitchViewLayout.LinearAlignment = LinearLayout.Alignment.Center;
                SwitchViewLayout.RequestLayout();
            };

            ButtonEnd.Clicked += (o, e) =>
            {
                SwitchViewLayout.LinearAlignment = LinearLayout.Alignment.End;
                SwitchViewLayout.RequestLayout();
                SwitchViewLayout.RequestLayout();
            };

            ButtonExpand.Clicked += (o, e) => 
            {
                SwitchComponent.WidthResizePolicy = ResizePolicyType.FillToParent;
                SwitchViewLayout.RequestLayout();
            };

            ButtonFit.Clicked += (o, e) => 
            {
                SwitchComponent.WidthResizePolicy = ResizePolicyType.FitToChildren;
                SwitchViewLayout.RequestLayout();
            };
        }
        
        protected override void Dispose(DisposeTypes type)
        {
            if (Disposed)
            {
                return;
            }

            if (type == DisposeTypes.Explicit)
            {
                RemoveAllChildren(true);
            }

            base.Dispose(type);
        }

        private void RemoveAllChildren(bool dispose = false)
        {
            RecursiveRemoveChildren(this, dispose);
        }

        private void RecursiveRemoveChildren(View parent, bool dispose)
        {
            if (parent == null)
            {
                return;
            }

            int maxChild = (int)parent.ChildCount;
            for (int i = maxChild - 1; i >= 0; --i)
            {
                View child = parent.GetChildAt((uint)i);
                if (child == null)
                {
                    continue;
                }

                RecursiveRemoveChildren(child, dispose);
                parent.Remove(child);
                if (dispose)
                {
                    child.Dispose();
                }
            }
        }
    }
}