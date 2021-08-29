using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Sprites;
using System.Collections.Generic;

public class PointGauge : MonoBehaviour
{
    /// <summary>
    /// 幅
    /// </summary>
    public int Width = 100;

    /// <summary>
    /// 高さ
    /// </summary>
    public int Height = 10;

    /// <summary>
    /// エッジサイズ
    /// </summary>
    public int Edge = 2;

    /// <summary>
    /// 背景色の色
    /// </summary>
    public Color BackgroundColor = Color.white;

    /// <summary>
    /// ゲージの色
    /// </summary>
    public Color GaugeColor = Color.green;

    /// <summary>
    /// ゲージの割合
    /// </summary>
    [SerializeField, Range(0.0f, 1.0f)]
    public float GaugeRatio = 1.0f;

    /// <summary>
    /// マスク
    /// </summary>
    private RectMask2D _rectMask;

    public void Start()
    {
        var background = new GameObject("PointGaugeBackground");
        background.transform.SetParent(transform);
        {
            var image = background.AddComponent<Gauge>();
            image.color = BackgroundColor;

            var outline = background.AddComponent<Outline>();
            outline.effectColor = new Color(0,0,0,0.3f);
            outline.effectDistance = new Vector2(2 ,-2);

            var rectTransform = background.transform as RectTransform;
            rectTransform.sizeDelta = new Vector2(Width, Height);
            rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }


        var frontMask = new GameObject("PointGaugeFrontMask");
        frontMask.transform.SetParent(transform);
        {
            _rectMask = frontMask.AddComponent<RectMask2D>();
            _rectMask.padding = new Vector4(0, 0, Width, 0);

            var rectTransform = frontMask.transform as RectTransform;
            rectTransform.sizeDelta = new Vector2(Width, Height);
            rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }

        var front = new GameObject("PointGaugeFront");
        front.transform.SetParent(frontMask.transform);
        {
            var image = front.AddComponent<Gauge>();
            image.color = GaugeColor;

            var rectTransform = front.transform as RectTransform;
            rectTransform.sizeDelta = new Vector2(Width - Edge, Height - Edge);
            rectTransform.anchoredPosition = new Vector3(0, 0, 0);
        }

        UpdateGauge();
    }

    /// <summary>
    /// ゲージの割合を設定
    /// </summary>
    public void SetGaugeRatio(float value)
    {
        GaugeRatio = value;
        UpdateGauge();
    }

    /// <summary>
    /// ゲージの表示割合を更新
    /// </summary>
    public void UpdateGauge()
    {
        if (_rectMask != null)
        {
            _rectMask.padding = new Vector4(0, 0, Width - (Width * GaugeRatio), 0);
        }
    }

    /// <summary>
    /// ゲージクラス
    /// </summary>
    public class Gauge : Image
    {
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            var radius = 16.0f;
            var triangleNum = 8;

            Vector4 v = GetDrawingDimensions(false);
            Vector4 uv = overrideSprite != null ? DataUtility.GetOuterUV(overrideSprite) : Vector4.zero;

            var color32 = color;
            vh.Clear();

            if (radius > (v.z - v.x) / 2) radius = (v.z - v.x) / 2;
            if (radius > (v.w - v.y) / 2) radius = (v.w - v.y) / 2;
            if (radius < 0) radius = 0;

            float uvRadiusX = radius / (v.z - v.x);
            float uvRadiusY = radius / (v.w - v.y);

            vh.AddVert(new Vector3(v.x, v.w - radius), color32, new Vector2(uv.x, uv.w - uvRadiusY));
            vh.AddVert(new Vector3(v.x, v.y + radius), color32, new Vector2(uv.x, uv.y + uvRadiusY));

            vh.AddVert(new Vector3(v.x + radius, v.w), color32, new Vector2(uv.x + uvRadiusX, uv.w));
            vh.AddVert(new Vector3(v.x + radius, v.w - radius), color32, new Vector2(uv.x + uvRadiusX, uv.w - uvRadiusY));
            vh.AddVert(new Vector3(v.x + radius, v.y + radius), color32, new Vector2(uv.x + uvRadiusX, uv.y + uvRadiusY));
            vh.AddVert(new Vector3(v.x + radius, v.y), color32, new Vector2(uv.x + uvRadiusX, uv.y));

