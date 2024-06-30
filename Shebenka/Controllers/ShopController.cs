using Shebenka.Models.Data;
using Shebenka.Models.Email;
using Shebenka.Models.ViewModels.Email;
using Shebenka.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Shebenka.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            return View("Index");
        }
        // GET: Shop/product-details/name 
        [HttpGet]
        // Добавляем другое имя контроллера
        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            if (name == null)
            {
                return HttpNotFound();
            }
            // Объявляем модели VM и DTO
            ProductVM model = new ProductVM();
            ProductDTO dto;

            // Инициализируем ID продукта
            int id = 0;

            using (Db db = new Db())
            {
                // Проверяем, доступен ли продукт
                if (!db.Products.Any(x => x.Slug.Equals(name)))
                {
                    return RedirectToAction("Index", "Shop");
                }

                // Инициализируем модель productDTO
                dto = db.Products.Where(x => x.Slug == name).FirstOrDefault();

                // Получаем ID
                id = dto.Id;
                //  model = dto.
                // Инициализируем модель данными
                model.ImageName = dto.ImageName;
                model.OldPrice = dto.OldPrice;
                model.Price = dto.Price;
                model.Description = dto.Description;
                model.Id = dto.Id;
                model.Name = dto.Name;
                model.Slug = dto.Slug;
                //model.View

                /*   model = new ProductVM(dto.Id, dto.Name, dto.Slug, dto.Description, dto.Price,
               dto.ImageName,dto.OldPrice, dto.View, dto.Address, dto.PhoneNumber);
    */
                // Получаем изображения из галереи
                try
                {
                    model.GalleryImages = Directory
                        .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                        .Select(fn => Path.GetFileName(fn));
                }
                catch
                {

                }

                // Возвращаем модель и представление
                return View("ProductDetails", model);
            }
        }

        [HttpGet]
        public ActionResult Products()
        {
            // Объявляем список типа List<ProductVM>
            List<ProductVM> productVMList;

            using (Db db = new Db())
            {
                // инициализируем список данными
                productVMList = db.Products.ToArray().Where(x => x.View == true).Select(x => new ProductVM(x)).ToList();
            }
            // Возвращаем представление с моделью
            return View(productVMList);
        }
        
        // GET: 
        [HttpGet]
        public ActionResult SendEmail()
        {
            // Объявляем модель
            EmailVM model = new EmailVM();
            //model.productname = name;
            // Возвращаем модель в представление
    
            return PartialView("_SendEmail", model);
        }

        // POST: 
        [HttpPost]
        public ActionResult SendEmail(string email, string subject, string htmlMessage, string name, string telefon,string productname)
        {
            DateTime Date = DateTime.Now;
            EmailVM model = new EmailVM(email, subject, htmlMessage, name, telefon, productname, Date);
            if (telefon != null && telefon != "")
            {
                ZvonkiDTO Zvonki = new ZvonkiDTO();
                Zvonki.htmlMessage = htmlMessage;
                Zvonki.name = name;
                Zvonki.telefon = telefon;
                Zvonki.productname = productname;
                Zvonki.subject = subject;
                Zvonki.Date = Date;
                
                //string str = "";
                using (Db db = new Db())
                {

                    
                        db.Zvonki.Add(Zvonki);
                        db.SaveChanges();
                    
                }

                    // EmailVM model = new EmailVM(email,subject,htmlMessage,name,telefon,productname);


                   
                EmailService emailService = new EmailService();          
            
                emailService.SendEmailCustom(model);
            
                // Добавляем сообщение в TempData
            
                TempData["SM"] = "Вы отправили письмо!";
                
                if(model.productname != null && model.productname != "Не указано")
                {

                    if (model.productname == null)
                    {
                        return HttpNotFound();
                    }

                    // Объявляем модели VM и DTO
                    ProductVM models = new ProductVM();
                    ProductDTO dto;
                    // Инициализируем ID продукта

                    int id = 0;

                    using (Db db = new Db())
                    {
                        // Проверяем, доступен ли продукт
                        if (!db.Products.Any(x => x.Name.Equals(model.productname)))
                        {
                            return RedirectToAction("Index", "Shop");
                        }

                        // Инициализируем модель productDTO
                        dto = db.Products.Where(x => x.Name == model.productname).FirstOrDefault();

                        // Получаем ID
                        id = dto.Id;
                      
                        // Инициализируем модель данными
                        models.ImageName = dto.ImageName;
                        models.OldPrice = dto.OldPrice;
                        models.Price = dto.Price;
                        models.Description = dto.Description;
                        models.Id = dto.Id;
                        models.Name = dto.Name;
                        models.Slug = dto.Slug;
                        
                        // Получаем изображения из галереи
                        try
                        {
                            models.GalleryImages = Directory
                                .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                .Select(fn => Path.GetFileName(fn));
                        }
                        catch
                        {

                        }

                        
                        return View("ProductDetails", models);
                    
                    }
                    

                }
                else
                {
                    return RedirectToAction("Products", "Shop");
                }

            }
            else
            {
                if (model.productname != null)
                {

                    if (model.productname == null)
                    {
                        return HttpNotFound();
                    }

                    // Объявляем модели VM и DTO
                    ProductVM models = new ProductVM();
                    ProductDTO dto;
                    // Инициализируем ID продукта

                    int id = 0;

                    using (Db db = new Db())
                    {
                        // Проверяем, доступен ли продукт
                        if (!db.Products.Any(x => x.Name.Equals(model.productname)))
                        {
                            return RedirectToAction("Index", "Shop");
                        }

                        // Инициализируем модель productDTO
                        dto = db.Products.Where(x => x.Name == model.productname).FirstOrDefault();

                        // Получаем ID
                        id = dto.Id;

                        // Инициализируем модель данными

                        models.ImageName = dto.ImageName;
                        models.OldPrice = dto.OldPrice;
                        models.Price = dto.Price;
                        models.Description = dto.Description;
                        models.Id = dto.Id;
                        models.Name = dto.Name;
                        models.Slug = dto.Slug;

                        // Получаем изображения из галереи
                        try
                        {
                            models.GalleryImages = Directory
                                .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                                .Select(fn => Path.GetFileName(fn));
                        }
                        catch
                        {

                        }


                        return View("ProductDetails", models);

                    }


                }
                else
                {
                    return RedirectToAction("Products", "Shop");
                }
            }
        }
    }   
}

