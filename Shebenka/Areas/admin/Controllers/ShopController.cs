using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Shebenka.Models.Data;
using Shebenka.Models.ViewModels.Shop;
using System.Drawing.Drawing2D;

namespace Shebenka.Areas.admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShopController : Controller
    {
        // GET: admin/Shop
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public ActionResult AddProduct()
        {
          
            // Объявляем модель
            ProductVM model = new ProductVM();

            // Возвращаем модель в представление
            return View(model);
        }

        //Создаём метод добавления товаров 
        // POST:  admin/Shop/AddProduct/model/file
        [HttpPost]
        [Authorize]
        public ActionResult AddProduct(ProductVM model, HttpPostedFileBase file)
        {
            int id;
          
            // Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Проверяем имя продукта на уникальность
            using (Db db = new Db())
            {
                if (db.Products.Any(x => x.Name == model.Name))
                {
                   
                    ModelState.AddModelError("", "Это название товара уже занято!");
                    return View(model);
                }
            }

            // Объявляем переменную product ID

            // Инициализируем и сохраняем в базу модель productDTO
            using (Db db = new Db())
            {
                
                ProductDTO product = new ProductDTO();

                product.Name = model.Name;
                product.Slug = model.Name.Replace(@"""", "").Replace(",", "").Replace("'", "").Replace("?", "").Replace("!", "").Replace("/", "").Replace(".", "").Replace(" ", "-").ToLower();
                product.Description = model.Description;
                product.Price = model.Price;
                product.OldPrice = model.OldPrice;
                product.View = model.View;
                db.Products.Add(product);
                db.SaveChanges();

                // Получаем введённый ID
                id = product.Id;
            }

            // Добавляем сообщение в TempData
            TempData["SM"] = "Вы добавили товар!";

            #region Upload Image
            // Создаём необходимые директории
            var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));

            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            // Проверяем, есть ли дериктория по пути
            if (!Directory.Exists(pathString1))
                Directory.CreateDirectory(pathString1);

            if (!Directory.Exists(pathString2))
                Directory.CreateDirectory(pathString2);

            if (!Directory.Exists(pathString3))
                Directory.CreateDirectory(pathString3);

            if (!Directory.Exists(pathString4))
                Directory.CreateDirectory(pathString4);

            if (!Directory.Exists(pathString5))
                Directory.CreateDirectory(pathString5);

            // Проверяем, был ли файл загружен
            if (file != null && file.ContentLength > 0)
            {
                // Получаем расширение файла
                string ext = file.ContentType.ToLower();

                // Проверяем расширение
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                       // model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                        ModelState.AddModelError("", "Изображение не было загружено - неправильное расширение изображения");
                        return View(model);
                    }
                }

                // Объявляем переменную имени изображения
                string imageName = file.FileName;

                // Сохраняем имя изображения в DTO
                using (Db db = new Db())
                {
                    ProductDTO dto = db.Products.Find(id);
                    dto.ImageName = imageName;

                    db.SaveChanges();
                }

                // Назначаем пути к оригинальному и уменьшенному изображению
                var path = string.Format($"{pathString2}\\{imageName}");
                var path2 = string.Format($"{pathString3}\\{imageName}");

                // Сохраняем оригинальное изображение
                file.SaveAs(path);

                // Создаём и сохраняем уменьшенную копию
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200).Crop(1, 1);
                img.Save(path2);
            }

            #endregion

            // Переадресовываем пользователя
            return RedirectToAction("AddProduct");
        }
        //Создаём метод отображения товаров 
        // GET:  admin/Shop/Products
        [HttpGet]
        [Authorize]
        public ActionResult Products(int? page)
        {
            // Объявляем ProductVM типа лист
            List<ProductVM> listOfProductVM;

            // Устанавливаем номер страницы
            var pageNumber = page ?? 1;
         
            using (Db db = new Db())
            {
               // UserDTO userdto = db.Users.FirstOrDefault(x => x.EmailAdress == userEmail);

                // Инициализируем лист
                listOfProductVM = db.Products.ToArray()
                        
                           .Select(x => new ProductVM(x))
                           .ToList();
            }

            // Устанавливаем постраничную навигацию
            var onePageOfProducts = listOfProductVM.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = onePageOfProducts;

            // Возвращаем представление и лист
            return View(listOfProductVM);
        }
        //Создаём метод редактирования товаров 
        // GET:  admin/Shop/EditProduct/id
        [HttpGet]
        [Authorize]
        public ActionResult EditProduct(int id)
        {
            // Объявляем модель ProductVM
            ProductVM model;

            using (Db db = new Db())
            {
                // Получаем продукт
                ProductDTO dto = db.Products.Find(id);

                // Проверяем, доступен ли продукт
                if (dto == null)
                {
                    return Content("Этого товара не существует.");
                }

                // Инициализируем модель данными
                model = new ProductVM(dto);


                // Получаем все изображения из галереи
                model.GalleryImages = Directory
                    .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                    .Select(fn => Path.GetFileName(fn));
            }

            // Возвращаем модель в представление
            return View(model);
        }

        // Создаём метод редактирования товаров 
        // POST:  admin/Shop/EditProduct
        [HttpPost]
        [Authorize]
        public ActionResult EditProduct(ProductVM model, HttpPostedFileBase file)
        {
            // Получаем ID продукта
            int id = model.Id;
        
            model.GalleryImages = Directory
                .EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                .Select(fn => Path.GetFileName(fn));

            // Проверяем модель на валидность
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Проверяем имя продукта на уникальность
            using (Db db = new Db())
            {
                if (db.Products.Where(x => x.Id != id).Any(x => x.Name == model.Name))
                {
                    ModelState.AddModelError("", "Это название товара уже занято!");
                    return View(model);
                }
            }
            // Обновляем продукт
            using (Db db = new Db())
            {
                ProductDTO dto = db.Products.Find(id);
                dto.Name = model.Name;
                dto.Slug = model.Name.Replace(@"""", "").Replace(",", "").Replace("'", "").Replace("?", "").Replace("!", "").Replace("/", "").Replace(".", "").Replace(" ", "-").ToLower();
                dto.Description = model.Description;
                dto.Price = model.Price;  
                dto.ImageName = model.ImageName;        
                dto.OldPrice = model.OldPrice;
                dto.View = model.View;
            
                db.SaveChanges();

            }

            // Устанавливаем сообщение в TempData
            TempData["SM"] = "Вы отредактировали товар!";

            // Загрузка изображений в следующим видео!

            #region Image Upload

            // Проверяем загрузку файла
            if (file != null && file.ContentLength > 0)
            {
                // Получаем расширение файла
                string ext = file.ContentType.ToLower();

                // Проверяем расширение
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg" &&
                    ext != "image/gif" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (Db db = new Db())
                    {
                        ModelState.AddModelError("", "Изображение не было загружено - неправильное расширение изображения");
                        return View(model);
                    }
                }

                // Устанавливаем пути загрузки
                var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));

                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                // Удаляем существующие файлы в директориях
                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);

                foreach (var file2 in di1.GetFiles())
                {
                    file2.Delete();
                }

                foreach (var file3 in di2.GetFiles())
                {
                    file3.Delete();
                }

                // Сохраняем имя изображения
                string imageName = file.FileName;

                using (Db db = new Db())
                {
                    ProductDTO dto = db.Products.Find(id);
                    dto.ImageName = imageName;

                    db.SaveChanges();
                }

                // Сохраняем оригинал и превью версии изображений
                var path = string.Format($"{pathString1}\\{imageName}");
                var path2 = string.Format($"{pathString2}\\{imageName}");

                // Сохраняем оригинальное изображение
                file.SaveAs(path);

                // Создаём и сохраняем уменьшенную копию
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200).Crop(1, 1);
                img.Save(path2);

            }

            #endregion

            // Переадресовываем пользователя
            return RedirectToAction("EditProduct");
        }
        // Создаём метод удаления товаров 
        // POST:  admin/Shop/DeleteProduct/id
        public ActionResult DeleteProduct(int id)
        {
            // Удаляем товар из базы данных
            using (Db db = new Db())
            {
                ProductDTO dto = db.Products.Find(id);
                db.Products.Remove(dto);
                db.SaveChanges();
            }
            // Удаляем директорию товара
            var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"")}Images\\Uploads"));
            var pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            try
            {
                if (Directory.Exists(pathString))
                {
                    Directory.Delete(pathString, true);
                }
                // Переадресовываем пользователя
                return RedirectToAction("Products");
            }
            catch
            {
                // Переадресовываем пользователя
                return RedirectToAction("Products");
            }
        }

        // Создаём метод добавления изображений в галерею
        // POST:  admin/Shop/SaveGalleryImages/id
        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            // Перебираем все файлы
            foreach (string fileName in Request.Files)
            {

                // Инициализируем файлы
                HttpPostedFileBase file = Request.Files[fileName];

                // Проверяем на NULL
                if (file != null && file.ContentLength > 0)
                {
                    // Назначаем пути к директориям
                    var originalDirectory = new DirectoryInfo(string.Format($"{Server.MapPath(@"\")}Images\\Uploads"));

                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

                    // Назначаем пути изображений
                    var path = string.Format($"{pathString1}\\{file.FileName}");
                    var path2 = string.Format($"{pathString2}\\{file.FileName}");

                    // Сохраняем оригинальные изображения и уменьшеные копии
                    file.SaveAs(path);

                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200).Crop(1, 1);
                    img.Save(path2);
                }
            }
        }

        // Создаём метод удаления изображений из галереи 
        // POST:  admin/Shop/DeleteImage/id
        public void DeleteImage(int id, string imageName)
        {
            string fullPath1 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/" + imageName);
            string fullPath2 = Request.MapPath("~/Images/Uploads/Products/" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if (System.IO.File.Exists(fullPath1))
                System.IO.File.Delete(fullPath1);

            if (System.IO.File.Exists(fullPath2))
                System.IO.File.Delete(fullPath2);
        }
    }
}


