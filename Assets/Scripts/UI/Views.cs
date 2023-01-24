using UnityEngine;
using Zenject;

namespace UI
{
    public class View : MonoBehaviour
    {
        #region Properties

        public bool IsActive { get; private set; }

        #endregion

        #region Fields

        [Inject] protected SignalBus SignalBus;

        #endregion

        #region Methods

        public void Show()
        {
            IsActive = true;
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            IsActive = false;
            gameObject.SetActive(false);
        }

        #endregion
    }
}