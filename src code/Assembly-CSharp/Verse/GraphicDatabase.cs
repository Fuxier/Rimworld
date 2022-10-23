using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003CF RID: 975
	public static class GraphicDatabase
	{
		// Token: 0x06001BEB RID: 7147 RVA: 0x000AAF04 File Offset: 0x000A9104
		public static Graphic Get<T>(string path) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, ShaderDatabase.Cutout, Vector2.one, Color.white, Color.white, null, 0, null, null));
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x000AAF44 File Offset: 0x000A9144
		public static Graphic Get<T>(string path, Shader shader) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, 0, null, null));
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x000AAF80 File Offset: 0x000A9180
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, 0, null, null));
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x000AAFB4 File Offset: 0x000A91B4
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, renderQueue, null, null));
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x000AAFE8 File Offset: 0x000A91E8
		public static Graphic Get<T>(Texture2D texture, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), texture, shader, drawSize, color, Color.white, null, renderQueue, null, null));
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x000AB01C File Offset: 0x000A921C
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, null, 0, null, null));
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x000AB04C File Offset: 0x000A924C
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, string maskPath = null) where T : Graphic, new()
		{
			return GraphicDatabase.GetInner<T>(new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, data, 0, null, maskPath));
		}

		// Token: 0x06001BF2 RID: 7154 RVA: 0x000AB080 File Offset: 0x000A9280
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, string maskPath = null)
		{
			return GraphicDatabase.Get(graphicClass, path, shader, drawSize, color, colorTwo, null, null, maskPath);
		}

		// Token: 0x06001BF3 RID: 7155 RVA: 0x000AB0A0 File Offset: 0x000A92A0
		public static Graphic Get(Type graphicClass, Texture2D texture, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters, string maskPath = null)
		{
			return GraphicDatabase.Get(new GraphicRequest(graphicClass, texture, shader, drawSize, color, colorTwo, data, 0, shaderParameters, maskPath));
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x000AB0C8 File Offset: 0x000A92C8
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters, string maskPath = null)
		{
			return GraphicDatabase.Get(new GraphicRequest(graphicClass, path, shader, drawSize, color, colorTwo, data, 0, shaderParameters, maskPath));
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x000AB0F0 File Offset: 0x000A92F0
		private static Graphic Get(GraphicRequest req)
		{
			try
			{
				Func<GraphicRequest, Graphic> func;
				if (!GraphicDatabase.cachedGraphicGetters.TryGetValue(req.graphicClass, out func))
				{
					MethodInfo method = typeof(GraphicDatabase).GetMethod("GetInner", BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic).MakeGenericMethod(new Type[]
					{
						req.graphicClass
					});
					func = (Func<GraphicRequest, Graphic>)Delegate.CreateDelegate(typeof(Func<GraphicRequest, Graphic>), method);
					GraphicDatabase.cachedGraphicGetters.Add(req.graphicClass, func);
				}
				return func(req);
			}
			catch (Exception ex)
			{
				Log.Error(string.Concat(new object[]
				{
					"Exception getting ",
					req.graphicClass,
					" at ",
					req.path,
					": ",
					ex.ToString()
				}));
			}
			return BaseContent.BadGraphic;
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x000AB1CC File Offset: 0x000A93CC
		private static T GetInner<T>(GraphicRequest req) where T : Graphic, new()
		{
			req.color = req.color;
			req.colorTwo = req.colorTwo;
			req.renderQueue = ((req.renderQueue == 0 && req.graphicData != null) ? req.graphicData.renderQueue : req.renderQueue);
			Graphic graphic;
			if (!GraphicDatabase.allGraphics.TryGetValue(req, out graphic))
			{
				graphic = Activator.CreateInstance<T>();
				graphic.Init(req);
				GraphicDatabase.allGraphics.Add(req, graphic);
			}
			return (T)((object)graphic);
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x000AB264 File Offset: 0x000A9464
		public static void Clear()
		{
			GraphicDatabase.allGraphics.Clear();
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x000AB270 File Offset: 0x000A9470
		[DebugOutput("System", false)]
		public static void AllGraphicsLoaded()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("There are " + GraphicDatabase.allGraphics.Count + " graphics loaded.");
			int num = 0;
			foreach (Graphic graphic in GraphicDatabase.allGraphics.Values)
			{
				stringBuilder.AppendLine(num + " - " + graphic.ToString());
				if (num % 50 == 49)
				{
					Log.Message(stringBuilder.ToString());
					stringBuilder = new StringBuilder();
				}
				num++;
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x0400141E RID: 5150
		private static Dictionary<GraphicRequest, Graphic> allGraphics = new Dictionary<GraphicRequest, Graphic>();

		// Token: 0x0400141F RID: 5151
		private static Dictionary<Type, Func<GraphicRequest, Graphic>> cachedGraphicGetters = new Dictionary<Type, Func<GraphicRequest, Graphic>>();
	}
}
