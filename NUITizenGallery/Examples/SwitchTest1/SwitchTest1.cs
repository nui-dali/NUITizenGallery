using System;
using Tizen.NUI;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class SwitchTest1 : IExample
    {
        private Window window;
        private SwitchTest1Page page;
        private Navigator navigator;
        public void Activate()
        {
            Console.WriteLine($"@@@ this.GetType().Name={this.GetType().Name}, Activate()");

            window = NUIApplication.GetDefaultWindow();
            page = new SwitchTest1Page();
            navigator = window.GetDefaultNavigator();
            navigator.Push(page);
        }
        public void Deactivate()
        {
            Console.WriteLine($"@@@ this.GetType().Name={this.GetType().Name}, Deactivate()");
            navigator.Pop();
            page = null;
        }
    }
}
