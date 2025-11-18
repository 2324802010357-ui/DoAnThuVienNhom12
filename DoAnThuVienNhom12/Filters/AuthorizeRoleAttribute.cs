using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnThuVienNhom12.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        private readonly string[] allowedRoles;

        public AuthorizeRoleAttribute(params string[] roles)
        {
            this.allowedRoles = roles;
        }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            var role = httpContext.Session["Role"] as string;
            if (role == null) return false; // chưa đăng nhập

            foreach (var allowedRole in allowedRoles)
            {
                if (string.Equals(role, allowedRole, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["Role"] == null)
            {
                // chưa đăng nhập -> chuyển đến trang đăng nhập
                filterContext.Result = new RedirectResult("~/NguoiDung/DangNhap");
            }
            else
            {
                // đã đăng nhập nhưng sai quyền -> thông báo AccessDenied
                filterContext.Result = new RedirectResult("~/NguoiDung/AccessDenied");
            }
        }
    }
}