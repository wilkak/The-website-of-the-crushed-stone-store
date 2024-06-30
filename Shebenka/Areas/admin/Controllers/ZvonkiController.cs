using PagedList;
using Shebenka.Models.Data;
using Shebenka.Models.ViewModels.Email;
using Shebenka.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Shebenka.Areas.admin.Controllers
{
    public class ZvonkiController : Controller
    {
        // GET: admin/Zvonki
        public ActionResult Index()
        {
            return View();
        }
        //Создаём метод отображения товаров 
        // GET:  admin/Shop/Products
        [HttpGet]
        [Authorize]
        public ActionResult Telefons(int? page)
        {
            
            // Объявляем ProductVM типа лист
            List<EmailListVM> listOfTelefonVM;

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
          //  string userEmail = User.Identity.Name;
            using (Db db = new Db())
            {
                //  UserDTO userdto = db.Users.FirstOrDefault(x => x.EmailAdress == userEmail);

                // Инициализируем лист
                /*  listOfTelefonVM = db.Zvonki.ToArray()
                             // .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                             .Select(x => new EmailListVM(x))
                             .ToList();*/
                listOfTelefonVM = db.Zvonki.ToArray().Select(x => new EmailListVM(x))
                             .ToList();
            }
      
            // Устанавливаем постраничную навигацию
            var onePageOfTelefonts = listOfTelefonVM.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfTelefons = onePageOfTelefonts;

            // Возвращаем представление и лист
            return View(listOfTelefonVM);
        }
        public ActionResult DeleteZvonok(int id)
        {
            // Удаляем товар из базы данных
            using (Db db = new Db())
            {
                ZvonkiDTO dto = db.Zvonki.Find(id);
                
                List<ZvonkiDTO> com = db.Zvonki.Where(x => x.Id == id).ToList();

                db.Zvonki.Remove(dto);
                db.SaveChanges();
              
            }
            // Переадресовываем пользователя
            return RedirectToAction("Telefons");
        }
    }
}