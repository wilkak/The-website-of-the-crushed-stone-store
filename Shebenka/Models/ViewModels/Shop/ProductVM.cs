using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Shebenka.Models.Data;

namespace Shebenka.Models.ViewModels.Shop
{
    public class ProductVM
    {
        public ProductVM()
        {
        }

        public ProductVM(ProductDTO row)
        {
            Id = row.Id;
            Name = row.Name;
            Slug = row.Slug;
            Description = row.Description;
            Price = row.Price;
            ImageName = row.ImageName;
            OldPrice = row.OldPrice;
            View = row.View;
        }

        public int Id { get; set; }
        [DisplayName("Название")]
        [StringLength(150, ErrorMessage = "Название должен содержать как минимум 3 символа, и как максимум 150 символов", MinimumLength = 3)]
        public string Name { get; set; }
        public string Slug { get; set; }
        [DisplayName("Описание")]
        [Required(ErrorMessage = "Поле описание должно быть заполнено")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DisplayName("Цена")]
        [Required(ErrorMessage = "Поле цены должно быть заполнено правильно")]
        public decimal Price { get; set; }
        [DisplayName("Изображение")]
        public string ImageName { get; set; }
        [DisplayName("Отображение")]
        public bool View { get; set; }
        [DisplayName("Старая цена")]
        public decimal OldPrice { get; set; }
        public IEnumerable<string> GalleryImages { get; set; }
    }
}