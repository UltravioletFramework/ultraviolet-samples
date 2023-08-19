using System;
using System.IO;
using Sample6_RenderingText.Assets;
using Sample6_RenderingText.Input;
using Ultraviolet;
using Ultraviolet.BASS;
using Ultraviolet.Content;
using Ultraviolet.Graphics;
using Ultraviolet.Graphics.Graphics2D;
using Ultraviolet.Graphics.Graphics2D.Text;
using Ultraviolet.OpenGL;
using Ultraviolet.SDL2;

namespace Sample6_RenderingText
{
    public class Game : UltravioletApplicationAdapter
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

            this.spriteFontGaramond = this.content.Load<SpriteFont>(GlobalFontID.Garamond);
            this.spriteFontSegoe = this.content.Load<SpriteFont>(GlobalFontID.SegoeUI);
            this.spriteBatch = SpriteBatch.Create();
            this.textRenderer = new TextRenderer();

            this.textRenderer.LayoutEngine.RegisterFont("segoe", this.spriteFontSegoe);
            this.textRenderer.LayoutEngine.RegisterFont("garamond", this.spriteFontGaramond);

            this.textRenderer.LayoutEngine.RegisterStyle("preset1", new TextStyle(spriteFontGaramond, true, null, Color.Lime));
            this.textRenderer.LayoutEngine.RegisterStyle("preset2", new TextStyle(spriteFontSegoe, null, true, Color.Red));

            var spriteOK = this.content.Load<Sprite>(GlobalSpriteID.OK);
            var spriteCancel = this.content.Load<Sprite>(GlobalSpriteID.Cancel);

            this.textRenderer.LayoutEngine.RegisterIcon("ok", spriteOK[0]);
            this.textRenderer.LayoutEngine.RegisterIcon("cancel", spriteCancel[0]);

            base.OnLoadingContent();
        }

        protected override void OnUpdating(UltravioletTime time)
        {
            if (Ultraviolet.GetInput().GetActions().ExitApplication.IsPressed())
            {
                Host.Exit();
            }

            base.OnUpdating(time);
        }

        protected override void OnDrawing(UltravioletTime time)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);

            DrawAlignedText();
            DrawColoredAndStyledText();

            spriteBatch.End();

            base.OnDrawing(time);
        }

        private void DrawAlignedText()
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var width = window.DrawableSize.Width;
            var height = window.DrawableSize.Height;

            var settingsTopLeft = new TextLayoutSettings(spriteFontSegoe, width, height, TextFlags.AlignTop | TextFlags.AlignLeft);
            textRenderer.Draw(spriteBatch, "Aligned top left", Vector2.Zero, Color.White, settingsTopLeft);

            var settingsTopCenter = new TextLayoutSettings(spriteFontSegoe, width, height, TextFlags.AlignTop | TextFlags.AlignCenter);
            textRenderer.Draw(spriteBatch, "Aligned top center", Vector2.Zero, Color.White, settingsTopCenter);

            var settingsTopRight = new TextLayoutSettings(spriteFontSegoe, width, height, TextFlags.AlignTop | TextFlags.AlignRight);
            textRenderer.Draw(spriteBatch, "Aligned top right", Vector2.Zero, Color.White, settingsTopRight);

            var settingsBottomLeft = new TextLayoutSettings(spriteFontSegoe, width, height, TextFlags.AlignBottom | TextFlags.AlignLeft);
            textRenderer.Draw(spriteBatch, "Aligned bottom left", Vector2.Zero, Color.White, settingsBottomLeft);

            var settingsBottomCenter = new TextLayoutSettings(spriteFontSegoe, width, height, TextFlags.AlignBottom | TextFlags.AlignCenter);
            textRenderer.Draw(spriteBatch, "Aligned bottom center", Vector2.Zero, Color.White, settingsBottomCenter);

            var settingsBottomRight = new TextLayoutSettings(spriteFontSegoe, width, height, TextFlags.AlignBottom | TextFlags.AlignRight);
            textRenderer.Draw(spriteBatch, "Aligned bottom right", Vector2.Zero, Color.White, settingsBottomRight);
        }

        private void DrawColoredAndStyledText()
        {
            var window = Ultraviolet.GetPlatform().Windows.GetPrimary();
            var width = window.DrawableSize.Width;
            var height = window.DrawableSize.Height;

            if (textLayoutCommands.Settings.Width != width || textLayoutCommands.Settings.Height != height)
            {
                const string text =
                    "Ultraviolet Formatting Commands\n" +
                    "\n" +
                    "||c:AARRGGBB| - Changes the color of text.\n" +
                    "|c:FFFF0000|red|c| |c:FFFF8000|orange|c| |c:FFFFFF00|yellow|c| |c:FF00FF00|green|c| |c:FF0000FF|blue|c| |c:FF6F00FF|indigo|c| |c:FFFF00FF|magenta|c|\n" +
                    "\n" +
                    "||font:name| - Changes the current font.\n" +
                    "We can |font:segoe|transition to a completely different font|font| within a single line\n" +
                    "\n" +
                    "||b| and ||i| - Changes the current font style.\n" +
                    "|b|bold|b| |i|italic|i| |b||i|bold italic|i||b|\n" +
                    "\n" +
                    "||style:name| - Changes to a preset style.\n" +
                    "|style:preset1|this is preset1|style| |style:preset2|this is preset2|style|\n" +
                    "\n" +
                    "||icon:name| - Draws an icon in the text.\n" +
                    "[|icon:ok| OK] [|icon:cancel| Cancel]";

                var settings = new TextLayoutSettings(spriteFontGaramond, width, height, TextFlags.AlignMiddle | TextFlags.AlignCenter);
                textRenderer.CalculateLayout(text, textLayoutCommands, settings);
            }

            textRenderer.Draw(spriteBatch, textLayoutCommands, Vector2.Zero, Color.White);
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
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

            Ultraviolet.GetContent().Manifests["Global"]["Fonts"].PopulateAssetLibrary(typeof(GlobalFontID));
            Ultraviolet.GetContent().Manifests["Global"]["Sprites"].PopulateAssetLibrary(typeof(GlobalSpriteID));
        }

        private ContentManager content;
        private SpriteFont spriteFontSegoe;
        private SpriteFont spriteFontGaramond;
        private SpriteBatch spriteBatch;
        private TextRenderer textRenderer;
        private readonly TextLayoutCommandStream textLayoutCommands = new TextLayoutCommandStream();
    }
}
