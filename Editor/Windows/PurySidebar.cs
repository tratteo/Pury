using System;
using UnityEngine;

namespace Pury.Editor
{
    public class PurySidebar
    {
        public Action<PurySidebar> drawJob;

        private GUIStyle cachedStyle;

        public float Width { get; private set; }

        public float Height { get; private set; }

        public Position Position { get; private set; }

        public RectOffset Margin { get; private set; }

        public RectOffset Padding { get; private set; }

        public string Style { get; private set; }

        private PurySidebar()
        {
            Style = string.Empty;
            Margin = new RectOffset();
            Padding = new RectOffset();
        }

        public static Builder Factory() => new Builder();

        public virtual void Draw()
        {
            var style = GetCachedStyle();
            switch (Position)
            {
                case Position.Top:
                    GUILayout.BeginHorizontal(style, GUILayout.ExpandWidth(true), GUILayout.Height(Height));
                    drawJob?.Invoke(this);
                    GUILayout.EndHorizontal();
                    break;

                case Position.Bottom:
                    GUILayout.BeginHorizontal(style, GUILayout.ExpandWidth(true), GUILayout.Height(Height));
                    drawJob?.Invoke(this);
                    GUILayout.EndHorizontal();
                    break;

                case Position.Left:
                    GUILayout.BeginVertical(style, GUILayout.ExpandHeight(true), GUILayout.Width(Width));
                    drawJob?.Invoke(this);
                    GUILayout.EndVertical();
                    break;

                case Position.Right:
                    GUILayout.BeginVertical(style, GUILayout.ExpandHeight(true), GUILayout.Width(Width));
                    drawJob?.Invoke(this);
                    GUILayout.EndVertical();
                    break;

                default:
                    break;
            }
        }

        private GUIStyle GetCachedStyle()
        {
            if (cachedStyle is not null) return cachedStyle;
            if (string.IsNullOrEmpty(Style))
            {
                cachedStyle = GUIStyle.none;
                cachedStyle.margin = Margin;
                cachedStyle.padding = Padding;
                return cachedStyle;
            }
            cachedStyle = new GUIStyle()
            {
                margin = Margin,
                padding = Padding
            };
            return cachedStyle;
        }

        public class Builder
        {
            private readonly PurySidebar feature;

            public Builder()
            {
                feature = new PurySidebar();
            }

            public static implicit operator PurySidebar(Builder builder) => builder.Build();

            public PurySidebar Build() => feature;

            public Builder Draw(Action<PurySidebar> drawJob)
            {
                feature.drawJob = drawJob;
                return this;
            }

            public Builder Left(float width = 0F)
            {
                feature.Position = Position.Left;
                feature.Width = width;
                return this;
            }

            public Builder Style(string style)
            {
                feature.Style = style;
                return this;
            }

            public Builder Margin(RectOffset margin)
            {
                feature.Margin = margin;
                return this;
            }

            public Builder Padding(RectOffset padding)
            {
                feature.Padding = padding;
                return this;
            }

            public Builder Right(float width = 0F)
            {
                feature.Position = Position.Right;
                feature.Width = width;
                return this;
            }

            public Builder Bottom(float height = 0F)
            {
                feature.Position = Position.Bottom;
                feature.Height = height;
                return this;
            }

            public Builder Top(float height = 0F)
            {
                feature.Position = Position.Top;
                feature.Height = height;
                return this;
            }
        }
    }
}