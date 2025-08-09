using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Abstractions;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BlazorAuto_API.Abstract
{
    public static class ClaimsPrincipalUltil
    {
        public static bool HasPermission(this ClaimsPrincipal user, params Enum[] permissions)
        {
            // Kiểm tra đầu vào
            if (user == null || permissions == null || permissions.Length == 0)
                return false;

            var firstPermission = permissions.First();
            if (firstPermission == null)
                throw new ArgumentNullException(nameof(firstPermission), "Permission cannot be null");

            var firstEnumType = firstPermission.GetType();
            var firstDeclaringClass = firstEnumType.DeclaringType;
            if (firstDeclaringClass == null)
                throw new ArgumentNullException(nameof(firstDeclaringClass), "Declaring class cannot be null");

            // Lấy claim chứa quyền của user
            var claims = user.Claims
                .Where(c => c.Type == firstDeclaringClass.Name)
                .ToList();

            if (claims.Count == 0)
                return false;
            if (claims.Count > 1)
                throw new Exception("Lỗi cấu hình dư claim");

            var totalPermission = Convert.ToInt32(claims.First().Value);

            // Gộp tất cả quyền yêu cầu thành một mask
            int requiredPermissionMask = 0;
            foreach (var permission in permissions)
            {
                if (permission == null)
                    throw new ArgumentNullException(nameof(permission), "Permission cannot be null");

                var declaringClass = permission.GetType().DeclaringType;
                if (declaringClass == null)
                    throw new ArgumentNullException(nameof(declaringClass), "Declaring class cannot be null");

                if (firstDeclaringClass.Name != declaringClass.Name)
                    throw new Exception("Lỗi xác nhận interface");

                requiredPermissionMask |= GetValuePermistion(Convert.ToInt32(permission));
            }

            // Kiểm tra xem user có đủ tất cả quyền không
            var result = HasPermisson(totalPermission, requiredPermissionMask);
            return result;

        }
        public static int GetValuePermistion(object value)
        {
            return (int)Math.Pow(2, (int)value);
        }
        public static bool HasPermisson(int totalPermission, int permistionRequired)
        {
            return (totalPermission & permistionRequired) == permistionRequired;
        }
    }
}