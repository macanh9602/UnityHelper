using UnityEngine;

namespace VTLTools
{
    public static class RendererExtensions
    {
        // Cache static để dùng chung cho toàn bộ game, tránh cấp phát bộ nhớ mới liên tục
        private static MaterialPropertyBlock _mpb;
        private static MaterialPropertyBlock Mpb
        {
            get
            {
                if (_mpb == null) _mpb = new MaterialPropertyBlock();
                return _mpb;
            }
        }

        /// <summary>
        /// Set màu bằng PropertyBlock (Hiệu năng cao, không tạo instance material).
        /// Dùng string name (tiện nhưng chậm hơn int ID).
        /// </summary>
        public static void SetColorMPB(this Renderer renderer, string propertyName, Color color)
        {
            SetColorMPB(renderer, Shader.PropertyToID(propertyName), color);
        }

        /// <summary>
        /// [Overload] Set màu bằng Property ID (Tối ưu nhất cho Update loop).
        /// </summary>
        public static void SetColorMPB(this Renderer renderer, int propertyId, Color color)
        {
            if (renderer == null) return;

            // 1. Lấy block hiện tại của renderer (để không ghi đè các property khác)
            renderer.GetPropertyBlock(Mpb);

            // 2. Thay đổi giá trị
            Mpb.SetColor(propertyId, color);

            // 3. Gán ngược lại
            renderer.SetPropertyBlock(Mpb);
        }

        /// <summary>
        /// Set màu HDR (Intensity) cho vật liệu phát sáng (Emission).
        /// </summary>
        public static void SetEmissionColor(this Renderer renderer, Color color, float intensity)
        {
            // "_EmissionColor" là chuẩn chung của Unity Standard/URP shader
            // Bạn có thể cache ID này ở ngoài constant nếu dùng nhiều
            int emissionId = Shader.PropertyToID("_EmissionColor");

            // Tính toán màu HDR: Màu gốc * Cường độ ^ 2 (Linear space)
            Color hdrColor = color * Mathf.Pow(2, intensity);

            renderer.SetColorMPB(emissionId, hdrColor);
        }
    }
}