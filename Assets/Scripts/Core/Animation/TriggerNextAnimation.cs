using UnityEngine;

namespace Core.Animation
{
    public class TriggerNextAnimation : MonoBehaviour
    {
        public void PlayNext()
        {
            SendMessageUpwards("NextAnimation",SendMessageOptions.DontRequireReceiver);
        }
    }
}