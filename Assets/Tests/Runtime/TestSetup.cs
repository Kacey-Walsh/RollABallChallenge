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
    public class RollABallTests : InputTestFixture
    {

        [UnityTest]
        public IEnumerator TestCameraControllerPresent()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject player = GameObject.Find("Player");
            GameObject maincam = GameObject.Find("Main Camera");
            CameraController cam = maincam.GetComponent<CameraController>();
            Assert.AreEqual(cam.player, player);
            Vector3 offset = maincam.transform.position - player.transform.position;
            Press(keyboard.rightArrowKey);
            Press(keyboard.upArrowKey);
            yield return new WaitForSeconds(2f);

            Vector3 offset2 = maincam.transform.position - player.transform.position;
            Debug.Log(player.transform.position);
            Assert.True(offset.x - offset2.x < .4);
            Assert.True(offset.z - offset2.z < .4);
        }
        
        [UnityTest]
        public IEnumerator TestPhysicsSetup()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject player = GameObject.Find("Player");
            Assert.NotNull(player);
            Assert.True(player.gameObject.GetComponent<MeshFilter>().mesh.name == "Sphere Instance");
        }
        [UnityTest]
        public IEnumerator TestPlayerControls()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            GameObject player;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            player = GameObject.Find("Player");
            Press(keyboard.downArrowKey);
            yield return new WaitForSeconds(1f);
            Assert.True(player.transform.position.z < 0f);
            Release(keyboard.downArrowKey);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return new WaitForSeconds(1f);
            player = GameObject.Find("Player");
            Press(keyboard.leftArrowKey);
            yield return new WaitForSeconds(1f);
            Assert.True(player.transform.position.x < 0f);
            Release(keyboard.leftArrowKey);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return new WaitForSeconds(1f);
            player = GameObject.Find("Player");
            Press(keyboard.rightArrowKey);
            yield return new WaitForSeconds(1f);
            Assert.True(player.transform.position.x > 0f);
            Release(keyboard.rightArrowKey);
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return new WaitForSeconds(.5f);
            player = GameObject.Find("Player");
            Press(keyboard.upArrowKey);
            yield return new WaitForSeconds(1f);
            Assert.True(player.transform.position.z > 0f);
        }
        

        [UnityTest]
        public IEnumerator TestPlayerDoesntFall()
        {
            Keyboard keyboard = InputSystem.AddDevice<Keyboard>();
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject player = GameObject.Find("Player");
            Press(keyboard.upArrowKey);
            yield return new WaitForSeconds(3.5f);
            Assert.True(player.transform.position.y > 0);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator TestPlayerExists()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MiniGame");
            yield return null;
            GameObject ground = GameObject.Find("Ground");
            Assert.NotNull(ground);
            Assert.True(ground.gameObject.GetComponent<MeshFilter>().mesh.name == "Plane Instance");
            GameObject player = GameObject.Find("Player");
            Assert.NotNull(player);
            Assert.True(player.gameObject.GetComponent<MeshFilter>().mesh.name == "Sphere Instance");
            Material mat = ground.GetComponent<Renderer>().material;
            Assert.True(mat.name == "Background (Instance)", "Make sure that the material assigned to the plane is Background and not background");
        }

    }
}
