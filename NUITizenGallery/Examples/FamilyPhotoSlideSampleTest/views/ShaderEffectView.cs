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
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using Tizen.NUI;
using Tizen.NUI.BaseComponents;

namespace NUIPhotoSlide
{
    internal class ShaderEffectView : View
    {
        private List<String> particleList;
        private List<TextureSet> textureList;
        private int imageIdx = 0;
        
        private float time = 0.0f;
        private Shader shader;
        private Animation ani1;
        private Animation ani2;

        [StructLayout(LayoutKind.Sequential)]
        public struct Vec2
        {
            float x;
            float y;
            public Vec2(float xIn, float yIn)
            {
                x = xIn;
                y = yIn;
            }
        }

        public struct TexturedQuadVertex
        {
            public Vec2 position;
            // public Vec2 textureCoordinates;
        };

        public static byte[] Struct2Bytes(TexturedQuadVertex[] obj)
        {
            int size = Marshal.SizeOf(obj);
            byte[] bytes = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);
            return bytes;
        }

        static readonly string VERTEX_SHADER =
          "attribute mediump vec2 aPosition;\n" +
          "uniform mediump mat4 uMvpMatrix;\n" +
          "uniform mediump vec3 uSize;\n" +
          "varying mediump vec2 vTexCoord;\n" +
          "void main()\n" +
          "{\n" +
          "    gl_Position = uMvpMatrix * vec4(aPosition * uSize.xy, 0.0, 1.0);\n" +
            "  vTexCoord = aPosition + vec2( 0.5 );\n" +
          "}\n";



        static readonly string FRAGMENT_SHADER =
        "precision mediump float;\n" +

        "uniform mediump float iY;\n" +
        "uniform mediump float iX;\n" +
        "uniform mediump float iWidth;\n" +
        "uniform mediump float iHeight;\n" +

        "uniform mediump float iTime;\n" +
        "varying mediump vec2 vTexCoord;\n" +
        "uniform lowp vec4 uColor;\n" +
        "uniform sampler2D sTexture;\n" +
        "void main()\n" +
        "{\n" +
            "lowp vec4 color = texture2D( sTexture, vTexCoord ) * uColor;\n" +
            "vec2 iResolution = vec2(iHeight, iWidth);\n" +
            "vec2 aspect = vec2(iResolution.x / iResolution.y, 1.0);\n" +
            "vec2 frag = vec2(gl_FragCoord.x-iY, gl_FragCoord.y-iX);\n" +
            "vec2 uv = frag / iResolution.xy;\n" +

            "vec2 origin = vec2(0.6 + 0.4 * sin( iTime * 0.2) , 0.5 + 0.5 * cos( iTime *0.13)) * aspect;\n" +
            "vec2 normal = normalize(vec2(1.0, 2.0 * abs(sin(iTime * 0.1))) * aspect);\n" +


            "vec3 col = color.rgb;\n" +
            "vec2 pt = uv * aspect - origin;\n" +
            "float side = dot(pt, normal);\n" +
            "if (side > 0.0)\n" +
            "{\n" +
              "col *= 0.3;\n" +
              "vec3 bgc = vec3(1.0, 1.0, 1.0);" +
              "float shadow = smoothstep(0.0, 0.05, side);\n" +
              //"col = mix(col * 0.6, bgc, shadow);\n" +
            "}\n" +
            "else\n" +
            "{\n" +
              "pt = (uv * aspect - 2.0 * side * normal) / aspect;\n" +
            "if (pt.x >= 0.0 && pt.x < 1.0 && pt.y >= 0.0 && pt.y < 1.0)\n" +
            "{\n" +

             "vec4 back = vec4(0.0, 0.0, 0.0, 1.0);\n" +
             "back.rgb = back.rgb* 0.25 + 0.75;\n" +
             "float shadow = smoothstep(0.0, 0.2, -side);\n" +
             "back.rgb = mix(back.rgb * 0.2, back.rgb, shadow);\n" +
             //"col = mix(col, back.rgb, back.a);\n" +
           "}\n" +
          "}\n" +
          "gl_FragColor = vec4(col, 1.0);\n" +
        "}\n";
        
