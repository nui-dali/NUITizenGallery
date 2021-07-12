using System;
using Tizen.NUI;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class WebViewTest2 : IExample
    {
        private Window window;
        private WebViewTest2Page page;
        public void Activate()
        {
            window = NUIApplication.GetDefaultWindow();
            page = new WebViewTest2Page();
            NUIApplication.GetDefaultWindow().GetDefaultNavigator().Push(page);
        }
        public void Deactivate()
        {
            page.Unparent();
            page.Dispose();
        }
    }
}

