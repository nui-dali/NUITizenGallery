/*
 * Copyright(c) 2022 Samsung Electronics Co., Ltd.
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
using Tizen.NUI;

namespace NUIPhotoSlide
{   
    internal class MapXYData
    {
        public static Position[] positions1 = new Position[6] { new Position(150, 830, 0), new Position(50, 450, 0),
                                                                new Position(50, 1300, 0), new Position(700, 600, 0),
                                                                new Position(650, 80, 0), new Position(630, 1250, 0) };

        public static Position[] positions2 = new Position[6] { new Position(390, 1030, 0), new Position(150, 450, 0),
                                                                new Position(100, 1500, 0), new Position(700, 860, 0),
                                                                new Position(670, 490, 0), new Position(550, 1300, 0) };
        
        public static Position[] positions3 = new Position[3] { new Position(230, 630, 0), new Position(550, 300, 0),
                                                                new Position(470, 1100, 0)};
    }
}
