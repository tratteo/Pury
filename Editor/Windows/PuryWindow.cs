using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Pury.Editor
{
    public abstract class PuryWindow : EditorWindow
    {
        private Dictionary<Position, List<PurySidebar>> sidebars;

        public Orientation SidebarPriority { get; protected set; }

        public Orientation ContentOrientation { get; protected set; }

        public string ContentStyle { get; protected set; }

        /// <summary>
        ///   Set properties and add <see cref="PurySidebar"/> to the window
        /// </summary>
        /// <param name="sidebars"> List where to append <see cref="PurySidebar"/> </param>
        protected virtual void Layout(List<PurySidebar> sidebars)
        { }

        /// <summary>
        ///   Draw the main content
        /// </summary>
        protected abstract void DrawContent();

        /// <summary>
        ///   <inheritdoc cref="VerticalGroup(Action, GUIStyle, GUILayoutOption[])"/>
        /// </summary>
        /// <param name="render"> </param>
        /// <param name="options"> </param>
        protected void VerticalGroup(Action render, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            render?.Invoke();
            GUILayout.EndVertical();
        }

        /// <summary>
        ///   Center the controls both horizontally and vertically in the current layout. Render the controls with the provided <see
        ///   cref="Orientation"/> layout
        /// </summary>
        /// <param name="render"> </param>
        /// <param name="controlsRenderOrientation"> </param>
        protected void CenterTotal(Action render, Orientation controlsRenderOrientation)
        {
            switch (controlsRenderOrientation)
            {
                case Orientation.Vertical:
                    Center(() => HorizontalGroup(() => Center(() => VerticalGroup(() => render?.Invoke()))));
                    break;

                case Orientation.Horizontal:
                    Center(() => VerticalGroup(() => Center(() => HorizontalGroup(() => render?.Invoke()))));
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///   <inheritdoc cref="HorizontalGroup(Action, GUIStyle, GUILayoutOption[])"/>
        /// </summary>
        /// <param name="render"> </param>
        /// <param name="options"> </param>
        protected void HorizontalGroup(Action render, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            render?.Invoke();
            GUILayout.EndHorizontal();
        }

        /// <summary>
        ///   Push the controls to the start of the layout and fill the remaining gap
        /// </summary>
        /// <param name="render"> </param>
        protected void GravitateStart(Action render)
        {
            render?.Invoke();
            GUILayout.FlexibleSpace();
        }

        /// <summary>
        ///   Push the controls to the end of the layout and fill the remaining gap
        /// </summary>
        /// <param name="render"> </param>
        protected void GravitateEnd(Action render)
        {
            GUILayout.FlexibleSpace();
            render?.Invoke();
        }

        /// <summary>
        ///   Center the controls in the current layout
        /// </summary>
        /// <param name="render"> </param>
        protected void Center(Action render)
        {
            GUILayout.FlexibleSpace();
            render?.Invoke();
            GUILayout.FlexibleSpace();
        }

        /// <summary>
        ///   Wrap controls inside a vertical group
        /// </summary>
        /// <param name="render"> </param>
        /// <param name="options"> </param>
        protected void VerticalGroup(Action render, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
            render?.Invoke();
            GUILayout.EndVertical();
        }

        /// <summary>
        ///   Wrap controls inside a horizontal group
        /// </summary>
        /// <param name="render"> </param>
        /// <param name="options"> </param>
        protected void HorizontalGroup(Action render, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
            render?.Invoke();
            GUILayout.EndHorizontal();
        }

        private void AddSidebar(PurySidebar feature)
        {
            if (sidebars.ContainsKey(feature.Position))
            {
                sidebars[feature.Position].Add(feature);
            }
            else
            {
                sidebars.Add(feature.Position, new List<PurySidebar>() { feature });
            }
        }

        private void OnEnable()
        {
            sidebars = new Dictionary<Position, List<PurySidebar>>();
            SidebarPriority = Orientation.Vertical;
            ContentOrientation = Orientation.Vertical;
            var bars = new List<PurySidebar>();
            Layout(bars);
            foreach (var b in bars) AddSidebar(b);
        }

        private void RenderSidebarsRelative(Position position)
        {
            if (sidebars.TryGetValue(position, out var bars))
            {
                foreach (var bar in bars)
                {
                    bar.Draw();
                }
            }
        }

        private void DrawContentContext()
        {
            var style = string.IsNullOrEmpty(ContentStyle) ? GUIStyle.none : ContentStyle;
            switch (ContentOrientation)
            {
                case Orientation.Vertical:
                    GUILayout.BeginVertical(style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    DrawContent();
                    GUILayout.EndVertical();
                    break;

                case Orientation.Horizontal:
                    GUILayout.BeginHorizontal(style, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    DrawContent();
                    GUILayout.EndHorizontal();
                    break;

                default:
                    break;
            }
        }

        private void OnGUI()
        {
            switch (SidebarPriority)
            {
                case Orientation.Horizontal:
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    RenderSidebarsRelative(Position.Top);
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    RenderSidebarsRelative(Position.Left);
                    DrawContentContext();
                    RenderSidebarsRelative(Position.Right);
                    GUILayout.EndHorizontal();
                    RenderSidebarsRelative(Position.Bottom);
                    GUILayout.EndVertical();
                    break;

                case Orientation.Vertical:
                    GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    RenderSidebarsRelative(Position.Left);
                    GUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                    RenderSidebarsRelative(Position.Top);
                    DrawContentContext();
                    RenderSidebarsRelative(Position.Bottom);
                    GUILayout.EndVertical();
                    RenderSidebarsRelative(Position.Right);
                    GUILayout.EndHorizontal();
                    break;

                default:
                    break;
            }
        }
    }
}