            vh.AddVert(new Vector3(v.z - radius, v.w), color32, new Vector2(uv.z - uvRadiusX, uv.w));
            vh.AddVert(new Vector3(v.z - radius, v.w - radius), color32, new Vector2(uv.z - uvRadiusX, uv.w - uvRadiusY));
            vh.AddVert(new Vector3(v.z - radius, v.y + radius), color32, new Vector2(uv.z - uvRadiusX, uv.y + uvRadiusY));
            vh.AddVert(new Vector3(v.z - radius, v.y), color32, new Vector2(uv.z - uvRadiusX, uv.y));

            vh.AddVert(new Vector3(v.z, v.w - radius), color32, new Vector2(uv.z, uv.w - uvRadiusY));
            vh.AddVert(new Vector3(v.z, v.y + radius), color32, new Vector2(uv.z, uv.y + uvRadiusY));

            vh.AddTriangle(1, 0, 3);
            vh.AddTriangle(1, 3, 4);

            vh.AddTriangle(5, 2, 6);
            vh.AddTriangle(5, 6, 9);

            vh.AddTriangle(8, 7, 10);
            vh.AddTriangle(8, 10, 11);

            List<Vector2> vCenterList = new List<Vector2>();
            List<Vector2> uvCenterList = new List<Vector2>();
            List<int> vCenterVertList = new List<int>();

            vCenterList.Add(new Vector2(v.z - radius, v.w - radius));
            uvCenterList.Add(new Vector2(uv.z - uvRadiusX, uv.w - uvRadiusY));
            vCenterVertList.Add(7);

            vCenterList.Add(new Vector2(v.x + radius, v.w - radius));
            uvCenterList.Add(new Vector2(uv.x + uvRadiusX, uv.w - uvRadiusY));
            vCenterVertList.Add(3);

            vCenterList.Add(new Vector2(v.x + radius, v.y + radius));
            uvCenterList.Add(new Vector2(uv.x + uvRadiusX, uv.y + uvRadiusY));
            vCenterVertList.Add(4);

            vCenterList.Add(new Vector2(v.z - radius, v.y + radius));
            uvCenterList.Add(new Vector2(uv.z - uvRadiusX, uv.y + uvRadiusY));
            vCenterVertList.Add(8);

            float degreeDelta = (float)(Mathf.PI / 2 / triangleNum);
            float curDegree = 0;

            for (int i = 0; i < vCenterVertList.Count; i++)
            {
                int preVertNum = vh.currentVertCount;
                for (int j = 0; j <= triangleNum; j++)
                {
                    float cosA = Mathf.Cos(curDegree);
                    float sinA = Mathf.Sin(curDegree);
                    Vector3 vPosition = new Vector3(vCenterList[i].x + cosA * radius, vCenterList[i].y + sinA * radius);
                    Vector3 uvPosition = new Vector2(uvCenterList[i].x + cosA * uvRadiusX, uvCenterList[i].y + sinA * uvRadiusY);
                    vh.AddVert(vPosition, color32, uvPosition);
                    curDegree += degreeDelta;
                }
                curDegree -= degreeDelta;
                for (int j = 0; j <= triangleNum - 1; j++)
                {
                    vh.AddTriangle(vCenterVertList[i], preVertNum + j + 1, preVertNum + j);
                }
            }
        }

        private Vector4 GetDrawingDimensions(bool shouldPreserveAspect)
        {
            var padding = overrideSprite == null ? Vector4.zero : DataUtility.GetPadding(overrideSprite);
            Rect r = GetPixelAdjustedRect();
            var size = overrideSprite == null ? new Vector2(r.width, r.height) : new Vector2(overrideSprite.rect.width, overrideSprite.rect.height);

            int spriteW = Mathf.RoundToInt(size.x);
            int spriteH = Mathf.RoundToInt(size.y);

            if (shouldPreserveAspect && size.sqrMagnitude > 0.0f)
            {
                var spriteRatio = size.x / size.y;
                var rectRatio = r.width / r.height;

                if (spriteRatio > rectRatio)
                {
                    var oldHeight = r.height;
                    r.height = r.width * (1.0f / spriteRatio);
                    r.y += (oldHeight - r.height) * rectTransform.pivot.y;
                }
                else
                {
                    var oldWidth = r.width;
                    r.width = r.height * spriteRatio;
                    r.x += (oldWidth - r.width) * rectTransform.pivot.x;
                }
            }

            var v = new Vector4(
                    padding.x / spriteW,
                    padding.y / spriteH,
                    (spriteW - padding.z) / spriteW,
                    (spriteH - padding.w) / spriteH);

            v = new Vector4(
                    r.x + r.width * v.x,
                    r.y + r.height * v.y,
                    r.x + r.width * v.z,
                    r.y + r.height * v.w
                    );

            return v;
        }
    }
}
