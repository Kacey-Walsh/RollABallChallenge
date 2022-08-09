using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEditor;
// using UnityEngine.SceneManager;


namespace Tests
{
    public class RollABallTestsPickups : InputTestFixture
    {

        public GameObject GetPickupPrefab()
        {
            string[] results;
            results = AssetDatabase.FindAssets("Pick t:prefab");
            GameObject pickup = null;
            foreach (string guid in results)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.Contains("PickUp") || path.Contains("Pick Up"))
                    Debug.Log("testI: " + AssetDatabase.GUIDToAssetPath(guid));
                pickup = (GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(GameObject));// (GameObject)Resources.Load("Prefabs/Pick Up");
            }
            return pickup;
        }

        [UnityTest]
        public IEnumerator TestPickupConfiguration()
        {
            // var action1 = new InputAction("action1", binding: "<keyboard>/upArrow", interactions: "Hold");
            // UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            // yield return null;
            GameObject pickup = GetPickupPrefab();
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            Assert.True(instance != null);
            // Press(keyboard.rightArrowKey);
            yield return new WaitForSeconds(1.0f);
            Assert.True(instance.GetComponent<Rigidbody>() != null, "Pickup does not have a Rigidbody component attached");
            Assert.True(instance.GetComponent<Rigidbody>().isKinematic, "Pickup Rigidbody is not set to be Kinematic");
            Assert.True(instance.GetComponent<BoxCollider>().isTrigger, "Pickup is not a trigger");
        }

        [UnityTest]
        public IEnumerator TestPickUpRemoved()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            // var action1 = new InputAction("action1", binding: "<keyboard>/upArrow", interactions: "Hold");
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject pickup = GetPickupPrefab();
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            Assert.True(instance != null);
            Press(keyboard.upArrowKey);
            // action1.
            yield return new WaitForSeconds(2.0f);
            Assert.False(instance.activeInHierarchy);
        }

        [UnityTest]
        public IEnumerator TestPickUpsHaveTag()
        {
            LogAssert.ignoreFailingMessages = true;
            GameObject pickup = GetPickupPrefab();
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            yield return new WaitForSeconds(1.0f);
            Assert.True(instance != null);
            // bool tagged;
            Assert.True(instance.tag == "Pick Up" || instance.tag == "PickUp");
        }

        [UnityTest]
        public IEnumerator TestRotatorPresent()
        {
            GameObject pickup = GetPickupPrefab();
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            // Press(keyboard.rightArrowKey);
            yield return new WaitForSeconds(1.0f);
            Assert.True(instance.transform.rotation.x != 0);
            Assert.True(instance.transform.rotation.y != 0);
            Assert.True(instance.transform.rotation.z != 0);
        }
    }
}