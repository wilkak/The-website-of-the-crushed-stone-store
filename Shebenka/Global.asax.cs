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
        // Cоздаём метод обработки запросов аутентификации
        protected void Application_AuthenticateRequest()
        {
            // Проверяем, что пользователь авторизован
            if (User == null)
                return;

            // Получаем имя пользователя
            string userEmail = Context.User.Identity.Name;

            // Объявляем массив ролей
            string[] roles = null;

            using (Db db = new Db())
            {
                // Заполняем роли
                UserDTO dto = db.Users.FirstOrDefault(x => x.EmailAdress == userEmail);

                if (dto == null)
                    return;

                roles = db.UserRoles.Where(x => x.UserId == dto.Id).Select(x => x.Role.Name).ToArray();
            }
            // Создаём объект интерфейса IPrincipal
            IIdentity userIdentity = new GenericIdentity(userEmail);
            IPrincipal newUserObj = new GenericPrincipal(userIdentity, roles);

            // Обновляем Context.User
            Context.User = newUserObj;
        }
    }
}
