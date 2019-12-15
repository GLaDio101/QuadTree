using System.Collections;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Core.View
{
    public class CoreView : EventView
    {
        public float DispatchDelay = 0;

        public UnityEngine.Animation[] CloseAnimations;

        protected IEnumerator DispatchDelayedInner(object eventType, float delay)
        {
            yield return new WaitForSeconds(delay);

            dispatcher.Dispatch(eventType);
        }

        protected void DispatchDelayed(object eventType, float delay)
        {
            PlayAnimations();
            StartCoroutine(DispatchDelayedInner(eventType, delay));
        }

        protected void DispatchDelayed(object eventType)
        {
            PlayAnimations();
            StartCoroutine(DispatchDelayedInner(eventType, DispatchDelay));
        }

        protected void PlayAnimations()
        {
            if (CloseAnimations == null)
                return;

            foreach (var closeAnimation in CloseAnimations)
            {
                PlayByIndex(closeAnimation, 0);
            }
        }

        private void PlayByIndex(UnityEngine.Animation anim, int index)
        {
            if (anim == null)
                return;

            var i = 0;
            foreach (AnimationState animationState in anim)
            {
                if (i == index)
                {
                    anim.Play(animationState.clip.name);
                    return;
                }

                i++;
            }
        }
    }
}