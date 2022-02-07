using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
	public static class UtilsExtensions
	{
		#region Invoke

		private static string GetMethodName(Expression<Action> expr)
		{
			return ((MethodCallExpression)expr.Body).Method.Name;
		}

		public static void Invoke(this MonoBehaviour monoBehaviour, Expression<Action> expr, float time)
		{
			if (((MethodCallExpression) expr.Body).Arguments.Count > 0)
				throw new ArgumentException($"Can't invoke method '{GetMethodName(expr)}' with parameters.'");

			monoBehaviour.Invoke(GetMethodName(expr), time);
		}

		public static bool IsInvoking(this MonoBehaviour monoBehaviour, Expression<Action> expr)
		{
			return monoBehaviour.IsInvoking(GetMethodName(expr));
		}

		public static void CancelInvoke(this MonoBehaviour monoBehaviour, Expression<Action> expr)
		{
			monoBehaviour.CancelInvoke(GetMethodName(expr));
		}

		public static void InvokeRepeating(this MonoBehaviour monoBehaviour, Expression<Action> expr, float time, float repeatRate)
		{
			monoBehaviour.InvokeRepeating(GetMethodName(expr), time, repeatRate);
		}

		#endregion

		#region Vector2

		public static Vector2 MakePixelPerfect(this Vector2 position)
		{
			return new Vector2((int)position.x, (int)position.y);
		}

		public static Vector2 Rotate(this Vector2 vector, float angle)
		{
			return Quaternion.AngleAxis(angle, Vector3.forward) * vector;
		}

		public static float Angle(this Vector2 direction)
		{
			return direction.y > 0
					   ? Vector2.Angle(new Vector2(1, 0), direction)
					   : -Vector2.Angle(new Vector2(1, 0), direction);
		}
		
		#endregion

		#region Color

		public static Color SetAlpha(this Color color, float alpha)
		{
			return new Color(color.r, color.g, color.b, alpha);
		}

		public static void SetAlpha(this Graphic graphic, float alpha)
		{
			graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
		}

		public static void SetAlpha(this SpriteRenderer spriteRenderer, float alpha)
		{
			spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
		}

		#endregion

		#region Transform.position

		public static void SetXYZ(this Transform transform, float x, float y, float z)
		{
			transform.position = new Vector3(x, y, z);
		}

		public static void SetLocalXYZ(this Transform transform, float x, float y, float z)
		{
			transform.localPosition = new Vector3(x, y, z);
		}

		public static void SetXY(this Transform transform, float x, float y)
		{
			transform.position = new Vector3(x, y, transform.position.z);
		}

		public static void SetXY(this Transform transform, Vector3 pos)
		{
			transform.position = new Vector3(pos.x, pos.y, transform.position.z);
		}

		public static void SetLocalXY(this Transform transform, float x, float y)
		{
			transform.localPosition = new Vector3(x, y, transform.localPosition.z);
		}

		public static void SetLocalXY(this Transform transform, Vector3 pos)
		{
			transform.localPosition = new Vector3(pos.x, pos.y, transform.localPosition.z);
		}

		public static void SetXZ(this Transform transform, float x, float z)
		{
			transform.position = new Vector3(x, transform.position.y, z);
		}

		public static void SetXZ(this Transform transform, Vector3 pos)
		{
			transform.position = new Vector3(pos.x, transform.position.y, pos.z);
		}

		public static void SetLocalXZ(this Transform transform, float x, float z)
		{
			transform.localPosition = new Vector3(x, transform.localPosition.y, z);
		}

		public static void SetLocalXZ(this Transform transform, Vector3 pos)
		{
			transform.localPosition = new Vector3(pos.x, transform.localPosition.y, pos.z);
		}

		public static void SetYZ(this Transform transform, float y, float z)
		{
			transform.position = new Vector3(transform.position.x, y, z);
		}

		public static void SetYZ(this Transform transform, Vector3 pos)
		{
			transform.position = new Vector3(transform.position.x, pos.y, pos.z);
		}

		public static void SetLocalYZ(this Transform transform, float y, float z)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, y, z);
		}

		public static void SetLocalYZ(this Transform transform, Vector3 pos)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, pos.y, pos.z);
		}

		public static void SetX(this Transform transform, float x)
		{
			transform.position = new Vector3(x, transform.position.y, transform.position.z);
		}

		public static void SetLocalX(this Transform transform, float x)
		{
			transform.localPosition = new Vector3(x, transform.localPosition.y, transform.localPosition.z);
		}

		public static void SetY(this Transform transform, float y)
		{
			transform.position = new Vector3(transform.position.x, y, transform.position.z);
		}

		public static void SetLocalY(this Transform transform, float y)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, y, transform.localPosition.z);
		}

		public static void SetZ(this Transform transform, float z)
		{
			transform.position = new Vector3(transform.position.x, transform.position.y, z);
		}

		public static void SetLocalZ(this Transform transform, float z)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, z);
		}

		public static void IncX(this Transform transform, float dx)
		{
			SetX(transform, transform.position.x + dx);
		}

		public static void IncLocalX(this Transform transform, float dx)
		{
			SetLocalX(transform, transform.localPosition.x + dx);
		}

		public static void IncY(this Transform transform, float dy)
		{
			SetY(transform, transform.position.y + dy);
		}

		public static void IncLocalY(this Transform transform, float dy)
		{
			SetLocalY(transform, transform.localPosition.y + dy);
		}

		public static void IncZ(this Transform transform, float dz)
		{
			SetZ(transform, transform.position.z + dz);
		}

		public static void IncLocalZ(this Transform transform, float dz)
		{
			SetLocalZ(transform, transform.localPosition.z + dz);
		}

		#endregion

		#region Transform.localScale

		public static void SetScale(this Transform transform, float scale)
		{
			transform.localScale = Vector3.one * scale;
		}

		public static void SetScaleX(this Transform transform, float scaleX)
		{
			transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
		}

		public static void SetScaleY(this Transform transform, float scaleY)
		{
			transform.localScale = new Vector3(transform.localScale.x, scaleY, transform.localScale.z);
		}

		public static void SetScaleZ(this Transform transform, float scaleZ)
		{
			transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, scaleZ);
		}

		#endregion

		#region Transform.rotation

		public static void SetRotation(this Transform transform, float angle)
		{
			transform.rotation = new Quaternion();
			transform.Rotate(Vector3.forward, angle);
		}

		#endregion

		#region RectTransform

		#region Left, Right, Top, Bottom

		public static void SetLeft(this RectTransform rectTransform, float left)
		{
			rectTransform.offsetMin = new Vector2(left, rectTransform.offsetMin.y);
		}

		public static void SetRight(this RectTransform rectTransform, float right)
		{
			rectTransform.offsetMax = new Vector2(-right, rectTransform.offsetMax.y);
		}

		public static void SetTop(this RectTransform rectTransform, float top)
		{
			rectTransform.offsetMax = new Vector2(rectTransform.offsetMax.x, -top);
		}

		public static void SetBottom(this RectTransform rectTransform, float bottom)
		{
			rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, bottom);
		}

		public static void SetLeftTopRightBottom(this RectTransform rectTransform, float left, float top, float right, float bottom)
		{
			rectTransform.offsetMin = new Vector2(left, bottom);
			rectTransform.offsetMax = new Vector2(-right, -top);
		}

		#endregion

		#region PosX, PosY, Width, Height

		public static void SetPosX(this RectTransform rectTransform, float posX)
		{
			rectTransform.anchoredPosition = new Vector2(posX, rectTransform.anchoredPosition.y);
		}

		public static void SetPosY(this RectTransform rectTransform, float posY)
		{
			rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, posY);
		}

		public static void SetPosXY(this RectTransform rectTransform, float posX, float posY)
		{
			rectTransform.anchoredPosition = new Vector2(posX, posY);
		}

		public static void SetWidth(this RectTransform rectTransform, float width)
		{
			rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
		}

		public static float GetWidth(this RectTransform rectTransform)
		{
			return rectTransform.rect.width;
		}

		public static void SetHeight(this RectTransform rectTransform, float height)
		{
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
		}

		public static float GetHeight(this RectTransform rectTransform)
		{
			return rectTransform.rect.height;
		}

		public static void SetWidthHeight(this RectTransform rectTransform, float width, float height)
		{
			rectTransform.sizeDelta = new Vector2(width, height);
		}

		public static void SetPosAndSize(this RectTransform rectTransform, float posX, float posY, float width, float height)
		{
			rectTransform.anchoredPosition = new Vector2(posX, posY);
			rectTransform.sizeDelta = new Vector2(width, height);
		}

		#endregion
		
		#region Anchor Offset
		
		public static void SetLeftAnchorOffset(this RectTransform rectTransform, float leftPercent)
		{
			rectTransform.anchorMin = new Vector2(leftPercent, rectTransform.anchorMin.y);
		}

		public static void SetRightAnchorOffset(this RectTransform rectTransform, float rightPercent)
		{
			rectTransform.anchorMax = new Vector2(1f - rightPercent, rectTransform.anchorMax.y);
		}

		public static void SetTopAnchorOffset(this RectTransform rectTransform, float topPercent)
		{
			rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, 1f - topPercent);
		}

		public static void SetBottomAnchorOffset(this RectTransform rectTransform, float bottomPercent)
		{
			rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, bottomPercent);
		}

		public static void SetAnchorOffset(this RectTransform rectTransform, float left, float top, float right, float bottom)
		{
			rectTransform.anchorMin = new Vector2(left, bottom);
			rectTransform.anchorMax = new Vector2(1f - right, 1f - top);
		}

		#endregion

		#region World positions

		private static readonly Vector3[] _fourCorners = new Vector3[4];//start bottom left and clockwise

		public static Vector2 GetWorldCenter(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return new Vector2((_fourCorners[0].x + _fourCorners[3].x) / 2f, (_fourCorners[0].y + _fourCorners[1].y) / 2f);
		}

		public static float GetWorldLeft(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return _fourCorners[0].x;
		}

		public static float GetWorldRight(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return _fourCorners[2].x;
		}

		public static float GetWorldTop(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return _fourCorners[1].y;
		}

		public static float GetWorldBottom(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return _fourCorners[0].y;
		}

		public static Vector2 GetWorldTopLeft(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return new Vector2(_fourCorners[0].x, _fourCorners[1].y);
		}

		public static Vector2 GetWorldTopRight(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return new Vector2(_fourCorners[2].x, _fourCorners[1].y);
		}

		public static Vector2 GetWorldBottomLeft(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return new Vector2(_fourCorners[0].x, _fourCorners[0].y);
		}

		public static Vector2 GetWorldBottomRight(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return new Vector2(_fourCorners[2].x, _fourCorners[0].y);
		}

		public static Rect GetWorldRect(this RectTransform rectTransform)
		{
			rectTransform.GetWorldCorners(_fourCorners);
			return new Rect(_fourCorners[0].x, _fourCorners[0].y, Mathf.Abs(_fourCorners[3].x - _fourCorners[0].x), Mathf.Abs(_fourCorners[1].y - _fourCorners[0].y));
		}

		#endregion
		
		#endregion

		#region ParticleSystem
		
		public static IEnumerator PlayAndWaitForFinish(this ParticleSystem particleSystem)
		{
			particleSystem.Play();
			while (particleSystem.isPlaying)
				yield return null;
		}

		#endregion
		
		#region Dot Net

		public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (T item in items)
				action(item);
		}

		#endregion
	}
}