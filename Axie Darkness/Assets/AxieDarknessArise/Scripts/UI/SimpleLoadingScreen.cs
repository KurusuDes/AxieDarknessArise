using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.SceneManagement;
using Doozy.Runtime.UIManager.Components;
using Doozy.Runtime.UIManager.Animators;

namespace ADR.UI
{
    public class SimpleLoadingScreen : MonoBehaviour
    {
        public UISlider slider;
        public UIContainerUIAnimator Animator;
        public float delay = 0.5f;
        void Start()
        {
            slider = GetComponent<UISlider>();
            //Animator.InstantHide();
        }
        public void LoadScene(string sceneName)
        {
            Animator.Show();
            StartCoroutine(LoadAsyncScene(sceneName, delay));
        }
        IEnumerator LoadAsyncScene(string sceneName,float delay)
        {
            yield return new WaitForSecondsRealtime(delay);

            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f)*100; // 0.9f es un valor de referencia
                slider.value = progress;

                yield return null;
            }
        }
    }
}
