using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WhoIsThat.Handlers
{
    public class PermissionHandler
    {
        /// <summary>
        /// Checks if user has already granted permission for camera
        /// </summary>
        /// <returns>bool</returns>
        public static async Task<bool> CheckForCameraPermission()
        {
            return await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera) == PermissionStatus.Granted;
        }

        /// <summary>
        /// Checks if user has already granted permission for storage
        /// </summary>
        /// <returns>bool</returns>
        public static async Task<bool> CheckForStoragePermission()
        {
            return await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage) == PermissionStatus.Granted;
        }
    }
}
