using Shebenka.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Shebenka
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        // C����� ����� ��������� �������� ��������������
        protected void Application_AuthenticateRequest()
        {
            // ���������, ��� ������������ �����������
            if (User == null)
                return;

            // �������� ��� ������������
            string userEmail = Context.User.Identity.Name;

            // ��������� ������ �����
            string[] roles = null;

            using (Db db = new Db())
            {
                // ��������� ����
                UserDTO dto = db.Users.FirstOrDefault(x => x.EmailAdress == userEmail);

                if (dto == null)
                    return;

                roles = db.UserRoles.Where(x => x.UserId == dto.Id).Select(x => x.Role.Name).ToArray();
            }
            // ������ ������ ���������� IPrincipal
            IIdentity userIdentity = new GenericIdentity(userEmail);
            IPrincipal newUserObj = new GenericPrincipal(userIdentity, roles);

            // ��������� Context.User
            Context.User = newUserObj;
        }
    }
}
