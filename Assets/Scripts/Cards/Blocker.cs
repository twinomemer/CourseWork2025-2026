using UnityEngine;

namespace Cards
{
    public class Blocker : MonoBehaviour
    {
        private GameObject _blocker;

        private void Awake()
        {
            _blocker = transform.GetChild(0).gameObject;
        }

        private void OnTransformChildrenChanged()
        {
            switch (_blocker.gameObject.activeSelf)
            {
                case true:
                    _blocker.SetActive(false);
                    break;
                case false:
                    _blocker.SetActive(true);
                    break;
            }
        }
    }
}
