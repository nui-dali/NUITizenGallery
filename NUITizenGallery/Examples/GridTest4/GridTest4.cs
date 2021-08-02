/*
 * Copyright(c) 2021 Samsung Electronics Co., Ltd.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System;
using Tizen.NUI;
using Tizen.NUI.Components;

namespace NUITizenGallery
{
    internal class GridTest4 : IExample
    {
        private Window window;
        private GridTest4Page page;
        public void Activate()
        {
            window = NUIApplication.GetDefaultWindow();
            page = new GridTest4Page();
            window.GetDefaultNavigator().Push(page);
        }

        public void Deactivate()
        {
            window.GetDefaultNavigator().Pop();
            page = null; ;
        }
    }
}

