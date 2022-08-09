using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEditor;
using System.Text.RegularExpressions;
using TMPro;
// using UnityEngine.SceneManager;


namespace Tests
{
    public class RollABallTestsScoring : InputTestFixture
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
        public IEnumerator TestScorePresent()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject goScore = GameObject.Find("countText");
            if (goScore == null)
            {
                goScore = GameObject.Find("CountText");
            }
            TextMeshProUGUI score = goScore.GetComponent<TextMeshProUGUI>();

            PlayerController pc = GameObject.Find("Player").GetComponent<PlayerController>();
            Assert.True(pc.countText != null);
            StringAssert.IsMatch(@"Count: ?\d+", score.text);
        }

        [UnityTest]
        public IEnumerator TestScoreOnCollide()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject parent = GameObject.FindGameObjectWithTag("PickUp").transform.parent.gameObject;
            parent.SetActive(false);

            GameObject pickup = GetPickupPrefab();
            GameObject instance = GameObject.Instantiate(pickup, new Vector3(0, 0, 3), new Quaternion(0, 0, 0, 0));
            TextMeshProUGUI score = GameObject.Find("CountText").GetComponent<TextMeshProUGUI>();
            var startScore = score.text;
            Debug.Log(Regex.Match(startScore, @"\d").Value);
            int curScore;
            int.TryParse(Regex.Match(startScore, @"\d").Value, out curScore);
            Press(keyboard.upArrowKey);
            yield return new WaitForSeconds(2.0f);
            StringAssert.IsMatch("Count: ?" + (curScore + 1), score.text);
        }

        [UnityTest]
        public IEnumerator TestWinText()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject parent = GameObject.FindGameObjectWithTag("PickUp").transform.parent.gameObject;
            parent.SetActive(false);


            GameObject pickup = GetPickupPrefab();
            for (var i = 0; i < 12; i++)
            {
                GameObject instance1 = GameObject.Instantiate(pickup, new Vector3(0, .25f, (3f / 12f) * i), new Quaternion(0, 0, 0, 0));
            }
            GameObject gOwinText = GameObject.Find("WinText");
            TextMeshProUGUI winText = null;
            Debug.Log(gOwinText);
            if (gOwinText != null)
            {
                winText = gOwinText.GetComponent<TextMeshProUGUI>();
                var starttext = winText.text;
                StringAssert.AreEqualIgnoringCase(starttext, "");
            }

            Press(keyboard.upArrowKey);
            yield return new WaitForSeconds(2.0f);
            gOwinText = GameObject.Find("WinText");
            winText = gOwinText.GetComponent<TextMeshProUGUI>();
            Match rm = Regex.Match(winText.text, "you win", RegexOptions.IgnoreCase);
            Assert.True(rm.Success);
        }
    }
}
