using System;
using Tizen.NUI;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class WebViewTest1 : IExample
    {
        private Window window;
        private WebViewTest1Page page;
        public void Activate()
        {
            window = NUIApplication.GetDefaultWindow();
            page = new WebViewTest1Page();
            NUIApplication.GetDefaultWindow().GetDefaultNavigator().Push(page);
        }
        public void Deactivate()
        {
            page.Unparent();
            page.Dispose();
        }
    }
}

