using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace StackMaps {
  /// <summary>
  /// This class handles all communication with the server. This follows the
  /// (Unity) singleton pattern. Server information should be filled out on the
  /// login screen.
  /// </summary>
  public class ServiceController : MonoBehaviour {
    /// <summary>
    /// The shared instance of the service controller.
    /// </summary>
    public static ServiceController shared;

    // This is the URL to the service php script. It is where all API calls are
    // sent to.
    string apiUrl = "http://zhengfu23.us/api.php";

    // This is our token, acting as a pass for making changes to the database.
    string token = "";

    #region Login

    // The coroutine handling network connection.
    Coroutine loginCoroutine;

    /// <summary>
    /// A callback to Login method called after server responds.
    /// </summary>
    /// <param name="success">Whether the request successful.</param>
    /// <param name="authenticated">Whether login is successful.</param>
    public delegate void LoginCallback(bool success,bool authenticated);

    /// <summary>
    /// Connect to the specified url with given username and password. If there
    /// is already a connecting attempt, does nothing.
    /// </summary>
    /// <param name="url">URL of the API.</param>
    /// <param name="username">Username.</param>
    /// <param name="password">Password.</param>
    /// <param name = "callback">Callback function.</param>
    public void Login(string url, string username, string password, LoginCallback callback) {
      if (loginCoroutine != null) {
        return;
      }

      apiUrl = url;

      // We send in username and password. If successful, we are granted a
      // token which we can use to gain access to editing the map database.
      loginCoroutine = StartCoroutine(LoginLoop(username, password, callback));
    }

    IEnumerator LoginLoop(string username, string password, LoginCallback callback) {
      WWWForm form = new WWWForm();
      form.AddField("request", "login");
      form.AddField("username", username);
      form.AddField("password", password);

      // Create a download object
      WWW request = new WWW(apiUrl, form);

      // Wait until the download is done
      yield return request;

      if (!string.IsNullOrEmpty(request.error)) {
        Debug.Log("Unable to login: " + request.error);
        callback(false, false);
      } else {
        JSONNode node = JSON.Parse(request.text);

        if (node["success"]) {
          token = node["token"];
          callback(true, true);
        } else {
          callback(true, false);
        }
      }

      loginCoroutine = null;
    }

    /// <summary>
    /// Cancels the connection. This guarantees that the callback passed in from
    /// connect will never be called. If no current connection is underway, does
    /// nothing.
    /// </summary>
    public void CancelLogin() {
      StopCoroutine(loginCoroutine);
      loginCoroutine = null;
    }

    #endregion

    #region GetLibraryList

    // The coroutine handling network connection.
    Coroutine getLibraryListCoroutine;

    /// <summary>
    /// A callback to GetLibraryList.
    /// </summary>
    /// <param name="success">Whether the request successful.</param>
    /// <param name="data">Parsed library data.</param>
    public delegate void GetLibraryListCallback(bool success,List<Library> data);

    /// <summary>
    /// Fetches the list of libraries in the database. Does nothing if there is
    /// already a request under way. Note this returns an incomplete list giving
    /// only library ids and names.
    /// </summary>
    /// <param name="callback">Callback function for data handling.</param>
    public void GetLibraryList(GetLibraryListCallback callback) {
      if (getLibraryListCoroutine != null) {
        return;
      }

      getLibraryListCoroutine = StartCoroutine(GetLibraryListLoop(callback));
    }

    IEnumerator GetLibraryListLoop(GetLibraryListCallback callback) {
      WWWForm form = new WWWForm();
      form.AddField("request", "get_library_list");

      // Create a download object
      WWW request = new WWW(apiUrl, form);

      // Wait until the download is done
      yield return request;

      if (!string.IsNullOrEmpty(request.error)) {
        Debug.Log("Unable to fetch library list: " + request.error);
        callback(false, null);
      } else {
        JSONNode root = JSON.Parse(request.text);

        if (root["success"]) {
          // Parse the library list here.
          List<Library> libList = new List<Library>();

          foreach (JSONNode node in root["data"].AsArray) {
            Library lib = new Library();
            lib.FromJSON(node);
            libList.Add(lib);
          }

          callback(true, libList);
        } else {
          callback(false, null);
        }
      }

      getLibraryListCoroutine = null;
    }

    /// <summary>
    /// Cancels the connection. This guarantees that the callback passed in from
    /// connect will never be called. If no current connection is underway, does
    /// nothing.
    /// </summary>
    public void CancelGetLibraryList() {
      StopCoroutine(getLibraryListCoroutine);
      getLibraryListCoroutine = null;
    }

    #endregion

    #region GetLibrary

    // The coroutine handling network connection.
    Coroutine getLibraryCoroutine;

    /// <summary>
    /// A callback to GetLibrary method called after server responds.
    /// </summary>
    /// <param name="success">Whether the request successful.</param>
    /// <param name="data">Parsed library data.</param>
    public delegate void GetLibraryCallback(bool success,Library data);

    /// <summary>
    /// Fetches the given library in the database. Does nothing if there is
    /// already a request under way.
    /// </summary>
    /// <param name = "libId"></param>
    /// <param name="callback">Callback function for data handling.</param>
    public void GetLibrary(int libId, GetLibraryCallback callback) {
      if (getLibraryCoroutine != null) {
        return;
      }

      getLibraryCoroutine = StartCoroutine(GetLibraryLoop(libId, callback));
    }

    IEnumerator GetLibraryLoop(int libId, GetLibraryCallback callback) {
      WWWForm form = new WWWForm();
      form.AddField("request", "get_library");
      form.AddField("library_id", libId);

      // Create a download object
      WWW request = new WWW(apiUrl, form);

      // Wait until the download is done
      yield return request;

      if (!string.IsNullOrEmpty(request.error)) {
        Debug.Log("Unable to fetch library: " + request.error);
        callback(false, null);
      } else {
        JSONNode root = JSON.Parse(request.text);
        Debug.Log(root);

        if (root["success"]) {
          // Parse the library list here.
          Library lib = new Library();
          lib.FromJSON(root["data"]);
          callback(true, lib);
        } else {
          callback(false, null);
        }
      }

      getLibraryCoroutine = null;
    }

    /// <summary>
    /// Cancels the connection. This guarantees that the callback passed in from
    /// connect will never be called. If no current connection is underway, does
    /// nothing.
    /// </summary>
    public void CancelGetLibrary() {
      StopCoroutine(getLibraryCoroutine);
      getLibraryCoroutine = null;
    }

    #endregion

    #region UpdateFloor

    // The coroutine handling network connection.
    Coroutine updateFloorCoroutine;

    /// <summary>
    /// A callback to UpdateFloor method called after server responds.
    /// </summary>
    /// <param name="success">Whether the sending of the request was successful.</param>
    /// <param name="success">Whether the execution of the request was successful.</param>
    public delegate void UpdateFloorCallback(bool success,bool authenticated);

    /// <summary>
    /// Fetches the given library in the database. Does nothing if there is
    /// already a request under way.
    /// </summary>
    /// <param name = "f">Floor to update.</param>
    /// <param name="callback">Callback function for data handling.</param>
    public void UpdateFloor(Floor f, UpdateFloorCallback callback) {
      if (updateFloorCoroutine != null) {
        return;
      }

      updateFloorCoroutine = StartCoroutine(UpdateFloorLoop(f, callback));
    }

    IEnumerator UpdateFloorLoop(Floor f, UpdateFloorCallback callback) {
      WWWForm form = new WWWForm();
      form.AddField("request", "update_floor");
      form.AddField("floor", f.ToJSON().ToString());
      form.AddField("token", token);

      // Create a download object
      WWW request = new WWW(apiUrl, form);

      // Wait until the download is done
      yield return request;

      if (!string.IsNullOrEmpty(request.error)) {
        Debug.Log("Unable to update floor: " + request.error);
        callback(false, false);
      } else {
        Debug.Log(request.text);
        JSONNode root = JSON.Parse(request.text);

        if (root["success"] != null && root["success"].AsBool) {
          callback(true, true);
        } else {
          callback(true, false);
        }
      }

      updateFloorCoroutine = null;
    }

    /// <summary>
    /// Cancels the connection. This guarantees that the callback passed in from
    /// connect will never be called. If no current connection is underway, does
    /// nothing.
    /// </summary>
    public void CancelUpdateFloor() {
      StopCoroutine(updateFloorCoroutine);
      updateFloorCoroutine = null;
    }

    #endregion

    #region CreateLibrary

    // The coroutine handling network connection.
    Coroutine createLibraryCoroutine;

    /// <summary>
    /// A callback to CreateLibrary method called after server responds.
    /// </summary>
    /// <param name="success">Whether the sending of the request was successful.</param>
    /// <param name="success">Whether the execution of the request was successful.</param>
    /// <param name="libId">Thew newly created library ID.</param>
    public delegate void CreateLibraryCallback(bool success,bool authenticated,int libId);

    /// <summary>
    /// Create a new library.
    /// </summary>
    /// <param name = "libName">Name of the library.</param>
    /// <param name="callback">Callback function for data handling.</param>
    public void CreateLibrary(string libName, CreateLibraryCallback callback) {
      if (createLibraryCoroutine != null) {
        return;
      }

      createLibraryCoroutine = StartCoroutine(CreateLibraryLoop(libName, callback));
    }

    IEnumerator CreateLibraryLoop(string libName, CreateLibraryCallback callback) {
      WWWForm form = new WWWForm();
      form.AddField("request", "create_library");
      form.AddField("token", token);
      form.AddField("library_name", libName);

      // Create a download object
      WWW request = new WWW(apiUrl, form);

      // Wait until the download is done
      yield return request;

      if (!string.IsNullOrEmpty(request.error)) {
        Debug.Log("Unable to create library: " + request.error);
        callback(false, false, 0);
      } else {
        Debug.Log(request.text);
        JSONNode root = JSON.Parse(request.text);

        if (root["success"] != null && root["success"].AsBool) {
          callback(true, true, root["id"]);
        } else {
          callback(true, false, 0);
        }
      }

      createLibraryCoroutine = null;
    }

    /// <summary>
    /// Cancels the connection. This guarantees that the callback passed in from
    /// connect will never be called. If no current connection is underway, does
    /// nothing.
    /// </summary>
    public void CancelCreateLibrary() {
      StopCoroutine(createLibraryCoroutine);
      createLibraryCoroutine = null;
    }

    #endregion

    #region CreateFloor

    // The coroutine handling network connection.
    Coroutine createFloorCoroutine;

    /// <summary>
    /// A callback to CreateLibrary method called after server responds.
    /// </summary>
    /// <param name="success">Whether the sending of the request was successful.</param>
    /// <param name="success">Whether the execution of the request was successful.</param>
    /// <param name="floorId">Thew newly created floor ID.</param>
    public delegate void CreateFloorCallback(bool success,bool authenticated,int floorId);

    /// <summary>
    /// Create a new floor.
    /// </summary>
    /// <param name = "libId">Id of the library.</param>
    /// <param name = "floorName">Name of the new floor.</param>
    /// <param name = "floorOrder">Order of the new floor.</param>
    /// <param name="callback">Callback function for data handling.</param>
    public void CreateFloor(int libId, string floorName, int floorOrder, CreateFloorCallback callback) {
      if (createFloorCoroutine != null) {
        return;
      }

      Debug.Log("Creating floor");

      createFloorCoroutine = StartCoroutine(CreateFloorLoop(libId, floorName, floorOrder, callback));
    }

    IEnumerator CreateFloorLoop(int libId, string floorName, int floorOrder, CreateFloorCallback callback) {
      WWWForm form = new WWWForm();
      form.AddField("request", "create_floor");
      form.AddField("token", token);
      form.AddField("library_id", libId);
      form.AddField("floor_name", floorName);
      form.AddField("floor_order", floorOrder);

      // Create a download object
      WWW request = new WWW(apiUrl, form);

      // Wait until the download is done
      yield return request;

      if (!string.IsNullOrEmpty(request.error)) {
        Debug.Log("Unable to create floor: " + request.error);
        callback(false, false, 0);
      } else {
        Debug.Log(request.text);
        JSONNode root = JSON.Parse(request.text);

        if (root["success"] != null && root["success"].AsBool) {
          callback(true, true, root["id"]);
        } else {
          callback(true, false, 0);
        }
      }

      createFloorCoroutine = null;
    }

    /// <summary>
    /// Cancels the connection. This guarantees that the callback passed in from
    /// connect will never be called. If no current connection is underway, does
    /// nothing.
    /// </summary>
    public void CancelCreateFloor() {
      StopCoroutine(createFloorCoroutine);
      createFloorCoroutine = null;
    }

    #endregion

    void Awake() {
      if (shared != null) {
        Destroy(gameObject);
      } else {
        shared = this;
        DontDestroyOnLoad(gameObject);
      }
    }

    /// <summary>
    /// This simply clears the token.
    /// </summary>
    public void Logout() {
      token = "";
    }
  }
}