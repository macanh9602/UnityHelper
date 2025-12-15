using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace VTLTools
{
    public static class AnimationUtils
    {
        public static Sequence PlayHandTUT(Transform pivot, Vector3 startPos, Vector3 endPos, Image img = null, SpriteRenderer sprite = null, bool isLoop = true, Vector3 originScale = default)
        {
            if (originScale == default)
                originScale = Vector3.one;
            Sequence _seq = DOTween.Sequence();
            if (img != null)
            {
                img.gameObject.SetActive(true);
                img.transform.localScale = originScale * 1.2f;
                img.transform.position = startPos;
                var color = img.color;
                color.a = 1f;
                img.color = color;
                _seq.Append(img.transform.DOScale(originScale, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(img.transform.DOMove(endPos, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(img.transform.DOScale(originScale * 1.3f, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(img.DOFade(0f, 0.5f));
            }
            else if (sprite != null)
            {
                pivot.gameObject.SetActive(true);
                pivot.transform.localScale = Vector3.one * 1.2f;
                pivot.transform.position = startPos;
                var color = sprite.color;
                color.a = 1f;
                sprite.color = color;
                _seq.Append(pivot.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(pivot.transform.DOMove(endPos, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(pivot.transform.DOScale(Vector3.one * 1.3f, 0.5f).SetEase(Ease.InOutSine));
                _seq.Append(sprite.DOFade(0f, 0.5f));
            }
            if (isLoop)
            {
                _seq.AppendInterval(0.5f); // Thêm delay trước khi lặp lại
                _seq.SetLoops(-1, LoopType.Restart); // Lặp vô hạn
            }
            return _seq;
        }

        public static Sequence PlayNotifyWinLevel(Image img)
        {
            Sequence _seq = DOTween.Sequence();
            if (img != null)
            {
                img.gameObject.SetActive(true);
                img.transform.localScale = Vector3.one * 0.5f;
                var color = img.color;

                img.color = color;
                _seq.Append(img.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack));
                _seq.Join(img.DOFade(1f, 0.5f));
                _seq.AppendInterval(1f);
                _seq.Append(img.transform.DOScale(Vector3.one * 1.2f, 0.5f).SetEase(Ease.InBack));
                _seq.Join(img.DOFade(0f, 0.5f));
                _seq.OnComplete(() => img.gameObject.SetActive(false));
            }

            return _seq;
        }

        public static Sequence PlayAnimShow(Transform target, float startScale = 0, float endScale = 1)
        {
            Sequence _seq = DOTween.Sequence();
            target.gameObject.SetActive(true);
            target.localScale = Vector3.one * startScale;
            _seq.Append(target.DOScale(Vector3.one * endScale, 0.5f).SetEase(Ease.OutBack));
            _seq.AppendInterval(1f);
            _seq.OnComplete(() => target.gameObject.SetActive(false));
            return _seq;
        }

        public static Sequence AnimScoreWorldSpace(Transform pivot, Text text, Vector3 startPos, Vector3 endPos, System.Action onComplete = null, bool moreRotate = false, float speedAvg = 0.5f)
        {
            Sequence _seq = DOTween.Sequence();
            text.color = text.color.SetAlpha(0f);
            pivot.gameObject.SetActive(true);
            pivot.transform.position = startPos;
            _seq.Append(pivot.DOMove(endPos, speedAvg).SetEase(Ease.InOutSine));
            _seq.Join(text.DOFade(1f, speedAvg));
            // Lấy rotation hiện tại theo Euler
            Vector3 currentEuler = pivot.transform.eulerAngles;
            // Tạo góc xoay Z ngẫu nhiên
            if (moreRotate)
            {
                float randomZ = Random.Range(-30f, 30f);
                Vector3 targetEuler = new Vector3(currentEuler.x, currentEuler.y, randomZ);
                //DPDebug.Log($"<color=green>[DA]</color> {pivot.transform.eulerAngles} - {targetEuler}");
                _seq.Append(pivot.DORotate(targetEuler, speedAvg, RotateMode.Fast));
            }
            _seq.AppendInterval(0.3f);
            _seq.OnComplete(() =>
                {
                    onComplete?.Invoke();
                });
            return _seq;
        }
    }
}