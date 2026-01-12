using Robust.Client.Graphics;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface;
using Robust.Shared.Maths;
using Robust.Shared.Utility;
using Robust.Shared.Timing;

namespace Content.Client._ALTSpace.Lobby.Parallax
{
    /// <summary>
    /// Контрол для лобби, который полностью заменяет стандартный фон
    /// </summary>
    public sealed class LobbyParallaxControl : Control
    {
        [Dependency] private readonly IResourceCache _resourceCache = default!;

        // Пути к текстурам
        private const string StaticBackgroundPath = "/Textures/Parallaxes/SyndicateParallaxBG.png";
        private const string Layer1Path = "/Textures/Parallaxes/SyndicateParallaxNeb.png";
        private const string Layer2Path = "/Textures/Parallaxes/derbis.png";
        private const string Layer3Path = "/Textures/Parallaxes/graveyard.png";

        // Базовая скорость и множители для эффекта глубины
        private const float BaseSpeed = 40f;
        private const float Layer1SpeedMult = 1.0f;
        private const float Layer2SpeedMult = 0.6f;
        private const float Layer3SpeedMult = 1.4f;

        private Texture? _cachedStatic;
        private Texture? _cachedLayer1;
        private Texture? _cachedLayer2;
        private Texture? _cachedLayer3;

        private float _offset1;
        private float _offset2;
        private float _offset3;

        public Texture? Texture { get; set; }

        public LobbyParallaxControl()
        {
            IoCManager.InjectDependencies(this);
            MouseFilter = MouseFilterMode.Ignore;
        }

        protected override void FrameUpdate(FrameEventArgs args)
        {
            base.FrameUpdate(args);
            _offset1 += BaseSpeed * Layer1SpeedMult * args.DeltaSeconds;
            _offset2 += BaseSpeed * Layer2SpeedMult * args.DeltaSeconds;
            _offset3 += BaseSpeed * Layer3SpeedMult * args.DeltaSeconds;
        }

        protected override void Draw(DrawingHandleScreen handle)
        {
            base.Draw(handle);

            var area = new UIBox2(0, 0, Size.X, Size.Y);

            handle.DrawRect(area, Color.Black);
            _cachedStatic ??= LoadTexture(StaticBackgroundPath);
            if (_cachedStatic != null)
                DrawStaticBackground(handle, _cachedStatic);
            _cachedLayer2 ??= LoadTexture(Layer2Path);
            if (_cachedLayer2 != null)
                DrawScrollingLayer(handle, _cachedLayer2, _offset2);
            _cachedLayer1 ??= LoadTexture(Layer1Path);
            if (_cachedLayer1 != null)
                DrawScrollingLayer(handle, _cachedLayer1, _offset1);
            _cachedLayer3 ??= LoadTexture(Layer3Path);
            if (_cachedLayer3 != null)
                DrawScrollingLayer(handle, _cachedLayer3, _offset3);
        }

        /// <summary>
        /// Отрисовка фона с сохранением пропорций
        /// </summary>
        private void DrawStaticBackground(DrawingHandleScreen handle, Texture tex)
        {
            if (tex.Width == 0 || tex.Height == 0)
                return;

            var screenWidth = Size.X;
            var screenHeight = Size.Y;

            var scaleX = screenWidth / tex.Width;
            var scaleY = screenHeight / tex.Height;
            var scale = Math.Max(scaleX, scaleY);

            var drawWidth = tex.Width * scale;
            var drawHeight = tex.Height * scale;

            var offsetX = (screenWidth - drawWidth) / 2;
            var offsetY = (screenHeight - drawHeight) / 2;

            var destRect = new UIBox2(offsetX, offsetY, offsetX + drawWidth, offsetY + drawHeight);
            handle.DrawTextureRect(tex, destRect);
        }

        private void DrawScrollingLayer(DrawingHandleScreen handle, Texture tex, float offset)
        {
            if (tex.Height == 0) return;

            float scale = Size.Y / tex.Height;
            float drawWidth = tex.Width * scale;
            float drawHeight = Size.Y;

            if (drawWidth <= 0) return;

            float xOffset = offset % drawWidth;

            for (float x = xOffset - drawWidth; x < Size.X; x += drawWidth)
            {
                var rect = new UIBox2(x, 0, x + drawWidth, drawHeight);
                handle.DrawTextureRect(tex, rect);
            }
        }

        private Texture? LoadTexture(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            return _resourceCache.TryGetResource<TextureResource>(new ResPath(path), out var res)
                ? res.Texture
                : null;
        }
    }
}