using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VTLTools.UIAnimation;
using VTLTools;
using DP.Utilities;

namespace AntiStress.UI
{
    public class PopupBase : MonoBehaviour
    {
        [SerializeField, BoxGroup("Popup Reference")] protected Button closeButton;
        private MenuAnimationControl menuAnimationControl;
        protected MenuAnimationControl ThisMenuAnimationControl
        {
            get
            {
                if (menuAnimationControl == null)
                    menuAnimationControl = GetComponent<MenuAnimationControl>();
                return menuAnimationControl;
            }
        }
        protected System.Action actionOnStartShow, actionOnCompleteShow, actionOnStartHide, actionOnCompleteHide;
        protected object data;
        public bool IsShow
        {
            get
            {
                return this.gameObject.activeSelf;
            }
        }

        #region SHOW
        public virtual void Show(object _data = null, float _delay = 0f, bool _hasAnimation = true, Action _actionOnStartShow = null, Action _actionOnCompleteShow = null, Action _actionOnStartHide = null, Action _actionOnCompleteHide = null)
        {
            if (IsShow)
                return;
            this.data = _data;
            this.actionOnStartShow = _actionOnStartShow;
            this.actionOnCompleteShow = _actionOnCompleteShow;
            this.actionOnStartHide = _actionOnStartHide;
            this.actionOnCompleteHide = _actionOnCompleteHide;
            this.Init();

            ButtonAddListener();
            if (ThisMenuAnimationControl == null || !_hasAnimation)
            {
                this.gameObject.SetActive(true);
                OnShowStarted();
                OnShowCompleted();
            }
            else
            {
                this.gameObject.SetActive(true);
                ThisMenuAnimationControl.StartShow(_delay, _onShowStarted: OnShowStarted, _onShowCompleted: OnShowCompleted);
            }
        }
        protected virtual void OnShowStarted()
        {
            this.actionOnStartShow?.Invoke();
        }
        protected virtual void OnShowCompleted()
        {
            this.actionOnCompleteShow?.Invoke();
        }
        [Button, BoxGroup("UI preview")]
        public void PreviewShow()
        {
            foreach (var _item in ThisMenuAnimationControl.menuItems)
            {
                _item.PreviewShow();
            }
        }

        #endregion

        #region HIDE
        public virtual void Hide(bool _hasAnimation = true)
        {
            if (!IsShow)
                return;
            ButtonRemoveListener();
            if (ThisMenuAnimationControl == null || !_hasAnimation)
            {
                OnHideStarted();
                OnHideCompleted();
                PreviewHide();
            }
            else
            {
                ThisMenuAnimationControl.StartHide(0f, _onHideStarted: OnHideStarted, _onHideCompleted: OnHideCompleted);
            }
        }
        protected virtual void OnHideStarted()
        {
            this.actionOnStartHide?.Invoke();
        }
        protected virtual void OnHideCompleted()
        {
            this.actionOnCompleteHide?.Invoke();
            this.gameObject.SetActive(false);
        }
        [Button, BoxGroup("UI preview")]
        public void PreviewHide()
        {
            foreach (var _item in ThisMenuAnimationControl.menuItems)
            {
                _item.PreviewHide();
            }
        }
        #endregion

        protected virtual void Init()
        {

        }
        protected virtual void ButtonAddListener()
        {
            closeButton?.onClick.AddListener(OnCloseClick);
        }
        protected virtual void ButtonRemoveListener()
        {
            closeButton?.onClick.RemoveListener(OnCloseClick);
        }
        protected virtual void OnCloseClick()
        {
            if (!IsShow)
                return;
            this.Hide();
            SoundSystem.Instance?.PlayUIClick();
            VibrationSystem.Instance?.PlayVibration();
        }
    }
}