        public ShaderEffectView(string resName, Size2D size, Position2D pos)
        {
            particleList = new List<String>();
            textureList = new List<TextureSet>();

            String FolderName = CommonResource.GetResourcePath() + "particle/";
            System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(FolderName);
            foreach (System.IO.FileInfo File in di.GetFiles())
            {
                if (File.Extension.ToLower().CompareTo(".png") == 0)
                {
                    String FileNameOnly = File.Name.Substring(0, File.Name.Length - 4);
                    String FullFileName = File.FullName;

                    particleList.Add(FullFileName);
                }
            }
            particleList.Sort();
            foreach(string str in particleList)
            {
                textureList.Add(CreateTextureSet(str));
            }

            /* Create Shader */
            shader = new Shader(VERTEX_SHADER, FRAGMENT_SHADER);

            /* Create Property buffer */
            PropertyMap vertexFormat = new PropertyMap();
            vertexFormat.Add("aPosition", new PropertyValue((int)PropertyType.Vector2));

            PropertyBuffer vertexBuffer = new PropertyBuffer(vertexFormat);
            vertexBuffer.SetData(RectangleDataPtr(), 4);

            /* Create geometry */
            Geometry geometry = new Geometry();
            geometry.AddVertexBuffer(vertexBuffer);
            geometry.SetType(Geometry.Type.TRIANGLE_STRIP);

            /* Create renderer */
            renderer = new Renderer(geometry, shader);
            renderer.BlendMode = 0;
            
            renderer.SetTextures(textureList[0]);

            this.Size2D = size;
            this.ParentOrigin = Tizen.NUI.ParentOrigin.TopLeft;
            this.PivotPoint = Tizen.NUI.PivotPoint.TopLeft;
            this.PositionUsesPivotPoint = true;
            this.BackgroundColor = Color.Black;
            this.Position2D = pos;

            time = (new Random()).Next(5)+5;

            shader.RegisterProperty("iTime", new PropertyValue(time));
            shader.RegisterProperty("iWidth", new PropertyValue((float)this.Size2D.Width));
            shader.RegisterProperty("iHeight", new PropertyValue((float)this.Size2D.Height));
            shader.RegisterProperty("iY", new PropertyValue((float)this.Position2D.Y));
            shader.RegisterProperty("iX", new PropertyValue((float)this.Position2D.X));
            
            this.AddRenderer(renderer);

            Thread t1 = new Thread(new ThreadStart(Run));
            t1.Start();

            //Timer timer = new Timer(33);
            //timer.Tick += Timer_Tick;
            //timer.Start();
        }

        public void Run()
        {
            while(true)
            {
                Tizen.Log.Error("PhotoSlide", "tick : " + imageIdx);
                renderer.SetTextures(textureList[imageIdx]);
                imageIdx++;
                if (imageIdx >= particleList.Count)
                    imageIdx = 0;
                Thread.Sleep(1000);

            }

        }
        Renderer renderer;
        /*
        private bool Timer_Tick(object source, Timer.TickEventArgs e)
        {

            Tizen.Log.Error("PhotoSlide", "tick : " + imageIdx);
            renderer.SetTextures(textureList[imageIdx]);
            imageIdx++;
            if (imageIdx >= particleList.Count)
                imageIdx = 0;

            return true;
        }
        */
        private void Ani1_Finished(object sender, EventArgs e)
        {
            ani2.Finished += Ani2_Finished;
            ani2.Play();
        }

        private void Ani2_Finished(object sender, EventArgs e)
        {
            ani1.Play();
        }


        private TextureSet CreateTextureSet(string path)
        {
            Tizen.Log.Error("PhotoSlide", "path : " + path);
            Texture texture = LoadStageFillingTexture(path); ;
            TextureSet textureSet = new TextureSet();
            textureSet.SetTexture(0, texture);
            return textureSet;
        }

        private IntPtr RectangleDataPtr()
        {
            TexturedQuadVertex vertex1 = new TexturedQuadVertex();
            TexturedQuadVertex vertex2 = new TexturedQuadVertex();
            TexturedQuadVertex vertex3 = new TexturedQuadVertex();
            TexturedQuadVertex vertex4 = new TexturedQuadVertex();
            vertex1.position = new Vec2(-0.5f, -0.5f);
            vertex2.position = new Vec2(-0.5f, 0.5f);
            vertex3.position = new Vec2(0.5f, -0.5f);
            vertex4.position = new Vec2(0.5f, 0.5f);

            TexturedQuadVertex[] texturedQuadVertexData = new TexturedQuadVertex[4] { vertex1, vertex2, vertex3, vertex4 };

            int lenght = Marshal.SizeOf(vertex1);
            IntPtr pA = Marshal.AllocHGlobal(lenght * 4);

            for (int i = 0; i < 4; i++)
            {
                Marshal.StructureToPtr(texturedQuadVertexData[i], pA + i * lenght, true);
            }

            return pA;
        }


        private Texture LoadStageFillingTexture(string filepath)
        {
            Size2D dimensions = new Size2D(Window.Instance.WindowSize.Width, Window.Instance.WindowSize.Height);
            PixelBuffer pb = ImageLoading.LoadImageFromFile(filepath, dimensions, FittingModeType.ScaleToFill);
            PixelData pd = PixelBuffer.Convert(pb);

            Texture texture = new Texture(TextureType.TEXTURE_2D, pd.GetPixelFormat(), pd.GetWidth(), pd.GetHeight());
            texture.Upload(pd);

            return texture;
        }
    }
}
