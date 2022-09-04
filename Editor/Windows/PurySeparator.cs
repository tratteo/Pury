using UnityEditor;
using UnityEngine;

namespace Pury.Editor
{
    public class PurySeparator
    {
        private GUIStyle style;

        public float Thickness { get; private set; }

        public RectOffset Margin { get; private set; }

        public float Length { get; private set; }

        public Color Color { get; private set; }

        public Orientation Direction { get; private set; }

        private PurySeparator(Orientation orientation)
        {
            Thickness = 1F;
            Color = new Color(0.5F, 0.5F, 0.5F, 1);
            Margin = new RectOffset();
            Direction = orientation;
        }

        public static Builder Towards(Orientation orientation) => new Builder(orientation);

        public void Draw()
        {
            switch (Direction)
            {
                case Orientation.Horizontal:
                    if (Length > 0)
                    {
                        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1, style, GUILayout.Height(Thickness), GUILayout.Width(Length)), Color);
                    }
                    else
                    {
                        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1, style, GUILayout.Height(Thickness), GUILayout.ExpandWidth(true)), Color);
                    }
                    break;

                case Orientation.Vertical:
                    if (Length > 0)
                    {
                        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1, style, GUILayout.Width(Thickness), GUILayout.Height(Length)), Color);
                    }
                    else
                    {
                        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1, style, GUILayout.Width(Thickness), GUILayout.ExpandHeight(true)), Color);
                    }
                    break;

                default:
                    break;
            }
        }

        public class Builder
        {
            private readonly PurySeparator separator;

            public Builder(Orientation orientation)
            {
                separator = new PurySeparator(orientation);
            }

            public static implicit operator PurySeparator(Builder builder) => builder.Build();

            public PurySeparator Build()
            {
                separator.style = new GUIStyle(GUIStyle.none)
                {
                    margin = separator.Margin
                };
                return separator;
            }

            public Builder Length(float length)
            {
                separator.Length = length;
                return this;
            }

            public Builder Colored(Color color)
            {
                separator.Color = color;
                return this;
            }

            public Builder Margin(RectOffset margin)
            {
                separator.Margin = margin;
                return this;
            }

            public Builder Thickness(float thickness)
            {
                separator.Thickness = thickness;
                return this;
            }
        }
    }
}