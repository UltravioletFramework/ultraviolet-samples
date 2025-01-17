using System;
using System.IO;
using Sample15_RenderTargetsAndBuffers.Assets;
using Sample15_RenderTargetsAndBuffers.Input;
using Ultraviolet;
using Ultraviolet.Audio;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.Core;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2;

namespace Sample15_RenderTargetsAndBuffers.Shared
{
    public interface IImageSaver
    {
        void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target, out Double messageOpacity, out String message);
    }

    public partial class Game : UltravioletApplicationAdapter
    {
        public Game(IUltravioletApplicationAdapterHost host)
            : base(host)
        { }

        protected override void OnConfiguring(UltravioletConfiguration configuration)
        {
            configuration.Plugins.Add(new OpenGLGraphicsPlugin());
            configuration.Plugins.Add(new BASSAudioPlugin());
        }

        protected override void OnInitialized()
        {
            LoadInputBindings();

            base.OnInitialized();
        }

        protected override void OnShutdown()
        {
            SaveInputBindings();

            base.OnShutdown();
        }

        protected override void OnLoadingContent()
        {
            this.content = ContentManager.Create("Content");
            LoadContentManifests();

            this.spriteBatch = SpriteBatch.Create();

            // A render target is composed of one or more render buffers. Each render buffer represents a particular
            // output channel from a pixel shader, such as color or depth.
            this.rbufferColor = Texture2D.CreateRenderBuffer(RenderBufferFormat.Color, 256, 256);
            this.rbufferDepth = Texture2D.CreateRenderBuffer(RenderBufferFormat.Depth16, 256, 256);
            this.rtarget = RenderTarget2D.Create(256, 256);

            // Render buffers must be explicitly attached to a render target before they can be used.
            this.rtarget.Attach(this.rbufferColor);
            this.rtarget.Attach(this.rbufferDepth);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            // ACTION: Save Image
            if (Ultraviolet.GetInput().GetActions().SaveImage.IsPressed() || (Ultraviolet.GetInput().GetPrimaryTouchDevice()?.WasTapped() ?? false))
            {
                content.Load<SoundEffect>(GlobalSoundEffectID.Shutter).Play();

                // The SurfaceSaver class contains platform-specific functionality needed to write image
                // data to streams. We can pass a render target directly to the SaveAsPng() or SaveAsJpg() methods.
                var saver = SurfaceSaver.Create();

                // The Android and iOS platforms have restrictions on where you can save files, so we'll just
                // save to the photo gallery on those devices. We'll use a partial method to implement
                // this platform-specific behavior.
                ((IImageSaver)Host).SaveImage(saver, rtarget, out confirmMsgOpacity, out confirmMsgText);

                // Alternatively, we could populate an array with the target's data using the GetData() method...
                //     var data = new Color[rtarget.Width * rtarget.Height];
                //     rtarget.GetData(data);
            }

            // ACTION: Exit Application
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Host.Exit();
            }

            // Fade out save confirmation message
            if (confirmMsgOpacity > 0)
                confirmMsgOpacity -= (1.0 / 4.0) * time.ElapsedTime.TotalSeconds;

            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            // We specify that we want to start drawing to a render target with the SetRenderTarget() method.
            Ultraviolet.GetGraphics().SetRenderTarget(rtarget);

            // IMPORTANT NOTE! 
            // When a render target is set for rendering, Ultraviolet will automatically clear it to a lovely shade of dark purple.
            // You can change this behavior by passing RenderTargetUsage.PreserveContents to the render target constructor.
            Ultraviolet.GetGraphics().Clear(Color.Black);

            var effect = content.Load<Effect>(GlobalEffectID.Noise);
            var effectTime = (Single)time.TotalTime.TotalSeconds * 0.1f;
            effect.Parameters["Time"].SetValue(effectTime);

            var blank = content.Load<Texture2D>(GlobalTextureID.Blank);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, effect);
            spriteBatch.Draw(blank, new RectangleF(0, 0, rtarget.Width, rtarget.Height), Color.White);
            spriteBatch.End();

            // When we finish drawing to a render target, we can revert to the compositor target by passing
            // null to the SetRenderTarget() method.
            Ultraviolet.GetGraphics().SetRenderTarget(null);
            Ultraviolet.GetGraphics().Clear(Color.CornflowerBlue);

            // IMPORTANT NOTE!
            // A render target (including its buffers) CANNOT BE BOUND FOR READING AND WRITING SIMULTANEOUSLY.
            // You MUST revert to a different render target before trying to draw your buffers!
            var compositor = Ultraviolet.GetPlatform().Windows.GetPrimary().Compositor;
            var compWidth = compositor.Width;
            var compHeight = compositor.Height;

            var font = content.Load<SpriteFont>(GlobalFontID.SegoeUI);

            spriteBatch.Begin(SpriteSortMode.Deferred, null);
            spriteBatch.Draw(rbufferColor, new Vector2(
                (compWidth - rbufferColor.Width) / 2,
                (compHeight - rbufferColor.Height) / 2), Color.White);

            var instruction = Ultraviolet.Platform == UltravioletPlatform.Android || Ultraviolet.Platform == UltravioletPlatform.iOS ?
                "Tap to save the image to the gallery" :
                "Press F1 to save the image to a file";

            spriteBatch.DrawString(font, instruction,
                new Vector2(8f, 8f), Color.White);
            spriteBatch.DrawString(font, confirmMsgText ?? String.Empty,
                new Vector2(8f, 8f + font.Regular.LineSpacing), Color.White * (Single)confirmMsgOpacity);

            spriteBatch.End();

            // IMPORTANT NOTE!
            // Remember, we can't be bound for both reading and writing. We're currently bound for reading,
            // so we need to remember to unbind our buffers before we write to them again in the next frame.
            // The UnbindTexture() method is provided for convenience; if you know which sampler your texture
            // is bound to, you can either unbind the sampler manually, or bind another texture to it using SetTexture().
            Ultraviolet.GetGraphics().UnbindTexture(rbufferColor);

            base.OnDrawing(time);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                if (this.rbufferDepth != null)
                    this.rbufferDepth.Dispose();

                if (this.rbufferColor != null)
                    this.rbufferColor.Dispose();

                if (this.rtarget != null)
                    this.rtarget.Dispose();

                if (this.content != null)
                    this.content.Dispose();

                if (this.spriteBatch != null)
                    this.spriteBatch.Dispose();
            }
            base.Dispose(disposing);
        }

        private String GetInputBindingsPath()
        {
            return Path.Combine(Host.GetRoamingApplicationSettingsDirectory(), "InputBindings.xml");
        }

        private void LoadInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Load(GetInputBindingsPath(), throwIfNotFound: false);
        }

        private void SaveInputBindings()
        {
            Ultraviolet.GetInput().GetActions().Save(GetInputBindingsPath());
        }

        private void LoadContentManifests()
        {
            var uvContent = Ultraviolet.GetContent();

            var contentManifestFiles = this.content.GetAssetFilePathsInDirectory("Manifests");
            uvContent.Manifests.Load(contentManifestFiles);

            Ultraviolet.GetContent().Manifests["Global"]["Textures"].PopulateAssetLibrary(typeof(GlobalTextureID));
            Ultraviolet.GetContent().Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
            Ultraviolet.GetContent().Manifests["Global"]["Effects"].PopulateAssetLibrary(typeof(GlobalEffectID));
            Ultraviolet.GetContent().Manifests["Global"]["SoundEffects"].PopulateAssetLibrary(typeof(GlobalSoundEffectID));
        }

        //partial void SaveImage(SurfaceSaver surfaceSaver, RenderTarget2D target);

        // Application resources
        private ContentManager content;
        private SpriteBatch spriteBatch;
        private RenderTarget2D rtarget;
        private RenderBuffer2D rbufferColor;
        private RenderBuffer2D rbufferDepth;

        // Save confirmation message state
        private Double confirmMsgOpacity;
        private String confirmMsgText;
    }
}
