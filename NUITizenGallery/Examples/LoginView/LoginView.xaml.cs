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
using Tizen.NUI;
using Tizen.NUI.BaseComponents;
using Tizen.NUI.Components;
using System.Text.RegularExpressions;
namespace NUITizenGallery
{
    public partial class LoginViewPage : ContentPage
    {
        public LoginViewPage()
        {
            InitializeComponent();

            setViewStyle(mainView);
            setLabelStyle(titleLabel);
            setEmailFieldStyle(emailField);
            setPasswordFieldStyle(pwdField);
            setCheckBoxStyle(checkBox);
        }

        private void setViewStyle(View view)
        {
            view.CornerRadius = 48.0f;
            view.Padding = new Extents(100, 100, 100, 100);
        }

        private void setLabelStyle(TextLabel label)
        {
            // FontClient.Instance.AddCustomFontDirectory(Tizen.Applications.Application.Current.DirectoryInfo.Resource);

            label.Ellipsis = false;
            label.MultiLine = true;
            label.TextColor = Color.Blue;
            label.HorizontalAlignment = HorizontalAlignment.Center;

            PropertyMap fontStyle = new PropertyMap();
            fontStyle.Add("width", new PropertyValue("condensed"));
            label.FontStyle = fontStyle;
        }

        private void setEmailFieldStyle(TextField field)
        {
            field.PointSize = 20.0f;
            field.MaxLength = 20;

            PropertyMap fontStyle = new PropertyMap();
            fontStyle.Add("weight", new PropertyValue("light"));
            field.FontStyle = fontStyle;

            InputMethod ime = new InputMethod();
            ime.PanelLayout = InputMethod.PanelLayoutType.Email;
            field.InputMethodSettings = ime.OutputMap;

            field.MaxLengthReached += (s, e) =>
            {
                Tizen.Log.Info("NUI", "Max length = " + e.TextField.MaxLength + "\n");
            };

            field.FocusGained += (s, e) =>
            {
                field.TextColor = Color.Blue;
                emailUnderline.BackgroundColor = Color.Blue;
            };

            field.FocusLost += (s, e) =>
            {
                field.TextColor = Color.Gray;
                emailUnderline.BackgroundColor = Color.Gray;
            };
        }

        private void setPasswordFieldStyle(TextField field)
        {
            field.PointSize = 20.0f;

            PropertyMap fontStyle = new PropertyMap();
            fontStyle.Add("weight", new PropertyValue("light"));
            field.FontStyle = fontStyle;

            PropertyMap pwdMap = new PropertyMap();
            pwdMap.Add(HiddenInputProperty.Mode, new PropertyValue((int)HiddenInputModeType.ShowLastCharacter));
            pwdMap.Add(HiddenInputProperty.ShowLastCharacterDuration, new PropertyValue(500));
            field.HiddenInputSettings = pwdMap;

            InputMethod ime = new InputMethod();
            ime.PanelLayout = InputMethod.PanelLayoutType.Password;
            field.InputMethodSettings = ime.OutputMap;

            field.TextChanged += (s, e) =>
            {
                string str = Regex.Replace(field.Text, @"[\D]", "");
                field.Text = str;
            };

            field.FocusGained += (s, e) =>
            {
                field.TextColor = Color.Blue;
                pwdUnderline.BackgroundColor = Color.Blue;
            };

            field.FocusLost += (s, e) =>
            {
                field.TextColor = Color.Gray;
                pwdUnderline.BackgroundColor = Color.Gray;
            };
        }

        private void setCheckBoxStyle(CheckBox check)
        {
            check.TextLabel.FontFamily = "BreezeSans";
            PropertyMap fontStyle = new PropertyMap();
            fontStyle.Add("weight", new PropertyValue("light"));
            check.TextLabel.FontStyle = fontStyle;
            check.TextLabel.PointSize = 15.0f;
        }
    }
}
