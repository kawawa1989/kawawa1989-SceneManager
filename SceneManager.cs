using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace kawawa1989.Scene
{
    public class SceneHistoryData
    {
        public string sceneName { get; set; } = "";
        public object param { get; set; } = null;
        public List<SceneHistoryData> child { get; set; } = new List<SceneHistoryData>();
    }

    public class SceneManager : MonoBehaviour
    {
        List<SceneHistoryData> history = new List<SceneHistoryData>();
        SceneHistoryData currentScene = null;

        public SceneHistoryData Pop()
        {
            var index = history.Count - 1;
            var ret = history[index];
            history.RemoveAt(index);
            return ret;
        }

        public void GoToScene(string sceneName, object param)
        {
            var prev = currentScene;
            var next = new SceneHistoryData();
            if (prev == null)
            {
                prev = new SceneHistoryData();
                var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
                prev.sceneName = scene.name;
            }
            next.sceneName = sceneName;
            next.param = param;
            history.Add(prev);

            var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            op.completed += (asyncOperation) =>
            {
                currentScene = next;
            };
        }

        public void GoToChildScene(string sceneName, object param)
        {
            var child = new SceneHistoryData();
            child.sceneName = currentScene.sceneName;
            child.param = param;
            currentScene.child.Add(child);
            var op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            op.completed += (asyncOperation) =>
            {
            };
        }

        public void GoToBack()
        {
            var previous = Pop();
            var loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(previous.sceneName, LoadSceneMode.Single);
            loadOperation.completed += (result2) =>
            {
                currentScene = previous;
            };
        }
    }
}