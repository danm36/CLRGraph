using clojure.lang;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CLRGraph
{
    public enum ProjectionType
    {
        RM_2D,
        RM_3D_Orth,
        RM_3D_Persp,
    }

    public class GLGraph : UserControl
    {
        public static GLGraph self; //For Clojure interop

        const float zNear = 0.01f;
        const float zFar = 2048.0f;
        const float FOV = 1.3333f;

        public GLControl glControl;
        bool GL_Loaded = false;
        bool GL_NeedRedraw = true;
        bool GL_SimpleRedraw = true;

        Matrix4 PVMMatrix, projectionMatrix, viewMatrix;
        Vector3 cameraLocation = new Vector3(0.0f, 1.0f, -6.0f), cameraFocus = new Vector3(0, 0, 0);
        float orthZoom = 0.01f;
        bool mouseDragsCamera = true;

        Shader axisShader = null;
        Shader microAxisShader = null;

        public static ProjectionType ProjectionType { get; private set; }

        Stopwatch fpsStopwatch = new Stopwatch();

        bool drawAxes = true, drawAxesInColor = true;
        float[] graphAxes;
        float[] graphMicroAxes = new float[] { 
            0, 0, 0, 1,
            1, 0, 0, 1,

            0, 0, 0, 2,
            0, 1, 0, 2,

            0, 0, 0, 3,
            0, 0, 1, 3,
        };
        int axisVBO = -1, microAxisVBO = -1;
        float axisPadding = 1;

        public GLGraph()
        {
            self = this;

            InitializeComponent();

            Application.Idle += (s, e) =>
            {
                if (GL_NeedRedraw)
                    glControl.Invalidate();
            };

            ProjectionType = ProjectionType.RM_2D;
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            if (DesignMode)
                return;

            glControl.VSync = true;

            GL_Loaded = true;

            GL.ClearColor(Color.White);
            GL.Enable(EnableCap.DepthTest);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.FrontFace(FrontFaceDirection.Cw);

            try
            {
                RecreateShaders();
            }
            catch
            {
                while(true);
            }

            axisVBO = GL.GenBuffer();
            microAxisVBO = GL.GenBuffer();

            UpdateGraphAxes();
            GL.BindBuffer(BufferTarget.ArrayBuffer, microAxisVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * graphMicroAxes.Length), graphMicroAxes, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Graph_Funcs.ResetGraph();
            ResetCamera();
        }

        private void InitializeComponent()
        {
            this.glControl = new OpenTK.GLControl(new GraphicsMode(32, 8, 8, 2));
            this.SuspendLayout();
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.Black;
            this.glControl.Location = new System.Drawing.Point(0, 0);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(4, 4);
            this.glControl.TabIndex = 0;
            this.glControl.VSync = false;
            this.glControl.Load += new System.EventHandler(this.glControl_Load);
            this.glControl.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl.MouseDown += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseDown);
            this.glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseMove);
            this.glControl.MouseUp += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseUp);
            this.glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_MouseWheel);
            // 
            // GLGraph
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.glControl);
            this.Name = "GLGraph";
            this.ResumeLayout(false);

        }

        public void SetBackgroundColor(Color newCol)
        {
            GL.ClearColor(newCol);

            Redraw();
        }

        public void RecreateShaders()
        {
            axisShader = new Shader("shaders/v_graphaxis.vert", "shaders/f_graphaxis.frag");
            microAxisShader = new Shader("shaders/v_graphmicroaxis.vert", "shaders/f_graphaxis.frag");

            Redraw();
        }

        public void SetProjectionType(ProjectionType newProjectionType)
        {
            ProjectionType = newProjectionType;

            UpdateMatrices(true);
            Redraw();
        }

        public void SetAxisDisplay(bool showAxis)
        {
            drawAxes = showAxis;
            Redraw();
        }

        public void SetAxisColor(bool showAxesInColor)
        {
            drawAxesInColor = showAxesInColor;
            Redraw();
        }

        public void UpdateGraphAxes()
        {
            graphAxes = new float[6 * 3 + 6];

            Box bb = new Box();
            for (int i = 0; i < DataSeries.AllDataSeries.Count; i++)
            {
                bb.Combine(DataSeries.AllDataSeries[i].GetDataExtents());
            }

            bb.MakeValid();

            graphAxes[3] = 1;
            graphAxes[7] = 1;
            graphAxes[11] = 2;
            graphAxes[15] = 2;
            graphAxes[19] = 3;
            graphAxes[23] = 3;

            graphAxes[0] = Math.Min(bb.minX == 0 ? bb.minX : bb.minX - axisPadding, 0); //X Min
            graphAxes[9] = Math.Min(bb.minY == 0 ? bb.minY : bb.minY - axisPadding, 0); //Y Min
            graphAxes[18] = Math.Min(bb.minZ == 0 ? bb.minZ : bb.minZ - axisPadding, 0); //Z Min

            graphAxes[4] = Math.Max(bb.maxX + axisPadding, 1); //X Max
            graphAxes[13] = Math.Max(bb.maxY + axisPadding, 1); //Y Max
            graphAxes[22] = Math.Max(bb.maxZ + axisPadding, 1); //Z Max

            GL.BindBuffer(BufferTarget.ArrayBuffer, axisVBO);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(sizeof(float) * graphAxes.Length), graphAxes, BufferUsageHint.StreamDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            Redraw();
        }

        public void UpdateMatrices(bool updateProjection = false)
        {
            if (updateProjection)
            {
                switch (ProjectionType)
                {
                    default:
                    case ProjectionType.RM_2D:
                    case ProjectionType.RM_3D_Orth:
                        projectionMatrix = Matrix4.CreateOrthographic((float)Width * orthZoom, (float)Height * orthZoom, -zFar, zFar);
                        break;
                    case ProjectionType.RM_3D_Persp:
                        projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(FOV, (float)Width / (float)Height, zNear, zFar);
                        break;
                }
            }

            if (ProjectionType == ProjectionType.RM_2D)
            {
                cameraLocation.Z = 6;
                cameraFocus = cameraLocation;
                cameraFocus.Z = 0;
            }

            cameraFocus.Y += 0.00001f; //Prevents weird NaN error

            viewMatrix = Matrix4.LookAt(cameraLocation, cameraFocus, new Vector3(0.0f, 1.0f, 0.0f));

            PVMMatrix = viewMatrix * projectionMatrix; //* model;

            for (int i = 0; i < Shader.loadedShaders.Count; i++)
            {
                GL.UseProgram(Shader.loadedShaders[i].shaderProgramHandle);
                GL.UniformMatrix4(Shader.loadedShaders[i].uPVMMatrixLocation, false, ref PVMMatrix);
            }

            //Handle rotation only matrices

            Matrix4 rotOnlyView, rotOnlyProjection;

            //if (false && ProjectionType == GLGraph_ProjectionType.RM_3D_Persp)
            //{
            //    rotOnlyProjection = Matrix4.CreatePerspectiveFieldOfView(FOV, (float)Width / (float)Height, zNear, zFar);
            //    rotOnlyView = Matrix4.CreateScale(0.125f) * viewMatrix.ClearTranslation() * Matrix4.CreateTranslation(0, 0, -1);
            //}
            //else
            //{
                rotOnlyView = Matrix4.CreateScale(0.25f) * viewMatrix.ClearTranslation();
                rotOnlyProjection = Matrix4.CreateOrthographic((float)Width * 0.01f, (float)Height * 0.01f, -zFar, zFar);
            //}

            Matrix4 rotPVMMatrix = rotOnlyView * rotOnlyProjection;

            GL.UseProgram(microAxisShader.shaderProgramHandle);
            GL.UniformMatrix4(microAxisShader.uPVMMatrixLocation, false, ref rotPVMMatrix);

            Redraw();
        }

        public void ResetCamera(bool focusOnly = false)
        {
            cameraFocus = new Vector3(0, 0, 0);

            if (!focusOnly)
            {
                cameraLocation = new Vector3(0, 0, 6.0f);
            }

            UpdateMatrices(true);
            Redraw();
        }

        public static void Redraw(bool simpleRedraw = false)
        {
            //self.Focus();
            self.GL_NeedRedraw = true;
            self.GL_SimpleRedraw = simpleRedraw;
        }

        protected override void OnResize(EventArgs e)
        {
            if (!DesignMode)
            {
                glControl.Dock = DockStyle.None;
                glControl.Dock = DockStyle.Fill;
            }

            if (!GL_Loaded)
                return;

            GL.Viewport(Size);
            UpdateMatrices(true);
            Redraw();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (!GL_Loaded)
                return;

            fpsStopwatch.Restart();

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            for (int i = 0; i < DataSeries.AllDataSeries.Count; i++)
                DataSeries.AllDataSeries[i].Draw(false);//GL_SimpleRedraw);

            if (drawAxes)
            {
                GL.UseProgram(axisShader.shaderProgramHandle);
                GL.Uniform1(axisShader.GetUniformLocation("uColorAxes"), drawAxesInColor ? 1 : 0);
                GL.LineWidth(1);
                GL.BindBuffer(BufferTarget.ArrayBuffer, axisVBO);
                GL.EnableVertexAttribArray(axisShader.vertexAttribLocation);
                GL.VertexAttribPointer(axisShader.vertexAttribLocation, 4, VertexAttribPointerType.Float, false, 0, 0);
                GL.DrawArrays(PrimitiveType.Lines, 0, graphAxes.Length);
                GL.DisableVertexAttribArray(axisShader.vertexAttribLocation);
            }

            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.UseProgram(microAxisShader.shaderProgramHandle);
            GL.LineWidth(2);
            GL.BindBuffer(BufferTarget.ArrayBuffer, microAxisVBO);
            GL.EnableVertexAttribArray(microAxisShader.vertexAttribLocation);
            GL.VertexAttribPointer(microAxisShader.vertexAttribLocation, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(PrimitiveType.Lines, 0, graphMicroAxes.Length);
            GL.DisableVertexAttribArray(microAxisShader.vertexAttribLocation);

            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            glControl.SwapBuffers();

            if (!GL_SimpleRedraw || mouseDown)
                GL_NeedRedraw = false;

            GL_SimpleRedraw = false;

            fpsStopwatch.Stop();
            CLRGraph_MainForm.self.FPSCounterLabel.Text = "Last FPS: " + (1.0 / fpsStopwatch.Elapsed.TotalSeconds);
        }

        #region Mouse Control
        bool mouseDown = false;
        Point mouseDownLocation, mouseLastLocation;
        private void glControl_MouseDown(object sender, MouseEventArgs e)
        {
            mouseDown = true;
            mouseDownLocation = mouseLastLocation = e.Location;
        }

        private void glControl_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;

            Redraw();
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;

            Vector3 delta = new Vector3(e.X - mouseLastLocation.X, -(e.Y - mouseLastLocation.Y), 0);
            if (mouseDragsCamera)
                delta *= -1;

            Vector3 deltaTransformed = Vector3.Transform(delta, viewMatrix.ExtractRotation().Inverted());

            float deltaMod = orthZoom;
            if (ProjectionType == ProjectionType.RM_3D_Persp)
                deltaMod = (float)Math.Max((cameraLocation - cameraFocus).LengthSquared / 4096.0, 0.05f);

            if (ProjectionType == ProjectionType.RM_2D || (ModifierKeys & Keys.Control) != 0)
            {
                if (ProjectionType == ProjectionType.RM_3D_Persp && e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    delta.Z = delta.Y;
                    delta.Y = 0;

                    deltaTransformed = Vector3.Transform(delta, viewMatrix.ExtractRotation().Inverted());
                }

                cameraLocation += deltaTransformed * deltaMod;
                cameraFocus += deltaTransformed * deltaMod;
            }
            else
            {
                deltaTransformed = (deltaTransformed * -1) / 32;

                float locFocDist = (cameraLocation - cameraFocus).Length;
                Vector3 direction = (cameraLocation - cameraFocus).Normalized();
                direction += deltaTransformed / 4;

                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                    cameraFocus = cameraLocation + (direction * -1) * locFocDist;
                else
                    cameraLocation = cameraFocus + direction * locFocDist;
            }
            UpdateMatrices();

            mouseLastLocation = e.Location;

            Redraw(true);
        }

        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {

            if (ProjectionType == ProjectionType.RM_2D || ProjectionType == ProjectionType.RM_3D_Orth)
            {
                orthZoom = Math.Max(Math.Min(orthZoom - (float)e.Delta / 12000, 4), 0.01f);

                UpdateMatrices(true);
            }
            else
            {
                float locFocDist = (cameraLocation - cameraFocus).Length;

                locFocDist = Math.Max(locFocDist - (float)e.Delta / 120 / (1 / (locFocDist / 50)), 0.01f);

                Vector3 direction = (cameraLocation - cameraFocus).Normalized();
                cameraLocation = cameraFocus + direction * locFocDist;

                /*Vector3 delta = new Vector3(0, 0, (float)-e.Delta / 240);
                delta = Vector3.Transform(delta, viewMatrix.ExtractRotation().Inverted());

                cameraLocation += delta;
                //cameraFocus += delta;*/

                UpdateMatrices();
            }

            Redraw(true);
        }
        #endregion
    }
}
