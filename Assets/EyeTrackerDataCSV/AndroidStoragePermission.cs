#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;
using UnityEngine.Android;
#endif

using UnityEngine;

public class AndroidStoragePermission : MonoBehaviour
{
    void Start()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        CheckStoragePermission();
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private void CheckStoragePermission() 
    {
        const string permission = Permission.ExternalStorageWrite;
        
        if (!Permission.HasUserAuthorizedPermission(permission)) 
        {
            Debug.Log("Request external storage permission");
            PermissionCallbacks callbacks = new PermissionCallbacks();
            callbacks.PermissionGranted += OnPermissionGranted;
            callbacks.PermissionDenied += OnPermissionDenied;
            Permission.RequestUserPermission(permission, callbacks);
        }
        else
        {
            Debug.Log("External storage permission granted");
        }
    }

    private void OnPermissionGranted(string permissionName) 
    {
        Debug.Log($"Permission granted: {permissionName}");
    }

    private void OnPermissionDenied(string permissionName) 
    {
        Debug.LogWarning($"Permission denied: {permissionName}");
    }
#endif
}