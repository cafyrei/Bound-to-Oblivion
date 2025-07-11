using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Oblivion
{
    public class SpriteAnimation2D
    {

        private Dictionary<int, int> _rowFrameCounts;
        private int FrameWidth;
        private int FrameHeight;
        private int CurrentRow;
        private float FrameTime;
        private float _timer;
        private int _currentColumn;

        private bool IsLooping;

        // Frame Properties
        public int FrameWidthAccess { get => FrameWidth; }
        public int FrameHeightAccess { get => FrameHeight; }
        public int CurrentFrame { get => _currentColumn; }
        public int TotalFrames => _rowFrameCounts.ContainsKey(CurrentRow) ? _rowFrameCounts[CurrentRow] : 1;
        public float FrameTimeAccess {set => FrameTime = value; }

        public SpriteAnimation2D(SpriteAnimation2D other)
        {
            FrameWidth = other.FrameWidth;
            FrameHeight = other.FrameHeight;
            FrameTime = other.FrameTime;
            IsLooping = other.IsLooping;

            _rowFrameCounts = new Dictionary<int, int>(other._rowFrameCounts);
            CurrentRow = 0;
            _timer = 0f;
            _currentColumn = 0;
        }

        public SpriteAnimation2D(int frameWidth, int frameHeight, Dictionary<int, int> rowFrameCount, float frameTime, bool looping = true)
        {
            FrameWidth = frameWidth;
            FrameHeight = frameHeight;
            FrameTime = frameTime;
            IsLooping = looping;

            _rowFrameCounts = rowFrameCount;
            CurrentRow = 0;
        }

        public void Update(GameTime gameTime)
        {
            int totalFrameCountInRow = _rowFrameCounts.ContainsKey(CurrentRow) ? _rowFrameCounts[CurrentRow] : 1;
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer >= FrameTime)
            {
                _timer = 0f;
                _currentColumn++;

                if (_currentColumn >= totalFrameCountInRow)
                {
                    _currentColumn = IsLooping ? 0 : totalFrameCountInRow - 1;
                }
            }
        }
        public void Reset() => _currentColumn = 0;

        public void SetRow(int row)
        {
            if (row != CurrentRow)
            {
                CurrentRow = row;
                Reset();
            }
        }

        public Rectangle GetSourceRect()
        {
            return new Rectangle(_currentColumn * FrameWidth, CurrentRow * FrameHeight, FrameWidth, FrameHeight);
        }
    }
}
