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
        public static async Task<bool> CheckForCameraPermission()
        {
            if (await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera) == PermissionStatus.Granted)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public static async Task<bool> CheckForStoragePermission()
        {
            if (await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Storage) == PermissionStatus.Granted)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
    }
}
